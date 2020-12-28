using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using findes.crm.plugins.entities.sindicato.Controller;

namespace findes.crm.tests.entities.syndicate
{
    [TestClass]
    public class PostCreateTest
    {
        [TestMethod]
        public void PreencheComGuid()
        {
            string crmUser = "admindynamics@sesi-es.org.br";
            string crmPwd = "adminDyn@m1cs";
            string crmUrl = "https://sesisenaiesdev.crm2.dynamics.com";

            IOrganizationService organizationService = ConnectionHelper.GetOrganizationService(crmUrl, crmUser, crmPwd);

            Entity image = organizationService.Retrieve("findes_sindicato", new Guid("4D74314E-6C13-E911-A950-000D3AC1BEBA"), new Microsoft.Xrm.Sdk.Query.ColumnSet("findes_sindicatoid"));

            var controller = new PostCreateController(organizationService);
            image["findes_codigo"] = image.Id.ToString().ToUpper();
            controller.setGuid(image);
        }
    }
}
