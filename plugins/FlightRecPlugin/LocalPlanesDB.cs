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
                var raw = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                // Normalise les cles (Trim) : XPUIPC 0x3D00 et SimBridge peuvent différer
                // par un espace final ("Cessna 172 SP GTN REP " vs "Cessna 172 SP GTN REP").
                planesDB = new Dictionary<string, List<string>>();
                if (raw != null)
                {
                    foreach (var kv in raw)
                    {
                        string k = (kv.Key ?? "").Trim();
                        if (k.Length > 0 && !planesDB.ContainsKey(k)) planesDB[k] = kv.Value;
                    }
                }
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
            string key = (simplane ?? "").Trim();
            if (planesDB.ContainsKey(key))
            {
                return planesDB[key];
            }
            return null;
        }
        public static void SetPlane(string simplane, string name)
        {
            if (planesDB == null)
            {
                LoadDB();
            }
            string key = (simplane ?? "").Trim();
            if (key.Length == 0) return;
            if (!planesDB.ContainsKey(key))
            {
                planesDB[key] = new List<string>();
            }
            if (!planesDB[key].Contains(name))
            {
                planesDB[key].Add(name);
                var json = System.Text.Json.JsonSerializer.Serialize(planesDB, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                var filename = Path.Combine(storageFolder, LocalPlanesDB.dbFile);
                File.WriteAllText(filename, json);
            }
        }

        public static void RemovePlane(string simplane,string name)
        {
            if (planesDB == null)
            {
                LoadDB();
            }
            string key = (simplane ?? "").Trim();
            if (planesDB.ContainsKey(key))
            {
                planesDB[key].Remove(name);
                if (planesDB[key].Count == 0)
                {
                    planesDB.Remove(key);
                }
                var json = System.Text.Json.JsonSerializer.Serialize(planesDB, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                var filename = Path.Combine(storageFolder, LocalPlanesDB.dbFile);
                File.WriteAllText(filename, json);
            }
        }
    }
}
