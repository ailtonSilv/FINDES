using findes.crm.plugins.entities.opportunityproduct.Request;
using findes.crm.plugins.repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.tests.entities.Opportunityproduct
{

    [TestClass]
    public class PluginPreCreateTests
    {
        [TestMethod]
        public void Preencher_Campos_Produto_Opportunidade()
        {
            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("opportunityproduct", new Guid("28ACF778-2665-E911-A967-000D3AC1B6C7"), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            Entity preImage = image;

            var request = new PreCreateRequest(image, null, null, preImage);

            var todasAsOportunidades = new TodasAsOportunidades(organizationService);

            Entity produto = todasAsOportunidades.GetProdutoPor(request.ProdutoId);

            if (produto == null)
            {
                throw new InvalidPluginExecutionException("Produto não existe.");
            }

            image["findes_tiposolicitacao"] = new OptionSetValue(482870004);
            image["isproductoverridden"] = new OptionSetValue(0);
               
            organizationService.Update(image);




        }
    }
}
