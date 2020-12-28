using FINDES.Plugin.CRMFields;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace FINDES.Plugin
{
    public class Product : IPlugin
    {
        protected static ITracingService tracingService;
        protected static IPluginExecutionContext context;
        protected static IOrganizationService service;
        protected static string ax4bConnectorWebApiURL;

        public Product(string unsecureString, string secureString)
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

                var entityId = Guid.Empty;
                var state = 0;
                var status = 1;

                if (context.InputParameters.ContainsKey("Target") && context.InputParameters["Target"] is Entity) {
                    entityId = ((Entity)context.InputParameters["Target"]).Id;
                } else if (context.InputParameters.ContainsKey("EntityMoniker") && context.InputParameters["EntityMoniker"] is EntityReference) {
                    state = ((OptionSetValue)context.InputParameters["State"]).Value;
                    status = ((OptionSetValue)context.InputParameters["Status"]).Value;
                    if (state.Equals(0) && status.Equals(1)) {
                        entityId = ((EntityReference)context.InputParameters["EntityMoniker"]).Id;
                    }
                }

                Entity preImage = null;
                if (context.PreEntityImages.Contains("preimage") && context.PreEntityImages["preimage"] is Entity)
                {
                    preImage = context.PreEntityImages["preimage"];
                }
                

                if (preImage != null)
                {
                    tracingService.Trace("productnumber: '" + preImage.GetAttributeValue<string>("productnumber").ToString() + "'");
                }
                
                if (entityId == Guid.Empty) return;

                PrepareSendProduct(entityId);
            }
            catch (FaultException<OrganizationServiceFault> ex) {
                tracingService.Trace("An error occurred in the Product plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the Product plug-in.", ex);
            }
            catch (Exception ex) {
                tracingService.Trace("An error occurred in the Product plug-in: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in the Product plug-in.", ex);
            }
        }

        private void PrepareSendProduct(Guid productId)
        {
            var cond1 = new ConditionExpression();
            cond1.AttributeName = "productid";
            cond1.Operator = ConditionOperator.Equal;
            cond1.Values.Add(productId);
            var filter1 = new FilterExpression();
            filter1.Conditions.Add(cond1);

            var fields = Fields.Product.fields;

            var getEntityResultError = "";
            var product = CRMHelper.getEntity(
                service,
                filter1,
                Fields.Product.entityName,
                fields.Select(s => s.Key.Replace("(id)", "")).ToArray(),
                ref getEntityResultError);

            var revisedStatus = (product.GetAttributeValue<OptionSetValue>("statuscode").Value.Equals(3) && product.GetAttributeValue<OptionSetValue>("statecode").Value.Equals(3));
            var draftStatus = (product.GetAttributeValue<OptionSetValue>("statuscode").Value.Equals(0) && product.GetAttributeValue<OptionSetValue>("statecode").Value.Equals(2));
            if (revisedStatus || draftStatus) return;

            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the product plug-in (PostRawJson): " + getEntityResultError);
                return;
            }

            // PREPARA O JSON
            var json = "{";
            json += $"\"entityname\": \"{Fields.Product.tableSQL}\",";
            json += "\"sendto\": \"SQLServer\",";
            json += "\"records\":[{";

            getEntityResultError = StringHelper.CreateJSONString(product, fields, ref json, service);
            if (!string.IsNullOrEmpty(getEntityResultError)) {
                tracingService.Trace("An error occurred in the product plug-in: " + getEntityResultError);
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
                tracingService.Trace("An error occurred in the Product plug-in (PostRawJson): " + oResult.message);
            }
        }
    }
}
