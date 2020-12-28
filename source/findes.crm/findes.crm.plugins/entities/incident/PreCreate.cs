using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace findes.crm.plugins.entities.incident
{
    public class PreCreate : BasePlugin
    {
        protected IOrganizationService organizationService;

        protected override void Execute(PluginHelper helper)
        {
            organizationService = helper.GetOrganizationService();
            Entity target = helper.ExtractEntity();

            if (target.Contains("caseorigincode")
                && target.Contains("findes_cpf")
                && target.GetAttributeValue<OptionSetValue>("caseorigincode").Value == 3) // Portal
            {
                Entity refContact = null;
                CheckContact(target, ref refContact);

                if (refContact != null)
                {
                    target.Attributes["customerid"] = new EntityReference(refContact.LogicalName, refContact.Id);
                    target.Attributes["findes_clienteclassificado"] = true;
                    target.Attributes["statuscode"] = new OptionSetValue(482870000); // Aguardando Reclassificação
                }
                else
                {
                    throw new InvalidPluginExecutionException("Contato não resolvido.");
                }
            }
        }

        protected void CheckContact(Entity target, ref Entity refContact)
        {
            string cpf = target.GetAttributeValue<string>("findes_cpf");

            QueryExpression queryContact = new QueryExpression("contact");
            queryContact.ColumnSet = new ColumnSet(new string[] { "fullname" });
            queryContact.Criteria.AddCondition(new ConditionExpression("findes_cpf", ConditionOperator.Equal, cpf));

            Entity contact = organizationService.RetrieveMultiple(queryContact).Entities.FirstOrDefault();

            // Contact Exists
            if (contact != null)
            {
                refContact = contact;
                //UpdateIncident(target, contact);
            }
            // Create Contact
            else
            {
                if (target.Contains("findes_nomecliente") 
                    && target.Contains("findes_telefoneresidencial")
                    && target.Contains("findes_telefonecelular")
                    && target.Contains("findes_emailcliente"))
                {
                    Entity newContact = new Entity("contact");
                    string[] nome = target.GetAttributeValue<string>("findes_nomecliente").Split(' ');
                    newContact.Attributes["firstname"] = nome.First();
                    newContact.Attributes["lastname"] = nome.Last();
                    newContact.Attributes["fullname"] = target.GetAttributeValue<string>("findes_nomecliente");
                    newContact.Attributes["findes_cpf"] = target.GetAttributeValue<string>("findes_cpf");
                    newContact.Attributes["telephone2"] = target.GetAttributeValue<string>("findes_telefoneresidencial");
                    newContact.Attributes["mobilephone"] = target.GetAttributeValue<string>("findes_telefonecelular");
                    newContact.Attributes["emailaddress1"] = target.GetAttributeValue<string>("findes_emailcliente");
                    newContact.Attributes["findes_tipocontato"] = new OptionSetValue(482870002);
                    newContact.Id = organizationService.Create(newContact);

                    //UpdateIncident(target, newContact);
                    refContact = newContact;
                }
                else
                {
                    throw new InvalidPluginExecutionException("Um ou mais campos necessários para a criação do contato não foram preenchidos.");
                }

            }
        }

        protected void UpdateIncident(Entity target, Entity contact)
        {
            Entity incident = new Entity(target.LogicalName, target.Id);
            incident.Attributes["customerid"] = new EntityReference(contact.LogicalName, contact.Id);
            incident.Attributes["findes_clienteclassificado"] = true;
            incident.Attributes["statuscode"] = new OptionSetValue(482870000); // Aguardando Reclassificação
            organizationService.Update(incident);
        }

        
    }
}
