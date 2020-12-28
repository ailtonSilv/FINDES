using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.salesorder.Request
{
    public class PostCreateRequest : BaseRequest
    {
        public Guid SalesOrderID { get; set; }

        protected PostCreateRequest() { }

        
        private ITracingService tracingService = null;

        public PostCreateRequest(Entity imagem, ITracingService tracingService, Entity preImage = null, Entity postImage = null)
          : base("salesorder", imagem, preImage, postImage)
        {

            this.tracingService = tracingService;

        }


        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null)
        {
            if (imagem.Attributes.Contains("salesorderid"))
            {
                this.SalesOrderID = ((Guid)postImage["salesorderid"]);
            }
        }
    }
}
