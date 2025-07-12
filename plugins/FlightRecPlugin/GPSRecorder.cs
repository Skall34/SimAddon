using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRecPlugin
{
    public class GPSPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class GPSRecorder
    {
        //add a point to the GPS trace, with latitude, longitude, altitude, and timestamp
        public List<GPSPoint> GPSPoints { get; private set; } = new List<GPSPoint>();
        public void AddPoint(double latitude, double longitude, double altitude, DateTime timestamp)
        {
            GPSPoints.Add(new GPSPoint
            {
                Latitude = latitude,
                Longitude = longitude,
                Altitude = altitude,
                Timestamp = timestamp
            });
        }

        public void ClearTrace()
        {
            GPSPoints.Clear();
        }

        public void OptimizeTrace()
        {
            // Implement optimization logic here, e.g., removing redundant points
            // For simplicity, this example does not implement any optimization
            // In a real application, you might want to use algorithms like Ramer-Douglas-Peucker
            // to reduce the number of points while preserving the shape of the trace.

            //remove points that are too close to each other
            GPSPoints = GPSPoints
                .Where((point, index) => index == 0 || 
                        (Math.Abs(point.Latitude - GPSPoints[index - 1].Latitude) > 0.00000001 ||
                         Math.Abs(point.Longitude - GPSPoints[index - 1].Longitude) > 0.00000001))
                .ToList();

            //remove intermediate points that are aligned with the previous and next points
            for (int i = 1; i < GPSPoints.Count - 1; i++)
            {
                var prev = GPSPoints[i - 1];
                var current = GPSPoints[i];
                var next = GPSPoints[i + 1];
                if (Math.Abs((current.Latitude - prev.Latitude) * (next.Longitude - current.Longitude) -
                             (current.Longitude - prev.Longitude) * (next.Latitude - current.Latitude)) < 0.0000001)
                {
                    GPSPoints.RemoveAt(i);
                    i--; // Adjust index after removal
                }
            }
        }

        public void SaveToKML(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
                writer.WriteLine("<Document>");
                //draw segments between points
                for (int i = 0; i < GPSPoints.Count - 1; i++)
                {
                    var start = GPSPoints[i];
                    var end = GPSPoints[i + 1];
                    writer.WriteLine("<Placemark>");
                    writer.WriteLine($"<name>{start.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff")} to {end.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff")}</name>");
                    writer.WriteLine("<LineString>");
                    writer.WriteLine("<coordinates>");
                    writer.WriteLine($"{start.Longitude.ToString(CultureInfo.InvariantCulture)},{start.Latitude.ToString(CultureInfo.InvariantCulture)},{start.Altitude.ToString(CultureInfo.InvariantCulture)} ");
                    writer.WriteLine($"{end.Longitude.ToString(CultureInfo.InvariantCulture)},{end.Latitude.ToString(CultureInfo.InvariantCulture)},{end.Altitude.ToString(CultureInfo.InvariantCulture)}");
                    writer.WriteLine("</coordinates>");
                    writer.WriteLine("</LineString>");
                    writer.WriteLine("</Placemark>");
                }
                writer.WriteLine("</Document>");
                writer.WriteLine("</kml>");
            }
        }

        public void SaveToGPX(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<gpx version=\"1.1\" creator=\"FlightRecPlugin\">");
                foreach (var point in GPSPoints)
                {
                    writer.WriteLine("<wpt lat=\"" + point.Latitude + "\" lon=\"" + point.Longitude + "\">");
                    writer.WriteLine($"<ele>{point.Altitude}</ele>");
                    writer.WriteLine($"<time>{point.Timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ")}</time>");
                    writer.WriteLine("</wpt>");
                }
                writer.WriteLine("</gpx>");
            }
        }

        public void SaveToCSV(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("Latitude,Longitude,Altitude,Timestamp");
                foreach (var point in GPSPoints)
                {
                    writer.WriteLine($"{point.Latitude},{point.Longitude},{point.Altitude},{point.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
        }

        internal void reset()
        {
            ClearTrace();
        }
    }
}
