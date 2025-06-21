using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;


//using System.Text.Json;
using System.Threading.Tasks;
using SimAddonLogger;


namespace SimDataManager
{
    public class UrlDeserializer
    {
        private readonly string _url;

        public UrlDeserializer(string url)
        {
            _url = url;
        }

        public async Task<(List<Avion>, List<Mission>)> FetchDataAsync()
        {
            List<Avion> avions = new List<Avion>();
            List<Mission> missions = new List<Mission>();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        // Désérialisation du JSON
                        var data = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(jsonString);

                        if ((data!=null) && (data.TryGetValue("flotte", out var flotte)))
                        {
                            foreach (Dictionary<string,string>item in flotte)
                            {
                                Avion avion = new Avion
                                {
                                    Index = int.TryParse(item.TryGetValue("index", out string index) ? index : "", out int indexValue) ? indexValue : 0,
                                    ICAO = item.TryGetValue("ICAO", out string icao) ? icao : "unknown",
                                    Designation = item.TryGetValue("Clair", out string design) ? design : "unknown",
                                    Type = item.TryGetValue("Type", out string type) ? type : "unknown",
                                    Immat = item.TryGetValue("Immat", out string immat) ? immat : "-----",
                                    Localisation = item.TryGetValue("Localisation", out string localisation) ? localisation : "",
                                    Hub = item.TryGetValue("Hub", out string hub) ? hub : "",
                                    CoutHoraire = int.TryParse(item.TryGetValue("Cout Horaire", out string cout) ? cout : "", out int coutValue) ? coutValue : 0,
                                    Etat = int.TryParse(item.TryGetValue("Etat", out string etat) ? etat : "", out int etatValue) ? etatValue : 0,
                                    Status = int.TryParse(item.TryGetValue("Status", out string status) ? status : "", out int statusValue) ? statusValue : 0,
                                    Horametre = item.TryGetValue("Horametre", out string horametre) ? horametre : "",
                                    DernierUtilisateur = item.TryGetValue("Dernier utilisateur", out string utilisateur) ? utilisateur : "",
                                    EnVol = int.TryParse(item.TryGetValue("En vol", out string envol) ? envol : "", out int envolValue) ? envolValue : 0
                                };

                                avions.Add(avion);
                            }
                        }

                        if ((null != data) &&(data.TryGetValue("missions", out var missionTemp)))
                        {
                            foreach (var item in missionTemp)
                            {
                                Mission mission = new Mission
                                {
                                    Libelle = item.TryGetValue("Libelle", out string libMission) ? libMission : "",
                                    Index = int.TryParse(item.TryGetValue("Index", out string indexMission) ? indexMission : "", out int index) ? index : 0,
                                };

                                missions.Add(mission);
                            }

                        }
                    }
                    else
                    {
                        // Gérer les erreurs si la requête n'a pas réussi
                        Logger.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    // Gérer les exceptions
                    Logger.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                }
            }

            return (avions, missions);
        }


        public async Task<List<Aeroport>> FetchAirportsFromDatabaseAsync(string connectionString)
        {
            var aeroports = new List<Aeroport>();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT ident, type_aeroport, name, municipality, latitude_deg, longitude_deg, elevation_ft, Piste, Longueur_de_piste, Type_de_piste, Observations, Wikipedia_Link, fret FROM AEROPORTS";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var aeroport = new Aeroport
                        {
                            ident = reader.GetString(0),
                            type = reader.GetString(1),
                            name = reader.GetString(2),
                            municipality = reader.GetString(3),
                            latitude_deg = reader.GetDouble(4),
                            longitude_deg = reader.GetDouble(5),
                            elevation_ft = reader.GetDouble(6),
                            Piste = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            Longueur_de_piste = reader.IsDBNull(8) ? "" : reader.GetString(8),
                            Type_de_piste = reader.IsDBNull(9) ? "" : reader.GetString(9),
                            Observations = reader.IsDBNull(10) ? "" : reader.GetString(10),
                            Wikipedia_Link = reader.IsDBNull(11) ? "" : reader.GetString(11),
                            fret = reader.IsDBNull(12) ? 0 : reader.GetFloat(12)
                        };
                        aeroports.Add(aeroport);
                    }
                }
            }
            return aeroports;
        }

        public async Task<DateTime?> GetAeroportsLastUpdateAsync(string connectionString)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                // Cette requête fonctionne si tu as un champ "updated_at" ou "last_modified" dans ta table AEROPORTS.
                // Sinon, tu peux utiliser la date de modification de la table (voir plus bas).
                string query = "SELECT UPDATE_TIME FROM information_schema.tables WHERE TABLE_SCHEMA = 'SKYWINGS_VA' AND TABLE_NAME = 'AEROPORTS'";

                using (var command = new MySqlCommand(query, connection))
                {
                    var result = await command.ExecuteScalarAsync();
                    if (result != DBNull.Value && result != null)
                        return Convert.ToDateTime(result);
                    else
                        return null;
                }
            }
        }

        public void SaveLastAeroportsUpdateDate(DateTime date, string filePath)
        {
            File.WriteAllText(filePath, date.ToString("o")); // format ISO 8601
        }

        public DateTime? LoadLastAeroportsUpdateDate(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var content = File.ReadAllText(filePath);
            if (DateTime.TryParse(content, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
                return dt;
            return null;
        }


        public async Task<List<Aeroport>> FetchAirportsDataAsync(string connectionString)
        {
            List<Aeroport> aeroports ;
            /*
                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                HttpResponseMessage response = await client.GetAsync(_url);

                                if (response.IsSuccessStatusCode)
                                {
                                    string jsonString = await response.Content.ReadAsStringAsync();
                                    // Désérialisation du JSON
                                    aeroports = Aeroport.deserializeAeroports(jsonString);
                                    //if we received airports, store them locally
                                    if (aeroports.Count > 0)
                                    {                           
                                        StreamWriter sw= new StreamWriter(filename);
                                        JsonSerializer serializer = new JsonSerializer();
                                        serializer.Formatting = Formatting.Indented;
                                        serializer.Serialize(sw, aeroports);
                                        sw.Close();
                                    }
                                }
                                else
                                {
                                    aeroports = new List<Aeroport>();
                                    // Gérer les erreurs si la requête n'a pas réussi
                                    Logger.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
                                }
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.Message,"Error while loading airports",MessageBoxButtons.OK,MessageBoxIcon.Error);

                                aeroports = new List<Aeroport>();
                                // Gérer les exceptions
                                Logger.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                            }
                        }
                        */
            return await FetchAirportsFromDatabaseAsync(connectionString); 
            //return aeroports;
        }
        

        
        public async Task<float> FetchFreightDataAsync()
        {
            float result;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        // Désérialisation du JSON
                        result = Aeroport.deserializeFreight(jsonString);
                        //if we received airports, store them locally
                    }
                    else
                    {
                        result = -1;
                        // Gérer les erreurs si la requête n'a pas réussi
                        Logger.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    result = -1;
                    // Gérer les exceptions
                    Logger.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                }
            }
            return result;
        }

        [Serializable]
        public class GithubToken
        {
            public string token;
        }

        public async Task<string> FetchGithubTokenAsync()
        {
            string result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        // Désérialisation du JSON
                        GithubToken githubtoken = JsonConvert.DeserializeObject<GithubToken>(jsonString);
                        //if we received airports, store them locally
                        if (githubtoken == null)
                        {
                            result = string.Empty;
                        }
                        else
                        {
                            result =  githubtoken.token;
                        }
                        
                    }
                    else
                    {
                        result = string.Empty;
                        // Gérer les erreurs si la requête n'a pas réussi
                        Logger.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                    // Gérer les exceptions
                    Logger.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                }
            }
            return result;
        }


        [Serializable]
        public class SaveFlightQuery
        {
            public string qtype { get; set; }
            public string query { get; set; }
            public string    cs { get; set; }
            public string plane { get; set; }
            public string sicao { get; set; }
            public string sfuel { get; set; }
            public string stime { get; set; }
            public string eicao { get; set; }
            public string efuel { get; set; }
            public string etime { get; set; }
            public string cargo { get; set; }
            public string note { get; set; }
            public string mission { get; set; }
            public string comment { get; set; }
        }

        [Serializable]
        public class PlaneUpdateQuery
        {
            public string qtype { get; set; }
            public string query { get; set; }
            public string cs { get; set; }
            public string plane { get; set; }
            public string sicao { get; set; }
            public int? flying{ get; set; }
            public string endIcao { get; set; }
        }

        public async Task<int> PushFlightAsync(SaveFlightQuery data)
        {
            int result;
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response = await client.PostAsJsonAsync<SaveFlightQuery>(_url, data);

                    if (response.IsSuccessStatusCode)
                    {
                        string res = await response.Content.ReadAsStringAsync();
                        result = 1;
                        //if we received airports, store them locally
                    }
                    else
                    {
                        result = 0;
                        // Gérer les erreurs si la requête n'a pas réussi
                        Logger.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    result = 0;
                    // Gérer les exceptions
                    Logger.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                }
            }
            return result;
        }

        public async  Task<int> PushJSonAsync<Serializable>(Serializable data)
        {
            int result;
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response = await client.PostAsJsonAsync<Serializable>(_url, data);

                    if (response.IsSuccessStatusCode)
                    {
                        string res = await response.Content.ReadAsStringAsync();
                        result = 1;
                        //if we received airports, store them locally
                    }
                    else
                    {
                        result = 0;
                        // Gérer les erreurs si la requête n'a pas réussi
                        Logger.WriteLine("Erreur lors de la récupération des données : " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    result = 0;
                    // Gérer les exceptions
                    Logger.WriteLine("Erreur lors de la récupération des données : " + ex.Message);
                }
            }
            return result;
        }

        public async Task SaveLocalJsonAsync<Serializable>(Serializable data, string fileName) where Serializable : class
        {
            try
            {
                // Get the application name
                string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

                // Get the path to the user's AppData folder
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                // Combine the AppData path with the folder name
                string fullPath = Path.Combine(appDataPath, appName);

                // Ensure the directory exists, if not, create it
                Directory.CreateDirectory(fullPath);

                string jsonFile = Path.Combine(fullPath, fileName);

                // Sérialiser l'objet en format JSON avec Newtonsoft.Json
                var jsonString = JsonConvert.SerializeObject(data, Formatting.Indented); // Formatting.Indented pour indenter le JSON

                // Écrire le contenu JSON dans le fichier spécifié de manière asynchrone
                using (StreamWriter writer = new StreamWriter(jsonFile))
                {
                    await writer.WriteAsync(jsonString);
                }
                Logger.WriteLine($"Le fichier {jsonFile} a été enregistré avec succès.");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Erreur lors de l'enregistrement du fichier : {ex.Message}");
            }
        }

    }
}
