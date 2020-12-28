using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.entities.opportunity.Request;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.entities.quote.Controller
{
    public class PostCreateController : BaseController
    {
        protected PostCreateController()
        {
        }

        public PostCreateController(IOrganizationService organizationService) : base(organizationService)
        {
        }

        public void createQuoteNumer(Guid propostaId)
        {
            Entity quote = new Entity("quote", propostaId);
            var result = OrganizationService.Retrieve("quote", propostaId, new ColumnSet("quotenumber"));

            var aux = result.GetAttributeValue<string>("quotenumber").ToString();
            aux = aux.Substring(4, 5);

            quote["quotenumber"] = aux.ToString();
            quote["findes_propostaid"] = aux.ToString();
            OrganizationService.Update(quote);
        }
    }
}
