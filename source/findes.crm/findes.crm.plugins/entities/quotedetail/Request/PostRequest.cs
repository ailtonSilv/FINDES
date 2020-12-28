using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.quotedetail.Request
{
    public class PostRequest : BaseRequest
    {
        public Guid ProductId { get; set; }

        private ITracingService tracingService = null;

        public PostRequest()
        {
        }

        public PostRequest(Entity imagem, ITracingService tracingService, Entity preImage = null, Entity postImage = null) 
            : base("quotedetail", imagem, preImage, postImage)
        {
            this.tracingService = tracingService;
        }

        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null)
        {
            if (imagem.Attributes.Contains("productid"))
            {
                this.ProductId = ((EntityReference)imagem["productid"]).Id;
            }
        }

        public void PreencherVolumeMinimo(int v)
        {
            base.Imagem.Attributes.Add("findes_volumeminimo", v);
        }

        public void PreencherCargaHoraria(int v)
        {
            base.Imagem.Attributes.Add("findes_cargahoraria", v);
        }

        public void PreencherQuantidadeMinima(int v)
        {
            base.Imagem.Attributes.Add("findes_quantidademinima", v);
        }

        public void PreencherModalidade(OptionSetValue optionSetValue)
        {
            base.Imagem.Attributes.Add("findes_modalidade", optionSetValue);
        }

        public void PreencherQuantidadeMaxima(int v)
        {
            base.Imagem.Attributes.Add("findes_quantidademaxima", v);
        }

        public void PreencherTipoProfissional(OptionSetValue optionSetValue)
        {
            base.Imagem.Attributes.Add("findes_tipoprofissional", optionSetValue);
        }

        public void PreencherPreRequisito(string v)
        {
            base.Imagem.Attributes.Add("findes_prerequisitos", v);
        }

        public void PreencherResumoProduto(string v)
        {
            base.Imagem.Attributes.Add("findes_resumoproduto",v);
        }

        public void PreencherObjetivo(string v)
        {
            base.Imagem.Attributes.Add("findes_objetivo", v);
        }

        public void PreencherObservacao(string v)
        {
            base.Imagem.Attributes.Add("findes_observacao", v);
        }

        //new function

        public void PreencherMetodologia(string v)
        {
            base.Imagem.Attributes.Add("findes_metodologia", v);
        }

        public void PreencherQuantidadeAlunos(int v)
        {
            base.Imagem.Attributes.Add("findes_quantidadealunosturma", v);
        }

        public void PreencherConteudoProgramatico(string v)
        {
            base.Imagem.Attributes.Add("findes_conteudoprogramatico", v);
        }
    }
}
