using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class UnidadedeAtendimento
        {
            public static string entityName = "findes_unidadedeatendimento";
            public static string tableSQL = "CRM_Filial";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "findes_unidadedeatendimentoid", new Tuple<string, string>("findes_unidadedeatendimentoid", "guid") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "modifiedby(id)", new Tuple<string, string>("modifiedby", "lookup") },
                { "findes_cnpj", new Tuple<string, string>("findes_cnpj", "string") },
                { "findes_municipioid(id)", new Tuple<string, string>("findes_municipioid", "lookup") },
                { "findes_inscricaoestadual", new Tuple<string, string>("findes_inscricaoestadual", "string") },
                { "findes_name", new Tuple<string, string>("findes_name", "string") },
                { "findes_coligadaid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
                { "findes_regionaldeatendimentoid(id)", new Tuple<string, string>("findes_regionaldeatendimentoid", "lookup") },
                { "findes_gerenteunidadeid(id)", new Tuple<string, string>("findes_gerenteunidadeid", "lookup") },
            };
        }
    }
}
