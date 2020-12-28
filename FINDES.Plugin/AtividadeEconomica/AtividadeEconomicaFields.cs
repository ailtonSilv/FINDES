using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class AtividadeEconomica
        {
            public static string entityName = "findes_atividadeeconomica";
            public static string tableSQL = "CRM_AtividadeEconomica";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "findes_atividadeeconomicaid", new Tuple<string, string>("findes_atividadeeconomicaid", "guid") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "findes_name", new Tuple<string, string>("findes_name", "string") },
                { "findes_codigo", new Tuple<string, string>("findes_codigo", "string") },
                { "findes_descricao", new Tuple<string, string>("findes_descricao", "string") },
                { "findes_nivelhierarquia", new Tuple<string, string>("findes_nivelhierarquia", "optionset") },
                { "findes_atividadesecaoid(id)", new Tuple<string, string>("findes_atividadesecaoid", "lookup") },
                { "findes_atividadedivisaoid(id)", new Tuple<string, string>("findes_atividadedivisaoid", "lookup") },
                { "findes_atividadegrupoid(id)", new Tuple<string, string>("findes_atividadegrupoid", "lookup") },
                { "findes_atividadeclasseid(id)", new Tuple<string, string>("findes_atividadeclasseid", "lookup") }
            };
        }
        
    }
}
