using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins
{

    public abstract class BasePlugin : IPlugin
    {

        protected ITracingService Tracing = null;
        protected string unsecureParameter = "";
        protected IOrganizationService CrmService = null;
        protected Entity Image = null;

        private Entity preImage = null;

        protected Entity PreImage {
            get {

                if (null == this.preImage)
                    this.preImage = new Entity();

                return this.preImage;
            }
            private set {
                this.preImage = value;
            }
        }

        private Entity postImage = null;

        protected Entity PostImage {
            get {

                if (null == this.postImage)
                    this.postImage = new Entity();

                return this.postImage;
            }
            private set {
                this.postImage = value;
            }
        }


        public BasePlugin(string unsecure = "")
        {

            if (!string.IsNullOrEmpty(unsecure))
                this.unsecureParameter = unsecure.Trim();
        }

        public void Execute(IServiceProvider serviceProvider)
        {

            var pluginManager = new PluginHelper(serviceProvider);
            this.Tracing = pluginManager.GetTracingService();
            this.CrmService = pluginManager.GetOrganizationService();
            this.Image = pluginManager.ExtractEntity();
            this.PostImage = pluginManager.ExtractEntity("post_image");
            this.PreImage = pluginManager.ExtractEntity("pre_image");

            try
            {

                this.Execute(pluginManager);

            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> erro)
            {
                Tracing.Trace("[ERRO - FaultException] - {0}\n[Stack] - {1}", erro.Message, erro.StackTrace);
                throw new InvalidPluginExecutionException(string.Format("Ocorreu um erro inesperado ao consultar o Serviço do CRM.\n{0}", erro.Message)) ;
            }
            catch (InvalidPluginExecutionException erro)
            {
                Tracing.Trace("[ERRO - InvalidPluginExecutionException] - {0}\n[Stack] - {1}", erro.Message, erro.StackTrace);
                throw erro;
            }
            catch (ArgumentNullException erro)
            {
                Tracing.Trace("[ERRO - ArgumentNullException] - {0}\n[Stack] - {1}", erro.Message, erro.StackTrace);
                throw new InvalidPluginExecutionException(erro.Message);
            }
            catch (Exception erro)
            {
                Tracing.Trace("[ERRO - Exception] - {0}\n[Stack] - {1}", erro.Message, erro.StackTrace);
                throw new InvalidPluginExecutionException("Ocorreu um erro inesperado ao executar esta operação. " + erro.Message);
            }
        }


        protected abstract void Execute(PluginHelper helper);

    }
}
