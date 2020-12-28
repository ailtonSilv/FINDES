using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins {

    public abstract class BaseRequest {

        protected Entity Imagem = null;

        internal Guid EntityId { get; set; } = Guid.Empty;

        protected BaseRequest() {

        }

        protected BaseRequest(string nomeDaEntidade, Entity imagem, Entity preImage = null, Entity postImage = null) {

            this.Imagem = imagem ??
                throw new InvalidPluginExecutionException(OperationStatus.Canceled, "Plugin: Não foi encontrada uma imagem válida para esta operação..");

            if (string.IsNullOrEmpty(imagem.LogicalName) || imagem.LogicalName != nomeDaEntidade)
                throw new InvalidPluginExecutionException(OperationStatus.Canceled, "O Nome da Imagem é inválido ou Nulo.");


            this.EntityId = imagem.Id;

            this.Begin(imagem, preImage, postImage);
        }

        protected abstract void Begin(Entity imagem, Entity preImagem = null, Entity postImage = null);
        internal virtual void End() {
        }
    }
}
