using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.opportunityproduct.Request
{
    public class PreCreateRequest : BaseRequest
    {
        public Guid ProdutoId { get; set; }
     
        private ITracingService tracingService = null;

        public PreCreateRequest()
        {
        }

        public PreCreateRequest(Entity imagem, ITracingService tracingService, Entity preImage, Entity postImage = null) 
            : base("opportunityproduct", imagem, preImage, postImage)
        {
            this.tracingService = tracingService;
        }

        protected override void Begin(Entity imagem, Entity preImagem, Entity postImage = null)
        {
            if (imagem.Attributes.Contains("productid"))
            {
                this.ProdutoId = ((EntityReference)imagem["productid"]).Id;
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
            base.Imagem.Attributes.Add("findes_resumoproduto", v);
        }

        public void PreencherObjetivo(string v)
        {
            base.Imagem.Attributes.Add("findes_objetivo", v);
        }

        public void PreencherObservacoes(string v)
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
