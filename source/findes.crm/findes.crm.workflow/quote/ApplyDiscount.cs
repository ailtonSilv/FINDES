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
	public class ApplyDiscount : CodeActivity
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

			var quote = service.Retrieve("quote", quoteReference.Get(executionContext).Id, new ColumnSet("customerid", "findes_valorinicialproposta", "findes_valortotaldescontoalcadajaaplicado"));

			var teams = Discount.GetUserTeams(userReference.Get(executionContext).Id, service);

			var discountRule = Discount.GetDiscountRule(teams, service);

			var details = Discount.GetQuoteProducts(quoteReference.Get(executionContext).Id, service);

			var discountSum = 0m;
			var totalSum = 0m;
			foreach (var detail in details)
			{
				discountSum += detail.GetAttributeValue<Money>("findes_valorsomadescontoalcada")?.Value ?? 0m;
				totalSum += detail.GetAttributeValue<Money>("extendedamount")?.Value ?? 0m;
			}
			/*
			// Se a variável de entrada "aproved" vier como TRUE, via WF, atualizar Cotação (Aplica desconto)
			if (aproved.Get(executionContext))
			{
				foreach (var detail in details)
				{
					if (detail.Contains("findes_valortotaldescontoalcada"))
					{
						Entity linhaDesconto = new Entity("findes_linhadedesconto");
						linhaDesconto["findes_name"] = "Desconto Fora da Alçada";
						linhaDesconto["findes_produtopropostaid"] = detail.ToEntityReference(); ;
						linhaDesconto["findes_descontoassociadoid"] = discountRule["findes_descontoassociadoid"];
						linhaDesconto["findes_tipodesconto"] = new OptionSetValue(482870001);
						linhaDesconto["findes_quantidade"] = Convert.ToInt32((decimal)detail["quantity"]);
						linhaDesconto["findes_precounidade"] = new Money(detail.GetAttributeValue<Money>("extendedamount").Value / (int)linhaDesconto["findes_quantidade"]);
						linhaDesconto["findes_percentualdesconto"] = detail.GetAttributeValue<Money>("findes_valortotaldescontoalcada").Value * 100 / detail.GetAttributeValue<Money>("extendedamount").Value;
						linhaDesconto["findes_valordescontoaplicado"] = detail.GetAttributeValue<Money>("findes_valortotaldescontoalcada");
						linhaDesconto["statuscode"] = new OptionSetValue(482870001);
						service.Create(linhaDesconto);

						detail["manualdiscountamount"] = new Money(detail.GetAttributeValue<Money>("manualdiscountamount").Value + detail.GetAttributeValue<Money>("findes_valortotaldescontoalcada").Value);
						detail["findes_valortotaldescontoalcada"] = new Money(0);
						//
						detail.Attributes.Remove("productid");
						detail.Attributes.Remove("quantity");
						service.Update(detail);
					}
				}
			} else {
				*/
				// Se a variável de Entrada "aproved" vier vazia ou false, prosseguir com a validação do desconto.

				var solicitacaoDesconto = new Entity("findes_solicitacaodedesconto");
				solicitacaoDesconto["subject"] = "Solicitação de Desconto";
				solicitacaoDesconto["findes_solicitadoporid"] = userReference.Get(executionContext);
				solicitacaoDesconto["findes_tipodesconto"] = new OptionSetValue(482870001);
				solicitacaoDesconto["regardingobjectid"] = quoteReference.Get(executionContext);
				solicitacaoDesconto["findes_clienteid"] = quote.GetAttributeValue<EntityReference>("customerid");
				solicitacaoDesconto["findes_valortotalproposta"] = new Money(totalSum);
				solicitacaoDesconto["findes_valortotaldescontoaplicado"] = new Money(discountSum);
				solicitacaoDesconto["findes_valortotaldescontopermitido"] = new Money(quote.GetAttributeValue<Money>("findes_valorinicialproposta").Value * discountRule.GetAttributeValue<decimal>("findes_porcentagemdesconto") / 100);

				if (discountSum > quote.GetAttributeValue<Money>("findes_valorinicialproposta").Value * discountRule.GetAttributeValue<decimal>("findes_porcentagemdesconto") / 100)
				{
					var team = service.Retrieve(discountRule.GetAttributeValue<EntityReference>("findes_equipeid").LogicalName, discountRule.GetAttributeValue<EntityReference>("findes_equipeid").Id, new ColumnSet("findes_equipeaprovadoraid"));
					var aproverTeam = service.Retrieve(team.LogicalName, team.GetAttributeValue<EntityReference>("findes_equipeaprovadoraid").Id, new ColumnSet("queueid"));
					solicitacaoDesconto["ownerid"] = aproverTeam.ToEntityReference();
					solicitacaoDesconto.Id = service.Create(solicitacaoDesconto);

					var req = new AddToQueueRequest
					{
						DestinationQueueId = aproverTeam.GetAttributeValue<EntityReference>("queueid").Id,
						Target = solicitacaoDesconto.ToEntityReference()
					};

					service.Execute(req);
					authorized.Set(executionContext, false);
					return;
				}
				else
				{
					solicitacaoDesconto["ownerid"] = discountRule.GetAttributeValue<EntityReference>("findes_equipeid");
					solicitacaoDesconto.Id = service.Create(solicitacaoDesconto);
					solicitacaoDesconto["statecode"] = new OptionSetValue(1);
					solicitacaoDesconto["statuscode"] = new OptionSetValue(482870002);
					service.Update(solicitacaoDesconto);

					foreach (var detail in details)
					{
						if (detail.Contains("findes_valortotaldescontoalcada"))
						{
							Entity linhaDesconto = new Entity("findes_linhadedesconto");
							linhaDesconto["findes_name"] = "Desconto de Alçada";
							linhaDesconto["findes_produtopropostaid"] = detail.ToEntityReference(); ;
							linhaDesconto["findes_descontoassociadoid"] = discountRule["findes_descontoassociadoid"];
							linhaDesconto["findes_tipodesconto"] = new OptionSetValue(482870001);
							linhaDesconto["findes_quantidade"] = Convert.ToInt32((decimal)detail["quantity"]);
							linhaDesconto["findes_precounidade"] = new Money(detail.GetAttributeValue<Money>("extendedamount").Value / (int)linhaDesconto["findes_quantidade"]);
							linhaDesconto["findes_percentualdesconto"] = detail.GetAttributeValue<Money>("findes_valortotaldescontoalcada").Value * 100 / detail.GetAttributeValue<Money>("extendedamount").Value;
							linhaDesconto["findes_valordescontoaplicado"] = detail.GetAttributeValue<Money>("findes_valortotaldescontoalcada");
							linhaDesconto["statuscode"] = new OptionSetValue(482870001);
							service.Create(linhaDesconto);

							detail["manualdiscountamount"] = new Money(detail.GetAttributeValue<Money>("manualdiscountamount").Value + detail.GetAttributeValue<Money>("findes_valortotaldescontoalcada").Value);
							detail["findes_valortotaldescontoalcada"] = new Money(0);
							//
							detail.Attributes.Remove("productid");
							detail.Attributes.Remove("quantity");
							service.Update(detail);
						}
					}
					authorized.Set(executionContext, true);
					return;
				}
			//}

			
		}

		[Input("Cotação")]
		[ReferenceTarget("quote")]
		public InArgument<EntityReference> quoteReference { get; set; }

		[Input("Usuário")]
		[ReferenceTarget("systemuser")]
		public InArgument<EntityReference> userReference { get; set; }

		[Input("Aprovado")]
		public InArgument<bool> aproved { get; set; }

		[Output("Desconto Autorizado")]
		public OutArgument<bool> authorized { get; set; }
	}


}
