using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.tests.plugins.entities.opportunity {
    [TestClass]
    public class PostUpdateTest {

        [TestMethod]
        public void PostUpdate() {


            //4D57CF9B-2AD9-E811-A970-000D3AC1BB19

            var crmService = CrmConnector.Connect();

            var cols = new ColumnSet(new string[] { "findes_gerarproposta", "name", "customerid", "parentaccountid", "pricelevelid", "parentcontactid", "findes_origemoportunidade" });
            var opp = crmService.Retrieve("opportunity", new Guid("4D57CF9B-2AD9-E811-A970-000D3AC1BB19"), cols);

            var request = new findes.crm.plugins.entities.opportunity.Request.PostUpdateRequest(null, null, null);


        }
    }
}
