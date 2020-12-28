using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using findes.crm.plugins.entities.salesorder.Controller;
using findes.crm.plugins.entities.salesorder.Request;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using findes.crm.tests.entities;

namespace findes.crm.plugins.entities.salesorder
{
    [TestClass]
    public class PostCreateTest
    {
        [TestMethod]
        public void Atualiza_Produtos_Do_Espelho_da_Proposta()
        {
            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaies.crm2.dynamics.com";

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("salesorder", new Guid("CFF0CFA2-9B36-E911-A957-000D3AC1A49A"), new Microsoft.Xrm.Sdk.Query.ColumnSet(
            "quoteid"));



            Entity postImage = image;

            var request = new PostCreateRequest(image, null, null, postImage);
            var controller = new PostCreateController(organizationService);


            Guid quoteId = controller.getQuoteGuid(image.LogicalName, image.Id);

            var todasPropostas = new TodasAsPropostas(organizationService);

            EntityCollection produtoDaProposta = todasPropostas.GetProdutoProposta(quoteId);

            var listProdutoProposta = produtoDaProposta.Entities.ToList();

            controller.criarProdutoOrdem(quoteId, listProdutoProposta);

            controller.UpdateOrderNumber(image.Id, image.LogicalName);


        }


    }
}
