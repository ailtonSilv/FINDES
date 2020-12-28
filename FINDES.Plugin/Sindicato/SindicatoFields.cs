using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class Sindicato
        {
            public static string entityName = "findes_sindicato";
            public static string tableSQL = "CRM_Sindicato";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "findes_name", new Tuple<string, string>("findes_name", "string") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "findes_codigo", new Tuple<string, string>("findes_codigo", "string") }
            };
        }
    }

}
