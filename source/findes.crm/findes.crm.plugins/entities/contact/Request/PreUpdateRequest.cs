using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.contact.Request {

    public class PreUpdateRequest : BaseRequest {

        public Guid EstadoId { get; set; } = Guid.Empty;
        public bool FoiInformadoOEstado { get; private set; } = false;

        public Guid CidadeId { get; set; } = Guid.Empty;
        public bool FoiInformadaACidade { get; private set; } = false;

        public string Cpf { get; private set; } = "";

        protected PreUpdateRequest() {
        }

        public PreUpdateRequest(Entity imagem, Entity preImage = null, Entity postImage = null) : 
            base("contact", imagem, preImage, postImage) {
        }

        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null) {

            if (imagem.Attributes.Contains("findes_estadoprincipalid")) {
                this.EstadoId = ((EntityReference)imagem["findes_estadoprincipalid"]).Id;
                this.FoiInformadoOEstado = true;
            }

            if (imagem.Attributes.Contains("findes_municipioprincipalid")) {
                this.CidadeId = ((EntityReference)imagem["findes_municipioprincipalid"]).Id;
                this.FoiInformadaACidade = true;
            }

            if (imagem.GetAttributeValue<string>("findes_cpf") != null)
            {
                this.Cpf = imagem["findes_cpf"].ToString();
            }

        }

        internal void PreencherNomeDoEstado(string nome) {
            this.Imagem.Attributes.Add("address1_stateorprovince", nome);
        }

        internal void PreencherNomeDaCidade(string nome) {
            this.Imagem.Attributes.Add("address1_city", nome);
        }

    }
}
