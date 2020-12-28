using System;
using System.Collections.Generic;

namespace FINDES.ConsoleCarga
{
    public partial class CargaFields
    {
        public class Estado
        {
            public static string entityName = "findes_estado";
            public static string tableSQL = "CRM_Estado";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "findes_codigo", new Tuple<string, string>("findes_codigo", "string") },
                { "findes_name", new Tuple<string, string>("findes_name", "string") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") }
            };
        }
    }

}
