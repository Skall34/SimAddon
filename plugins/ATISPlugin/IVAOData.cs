using Newtonsoft.Json;
using SimAddonLogger;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATISPlugin
{
    public class IVAOData
    {
        public class AtcSession
        {
            public string position { get; set; }
            public float frequency { get; set; }
        }
        public class RegionPoint
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Airport
        {
            public string icao { get; set; }
            public string iata { get; set; }
            public string name { get; set; }
            public string countryId { get; set; }
            public string city { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }
            public bool military { get; set; }
        }

        public class AtcPosition
        {
            public long id { get; set; }
            public string airportIcao { get; set; }
            public string atcCallsign { get; set; }
            public string middleIdentifier { get; set; }
            public string position { get; set; }
            public string composePosition { get; set; }
            public bool military { get; set; }
            public float frequency { get; set; }

            public RegionPoint[] regionMap { get; set; }
            public float[][] regionMapPolygon { get; set; }

            public Airport airport { get; set; }
        }

        public class BaseCenter
        {
            public long id { get; set; }
            public string name { get; set; }
            public string countryId { get; set; }
            public bool military { get; set; }
        }

        public class  SubCenter
        {
            public long id { get; set; }
            public string centerId { get; set; }
            public string atcCallsign { get; set; }
            public string middleIdentifier { get; set; }
                        public string position { get; set; }
            public string composePosition { get; set; }
            public bool military { get; set; }
            public float frequency { get; set; }
            public RegionPoint[] regionMap { get; set; }
            public float[][] regionMapPolygon { get; set; }

            public BaseCenter center { get; set; }
        }

        public long id { get; set; }
        public string callsign { get; set; }
        public long userId { get; set; }
        public string connectionType { get; set; }

        public AtcSession atcSession { get; set; }

        public AtcPosition atcPosition { get; set; }

        public SubCenter subCenter { get; set; }
    }

    public class IVAOATC : genericATC
    {
        public IVAOData[] data;

        private static readonly HttpClient httpClient = new HttpClient();

        public override async Task<bool> refresh(string url)
        {
            // Construire l'URL avec le code ICAO
            //string url = $"https://api.ivao.aero/v2/tracker/now/atc/summary";
            bool result = false;
            try
            {
                // Envoyer la requête HTTP GET
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Vérifier que la requête a réussi

                // Lire le contenu de la réponse
                string responseBody = await response.Content.ReadAsStringAsync();

                // Extraire la section METAR du contenu HTML (selon le format attendu)
                string RawIVAOData = responseBody.TrimEnd();
                data = JsonConvert.DeserializeObject<IVAOData[]>(RawIVAOData);
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Erreur lors de la récupération de la liste des ATIS : {ex.Message}");
                return false;
            }
        }

        public override List<string> FindATISInRange(double targetLatitude, double targetLongitude, uint range)
        {
            List<string> atisList = new List<string>();
            if (data == null)
            {
                return atisList;
            }
            foreach (var atc in data)
            {
                if (atc.atcPosition != null && atc.atcPosition.airport != null)
                {
                    double distance = NavigationHelper.GetDistance(targetLatitude, targetLongitude, atc.atcPosition.airport.latitude, atc.atcPosition.airport.longitude);
                    if (distance <= range)
                    {
                        atisList.Add(atc.atcPosition.airport.icao);
                    }
                }
            }
            return atisList;
        }

        public class ATISInfo
        {
            public string[] lines { get; set; }
            public long id { get; set; }
            public long sessionId { get; set; }
            public string revision { get; set; }
            public string timestamp { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public override async Task<List<string>> GetATIS(string ICAO,string url = "")
        {
            List<string> result = new List<string>();
            //make a web request to url, using the httpClient
            try
            {
                // Envoyer la requête HTTP GET
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Vérifier que la requête a réussi

                // Lire le contenu de la réponse
                string responseBody = await response.Content.ReadAsStringAsync();

                // Extraire la section METAR du contenu HTML (selon le format attendu)
                string RawIVAOData = responseBody.TrimEnd();
                ATISInfo data = JsonConvert.DeserializeObject<ATISInfo>(RawIVAOData);
                return data.lines.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Erreur lors de la récupération de la liste des ATIS : {ex.Message}");
            }


            List<string> atisList = new List<string>();
            if (data == null)
            {
                return atisList;
            }
            foreach (var atc in data)
            {
                if (atc.atcPosition != null && atc.atcPosition.airport != null && atc.atcPosition.airport.icao.Equals(ICAO, StringComparison.OrdinalIgnoreCase))
                {
                    atisList.Add(atc.callsign);
                }
            }
            return atisList;
        }

        public override System.Drawing.Image GetNetworkImage()
        {
            //get the folder of the current assembly
            string dllpath =Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Image result = Image.FromFile(Path.Combine(dllpath,"IVAOlogo.png"));
            return result;
        }
    }
}
