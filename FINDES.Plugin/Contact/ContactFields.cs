using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin.CRMFields
{
    public partial class Fields
    {
        public class Contact
        {
            public static string entityName = "contact";
            public static string tableSQL = "CRM_Contact";
            public static Dictionary<string, Tuple<string, string>> fields = new Dictionary<string, Tuple<string, string>>() {
                { "contactid", new Tuple<string, string>("contactid", "guid") },
                { "createdon", new Tuple<string, string>("createdon", "datetime") },
                { "modifiedon", new Tuple<string, string>("modifiedon", "datetime") },
                { "firstname", new Tuple<string, string>("firstname", "string") },
                { "lastname", new Tuple<string, string>("lastname", "string") },
                { "fullname", new Tuple<string, string>("fullname", "string") },
                { "jobtitle", new Tuple<string, string>("jobtitle", "string") },
                { "parentcustomerid(id)", new Tuple<string, string>("parentcustomerid", "lookup") },
                { "findes_cpf", new Tuple<string, string>("findes_cpf", "string") },
                { "modifiedby(id)", new Tuple<string, string>("modifiedby", "lookup") },
                { "description", new Tuple<string, string>("description", "string") },
                { "emailaddress1", new Tuple<string, string>("emailaddress1", "string") },
                { "address2_line2", new Tuple<string, string>("address2_line2", "string") },
                { "address2_postalcode", new Tuple<string, string>("address2_postalcode", "string") },
                { "address1_postalcode", new Tuple<string, string>("address1_postalcode", "string") },
                { "findes_municipioprincipalid(id)", new Tuple<string, string>("findes_municipioprincipalid", "lookup") },
                { "findes_municipiopagamentoid(id)", new Tuple<string, string>("findes_municipiopagamentoid", "lookup") },
                { "findes_estadoprincipalid(id)", new Tuple<string, string>("findes_estadoprincipalid", "lookup") },
                { "findes_estadopagamentoid(id)", new Tuple<string, string>("findes_estadopagamentoid", "lookup") },
                { "address1_line2", new Tuple<string, string>("address1_line2", "string") },
                { "address1_line3", new Tuple<string, string>("address1_line3", "string") },
                { "findes_codigoerp", new Tuple<string, string>("findes_codigoerp", "string") },
                { "statecode", new Tuple<string, string>("statecode", "optionset") },
                { "mobilephone", new Tuple<string, string>("mobilephone", "string") },
                { "telephone1", new Tuple<string, string>("telephone1", "string") },
                { "telephone2", new Tuple<string, string>("telephone2", "string") },
                { "findes_funcaocontato", new Tuple<string, string>("findes_funcaocontato", "optionset") },
                { "findes_tipocontato", new Tuple<string, string>("findes_tipocontato", "optionset") },
                { "findes_titular(id)", new Tuple<string, string>("findes_titular", "lookup") },
                { "findes_rg", new Tuple<string, string>("findes_rg", "string") },
                { "birthdate", new Tuple<string, string>("birthdate", "datetime") },
                { "familystatuscode", new Tuple<string, string>("familystatuscode", "optionset") },
                { "gendercode", new Tuple<string, string>("gendercode", "optionset") },
                { "findes_orgaoemissor", new Tuple<string, string>("findes_orgaoemissor", "string") },
                { "findes_estadoemissor", new Tuple<string, string>("findes_estadoemissor", "string") },
                { "emailaddress2", new Tuple<string, string>("emailaddress2", "string") },
                { "findes_coligadaid(id)", new Tuple<string, string>("findes_codigo", "lookup") },
                { "findes_categoriacontato", new Tuple<string, string>("findes_categoriacontato", "optionset") },
                { "ownerid(id)", new Tuple<string, string>("ownerid", "lookup") },
                { "address1_line1", new Tuple<string, string>("address1_line1", "string") },
                { "address2_line3", new Tuple<string, string>("address2_line3", "string") },
                { "address2_line1", new Tuple<string, string>("address2_line1", "string") },
                { "findes_clientefornecedortotvs", new Tuple<string, string>("findes_clientefornecedortotvs", "optionset") }
            };
        }
        
    }
}
