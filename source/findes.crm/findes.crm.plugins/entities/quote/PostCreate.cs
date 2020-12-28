using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.quote.Controller;
using findes.crm.plugins.entities.quote.Request;

namespace findes.crm.plugins.entities.quote
{
    public class PostCreate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity();
            Entity postImage = helper.ExtractEntity("post_image");

            var request = new PostCreateRequest(image, helper.GetTracingService(), null, postImage);
            var controller = new PostCreateController(helper.GetOrganizationService());

            controller.createQuoteNumer(image.Id);
        }
    }
}
