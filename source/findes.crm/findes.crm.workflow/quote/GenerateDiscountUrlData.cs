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
using findes.crm.workflow.Helper;

namespace findes.crm.workflow.quote
{
	public class GenerateDiscountUrlData : CodeActivity
	{
		protected override void Execute(CodeActivityContext executionContext)
		{
			//Create the tracing service
			ITracingService tracingService = executionContext.GetExtension<ITracingService>();

			//Create the context
			IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
			IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
			IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

			if (quoteReference.Get(executionContext) == null || userReference.Get(executionContext) == null)
			{
				throw new InvalidPluginExecutionException("A cotação e o usuário de contexto são obrigatórios.");
			}
			var quote = service.Retrieve("quote", quoteReference.Get(executionContext).Id, new ColumnSet("totalamount", "findes_valorinicialproposta", "findes_valorpropostaprimeiracriacao"));

			var teams = Discount.GetUserTeams(userReference.Get(executionContext).Id, service);

			var discountRule = Discount.GetDiscountRule(teams, service);

			var details = Discount.GetQuoteProducts(quoteReference.Get(executionContext).Id, service);

			string strAction = "";
			if (actionType.Get<string>(executionContext) != null)
			{
				strAction = actionType.Get<string>(executionContext).ToLower();
			}

			var sb = new StringBuilder();
			var discountTotal = 0m;
			
			for (int i = 0; i < details.Count; i++)
			{
				discountTotal += details[i].GetAttributeValue<Money>("findes_valorsomadescontoalcada")?.Value ?? 0m;
				sb.Append($"&p{i}d={details[i].FormattedValues["productid"]}&p{i}f={details[i].GetAttributeValue<Money>("extendedamount").Value.ToString("0.00")}&p{i}m={details[i].GetAttributeValue<Double?>("findes_margemlucratividade")?.ToString("0.00") ?? "0.00"}&p{i}desc={details[i].GetAttributeValue<Money>("findes_valorsomadescontoalcada")?.Value.ToString("0.00") ?? "0.00"}&p{i}id={details[i].Id}&p{i}descatual={details[i].GetAttributeValue<Money>("findes_valortotaldescontoalcada")?.Value.ToString("0.00") ?? "0.00"}");
			}
			var sbStart = new StringBuilder();
			sbStart.Append($"id={quote.Id}&vo={quote.GetAttributeValue<Money>("findes_valorpropostaprimeiracriacao").Value.ToString()}&tp={quote.GetAttributeValue<Money>("totalamount").Value.ToString("0.00")}&da={(quote.GetAttributeValue<Money>("findes_valorinicialproposta").Value * discountRule.GetAttributeValue<decimal>("findes_porcentagemdesconto") / 100).ToString("0.00")}&dp={discountTotal.ToString("0.00")}&act={strAction}");
			sbStart.Append(sb.ToString());
			discountUrl.Set(executionContext, sbStart.ToString());
		}

		

		[Input("Cotação")]
		[ReferenceTarget("quote")]
		public InArgument<EntityReference> quoteReference { get; set; }

		[Input("Usuário")]
		[ReferenceTarget("systemuser")]
		public InArgument<EntityReference> userReference { get; set; }

		[Input("Ação")]
		public InArgument<string> actionType { get; set; }

		[Output("UrlDesconto")]
		public OutArgument<string> discountUrl { get; set; }
	}
}
