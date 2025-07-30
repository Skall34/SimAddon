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
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Alt { get; set; }
        public DateTime Time { get; set; }
    }

    public class GPSRecorder
    {
        // create a palette of 16 colors for altitude visualization
        //from blue to red, with some variations    
        // The colors are chosen to represent a gradient from low to high altitude
        // The colors are represented in hexadecimal format
        public static readonly string[] AltitudeColors = new string[]
        {
            "#0000FF", // Blue
            "#0033FF", // Light Blue
            "#0066FF", // Sky Blue
            "#0099FF", // Cyan
            "#00CCFF", // Light Cyan
            "#00FFFF", // Aqua
            "#33FFCC", // Light Greenish Cyan
            "#66FF99", // Light Green
            "#99FF66", // Yellowish Green
            "#CCFF33", // Yellow
            "#FFFF00", // Yellow
            "#FFCC00", // Orange Yellow
            "#FF9900", // Orange
            "#FF6600", // Dark Orange
            "#FF3300", // Red Orange
            "#FF0000"  // Red
        };

        //add a point to the GPS trace, with latitude, longitude, altitude, and timestamp
        public List<GPSPoint> GPSPoints { get; private set; } = new List<GPSPoint>();

        private double minAltitude = double.MaxValue;
        private double maxAltitude = double.MinValue;
        public void AddPoint(double latitude, double longitude, double altitude, DateTime timestamp)
        {
            GPSPoints.Add(new GPSPoint
            {
                Lat = latitude,
                Long = longitude,
                Alt = altitude,
                Time = timestamp
            });
            // Update min and max altitude
            if (altitude < minAltitude)
            {
                minAltitude = altitude;
            }
            if (altitude > maxAltitude)
            {
                maxAltitude = altitude;
            }
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
            //GPSPoints = GPSPoints
            //    .Where((point, index) => index == 0 || 
            //            (Math.Abs(point.Lat - GPSPoints[index - 1].Lat) > 0.000000001 ||
            //             Math.Abs(point.Long - GPSPoints[index - 1].Long) > 0.000000001))
            //    .ToList();

            //remove intermediate points that are aligned with the previous and next points
            for (int i = 1; i < GPSPoints.Count - 1; i++)
            {
                var prev = GPSPoints[i - 1];
                var current = GPSPoints[i];
                var next = GPSPoints[i + 1];
                if (Math.Abs((current.Lat - prev.Lat) * (next.Long - current.Long) -
                             (current.Long - prev.Long) * (next.Lat - current.Lat)) < 0.0000001)
                {
                    GPSPoints.RemoveAt(i);
                    i--; // Adjust index after removal
                }
            }
        }

        //optimize the trace to reduce the number of points using Ramer-Douglas-Peucker algorithm
        //implement ramer douglas -peucker algorithm
        public void OptimizeTraceRamerDouglasPeucker(double epsilon)
        {
            if (GPSPoints.Count < 3)
                return; // No optimization needed for less than 3 points
            GPSPoints = RamerDouglasPeucker(GPSPoints, epsilon);
        }

        private List<GPSPoint> RamerDouglasPeucker(List<GPSPoint> gPSPoints, double epsilon)
        {
            if (gPSPoints.Count < 3)
                return gPSPoints; // No optimization needed for less than 3 points
            int firstIndex = 0;
            int lastIndex = gPSPoints.Count - 1;
            // Find the point with the maximum distance from the line segment
            double maxDistance = 0.0;
            int index = -1;
            for (int i = firstIndex + 1; i < lastIndex; i++)
            {
                double distance = PerpendicularDistance(gPSPoints[firstIndex], gPSPoints[lastIndex], gPSPoints[i]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    index = i;
                }
            }
            // If the maximum distance is greater than epsilon, recursively simplify
            if (maxDistance > epsilon)
            {
                List<GPSPoint> left = RamerDouglasPeucker(gPSPoints.GetRange(firstIndex, index - firstIndex + 1), epsilon);
                List<GPSPoint> right = RamerDouglasPeucker(gPSPoints.GetRange(index, lastIndex - index + 1), epsilon);
                // Combine the results
                List<GPSPoint> result = new List<GPSPoint>();
                result.AddRange(left);
                result.AddRange(right.Skip(1)); // Skip the first point of right to avoid duplication
                return result;
            }
            else
            {
                // If the maximum distance is less than epsilon, return the endpoints
                return new List<GPSPoint> { gPSPoints[firstIndex], gPSPoints[lastIndex] };
            }
        }

        private double PerpendicularDistance(GPSPoint gPSPoint1, GPSPoint gPSPoint2, GPSPoint gPSPoint3)
        {
            double A = gPSPoint2.Lat - gPSPoint1.Lat;
            double B = gPSPoint1.Long - gPSPoint2.Long;
            double C = A * gPSPoint1.Long + B * gPSPoint1.Lat;
            // Calculate the distance from point 3 to the line defined by points 1 and 2
            return Math.Abs(A * gPSPoint3.Long + B * gPSPoint3.Lat - C) / Math.Sqrt(A * A + B * B);
        }


        // Normalize altitude to a range of 0 to 16
        int NormalizeAltitude(double altitude)
        {
            double deltaAltitude = maxAltitude - minAltitude;
            if (deltaAltitude < 0.0001)
            {
                deltaAltitude = 0.0001; // Avoid division by zero
            }
            return (int)(16 * (altitude - minAltitude) / deltaAltitude);
        }

        public void SaveToKML(string filePath)
        {

            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
                writer.WriteLine("<Document>");
                //create the styles
                for (int i = 0; i < AltitudeColors.Length; i++)
                {
                    writer.WriteLine($"<Style id=\"altitude{i}\">");
                    writer.WriteLine("<LineStyle>");
                    writer.WriteLine($"<color>{AltitudeColors[i].Substring(1)}</color>"); // Remove the '#' character
                    writer.WriteLine("<width>6</width>");
                    writer.WriteLine("</LineStyle>");
                    writer.WriteLine("</Style>");
                }

                //draw segments between points
                for (int i = 0; i < GPSPoints.Count - 1; i++)
                {
                    var start = GPSPoints[i];
                    var end = GPSPoints[i + 1];
                    writer.WriteLine("<Placemark>");
                    // Use the normalized altitude to select the color
                    int normalizedAltitude = NormalizeAltitude(start.Alt);
                    writer.WriteLine($"<styleUrl>#altitude{normalizedAltitude}</styleUrl>");

                    // Use the timestamp for the name
                    writer.WriteLine($"<name>{start.Time.ToString("yyyy-MM-dd HH:mm:ss:fff")} to {end.Time.ToString("yyyy-MM-dd HH:mm:ss:fff")}</name>");
                    writer.WriteLine("<LineString>");
                    writer.WriteLine("<coordinates>");
                    writer.WriteLine($"{start.Long.ToString(CultureInfo.InvariantCulture)},{start.Lat.ToString(CultureInfo.InvariantCulture)},{start.Alt.ToString(CultureInfo.InvariantCulture)} ");
                    writer.WriteLine($"{end.Long.ToString(CultureInfo.InvariantCulture)},{end.Lat.ToString(CultureInfo.InvariantCulture)},{end.Alt.ToString(CultureInfo.InvariantCulture)}");
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
                    writer.WriteLine("<wpt lat=\"" + point.Lat + "\" lon=\"" + point.Long + "\">");
                    writer.WriteLine($"<ele>{point.Alt}</ele>");
                    writer.WriteLine($"<time>{point.Time.ToString("yyyy-MM-ddTHH:mm:ssZ")}</time>");
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
                    writer.WriteLine($"{point.Lat},{point.Long},{point.Alt},{point.Time.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
        }

        public void SaveToJSON(string filePath)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(GPSPoints);
            System.IO.File.WriteAllText(filePath, json);
        }

        public string GetTraceJSON()
        {
            return System.Text.Json.JsonSerializer.Serialize(GPSPoints);
        }

        internal void reset()
        {
            ClearTrace();
        }
    }
}
