using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.repositories {

    public class TodosOsEstados {

        public IOrganizationService organizationService = null;

        public TodosOsEstados(IOrganizationService organizationService) {
            this.organizationService = organizationService;
        }


        public Entity GetEstado(Guid id) {
            return this.GetEntityBy("findes_estado", id);
        }

        public Entity GetCidade(Guid id) {
            return this.GetEntityBy("findes_municipio", id);
        }

        public Entity GetEntityBy(string nomeDaEntidade, Guid id) {
            try {
                return this.organizationService.Retrieve(nomeDaEntidade, id, new ColumnSet(new string[] { "findes_name" }));
            } catch {
            }

            return null;
        }
    }
}
