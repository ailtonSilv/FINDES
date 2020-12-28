        using findes.crm.plugins.entities.contact.controller;
using findes.crm.plugins.entities.contact.request;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findes.crm.plugins.entities.contact {

    public class PreCreate : BasePlugin {

        public PreCreate(string unsecure = "") : base(unsecure) {
        }

        protected override void Execute(PluginHelper helper) {

            IOrganizationService organizationService = helper.GetOrganizationService();

            var request = new PreCreateRequest(helper.ExtractEntity());
            var controller = new PreCreateController(organizationService);


            if (request.TipoContato == null)
            {
                throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("Tipo de Contato não foi preenchido para {0}, preencha o campo e tente novamente.",request.name));
            }

            if(request.TipoContato.Value != 482870001)
            {
                if (request.Cpf == null)
                {
                    throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("Não é possivel criar o contato {0}, pois o CPF é obrigatório.", request.name));
                }
                else
                {
                    if (controller.ExisteUmContatoComEste(request.Cpf, out Entity contato))
                    {

                        var nomeDoContato = "";
                        if (contato.Attributes.Contains("firstname"))
                            nomeDoContato = contato["firstname"].ToString();

                        throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("O contato {0} já está cadastrado com o CPF informado.", nomeDoContato));
                    }
                }
            }

            
           
            //if (controller.ExisteUmContatoComEste(request.Cpf, out Entity contato))
            //{

            //    var nomeDoContato = "";
            //    if (contato.Attributes.Contains("firstname"))
            //        nomeDoContato = contato["firstname"].ToString();

            //    throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("O contato {0} já está cadastrado com o CPF informado.", nomeDoContato));
            //}
            
            
        }
    }
}
