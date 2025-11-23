using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATISPlugin
{
    public class ATCInfo
    {
        public string name { get; set; }
        public string tag { get; set; }
    }

    public class genericATC
    {
        public virtual async Task<bool> refresh(string url)
        {
            return false;
        }

        public virtual List<ATCInfo> FindATISList()
        {
            return new List<ATCInfo>();
        }

        public virtual async Task<List<string>> GetATISText(ATCInfo info, string url = "")
        {
           return new List<string>();
        }

        public virtual Image GetNetworkImage()
        {
            return null;
        }
    }
}
