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

namespace findes.crm.workflow
{
	public class ChangeDiscountState : CodeActivity
	{
		protected override void Execute(CodeActivityContext executionContext)
		{
			//Create the tracing service
			ITracingService tracingService = executionContext.GetExtension<ITracingService>();

			//Create the context
			IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
			IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
			IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

			try
			{
				if (discountRequest.Get(executionContext) == null || userReference.Get(executionContext) == null || newState.Get(executionContext) == null)
				{
					throw new InvalidPluginExecutionException("A solicitação de desconto, usuário e ou o novo status de contexto são obrigatórios.");
				}

				string sts = newState.Get(executionContext).ToLower();
				int state = 0;
				int status = 0;
				switch (sts)
				{
					case "aprovar":
						state = 1;
						status = 2;
						break;
					case "reprovar":
						state = 1;
						status = 482870001;
						break;
				}
				Entity desconto = new Entity(discountRequest.Get(executionContext).LogicalName, discountRequest.Get(executionContext).Id);
				desconto["findes_aprovadorid"] = new EntityReference("systemuser", userReference.Get(executionContext).Id);
				service.Update(desconto);

				SetStateRequest request = new SetStateRequest
				{
					EntityMoniker = new EntityReference("findes_solicitacaodedesconto", discountRequest.Get(executionContext).Id),
					State = new OptionSetValue(state),
					Status = new OptionSetValue(status)
				};
				service.Execute(request);
			}
			catch (Exception ex)
			{
				message.Set(executionContext, ex.Message);
			}
			
		}

		[Input("Solicitação")]
		[ReferenceTarget("findes_solicitacaodedesconto")]
		public InArgument<EntityReference> discountRequest { get; set; }

		[Input("Usuário")]
		[ReferenceTarget("systemuser")]
		public InArgument<EntityReference> userReference { get; set; }

		[Input("Novo Status")]
		public InArgument<string> newState { get; set; }

		[Output("Message")]
		public OutArgument<string> message { get; set; }
	}
}
