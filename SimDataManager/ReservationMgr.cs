using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SimDataManager
{
    public class Reservation
    {
        public bool Reserved { get; set; }
        public string Immat { get; set; } = string.Empty;
        public string DepartureIcao { get; set; } = string.Empty;
        public string ArrivalIcao { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool checkedOnce { get; set; } = false;
    }

    public static class ReservationMgr
    {
        public enum ReservationStatus
        {
            Ignored,
            Accepted,
            Unknown
        }

        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Call the reservation API using Settings.Default.callsign and show a popup if a reservation exists.
        /// </summary>
        internal static async Task<Reservation> CheckReservation(string callsign, string baseUrl,string token, CancellationToken cancellationToken = default)
        {
            var reservation = new Reservation { Reserved = false };
            reservation.Reserved = false;
            reservation.checkedOnce = true;
            try
            {
                // Try to get callsign from FlightRecorder plugin Settings or control
                if (string.IsNullOrWhiteSpace(callsign))
                {
                    Logger.WriteLine("CheckReservation: no callsign configured, skipping.");
                    return reservation;
                }

                string apiUrl = baseUrl.TrimEnd('/') + "/api/api_check_reservation.php?callsign=" + Uri.EscapeDataString(callsign);
                apiUrl += "&session_token=" + Uri.EscapeDataString(token);

                HttpResponseMessage resp = await httpClient.GetAsync(apiUrl, cancellationToken).ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                {
                    Logger.WriteLine($"CheckReservation: HTTP {(int)resp.StatusCode}");
                    return reservation;
                }

                string body = await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(body))
                {
                    Logger.WriteLine("CheckReservation: empty response");
                    return reservation;
                }


                // parse JSON: { status:'ok', reserved:true, reservation: {...} }
                try
                {
                    using JsonDocument doc = JsonDocument.Parse(body);
                    var root = doc.RootElement;
                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("reserved", out var r))
                        {
                            if (r.ValueKind == JsonValueKind.True) reservation.Reserved = true;
                            else if (r.ValueKind == JsonValueKind.False) reservation.Reserved = false;
                            else if (r.ValueKind == JsonValueKind.Number) reservation.Reserved = r.GetInt32() != 0;
                        }

                        if (root.TryGetProperty("reservation", out var res) && res.ValueKind == JsonValueKind.Object)
                        {
                            var jsonResa = res;
                            if (jsonResa.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String) reservation.Message = m.GetString() ?? string.Empty;
                            if (jsonResa.TryGetProperty("immat", out var im) && im.ValueKind == JsonValueKind.String) reservation.Immat = im.GetString() ?? string.Empty;
                            if (jsonResa.TryGetProperty("icao_dep", out var id) && id.ValueKind == JsonValueKind.String) reservation.DepartureIcao = id.GetString() ?? string.Empty;
                            if (jsonResa.TryGetProperty("icao_arr", out var ia) && ia.ValueKind == JsonValueKind.String) reservation.ArrivalIcao = ia.GetString() ?? string.Empty;
                        }
                        else
                        {
                            // fallback: maybe message at top
                            if (root.TryGetProperty("message", out var m2) && m2.ValueKind == JsonValueKind.String) reservation.Message = m2.GetString() ?? string.Empty;
                        }
                    }
                    else
                    {
                        reservation.Reserved = !string.IsNullOrWhiteSpace(body);
                        reservation.Message = body.Trim();
                    }
                }
                catch (JsonException)
                {
                    reservation.Reserved = !string.IsNullOrWhiteSpace(body);
                    reservation.Message = body.Trim();
                }

                if (!reservation.Reserved)
                {
                    return reservation;
                }
            }
            catch (OperationCanceledException)
            {
                Logger.WriteLine("CheckReservation cancelled.");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("CheckReservation exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Logger.WriteLine(" CheckReservation inner exception: " + ex.InnerException.Message);
                }
            }
            return reservation;
        }

        /// <summary>
        /// Tell the server that the reservation has been used.
        internal static async Task CompleteReservation(string callsign, Reservation reservation, string BASEURL, string sessionToken)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "callsign", callsign },
                    { "immat", reservation.Immat },
                    { "departureIcao", reservation.DepartureIcao },
                    { "arrivalIcao", reservation.ArrivalIcao },
                    { "session_token", sessionToken   }
                };

                var content = new FormUrlEncodedContent(values);
                string URL = BASEURL + "/api/api_complete_reservation.php";
                var response = await httpClient.PostAsync(BASEURL + "/api/api_complete_reservation.php", content, CancellationToken.None).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Logger.WriteLine($"api_complete_reservation.php response: {responseString}");
            }
            catch (OperationCanceledException)
            {
                Logger.WriteLine("api_complete_reservation.php cancelled.");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"api_complete_reservation.php error: {ex.Message}");
            }
        }

        /// <summary>
        /// Confirm the reservation will be used.
        internal static async Task ApplyReservation(string callsign, Reservation reservation, string BASEURL, string sessionToken)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "callsign", callsign },
                    { "immat", reservation.Immat },
                    { "departureIcao", reservation.DepartureIcao },
                    { "arrivalIcao", reservation.ArrivalIcao },
                    { "session_token", sessionToken }
                };
                var content = new FormUrlEncodedContent(values);
                var response = await httpClient.PostAsync(BASEURL + "/api/api_consume_reservation.php", content).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Logger.WriteLine($"api_consume_reservation.php response: {responseString}");
            }
            catch (OperationCanceledException)
            {
                Logger.WriteLine("api_consume_reservation.php cancelled.");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"api_consume_reservation.php error: {ex.Message}");
            }
        }
    }
}
