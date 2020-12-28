using findes.crm.plugins.entities.account.controller;
using findes.crm.plugins.entities.account.request;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.entities.account {

    public class PreCreate : BasePlugin {
        

        protected override void Execute(PluginHelper helper) {

            var todosOsEstados = new TodosOsEstados(helper.GetOrganizationService());
            var request = new PreCreateRequest(helper.ExtractEntity());
            var controller = new PreCreateController(helper.GetOrganizationService());

            if (controller.ExisteUmaContaComEste(request.Cnpj, out Entity conta)) {

                var nomeDaConta = "";
                if (conta.Attributes.Contains("name"))
                    nomeDaConta = conta["name"].ToString();

                throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("A conta {0} já está cadastrada com o CNPJ informado.", nomeDaConta));
            }

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
