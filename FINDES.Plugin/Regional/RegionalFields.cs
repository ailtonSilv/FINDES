using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class Regional
        {
            public static string entityName = "territory";
            public static string tableSQL = "CRM_Regional";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "territoryid", new Tuple<string, string>("territoryid", "guid") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "name", new Tuple<string, string>("name", "string") },
                { "findes_gerenteresponsavelid(id)", new Tuple<string, string>("findes_gerenteresponsavelid", "lookup") },
                { "findes_equiperesponsavelid(id)", new Tuple<string, string>("findes_equiperesponsavelid ", "lookup") },
                { "managerid(id)", new Tuple<string, string>("managerid", "lookup") },
            };
        }
        
    }
}
