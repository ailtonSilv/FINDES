using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace findes.crm.plugins.entities.findes_matriculacontato
{
    public class PreCreate : BasePlugin
    {
        public IOrganizationService organizationService;
        protected override void Execute(PluginHelper helper)
        {
            organizationService = helper.GetOrganizationService();

            /*
            1) Se campo “Responsável Financeiro” (findes_responsavelfinanceiroid) vier vazio "não permitir criação" = retornar excessão "Matrícula sem responsável financeiro."
            */
            if (!this.Image.Contains("findes_responsavelfinanceiroid"))
            {
                throw new InvalidPluginExecutionException("Matrícula sem Responsável Financeiro.");
            }

            if (!this.Image.Contains("findes_produtoid"))
            {
                throw new InvalidPluginExecutionException("Matrícula sem Produto.");
            }

            EntityReference responsavelFinanceiro = this.Image.GetAttributeValue<EntityReference>("findes_responsavelfinanceiroid");
            
            string tipo = responsavelFinanceiro.LogicalName;
            switch (tipo)
            {
                case "contact":
                    RelacionarContato(responsavelFinanceiro);
                    break;
                case "account":
                    RelacionarConta(responsavelFinanceiro);
                    break;
            }          
        }

        /*
        2) Se é uma entidade do tipo Contato (contact):
	        a) Verificar pelo “Email” (emailaddress1) se existe na entidade “Cliente Potencial” (lead):
		        1) Se Existe:
			        - guardar GUID
			        a) Verificar se “Curso de Interesse” (findes_productid) do “Cliente Potencial” (lead) é IGUAL ao 
				        “Curso de Interesse” (findes_productid) da entidade Matricula do Contato (findes_matriculacontato):
				        - Se for igual:
					        > Atualizar o campo no Contato (contat) "Cliente Potencial Originador" (originatingleadid) = lead
					        > Atualizar o Cliente Potencial (lead) com o Status Qualificado (statuscode = 3)
		        2) não existe: 
			        - ignorar (não criar lead)
         */
        protected void RelacionarContato (EntityReference responsavelFinanceiro)
        {
            Entity contato = organizationService.Retrieve(responsavelFinanceiro.LogicalName, responsavelFinanceiro.Id, new ColumnSet(new string[2] { "contactid", "emailaddress1" }));

            QueryExpression queryLead = new QueryExpression("lead");
            queryLead.ColumnSet = new ColumnSet(new string[3] { "leadid", "emailaddress1", "findes_produtoid" });
            queryLead.Criteria.AddCondition(new ConditionExpression("emailaddress1", ConditionOperator.Equal, contato.GetAttributeValue<string>("emailaddress1")));

            EntityCollection resultsLead = organizationService.RetrieveMultiple(queryLead);

            // Verificar
            foreach (Entity lead in resultsLead.Entities)
            {
                //a) Verificar se “Curso de Interesse” (findes_productid) do “Cliente Potencial” (lead)é IGUAL ao 
				//        “Curso de Interesse” (findes_productid)da entidade Matricula do Contato(findes_matriculacontato):
                if (lead.Contains("findes_produtoid") && lead.GetAttributeValue<EntityReference>("findes_produtoid").Id == this.Image.GetAttributeValue<EntityReference>("findes_produtoid").Id)
                {
                    VincularLead("contato", lead, contato);
                }
            }
        }

        /*
        3) Se é uma entidade do tipo Conta (account):
	        a) Verificar pelo “CNPJ” (findes_cnpj) se existe na entidade “Cliente Potencial” (lead):
		        1) Se Existe:
			        - guardar GUID
			        a) Verificar se “Curso de Interesse” (findes_productid) do “Cliente Potencial” (lead) é IGUAL ao 
				        “Curso de Interesse” (findes_productid) da entidade Matricula do Contato (findes_matriculacontato):
				        - Se for igual:
					        > Atualizar o campo na Conta (account) "Cliente Potencial Originador" (originatingleadid) = account
					        > Atualizar o Cliente Potencial (lead) com o Status Qualificado (statuscode = 3)
		        2) Não existe:
			        - ignorar (não criar lead)
        */
        protected void RelacionarConta (EntityReference responsavelFinanceiro)
        {
            Entity conta = organizationService.Retrieve(responsavelFinanceiro.LogicalName, responsavelFinanceiro.Id, new ColumnSet(new string[3] { "accountid", "name", "findes_cnpj" }));

            string contaCnpj = conta.GetAttributeValue<string>("findes_cnpj");
            /*
            if (!String.IsNullOrEmpty(contaCnpj))
            {
                contaCnpj = contaCnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            }
            */

            QueryExpression queryLead = new QueryExpression("lead");
            queryLead.ColumnSet = new ColumnSet(new string[3] { "leadid", "emailaddress1", "findes_produtoid" });
            queryLead.Criteria.AddCondition(new ConditionExpression("findes_cnpj", ConditionOperator.Equal, contaCnpj));

            EntityCollection resultsLead = organizationService.RetrieveMultiple(queryLead);

            // Verificar
            foreach (Entity lead in resultsLead.Entities)
            {
                //a) Verificar se “Curso de Interesse” (findes_productid) do “Cliente Potencial” (lead)é IGUAL ao 
                //        “Curso de Interesse” (findes_productid)da entidade Matricula do Contato(findes_matriculacontato):
                if (lead.Contains("findes_produtoid") && lead.GetAttributeValue<EntityReference>("findes_produtoid").Id == this.Image.GetAttributeValue<EntityReference>("findes_produtoid").Id)
                {
                    VincularLead("conta", lead, conta);
                }
            }
        }

        protected void VincularLead(string tipo, Entity lead, Entity contaContato)
        {
            //> Atualizar o campo na Conta(account) "Cliente Potencial Originador"(originatingleadid) = account
            //> Atualizar o Cliente Potencial(lead) com o Status Qualificado(statuscode = 3)
            contaContato["originatingleadid"] = new EntityReference(lead.LogicalName, lead.Id);
            switch (tipo)
            {
                case "contato":
                    lead["contactid"] = new EntityReference(contaContato.LogicalName, contaContato.Id);
                    break;
                case "conta":
                    lead["accountid"] = new EntityReference(contaContato.LogicalName, contaContato.Id);
                    break;
            }

            QualifyLeadRequest qualifyLead = new QualifyLeadRequest
            {
                CreateAccount = false,
                CreateContact = false,
                CreateOpportunity = false,
                Status = new OptionSetValue(3),
                LeadId = new EntityReference(lead.LogicalName, lead.Id)
            };

            // Atualizar
            organizationService.Update(contaContato);
            organizationService.Update(lead);
            organizationService.Execute(qualifyLead);
        }
    }
}
