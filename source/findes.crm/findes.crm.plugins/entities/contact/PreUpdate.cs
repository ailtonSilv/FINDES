using findes.crm.plugins.entities.contact.Request;
using findes.crm.plugins.entities.contact.controller;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.entities.contact {

    public class PreUpdate : BasePlugin {

        public PreUpdate(string unsecure = "") : base(unsecure) {
        }

        protected override void Execute(PluginHelper helper) {

            IOrganizationService organizationService = helper.GetOrganizationService();

            var todosOsEstados = new TodosOsEstados(organizationService);
            var request = new PreUpdateRequest(helper.ExtractEntity());
            var controller = new PreCreateController(organizationService);

            if (request.Cpf != null && request.Cpf != "")
            {
                if (controller.ExisteUmContatoComEste(request.Cpf, out Entity contato))
                {

                    var nomeDoContato = "";
                    if (contato.Attributes.Contains("firstname"))
                        nomeDoContato = contato["firstname"].ToString();

                    throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("O contato {0} já está cadastrado com o CPF informado.", nomeDoContato));
                }
            }

            if (request.FoiInformadoOEstado) {

                Entity estadoInformado = todosOsEstados.GetEstado(request.EstadoId);

                if (null != estadoInformado)
                    request.PreencherNomeDoEstado(estadoInformado["findes_name"].ToString());
            }

            if (request.FoiInformadaACidade) {

                Entity cidadeInformada = todosOsEstados.GetCidade(request.CidadeId);

                if (null != cidadeInformada)
                    request.PreencherNomeDaCidade(cidadeInformada["findes_name"].ToString());
            }
        }
    }
}
