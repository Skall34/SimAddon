using System.Collections.Generic;
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

        public static async Task<List<Mission>> FetchMissionsFromSheet(HttpClient client, string BASEURL)
        {
            bool success = false;
            int nbRetry = 0;
            int result = 0;
            string url = BASEURL + "/api/api_getMissions.php";
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
