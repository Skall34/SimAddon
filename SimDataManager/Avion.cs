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
        public PlaneStatus Status { get; set; }
        public string Horametre { get; set; }
        public string DernierUtilisateur { get; set; }
        public int EnVol {  get; set; }
        public int Reserved { get; set; }

        //public const int StatusDisponible = 0;
        //public const int StatusMaintenance = 1;
        //public const int StatusMaintenance2 = 2;
        //public const int StatusReserve = 3; // pour réservé

        public enum PlaneStatus
        {
            Disponible = 0, //soit dispo
            Maintenance = 1, //soit en maintenance
            Maintenance2 = 2, //soit en maintenance longue
            Reserved = 3 //soit déja en vol, soit réservé par qqun
        }

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

            foreach(Avion avion in avions)
            {
                avion.Status = PlaneStatus.Disponible; // Par défaut, l'avion est disponible

                if ((avion.EnVol != 0)||(avion.Reserved!=0))
                {
                    avion.Status = PlaneStatus.Reserved; // Indiquer que l'avion est en vol
                }
                if (avion.Etat == 1)
                {
                    avion.Status = PlaneStatus.Maintenance; // Indiquer que l'avion est en maintenance
                }
                if (avion.Etat == 2)
                {
                    avion.Status = PlaneStatus.Maintenance2; // Indiquer que l'avion est en maintenance longue ?
                }
            }
            return avions;
        }

        public bool IsSelectable(string currentCallsign)
        {
            if (Status == Avion.PlaneStatus.Disponible) return true;
            if ((Status == Avion.PlaneStatus.Reserved) && (DernierUtilisateur == currentCallsign)) return true;
            return false;
        }
    }
}
