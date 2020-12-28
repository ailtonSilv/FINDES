using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class Product
        {
            public static string entityName = "product";
            public static string tableSQL = "CRM_Product";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "productid", new Tuple<string, string>("productid", "guid") },
                { "productnumber", new Tuple<string, string>("productnumber", "string") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "defaultuomid", new Tuple<string, string>("defaultuomid", "lookup") },
                { "defaultuomscheduleid(id)", new Tuple<string, string>("defaultuomscheduleid", "lookup") },
                { "producttypecode", new Tuple<string, string>("producttypecode", "optionset") },
                { "findes_instraestruturarequirida", new Tuple<string, string>("findes_instraestruturarequirida", "string") },
                { "findes_oportunidadedemelhoria", new Tuple<string, string>("findes_oportunidadedemelhoria", "string") },
                { "quantitydecimal", new Tuple<string, string>("quantitydecimal", "int") },
                { "description", new Tuple<string, string>("description", "string") },
                { "name", new Tuple<string, string>("name", "string") },
                { "parentproductid(id)", new Tuple<string, string>("parentproductid", "lookup") },
                { "findes_codigoerp", new Tuple<string, string>("findes_codigoerp", "string") },
                { "quantityonhand", new Tuple<string, string>("quantityonhand", "decimal") },
                { "findes_statusintegracao", new Tuple<string, string>("findes_statusintegracao", "optionset") },
                { "findes_codigoauxiliarerp", new Tuple<string, string>("findes_codigoauxiliarerp", "string") },
                { "findes_cargahoraria", new Tuple<string, string>("findes_cargahoraria", "int") },
                { "findes_modalidade", new Tuple<string, string>("findes_modalidade", "optionset") },
                { "findes_homologadodn", new Tuple<string, string>("findes_homologadodn", "boolean") },
                { "findes_quantidademinima", new Tuple<string, string>("findes_quantidademinima", "int") },
                { "findes_quantidademaxima", new Tuple<string, string>("findes_quantidademaxima", "int") },
                { "findes_objetivo", new Tuple<string, string>("findes_objetivo", "string") },
                { "findes_conteudoprogramatico", new Tuple<string, string>("findes_conteudoprogramatico", "string") },
                { "findes_publicoalvo", new Tuple<string, string>("findes_publicoalvo", "string") },
                { "findes_responsabilidadecontratante", new Tuple<string, string>("findes_responsabilidadecontratante", "string") },
                { "findes_responsabilidadecontratado", new Tuple<string, string>("findes_responsabilidadecontratado", "string") },
                { "modifiedby(id)", new Tuple<string, string>("modifiedby", "lookup") },
                { "findes_coligadaid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
                { "findes_tipoprofissional", new Tuple<string, string>("findes_tipoprofissional", "optionset") },
                { "findes_prerequisitos", new Tuple<string, string>("findes_prerequisitos", "string") },
                { "findes_beneficios", new Tuple<string, string>("findes_beneficios", "string") },
                { "findes_beneficiostrabalhadores", new Tuple<string, string>("findes_beneficiostrabalhadores", "string") },
                { "findes_etapasexecucao", new Tuple<string, string>("findes_etapasexecucao", "string") },
                { "findes_materialentregue", new Tuple<string, string>("findes_materialentregue", "string") },
                { "findes_instalacoesfisicas", new Tuple<string, string>("findes_instalacoesfisicas", "string") },
                { "findes_equipamentosnecessarios", new Tuple<string, string>("findes_equipamentosnecessarios", "string") },
                { "findes_perfilprofissional", new Tuple<string, string>("findes_perfilprofissional", "string") },
                { "findes_elementosdecusto", new Tuple<string, string>("findes_elementosdecusto", "string") },
                { "findes_volumeminimo", new Tuple<string, string>("findes_volumeminimo", "int") },
                { "findes_principaldiferencial", new Tuple<string, string>("findes_principaldiferencial", "string") },
                { "statecode", new Tuple<string, string>("statecode", "optionset") },
                { "statuscode", new Tuple<string, string>("statuscode", "optionset") },
                { "findes_principalfraqueza", new Tuple<string, string>("findes_principalfraqueza", "string") },
                { "findes_contornarfraqueza", new Tuple<string, string>("findes_contornarfraqueza", "string") },
                { "findes_metodologia", new Tuple<string, string>("findes_metodologia", "string") },
                { "findes_solucaoid(id)", new Tuple<string, string>("findes_solucaoid", "lookup") },
                { "findes_linhaatuacaoid(id)", new Tuple<string, string>("findes_linhaatuacaoid", "lookup") },
                { "findes_familiaprodutoid(id)", new Tuple<string, string>("findes_familiaprodutoid", "lookup") },
                { "findes_categoriaid(id)", new Tuple<string, string>("findes_categoriaid", "lookup") }
            };
        }
    }
}
