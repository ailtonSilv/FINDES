using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.regionalservice.Controller;

namespace findes.crm.plugins.entities.regionalservice
{
    public class PostCreate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {

            Entity imagem = helper.ExtractEntity();

            var controller = new PostCreateController(helper.GetOrganizationService());

            Entity regionalService = new Entity("territory", imagem.Id);

            regionalService["findes_codigo"] = imagem.Id.ToString().ToUpper();

            controller.setGuid(regionalService);
        }
    }
}
