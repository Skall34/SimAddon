using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Dictionary<String, PluginSettings> Plugins { get; set; } = new Dictionary<string, PluginSettings>();

        public void loadFromJsonFile(string filePath)
        {
            try
            {
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
                // Serialize the C# object to JSON
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(Plugins, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                // Write the JSON content to the file
                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred while saving plugins settings: {ex.Message}");
            }
        }
    }

}
