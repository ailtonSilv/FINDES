using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class Account
        {
            public static string entityName = "account";
            public static string tableSQL = "CRM_Account";
            /*
             * Removidos e-mail Helbertt 27-ABR-2020
                { "findes_classeid(id)", new Tuple<string, string>("findes_classeid", "lookup") },
                { "numberofemployees", new Tuple<string, string>("numberofemployees", "int") },
                { "customersizecode", new Tuple<string, string>("customersizecode", "optionset") },
                { "findes_sindicatoid(id)", new Tuple<string, string>("findes_sindicatoid", "lookup") },
                { "findes_tipodecliente", new Tuple<string, string>("findes_tipodecliente", "optionset") },
            */
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "accountid", new Tuple<string, string>("accountid", "guid") },
                { "name", new Tuple<string, string>("name", "string") },
                { "ownerid(id)", new Tuple<string, string>("ownerid", "lookup") },
                { "createdon", new Tuple<string, string>("createdon", "datetime") },
                { "modifiedby(id)", new Tuple<string, string>("modifiedby", "lookup") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "address2_line3", new Tuple<string, string>("address2_line3", "string") },
                { "address1_line3", new Tuple<string, string>("address1_line3", "string") },
                { "address1_postalcode", new Tuple<string, string>("address1_postalcode", "string") },
                { "address2_postalcode", new Tuple<string, string>("address2_postalcode", "string") },
                { "findes_municipiopagamentoid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
                { "findes_municipioprincipalid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
                { "findes_clientefornecedortotvs", new Tuple<string, string>("findes_clientefornecedortotvs", "optionset") },
                { "findes_clientefornecedor", new Tuple<string, string>("findes_clientefornecedor", "optionset") },
                { "findes_clientedn", new Tuple<string, string>("findes_clientedn", "bool") },
                { "findes_cnpj", new Tuple<string, string>("findes_cnpj", "string") },
                { "findes_coligadaid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
                { "address1_line2", new Tuple<string, string>("address1_line2", "string") },
                { "parentaccountid(id)", new Tuple<string, string>("parentaccountid", "lookup") },
                { "primarycontactid(id)", new Tuple<string, string>("primarycontactid", "lookup") },
                { "findes_contribuintefindes", new Tuple<string, string>("findes_contribuintefindes", "optionset") },
                { "emailaddress1", new Tuple<string, string>("emailaddress1", "string") },
                { "findes_estadopagamentoid(id)", new Tuple<string, string>("findes_estadopagamentoid", "lookup") },
                { "findes_estadoprincipalid(id)", new Tuple<string, string>("findes_estadoprincipalid", "lookup") },
                { "findes_codigoerp", new Tuple<string, string>("findes_codigoerp", "string") },
                { "findes_inscricaoestadual", new Tuple<string, string>("findes_inscricaoestadual", "string") },
                { "findes_inscricaomunicipal", new Tuple<string, string>("findes_inscricaomunicipal", "string") },
                { "address1_line1", new Tuple<string, string>("address1_line1", "string") },
                { "address2_line1", new Tuple<string, string>("address2_line1", "string") },
                { "findes_razaosocial", new Tuple<string, string>("findes_razaosocial", "string") },
                { "territoryid(id)", new Tuple<string, string>("territoryid", "lookup") },
                { "findes_situacaofinanceira", new Tuple<string, string>("findes_situacaofinanceira", "boolean") },
                { "statecode", new Tuple<string, string>("statecode", "optionset") },
                { "telephone1", new Tuple<string, string>("telephone1", "string") },
                { "findes_tipoindustria", new Tuple<string, string>("findes_tipoindustria", "optionset") }
            };
        }
        
    }
}
