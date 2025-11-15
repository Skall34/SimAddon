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

        // éléments à entourer par CDATA
        private static readonly string[] ElementsToWrapCData = new[]
        {
            "link",
            "pilotedge_prefile",
            "vatsim_prefile",
            "ivao_prefile",
            "poscon_prefile"
        };

        public static string Sanitize(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return xml;

            // Supprimer les balises BR non conformes (option : remplacer par "<br/>" si besoin)
            xml = BrTagRegex.Replace(xml, string.Empty);

            // ferme les balises <img> si non fermées
            xml = ImgTagFixRegex.Replace(xml, "<img$1/>");

            // ferme les balises <input> si non fermées
            xml = InputTagFixRegex.Replace(xml, "<input$1/>");

            // Entourer le contenu des éléments problématiques par CDATA (en évitant de casser les séquences "]]>")
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