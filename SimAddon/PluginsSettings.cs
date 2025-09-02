using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimAddon
{
    public class PluginSettings
    {
        public bool Visible { get; set; }
    }

    public class PluginsSettings
    {
        private static string fullPath;
        public PluginsSettings()
        {
            // Get the application name
            string appName = Assembly.GetEntryAssembly().GetName().Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            fullPath = Path.Combine(appDataPath, appName);

            // Ensure the directory exists, if not, create it
            Directory.CreateDirectory(fullPath);

        }

        public Dictionary<String, PluginSettings> Plugins { get; set; } = new Dictionary<string, PluginSettings>();

        public void loadFromJsonFile(string filePath)
        {
            try
            {
                string settingsFile = Path.Combine(fullPath, filePath);

                // Ensure the file exists before attempting to read it
                if (!File.Exists(filePath))
                {
                    Logger.WriteLine($"Plugins settings file not found: {filePath}");
                    return;
                }
                // Read the JSON file content
                string jsonContent = File.ReadAllText(filePath);
                // Deserialize the JSON content into a C# object
                Plugins = System.Text.Json.JsonSerializer.Deserialize<Dictionary<String, PluginSettings>>(jsonContent);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred while loading plugins settings: {ex.Message}");
            }
        }

        public void saveToJsonFile(string filePath)
        {
            try
            {
                string settingsFile = Path.Combine(fullPath, filePath);

                // Serialize the C# object to JSON
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(Plugins, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                // Write the JSON content to the file
                File.WriteAllText(settingsFile, jsonContent);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred while saving plugins settings: {ex.Message}");
            }
        }
    }

}
