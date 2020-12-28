using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class SalesOrder
        {
            public static string entityName = "salesorder";
            public static string tableSQL = "CRM_EspelhoProposta";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "findes_espelhopropostaid", new Tuple<string, string>("findes_espelhopropostaid", "string") },
                { "name", new Tuple<string, string>("name", "string") },
                { "ownerid(id)", new Tuple<string, string>("ownerid", "lookup") },
                { "modifiedby(id)", new Tuple<string, string>("modifiedby", "lookup") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "findes_quantidadefaturado", new Tuple<string, string>("findes_quantidadefaturado", "money") },
                { "findes_agencia", new Tuple<string, string>("findes_agencia", "int") },
                { "findes_bancoid(id)", new Tuple<string, string>("findes_bancoid", "lookup") },
                { "findes_contabancaria", new Tuple<string, string>("findes_contabancaria", "int") },
                { "findes_digito", new Tuple<string, string>("findes_digito", "int") },
                { "paymenttermscode", new Tuple<string, string>("paymenttermscode", "optionset") },
                { "customerid(id)", new Tuple<string, string>("findes_codigoerp", "lookup") },
                { "findes_contaservicoid(id)", new Tuple<string, string>("findes_codigoerp", "lookup") },
                { "findes_contatopagamentoid(id)", new Tuple<string, string>("findes_contatopagamentoid", "lookup") },
                { "findes_contratobasenacional", new Tuple<string, string>("findes_contratobasenacional", "boolean") },
                { "findes_codigoerp", new Tuple<string, string>("findes_codigoerp", "string") },
                { "findes_iniciocontrato", new Tuple<string, string>("findes_iniciocontrato", "datetime") },
                { "findes_quantidadeparcelas", new Tuple<string, string>("findes_quantidadeparcelas", "optionset") },
                { "findes_statusfaturamento", new Tuple<string, string>("findes_statusfaturamento", "optionset") },
                { "findes_statusexecucao", new Tuple<string, string>("findes_statusexecucao", "optionset") },
                { "findes_tipoinstrumento", new Tuple<string, string>("findes_tipoinstrumento", "optionset") },
                { "totalamount", new Tuple<string, string>("totalamount", "money") },
                { "billto_line1", new Tuple<string, string>("billto_line1", "string") },
                { "billto_line2", new Tuple<string, string>("billto_line2", "string") },
                { "billto_line3", new Tuple<string, string>("billto_line3", "string") },
                { "billto_postalcode", new Tuple<string, string>("billto_postalcode", "string") },
                { "findes_estadopagamentoid(id)", new Tuple<string, string>("findes_estadopagamentoid", "lookup") },
                { "findes_municipiopagamentoid(id)", new Tuple<string, string>("findes_municipiopagamentoid", "lookup") },
                { "findes_terminocontrato", new Tuple<string, string>("findes_terminocontrato", "datetime") },
                { "findes_coligadaid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
            };
        }
    }

}
