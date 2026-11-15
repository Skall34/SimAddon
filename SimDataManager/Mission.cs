using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimDataManager
{
    public class  MissionsPhpList
    {
        public bool success;
        public List<Dictionary<string, string>> missions;
    }

    public class Mission
    {
        public int Index { get; set; }
        public string Libelle { get; set; }
        public int Active { get; set; }
        public int EstControle { get; set; }
        /// <summary>Types d'appareils autorisés (CSV, '*' = tous). "*" si le serveur ne renvoie pas le champ.</summary>
        public string TypesAutorises { get; set; } = "*";

        /// <summary>
        /// Mission par défaut au lancement de l'ACARS : active (Active==1), pas une mission de
        /// contrôle, et accepte tous les types d'avion (types_autorises '*' ou vide).
        /// </summary>
        public static Mission ParDefaut(IEnumerable<Mission> missions)
        {
            if (missions == null) return null;
            return missions
                .Where(m => m.Active == 1 && m.EstControle == 0 && (string.IsNullOrEmpty(m.TypesAutorises) || m.TypesAutorises.Trim() == "*"))
                .OrderBy(m => m.Libelle)
                .FirstOrDefault();
        }

        /// <summary>
        /// Mission à conserver après un changement d'immatriculation : la mission de contrôle
        /// n'est posée automatiquement que pour un avion en attente de contrôle (ControleAFaire=1).
        /// Si l'utilisateur change d'avion pour un appareil qui n'est PAS en attente de contrôle,
        /// on retombe sur la mission par défaut ; dans tous les autres cas la mission est inchangée.
        /// </summary>
        public static Mission ApresChangementAvion(Mission courante, Avion nouvelAvion, IEnumerable<Mission> toutes)
        {
            if (courante != null && courante.EstControle == 1
                && (nouvelAvion == null || nouvelAvion.ControleAFaire != 1))
            {
                return ParDefaut(toutes);
            }
            return courante;
        }

        // comparaison par libellé (et non par référence) : indispensable pour que
        // ComboBox.Items.Contains() retrouve la mission après un rechargement de la liste
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Mission objAsMission = obj as Mission;
            if (objAsMission == null) return false;
            return this.Libelle == objAsMission.Libelle;
        }

        public override int GetHashCode()
        {
            return Libelle != null ? Libelle.GetHashCode() : 0;
        }

        public static async Task<List<Mission>> FetchMissionsFromSheet(HttpClient client, string BASEURL, string token, string planeType = null)
        {
            bool success = false;
            int nbRetry = 0;
            int result = 0;
            string url = BASEURL + "/api/api_getMissions.php?session_token="+Uri.EscapeDataString(token);
            // si un type d'appareil est précisé, ne récupérer que les missions compatibles
            if (!string.IsNullOrEmpty(planeType))
            {
                url += "&type=" + Uri.EscapeDataString(planeType);
            }
            List<Mission> missions = null;
            UrlDeserializer dataReader = new UrlDeserializer(client, url);
            while ((!success)&&(nbRetry<3))
            {
                try
                {
                    missions = await dataReader.FetchMissionsDataAsync();
                    if ((missions == null)||(missions.Count==0))
                    {
                        throw new System.Exception("Missions data is null");
                    }
                    success = true;
                }
                catch{
                    success = false;
                    //wait and retry
                    System.Threading.Thread.Sleep(1000);
                    nbRetry++;
                }
            }
            return missions;
        }

        public bool IsSelectable(bool IsReserved)
        {
            //a mission is selectable if active = 1 or reserved
            if ((Active == 2)&&(!IsReserved)) return false;
            return true;
        }

    }
}
