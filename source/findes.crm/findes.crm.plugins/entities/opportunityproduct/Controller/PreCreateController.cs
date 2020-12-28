using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.entities.opportunityproduct.Request;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.opportunityproduct.Controller
{
    class PreCreateController : BaseController
    {
        protected PreCreateController()
        {
        }

        public PreCreateController(IOrganizationService organizationService) : base(organizationService)
        {
        }

        
    }
}
