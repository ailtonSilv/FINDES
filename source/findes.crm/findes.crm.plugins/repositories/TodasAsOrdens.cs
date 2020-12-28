using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.repositories
{
    public class TodasAsOrdens
    {

        private IOrganizationService service = null;


        public TodasAsOrdens(IOrganizationService service)
        {
            this.service = service ?? throw new ArgumentNullException("Organization Service", "O Serviço do CRM não foi informado ou é nulo.");
        }


        public EntityCollection getIdOrderDetail(Guid guid)
        {
            QueryExpression query = new QueryExpression("salesorderdetail")
            {
                ColumnSet = new ColumnSet("salesorderdetail", "salesorderid")
            };
            query.Criteria.AddCondition("salesorderid", ConditionOperator.Equal, guid);

            return service.RetrieveMultiple(query);

            
        }

       

       
    }
}
