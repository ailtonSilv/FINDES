using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.sindicato.Controller;
using findes.crm.plugins.repositories;

namespace findes.crm.plugins.entities.sindicato
{
    public class PostCreate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {

            Entity image = helper.ExtractEntity();

            var controller = new PostCreateController(helper.GetOrganizationService());

            Entity sindicatoEntity = new Entity("findes_sindicato", image.Id);

            sindicatoEntity["findes_codigo"] = image.Id.ToString().ToUpper();

            controller.setGuid(sindicatoEntity);
        }
    }
}
