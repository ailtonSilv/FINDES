using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System;
using Findes.CustomAction.ExtratorCRM.Model;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace Findes.CustomAction.ExtratorCRM
{
    public class ExtratorCRMCodeActivity : CodeActivity
    {
        [Input("Pagina")]
        public InArgument<int> Pagina
        {
            get;
            set;
        }

        [Input("QtdePagina")]
        public InArgument<int> QtdePagina
        {
            get;
            set;
        }

        [Input("DataModificacao")]
        public InArgument<DateTime> DataModificacao
        {
            get;
            set;
        }

        [Input("Layout")]
        public InArgument<string> Layout
        {
            get;
            set;
        }

        [Input("ThrowEx")]
        public InArgument<string> ThrowEx
        {
            get;
            set;
        }

        [Output("Message")]
        public OutArgument<string> Message
        {
            get;
            set;
        }
        
        [Output("Result")]
        public OutArgument<string> Result
        {
            get;
            set;
        }

        protected override void Execute(CodeActivityContext context)
        {
            var workflowContext = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var tracingService = context.GetExtension<ITracingService>();
            var orgService = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            bool throwEx ;
            bool.TryParse(ThrowEx.Get<string>(context), out throwEx);
            try
            {
                int pagina = Pagina.Get<int>(context);
                int qtdePagina = QtdePagina.Get<int>(context);
                DateTime dataModificacao = DataModificacao.Get<DateTime>(context);
                string layout = Layout.Get<string>(context).ToLower();
                string dadosRecebidos = $"Dados solicitados = pagina: {pagina} / qtdePagina: {qtdePagina} / data: {dataModificacao} / layout: {layout} / throwEx: {throwEx}";

                tracingService.Trace(dadosRecebidos);
                //throw new InvalidPluginExecutionException(dadosRecebidos);

                string retorno = "Sucesso.";
                string resultadoJson = "";

                Util util = new Util(orgService, tracingService);
                switch (layout)
                {
                    case "contas":
                    case "contatos":
                    case "oportunidades":
                    case "produtooportunidades":
                    case "propostas":
                    case "produtopropostas":
                    case "contratos":
                    case "produtocontratos":
                    case "ocorrencias":
                    case "produtos":
                        resultadoJson = util.GetCRMData(qtdePagina, pagina, dataModificacao, layout);
                        break;
                    default:
                        retorno = "CWF Execute - Layout não encontrado";
                        break;
                }

                Result.Set(context, resultadoJson);
                Message.Set(context, retorno);
            }
            catch (Exception ex)
            {
                tracingService.Trace(ex.Message);
                tracingService.Trace(ex.StackTrace);
                Result.Set(context, "");
                Message.Set(context, $"CWF ERRO: {ex.Message}");
                if (throwEx)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex.InnerException);
                }
            }
        }
    }
}
