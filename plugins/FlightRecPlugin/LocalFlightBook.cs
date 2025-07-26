using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRecPlugin
{
    public class Flight
    {
        public string immatriculation { get; set; }
        public string departureICAO { get; set; }
        public double departureFuel { get; set; }
        public DateTime departureTime { get; set; }
        public string arrivalICAO { get; set; }
        public double arrivalFuel { get; set; }
        public DateTime arrivalTime { get; set; }
        public short noteDuVol { get; set; }
        public string mission { get; set; }
        public string commentaire { get; set; }
        public double payload { get; set; }
        public string GPSData { get; set; } = string.Empty;
    }

    public class LocalFlightBook
    {
        public List<Flight> Flights { get; set; } = new List<Flight>();
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

        public void loadFromJson(string jsonfilename)
        {
            try
            {
                string json = System.IO.File.ReadAllText(jsonfilename, Encoding.UTF8);
                Flights = System.Text.Json.JsonSerializer.Deserialize<List<Flight>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading flights from JSON: {ex.Message}");
            }
        }

        public void saveToJson(string filename)
        {
            try
            {
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
