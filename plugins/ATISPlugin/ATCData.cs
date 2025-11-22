using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATISPlugin
{
    public class genericATC
    {
        public virtual async Task<bool> refresh(string url)
        {
            return false;
        }

        public virtual List<string> FindATISInRange(double targetLatitude, double targetLongitude, uint range)
        {
            return new List<string>();
        }

        public virtual async Task<List<string>> GetATIS(string ICAO, string url = "")
        {
           return new List<string>();
        }

        public virtual Image GetNetworkImage()
        {
            return null;
        }
    }
}
