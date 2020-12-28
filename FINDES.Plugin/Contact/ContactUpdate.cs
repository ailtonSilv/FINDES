using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FINDES.Plugin.CRMFields;

namespace FINDES.Plugin
{
    public class ContactUpdate : IPlugin
    {
        protected static ITracingService tracingService;
        protected static IPluginExecutionContext context;
        protected static IOrganizationService service;
        protected static string ax4bConnectorWebApiURL;

        public ContactUpdate(string unsecureString, string secureString)
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
                PrepareSendContact(entity.Id);
            }
            catch (FaultException<OrganizationServiceFault> ex) {
                tracingService.Trace("An error occurred in the ContactUpdate plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the ContactUpdate plug-in.", ex);
            }
            catch (Exception ex) {
                tracingService.Trace("An error occurred in the ContactUpdate plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the ContactUpdate plug-in.", ex);
            }
        }

        private void PrepareSendContact(Guid contactId)
        {
            var cond1 = new ConditionExpression();
            cond1.AttributeName = "contactid";
            cond1.Operator = ConditionOperator.Equal;
            cond1.Values.Add(contactId);
            var filter1 = new FilterExpression();
            filter1.Conditions.Add(cond1);

            var fields = Fields.Contact.fields;

            var getEntityResultError = "";
            var contact = CRMHelper.getEntity(
                service,
                filter1,
                Fields.Contact.entityName,
                fields.Select(s => s.Key.Replace("(id)", "")).ToArray(),
                ref getEntityResultError);

            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the ContactUpdate plug-in (PostRawJson): " + getEntityResultError);
                return;
            }

            if (!contact.Attributes.ContainsKey("findes_codigoerp") && context.MessageName.ToLower() == "update") return;

            // PREPARA O JSON
            var json = "{";
            json += $"\"entityname\": \"{Fields.Contact.tableSQL}\",";
            json += "\"sendto\": \"SQLServer\",";
            json += "\"records\":[{";

            getEntityResultError = StringHelper.CreateJSONString(contact, fields, ref json, service);
            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the ContactUpdate plug-in: " + getEntityResultError);
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
                tracingService.Trace("An error occurred in the ContactUpdate plug-in (PostRawJson): " + oResult.message);
            }
        }
    }
}
