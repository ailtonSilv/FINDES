using Microsoft.VisualStudio.TestTools.UnitTesting;
using findes.crm.plugins.entities.quotedetail.Request;
using findes.crm.plugins.entities.quotedetail.Controller;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;

namespace findes.crm.tests.entities.quotedetail
{
    [TestClass]
    public class PluginPreCreateTests
    {
        [TestMethod]
        public void Preencher_Campos_Produto_Cotacao_via_UPDATE()
        {
            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("quotedetail", new Guid("0D1030AE-8933-E911-A955-000D3AC1BEBA"), new Microsoft.Xrm.Sdk.Query.ColumnSet(
               "productid","quoteid", "ispriceoverridden"));

            Entity preImage = image;

            var request = new PostRequest(image, null, null, preImage);
            var controller = new PostController(organizationService);


            if (image.GetAttributeValue<bool>("ispriceoverridden") == false)
            {

                if (request.ProductId != Guid.Empty)
                {
                    var todasAsOportunidades = new TodasAsOportunidades(organizationService);

                    Entity produto = todasAsOportunidades.GetProdutoPor(request.ProductId);

                    if (produto == null)
                    {
                        throw new InvalidPluginExecutionException("Produto não existe");
                    }

                    if (!image.Attributes.Contains("description"))
                    {
                        image["description"] = produto.GetAttributeValue<string>("description");
                    }
                    if (!image.Attributes.Contains("findes_volumeminimo"))
                    {
                        image["findes_volumeminimo"] = produto.GetAttributeValue<int>("findes_volumeminimo");
                    }
                    if (!image.Attributes.Contains("findes_cargahoraria"))
                    {
                        image["findes_cargahoraria"] = produto.GetAttributeValue<int>("findes_cargahoraria");
                    }
                    if (!image.Attributes.Contains("findes_quantidademinima"))
                    {
                        image["findes_quantidademinima"] = produto.GetAttributeValue<int>("findes_quantidademinima");
                    }
                    if (!image.Attributes.Contains("findes_modalidade"))
                    {
                        image["findes_modalidade"] = produto.GetAttributeValue<OptionSetValue>("findes_modalidade");
                    }
                    if (!image.Attributes.Contains("findes_quantidademaxima"))
                    {
                        image["findes_quantidademaxima"] = produto.GetAttributeValue<int>("findes_quantidademaxima");
                    }
                    if (!image.Attributes.Contains("findes_tipoprofissional"))
                    {
                        image["findes_tipoprofissional"] = produto.GetAttributeValue<OptionSetValue>("findes_tipoprofissional");
                    }
                    if (!image.Attributes.Contains("findes_prerequisitos"))
                    {
                        image["findes_prerequisitos"] = produto.GetAttributeValue<string>("findes_prerequisitos");
                    }
                    if (!image.Attributes.Contains("findes_objetivo"))
                    {
                        image["findes_objetivo"] = produto.GetAttributeValue<string>("findes_objetivo");
                    }
                    if (!image.Attributes.Contains("findes_observacoes"))
                    {
                        image["findes_observacoes"] = produto.GetAttributeValue<string>("findes_observacoes");
                    }
                    if (!image.Attributes.Contains("findes_quantidadealunosturma"))
                    {
                        image["findes_quantidadealunosturma"] = produto.GetAttributeValue<int>("findes_quantidadealunosturma");
                    }
                    if (!image.Attributes.Contains("findes_metodologia"))
                    {
                        image["findes_metodologia"] = produto.GetAttributeValue<string>("findes_metodologia");
                    }
                    if (!image.Attributes.Contains("findes_conteudoprogramatico"))
                    {
                        image["findes_conteudoprogramatico"] = produto.GetAttributeValue<string>("findes_conteudoprogramatico");
                    }
                    if (!image.Attributes.Contains("findes_quantidadealunosturma"))
                    {
                        image["findes_quantidadealunosturma"] = produto.GetAttributeValue<int>("findes_quantidadealunosturma");
                    }
                    if (!image.Attributes.Contains("findes_conteudoprogramatico"))
                    {
                        image["findes_conteudoprogramatico"] = produto.GetAttributeValue<string>("findes_conteudoprogramatico");
                    }
                    if (!image.Attributes.Contains("findes_resumoproduto"))
                    {
                        image["findes_resumoproduto"] = produto.GetAttributeValue<string>("description");
                    }
                    if (!image.Attributes.Contains("ispriceoverridden"))
                    {
                        image["ispriceoverridden"] = image.GetAttributeValue<Boolean>("ispriceoverridden");
                    }
                    if (!image.Attributes.Contains("priceperunit"))
                    {
                        image["priceperunit"] = image.GetAttributeValue<Money>("priceperunit");
                    }
                    controller.UpdateQuoteDetail(image);

                }
                
            }
            else
            {
                var todasAsOportunidades = new TodasAsOportunidades(organizationService);

                Entity produto = todasAsOportunidades.GetProdutoPor(request.ProductId);

                var newImage = controller.UpdateDescripton(produto, image.Id);
                Entity updatedImage = newImage;

                controller.UpdateQuoteDetail(updatedImage);

            }

        }


        [TestMethod]
        public void Preencher_Campos_Produto_Cotacao()
        {
            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("quotedetail", new Guid("33E90BDE-8425-E911-A955-000D3AC1BEBA"), new Microsoft.Xrm.Sdk.Query.ColumnSet(
               "productid", "quoteid"));

            Entity preImage = image;

            var request = new PostRequest(image, null, null, preImage);
            var controller = new PostController(organizationService);


           
                var todasAsOportunidades = new TodasAsOportunidades(organizationService);

                Entity produto = todasAsOportunidades.GetProdutoPor(request.ProductId);

                if (produto == null)
                {
                    throw new InvalidPluginExecutionException("Produto não existe.");
                }

                request.PreencherVolumeMinimo(produto.GetAttributeValue<int>("findes_volumeminimo"));
                request.PreencherCargaHoraria(produto.GetAttributeValue<int>("findes_cargahoraria"));
                request.PreencherQuantidadeMinima(produto.GetAttributeValue<int>("findes_quantidademinima"));
                request.PreencherModalidade(produto.GetAttributeValue<OptionSetValue>("findes_modalidade"));
                request.PreencherQuantidadeMaxima(produto.GetAttributeValue<int>("findes_quantidademaxima"));
                request.PreencherTipoProfissional(produto.GetAttributeValue<OptionSetValue>("findes_tipoprofissional"));
                request.PreencherPreRequisito(produto.GetAttributeValue<string>("findes_prerequisitos"));
                request.PreencherResumoProduto(produto.GetAttributeValue<string>("description"));
                request.PreencherObjetivo(produto.GetAttributeValue<string>("findes_objetivo"));
                request.PreencherObservacao(produto.GetAttributeValue<string>("findes_observacao"));
        }
        
    }
}
