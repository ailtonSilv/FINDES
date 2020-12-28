using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.salesorderproduct.Request
{
    public class PreCreateRequest : BaseRequest
    {
        public Guid Product { get; set; }

        private ITracingService tracingService = null;

        public PreCreateRequest() { }

        public PreCreateRequest(Entity imagem, ITracingService tracingService, Entity preImage = null, Entity postImage = null)
            : base("salesorderdetail", imagem, preImage, postImage)
        {
            this.tracingService = tracingService;
        }
        
        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null)
        {
            if (imagem.Attributes.Contains("productid"))
            {
                this.Product = ((EntityReference)imagem["productid"]).Id;
            }
        }

        public void setColigada(EntityReference coligada)
        {
            base.Imagem.Attributes.Add("findes_coligadaid", coligada);
        }

        public void setSolution(EntityReference solution)
        {
            base.Imagem.Attributes.Add("findes_solucaoid", solution);
        }

        public void setLineOfActin(EntityReference LineOfActin)
        {
            base.Imagem.Attributes.Add("findes_linhaatuacaoid", LineOfActin);
        }

        public void setCategory(EntityReference category)
        {
            base.Imagem.Attributes.Add("findes_categoriaid", category);
        }

        public void setProduct(EntityReference productId)
        {
            base.Imagem.Attributes.Add("productid", productId);
        }

        public void setBranchProduct(EntityReference Product)
        {
            base.Imagem.Attributes.Add("findes_produtofilialid", Product);
        }

        public void setUnity(EntityReference unity)
        {
            base.Imagem.Attributes.Add("uomid", unity);
        }

        public void setExecutingUnit(EntityReference ExecutingUnit)
        {
            base.Imagem.Attributes.Add("findes_unidadeexecutoraid", ExecutingUnit);
        }
    } 
    
    
}
