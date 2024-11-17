using SimAddonLogger;
using System.Text.RegularExpressions;
using static MeteoPlugin.Meteo.METARData;

namespace MeteoPlugin
{
    public static class Meteo
    {
        public class METARData
        {
            // https://meteocentre.com/doc/metar.html

            public const string CST_AUTO = "AUTO";
            public const string CST_TEMPO = "TEMPO";
            public const string CST_BECOMING = "BECMG";
            public const string CST_RMK = "RMK";

            public abstract class METARItem
            {
                public string _category { get; set; }
            }

            public class METARIcao:METARItem
            {
                public string code { get; set; }
                public string name { get; set; }

                public METARIcao(string category,string METARPart)
                {
                    _category = category;
                    code = METARPart;
                }
                public override string ToString()
                {
                    return _category + " : " + code;
                }
            }

            public class METARDate : METARItem
            {
                public string day { get; set; }
                public  string time { get; set; }

                public METARDate (string category, string METARPart) {
                    _category = category;
                    Regex r = new Regex("^(?<day>\\d{2})(?<hour>\\d{2})(?<minute>\\d{2})Z");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        day = result.Groups["day"].Value;
                        time = result.Groups["hour"].Value+":"+ result.Groups["minute"].Value;
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid METAR date format" + METARPart);
                    }
                }

                public override string ToString()
                {
                    return _category + " : " + day+"/"+ time;                   
                }
            }
            public class METARWind : METARItem
            {
                public string Direction { get; set; }
                public string Speed { get; set; }
                public bool HasGusts { get; set; }
                public string GustsSpeed { get; set; }
                public string Unit { get; set; }

                public METARWind(string category, string METARPart)
                {
                    _category = category;
                    Regex r = new Regex("^(?<direction>\\S{3})(?<speed>\\d{2,3})(?<gusts>G\\d{2})?KT");
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
                        throw new InvalidDataException("Invalid METAR wind format" + METARPart);
                    }
                }

                public override string ToString()
                {

                    string result = _category;
                    if (Direction == "VRB")
                    {
                        result += string.Format(" : {0} {1} variable direction", Speed, Unit);
                    }
                    else
                    {
                        result += string.Format(" : {0} {1} at {2}", Speed, Unit, Direction);
                    }
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

                public METARWindVariation(string category, string METARPart)
                {
                    _category = category;
                    Regex r = new Regex("^(?<start>\\d{3})V(?<end>\\d{3})");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        StartAngle = result.Groups["start"].Value;
                        EndAngle = result.Groups["end"].Value;
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid METAR wind variation format" + METARPart);
                    }
                }

                public override string ToString()
                {
                    string result = _category + string.Format(" between {0} and {1}", StartAngle, EndAngle);
                    return result;
                }


            }

            public class METARVisibility : METARItem
            {
                public string Distance { get; set; }
                public string Unit { get; set; }

                public METARVisibility(string category, string METARPart)
                {
                    _category = category;
                    Regex r = new Regex(@"^(?<distance>(\d{1,4})|(\d\/\d))(?<unit>\S{1,2})?");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        Distance = result.Groups["distance"].Value;
                        Unit = result.Groups["unit"].Value;
                        if (Unit=="")
                        {
                            //in case of no unit, use meters
                            Unit = "m";
                        }
                        if (Distance == "9999")
                        {
                            Distance = ">10";
                            Unit = "km";
                        }
                    }
                    else
                    {
                            throw new InvalidDataException("Invalid METAR wind variation format" + METARPart);
                    }

                }

                public override string ToString()
                {
                        return _category + " : " + Distance + " " + Unit;
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

                public METARRunwayVisualRange(string category, string METARPart)
                {
                    _category = category;
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
                            throw new InvalidDataException("bad RVR distance value " + METARPart);
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("bad RVR value "+METARPart);
                    }

                }

                public override string ToString()
                {
                    string result = _category;
                    result += string.Format(" for {0}, distance {1}FT");
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

                public METARWeather(string category, string METARPart)
                {
                    _category = category;
                    Regex r = new Regex(@"^(RE)?(\+|-)?([A-Z]{2,6})$");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        int index = 0;
                        if (METARPart.StartsWith("RE"))
                        {
                            index = 2;
                        }

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
                            bool found = false;
                            string toParse = METARPart.Substring(index);
                            foreach (string k in DescriptorValue.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Descriptor += DescriptorValue.Values[k] + " ";
                                    index += k.Length;
                                    found = true;
                                }
                            }
                            foreach (string k in PrecipitationValue.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Precipitation += PrecipitationValue.Values[k] + " ";
                                    index += k.Length;
                                    found = true;
                                }
                            }
                            foreach (string k in ObscurationValue.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Obscuration = ObscurationValue.Values[k];
                                    index += k.Length;
                                    found = true;
                                }
                            }
                            foreach (string k in OtherValues.Values.Keys)
                            {
                                if (toParse.StartsWith(k))
                                {
                                    Other = OtherValues.Values[k];
                                    index += k.Length;
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                throw new InvalidDataException("Not weather data " + METARPart);
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
                    string result = _category + " : " + Qualifier + " ";
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
                        { "NSC","No significant cloud" },
                        { "VV","vertical visibility" }
                    };
                }

                public bool IsCAVOK { get; set; }

                public string Amount { get; set; }

                public string Level { get; set; }
                public string cloud { get; set; }
                public METARCloudLayer(string category, string METARPart)
                {
                    _category = category;
                    if (METARPart == "CAVOK")
                    {
                        IsCAVOK = true;
                    }
                    else
                    {
                        IsCAVOK = false;
                        if ((METARPart == "CLR")||(METARPart=="NSC"))
                        {
                            Amount = METARPart;
                        }
                        else
                        {
                            Regex r = new Regex(@"^(?<amount>(\S{2,3})|(\/{3})?)(?<level>(\d{3})|(\/{3}))(?<cloud>[A-Z]{2,3})?");
                            Match result = r.Match(METARPart);
                            if (result.Success)
                            {
                                Amount = result.Groups["amount"].Value;
                                Level = result.Groups["level"].Value;
                                cloud = result.Groups["cloud"].Value;
                            }
                            else
                            {
                                throw new InvalidDataException("Error while parsing cloud layer "+METARPart);
                            }
                        }
                    }
                }

                public override string ToString()
                {
                    string result = _category;

                    if (IsCAVOK)
                    {
                        result += " : CAVOK (Ceiling And Visibility OK)";
                    }
                    else
                    {
                        try
                        {
                            if ((Amount == "CLR")||(Amount == "NSC"))
                            {
                                result += " : " + CloudAmountValue.Values[Amount];
                            }
                            else
                            {
                                if (Amount != "///") result += " : " + CloudAmountValue.Values[Amount] + " at " + Level + "00 FT";
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine(ex.Message);
                        }

                        if (cloud == "CB") result += " cumulonimbus";
                        if (cloud == "TCU") result += " towering cumulus";
                    }
                    return result;
                }
            }

            public class METARTemperature : METARItem
            {
                public int AirTemperature { get; set; }
                public int DewPointTemperature { get; set; }

                public METARTemperature(string category, string METARPart)
                {
                    _category = category;
                    Regex r = new Regex("^(?<temp>M?\\d{2})\\/(?<dew>M?\\d{2})$");
                    Match result= r.Match(METARPart);
                    if (result.Success)
                    {
                        if (result.Groups["temp"].Value.StartsWith('M'))
                        {
                            AirTemperature = -1 * int.Parse(result.Groups["temp"].Value.Substring(1));
                        }
                        else
                        {
                            AirTemperature = int.Parse(result.Groups["temp"].Value);
                        }

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
                    return _category + string.Format(" : Air = {0}°C, Dew point = {1}°C",
                        AirTemperature.ToString(),
                        DewPointTemperature.ToString());
                }
            }

            public class METARAltimeter : METARItem
            {

                public int hpaValue { get
                    {
                        if (Unit == "hpa")
                        {
                            return rawValue;
                        }
                        else
                        {
                            return (int)(rawValue * 33.8639 / 100);
                        }
                    }
                }
                public float inHgValue { get
                    {
                        if (Unit == "hpa")
                        {
                            return (float)(rawValue * 0.02953);
                        }
                        else
                        {
                            return (float)(rawValue) / 100;
                        }
                    }

                }

                public int rawValue { get; set; }
                public string Unit { get; set; }
                public METARAltimeter(string category, string METARPart)
                {
                    _category = category;
                    Regex r = new Regex("^(Q|A)(?<alti>\\d{4})$");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        rawValue = int.Parse(result.Groups["alti"].Value);
                        if (METARPart.StartsWith('Q'))
                        {
                            Unit = "hpa";
                        }
                        else
                        {
                            Unit = "inHg";
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("invalid altimeter data " + METARPart);
                    }
                }

                public override string ToString()
                {
                    return _category + " : " + hpaValue.ToString() + " hpa ( "+inHgValue.ToString("0.00")+" inHg )";
                }
            }

            public class METARWindShear : METARItem
            {
                public string Runway { get; set; }

                public METARWindShear(string category,string METARPart)
                {
                    _category = category;
                    Regex r = new Regex(@"^(RWY(?<runway>\\d{2}\\S?))|(ALL RWY)");
                    Match result = r.Match(METARPart);
                    if (result.Success)
                    {
                        if(METARPart=="ALL RWY")
                        {
                            Runway = "all runways";
                        }
                        else
                        {
                            Runway = result.Groups["runway"].Value;
                        }
                    }
                    else
                    {
                        throw new InvalidDataException("invalid windshear data " + METARPart);
                    }
                }

                public override string ToString()
                {
                    string result = _category;
                    if (Runway == "all runway")
                    {
                        result += " on " + Runway;
                    }
                    else {
                        result = " on runway " + Runway;
                    }
                    return result;
                }
            }

            public List<METARItem> items;
            public  string ICAO { get; set; }
            public METARDate Date { get; set; }

            public METARWind Wind { get; set; }
            public METARWind TemporaryWind { get; set; }
            public METARWindVariation WindVariation { get; set; }
            public METARVisibility Visibility { get; set; }
            public METARVisibility TemporaryVisibility { get; set; }
            public METARRunwayVisualRange RunwayVisualRange { get; set; }
            public List<METARWeather> PresentWeather { get; set; }
            public METARWeather TemporaryWeather { get; set; }

            public List<METARCloudLayer> CloudLayers { get; set; }
            public List<METARCloudLayer> TemporaryCloudLayers { get; set; }

            public METARTemperature Temperature { get; set; }
            public METARAltimeter Altimeter { get; set; }

            public METARWeather RecentWeather { get; set; }

            public METARWindShear WindShear { get; set; }

            public METARData(string rawMETAR)
            {
                items=new List<METARItem>();
                try
                {

                    string[] parts = rawMETAR.Split(' ');
                    int index = 0;

                    METARIcao icao = new METARIcao("Station", parts[(int)index]);
                    items.Add(icao);
                    index++;

                    Date = new METARDate("Date/Time", parts[index]);
                    items.Add(Date);
                    index++;

                    if (parts[index] == METARData.CST_AUTO)
                    {
                        index++;
                    }

                    Wind = new METARWind("Wind", parts[index]);
                    items.Add(Wind);
                    index++;

                    try
                    {
                        WindVariation = new METARWindVariation("Wind variation", parts[index]);
                        items.Add(WindVariation);

                        index++;
                    }
                    catch (Exception ex)
                    {
                        //can happen if there is no wind variation
                    }

                    try
                    {
                        Visibility = new METARVisibility("Visibility", parts[index]);
                        items.Add(Visibility);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        RunwayVisualRange = new METARRunwayVisualRange("Runway visual range", parts[index]);
                        items.Add(RunwayVisualRange);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    bool weatherdone = false;
                    PresentWeather = new List<METARWeather>();
                    while (!weatherdone)
                    {
                        try
                        {
                            METARWeather PresentWeatherItem = new METARWeather("Weather", parts[index]);
                            PresentWeather.Add(PresentWeatherItem);
                            items.Add(PresentWeatherItem);
                            index++;
                        }
                        catch (Exception ex)
                        {
                            //if we can't decode that as weather when we're done with weather decoding. 
                            weatherdone = true;
                        }
                    }

                    bool cloudlayersDone = false;
                    CloudLayers = new List<METARCloudLayer>();
                    while (!cloudlayersDone)
                    {
                        try
                        {
                            METARCloudLayer layer = new METARCloudLayer("Cloud layer", parts[index]);
                            CloudLayers.Add(layer);
                            items.Add(layer);
                            index++;
                        }
                        catch (Exception ex)
                        {
                            cloudlayersDone = true;
                        }
                    }

                    try
                    {
                        Temperature = new METARTemperature("Temperature", parts[index]);
                        items.Add(Temperature);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        Altimeter = new METARAltimeter("Altimeter setting", parts[index]);
                        items.Add(Altimeter);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        RecentWeather = new METARWeather("Recent weather", parts[index]);
                        items.Add(RecentWeather);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        //windshear is particular, there is a space between WS and the windshear definition. ex :WS RWY36
                        if (parts[index] == "WS")
                        {
                            index++; //go to next part for windshear definition

                            WindShear = new METARWindShear("Windshear", parts[index]);
                            items.Add(WindShear);
                            index++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("bad windshear definition " + parts[index]);
                    }

                    //skip until finding TEMPO.
                    while ((index < parts.Length) && (parts[index] != CST_TEMPO) && ((parts[index] != CST_BECOMING)))
                    {
                        index++;
                    }
                    if (index == parts.Length)
                    {
                        return;
                    }

                    string nextpart = "";
                    if (parts[index] == CST_TEMPO)
                    {
                        nextpart = "Temporary";
                    }
                    if (parts[index] == CST_BECOMING)
                    {
                        nextpart = "Becoming";
                    }
                    //evenutally we have reached the end of the elements to parse. stop here.

                    //start parsing temporary values.
                    index++;

                    //Temporary wind
                    try
                    {
                        TemporaryWind = new METARWind(nextpart + " wind", parts[index]);
                        items.Add(TemporaryWind);
                        index++;
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        TemporaryVisibility = new METARVisibility(nextpart + " visibility", parts[index]);
                        items.Add(TemporaryVisibility);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        TemporaryWeather = new METARWeather(nextpart + " weather", parts[index]);
                        items.Add(TemporaryWeather);
                        index++;
                    }
                    catch (Exception ex)
                    {
                    }

                    cloudlayersDone = false;
                    TemporaryCloudLayers = new List<METARCloudLayer>();
                    while (!cloudlayersDone)
                    {
                        try
                        {
                            METARCloudLayer layer = new METARCloudLayer(nextpart + " cloud layer", parts[index]);
                            TemporaryCloudLayers.Add(layer);
                            items.Add(layer);
                            index++;
                        }
                        catch (Exception ex)
                        {
                            cloudlayersDone = true;
                        }
                    }
                }catch(Exception ex)
                {
                    Logger.WriteLine("Error while decoding metar " + rawMETAR);
                }
            }

            public string toString()
            {
                string decoded = "";
                try
                {
                        foreach (METARItem item in items)
                        {
                            try
                            {
                                decoded += item.ToString() + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLine(ex.Message);
                            }
                        }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }

                return decoded;
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
                Logger.WriteLine($"Got metar data : {metar}");
                return metar;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Erreur lors de la récupération du METAR : {ex.Message}");
                return null;
            }
        }

        public static METARData decodeMetar(string rawMETAR)
        {
            METARData data = null;
            try
            {
                if (rawMETAR != string.Empty)
                {
                    data = new METARData(rawMETAR);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
