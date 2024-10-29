using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SimAddonLogger;
using SimAddonPlugin;
namespace SimAddon
{
    public class Plugin
    {
        public string Nom { get; set; }
        public string NomFichier { get; set; }
    }
    internal class PluginsMgr
    {
        private List<ISimAddonPluginCtrl> _plugins { get; }
        public List<ISimAddonPluginCtrl> plugins { get
            {
                return _plugins;
            }
        }
        public PluginsMgr() { 
            _plugins = new List<ISimAddonPluginCtrl>();
        }

        // Méthode pour charger les plugins depuis un fichier JSON
        public void LoadPluginsFromJson(string filePath)
        {
            try
            {
                // Lire le fichier JSON
                string jsonContent = File.ReadAllText(filePath);

                // Désérialiser le contenu JSON en une liste de plugins
                List<Plugin> pluginNames = JsonConvert.DeserializeObject<List<Plugin>>(jsonContent);

            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Erreur lors du chargement des plugins : {ex.Message}");
            }
        }

        public void LoadPluginsFromFolder(string filePath,TabControl parent)
        {
            if (Directory.Exists(filePath)) {
            string[] subfolders = Directory.GetDirectories(filePath);
                foreach (string subfolder in subfolders)
                {

                    string[] dllFiles = Directory.GetFiles(subfolder, "*.dll", SearchOption.TopDirectoryOnly);
                    foreach (string item in dllFiles)
                    {
                        Assembly assembly = Assembly.LoadFrom(item);
                        if (assembly.GetExportedTypes().Count() > 0)
                        {
                            foreach (Type pluginType in assembly.GetExportedTypes())
                            {
                                //Type pluginType = assembly.GetExportedTypes()[0]; // Remplacer par le namespace et le nom de la classe
                                if (pluginType != null)
                                {
                                    try
                                    {
                                        // Créer une instance de la classe dynamiquement
                                        ISimAddonPluginCtrl pluginInstance = (ISimAddonPluginCtrl)Activator.CreateInstance(pluginType);
                                        _plugins.Add(pluginInstance);

                                    }
                                    catch (Exception ex)
                                    {
                                        //just ignore, that's not a plugin file
                                    }
                                }
                                else
                                {
                                    //Logger.WriteLine("Type (classe) non trouvé dans la DLL" + item);
                                }
                            }
                        }
                        else
                        {
                            //Logger.WriteLine("pas de type exporté dans la dll " + item);

                        }
                    }
                }
            }
        }

    }
}
