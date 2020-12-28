using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.repositories {

    internal class TodosOsContatos {

        private IOrganizationService organizationService = null;
        internal TodosOsContatos(IOrganizationService organizationService) {
            this.organizationService = organizationService;
        }

        internal Entity GetContatoPorCpf(string cnpj) {

            var query = new QueryExpression("contact") {
                ColumnSet = new ColumnSet("firstname")
            };
            query.Criteria.AddCondition("findes_cpf", ConditionOperator.Equal, cnpj);

            EntityCollection ec = this.organizationService.RetrieveMultiple(query);

            foreach (var item in ec.Entities) {
                return item;
            }

            return null;
        }


        

        private Guid getContactIDbyOpportunity(Guid OpportunityID)
        {
            var result = organizationService.Retrieve("opportunity", OpportunityID, new ColumnSet("parentcontactid"));
            Guid ContactID = result.GetAttributeValue<Guid>("parentcontactid");

            return ContactID;
        }

    }
}
