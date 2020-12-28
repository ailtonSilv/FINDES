using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.requestProduct
{
    public class PostUpdate : BasePlugin
    {        
        protected override void Execute(PluginHelper helper)
        {
            var organizationService = helper.GetOrganizationService();
            
            Entity post_image = helper.ExtractEntity("post_image");
           

            if (post_image.GetAttributeValue<OptionSetValue>("statuscode").Value == 482870002 || post_image.GetAttributeValue<OptionSetValue>("statuscode").Value == 482870004) 
            {
                var todasOpp = new TodasAsOportunidades(organizationService);
                                 
                todasOpp.updateProductOpportunity(post_image); 

            }
        }
    }
}
