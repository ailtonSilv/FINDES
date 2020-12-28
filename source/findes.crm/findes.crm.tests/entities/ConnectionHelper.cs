using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.tests.entities
{
    class ConnectionHelper
    {
        public static IOrganizationService GetOrganizationService(string url, string user, string password, string authType = "Office365", string domain = "")
        {
            IOrganizationService service = null;
            var connectionString = "";

            try
            {
                connectionString = string.Format("Url={0}; Username={1}; Password={2}; authtype={3}",
                                                     url,
                                                     user,
                                                     password,
                                                     authType);

                var conn = new CrmServiceClient(connectionString);
                var serviceProxy = conn.OrganizationServiceProxy;
                var proxyClient = conn.OrganizationWebProxyClient;

                if (serviceProxy != null)
                {
                    serviceProxy.Timeout = new TimeSpan(0, 7, 0);
                    service = (IOrganizationService)serviceProxy;
                }
                else if (proxyClient != null)
                {
                    service = (IOrganizationService)proxyClient;
                }

            }
            catch (Exception ex)
            {
                
            }

            return service;
        }
    }
}
