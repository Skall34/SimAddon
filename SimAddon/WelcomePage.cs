using System;
using System.IO;
using System.Text;
using SimAddonLogger;

namespace SimAddon
{
    /// <summary>
    /// Page d'accueil HTML de SimAddon : génère %AppData%/SimAddon/welcome.html
    /// (thème SkyWings) avec les indications principales du moment, et l'ouvre
    /// dans le navigateur par défaut. Rafraîchie en arrière-plan si activée.
    /// </summary>
    public static class WelcomePage
    {
        public class Data
        {
            public string Callsign = "";
            public string AppVersion = "";
            public string SimSource = "";        // "SimBridge (UDP X-Plane)" ou "FSUIPC/XUIPC"
            public bool SimConnected;
            public bool BridgeActive;
            public long BridgeFrames;
            public string AircraftType = "";
            public string AircraftUiName = "";
            public string TailNumber = "";
            public double Lat, Lon, AltFt, IasKt;
            public bool OnGround;
            public double FuelKg, PayloadKg;
            public string SiteUrl = "https://skywings.ovh/";
            public string FlightState = "waiting"; // clé : waiting/taxiing/inflight/gliding/ended (FlightRecCtrl.FlightStateKey)
            public string GeneratedAt = "";
        }

        public static string FilePath
        {
            get
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SimAddon");
                Directory.CreateDirectory(dir);
                return Path.Combine(dir, "welcome.html");
            }
        }

        private static string Esc(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        /// <summary>Écrit le fichier HTML (sans ouvrir le navigateur).</summary>
        public static void Refresh(Data d)
        {
            try { File.WriteAllText(FilePath, BuildHtml(d), new UTF8Encoding(false)); }
            catch (Exception ex) { Logger.WriteLine("WelcomePage.Refresh: " + ex.Message); }
        }

        /// <summary>Écrit le fichier puis l'ouvre dans le navigateur par défaut.</summary>
        public static void Show(Data d)
        {
            Refresh(d);
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = FilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLine("WelcomePage.Show: " + ex.Message);
            }
        }

        private static string BuildHtml(Data d)
        {
            string cs = Esc(d.Callsign);
            string site = d.SiteUrl.TrimEnd('/');
            // Page des vols du pilote : skywings.ovh/pages/flights.php?callsign=CALLSIGN
            string volsUrl = site + "/pages/flights.php?callsign=" + Uri.EscapeDataString(d.Callsign ?? "");
            // Sans simulateur détecté : "Non connecté" (pas l'état résiduel du dernier vol)
            string fsKey = string.IsNullOrEmpty(d.FlightState) ? "waiting" : d.FlightState;
            string fsLabel, fsColor;
            if (!d.SimConnected)
            {
                fsLabel = "Non connecté"; fsColor = "#c62828";
            }
            else switch (fsKey)
            {
                case "inflight": fsLabel = "En vol"; fsColor = "#2e7d32"; break;
                case "taxiing": fsLabel = "Roulage"; fsColor = "#f9a825"; break;
                case "gliding": fsLabel = "Vol à voile"; fsColor = "#478ED4"; break;
                case "ended": fsLabel = "Vol terminé"; fsColor = "#6b7f99"; break;
                default: fsLabel = "Au parking"; fsColor = "#9e9e9e"; break;
            }
            string simColor = d.SimConnected ? "#2e7d32" : "#c62828";
            string simText = d.SimConnected ? "Connecté — " + Esc(d.SimSource) : "Simulateur non détecté";
            string bridgeText = d.BridgeActive
                ? "Plugin SimBridge actif (" + d.BridgeFrames + " trames reçues) — stall warning disponible"
                : "Plugin SimBridge non détecté (X-Plane 12 : installer SimBridge.xpl)";
            string plane = !string.IsNullOrEmpty(d.AircraftType) ? Esc(d.AircraftType) : "Non détecté";
            if (!string.IsNullOrEmpty(d.TailNumber)) plane += " · " + Esc(d.TailNumber);
            string pos = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0:0.000000} / {1:0.000000} — alt {2:0} ft — IAS {3:0} kt — {4}",
                d.Lat, d.Lon, d.AltFt, d.IasKt, d.OnGround ? "au sol" : "en vol");

            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html lang='fr'><head><meta charset='utf-8'>");
            sb.Append("<meta name='viewport' content='width=device-width, initial-scale=1'>");
            sb.Append("<meta http-equiv='refresh' content='30'>");
            sb.Append("<title>SkyWings · SimAddon · ").Append(cs).Append("</title><style>");
            sb.Append(@"
:root{--blue:#004080;--blue2:#478ED4;--bg:#f4f6f8;--card:#ffffff;--hover:#e6f0ff;--green:#2e7d32;--red:#c62828;}
*{box-sizing:border-box;margin:0;padding:0}
body{font-family:'Segoe UI',Arial,sans-serif;background:var(--bg);color:var(--blue);padding:24px;}
header{background:var(--blue);color:#fff;border-radius:12px;padding:20px 28px;display:flex;align-items:center;justify-content:space-between;flex-wrap:wrap;gap:12px;box-shadow:0 2px 8px rgba(0,0,0,.15);}
header h1{font-size:22px;font-weight:600;letter-spacing:.5px;}
header h1 span{color:#9ec3ff;font-weight:400;}
.badge{background:var(--blue2);color:#fff;font-size:18px;font-weight:700;padding:6px 16px;border-radius:999px;}
.grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(240px,1fr));gap:16px;margin-top:20px;}
.card{background:var(--card);border-radius:12px;padding:16px 18px;box-shadow:0 1px 4px rgba(0,64,128,.12);border-top:3px solid var(--blue2);}
.card h2{font-size:12px;text-transform:uppercase;letter-spacing:1px;color:#6b7f99;margin-bottom:8px;}
.card .val{font-size:17px;font-weight:600;color:#1a2b3c;word-break:break-word;}
.card .sub{font-size:13px;color:#5a6b7d;margin-top:4px;}
.dot{display:inline-block;width:10px;height:10px;border-radius:50%;margin-right:6px;vertical-align:middle;}
.links{display:flex;flex-wrap:wrap;gap:12px;margin-top:20px;}
.links a{background:var(--blue2);color:#fff;text-decoration:none;padding:10px 18px;border-radius:8px;font-weight:600;font-size:14px;transition:background .2s;}
.links a:hover{background:var(--blue);}
.links a.alt{background:#fff;color:var(--blue);border:1px solid var(--blue2);}
.links a.alt:hover{background:var(--hover);}
footer{margin-top:24px;font-size:12px;color:#7a8aa0;text-align:center;}
");
            sb.Append("</style></head><body>");
            sb.Append("<header><h1>SkyWings <span>· SimAddon</span></h1>");
            if (!string.IsNullOrEmpty(cs)) sb.Append("<div class='badge'>").Append(cs).Append("</div>");
            sb.Append("</header>");

            sb.Append("<div class='grid'>");
            sb.Append("<div class='card'><h2>Simulateur</h2><div class='val'><span class='dot' style='background:").Append(simColor).Append("'></span>").Append(simText).Append("</div>");
            sb.Append("<div class='sub'>SimAddon v").Append(Esc(d.AppVersion)).Append("</div></div>");

            sb.Append("<div class='card'><h2>Statut du vol</h2><div class='val'><span class='dot' style='background:").Append(fsColor).Append("'></span>").Append(Esc(fsLabel)).Append("</div></div>");

            sb.Append("<div class='card'><h2>Avion (SIM)</h2><div class='val'>").Append(plane).Append("</div>");
            if (!string.IsNullOrEmpty(d.AircraftUiName) && d.AircraftUiName != d.AircraftType)
                sb.Append("<div class='sub'>UI : ").Append(Esc(d.AircraftUiName)).Append("</div>");
            sb.Append("</div>");

            sb.Append("<div class='card'><h2>Position</h2><div class='val'>").Append(Esc(pos)).Append("</div></div>");

            sb.Append("<div class='card'><h2>Carburant / Payload</h2><div class='val'>")
              .Append(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0}", d.FuelKg)).Append(" kg fuel · ")
              .Append(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.0}", d.PayloadKg)).Append(" kg payload</div></div>");

            sb.Append("<div class='card'><h2>Pont SimBridge (X-Plane 12)</h2><div class='val' style='font-size:14px'>")
              .Append(Esc(bridgeText)).Append("</div></div>");
            sb.Append("</div>");

            sb.Append("<div class='links'>");
            if (!string.IsNullOrEmpty(cs)) sb.Append("<a target='_blank' rel='noopener' href='").Append(volsUrl).Append("'>Mes vols · ").Append(cs).Append("</a>");
            sb.Append("<a target='_blank' rel='noopener' href='").Append(site).Append("' class='alt'>Site SkyWings</a>");
            sb.Append("<a target='_blank' rel='noopener' href='").Append(site).Append("/pages/documentation.php' class='alt'>Documentation</a>");
            sb.Append("</div>");

            sb.Append("<footer>SimAddon · page générée à ").Append(Esc(d.GeneratedAt)).Append(" · se rafraîchit toutes les 30 s</footer>");
            sb.Append("</body></html>");
            return sb.ToString();
        }
    }
}
