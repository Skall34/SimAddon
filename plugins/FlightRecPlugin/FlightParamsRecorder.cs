using SimAddonLogger;
using SimAddonPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlightRecPlugin
{
    public class FLightParamsSample
    {
        
        public DateTime sampleTime;
        public double Lat { get; set; }
        public double Lon { get; set; }
        //only 2 decimals 
        public double PlaneWeight { get; set; }
        public double Alt { get; set; }
        public double Speed { get; set; }
        public double VSpeed { get; set; }
        public double Heading { get; set; }
        public double Bank { get; set; }
        public double Pitch { get; set; }
        public double FuelFlow { get; set; }
        public double Manifold { get; set; }
        public double RPMs { get; set; }

        // Constructeur sans paramètres pour la désérialisation JSON
        public FLightParamsSample()
        {
        }

        public FLightParamsSample(situation param)
        {
             Lat = param.position.Location.Latitude;
             Lon = param.position.Location.Longitude;
            //only 2 decimals 
             PlaneWeight = param.planeWeight;
             Alt = param.position.Altitude;
             Speed = param.airSpeed;
             VSpeed = param.verticalSpeed;
             Heading = param.position.HeadingDegreesTrue;
             Bank = param.position.BankDegrees;
             Pitch = param.position.PitchDegrees;
             FuelFlow = param.totalFuelFlow;
             Manifold = param.engine1ManifoldPressure;
             RPMs = param.engine1RPM;
            
            sampleTime = DateTime.Now;
        }

        public string toCSVString()
        {
            string sLat = Lat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
            string sLon = Lon.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
            string sAlt = Alt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sPlaneWeight = PlaneWeight.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sSpeed = Speed.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sVSpeed = VSpeed.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sHeading = Heading.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sBank = Bank.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sPitch = Pitch.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sFuelFlow = FuelFlow.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sManifold = Manifold.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string sRPMs = RPMs.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

            return $"{sampleTime};{sLat};{sLon};{sAlt};{sPlaneWeight};{sSpeed};{sVSpeed};{sHeading};{sBank};{sPitch};{sFuelFlow};{sManifold};{sRPMs}";
        }

        public static string getCSVHeader()
        {
            return "Timestamp;Latitude;Longitude;Altitude;PlaneWeight;AirSpeed;VSpeed;Heading;Bank;Pitch;Fuelflow;Manifold;RPMs";
        }
    }

    public class FlightParamsRecorder
    {
        private string storageFolder;

        private List<FLightParamsSample> FlightParams;
        public FlightParamsRecorder()
        {
            FlightParams = new List<FLightParamsSample>();
            // Get the application name
            string appName = Assembly.GetEntryAssembly().GetName().Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            storageFolder = Path.Combine(appDataPath, appName);

            // Ensure the directory exists, if not, create it
            Directory.CreateDirectory(storageFolder);
        }
        public void RecordFlightParams(situation data)
        {
            FLightParamsSample newSituation = new FLightParamsSample(data);
            FlightParams.Add(newSituation);
        }

        public List<FLightParamsSample> GetRecordedFlightParams()
        {
            return FlightParams;
        }

        public void ClearRecordedFlightParams()
        {
            FlightParams.Clear();
        }

        public StringBuilder getCSVText()
        {
            StringBuilder csvContent = new StringBuilder();
            // Header
            csvContent.AppendLine(FLightParamsSample.getCSVHeader());
            foreach (FLightParamsSample param in FlightParams)
            {
                csvContent.AppendLine(param.toCSVString());
            }
            return csvContent;
        }

        public static string getCSVText(List<FLightParamsSample> flightParams)
        {
            StringBuilder csvContent = new StringBuilder();
            // Header
            csvContent.AppendLine(FLightParamsSample.getCSVHeader());
            foreach (FLightParamsSample param in flightParams)
            {
                csvContent.AppendLine(param.toCSVString());
            }
            return csvContent.ToString();
        }

        public void saveToCSV(string filename)
        {
            string fullFileName = filename+".csv";
            if (Path.IsPathFullyQualified(fullFileName) == false)
            {
                fullFileName = Path.Combine(storageFolder, fullFileName);
            }

            try
            {
                //use GetCSVText to get the content
                StringBuilder csvContent = getCSVText();
                System.IO.File.WriteAllText(fullFileName, csvContent.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred while saving flight parameters to CSV: {ex.Message}");
            }
        }

        public void saveToJSON(string filename)
        {
            string fullFileName = filename + ".json";

            if (Path.IsPathFullyQualified(fullFileName) == false)
            {
                fullFileName = Path.Combine(storageFolder, fullFileName);
            }
            try
            {

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonString = System.Text.Json.JsonSerializer.Serialize(FlightParams, options);
                System.IO.File.WriteAllText(fullFileName, jsonString);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred while saving flight parameters to JSON: {ex.Message}");
            }
        }

        public static string toMDString(List<FLightParamsSample> samples)
        {
            //compute some max values
            double maxAlt = samples.Max(f => f.Alt);
            double maxSpeed = samples.Max(f => f.Speed);
            double maxRPMs = samples.Max(f => f.RPMs);
            double averadgeFuelFlow = samples.Average(f => f.FuelFlow);
            double averageManifold = samples.Average(f => f.Manifold);

            StringBuilder mdContent = new StringBuilder();
            mdContent.AppendLine("# Flight Parameters Recording");
            mdContent.AppendLine();
            mdContent.AppendLine("## Summary Statistics");
            mdContent.AppendLine();
            mdContent.AppendLine($"- Maximum Altitude: {maxAlt:F2} ft");
            mdContent.AppendLine();
            mdContent.AppendLine($"- Maximum Speed: {maxSpeed:F2} knots");
            mdContent.AppendLine();
            mdContent.AppendLine($"- Maximum RPMs: {maxRPMs:F2} RPM");
            mdContent.AppendLine();
            mdContent.AppendLine($"- Average Fuel Flow: {averadgeFuelFlow:F2} gallons/hour");
            mdContent.AppendLine();
            mdContent.AppendLine($"- Average Manifold Pressure: {averageManifold:F2} inHg");
            mdContent.AppendLine();

            return mdContent.ToString();
        }

        internal static string toHTMLString(List<FLightParamsSample> flightParamsData)
        {
            //compute some max values
            double maxAlt = flightParamsData.Max(f => f.Alt);
            double maxSpeed = flightParamsData.Max(f => f.Speed);
            double maxRPMs = flightParamsData.Max(f => f.RPMs);
            double averadgeFuelFlow = flightParamsData.Average(f => f.FuelFlow);
            double averageManifold = flightParamsData.Average(f => f.Manifold);
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.AppendLine("<h3>Flight Parameters Recording</h3>");
            htmlContent.AppendLine("<h4>Summary Statistics</h4>");
            htmlContent.AppendLine("<ul>");
            htmlContent.AppendLine($"<li><strong>Maximum Altitude:</strong> {maxAlt:F2} ft</li>");
            htmlContent.AppendLine($"<li><strong>Maximum Speed:</strong> {maxSpeed:F2} knots</li>");
            htmlContent.AppendLine($"<li><strong>Maximum RPMs:</strong> {maxRPMs:F2} RPM</li>");
            htmlContent.AppendLine($"<li><strong>Average Fuel Flow:</strong> {averadgeFuelFlow:F2} gallons/hour</li>");
            htmlContent.AppendLine($"<li><strong>Average Manifold Pressure:</strong> {averageManifold:F2} inHg</li>");
            htmlContent.AppendLine("</ul>");
            return htmlContent.ToString();
        }
    }
}
