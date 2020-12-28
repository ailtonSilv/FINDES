using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using findes.crm.plugins.entities.quotedetail.Request;
using findes.crm.plugins.entities.quotedetail.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;

namespace findes.crm.plugins.entities.quotedetail
{
    public class PreCreate : BasePlugin
    {
        public PreCreate(string unsecure = "") : base(unsecure)
        {
        }

        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity();
            Entity preImage = helper.ExtractEntity("pre_image");

            var request = new Request.PostRequest(image, helper.GetTracingService(), preImage, null);
            var controller = new Controller.PostController(helper.GetOrganizationService());


            

                if (request.ProductId != Guid.Empty)
                {
                    var todasAsOportunidades = new TodasAsOportunidades(helper.GetOrganizationService());

                    Entity produto = todasAsOportunidades.GetProdutoPor(request.ProductId);

                    if (produto == null)
                    {
                        throw new InvalidPluginExecutionException("Produto não existe");
                    }

                    if (!image.Attributes.Contains("description"))
                    {
                        request.PreencherResumoProduto(produto.GetAttributeValue<string>("description"));
                    }
                    if (!image.Attributes.Contains("findes_volumeminimo"))
                    {
                        request.PreencherVolumeMinimo(produto.GetAttributeValue<int>("findes_volumeminimo"));
                    }
                    if (!image.Attributes.Contains("findes_cargahoraria"))
                    {
                        request.PreencherCargaHoraria(produto.GetAttributeValue<int>("findes_cargahoraria"));
                    }
                    if (!image.Attributes.Contains("findes_quantidademinima"))
                    {
                        request.PreencherQuantidadeMinima(produto.GetAttributeValue<int>("findes_quantidademinima"));
                    }
                    if (!image.Attributes.Contains("findes_modalidade"))
                    {
                        request.PreencherModalidade(produto.GetAttributeValue<OptionSetValue>("findes_modalidade"));
                    }
                    if (!image.Attributes.Contains("findes_quantidademaxima"))
                    {
                        request.PreencherQuantidadeMaxima(produto.GetAttributeValue<int>("findes_quantidademaxima"));
                    }
                    if (!image.Attributes.Contains("findes_tipoprofissional"))
                    {
                        request.PreencherTipoProfissional(produto.GetAttributeValue<OptionSetValue>("findes_tipoprofissional"));
                    }
                    if (!image.Attributes.Contains("findes_prerequisitos"))
                    {
                        request.PreencherPreRequisito(produto.GetAttributeValue<string>("findes_prerequisitos"));
                    }
                    if (!image.Attributes.Contains("findes_objetivo"))
                    {
                        request.PreencherObjetivo(produto.GetAttributeValue<string>("findes_objetivo"));
                    }
                    if (!image.Attributes.Contains("findes_observacao"))
                    {
                        request.PreencherObservacao(produto.GetAttributeValue<string>("findes_observacao"));
                    }
                    //new fields
                    if (!image.Attributes.Contains("findes_quantidadealunosturma"))
                    {
                        request.PreencherQuantidadeAlunos(produto.GetAttributeValue<int>("findes_quantidadealunosturma"));
                    }
                    if (!image.Attributes.Contains("findes_metodologia"))
                    {
                        request.PreencherMetodologia(produto.GetAttributeValue<string>("findes_metodologia"));
                    }
                    if (!image.Attributes.Contains("findes_conteudoprogramatico"))
                    {
                        request.PreencherConteudoProgramatico(produto.GetAttributeValue<string>("findes_conteudoprogramatico"));
                    }

                }

            }
        }
    }

