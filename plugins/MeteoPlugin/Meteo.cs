using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MeteoPlugin.Meteo.METARData;

namespace MeteoPlugin
{
    public static class Meteo
    {
        public class METARData
        {
            public const string CST_AUTO = "AUTO";
            public const string CST_RMK = "RMK";

            public enum METARPARTS{ 
                ICAO,
                DATE,
                AUTO,
                WIND,
                WINDVARIATION,
                VISIBILITY,
                RUNWAYVISUALRANGE,
                PRESENTWEATHER,
                CLOUDLAYERS,
                TEMPERATURE,
                ALTIMETER,
                RECENTWEATHER,
                WINDSHEAR,
                RMK
            }

            public abstract class METARItem
            {

            }

            public class METARIcao:METARItem
            {
                public string code { get; set; }

                public METARIcao(string METARPart)
                {
                    code = METARPart;
                }
                public override string ToString()
                {
                    return "Station: "+code;
                }
            }

            public class METARDate : METARItem
            {
                public string day { get; set; }
                public  string time { get; set; }

                public METARDate (string METARPart) {
                    Regex r = new Regex("^(?<day>\\d{2})(?<hour>\\d{2})(?<minute>\\d{2})Z");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        day = result.Groups["day"].Value;
                        time = result.Groups["hour"].Value+":"+ result.Groups["minute"].Value;
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid METAR date format");
                    }
                }

                public override string ToString()
                {
                    return "Date/time : "+day+"/"+ time;
                    
                }
            }
            public class METARWind : METARItem
            {
                public string Direction { get; set; }
                public string Speed { get; set; }
                public bool HasGusts { get; set; }
                public string GustsSpeed { get; set; }
                public string Unit { get; set; }

                public METARWind(string METARPart)
                {
                    Regex r = new Regex("^(?<direction>\\d{3})(?<speed>\\d{2,3})(?<gusts>G\\d{2})?KT");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        Direction = result.Groups["direction"].Value;
                        Speed = result.Groups["speed"].Value;
                        if (result.Groups["gusts"].Value != "")
                        {
                            GustsSpeed = result.Groups["gusts"].Value.Substring(1);
                            HasGusts = true;
                        }
                        else
                        {
                            HasGusts = false;
                        }
                        Unit = METARPart.Substring(METARPart.Length - 2, 2);
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid METAR wind format");
                    }
                }

                public override string ToString()
                {
                    string result = string.Format("Wind : {0} {1} at {2}", Speed, Unit, Direction);
                    if (HasGusts)
                    {
                        result += string.Format("with {0} {1} gusts",GustsSpeed,Unit);
                    }
                    return result;
                }
            }

            public class METARWindVariation : METARItem
            {
                public string StartAngle { get; set; }
                public string EndAngle { get; set; }

                public METARWindVariation(string METARPart)
                {
                    Regex r = new Regex("^(?<start>\\d{3})V(?<end>\\d{3})");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        StartAngle = result.Groups["start"].Value;
                        EndAngle = result.Groups["end"].Value;
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid METAR wind variation format");
                    }
                }

                public override string ToString()
                {
                    string result = string.Format("Variation between {0} and {1}", StartAngle, EndAngle);
                    return result;
                }


            }

            public class METARVisibility : METARItem
            {
                public bool IsCAVOK { get; set; }
                public string Distance { get; set; }
                public string Unit { get; set; }

                public METARVisibility(string METARPart)
                {
                    Regex r = new Regex("^(?<distance>\\S{2,3}\\d)(?<unit>\\S{1,2})?");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        Distance = result.Groups["distance"].Value;
                        Unit = result.Groups["unit"].Value;
                        IsCAVOK = false;
                    }
                    else
                    {
                        if (METARPart == "CAVOK")
                        {
                            IsCAVOK = true;
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid METAR wind variation format");
                        }
                    }

                }

                public override string ToString()
                {
                    if (IsCAVOK)
                    {
                        return "Visibility : CAVOK";
                    }
                    else
                    {
                        return "Visibility : "+ Distance + " " + Unit;
                    }
                }
            }

            public class METARRunwayVisualRange : METARItem
            {

                public enum TrendValue
                {
                    UPWARD,
                    DOWNWARD,
                    NO_CHANGE
                }
                public string Runway { get; set; }
                public string Distance { get; set; }
                public string DistanceQualifier { get; set; }
                public string variableDistance { get; set; }

                public string Unit { get; set; }
                public TrendValue Trend { get; set; }

                public METARRunwayVisualRange(string METARPart)
                {
                    string[]subparts = METARPart.Split('/');
                    if (subparts.Length == 3)
                    {
                        Runway = subparts[0].Substring(1);

                        Trend = TrendValue.NO_CHANGE;
                        if (subparts[2] == "U")
                        {
                            Trend = TrendValue.UPWARD;
                        }
                        if (subparts[2] == "D")
                        {
                            Trend = TrendValue.DOWNWARD;
                        }

                        string[] distanceItems = subparts[1].Split("V");
                        Regex r = new Regex("^(?<qualifier>^P?M?)(?<distance>\\d{3,4}FT)");
                        Match result = r.Match(subparts[0]);
                        if (result.Success)
                        {
                            DistanceQualifier = result.Groups["qualifier"].Value;
                            Distance = result.Groups["distance"].Value;
                            if (distanceItems.Length == 2)
                            {
                                variableDistance = distanceItems[1];
                            }
                        }
                        else
                        {
                            throw new InvalidDataException("bad RVR distance value");
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("bad RVR value");
                    }

                }

                public override string ToString()
                {
                    string result = "";
                    result += string.Format("Runway visual range for {0}, distance {1}FT");
                    if (Trend == TrendValue.UPWARD)
                    {
                        result += " upward";
                    }
                    if (Trend == TrendValue.DOWNWARD)
                    {
                        result += " downward";
                    }
                    return result;
                }

            }

            public class METARWeather : METARItem
            {
                public static class QualifierValue
                {
                    public const string LIGHT = "-";
                    public const string NORMAL = "";
                    public const string HEAVY = "+";
                    public const string PROXIMITY = "VC";
                }

                public static class DescriptorValue
                {
                    public static readonly Dictionary<string, string> Values = new Dictionary<string, string>
                    {
                        { "MI","shallow"},
                        { "BC","patches" },
                        { "PR","partial" },
                        { "DR","drifting" },
                        { "BL","blowing" },
                        { "SH","shower" },
                        { "TS","thunderstorm" },
                        { "FZ","freezing" },
                    };          
                }


                public static class PrecipitationValue
                {
                    public static readonly Dictionary<string, string> Values = new Dictionary<string, string>
                    {
                        { "DZ","drizzle"},
                        { "RA","rain" },
                        { "SN","snow" },
                        { "SG","snowgrain" },
                        { "IC","ice crystals" },
                        { "PL","ice pellets" },
                        { "GR","hail" },
                        { "GS","snow pellets" },
                        { "UP","unknown" }
                    };
                }

                public static class ObscurationValue
                {
                    public static readonly Dictionary<string, string> Values = new Dictionary<string, string>
                    {
                        { "BR","mist"},
                        { "FG","fog" },
                        { "FU","smoke" },
                        { "VA","volcanic ash" },
                        { "DU","dust" },
                        { "SA","sand" },
                        { "HZ","haze" }
                    };
                }

                public static class OtherValues
                {
                    public static readonly Dictionary<string, string> Values = new Dictionary<string, string>
                    {
                        { "PO","dust devil"},
                        { "SQ","squalls" },
                        { "+FC","tornado" },
                        { "FC","funnel cloud" },
                        { "SS","sand storm" },
                        { "DS","dust storm" }
                    };
                }

                public string Qualifier;
                public string Descriptor;
                public string Precipitation;
                public string Obscuration;
                public string Other;

                public METARWeather(string METARPart)
                {
                    Regex r = new Regex("^(\\+|-)?([A-Z]{2,6})$");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        int index = 0;
                        Qualifier = "Moderate"; //default qualifier
                        if (METARPart.StartsWith(QualifierValue.LIGHT))
                        {
                            Qualifier = "Light";
                            index++;
                        }
                        if (METARPart.StartsWith(QualifierValue.HEAVY))
                        {
                            Qualifier = "Heavy";
                            index++;
                        }
                        if (METARPart.StartsWith(QualifierValue.PROXIMITY))
                        {
                            Qualifier = "Proximity";
                            index += 2;
                        }
                        while (index < METARPart.Length)
                        {
                            string toParse = METARPart.Substring(index);
                            foreach (string k in DescriptorValue.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Descriptor += DescriptorValue.Values[k] + " ";
                                    index += k.Length;
                                }
                            }
                            foreach (string k in PrecipitationValue.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Precipitation += PrecipitationValue.Values[k] + " ";
                                    index += k.Length;
                                }
                            }
                            foreach (string k in ObscurationValue.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Obscuration = ObscurationValue.Values[k];
                                    index += k.Length;
                                }
                            }
                            foreach (string k in OtherValues.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Other = OtherValues.Values[k];
                                    index += k.Length;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("Not weather data " + METARPart);
                    }


                }

                public override string ToString()
                {
                    string result = "Weather : " + Qualifier + " ";
                    if (Descriptor!=string.Empty)
                    {
                        result += Descriptor + " ";
                    }
                    if (Precipitation != string.Empty)
                    {
                        result += Precipitation + " ";
                    }
                    if (Obscuration != string.Empty)
                    {
                        result += Obscuration + " ";
                    }
                    if (Other != string.Empty)
                    {
                        result += Other + " ";
                    }
                    return result;

                }
            }

            public class METARCloudLayer : METARItem
            {

                public static class CloudAmountValue
                {
                    public static readonly Dictionary<string, string> Values = new Dictionary<string, string>
                    {
                        { "SKC","sky clear"},
                        { "FEW","few" },
                        { "SCT","scattered" },
                        { "BKN","broken" },
                        { "OVC","overcast" },
                        { "CLR","clear" },
                        { "VV","vertical visibility" }
                    };
                }

                public string Amount { get; set; }

                public string Level { get; set; }
                public string cloud { get; set; }
                public METARCloudLayer(string METARPart)
                {
                    Regex r = new Regex(@"^(?<amount>(\S{2,3})|(\/{3}))(?<level>(\d{3})|(\/{3}))(?<cloud>[A-Z]{2,3})?");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        Amount = result.Groups["amount"].Value;
                        Level = result.Groups["level"].Value;
                        cloud = result.Groups["cloud"].Value;
                    }
                    else
                    {
                        throw new InvalidDataException("Error while parsing cloud layer");
                    }
                }

                public override string ToString()
                {
                    string result = "";

                    try
                    {
                        if (Amount != "///") result += "Layer : " + CloudAmountValue.Values[Amount] + " at " + Level + " 00FT";
                    }catch (Exception ex)
                    {
                    }

                    if (cloud == "CB") result += " cumulonimbus";
                    if (cloud == "TCU") result += " towering cumulus";

                    return result;
                }
            }

            public class METARTemperature : METARItem
            {
                public int AirTemperature { get; set; }
                public int DewPointTemperature { get; set; }

                public METARTemperature(string METARPart)
                {
                    Regex r = new Regex("^(?<temp>\\d{2})\\/(?<dew>M?\\d{2})$");
                    Match result= r.Match(METARPart);
                    if (result.Success)
                    {
                        AirTemperature = int.Parse(result.Groups["temp"].Value);
                        if (result.Groups["dew"].Value.StartsWith('M'))
                        {
                            DewPointTemperature = -1 * int.Parse(result.Groups["dew"].Value.Substring(1));
                        }
                        else {
                            DewPointTemperature = int.Parse(result.Groups["dew"].Value);
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("invalid temperature data " + METARPart);
                    }
                }

                public override string ToString()
                {
                    return string.Format("Air temperature : {0}, dew point {1}",
                        AirTemperature.ToString(),
                        DewPointTemperature.ToString());
                }
            }

            public class METARAltimeter : METARItem
            {

                public int Value { get; set; }
                public string Unit { get; set; }
                public METARAltimeter(string METARPart)
                {
                    Regex r = new Regex("^(Q|A)(?<alti>\\d{4})$");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        Value = int.Parse(result.Groups["alti"].Value);
                        if (METARPart.StartsWith('Q'))
                        {
                            Unit = "hectopascals";
                        }
                        else
                        {
                            Unit = "inches of mercury";
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("invalid altimeter data " + METARPart);
                    }
                }

                public override string ToString()
                {
                    return Value.ToString() + Unit;
                }
            }

            public class METARWindShear : METARItem
            {
                public string Runway { get; set; }
            }

            public List<METARItem> items;
            public  string ICAO { get; set; }
            public METARDate Date { get; set; }

            public METARWind Wind { get; set; }
            public METARWindVariation WindVariation { get; set; }
            public METARVisibility Visibility { get; set; }
            public METARRunwayVisualRange RunwayVisualRange { get; set; }
            public METARWeather PresentWeather { get; set; }

            public List<METARCloudLayer> CloudLayers { get; set; }

            public METARTemperature Temperature { get; set; }
            public METARAltimeter Altimeter { get; set; }

            public METARWeather RecentWeather { get; set; }

            public METARData(string rawMETAR)
            {
                items=new List<METARItem>();

                string[] parts = rawMETAR.Split(' ');
                int index = 0;

                METARIcao icao = new METARIcao(parts[(int)index]);
                items.Add(icao);
                index++;

                Date = new METARDate(parts[index]);
                items.Add(Date);
                index++;

                if (parts[index] == METARData.CST_AUTO)
                {
                    index++;
                }

                Wind = new METARWind(parts[index]);
                items.Add(Wind);
                index++;

                try
                {
                    WindVariation = new METARWindVariation(parts[index]);
                    items.Add(WindVariation);

                    index++;
                } catch (Exception ex) {
                    //can happen if there is no wind variation
                }

                try
                {
                    Visibility = new METARVisibility(parts[index]);
                    items.Add(Visibility);
                    index++;
                }
                catch (Exception ex)
                {
                }

                try
                {
                    RunwayVisualRange = new METARRunwayVisualRange(parts[index]);
                    items.Add(RunwayVisualRange);
                    index++;
                }
                catch (Exception ex)
                {
                }

                try
                {
                    PresentWeather = new METARWeather(parts[index]);
                    items.Add(PresentWeather);
                    index++;
                }
                catch (Exception ex)
                {
                }

                bool cloudlayersDone = false;
                while (!cloudlayersDone)
                {
                    try
                    {
                        METARCloudLayer layer = new METARCloudLayer(parts[index]);
                        items.Add(layer);
                        index++;
                    }
                    catch(Exception ex)
                    {
                        cloudlayersDone = true;
                    }
                }

                try
                {
                    Temperature = new METARTemperature(parts[index]);
                    items.Add(Temperature);
                    index++;
                }
                catch (Exception ex)
                {
                }

                try
                {
                    Altimeter = new METARAltimeter(parts[index]);
                    items.Add(Altimeter);
                    index++;
                }
                catch (Exception ex)
                {
                }



            }

        }

        private static readonly HttpClient httpClient = new HttpClient();

        public static async  Task<string> getMetar(string ICAO)
        {
            // Construire l'URL avec le code ICAO
            string url = $"https://aviationweather.gov/cgi-bin/data/metar.php?ids={ICAO}";

            try
            {
                // Envoyer la requête HTTP GET
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Vérifier que la requête a réussi

                // Lire le contenu de la réponse
                string responseBody = await response.Content.ReadAsStringAsync();

                // Extraire la section METAR du contenu HTML (selon le format attendu)
                string metar = responseBody.TrimEnd();

                return metar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du METAR : {ex.Message}");
                return null;
            }
        }

        public static string decodeMetar(string rawMETAR)
        {
            string decoded = "";
            try
            {
                METARData data = new METARData(rawMETAR);

                foreach(METARItem item in data.items)
                {
                    try
                    {
                        decoded += item.ToString() + Environment.NewLine;
                    }catch(Exception ex)
                    {

                    }
                }
            }catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
                
            return decoded;
        }
    }
}
