using System.Collections.Generic;
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

        public static async Task<List<Mission>> FetchMissionsFromSheet(string BASEURL)
        {
            int result = 0;
            string url = BASEURL + "/api/api_getMissions.php";
            UrlDeserializer dataReader = new UrlDeserializer(url);
            List<Mission> missions = await dataReader.FetchMissionsDataAsync();
            return missions;
        }
    }
}
