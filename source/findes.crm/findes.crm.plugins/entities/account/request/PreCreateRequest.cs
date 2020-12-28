using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.account.request {
    public class PreCreateRequest : BaseRequest {

        protected PreCreateRequest() {
        }

        public PreCreateRequest(Entity imagem, Entity preImage = null, Entity postImage = null) 
            : base("account", imagem, preImage, postImage) {
        }

        public string Cnpj { get; private set; } = "";

        public Guid EstadoId { get; set; } = Guid.Empty;
        public bool FoiInformadoOEstado { get; set; } = false;

        public Guid CidadeId { get; set; } = Guid.Empty;
        public bool FoiInformadaACidade { get; set; } = false;

        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null) {
            
            if (!imagem.Attributes.Contains("findes_cnpj")) {
                throw new InvalidPluginExecutionException(OperationStatus.Failed, "O CNPJ não foi informado.");
            }

            if (imagem.Attributes.Contains("findes_estadoprincipalid"))
            {
                this.EstadoId = ((EntityReference)imagem["findes_estadoprincipalid"]).Id;
                this.FoiInformadoOEstado = true;
            }

            if (imagem.Attributes.Contains("findes_municipioprincipalid"))
            {
                this.CidadeId = ((EntityReference)imagem["findes_municipioprincipalid"]).Id;
                this.FoiInformadaACidade = true;
            }

            this.Cnpj = imagem["findes_cnpj"].ToString();
        }

        internal void PreencherNomeDoEstado(string nome)
        {
            this.Imagem.Attributes.Add("address1_stateorprovince", nome);
        }

        internal void PreencherNomeDaCidade(string nome)
        {
            this.Imagem.Attributes.Add("address1_city", nome);
        }
    }
}
