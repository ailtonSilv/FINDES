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
    public class PostCreate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity();
            Entity postImage = helper.ExtractEntity("post_image");


            var request = new PostCreateRequest(image, helper.GetTracingService(), null, postImage);
            var controller = new PostCreateController(helper.GetOrganizationService());

            Guid quoteId = controller.getQuoteGuid(image.LogicalName, image.Id);

            var todasAsPropostas = new TodasAsPropostas(helper.GetOrganizationService());

            EntityCollection produtoDaProposta = todasAsPropostas.GetProdutoProposta(quoteId);

            var prod = produtoDaProposta.Entities.ToList();

            var sales = new { id = request.EntityId };

            controller.criarProdutoOrdem(quoteId, prod);

        }
    }
}
