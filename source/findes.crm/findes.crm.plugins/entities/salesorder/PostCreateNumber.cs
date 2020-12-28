using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.salesorder.Controller;
using findes.crm.plugins.entities.salesorder.Request;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.entities.salesorder
{
    public class PostCreateNumber : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity();
            Entity postImage = helper.ExtractEntity("post_image");

            var request = new PostCreateRequest(image, helper.GetTracingService(), null, postImage);
            var controller = new PostCreateController(helper.GetOrganizationService());


            controller.UpdateOrderNumber(image.Id, image.LogicalName);
        }
    }
}
