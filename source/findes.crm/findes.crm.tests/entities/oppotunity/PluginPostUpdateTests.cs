using System;
using System.Configuration;
using findes.crm.plugins.repositories;
using findes.crm.plugins.entities.opportunityproduct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.opportunity.Controller;
using findes.crm.plugins.entities.opportunity.Request;


namespace findes.crm.tests.entities.Opportunityproduct
{
    [TestClass]
    public class PluginPostUpdateTests 
    {


        [TestMethod]
        public void Atualizar_Produto_Opp()
        {

            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";
           

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);
            

            Entity solicitaçãoProdutoIMG = organizationService.Retrieve("findes_solicitacaodoproduto", new Guid("AB88DE31-3565-E911-A967-000D3AC1BB7C"), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            var todasOpp = new TodasAsOportunidades(organizationService);

            todasOpp.updateProductOpportunity(solicitaçãoProdutoIMG);
            
            
        }

       [TestMethod]
       public void createQuote(){


            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";


            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("opportunity", new Guid("98BE95AE-C371-E911-A967-000D3AC1B6C7"), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            var request = new PostUpdateRequest(image, null, null, image);
            var controller = new PostUpdateController(organizationService);

            var todasAsOportunidades = new TodasAsOportunidades(organizationService);

            EntityCollection produtosDaOportunidade =
                todasAsOportunidades.GetProdutosDaOportunidade(image.Id, true);

            Entity contaDePagamento = todasAsOportunidades.GetConta(image.GetAttributeValue<EntityReference>("customerid").Id);

            

            controller.GerarPropostas(request, contaDePagamento, produtosDaOportunidade);

        }


        
    }
}
