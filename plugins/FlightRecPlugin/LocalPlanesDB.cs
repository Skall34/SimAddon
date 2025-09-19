using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlightRecPlugin
{
    public static class LocalPlanesDB
    {
        const string dbFile= "planesdb.json";
        private static string storageFolder = AppDomain.CurrentDomain.BaseDirectory;

        private static Dictionary<string, List<string>> planesDB;

        private static void LoadDB()
        {
            var filename = Path.Combine(storageFolder, LocalPlanesDB.dbFile);
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                planesDB = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>> >(json);
            }
            else
            {
                planesDB = new Dictionary<string, List<string>> ();
            }
        }

        static LocalPlanesDB()
        {
            // Get the application name
            string appName = Assembly.GetEntryAssembly().GetName().Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            storageFolder = Path.Combine(appDataPath, appName);

            // Ensure the directory exists, if not, create it
            Directory.CreateDirectory(storageFolder);

            LoadDB();
        }

        public static List<string> GetPlaneName(string simplane)
        {
            if (planesDB == null)
            {
                LoadDB();
            }
            if (planesDB.ContainsKey(simplane))
            {
                return planesDB[simplane];
            }
            return null;
        }
        public static void SetPlane(string simplane, string name)
        {
            if (planesDB == null)
            {
                LoadDB();
            }
            if (!planesDB.ContainsKey(simplane))
            {
                planesDB[simplane] = new List<string>();
            }
            planesDB[simplane].Add(name);
            var json = System.Text.Json.JsonSerializer.Serialize(planesDB, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            var filename = Path.Combine(storageFolder, LocalPlanesDB.dbFile);
            File.WriteAllText(filename, json);
        }

        public static void RemovePlane(string simplane,string name)
        {
            if (planesDB == null)
            {
                LoadDB();
            }
            if (planesDB.ContainsKey(simplane))
            {
                planesDB[simplane].Remove(name);
                if (planesDB[simplane].Count == 0)
                {
                    planesDB.Remove(simplane);
                }
                var json = System.Text.Json.JsonSerializer.Serialize(planesDB, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                var filename = Path.Combine(storageFolder, LocalPlanesDB.dbFile);
                File.WriteAllText(filename, json);
            }
        }
    }
}
