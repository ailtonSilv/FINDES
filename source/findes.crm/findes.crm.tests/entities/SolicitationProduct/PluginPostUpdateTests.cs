using System;
using System.Configuration;
using findes.crm.plugins.entities.opportunity.Controller;
using findes.crm.plugins.entities.opportunity.Request;
using findes.crm.plugins.repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace findes.crm.tests.entities.oppotunity
{
    [TestClass]
    public class PluginPostUpdateTests 
    {


        [TestMethod]
        public void updateProduct()
        {
            

            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";
            

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);
            

            Entity image = organizationService.Retrieve("findes_solicitacaodoproduto", new Guid("B7D958EF-C371-E911-A964-000D3AC1B5F1"), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            image["statuscode"] = new OptionSetValue(482870002);
            if (image.GetAttributeValue<OptionSetValue>("statuscode").Value == 482870002 || image.GetAttributeValue<OptionSetValue>("statuscode").Value == 482870004)
            {
                var todasAsOportunidades = new TodasAsOportunidades(organizationService);               
                todasAsOportunidades.updateProductOpportunity(image);        
            }
            
           

        }

        [TestMethod]
        public void updateProductOPP()
        {
            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("opportunityproduct", new Guid("A7A0D8AA-5A5C-E911-A961-000D3AC1B17A"), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            //image["findes_tiposolicitacao"] = new OptionSetValue(482870004);
            //image["isproductoverridden"] = new OptionSetValue(0); //Existente

            image["findes_tiposolicitacao"] = new OptionSetValue(482870000);
            image["isproductoverridden"] = new OptionSetValue(1); //Gravação

            organizationService.Update(image);


        }

    }
}
