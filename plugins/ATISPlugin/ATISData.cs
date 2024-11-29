using Newtonsoft.Json;
using SimAddonLogger;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATISPlugin
{
    public class ATISData
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string real_name { get; set; }
        public string callsign { get; set; }
        public string frequency { get; set; }
        public int facility { get; set; }
        public int rating { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string server { get; set; }
        public int visual_range { get; set; }
        public string atis_code { get; set; }
        public string[] text_atis { get; set; }
        public string last_updated { get; set; }
        public string logon_time { get; set; }
    }

    public class ATIS
    {
        public static List<ATISData> data = new List<ATISData>();

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<bool> refresh()
        {
            // Construire l'URL avec le code ICAO
            string url = $"https://data.vatsim.net/v3/afv-atis-data.json";
            bool result = false; 
            try
            {
                // Envoyer la requête HTTP GET
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Vérifier que la requête a réussi

                // Lire le contenu de la réponse
                string responseBody = await response.Content.ReadAsStringAsync();

                // Extraire la section METAR du contenu HTML (selon le format attendu)
                string RawATISData = responseBody.TrimEnd();
                data = JsonConvert.DeserializeObject<List<ATISData>>(RawATISData);
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Erreur lors de la récupération de la liste des ATIS : {ex.Message}");
                return false;
            }
        }

        public static List<String> FindATISInRange(double targetLatitude, double targetLongitude, uint range)
        {
            List<string> airportsInRange = new List<string>();
            foreach (ATISData airport in data)
            {
                double distance = NavigationHelper.GetDistance(airport.latitude,airport.longitude,targetLatitude,targetLongitude);
                if (distance < range)
                {
                    airportsInRange.Add(airport.name);
                }
            }
            return airportsInRange;
        }
    }

}
