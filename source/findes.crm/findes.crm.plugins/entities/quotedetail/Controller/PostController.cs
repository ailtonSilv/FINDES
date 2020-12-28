using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.entities.quotedetail.Controller
{
    public class PostController : BaseController
    {
        public PostController()
        {
        }

        public PostController(IOrganizationService organizationService) : base(organizationService)
        {
        }

        public void UpdateQuoteDetail(Entity quoteDetail)
        {
            OrganizationService.Update(quoteDetail);
        }

        public Entity UpdateDescripton(Entity product,Guid quoteDetailID)
        {

            Entity quoteDetail = new Entity("quotedetail", quoteDetailID);

            quoteDetail["findes_volumeminimo"] = product.GetAttributeValue<int>("findes_volumeminimo");
            quoteDetail["findes_cargahoraria"] = product.GetAttributeValue<int>("findes_cargahoraria");
            quoteDetail["findes_quantidademinima"] = product.GetAttributeValue<int>("findes_quantidademinima");
            quoteDetail["findes_modalidade"] = product.GetAttributeValue<OptionSetValue>("findes_modalidade");
            quoteDetail["findes_quantidademaxima"] = product.GetAttributeValue<int>("findes_quantidademaxima");
            quoteDetail["findes_tipoprofissional"] = product.GetAttributeValue<OptionSetValue>("findes_tipoprofissional");
            quoteDetail["findes_quantidadealunosturma"] = product.GetAttributeValue<int>("findes_quantidadealunosturma");
            quoteDetail["findes_quantidadealunosturma"] = product.GetAttributeValue<int>("findes_quantidadealunosturma");

            if (product.GetAttributeValue<string>("findes_prerequisitos") != null)
            {
                quoteDetail["findes_prerequisitos"] = product.GetAttributeValue<string>("findes_prerequisitos");
            }

            if (product.GetAttributeValue<string>("findes_conteudoprogramatico") != null)
            {
                quoteDetail["findes_conteudoprogramatico"] = product.GetAttributeValue<string>("findes_conteudoprogramatico");
            }
            if (product.GetAttributeValue<string>("findes_conteudoprogramatico") != null)
            {
                quoteDetail["findes_conteudoprogramatico"] = product.GetAttributeValue<string>("findes_conteudoprogramatico");
            }
            if (product.GetAttributeValue<string>("findes_metodologia") != null)
            {
                quoteDetail["findes_metodologia"] = product.GetAttributeValue<string>("findes_metodologia");
            }
            if (product.GetAttributeValue<string>("findes_objetivo") != null)
            {
                quoteDetail["findes_objetivo"] = product.GetAttributeValue<string>("findes_objetivo");
            }
            if (product.GetAttributeValue<string>("findes_observacoes") != null)
            {
                quoteDetail["findes_observacoes"] = product.GetAttributeValue<string>("findes_observacoes");
            }
            if (product.GetAttributeValue<string>("description") != null)
            {
                quoteDetail["description"] = product.GetAttributeValue<string>("description");
                quoteDetail["findes_resumoproduto"] = product.GetAttributeValue<string>("description");
            }

            return quoteDetail;
        }
    }
}
