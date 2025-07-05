using System.Collections.Generic;

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
    }
}
