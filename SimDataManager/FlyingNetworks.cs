using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimDataManager
{
    public class VATSIMFlyingNetwork : FlyingNetwork
    {
        
        public new const string RADARURL = "https://vatsim-radar.com/";
        public new const string SEARCHURL = "https://vatsim-radar.com/?airport={0}";
        public VATSIMFlyingNetwork()
        {
            _type = NetworkType.VATSIM;
        }

        public override string GetRadarUrl()
        {
            return RADARURL;
        }

        public override string GetRadarSearchUrl(string ICAO)
        {
            return string.Format(SEARCHURL, ICAO);
        }

        public override string GetMETARUrl(string ICAO)
        {
            return string.Format("https://metar.vatsim.net/{0}", ICAO);
        }

        public override string GetATISUrl(string ICAO)
        {
            return string.Format("https://api.vatsim.net/v2/weather/atis/{0}", ICAO);
        }
    }

    public class IVAOFlyingNetwork : FlyingNetwork
    {
        private string APIKEY = "7ULUTLCXVCPM6QERISNC80GKFSJOY5B9";
        public new const string RADARURL = "https://webeye.ivao.aero/";
        public new const string SEARCHURL = "https://webeye.ivao.aero/?airportId={0}";

        public IVAOFlyingNetwork()
        {
            _type = NetworkType.IVAO;
        }

        public override string GetRadarUrl()
        {
            return RADARURL;
        }

        public override string GetRadarSearchUrl(string ICAO)
        {
            return string.Format(SEARCHURL, ICAO);
        }

        public override string GetMETARUrl(string ICAO)
        {
            return string.Format("https://api.ivao.aero/v2/airports/{0}/metar?apiKey={1}", ICAO, APIKEY);
        }

        public override string GetRawMETARText(string serverData)
        {
            //IVAO returns json data, need to parse out raw metar text
            try
            {
                var json = System.Text.Json.JsonDocument.Parse(serverData);
                var root = json.RootElement;
                if (root.TryGetProperty("metar", out var metarElement))
                {
                    return metarElement.GetString();
                }
            }
            catch (Exception ex)
            {
                //handle parsing error
                Logger.WriteLine("Error parsing IVAO METAR JSON: " + ex.Message);
            }
            return string.Empty;
        }


        public override string GetATISUrl(string ICAO)
        {
            return string.Format("https://api.ivao.aero/v2/weather{0}/atis?apiKey={1}", ICAO, APIKEY);
        }
    }
}
