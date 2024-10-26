using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimDataManager
{
    public static class Meteo
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static string DecodeMetar(string metar)
        {
            try
            {
                string[] parts = metar.Split(' ');
                if (parts.Length < 7) return "Format METAR invalide.";

                string decoded = "";

                int Index = 0;
                // Décodage du code de la station (premier élément)
                string station = parts[Index];
                decoded += $"Station: {station}" + Environment.NewLine;
                Index++;
                // Décodage de la date et de l'heure
                string datetime = parts[Index];
                string day = datetime.Substring(0, 2);
                string hour = datetime.Substring(2, 2);
                string minute = datetime.Substring(4, 2);
                decoded += $"Date/Heure: jour : {day} à {hour}:{minute} UTC" + Environment.NewLine;
                Index++;

                if (parts[Index]=="AUTO")
                {
                    Index++;
                }

                // Décodage du vent
                string wind = parts[Index];
                string windDirection = wind.Substring(0, 3);
                string windSpeed = wind.Substring(3, 2);
                string gusts = wind.Contains("G") ? $" avec des rafales jusqu'à {wind.Substring(wind.IndexOf('G') + 1, 2)} nœuds" : "";
                decoded += $"Vent: {windDirection}° à {windSpeed} nœuds{gusts}" + Environment.NewLine;
                Index++;

                // Décodage de la visibilité
                string visibility = parts[Index];
                decoded += $"Visibilité: {visibility.Replace("SM", " miles statutaires")}" + Environment.NewLine;
                Index++;

                // Décodage des nuages
                decoded += "Couverture nuageuse:\n";
                int counter = 0;
                bool foundClouds = false;
                for (int i = Index; i < parts.Length && parts[i].Length >= 5 && (parts[i].StartsWith("FEW") || parts[i].StartsWith("SCT") || parts[i].StartsWith("BKN") || parts[i].StartsWith("OVC")); i++)
                {
                    string cloud = parts[i];
                    string type = cloud.Substring(0, 3);
                    string altitude = cloud.Substring(3) + "00 pieds";
                    string description = type switch
                    {
                        "FEW" => "Peu de nuages",
                        "SCT" => "Nuages épars",
                        "BKN" => "Nuages fragmentés",
                        "OVC" => "Ciel couvert",
                        _ => type
                    };
                    decoded += $"- {description} à {altitude}" + Environment.NewLine;
                    counter++;
                    foundClouds = true;
                }
                if (!foundClouds && parts[Index]=="CLR")
                {
                    decoded += "Ciel clair";
                    Index++;
                }

                Index += counter;
                // Décodage de la température et du point de rosée
                string tempdp = parts[Index];
                string[] temps = tempdp.Split('/');
                decoded += $"Température: {temps[0]}°C, Point de rosée: {temps[1]}°C" + Environment.NewLine;
                Index++;

                // Décodage de l'altimètre
                string nextField = parts[Index];
                if (nextField.StartsWith("A"))
                {
                    decoded += $"Altimètre: {nextField.Substring(1).Insert(2,".")} pouces de mercure" + Environment.NewLine;
                    Index++;
                }
                if (nextField.StartsWith("Q"))
                {
                    decoded += $"Altimètre: {nextField.Substring(1)} hpa" + Environment.NewLine;
                    Index++;
                }

                nextField = parts[Index];

                return decoded;
            }
            catch (Exception ex)
            {
                return $"Erreur de décodage du METAR : {ex.Message}";
            }
        }

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
                string metar = responseBody;

                return metar;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du METAR : {ex.Message}");
                return null;
            }
        }
    }
}
