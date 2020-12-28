using findes.crm.plugins.entities.account.request;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.entities.account
{
    public class PreUpdate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            IOrganizationService organizationService = helper.GetOrganizationService();

            var todosOsEstados = new TodosOsEstados(organizationService);
            var request = new PreUpdateRequest(helper.ExtractEntity());

            if (request.FoiInformadoOEstado)
            {
                Entity estadoInformado = todosOsEstados.GetEstado(request.EstadoId);

                if (null != estadoInformado)
                {
                    request.PreencherNomeDoEstado(estadoInformado["findes_name"].ToString());
                }
            }

            if (request.FoiInformadaACidade)
            {
                Entity cidadeInformada = todosOsEstados.GetCidade(request.CidadeId);

                if (null != cidadeInformada)
                {
                    request.PreencherNomeDaCidade(cidadeInformada["findes_name"].ToString());
                }
            }
        }
    }
}
