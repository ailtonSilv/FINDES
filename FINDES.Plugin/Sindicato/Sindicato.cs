using FINDES.Plugin.CRMFields;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace FINDES.Plugin
{
    public class Sindicato : IPlugin
    {
        protected static ITracingService tracingService;
        protected static IPluginExecutionContext context;
        protected static IOrganizationService service;
        protected static string ax4bConnectorWebApiURL;

        public Sindicato(string unsecureString, string secureString)
        {
            ax4bConnectorWebApiURL = unsecureString;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            try {
                tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                service = serviceFactory.CreateOrganizationService(context.UserId);

                var entity = (Entity)context.InputParameters["Target"];
                PrepareSendSindicato(entity.Id);
            }
            catch (FaultException<OrganizationServiceFault> ex) {
                tracingService.Trace("An error occurred in the Sindicato plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the Sindicato plug-in.", ex);
            }
            catch (Exception ex) {
                tracingService.Trace("An error occurred in the Sindicato plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the Sindicato plug-in.", ex);
            }
        }

        private void PrepareSendSindicato(Guid findes_sindicatoid)
        {
            var cond1 = new ConditionExpression();
            cond1.AttributeName = "findes_sindicatoid";
            cond1.Operator = ConditionOperator.Equal;
            cond1.Values.Add(findes_sindicatoid);
            var filter1 = new FilterExpression();
            filter1.Conditions.Add(cond1);

            var fields = Fields.Sindicato.fields;

            var getEntityResultError = "";
            var sindicato = CRMHelper.getEntity(
                service,
                filter1,
                Fields.Sindicato.entityName,
                fields.Select(s => s.Key.Replace("(id)", "")).ToArray(),
                ref getEntityResultError);

            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the Sindicato plug-in (PostRawJson): " + getEntityResultError);
                return;
            }

            // PREPARA O JSON
            var json = "{";
            json += $"\"entityname\": \"{Fields.Sindicato.tableSQL}\",";
            json += "\"sendto\": \"SQLServer\",";
            json += "\"records\":[{";

            getEntityResultError = StringHelper.CreateJSONString(sindicato, fields, ref json, service);
            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the Sindicato plug-in: " + getEntityResultError);
                return;
            }

            json += "\"findes_dataatualizacao\":{";
            json += "\"type\":\"datetime\",";
            json += "\"value\":\"" + DateTime.Now.AddHours(-2).ToString("yyyy/MM/dd HH:mm:ss") + "\"";
            json += "}";
            json += "}]";
            json += "}";

            // ENVIA PARA O STAGING
            var result = RESTHelper.PostRawJson(ax4bConnectorWebApiURL, json);
            var oResult = JsonHelper<ResultModel>.DeSerialize(result);

            if (!string.IsNullOrEmpty(oResult.status) && oResult.status != "Success") {
                tracingService.Trace("An error occurred in the Sindicato plug-in (PostRawJson): " + oResult.message);
            }
        }
    }
}
