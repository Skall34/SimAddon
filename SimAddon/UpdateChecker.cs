using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimAddonLogger;

namespace SimAddon
{
    /// <summary>
    /// Classe responsable de la vérification des mises à jour depuis GitHub
    /// </summary>
    public class UpdateChecker
    {
        private const string GITHUB_API_URL = "https://api.github.com/repos/Skall34/SimAddon/releases/latest";
        private const string GITHUB_RELEASES_URL = "https://github.com/Skall34/SimAddon/releases";
        
        private static readonly HttpClient httpClient = new HttpClient();

        public class ReleaseInfo
        {
            public string TagName { get; set; }
            public string Name { get; set; }
            public string Body { get; set; }
            public string HtmlUrl { get; set; }
            public bool Prerelease { get; set; }
        }

        public UpdateChecker()
        {
            // Configure HttpClient with GitHub API requirements
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SimAddon-UpdateChecker");
        }

        /// <summary>
        /// Obtient la version actuelle de l'application
        /// </summary>
        /// <returns>Version object</returns>
        public static Version GetCurrentVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                Version version = assembly.GetName().Version;
                return version ?? new Version("0.0.0.0");
            }
            return new Version("0.0.0.0");
        }

        /// <summary>
        /// Convertit une version tag GitHub (ex: "v1.2.3") en objet Version
        /// </summary>
        /// <param name="tagName">Tag name from GitHub</param>
        /// <returns>Version object</returns>
        private static Version ParseVersionFromTag(string tagName)
        {
            try
            {
                // Supprimer le préfixe 'v' s'il existe
                string versionStr = tagName.StartsWith("v", StringComparison.OrdinalIgnoreCase) 
                    ? tagName.Substring(1) 
                    : tagName;

                if (Version.TryParse(versionStr, out Version version))
                {
                    return version;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error parsing version from tag '{tagName}': {ex.Message}");
            }

            return new Version("0.0.0.0");
        }

        /// <summary>
        /// Vérifie s'il y a une nouvelle version disponible sur GitHub
        /// </summary>
        /// <param name="includePrerelease">Si true, inclut les versions préliminaires</param>
        /// <returns>ReleaseInfo si une nouvelle version est disponible, null sinon</returns>
        public async Task<ReleaseInfo> CheckForUpdatesAsync(bool includePrerelease = false)
        {
            try
            {
                Logger.WriteLine("Checking for updates from GitHub...");

                using (var response = await httpClient.GetAsync(GITHUB_API_URL))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.WriteLine($"Failed to check for updates: HTTP {response.StatusCode}");
                        return null;
                    }

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                    {
                        JsonElement root = doc.RootElement;

                        // Vérifier si c'est une préversion
                        bool isPrerelease = root.TryGetProperty("prerelease", out var prereleaseElement) 
                            && prereleaseElement.GetBoolean();

                        if (isPrerelease && !includePrerelease)
                        {
                            Logger.WriteLine("Latest release is a prerelease, skipping check");
                            return null;
                        }

                        // Extraire les informations de la version
                        string tagName = root.GetProperty("tag_name").GetString();
                        string name = root.GetProperty("name").GetString();
                        string body = root.TryGetProperty("body", out var bodyElement) 
                            ? bodyElement.GetString() 
                            : "";
                        string htmlUrl = root.GetProperty("html_url").GetString();

                        // Parser la version
                        Version latestVersion = ParseVersionFromTag(tagName);
                        Version currentVersion = GetCurrentVersion();

                        Logger.WriteLine($"Current version: {currentVersion}");
                        Logger.WriteLine($"Latest version: {latestVersion}");

                        // Comparer les versions
                        if (latestVersion > currentVersion)
                        {
                            Logger.WriteLine($"New version available: {latestVersion}");
                            return new ReleaseInfo
                            {
                                TagName = tagName,
                                Name = name,
                                Body = body,
                                HtmlUrl = htmlUrl,
                                Prerelease = isPrerelease
                            };
                        }
                        else
                        {
                            Logger.WriteLine("Application is up to date");
                            return null;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.WriteLine($"Network error while checking for updates: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Logger.WriteLine($"Error parsing GitHub API response: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Unexpected error checking for updates: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Affiche une boîte de dialogue proposant à l'utilisateur de télécharger la nouvelle version
        /// </summary>
        /// <param name="releaseInfo">Informations sur la nouvelle version</param>
        /// <param name="owner">Formulaire propriétaire pour centrer la dialogue</param>
        /// <returns>True si l'utilisateur a choisi de télécharger, false sinon</returns>
        public static bool ShowUpdateDialog(ReleaseInfo releaseInfo, Form owner = null)
        {
            if (releaseInfo == null)
                return false;

            string message = $"Une nouvelle version de SimAddon est disponible !\n\n" +
                           $"Version actuelle : {GetCurrentVersion()}\n" +
                           $"Nouvelle version : {releaseInfo.Name}\n\n" +
                           $"Notes de version :\n{releaseInfo.Body}\n\n" +
                           $"Voulez-vous télécharger la nouvelle version maintenant ?";

            DialogResult result = MessageBox.Show(
                message,
                "Mise à jour disponible",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Ouvre la page de téléchargement de la nouvelle version dans le navigateur par défaut
        /// </summary>
        /// <param name="releaseInfo">Informations sur la nouvelle version</param>
        public static void OpenDownloadPage(ReleaseInfo releaseInfo)
        {
            if (releaseInfo == null)
                return;

            try
            {
                Logger.WriteLine($"Opening download page: {releaseInfo.HtmlUrl}");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = releaseInfo.HtmlUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error opening download page: {ex.Message}");
                MessageBox.Show(
                    $"Impossible d'ouvrir la page de téléchargement.\n\n" +
                    $"Veuillez visiter : {releaseInfo.HtmlUrl}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
