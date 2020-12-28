using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using findes.crm.plugins.repositories;

namespace findes.crm.plugins.entities.product
{
    public class PreCreate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            var organizationService = helper.GetOrganizationService();

            var context = helper.GetContext();
            if (context.MessageName != "Create")
            {
                return;
            }

            Entity target = helper.ExtractEntity();
            //Entity post_image = helper.ExtractEntity("post_image");

            TodasAsOportunidades todasAsOportunidades = new TodasAsOportunidades(organizationService);

            EntityCollection tree = new EntityCollection();
            tree.Entities.Add(target);
            todasAsOportunidades.getTreeProduct(target, ref tree);

            foreach (var item in tree.Entities.ToList())
            {
                var aux = (int)item.GetAttributeValue<OptionSetValue>("producttypecode").Value;
                switch (aux)
                {
                    case 1: //Solução
                        target["findes_solucaoid"] = item.ToEntityReference(); //ok
                        break;

                    case 2: //Linha de atuação
                        target["findes_linhaatuacaoid"] = item.ToEntityReference(); //ok
                        break;

                    case 10: //Família do Produto
                        target["findes_familiaprodutoid"] = item.ToEntityReference(); //ok
                        break;

                    case 9: //Categoria
                        target["findes_categoriaid"] = item.ToEntityReference(); //ok                    
                        break;
                }
            }

            //organizationService.Update(target);
        }
    }
}
