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
        public static async Task<Reservation> CheckReservation(string callsign, string baseUrl, CancellationToken cancellationToken = default)
        {
            var reservation = new Reservation { Reserved = false };
            reservation.Reserved = false;
            try
            {
                // Try to get callsign from FlightRecorder plugin Settings or control
                if (string.IsNullOrWhiteSpace(callsign))
                {
                    Logger.WriteLine("CheckReservation: no callsign configured, skipping.");
                    return reservation;
                }

                string apiUrl = baseUrl.TrimEnd('/') + "/api/api_check_reservation.php?callsign=" + Uri.EscapeDataString(callsign);

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
                    Logger.WriteLine("CheckReservation: no reservation for " + callsign);
                    return reservation;
                }

                //        string popup = $"Réservation trouvée pour {callsign}";
                //        if (!string.IsNullOrWhiteSpace(message)) popup += Environment.NewLine + message;
                //        if (!string.IsNullOrWhiteSpace(immat)) popup += Environment.NewLine + "Immatriculation réservée: " + immat;
                //        if (!string.IsNullOrWhiteSpace(departure)) popup += Environment.NewLine + "Aéroport départ : " + departure;
                //        if (!string.IsNullOrWhiteSpace(arrival)) popup += Environment.NewLine + "Aéroport arrivée : " + arrival;
                //        popup += Environment.NewLine + Environment.NewLine + "Charger la réservation ?";

                //        var answer = Plugin_OnShowMsgbox(this, popup, "Réservation détectée", MessageBoxButtons.YesNo);
                //        if (answer != DialogResult.Yes)
                //        {
                //            Logger.WriteLine("CheckReservation: user declined to load reservation.");
                //            return;
                //        }

                //        // apply reservation: try FlightRecorder.ApplyReservation
                //        var fr = plugsMgr.plugins.FirstOrDefault(p => string.Equals(p.getName(), "FlightRecorder", StringComparison.OrdinalIgnoreCase));
                //        if (fr == null)
                //        {
                //            Logger.WriteLine("CheckReservation: FlightRecorder plugin not found.");
                //            return;
                //        }

                //        // Check current airport and compare with reservation
                //        if (!string.IsNullOrWhiteSpace(departure))
                //        {
                //            try
                //            {
                //                var pos = _simData.GetPosition();
                //                if (pos != null && pos.Location != null && _simData.aeroports != null)
                //                {
                //                    var closestDeparture = Aeroport.FindClosestAirport(_simData.aeroports, pos.Location.Latitude, pos.Location.Longitude);
                //                    if (closestDeparture != null)
                //                    {
                //                        Logger.WriteLine("CheckReservation: closest departure airport=" + closestDeparture.ident);
                //                        if (!string.Equals(closestDeparture.ident, departure, StringComparison.OrdinalIgnoreCase))
                //                        {
                //                            Plugin_OnShowMsgbox(this, "L'aéroport de départ actuel (" + closestDeparture.ident + ") ne correspond pas à la réservation (" + departure + ").", "Avertissement de réservation", MessageBoxButtons.OK);
                //                            return;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        Logger.WriteLine("CheckReservation: no closest airport found to compare departure");
                //                        Plugin_OnShowMsgbox(this, "Impossible de déterminer l'aéroport actuel pour vérifier la réservation.", "Avertissement de réservation", MessageBoxButtons.OK);
                //                        return;
                //                    }
                //                }
                //                else
                //                {
                //                    Logger.WriteLine("CheckReservation: position or airport database not available");
                //                    Plugin_OnShowMsgbox(this, "Impossible de déterminer l'aéroport actuel pour vérifier la réservation.", "Avertissement de réservation", MessageBoxButtons.OK);
                //                    return;
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                Logger.WriteLine("CheckReservation: error while checking current airport: " + ex.Message);
                //                Plugin_OnShowMsgbox(this, "Erreur lors de la vérification de l'aéroport actuel.", "Avertissement de réservation", MessageBoxButtons.OK);
                //                return;
                //            }
                //        }

                //        // If arrival ICAO is available, pass it to ApplyReservation; otherwise, fallback to reflection
                //        var method = fr.GetType().GetMethod("ApplyReservation", BindingFlags.Instance | BindingFlags.Public);
                //        if (method != null)
                //        {
                //            try
                //            {
                //                // With arrival ICAO
                //                if (!string.IsNullOrWhiteSpace(arrival))
                //                {
                //                    method.Invoke(fr, new object[] { immat ?? string.Empty, departure ?? string.Empty, arrival ?? string.Empty });
                //                    Logger.WriteLine("CheckReservation: applied reservation via ApplyReservation (with arrival ICAO).");
                //                }
                //                else
                //                {
                //                    method.Invoke(fr, new object[] { immat ?? string.Empty, departure ?? string.Empty });
                //                    Logger.WriteLine("CheckReservation: applied reservation via ApplyReservation (fallback).");
                //                }
                //            }
                //            catch (Exception ex)
                //            {
                //                Logger.WriteLine("CheckReservation: ApplyReservation failed: " + ex.Message);
                //            }
                //        }
                //        else
                //        {
                //            // Method not found, try fallback reflection to set controls on plugin
                //            try
                //            {
                //                // Set immatriculation
                //                var cbField = fr.GetType().GetField("cbImmat", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                //                if (cbField != null)
                //                {
                //                    var cb = cbField.GetValue(fr) as ComboBox;
                //                    if (cb != null)
                //                    {
                //                        cb.BeginInvoke(new Action(() =>
                //                        {
                //                            if (!string.IsNullOrWhiteSpace(immat))
                //                            {
                //                                var match = cb.Items.Cast<object>().FirstOrDefault(it =>
                //                                {
                //                                    if (it == null) return false;
                //                                    var t = it.GetType();
                //                                    var imProp = t.GetProperty("Immat");
                //                                    if (imProp != null)
                //                                    {
                //                                        var val = imProp.GetValue(it)?.ToString();
                //                                        return string.Equals(val, immat, StringComparison.OrdinalIgnoreCase);
                //                                    }
                //                                    return string.Equals(it.ToString(), immat, StringComparison.OrdinalIgnoreCase);
                //                                });

                //                                if (match != null) cb.SelectedItem = match;
                //                                else cb.Text = immat;

                //                                cb.Enabled = false;
                //                            }
                //                        }));
                //                    }
                //                }

                //                // Set departure label if exists
                //                var lbField = fr.GetType().GetField("lbStartIata", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                //                if (lbField != null)
                //                {
                //                    var lb = lbField.GetValue(fr) as Label;
                //                    if (lb != null)
                //                    {
                //                        lb.BeginInvoke(new Action(() => lb.Text = (departure ?? string.Empty).ToUpper()));
                //                    }
                //                }

                //                // Set arrival in tbEndICAO if exists
                //                var tbEndField = fr.GetType().GetField("tbEndICAO", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                //                if (tbEndField != null)
                //                {
                //                    var tbEnd = tbEndField.GetValue(fr) as TextBox;
                //                    if (tbEnd != null)
                //                    {
                //                        tbEnd.BeginInvoke(new Action(() => tbEnd.Text = (arrival ?? string.Empty).ToUpper()));
                //                    }
                //                }

                //                // Set comment
                //                var tbCommentsField = fr.GetType().GetField("tbCommentaires", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                //                if (tbCommentsField != null)
                //                {
                //                    var tbC = tbCommentsField.GetValue(fr) as TextBox;
                //                    if (tbC != null)
                //                    {
                //                        tbC.BeginInvoke(new Action(() => tbC.Text = "vol régulier"));
                //                    }
                //                }

                //                Logger.WriteLine("CheckReservation: applied reservation by reflection fallback.");
                //            }
                //            catch (Exception ex)
                //            {
                //                Logger.WriteLine("CheckReservation: fallback reflection failed: " + ex.Message);
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Logger.WriteLine("CheckReservation exception: " + ex.Message);
                //    }
                //}
            }
            catch (OperationCanceledException)
            {
                Logger.WriteLine("CheckReservation cancelled.");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("CheckReservation exception: " + ex.Message);
            }
            return reservation;
        }

        internal static async Task CompleteReservation(string callsign, Reservation reservation, string BASERURL, CancellationToken cancellationToken = default)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "callsign", callsign },
                    { "immat", reservation.Immat },
                    { "departureIcao", reservation.DepartureIcao },
                    { "arrivalIcao", reservation.ArrivalIcao }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await httpClient.PostAsync(BASERURL + "/api/api_complete_reservation.php", content, cancellationToken).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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

        internal static async Task ApplyReservation(string callsign, Reservation reservation, string BASEURL, CancellationToken cancellationToken = default)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "callsign", callsign },
                    { "immat", reservation.Immat },
                    { "departureIcao", reservation.DepartureIcao },
                    { "arrivalIcao", reservation.ArrivalIcao }
                };
                var content = new FormUrlEncodedContent(values);
                var response = await httpClient.PostAsync(BASEURL + "/api/api_consume_reservation.php", content, cancellationToken).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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
