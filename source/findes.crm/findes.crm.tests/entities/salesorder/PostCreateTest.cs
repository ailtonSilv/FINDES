using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.salesorder.Controller;
using findes.crm.plugins.entities.salesorder.Request;
using findes.crm.plugins.repositories;


namespace findes.crm.plugins.entities.salesorder
{
    public class PostCreate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity();
            Entity postImage = helper.ExtractEntity("post_image");

            var request = new PostCreateRequest(image, helper.GetTracingService(), null, postImage);
            var controller = new PostCreateController(helper.GetOrganizationService());

            var todasAsPropostas = new TodasAsPropostas(helper.GetOrganizationService());

            EntityCollection produtoDaProposta = todasAsPropostas.GetProdutoProposta(request.EntityId);

            var prod = produtoDaProposta.Entities.ToList();

            var proposta = new
            {
                ID = request.EntityId
            };

            controller.criarProdutoOrdem(proposta.ID, prod);

        }
    }
}
