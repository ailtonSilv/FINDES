using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins {
    public class PluginHelper
    {

        private IServiceProvider serviceProvider = null;
        private RemoteExecutionContext remoteExecutionContext = null;

        public PluginHelper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public PluginHelper(RemoteExecutionContext context)
        {
            this.remoteExecutionContext = context;
        }

        public Entity ExtractEntity()
        {
            return this.ExtractEntity(null);
        }

        public Entity ExtractEntity(string alias)
        {

            var context = this.GetContext();

            Entity entity = null;

            if (!string.IsNullOrEmpty(alias))
            {

                if (context.PostEntityImages.Contains(alias) && context.PostEntityImages[alias] is Entity)
                    entity = (Entity)context.PostEntityImages[alias];

                else if (context.PreEntityImages.Contains(alias) && context.PreEntityImages[alias] is Entity)
                    entity = (Entity)context.PreEntityImages[alias];

                return entity;
            }

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                entity = (Entity)context.InputParameters["Target"];
            }

            //else { 
            //    entity = (Entity)context.InputParameters["Assignee"];
            //}

            return entity;

        }

        public object ExtractObject(string inputParameterKey)
        {

            var context = this.GetContext();

            object obj = null;

            if (context.InputParameters.Contains(inputParameterKey))
                obj = context.InputParameters[inputParameterKey];

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPluginExecutionContext GetContext()
        {

            if (null == this.remoteExecutionContext)
                return (IPluginExecutionContext)
                    serviceProvider.GetService(typeof(IPluginExecutionContext));

            return this.remoteExecutionContext;

        }

        /// <summary>
        /// Obtém a instância do organizationservice corrente para as operações comuns.
        /// </summary>
        /// <param name="userId">Usuário que será utilizado como owner das operações. este valor poderá ser recuperado através do objeto Context.</param>
        public IOrganizationService GetOrganizationService(Guid? userId)
        {

            var factory = (IOrganizationServiceFactory)
                serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            return factory.CreateOrganizationService(userId);
        }

        /// <summary>
        /// Obtém a instância do organizationservice corrente para as operações comuns.
        /// </summary>
        public IOrganizationService GetOrganizationService()
        {

            var factory = (IOrganizationServiceFactory)
                serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            return factory.CreateOrganizationService(null);
        }

        public ITracingService GetTracingService()
        {
            return (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        }
    }
}