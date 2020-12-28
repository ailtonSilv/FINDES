using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.findes_solicitacaodedesconto
{
    public class PostUpdate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            var organizationService = helper.GetOrganizationService();

            Entity post_image = helper.ExtractEntity("post_image");

            // "Reprovado" (482.870.001) OU "Cancelado"(3)
            if (post_image.GetAttributeValue<OptionSetValue>("statuscode").Value == 482870001 || post_image.GetAttributeValue<OptionSetValue>("statuscode").Value == 3) 
            {
                /*
                CONSULTAR a o campo "Proposta" (regardingobjectid) 
                E listar os produtos da proposta (quotedetail, após listar os produtos da proposta o plugin deverá:
                */
                var todasAsPropostas = new TodasAsPropostas(helper.GetOrganizationService());

                EntityReference proposta = post_image.GetAttributeValue<EntityReference>("regardingobjectid");

                EntityCollection produtoDaProposta = todasAsPropostas.GetProdutoProposta(proposta.Id);

                /*
                SUBTRAIR o valor do campo "Valor Soma Desconto Alçada" (findes_valorsomadescontoalcada) 
                COM o valor do campo "Valor Total Desconto Alçada" (findes_valortotaldescontoalcada) 

                Ex:  SE findes_valorsomadescontoalcada = 80 
                E findes_valortotaldescontoalcada = 10, 
                ENTÃO findes_valorsomadescontoalcada = 70

                Após subtrair os valores o plugin deverá limpar o valor do campo "Valor Total Desconto Alçada" (findes_valortotaldescontoalcada)
                */
                decimal novoTotalProposta = 0m;
                bool atualizarProposta = false;
                foreach (Entity prodProp in produtoDaProposta.Entities)
                {
                    decimal findes_valorsomadescontoalcada = 0m;
                    decimal findes_valortotaldescontoalcada = 0m;

                    if (prodProp.Contains("findes_valorsomadescontoalcada"))
                    {
                        findes_valorsomadescontoalcada = prodProp.GetAttributeValue<Money>("findes_valorsomadescontoalcada").Value;
                    }
                    if (prodProp.Contains("findes_valortotaldescontoalcada"))
                    {
                        findes_valortotaldescontoalcada = prodProp.GetAttributeValue<Money>("findes_valortotaldescontoalcada").Value;
                    }
                    if (findes_valorsomadescontoalcada > 0 && findes_valortotaldescontoalcada > 0)
                    {
                        decimal novoTotal = findes_valorsomadescontoalcada - findes_valortotaldescontoalcada;
                        novoTotalProposta += novoTotal;
                        prodProp["findes_valorsomadescontoalcada"] = new Money(novoTotal);
                        prodProp["findes_valortotaldescontoalcada"] = new Money(0m);
                        prodProp["findes_descontorecusado"] = true;
                        // Atualiza o Produto da Proposta
                        organizationService.Update(prodProp);
                        atualizarProposta = true;
                    }
                }

                if (atualizarProposta)
                {
                    // Atualizar a Proposta com o valor atualizado do desconto;
                    Entity prop = new Entity("quote", proposta.Id);
                    prop["findes_valortotaldescontoalcadajaaplicado"] = new Money(novoTotalProposta);
                    // Atualiza a Proposta
                    organizationService.Update(prop);
                }
            }
        }
    }
}
