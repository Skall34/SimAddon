using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimDataManager
{
    public class AvionsPhpList
    {
        public bool success;
        public List<Dictionary<string, string>> immats;
    }

    public class Avion : IEquatable<Avion>, IComparable<Avion>
    {
        public int Index { get; set; }
        public string ICAO { get; set; }
        public string Type { get; set; }
        public string Designation { get; set; }
        public string Immat { get; set; }
        public string Localisation { get; set; }
        public string Hub { get; set; }
        public int CoutHoraire { get; set; }
        public int Etat { get; set; }
        public int Status { get; set; }
        public string Horametre { get; set; }
        public string DernierUtilisateur { get; set; }
        public int EnVol {  get; set; }

        public const int StatusDisponible = 0;
        public const int StatusMaintenance = 1;
        public const int StatusMaintenance2 = 2;
        public const int StatusReserve = 3; // pour réservé

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Avion objAsAvion = obj as Avion;
            if (objAsAvion == null) return false;
            else return base.Equals(obj);
        }

        public bool Equals(Avion obj)
        {
            if (obj == null) return false;
            return (this.Immat.Equals(obj.Immat));
        }

        public int CompareTo(Avion other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;
            else
                if (this.Immat == null)
            {
                return -1;
            }
            else
            {
                return this.Immat.CompareTo(other.Immat);
            }
        }

        public int SortByNameAscending(string name1, string name2)
        {

            return name1.CompareTo(name2);
        }

        public static async Task<List<Avion>> FetchAvionsFromSheet(string BASEURL)
        {
            string url = BASEURL + "/api/api_getFlotte.php";
            UrlDeserializer dataReader = new UrlDeserializer(url);
            List<Avion> avions = await dataReader.FetchAvionsDataAsync();
            return avions;
        }

        public bool IsSelectable(string currentCallsign)
        {
            if (Status == StatusDisponible) return true;
            if (Status == StatusReserve && DernierUtilisateur == currentCallsign) return true;
            return false;
        }
    }
}
