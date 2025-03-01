using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushTripPlugin
{
    internal class fms
    {
        public int versionNumber;
        public int airacCycle;
        public string departure;
        public string depRunway;
        public string SID;
        public string SIDTRANS;

        public string destination;
        public string destRunway;
        public string STAR;
        public string STARTRANS;
        public string approach;

        public int numenr;

        public class fmsWpt
        {
            public enum WPTYPE
            {
                AIRPORT=1,
                NDB=2,
                VOR=3,
                NAMED=11,
                UNNAMED=28
            }
            public WPTYPE wpType;
            public string name;
            public string special;
            public float altitude;
            public double latitude;
            public double longitude;
        }

        public List<fmsWpt> waypoints;

        public fms(string rawText)
        {
            waypoints = new List<fmsWpt>();

            string[] lines = rawText.Split(Environment.NewLine);
            if (lines.Length > 0)
            {
                if (lines[0]=="I")
                {
                    //first read the version
                    string[] versionItems = lines[1].Split(' ');
                    if (versionItems.Length == 2)
                    {
                        versionNumber = int.Parse(versionItems[0]);
                    }
                    else
                    {
                        throw new Exception("Invalid file format line 1");
                    }

                    //then read the cycle
                    string[] airac = lines[2].Split(' ');
                    if (airac.Length == 2)
                    {
                        airacCycle = int.Parse(airac[1]);
                    }
                    else
                    {
                        throw new Exception("Invalid file format line 2");
                    }

                    //read departure block
                    int numline = 3;
                    string[] elements = lines[numline].Split(' ');
                    while (elements[0]!="NUMENR")
                    {
                        if (elements[0]=="ADEP")
                        {
                            departure = elements[1];
                        }
                        if (elements[0] == "DEP")
                        {
                            departure = elements[1];
                        }
                        if (elements[0] == "DEPRWY")
                        {
                            depRunway = elements[1];
                        }
                        if (elements[0] == "SID")
                        {
                            SID = elements[1];
                        }
                        if (elements[0] == "SIDTRANS")
                        {
                            SIDTRANS = elements[1];
                        }

                        if (elements[0] == "ADES")
                        {
                            destination = elements[1];
                        }
                        if (elements[0] == "DES")
                        {
                            destination = elements[1];
                        }
                        if (elements[0] == "DESRWY")
                        {
                            destRunway = elements[1];
                        }
                        if (elements[0] == "STAR")
                        {
                            STAR = elements[1];
                        }
                        if (elements[0] == "STARTRANS")
                        {
                            STARTRANS = elements[1];
                        }
                        if (elements[0] == "APP")
                        {
                            approach = elements[1];
                        }

                        numline++;
                        elements = lines[numline].Split(' ');
                    }

                    //next element must be the number of waypoints in the flightplan
                    numenr = int.Parse(elements[1]);

                    numline++;
                    for (int i=0;i<numenr;i++)
                    {
                        string enr = lines[i+numline];
                        string[]wpItems = enr.Split(" ");
                        try
                        {
                            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;
                            fmsWpt wpt = new fmsWpt()
                            {
                                wpType = (fmsWpt.WPTYPE)int.Parse(wpItems[0]),
                                name = wpItems[1],
                                special = wpItems[2],
                                altitude = float.Parse(wpItems[3], formatProvider),
                                latitude = float.Parse(wpItems[4], formatProvider),
                                longitude = float.Parse(wpItems[5], formatProvider)
                            };
                            waypoints.Add(wpt);
                        }
                        catch (Exception ex) { 
                            Logger.WriteLine(ex.ToString());
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid file format line 0");
                }
            }
            else
            {
                throw new Exception("No lines in file");
            }
        }
    }
}
