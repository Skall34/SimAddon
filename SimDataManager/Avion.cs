using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public static async Task<List<Avion>> FetchAvionsFromSheet(HttpClient client, string BASEURL,string token)
        {
            bool success = false;
            int nbRetry = 0;
            string url = BASEURL + "/api/api_getFlotte.php?session_token=" + Uri.EscapeDataString(token);
            List<Avion> avions = null;
            // Retry logic to handle transient errors
            UrlDeserializer dataReader = new UrlDeserializer(client, url);
            while ((!success)&&(nbRetry<3)) { 
                try
                {
                    avions = await dataReader.FetchAvionsDataAsync();
                    if ((avions == null)||(avions.Count==0))
                    {
                        throw new Exception("Planes list is empty");
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la récupération des avions : " + ex.Message);
                    await Task.Delay(1000); // Attendre 2 secondes avant de réessayer
                    nbRetry++;
                }
            }

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

        //update the list of avions with their status (reserved, in flight, maintenance, available)
        public static async void UpdateAvionsStatus(HttpClient client, List<Avion> avions, string BASEURL, string token)
        {
            //fecth the list of planes from the database
            string url = BASEURL + "/api/api_getFlotte.php?session_token=" + Uri.EscapeDataString(token);
            UrlDeserializer dataReader = new UrlDeserializer(client, url);
            List<Avion> planesInDB = await dataReader.FetchAvionsDataAsync();

            foreach (Avion avion in avions)
            {
                //find the corresponding plane in the database
                Avion planeInDB = planesInDB.Find(p => p.Immat == avion.Immat);
                if (planeInDB != null)
                {
                    //copy the plane in DB properties to the current plane
                    avion.Etat = planeInDB.Etat;
                    avion.EnVol = planeInDB.EnVol;
                    avion.Reserved = planeInDB.Reserved;
                    //update the status
                    if ((avion.EnVol != 0) || (avion.Reserved != 0))
                    {
                        avion.Status = PlaneStatus.Reserved; // Indiquer que l'avion est en vol
                    }
                    else if (avion.Etat == 1)
                    {
                        avion.Status = PlaneStatus.Maintenance; // Indiquer que l'avion est en maintenance
                    }
                    else if (avion.Etat == 2)
                    {
                        avion.Status = PlaneStatus.Maintenance2; // Indiquer que l'avion est en maintenance longue ?
                    }
                    else
                    {
                        avion.Status = PlaneStatus.Disponible; // Par défaut, l'avion est disponible
                    }
                }
            }
        }

        public bool IsSelectable(string currentCallsign, ReservationMgr.ReservationStatus resaStatus)
        {
            // Un avion est sélectionnable s'il est disponible ou s'il est réservé par le currentCallsign avec une réservation acceptée
            if (Status == Avion.PlaneStatus.Disponible) return true;

            if ((Status == Avion.PlaneStatus.Reserved) && (DernierUtilisateur == currentCallsign) &&
                (resaStatus== ReservationMgr.ReservationStatus.Accepted)) return true;
            return false;
        }
    }
}
