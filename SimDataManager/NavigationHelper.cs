using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimDataManager
{

    public static class NavigationHelper
    {
        private const double EarthRadiusMiles = 3440.1;  // Rayon moyen de la Terre en miles nautiques
        private const double EarthRadiusKm = 6371; // Rayon de la terre en kilometres

        // Conversion de degrés en radians
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        // Conversion de radians en degrés
        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static double GetNavRoute(double lat1, double lon1, double lat2, double lon2)
        {
            // Conversion des degrés en radians
            lat1 = DegreesToRadians(lat1);
            lon1 = DegreesToRadians(lon1);
            lat2 = DegreesToRadians(lat2);
            lon2 = DegreesToRadians(lon2);

            // Différence des longitudes
            double deltaLon = lon2 - lon1;

            // Calcul de l'azimut
            double y = Math.Sin(deltaLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon);
            double initialBearing = Math.Atan2(y, x);

            // Conversion du cap en degrés
            double initialBearingDegrees = RadiansToDegrees(initialBearing);

            // Normalisation de l'angle pour qu'il soit entre 0 et 360
            return Math.Round((initialBearingDegrees + 360) % 360);
        }


        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Conversion des degrés en radians
            lat1 = DegreesToRadians(lat1);
            lon1 = DegreesToRadians(lon1);
            lat2 = DegreesToRadians(lat2);
            lon2 = DegreesToRadians(lon2);

            // Calcul des différences de latitude et longitude
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            // Formule de Haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calcul de la distance en miles
            return Math.Round(EarthRadiusMiles * c);
        }

    }

}
