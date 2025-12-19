using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlightRecPlugin
{
    public class Flight
    {
        public string immatriculation { get; set; }
        public string departureICAO { get; set; }
        public string departureAirportName { get; set; }
        public double departureFuel { get; set; }
        public DateTime departureTime { get; set; }
        public string arrivalICAO { get; set; }
        public string arrivalAirportName { get; set; }
        public double arrivalFuel { get; set; }
        public DateTime arrivalTime { get; set; }
        public short noteDuVol { get; set; }
        public string mission { get; set; }
        public string commentaire { get; set; }
        public double payload { get; set; }
        public List<GPSPoint> GPSData { get; set; }
        public List<FLightParamsSample> FlightParamsData { get; set; }
        public string SimPlane { get; set; }

        public string GenerateHTMLReport()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"<h2>Flight Report for {immatriculation}</h2>");
            report.AppendLine("<br>");
            
            // Sim Plane
            report.AppendLine($"<strong>Aircraft:</strong> {SimPlane}<br><br>");
            // Departure and Arrival
            report.AppendLine($"<strong>Departure:</strong> {departureAirportName} ({departureICAO}) at {departureTime.ToString("g", CultureInfo.InvariantCulture)}<br><br>");
            report.AppendLine($"<strong>Arrival:</strong> {arrivalAirportName} ({arrivalICAO}) at {arrivalTime.ToString("g", CultureInfo.InvariantCulture)}<br><br>");
            report.AppendLine($"<strong>Departure Fuel:</strong> {departureFuel} Kg<br><br>");
            report.AppendLine($"<strong>Arrival Fuel:</strong> {arrivalFuel} Kg<br><br>");
            // Payload & Mission
            report.AppendLine($"<strong>Payload:</strong> {payload} Kg<br><br>");
            report.AppendLine($"<strong>Mission:</strong> {mission}<br><br>");
            report.AppendLine($"<strong>Flight Rating:</strong> {noteDuVol}/10<br><br>");
            report.AppendLine("<h3>Comments</h3>");
            report.AppendLine($"{commentaire}<br><br>");
            if (FlightParamsData != null && FlightParamsData.Count > 0)
            {
                report.AppendLine(FlightParamsRecorder.toHTMLString(FlightParamsData));
            }
            return report.ToString();
        }
        public string GenerateMarkdownReport()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"# Flight Report for {immatriculation}");
            report.AppendLine();
            
            // Sim Plane
            report.AppendLine($"**Aircraft:** {SimPlane}");
            report.AppendLine();

            // Departure and Arrival
            report.AppendLine($"**Departure:** {departureAirportName} ({departureICAO}) at {departureTime.ToString("g", CultureInfo.InvariantCulture)}");
            report.AppendLine();
            report.AppendLine($"**Arrival:** {arrivalAirportName} ({arrivalICAO}) at {arrivalTime.ToString("g", CultureInfo.InvariantCulture)}");
            report.AppendLine();
            report.AppendLine($"**Departure Fuel:** {departureFuel} Kg");
            report.AppendLine();
            report.AppendLine($"**Arrival Fuel:** {arrivalFuel} Kg");
            report.AppendLine();

            // Payload & Mission
            report.AppendLine($"**Payload:** {payload} Kg");
            report.AppendLine();
            report.AppendLine($"**Mission:** {mission}");
            report.AppendLine();
            report.AppendLine($"**Flight Rating:** {noteDuVol}/10");
            report.AppendLine();
            report.AppendLine("## Comments");
            report.AppendLine(commentaire);
            report.AppendLine();
            if (FlightParamsData != null && FlightParamsData.Count > 0)
            {
                report.AppendLine(FlightParamsRecorder.toMDString(FlightParamsData));
            }

            return report.ToString();
        }

        public string GenerateJSONReport()
        {
            return System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        }
    }

    public class LocalFlightBook
    {
        public List<Flight> Flights { get; set; } = new List<Flight>();
        private string storageFolder;

        public void AddFlight(Flight flight)
        {
            Flights.Add(flight);
        }
        public void RemoveFlight(Flight flight)
        {
            Flights.Remove(flight);
        }
        public List<Flight> GetFlights()
        {
            return Flights;
        }
        public void ClearFlights()
        {
            Flights.Clear();
        }

        public Flight GetLastFLight()
        {
            if (Flights.Count > 0)
            {
                return Flights[Flights.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public LocalFlightBook()
        {
            Flights = new List<Flight>();
            // Get the application name
            string appName = Assembly.GetEntryAssembly().GetName().Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            storageFolder = Path.Combine(appDataPath, appName);

            // Ensure the directory exists, if not, create it
            Directory.CreateDirectory(storageFolder);
        }

        //load the flightbook. Return the size of the file in Mo
        public float loadFromJson(string jsonfilename)
        {
            float filesizeInMo = 0;
            try
            {
                if (Path.IsPathFullyQualified(jsonfilename) == false)
                {
                    jsonfilename = Path.Combine(storageFolder, jsonfilename);
                }
                string json = System.IO.File.ReadAllText(jsonfilename, Encoding.UTF8);
                Flights = System.Text.Json.JsonSerializer.Deserialize<List<Flight>>(json);
                FileInfo fileInfo = new FileInfo(jsonfilename);

                filesizeInMo = ((float)fileInfo.Length) / (1024 * 1024);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred while loading flights from JSON: {ex.Message}");
            }
            return filesizeInMo;
        }

        public void saveToJson(string filename)
        {
            try
            {
                if (Path.IsPathFullyQualified(filename) == false)
                {
                    filename = Path.Combine(storageFolder, filename);
                }

                string allflights = System.Text.Json.JsonSerializer.Serialize(Flights);
                using (var file = new System.IO.StreamWriter(filename, false, Encoding.UTF8))
                {
                    file.Write(allflights);
                }
                Logger.WriteLine($"Flights saved to {filename} successfully.");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error saving flights to {filename}: {ex.Message}");
            }
        }

        internal void RemoveFlightAt(int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < Flights.Count)
            {
                Flights.RemoveAt(selectedIndex);
                Logger.WriteLine($"Flight at index {selectedIndex} removed successfully.");
            }
            else
            {
                Logger.WriteLine($"Error: Invalid index {selectedIndex} for removing flight.");
            }
        }
    }
}
