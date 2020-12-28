using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.quote.Request
{
    public class PostCreateRequest : BaseRequest
    {
        public Guid quoteId { get; set; }

        protected PostCreateRequest() { }

        private ITracingService tracingService = null;


        public PostCreateRequest(Entity imagem, ITracingService tracingService, Entity preImage = null, Entity postImage = null)
          : base("quote", imagem, preImage, postImage)
        {

            this.tracingService = tracingService;

        }


        protected override void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null)
        {
            if (imagem.Attributes.Contains("quoteid"))
            {
                this.quoteId = ((Guid)postImage["quoteid"]);
            }
        }
    }
}
