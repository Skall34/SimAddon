using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimDataManager
{
    public class FlyingNetwork
    {
        public const string VATSIM = "VATSIM";
        public const string IVAO = "IVAO";

        public const string RADARURL = "";
        public const string SEARCHURL = "";
        public enum NetworkType
        {
            VATSIM,
            IVAO
        }

        protected NetworkType _type;
        public string Name
        {
            get
            {
                return _type.ToString();
            }

            set
            {
                if (value.Equals(VATSIM, StringComparison.OrdinalIgnoreCase))
                {
                    _type = NetworkType.VATSIM;
                }
                else if (value.Equals(IVAO, StringComparison.OrdinalIgnoreCase))
                {
                    _type = NetworkType.IVAO;
                }
                else
                {
                    throw new ArgumentException("Invalid network name");
                }
            }
        }

        public virtual string GetRadarUrl()
        {
            //by default, return vatsim radar url
            return RADARURL;
        }

        public virtual string GetRadarSearchUrl(string ICAO)
        {
            //by default, return google maps url
            return string.Format("https://www.google.com/maps/search/{0}", ICAO);
        }

        public virtual string GetMETARUrl(string ICAO)
        {
            //by default, return aviation weather url
            return string.Format("https://aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&stationString={0}&hoursBeforeNow=2", ICAO);
        }

        //if sever sent back json, xml, etc, override this method to parse out raw metar text
        public virtual string GetRawMETARText(string serverData)
        {
            //by default, return aviation weather url
            return serverData;
        }

        public virtual string GetATISUrl(string ICAO)
        {
            //by default, return aviation weather url
            return string.Format("https://aviationweather.gov/adds/dataserver_current/httpparam?dataSource=metars&requestType=retrieve&format=xml&stationString={0}&hoursBeforeNow=2", ICAO);
        }
    }
}
