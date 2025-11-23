using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimAddonLogger;

namespace SimDataManager
{

    public static class NavigationHelper
    {
        private const double EarthRadiusMiles = 3440.1;  // Rayon moyen de la Terre en miles nautiques
        private const double EarthRadiusKm = 6371; // Rayon de la terre en kilometres

        private const double a = 6378137.0; // demi-grand axe de l'ellipsoïde en mètres
        private const double f = 1 / 298.257223563; // aplatissement de l'ellipsoïde
        private const double b = (1 - f) * a; // demi-petit axe de l'ellipsoïde
        
        //url pour récupérer la déclinaison magnétique locale en un point.
        private static readonly string BaseUrl = "https://www.ngdc.noaa.gov/geomag-web/calculators/calculateDeclination";
        //https://www.ngdc.noaa.gov/geomag-web/calculators/calculateDeclination?lat1=40&lon1=-105.25&key=zNEw7&resultFormat=xml
        //https://www.ngdc.noaa.gov/
        //https://www.ngdc.noaa.gov/geomag/CalcSurveyFin.shtml
        private static readonly string NOAAKEY_DECLINAISON = "zNEw7";
        private static readonly string NOAAKEY_HISTORY = "yKc9F";
        private static readonly string NOAAKEY_MAGFIELD = "EAU2y";
        private static readonly string NOAAKEY_MAGFIELD_COMPONENT = "gFE5W";


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

        private static double NormalizeLongitude(double lon)
        {
            // Normaliser la longitude pour qu'elle soit dans l'intervalle [-π, π]
            while (lon > Math.PI) lon -= 2 * Math.PI;
            while (lon < -Math.PI) lon += 2 * Math.PI;
            return lon;
        }

        public static  async Task<double> GetNavRouteAsync(double lat1, double lon1, double lat2, double lon2, double declinaison)
        {
            // Convertir les coordonnées en radians
            double radLat1 = DegreesToRadians(lat1);
            double radLon1 = DegreesToRadians(lon1);
            double radLat2 = DegreesToRadians(lat2);
            double radLon2 = DegreesToRadians(lon2);

            // Différence de longitude
            double L = radLon2 - radLon1;

            // Calcul de l'azimut en utilisant la formule de Vincenty
            double U1 = Math.Atan((1 - f) * Math.Tan(radLat1));
            double U2 = Math.Atan((1 - f) * Math.Tan(radLat2));
            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);

            double lambda = L;
            double lambdaP;
            double sinSigma, cosSigma, sigma, sinAlpha, cos2Alpha, cos2SigmaM, C;

            int iterationLimit = 100;
            do
            {
                double sinLambda = Math.Sin(lambda);
                double cosLambda = Math.Cos(lambda);
                sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) +
                                     (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) *
                                     (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (sinSigma == 0) return 0; // points coincidents

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);
                sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                cos2Alpha = 1 - sinAlpha * sinAlpha;
                cos2SigmaM = (cos2Alpha != 0) ? cosSigma - 2 * sinU1 * sinU2 / cos2Alpha : 0; // équateur

                C = f / 16 * cos2Alpha * (4 + f * (4 - 3 * cos2Alpha));
                lambdaP = lambda;
                lambda = L + (1 - C) * f * sinAlpha *
                         (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));

            } while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterationLimit > 0);

            if (iterationLimit == 0) throw new Exception("La formule de Vincenty ne converge pas.");

            // Calcul de l'azimut initial
            double azimut = Math.Atan2(cosU2 * Math.Sin(lambda), cosU1 * sinU2 - sinU1 * cosU2 * Math.Cos(lambda));

            // Convertir l'azimut en degrés
            azimut = RadiansToDegrees(azimut);

            //déduire la declinaison
            azimut -= declinaison;

            // Assurer que l'azimut est dans l'intervalle [0, 360]
            if (azimut < 0) azimut += 360;

            return Math.Floor(azimut);
        }

        public static async Task<double> GetApproxNavRouteAsync(double lat1, double lon1, double lat2, double lon2)
        {
            return await GetNavRouteAsync(lat1, lon1, lat2, lon2, 0);
        }

        public static async Task<double> GetMagneticDeclinaison(double latitude, double longitude, double altitude = 0, int year = 2025)
        {
            // Créer une instance HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Construire l'URL de la requête avec les paramètres
                string lat1 = latitude.ToString(NumberFormatInfo.InvariantInfo);
                string lon1 = longitude.ToString(NumberFormatInfo.InvariantInfo);

                string url = $"{BaseUrl}?lat1={lat1}&lon1={lon1}&key={NOAAKEY_DECLINAISON}&resultFormat=json&startYear={year}";

                try
                {
                    // Envoyer la requête GET à l'API
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Lire le contenu de la réponse
                        string content = await response.Content.ReadAsStringAsync();

                        // Analyser la réponse JSON
                        JObject json = JObject.Parse(content);

                        // Extraire la déclinaison magnétique en degrés
                        double declinaison = (double)json["result"][0]["declination"];

                        return declinaison;
                    }
                    else
                    {
                        Logger.WriteLine($"Erreur lors de la requête : {response.StatusCode}");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"Exception : {ex.Message}");
                    return 0;
                }
            }
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

        public static uint GetVHFRangeNauticalMiles(double altitudeFeet)
        {
            // Validate input
            if (altitudeFeet < 0)
            {
                altitudeFeet = 0;
            }

            // Calculate range in nautical miles
            return (uint)(50 + 1.065 * Math.Sqrt(altitudeFeet));
        }

    }

}
