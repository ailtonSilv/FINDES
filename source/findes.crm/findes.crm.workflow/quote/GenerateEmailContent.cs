using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;
using System.Globalization;

namespace findes.crm.workflow.quote
{
    public class GenerateEmailContent : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext) {

            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (QuoteReference.Get(executionContext) == null || SolicitacaoReference.Get(executionContext) == null || AccountReference.Get(executionContext) == null) {

                throw new InvalidPluginExecutionException("A Solicitação, Cotação e Conta são parâmetros obrigatórios.");
            }

            var Cliente = service.Retrieve("account", AccountReference.Get(executionContext).Id, new ColumnSet("findes_razaosocial"));
            var Proposta = service.Retrieve("quote", QuoteReference.Get(executionContext).Id, new ColumnSet("findes_valortotalsemdesconto", "totalamount"));
            var Solicitacao = service.Retrieve("findes_solicitacaodedesconto", SolicitacaoReference.Get(executionContext).Id, new ColumnSet("findes_valortotaldescontoaplicado"));
            var PercentualDesconto = (Solicitacao.GetAttributeValue<Money>("findes_valortotaldescontoaplicado").Value * 100)/Proposta.GetAttributeValue<Money>("findes_valortotalsemdesconto").Value;
                                 
            // Email Header
            var strHtmlHeader = new StringBuilder();
            strHtmlHeader.Append("<table style='border: 1px solid black; border-collapse: collapse; width: 100%;'>");
            strHtmlHeader.Append("<thead style='text-align: center;'>");
            strHtmlHeader.Append("<tr>");
            strHtmlHeader.Append("<th style='border: 1px solid black; padding: 6px;'>Cliente</th>");
            strHtmlHeader.Append("<th style='border: 1px solid black; padding: 6px;'>Valor Original da Proposta</th>");
            strHtmlHeader.Append("<th style='border: 1px solid black; padding: 6px;'>Valor Total de Desconto Solicitado</th>");
            strHtmlHeader.Append("<th style='border: 1px solid black; padding: 6px;'>Percentual de Desconto Solicitado</th>");
            strHtmlHeader.Append("<th style='border: 1px solid black; padding: 6px;'>Valor Final da Proposta</th>");
            strHtmlHeader.Append("</tr>");
            strHtmlHeader.Append("<tbody style='text-align: center;'></thead>");


            decimal valorComDesconto = Proposta.GetAttributeValue<Money>("findes_valortotalsemdesconto").Value - Solicitacao.GetAttributeValue<Money>("findes_valortotaldescontoaplicado").Value;

            strHtmlHeader.Append("<tr>");
            strHtmlHeader.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", Cliente.GetAttributeValue<string>("findes_razaosocial")));
            strHtmlHeader.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", Proposta.GetAttributeValue<Money>("findes_valortotalsemdesconto").Value.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))));
            strHtmlHeader.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", Solicitacao.GetAttributeValue<Money>("findes_valortotaldescontoaplicado").Value.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))));
            strHtmlHeader.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}%</td>", PercentualDesconto.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))));
            strHtmlHeader.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", valorComDesconto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))));
            strHtmlHeader.Append("</tr>");

            EmailHeader.Set(executionContext, strHtmlHeader.ToString());

            // Email products
            var query = new QueryExpression("quotedetail");
            query.Criteria.AddCondition("quoteid", ConditionOperator.Equal, QuoteReference.Get(executionContext).Id);
            query.ColumnSet = new ColumnSet(
                "findes_coligadaid",
                "findes_solucaoid",
                "quantity",
                "baseamount",
                "findes_valortotaldescontoalcada"
                );

            var quoteDetails = service.RetrieveMultiple(query);

            var strHtmlProducts = new StringBuilder(); 
            strHtmlProducts.Append("<table style='border-collapse: collapse; width: 100%;'>");
            strHtmlProducts.Append("<thead style='text-align: center;'>");
            strHtmlProducts.Append("<tr><td style='height: 30px;'></td></tr><tr><td style='text-align: left;'><b>Itens Proposta: </b></td>");
            strHtmlProducts.Append("</tr><tr><td style='height: 30px;'></td></tr>");

            strHtmlProducts.Append("<tr>");
            strHtmlProducts.Append("<th style='border: 1px solid black; padding: 6px;'>Entidade</th>");
            strHtmlProducts.Append("<th style='border: 1px solid black; padding: 6px;'>Solução</th>");
            strHtmlProducts.Append("<th style='border: 1px solid black; padding: 6px;'>Quantidade de Itens</th>");
            strHtmlProducts.Append("<th style='border: 1px solid black; padding: 6px;'>Valor Total dos Itens</th>");
            strHtmlProducts.Append("<th style='border: 1px solid black; padding: 6px;'>Valor Total de Desconto Aplicado aos Itens</th>");
            strHtmlProducts.Append("</tr>");
            strHtmlProducts.Append("<tbody style='text-align: center;'></thead>");

            foreach (var prod in quoteDetails.Entities) {

                strHtmlProducts.Append("<tr>");
                strHtmlProducts.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", prod.GetAttributeValue<EntityReference>("findes_coligadaid").Name));
                strHtmlProducts.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", prod.GetAttributeValue<EntityReference>("findes_solucaoid").Name));
                strHtmlProducts.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", Decimal.ToInt32(prod.GetAttributeValue<decimal>("quantity"))));
                strHtmlProducts.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", prod.GetAttributeValue<Money>("baseamount").Value.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))));
                strHtmlProducts.Append(string.Format("<td style='border: 1px solid black; padding: 6px;'>{0}</td>", prod.GetAttributeValue<Money>("findes_valortotaldescontoalcada").Value.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))));
                strHtmlProducts.Append("</tr>");
            }
            strHtmlProducts.Append("</tbody></table>");

            EmailProducts.Set(executionContext, strHtmlProducts.ToString());
        }

        [RequiredArgument]
        [Input("QuoteReference input")]
        [ReferenceTarget("quote")]
        public InArgument<EntityReference> QuoteReference { get; set; }

        [RequiredArgument]
        [Input("SolicitacaoReference input")]
        [ReferenceTarget("findes_solicitacaodedesconto")]
        public InArgument<EntityReference> SolicitacaoReference { get; set; }

        [RequiredArgument]
        [Input("AccountReference input")]
        [ReferenceTarget("account")]
        public InArgument<EntityReference> AccountReference { get; set; }

        [Output("Email Header")]
        public OutArgument<string> EmailHeader { get; set; }

        [Output("Email Products")]
        public OutArgument<string> EmailProducts { get; set; }
    }
}
