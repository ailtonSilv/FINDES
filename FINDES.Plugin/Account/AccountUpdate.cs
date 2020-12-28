using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FINDES.Plugin.CRMFields;

namespace FINDES.Plugin
{
    public class AccountUpdate : IPlugin
    {
        protected static ITracingService tracingService;
        protected static IPluginExecutionContext context;
        protected static IOrganizationService service;
        protected static string ax4bConnectorWebApiURL;

        public AccountUpdate(string unsecureString, string secureString)
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
                PrepareSendAccount(entity.Id);
            }
            catch (FaultException<OrganizationServiceFault> ex) {
                tracingService.Trace("An error occurred in the AccountUpdate plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the AccountUpdate plug-in.", ex);
            }
            catch (Exception ex) {
                tracingService.Trace("An error occurred in the AccountUpdate plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the AccountUpdate plug-in.", ex);
            }
        }

        private void PrepareSendAccount(Guid accountId)
        {
            var cond1 = new ConditionExpression();
            cond1.AttributeName = "accountid";
            cond1.Operator = ConditionOperator.Equal;
            cond1.Values.Add(accountId);
            var filter1 = new FilterExpression();
            filter1.Conditions.Add(cond1);

            var fields = Fields.Account.fields;

            var getEntityResultError = "";
            var account = CRMHelper.getEntity(
                service,
                filter1,
                Fields.Account.entityName,
                fields.Select(s => s.Key.Replace("(id)", "")).ToArray(),
                ref getEntityResultError);

            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the AccountUpdate plug-in (PostRawJson): " + getEntityResultError);
                return;
            }

            if (!account.Attributes.ContainsKey("findes_codigoerp") && context.MessageName.ToLower() == "update") return;

            // PREPARA O JSON
            var json = "{";
            json += $"\"entityname\": \"{Fields.Account.tableSQL}\",";
            json += "\"sendto\": \"SQLServer\",";
            json += "\"records\":[{";

            getEntityResultError = StringHelper.CreateJSONString(account, fields, ref json, service);
            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the AccountUpdate plug-in: " + getEntityResultError);
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
                tracingService.Trace("An error occurred in the AccountUpdate plug-in (PostRawJson): " + oResult.message);
            }
        }
    }
}
