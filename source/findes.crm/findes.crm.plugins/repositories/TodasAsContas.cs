using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.repositories
{
    internal class TodasAsContas
    {
        private IOrganizationService organizationService = null;

        internal TodasAsContas(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
        }


        internal EntityReference getCustomerReferenceByQuote(Guid QuoteId, out EntityReference contact)
        {
            var result = organizationService.Retrieve("quote", QuoteId, new ColumnSet("customerid", "contactid"));
            EntityReference customerID = result.GetAttributeValue<EntityReference>("customerid");
            contact = result.GetAttributeValue<EntityReference>("contactid");
            //customerID.Id = result.GetAttributeValue<Guid>("customerid");

            return customerID;
        }


      
    }
}
