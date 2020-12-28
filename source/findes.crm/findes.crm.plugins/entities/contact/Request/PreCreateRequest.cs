using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.contact.request {
    public class PreCreateRequest : BaseRequest {

        protected PreCreateRequest() {
        }

        public PreCreateRequest(Entity imagem, Entity preImage = null, Entity postImage = null)
            : base("contact", imagem, preImage, postImage) {
        }

        public string Cpf { get; private set; } = "";
        public OptionSetValue TipoContato { get; private set; }
        public string name { get; private set; } = "";

        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null) {

           
            if (imagem.GetAttributeValue<string>("findes_cpf")!=null)
            {
                this.Cpf = imagem["findes_cpf"].ToString();
            }

            if (imagem.GetAttributeValue<OptionSetValue>("findes_tipocontato")!=null)
            {
                this.TipoContato = imagem.GetAttributeValue<OptionSetValue>("findes_tipocontato");
            }

            if (imagem.GetAttributeValue<string>("firstname") != null)
            {
                this.name = imagem["firstname"].ToString();
            }
            
        }

    }
}
