using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.salesorderproduct.Request;
using findes.crm.plugins.entities.salesorderproduct.Controller;
using findes.crm.plugins.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.entities.salesorderdetail
{
    public class PostCreate : BasePlugin
    {

        public PostCreate(string unsecure ="") : base(unsecure)
        {

        }
        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity();
            Entity preImage = helper.ExtractEntity("pre_image");

            var request = new PreCreateRequest(image, helper.GetTracingService(), null, preImage);
            var controller = new PreCreateController(helper.GetOrganizationService());

            if (request.Product != Guid.Empty)
            {
                var todasPropostas = new TodasAsPropostas(helper.GetOrganizationService());

                Entity produto = todasPropostas.getProduto(request.Product);

                if (produto == null)
                {
                    throw new InvalidPluginExecutionException("Produto não existe");
                }

                if (!image.Attributes.Contains("findes_coligadaid"))
                {
                    request.setColigada(produto.GetAttributeValue<EntityReference>("findes_coligadaid"));
                }
                if (!image.Attributes.Contains("findes_solucaoid"))
                {
                    request.setSolution(produto.GetAttributeValue<EntityReference>("findes_solucaoid"));
                }
                if (!image.Attributes.Contains("findes_linhaatuacaoid"))
                {
                    request.setLineOfActin(produto.GetAttributeValue<EntityReference>("findes_categoriaid"));
                }
                if (!image.Attributes.Contains("productid"))
                {
                    request.setProduct(produto.GetAttributeValue<EntityReference>("productid"));
                }
                if (!image.Attributes.Contains("findes_produtofilialid"))
                {
                    request.setBranchProduct(produto.GetAttributeValue<EntityReference>("findes_produtofilialid"));
                }
                if (!image.Attributes.Contains("uomid"))
                {
                    request.setUnity(produto.GetAttributeValue<EntityReference>("uomid"));
                }
                if (!image.Attributes.Contains("findes_unidadeexecutoraid"))
                {
                    request.setExecutingUnit(produto.GetAttributeValue<EntityReference>("findes_unidadeexecutoraid"));
                }
            }
        }
    }
}
