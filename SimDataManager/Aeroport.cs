using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SimAddonLogger;

//using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SimDataManager
{
    public class Fret
    {
        public float fret { get; set; }
    }

    public class Aeroport
    {
        public string ident { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string municipality { get; set; }
        public double latitude_deg { get; set; }
        public double longitude_deg { get; set; }
        public double elevation_ft { get; set; }
        public string Piste { get; set; }
        public string LongueurDePiste { get; set; }
        public string TypeDePiste { get; set; }
        public string Observations { get; set; }
        public string Wikipedia_Link { get; set; }
        public float fret { get; set; }

        private const string DBFILE = "aeroports.json";

        private static string DBFILEPATH;


        public Aeroport()
        {
        }

        private static void initPath()
        {
            // Get the application name
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            string fullPath = Path.Combine(appDataPath, appName);

            // Ensure the directory exists, if not, create it
            Directory.CreateDirectory(fullPath);

            DBFILEPATH = Path.Combine(fullPath, DBFILE);
        }

        public Aeroport(uint id, string _ident, string _type, string _name, double latitude, double longitude)
        {
            type = _type;
            name = _name;
            ident = _ident;
            latitude_deg = latitude;
            longitude_deg = longitude;
        }

        public double DistanceTo(double targetLatitude, double targetLongitude)
        {
            double distance = NavigationHelper.GetDistance(this.latitude_deg,this.longitude_deg,targetLatitude,targetLongitude);

            return distance;
        }
        //private double DegreeToRadian(double angle)
        //{
        //    return Math.PI * angle / 180.0;
        //}

        public static Aeroport FindClosestAirport(List<Aeroport> airports, double targetLatitude, double targetLongitude)
        {
            Aeroport closestAirport = null;
            double shortestDistance = double.MaxValue;

            foreach (var airport in airports)
            {
                double distance = airport.DistanceTo(targetLatitude, targetLongitude);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestAirport = airport;
                }
            }
            return closestAirport;
        }

        public static async Task<List<Aeroport>> fetchAirports(string baseUrl, DateTime lastUpdateFileTime)
        {
            initPath();
            long epoch = 0; // lastUpdateFileTime.ToFileTime();
            if (File.Exists(DBFILEPATH))
            {
                FileInfo fi = new FileInfo(DBFILEPATH);
                DateTime creationTime = fi.CreationTime;
                epoch = (long)(creationTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }

            string url = baseUrl + "?query=airports&date=" + epoch.ToString();
            UrlDeserializer dataReader = new UrlDeserializer(url);
            List<Aeroport> result;
            Logger.WriteLine("Fechting airport informations from server");
            result = await dataReader.FetchAirportsDataAsync(DBFILEPATH);
            //if no new airport database, just load the local one.
            if (result.Count == 0)
            {
                //no airports from the server, try to load the local database.
                if (File.Exists(DBFILEPATH))
                {
                    Logger.WriteLine("Loading local airport database");
                    //read the aeroports.json file.
                    StreamReader sr = new StreamReader(DBFILEPATH);
                    string allData = sr.ReadToEnd();
                    result = deserializeAeroports(allData);
                    if (null == result)
                    {
                        result = new List<Aeroport>();
                    }
                }
                else
                {
                    //no data from server and no local file available... it sucks....
                    result = new List<Aeroport>();
                }
            }
            return result;
        }

        public static async Task<float> fetchFreight(string baseUrl, string airportID)
        {

            string url = baseUrl + "?query=freight&airport=" + airportID;
            UrlDeserializer dataReader = new UrlDeserializer(url);
            float result;
            result = await dataReader.FetchFreightDataAsync();
            return result;
        }

        public static string GetStringValueOrDefault(Dictionary<string, string> item, string key, string _default) {

            return item.TryGetValue(key, out string value) ? value : _default;          
        }
        public static double GetDoubleValueOrDefault(Dictionary<string, string> item, string key, string _default)
        {
            IFormatProvider provider = CultureInfo.InvariantCulture;
            return double.Parse(GetStringValueOrDefault(item,key, _default),provider);
        }

        public static List<Aeroport> deserializeAeroports(string jsonString)
        {
            ////desrialize the whole aiport list at once. But this is bad because if one airport is bad, the whole
            ////airport database is fucked up.
            //List<Aeroport>? aeroports = JsonConvert.DeserializeObject<List<Aeroport>>(jsonString);
            
            //instead, deserialize one by one, to skip any wrongly informed airport
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);
            List<Aeroport> aeroports = new List<Aeroport>();
            IFormatProvider provider = CultureInfo.InvariantCulture;

            //index to count the airports, to help finding a potential error in the airports.
            int i = 1;
            if (data != null)
            {
                foreach (Dictionary<string, string> item in data)
                {
                    try
                    {
                        Aeroport a = new Aeroport();
                        a.ident = GetStringValueOrDefault(item,"ident", "unknown" + i);
                        a.type = GetStringValueOrDefault(item,"type", "unknown" + i);
                        a.name = GetStringValueOrDefault(item,"name", "unknown" + i);
                        a.municipality = GetStringValueOrDefault(item,"municipality", "unknown" + i);
                        a.latitude_deg = GetDoubleValueOrDefault(item,"latitude_deg", "0");
                        a.longitude_deg = GetDoubleValueOrDefault(item,"longitude_deg", "0");
                        a.elevation_ft = GetDoubleValueOrDefault(item,"ekevation_ft", "0");
                        a.Piste = GetStringValueOrDefault(item, "Piste", "unknown" + i);
                        a.LongueurDePiste = GetStringValueOrDefault(item,"LongueueDePiste", "? " + i);
                        a.TypeDePiste = GetStringValueOrDefault(item, "Type de piste", "unknown" + i);
                        a.Observations = GetStringValueOrDefault(item, "Observations", "unknown" + i);
                        a.Wikipedia_Link = GetStringValueOrDefault(item, "wikipedia_link", "unknown" + i);

                        //a.ident = item["ident"];
                        //a.type = item["type"];
                        //a.name = item["name"];
                        //a.municipality = item["municipality"];
                        //a.latitude_deg = double.Parse(item["latitude_deg"], provider);
                        //a.longitude_deg = double.Parse(item["longitude_deg"], provider);
                        //a.elevation_ft = double.Parse(item["elevation_ft"], provider);
                        //a.Piste = item["Piste"];
                        //a.LongueurDePiste = item["Longueur de piste"];
                        //a.TypeDePiste = item["Type de piste"];
                        //a.Observations = item["Observations"];
                        //a.Wikipedia_Link = item["wikipedia_link"];

                        aeroports.Add(a);
                    }
                    catch (Exception ex)
                    {
                        //badly formed airport. trace it for fix, but skip it.
                        Logger.WriteLine("Error in airport DB, for entry " + i + " : " + ex.Message);
                    }
                    i++;
                }
            }
            return aeroports;
        }

        
        public static float deserializeFreight(string jsonString)
        {
            //var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);
            Fret result = JsonConvert.DeserializeObject<Fret>(jsonString);
            if (null != result)
            {
                return result.fret;
            }
            else
            {
                return -1;
            }
        }
    }

}
