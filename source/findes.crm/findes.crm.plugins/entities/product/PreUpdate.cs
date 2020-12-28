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
    public class PreUpdate : BasePlugin
    {
        protected override void Execute(PluginHelper helper)
        {
            var organizationService = helper.GetOrganizationService();

            var context = helper.GetContext();
            if (context.MessageName != "Update")
            {
                return;
            }

            Entity target = helper.ExtractEntity();
            Entity pre_image = helper.ExtractEntity("pre_image");

            TodasAsOportunidades todasAsOportunidades = new TodasAsOportunidades(organizationService);

            EntityCollection tree = new EntityCollection();
            //tree.Entities.Add(pre_image);
            todasAsOportunidades.getTreeProduct(pre_image, ref tree);

            if (!pre_image.Contains("findes_solucaoid") || 
                !pre_image.Contains("findes_linhaatuacaoid") ||
                !pre_image.Contains("findes_familiaprodutoid") ||
                !pre_image.Contains("findes_categoriaid")
                ) 
            {
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
            }
        }
    }
}
