using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.entities.account.controller {
    public class PreCreateController : BaseController {

        protected PreCreateController() {
        }

        public PreCreateController(IOrganizationService organizationService) : base(organizationService) {

        }

        public bool ExisteUmaContaComEste(string cnpj, out Entity conta) {

            conta = this.GetContaPorCnpj(cnpj);

            return null != conta;

        }

        private Entity GetContaPorCnpj(string cnpj) {

            var query = new QueryExpression("account") {
                ColumnSet = new ColumnSet("name")
            };
            query.Criteria.AddCondition("findes_cnpj", ConditionOperator.Equal, cnpj);

            EntityCollection ec = base.OrganizationService.RetrieveMultiple(query);

            foreach (var item in ec.Entities) {
                return item;
            }

            return null;
        }
    }
}
