using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins {
    public abstract class BaseController {

        protected IOrganizationService OrganizationService = null;

        protected BaseController() {

        }

        protected BaseController(IOrganizationService organizationService) {

            this.OrganizationService = organizationService ??
                throw new InvalidPluginExecutionException(OperationStatus.Canceled, "A instância do ORGANIZATION SERVICE não foi informada ou é inválida.");

        }

       
    }
}
