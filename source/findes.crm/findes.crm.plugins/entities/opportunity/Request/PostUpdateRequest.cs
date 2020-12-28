using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.opportunity.Request {

    public class PostUpdateRequest : BaseRequest {

        public bool EhPraGerarProposta { get; private set; } = false;
        public string NomeDaOportunidade { get; private set; }
        public Guid Coligada { get; set; }
        public Guid ContaDePagamentoId { get; private set; }
        public Guid ContaDeServicoId { get; private set; }
        public Guid ListaDePrecoId { get; private set; }
        public Guid ContatoDeServicoId { get; private set; }
        public int OrigemDaOportunidade { get; private set; }
        public Guid Proprietario { get; set; }
        public Boolean BaseNacional { get; private set; }
        public OptionSetValue PapelDoRegional { get; private set; }
        public OptionSetValueCollection drsEnvolvidas { get; private set; } 
        public Guid UnidadeCordenadora { get; private set; }
       


        private ITracingService tracingService = null;

        protected PostUpdateRequest() {
        }

        public PostUpdateRequest(Entity imagem, ITracingService tracingService, Entity preImage = null, Entity postImage = null) 
            : base("opportunity", imagem, preImage, postImage) {

            this.tracingService = tracingService;

        }

        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null) {


            if (imagem.Attributes.Contains("findes_gerarproposta")) {

                this.EhPraGerarProposta = (bool)imagem["findes_gerarproposta"];
                imagem.Attributes.Remove("findes_gerarproposta");

                if (null == postImage)
                    throw new ArgumentNullException("title", "post image null");

                this.Proprietario = ((EntityReference)postImage["ownerid"]).Id;

                this.NomeDaOportunidade = postImage["name"].ToString();

                this.ContaDePagamentoId = ((EntityReference)postImage["customerid"]).Id;

                if (postImage.GetAttributeValue<EntityReference>("parentaccountid") != null)
                {
                    this.ContaDeServicoId = ((EntityReference)postImage["parentaccountid"]).Id;
                }

                if (postImage.Attributes.Contains("findes_unidadecoordenadoraid"))
                {
                    this.UnidadeCordenadora = ((EntityReference)postImage["findes_unidadecoordenadoraid"]).Id;
                }

                if (postImage.Attributes.Contains("findes_drsenvolvidas"))
                {
                    this.drsEnvolvidas = (OptionSetValueCollection)postImage["findes_drsenvolvidas"];
                }

                if (postImage.Attributes.Contains("findes_basenacional"))
                {
                    this.BaseNacional = postImage.GetAttributeValue<Boolean>("findes_basenacional");
                }

                if (postImage.Attributes.Contains("findes_papelregional"))
                {
                    this.PapelDoRegional = postImage.GetAttributeValue<OptionSetValue>("findes_papelregional");
                }

                if (postImage.Attributes.Contains("pricelevelid"))
                    this.ListaDePrecoId = ((EntityReference)postImage["pricelevelid"]).Id;

                if (postImage.GetAttributeValue<EntityReference>("parentcontactid") != null)
                {
                    this.ContatoDeServicoId = ((EntityReference)postImage["parentcontactid"]).Id;
                }
                
                this.OrigemDaOportunidade = ((OptionSetValue)postImage["findes_origemoportunidade"]).Value;

            }
        }
    }
}
