using System;
using System.Collections.Generic;

namespace FINDES.ConsoleCarga
{
    public partial class CargaFields
    {
        public class Municipio
        {
            public static string entityName = "findes_municipio";
            public static string tableSQL = "CRM_Municipio";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "findes_name", new Tuple<string, string>("findes_name", "string") },
                { "findes_codigo", new Tuple<string, string>("findes_codigo", "string") },
                { "findes_estadoid(id)", new Tuple<string, string>("findes_estadoid", "lookup") },
                { "findes_regionalid(id)", new Tuple<string, string>("findes_regionalid", "lookup") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") }
            };
        }
    }

}
