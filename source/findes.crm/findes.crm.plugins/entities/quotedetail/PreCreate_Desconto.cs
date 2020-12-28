using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;

namespace findes.crm.plugins.entities.opportunityproduct
{
    public class PreCreate_Desconto : BasePlugin
    {
        public PreCreate_Desconto(string unsecure = "") : base(unsecure)
        {
        }

        protected override void Execute(PluginHelper helper)
        {
            Entity image = helper.ExtractEntity(); //Prod Proposta


            var service = helper.GetOrganizationService();
            var todasAsPropostas = new TodasAsPropostas(service);
            if (helper.GetContext().MessageName.Equals("update", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!image.Contains("quantity") && !image.Contains("priceperunit") && !image.Contains("productid"))
                {
                    return;
                }
                // Caso o desconto seja recusado (true) não excluir as linhas
                if (image.Contains("findes_descontorecusado"))
                {
                    if (image.GetAttributeValue<bool>("findes_descontorecusado").Equals(true))
                    {
                        return;
                    }
                }

                image = helper.ExtractEntity("postimage");
                todasAsPropostas.DeleteAllDiscountLines(image.Id);

            }
            
			if (image.GetAttributeValue<OptionSetValue>("findes_tiposolicitacao").Value != 482870001 && image.GetAttributeValue<OptionSetValue>("findes_tiposolicitacao").Value != 482870000)
			{
				var todasAsContas = new TodasAsContas(service);

                var accountReference = todasAsContas.getCustomerReferenceByQuote(image.GetAttributeValue<EntityReference>("quoteid").Id, out var contactReference);

                var descontos = todasAsPropostas.getDesconto(((EntityReference)image["findes_solucaoid"]).Id, ((EntityReference)image["findes_linhaatuacaoid"]).Id, ((EntityReference)image["findes_familiaprodutoid"]).Id, ((EntityReference)image["findes_categoriaid"]).Id, ((EntityReference)image["productid"]).Id);

                var descontoOrganizado = todasAsPropostas.OrganizeDiscounts(descontos, "rd");

                List<EntityReference> staticList = null;
                decimal totalDescontoAutomatico = 0m;
                decimal totalDescontoAplicavel = 0m;
                Entity substitutivediscount = null;

                foreach (var desconto in descontoOrganizado)
                {
                    foreach (var regraDesconto in desconto.Value)
                    {
                        if (todasAsPropostas.IsListMember(service, accountReference, ((EntityReference)regraDesconto.GetAttributeValue<AliasedValue>("rd.findes_segmentacao").Value), ref staticList) || todasAsPropostas.IsListMember(service, contactReference, ((EntityReference)regraDesconto.GetAttributeValue<AliasedValue>("rd.findes_segmentacao").Value), ref staticList))
                        {
                            if (regraDesconto.GetAttributeValue<OptionSetValue>("findes_aplicacao").Value == 482870000) //cumulativo
                            {
                                todasAsPropostas.criarLinhaDesconto(image, regraDesconto, ref totalDescontoAutomatico, ref totalDescontoAplicavel);



                            }
                            else
                            {
                                if (substitutivediscount == null)
                                {
                                    substitutivediscount = regraDesconto;
                                }
                                else if ((decimal)regraDesconto.GetAttributeValue<AliasedValue>("rd.findes_porcentagemdesconto").Value > (decimal)substitutivediscount.GetAttributeValue<AliasedValue>("rd.findes_porcentagemdesconto").Value)
                                {
                                    substitutivediscount = regraDesconto;
                                }
                            }


                            break;
                        }
                    }
                }

                if (substitutivediscount != null)
                {
                    todasAsPropostas.criarLinhaDesconto(image, substitutivediscount, ref totalDescontoAutomatico, ref totalDescontoAplicavel);

                }

                image = new Entity(image.LogicalName, image.Id); //Limpa campos para enviar para o update e não entrar em looping
                image["manualdiscountamount"] = new Money(totalDescontoAutomatico);
                service.Update(image);
                //TODO fazem implementação do descontro aplicavel
            }

        }
    }
}
