using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;

namespace findes.crm.plugins.WorkFlowAssembly
{
    public class CreateAutoTasks : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (taskConfig.Get(executionContext) == null)
            {
                throw new InvalidPluginExecutionException("O parâmetro de configuração de lembrete é obrigatório.");
            }

            var config = service.Retrieve(taskConfig.Get(executionContext).LogicalName, taskConfig.Get(executionContext).Id, new ColumnSet("findes_tipotarefa", "findes_segmentacao", "findes_periodotarefa"));
            var segment = service.Retrieve(config.GetAttributeValue<EntityReference>("findes_segmentacao").LogicalName, config.GetAttributeValue<EntityReference>("findes_segmentacao").Id, new ColumnSet("createdfromcode", "type", "query"));
            if (segment.GetAttributeValue<OptionSetValue>("createdfromcode").Value != 1)
            {
                throw new InvalidPluginExecutionException("A segmentação deve ser voltada para contas.");
            }
            List<Entity> accounts = new List<Entity>();
            if (segment.GetAttributeValue<bool>("type")) //Dynamic List
            {
                FetchXmlToQueryExpressionRequest req = new FetchXmlToQueryExpressionRequest { FetchXml = segment.GetAttributeValue<string>("query") };
                var resp = (FetchXmlToQueryExpressionResponse)service.Execute(req);
                resp.Query.ColumnSet = new ColumnSet("name", "ownerid");
                var ret = service.RetrieveMultiple(resp.Query);
                accounts.AddRange(ret.Entities);
                while (ret.MoreRecords)
                {
                    resp.Query.PageInfo.PageNumber++;
                    resp.Query.PageInfo.PagingCookie = ret.PagingCookie;

                    ret = service.RetrieveMultiple(resp.Query);
                    accounts.AddRange(ret.Entities);
                }
            }
            else
            {
                var query = new QueryExpression("account");
                query.ColumnSet = new ColumnSet("name", "ownerid");
                var listMemberLink = query.AddLink("listmember", "accountid", "entityid");
                listMemberLink.EntityAlias = "lm";
                var listLink = listMemberLink.AddLink("list", "listid", "listid");
                listLink.LinkCriteria.AddCondition("listid", ConditionOperator.Equal, segment.Id);
                listLink.EntityAlias = "list";

                var ret = service.RetrieveMultiple(query);
                accounts.AddRange(ret.Entities);
                while (ret.MoreRecords)
                {
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = ret.PagingCookie;

                    ret = service.RetrieveMultiple(query);
                    accounts.AddRange(ret.Entities);
                }
            }

            foreach (var account in accounts)
            {
                var tempTask = new Entity("task");
                tempTask["findes_tipotarefa"] = config["findes_tipotarefa"];
                var tempName = config.FormattedValues["findes_tipotarefa"] + " - " + account.GetAttributeValue<string>("name");
                tempTask["subject"] = tempName.Substring(0, tempName.Length < 200 ? tempName.Length : 200);
                tempTask["regardingobjectid"] = account.ToEntityReference();
                tempTask["actualdurationminutes"] = config["findes_periodotarefa"];
                tempTask["prioritycode"] = new OptionSetValue(2);
                tempTask["ownerid"] = account["ownerid"];
                tempTask["scheduledend"] = DateTime.UtcNow.AddMinutes(config.GetAttributeValue<int?>("findes_periodotarefa") ?? 0);

                service.Create(tempTask);
            }
            config["findes_datageracaotarefa"] = DateTime.UtcNow;
            config["findes_dataproximageracao"] = DateTime.UtcNow.AddMinutes(config.GetAttributeValue<int>("findes_periodotarefa"));
            service.Update(config);
        }

        [Input("Configuração de lembrete")]
        [ReferenceTarget("findes_configuracaolembrete")]
        public InArgument<EntityReference> taskConfig { get; set; }
    }
}
