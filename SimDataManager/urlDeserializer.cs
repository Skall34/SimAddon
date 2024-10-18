using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
//using System.Net.Http.Json;
using System.Runtime.Serialization;

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

        public async Task<List<Aeroport>> FetchAirportsDataAsync(string filename)
        {
            List<Aeroport> aeroports ;

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

            return aeroports;
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
