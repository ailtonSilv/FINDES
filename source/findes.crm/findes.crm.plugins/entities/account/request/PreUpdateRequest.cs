using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.account.request
{
    public class PreUpdateRequest : BaseRequest
    {
        public Guid EstadoId { get; set; } = Guid.Empty;
        public bool FoiInformadoOEstado { get; set; } = false;

        public Guid CidadeId { get; set; } = Guid.Empty;
        public bool FoiInformadaACidade { get; set; } = false;

        protected PreUpdateRequest()
        {

        }

        public PreUpdateRequest(Entity image, Entity preImage = null, Entity postImage = null) : base("account", image, preImage, postImage)
        {

        }
        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null)
        {
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
        }

        internal void PreencherNomeDoEstado(string nome)
        {
            if (this.Imagem.Attributes.Contains("address1_stateorprovince"))
            {
                this.Imagem["address1_stateorprovince"] = nome;
            }
            else
            {
                this.Imagem.Attributes.Add("address1_stateorprovince", nome);
            }
        }

        internal void PreencherNomeDaCidade(string nome)
        {
            if (this.Imagem.Attributes.Contains("address1_city"))
            {
                this.Imagem["address1_city"] = nome;
            }
            else
            {
                this.Imagem.Attributes.Add("address1_city", nome);
            }

        }
    }
}
