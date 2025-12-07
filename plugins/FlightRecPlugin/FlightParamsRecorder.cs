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
    public class FlightParamsRecorder
    {
        private string storageFolder;

        private List<situation> FlightParams;
        public FlightParamsRecorder()
        {
            FlightParams = new List<situation>();
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
            situation newSituation = new situation(data);
            FlightParams.Add(newSituation);
        }

        public List<situation> GetRecordedFlightParams()
        {
            return FlightParams;
        }

        public void ClearRecordedFlightParams()
        {
            FlightParams.Clear();
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
                StringBuilder csvContent = new StringBuilder();
                // Header
                csvContent.AppendLine("Timestamp;Latitude;Longitude;Altitude;AirSpeed;VSpeed;Heading;Bank;Pitch;Fuelflow");
                foreach (situation param in FlightParams)
                {
                    string sLat = param.position.Location.Latitude.ToString();
                    string sLon = param.position.Location.Longitude.ToString();
                    //only 2 decimals 
                    string sAlt = param.position.Altitude.ToString("F2");
                    string sSpeed = param.airSpeed.ToString("F2");
                    string sVSpeed = param.verticalSpeed.ToString("F2");
                    string sHeading = param.position.HeadingDegreesTrue.ToString("F2");
                    string sBank = param.position.BankDegrees.ToString("F2");
                    string sPitch = param.position.PitchDegrees.ToString("F2");
                    string sFuelFlow = param.averageFuelFlow.ToString("F2");

                    csvContent.AppendLine($"{param.timestamp};{sLat};{sLon};{sAlt};{sSpeed};{sVSpeed};{sHeading};{sBank};{sPitch};{sFuelFlow}");
                }
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
