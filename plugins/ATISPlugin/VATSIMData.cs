﻿using FSUIPC;
using Newtonsoft.Json;
using SimAddonLogger;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ATISPlugin
{

    public class GeneralData
    {
        public int version { get; set; }
        public int reload { get; set; }
        public string update { get; set; }
        public string update_timestamp { get; set; }
        public int connected_clients { get; set; }
        public int unique_users { get; set; }
    }

    public class FlightplanData
    {
        public string flight_rules{ get; set; }
        public string aircraft { get; set; }
        public string aircraft_faa { get; set; }
        public string aircraft_short { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public string alternate { get; set; }
        public string deptime { get; set; }
        public string route_time { get; set; }
        public string fuel_time { get; set; }
        public string remarks { get; set; }
        public string route { get; set; }
        public int revision_id { get; set; }
        public string assigned_transponder { get; set; }
    }

    public class PilotData
    {
        public int cid { get; set; }
        public string name { get; set; }

        public string callsign { get; set; }
        public string server { get; set; }
        public int pilot_rating { get; set; }
        public int military_rating { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }
        public int groundspeed { get; set; }
        public string transponder { get; set; }
        public int heading { get; set; }
        public double qnh_i_hg { get; set; }
        public int qnh_mb { get; set; }

        public FlightplanData flight_plan;

        public string logon_time { get; set; }
        public string last_updated { get; set; }
    }

    public class ControllerData 
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string callsign { get; set; }
        public string frequency { get; set; }

        public int facility { get; set; }
        public int rating { get; set; }
        public string server { get; set; }
        public int visual_range { get; set; }

        public string[] text_atis;
        public  string last_updated { get; set; }
        public string logon_time { get; set; }
    }

    public class ATISData : ControllerData
    {
        public string real_name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class ServerData
    {
        public string ident { get; set; }
        public string hostname_or_ip { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public bool client_connection_allowed { get; set; }
        public bool is_sweatbox { get; set; }
    }

    public class PreFileData
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string callsign { get; set; }

        public FlightplanData flight_plan;
        public string date_time { get; set; }
    }

    public class FacilityData
    {
        public int id { get; set; }

        [JsonProperty("short")]
        public string short_name { get; set; }

        [JsonProperty("long")]
        public string long_name { get; set; }
    }

    public class RatingData
    {
        public int id { get; set; }

        [JsonProperty("short")]
        public string short_name { get; set; }

        [JsonProperty("long")]
        public string long_name { get; set; }
    }

    public class PilotRatingData
    {
        public int id { get; set; }

        public string short_name { get; set; }

        public string long_name { get; set; }
    }
    public class MilitaryRatingData
    {
        public int id { get; set; }

        public string short_name { get; set; }

        public string long_name { get; set; }
    }

    public class VatsimData
    {
        public GeneralData general;
        public PilotData[] pilots;
        public ControllerData[] controllers;
        public ATISData[] atis;
        public ServerData[] servers;
        public PreFileData[] prefiles;
        public FacilityData[] facilities;
        public RatingData[] ratings;
        public PilotRatingData[] pilot_ratings;
        public MilitaryRatingData[] military_ratings;
    }

    //a clas to get only ATIS Data from vatsim
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

    public class VATSIM
    {
        public static VatsimData data = new VatsimData();

        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<bool> refresh()
        {
            // Construire l'URL avec le code ICAO
            string url = $"https://data.vatsim.net/v3/vatsim-data.json";
            bool result = false;
            try
            {
                // Envoyer la requête HTTP GET
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Vérifier que la requête a réussi

                // Lire le contenu de la réponse
                string responseBody = await response.Content.ReadAsStringAsync();

                // Extraire la section METAR du contenu HTML (selon le format attendu)
                string RawVATSIMData = responseBody.TrimEnd();
                data = JsonConvert.DeserializeObject<VatsimData>(RawVATSIMData);
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
            foreach (ATISData airport in data.atis)
            {

                double distance = NavigationHelper.GetDistance(airport.latitude, airport.longitude, targetLatitude, targetLongitude);
                if (distance < range)
                {
                    airportsInRange.Add(airport.name);
                }
            }
            return airportsInRange;
        }

        public static List<ATISData> FindATIS(string ICAO)
        {
            List<ATISData> result = new List<ATISData>();

            if (data.atis != null)
            {
                foreach (ATISData a in data.atis)
                {
                    if (a.callsign.StartsWith(ICAO))
                    {
                        result.Add(a);
                    }
                }
            }

            return result;
        }

        public static string GetRatingLabel(ControllerData c)
        {
            string result = "";
            int rating = c.rating;
            result = data.ratings.FirstOrDefault(r => r.id == rating).long_name;
            return result;
        }

        public static string GetFacilityLabel(ControllerData c)
        {
            string result = "";
            int facility = c.facility;
            result = data.facilities.FirstOrDefault(r => r.id == facility).long_name;
            return result;
        }


        public static List<ControllerData> FindControllers(string ICAO)
        {
            List<ControllerData> result = new List<ControllerData>();
            if (data.controllers != null)
            {
                foreach (ControllerData c in data.controllers)
                {
                    if (c.callsign.StartsWith(ICAO))
                    {
                        result.Add(c);
                    }
                    else
                    {
                        //look for remote controllers... 
                        //search for ICAO in ATIS_Text
                        if ((c.facility >= 4) && (c.facility <= 6)) // 4 = tower, 5 = approach, 6 = en route
                        {
                            if (c.text_atis != null)
                            {
                                // search for the ICAO
                                foreach (string s in c.text_atis)
                                {
                                    if (s.Contains(ICAO))
                                    {
                                        result.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (data.atis != null)
            {
                foreach (ControllerData c in data.atis)
                {
                    if (c.callsign.StartsWith(ICAO))
                    {
                        result.Add(c);
                    }

                }
            }
            return result;
        }
    }

}
