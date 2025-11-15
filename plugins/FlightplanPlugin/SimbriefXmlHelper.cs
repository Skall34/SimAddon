using System;
using System.Text.RegularExpressions;

namespace simbrief
{
    internal static class SimbriefXmlHelper
    {
        // remplace <br> et variantes par rien (on veut ignorer ces balises)
        // si vous préférez garder une balise valide XML, remplacez par "<br/>" au lieu de ""
        private static readonly Regex BrTagRegex = new Regex(@"<\s*br\s*(?:/?)\s*>|<\s*/\s*br\s*>",
                                                             RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // remplace les <img ...> non auto-fermés par <img .../>
        private static readonly Regex ImgTagFixRegex = new Regex(@"<img([^>]*)(?<!/)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // remplace les <input ...> non auto-fermés par <input .../>
        private static readonly Regex InputTagFixRegex = new Regex(@"<input([^>]*)(?<!/)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // éléments à entourer par CDATA (on ne change pas leur contenu autrement)
        private static readonly string[] ElementsToWrapCData = new[]
        {
            "pilotedge_prefile",
            "vatsim_prefile",
            "ivao_prefile",
            "poscon_prefile"
        };

        // regex ciblée pour <link>...</link>
        private static readonly Regex LinkRegex = new Regex(@"<\s*link\s*>(.*?)<\s*/\s*link\s*>",
                                                            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        // pattern pour détecter & non échappés (nous n'échappons pas les entités valides déjà présentes)
        private const string AmpersandPattern = @"&(?!([A-Za-z]+|#\d+|#x[0-9A-Fa-f]+);)";

        public static string Sanitize(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return xml;

            // Supprimer les balises BR non conformes (option : remplacer par "<br/>" si besoin)
            xml = BrTagRegex.Replace(xml, string.Empty);

            // ferme les balises <img> si non fermées
            xml = ImgTagFixRegex.Replace(xml, "<img$1/>");

            // ferme les balises <input> si non fermées
            xml = InputTagFixRegex.Replace(xml, "<input$1/>");

            // --- Traitement spécial pour <link> : échapper le contenu (ampersands, et caractères '<' '>');
            xml = LinkRegex.Replace(xml, new MatchEvaluator(m =>
            {
                string inner = m.Groups[1].Value;

                // Ne pas rééchapper si déjà en CDATA
                if (inner.StartsWith("<![CDATA[", StringComparison.Ordinal) && inner.EndsWith("]]>", StringComparison.Ordinal))
                {
                    return $"<link>{inner}</link>";
                }

                // Échapper les & nus : & -> &amp; sauf si déjà entité
                string escaped = Regex.Replace(inner, AmpersandPattern, "&amp;");

                // Échapper aussi les '<' et '>' à l'intérieur du contenu pour rester XML valide
                escaped = escaped.Replace("<", "&lt;").Replace(">", "&gt;");

                return $"<link>{escaped}</link>";
            }));

            // --- Conserver le comportement CDATA pour les autres éléments listés (inchangé)
            foreach (var tag in ElementsToWrapCData)
            {
                var regex = new Regex($@"<\s*{Regex.Escape(tag)}\s*>(.*?)<\s*/\s*{Regex.Escape(tag)}\s*>",
                                      RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

                xml = regex.Replace(xml, new MatchEvaluator(m =>
                {
                    string inner = m.Groups[1].Value;

                    // Si le contenu est déjà en CDATA, on ne touche pas
                    if (inner.StartsWith("<![CDATA[", StringComparison.Ordinal) && inner.EndsWith("]]>", StringComparison.Ordinal))
                    {
                        return $"<{tag}>{inner}</{tag}>";
                    }

                    // Protéger toute occurrence de "]]>" en la scindant pour rester valide dans CDATA
                    // Remplace "]]>" par "]]]]><![CDATA[>" — technique standard pour inclure "]]>" dans CDATA
                    string safeInner = inner.Replace("]]>", "]]]]><![CDATA[>");

                    return $"<{tag}><![CDATA[{safeInner}]]></{tag}>";
                }));
            }

            return xml;
        }
    }
}