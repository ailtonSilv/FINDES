using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.entities.contact.controller {

    public class PreCreateController : BaseController {

        private TodosOsContatos todosOsContatos = null;

        protected PreCreateController() {
        }

        public PreCreateController(IOrganizationService organizationService) : base(organizationService) {
            this.todosOsContatos = new TodosOsContatos(organizationService);
        }

        public bool ExisteUmContatoComEste(string cpf, out Entity contato) {

            contato = this.todosOsContatos.GetContatoPorCpf(cpf);

            return null != contato;

        }
       
    }
}
