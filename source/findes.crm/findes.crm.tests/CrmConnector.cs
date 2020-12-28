using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.tests {
    internal class CrmConnector {


        public static IOrganizationService Connect() {

            var connectionString = string.Format("Url={0}; Username={1}; Password={2}; authtype={3}",
                                                     "https://sesisenaiesdev.crm2.dynamics.com",
                                                     "customservicesdynamics@sesi-es.org.br",
                                                     "s3rvicesDyn@m1cs",
                                                     "Office365");

            var conn = new CrmServiceClient(connectionString);

            return conn.OrganizationServiceProxy;

        }
    }
}
