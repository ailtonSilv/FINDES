using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.entities.salesorderproduct.Request;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.salesorderproduct.Controller
{
    public class PreCreateController : BaseController
    {
        protected PreCreateController()
        {
        }

        public PreCreateController(IOrganizationService organizationService) : base(organizationService)
        {
        }
    }
}
