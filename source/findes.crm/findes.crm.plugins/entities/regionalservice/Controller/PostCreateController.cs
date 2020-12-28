using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;


namespace findes.crm.plugins.entities.regionalservice.Controller
{
    public class PostCreateController : BaseController
    {
        protected PostCreateController() { }

        public PostCreateController(IOrganizationService organizationService) : base(organizationService)
        {

        }

        public void setGuid(Entity entity)
        {
            OrganizationService.Update(entity);
        }
    }
}
