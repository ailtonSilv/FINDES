using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findes.CustomAction.ExtratorCRM.Model
{
    public class DadosEntidade
    {
        public string entityName { get; set; }
        public string entityID { get; set; }
        public string[] entityColumns { get; set; }
        public Dictionary<string, CrmProperty> entityData { get; set; }
        public List<CrmDeParaDN> crmDeParaDNs { get; set; }
    }

    public class CrmProperty
    {
        public string crmname { get; set; }
        public string crmtype { get; set; }
        public string crmtypefield { get; set; }
        public string crmlookupentityname { get; set; }
        public string crmlookupto { get; set; }
        public string crmlookupcolumn { get; set; }
        public bool ismultilookup { get; set; }
        public List<MultiLookup> multiLookupItens { get; set; }
        public List<Filter> filterItens { get; set; }
    }

    public class CrmDeParaDN
    {
        public string entityname { get; set; }
        public string dnname { get; set; }
        public string crmname { get; set; }
        public List<ItemDePara> itens { get; set; }
    }

    public class ItemDePara
    {
        public string name { get; set; }
        public string dn { get; set; }
        public string[] crm { get; set; }
    }

    public class MultiLookup
    {
        public string crmname { get; set; }
        public string crmfromentityname { get; set; }
        public string crmlookupentityname { get; set; }
        public string crmlookupto { get; set; }
        public string crmlookupcolumn { get; set; }
    }

    public class Filter
    {
        public string filtertype { get; set; }
        public List<Conditions> conditions { get; set; }
    }

    public class Conditions
    {
        public string crmfield { get; set; }
        public string operation { get; set; }
        public string value { get; set; }
    }
}
