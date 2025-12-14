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
         string sLat ;
         string sLon;
        //only 2 decimals 
         string sPlaneWeight;
        string sAlt ;
        string sSpeed ;
        string sVSpeed ;
        string sHeading ;
        string sBank ;
        string sPitch ;
        string sFuelFlow ;
        string sManifold ;
        string sRPMs ;

        public FLightParamsSample(situation param)
        {
             sLat = param.position.Location.Latitude.ToString();
             sLon = param.position.Location.Longitude.ToString();
            //only 2 decimals 
             sPlaneWeight = param.planeWeight.ToString("F2");
             sAlt = param.position.Altitude.ToString("F2");
             sSpeed = param.airSpeed.ToString("F2");
             sVSpeed = param.verticalSpeed.ToString("F2");
             sHeading = param.position.HeadingDegreesTrue.ToString("F2");
             sBank = param.position.BankDegrees.ToString("F2");
             sPitch = param.position.PitchDegrees.ToString("F2");
             sFuelFlow = param.averageFuelFlow.ToString("F2");
             sManifold = param.engine1ManifoldPressure.ToString();
             sRPMs = param.engine1RPM.ToString();
            
            sampleTime = DateTime.Now;
        }

        public string toCSVString()
        {
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
    }
}
