using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SimAddonLogger;

//using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Net;
using System.Linq;
using System.Runtime.Serialization;

namespace SimDataManager
{
    public class lastUpdate
    {
        public bool success { get; set; }
        public string last_update { get; set; }
    }

    public class AeroportPhpList
    {
        public bool success { get; set; }
        public List<Dictionary<string, string>> aeroports { get; set; }
    }

    public class FretPhpInfo
    {
        public bool success { get; set; }
        public string ICAO { get; set; }
        public float fret { get; set; }
    }

    public class Fret
    {
        public float fret { get; set; }
    }

    public class lien()
    {
        private Aeroport _Departure;
        public  Aeroport Departure{ get {
                return _Departure;
            }
            set
            {
                _Departure = value;
                if (_Arrival!=null)
                {
                    _Length = _Departure.DistanceTo(_Arrival);
                }
            }
        }
        private Aeroport _Arrival;
        public Aeroport Arrival { get
            {
                return _Arrival;
            }

            set
            {
                _Arrival = value;
                if (_Departure != null)
                {
                    _Length = _Departure.DistanceTo(_Arrival);
                }
            }
        }

        private double _Length = double.MaxValue;
        public double Length { get
            {
                return _Length;
            }
        }
    }

    public class Aeroport
    {
        public string ident { get; set; }
        public string type_aeroport { get; set; }
        public string name { get; set; }
        public string municipality { get; set; }
        public double latitude_deg { get; set; }
        public double longitude_deg { get; set; }
        public double elevation_ft { get; set; }
        public string Piste { get; set; }
        public string Longueur_de_piste { get; set; }
        public string Type_de_piste { get; set; }
        public string Observations { get; set; }
        public string Wikipedia_Link { get; set; }
        public float fret { get; set; }

        public string fullName { get
            {
                return(ident+" ("+name+")");
            } 
        }

        //Needed to sort an airport list by distance
        private double _distance;
        public double distance
        {
            get
            {
                return _distance;
            }
        }

        [IgnoreDataMember]
        public List<Aeroport> AirportsInRange;

        private const string DBFILE = "aeroports.json";
        private static string DBFILEPATH;


        public Aeroport()
        {
            AirportsInRange = new List<Aeroport>();
        }

        public override string ToString()
        {
            return fullName;
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
            type_aeroport = _type;
            name = _name;
            ident = _ident;
            latitude_deg = latitude;
            longitude_deg = longitude;
        }

        public double DistanceTo(double targetLatitude, double targetLongitude)
        {
            double _dist = NavigationHelper.GetDistance(this.latitude_deg,this.longitude_deg,targetLatitude,targetLongitude);
            return _dist;
        }

        public double DistanceTo(Aeroport target)
        {
            double _dist = NavigationHelper.GetDistance(this.latitude_deg, this.longitude_deg, target.latitude_deg, target.longitude_deg);
            return _dist;
        }

        public static Aeroport FindClosestAirport(List<Aeroport> airports, double targetLatitude, double targetLongitude)
        {
            Aeroport closestAirport = null;
            double shortestDistance = double.MaxValue;

            foreach (var airport in airports)
            {
                double _dist = airport.DistanceTo(targetLatitude, targetLongitude);
                if (_dist < shortestDistance)
                {
                    shortestDistance = _dist;
                    closestAirport = airport;
                    closestAirport._distance = _dist;                  
                }
            }
            return closestAirport;
        }

        public Aeroport FindClosestAirport(List<Aeroport> airports)
        {
            return Aeroport.FindClosestAirport(airports,this.latitude_deg,this.longitude_deg) ;
        }

        public static List<Aeroport> FindAirportsInRange(List<Aeroport> airports, double targetLatitude, double targetLongitude,uint range)
        {
            List<Aeroport> _InRange = new List<Aeroport>();
            foreach (var airport in airports)
            {
                double _dist = airport.DistanceTo(targetLatitude, targetLongitude);
                if (_dist < range)
                {
                    airport._distance = _dist;
                    _InRange.Add(airport);
                }
            }
            return _InRange;
        }

        public int FindAirportsInRange(List<Aeroport> airports, double rangeMax,double rangeMin=0)
        {
            AirportsInRange.Clear();
            int num = 0;
            foreach (var airport in airports)
            {
                if (airport != this)
                {
                    double _dist = airport.DistanceTo(this);
                    if ((_dist < rangeMax)&&(_dist>rangeMin))
                    {
                        airport._distance = _dist;
                        AirportsInRange.Add(airport);
                    }
                }
            }
            AirportsInRange.Sort((a, b) => b.distance.CompareTo(a.distance));
            return AirportsInRange.Count;
        }


        public static async Task<List<Aeroport>> fetchAirports(string baseUrl, DateTime lastUpdateFileTime,string token)
        {
            initPath();
            //long epoch = 0; // lastUpdateFileTime.ToFileTime();
            DateTime creationTime = DateTime.MinValue;
            if (File.Exists(DBFILEPATH))
            {
                FileInfo fi = new FileInfo(DBFILEPATH);
                creationTime = fi.LastWriteTime;
                //epoch = (long)(creationTime - new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc)).TotalMilliseconds;
            }
            string url = baseUrl + "api/api_getLastAirportUpdate.php";
            url += "?session_token=" + Uri.EscapeDataString(token);

            UrlDeserializer dataReader = new UrlDeserializer(url);
            DateTime lastUpdate = await dataReader.FetchLastUpdateAsync();

            List<Aeroport> result= new List<Aeroport>();
            if (lastUpdate > creationTime)
            {
                //if the last update is more recent than the local file, fetch the airports from the server.
                Logger.WriteLine("Last update of the airport database is more recent than the local file, fetching from server.");
                //fetch the airports from the server.               
                
                url = baseUrl + "api/api_getAirports.php";
                url += "?session_token=" + Uri.EscapeDataString(token);

                dataReader = new UrlDeserializer(url);
                Logger.WriteLine("Fechting airport informations from server");
                result = await dataReader.FetchAirportsDataAsync(DBFILEPATH);
            }
            //if no new airport database, just load the local one.
            if (result.Count == 0)
            {
                //no airports from the server, try to load the local database.
                if (File.Exists(DBFILEPATH))
                {
                    Logger.WriteLine("Loading local airport database");
                    //read the aeroports.json file.
                    StreamReader sr = new StreamReader(DBFILEPATH);
                    string allData = "{\"success\":true,\"aeroports\":"+sr.ReadToEnd()+"}";
                    result = deserializeAeroports(allData);
                    if (null == result)
                    {
                        result = new List<Aeroport>();
                    }
                }
                else
                {
                    //no data from server and no local file available... it sucks....
                    Logger.WriteLine("No airport database available, please check your internet connection or the server status.");
                }
            }

            return result;
        }

        public static async Task<float> fetchFreight(string baseUrl, string airportID, string token)
        {

            string url = baseUrl + "api/api_getFretByIcao.php?ICAO=" + airportID;
            url += "&session_token=" + Uri.EscapeDataString(token);

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
            var data = JsonConvert.DeserializeObject<AeroportPhpList>(jsonString);
            List<Aeroport> aeroports = new List<Aeroport>();
            IFormatProvider provider = CultureInfo.InvariantCulture;

            //index to count the airports, to help finding a potential error in the airports.
            int i = 1;
            if (data != null)
            {
                foreach (Dictionary<string, string> item in data.aeroports)
                {
                    try
                    {
                        Aeroport a = new Aeroport();
                        a.ident = GetStringValueOrDefault(item,"ident", "unknown" + i);
                        a.type_aeroport = GetStringValueOrDefault(item, "type_aeroport", "unknown" + i);
                        a.name = GetStringValueOrDefault(item,"name", "unknown" + i);
                        a.municipality = GetStringValueOrDefault(item,"municipality", "unknown" + i);
                        a.latitude_deg = GetDoubleValueOrDefault(item,"latitude_deg", "0");
                        a.longitude_deg = GetDoubleValueOrDefault(item,"longitude_deg", "0");
                        a.elevation_ft = GetDoubleValueOrDefault(item,"ekevation_ft", "0");
                        a.Piste = GetStringValueOrDefault(item, "Piste", "unknown" + i);
                        a.Longueur_de_piste = GetStringValueOrDefault(item,"Longueur_de_piste", "? " + i);
                        a.Type_de_piste = GetStringValueOrDefault(item, "Type_de_piste", "unknown" + i);
                        a.Observations = GetStringValueOrDefault(item, "Observations", "unknown" + i);
                        a.Wikipedia_Link = GetStringValueOrDefault(item, "wikipedia_link", "unknown" + i);
                        a.fret = (float)GetDoubleValueOrDefault(item, "fret", "-1");
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
            FretPhpInfo result = JsonConvert.DeserializeObject<FretPhpInfo>(jsonString);
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
