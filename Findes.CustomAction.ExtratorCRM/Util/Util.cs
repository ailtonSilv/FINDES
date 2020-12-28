using Findes.CustomAction.ExtratorCRM.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Linq;

namespace Findes.CustomAction.ExtratorCRM
{
    public class Util
    {
        private IOrganizationService organizationService;
        private ITracingService tracingService;
        public Util(IOrganizationService oservice, ITracingService tservice)
        {
            organizationService = oservice;
            tracingService = tservice;
        }

        /// <summary>
        /// Extrai os dados do CRM da entidade ax4b_parametrointegracao e valida o modelo de dados
        /// </summary>
        /// <param name="parametro"></param>
        /// <param name="dados"></param>
        /// <param name="modeloBase"></param>
        public void GetEntityInfo(string parametro, string parametroDePara, ref DadosEntidade dados, string[] modeloBase)
        {
            #region > Parametro Entidade
            QueryExpression query = new QueryExpression
            {
                EntityName = "ax4b_parametrointegracao",
                ColumnSet = new ColumnSet(true),
            };
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("ax4b_name", ConditionOperator.Equal, parametro);

            EntityCollection data = organizationService.RetrieveMultiple(query);

            if (data.Entities.Count > 0 && data.Entities[0].Contains("ax4b_valor"))
            {
                string xmlValue = data.Entities[0].GetAttributeValue<string>("ax4b_valor");

                XmlDocument doc = new XmlDocument();
                doc.Load(new StringReader(xmlValue));

                Dictionary<string, CrmProperty> jObject = new Dictionary<string, CrmProperty>();
                 
                foreach (XmlNode node in doc.SelectNodes("/xml/*"))
                {
                    CrmProperty crmProperty = new CrmProperty();
                    crmProperty.multiLookupItens = new List<MultiLookup>();

                    crmProperty.crmname = node.Attributes["crmname"] != null ? node.Attributes["crmname"].Value : null;
                    crmProperty.crmtype = node.Attributes["crmtype"] != null ? node.Attributes["crmtype"].Value : null;
                    crmProperty.crmtypefield = node.Attributes["crmtypefield"] != null ? node.Attributes["crmtypefield"].Value : null;
                    crmProperty.crmlookupentityname = node.Attributes["crmlookupentityname"] != null ? node.Attributes["crmlookupentityname"].Value : null;
                    crmProperty.crmlookupto = node.Attributes["crmlookupto"] != null ? node.Attributes["crmlookupto"].Value : null;
                    crmProperty.crmlookupcolumn = node.Attributes["crmlookupcolumn"] != null ? node.Attributes["crmlookupcolumn"].Value : null;
                    crmProperty.ismultilookup = node.Attributes["ismultilookup"] != null ? bool.Parse(node.Attributes["ismultilookup"].Value) : false;
                    if (crmProperty.ismultilookup)
                    {
                        foreach (XmlNode itemlookup in node.SelectNodes("Multilookup"))
                        {
                            MultiLookup multiLookup = new MultiLookup
                            {
                                crmname = itemlookup.Attributes["crmname"] != null ? itemlookup.Attributes["crmname"].Value : null,
                                crmlookupentityname = itemlookup.Attributes["crmlookupentityname"] != null ? itemlookup.Attributes["crmlookupentityname"].Value : null,
                                crmlookupto = itemlookup.Attributes["crmlookupto"] != null ? itemlookup.Attributes["crmlookupto"].Value : null,
                                crmlookupcolumn = itemlookup.Attributes["crmlookupcolumn"] != null ? itemlookup.Attributes["crmlookupcolumn"].Value : null
                            };
                            //crmProperty.crmlookupentityname = itemlookup.Attributes["crmlookupentityname"].Value;
                            //crmProperty.crmlookupcolumn = itemlookup.Attributes["crmlookupcolumn"].Value;
                            crmProperty.multiLookupItens.Add(multiLookup);
                        }
                    }
                    /*
                    tracingService.Trace($"Filter Count: {node.SelectNodes("Filter").Count.ToString()}");
                    foreach (XmlNode itemFilter in node.SelectNodes("Filter"))
                    {
                        Filter filterItem = new Filter
                        {
                            filtertype = itemFilter.Attributes["type"] != null ? itemFilter.Attributes["type"].Value : null
                        };
                        foreach(XmlNode condItens in itemFilter.SelectNodes("Conditions"))
                        {
                            Conditions condItem = new Conditions
                            {
                                crmfield = condItens.Attributes["crmfield"] != null ? condItens.Attributes["crmfield"].Value : null,
                                operation = condItens.Attributes["operation"] != null ? condItens.Attributes["operation"].Value : null,
                                value = condItens.Attributes["value"] != null ? condItens.Attributes["value"].Value : null
                            };
                            filterItem.conditions.Add(condItem);
                        }
                        crmProperty.filterItens.Add(filterItem);
                    }
                    */
                    jObject.Add(node.Name, crmProperty);
                }

                dados.entityData = jObject;

                // Verifica se as chaves recebidas conferem com o modelo base
                ValidateModel(jObject, modeloBase, dados, parametro);

                List<string> colunas = new List<string>();
                foreach (var x in dados.entityData)
                {
                    if (x.Key == "Entidade")
                    {
                        dados.entityName = dados.entityData["Entidade"].crmname.ToString().ToLower();
                        dados.entityID = dados.entityData["Entidade"].crmname.ToString().ToLower() + "id";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(x.Value.crmtype)
                            && !string.IsNullOrEmpty(x.Value.crmname)
                            && x.Value.crmtype != "lookup")
                        {
                            colunas.Add(x.Value.crmname.ToString());
                        }
                    }
                }
                dados.entityColumns = colunas.ToArray();

            }
            else
            {
                throw new InvalidPluginExecutionException($"Parâmetro ou o valor do parâmetro > '{parametro}' , não encontrado na entidade {query.EntityName}.");
            }
            #endregion

            #region > Parametro DePara
            QueryExpression queryDePara = new QueryExpression
            {
                EntityName = "ax4b_parametrointegracao",
                ColumnSet = new ColumnSet(true),
            };
            queryDePara.Criteria = new FilterExpression();
            queryDePara.Criteria.AddCondition("ax4b_name", ConditionOperator.Equal, parametroDePara);

            EntityCollection dataDePara = organizationService.RetrieveMultiple(queryDePara);

            // Inicializar Lista
            dados.crmDeParaDNs = new List<CrmDeParaDN>();

            if (dataDePara.Entities.Count > 0 && dataDePara.Entities[0].Contains("ax4b_valor"))
            {
                string[] camposDePara = dataDePara.Entities[0].GetAttributeValue<string>("ax4b_valor").Split(',');
                foreach (string campo in camposDePara)
                {
                    QueryExpression queryDeParaCampo = new QueryExpression
                    {
                        EntityName = "ax4b_parametrointegracao",
                        ColumnSet = new ColumnSet(true),
                    };
                    queryDeParaCampo.Criteria = new FilterExpression();
                    queryDeParaCampo.Criteria.AddCondition("ax4b_name", ConditionOperator.Equal, campo);

                    EntityCollection dadosDePara = organizationService.RetrieveMultiple(queryDeParaCampo);

                    if (dadosDePara.Entities.Count > 0 && dadosDePara.Entities[0].Contains("ax4b_valor"))
                    {
                        string xmlValue = dadosDePara.Entities[0].GetAttributeValue<string>("ax4b_valor");

                        XmlDocument doc = new XmlDocument();
                        doc.Load(new StringReader(xmlValue));

                        CrmDeParaDN deParaDN = new CrmDeParaDN();
                        // Inicializar Lista
                        deParaDN.itens = new List<ItemDePara>();

                        foreach (XmlNode node in doc.SelectNodes("/xml/*"))
                        {
                            if (node.LocalName == "DeParaEntidade")
                            {
                                deParaDN.entityname = node.Attributes["crmname"].Value;
                            }
                            if (node.LocalName == "Campo")
                            {
                                deParaDN.dnname = node.Attributes["dnname"].Value;
                                deParaDN.crmname = node.Attributes["crmname"].Value;
                            }
                            if (node.LocalName == "Valores" && node.SelectNodes("item").Count > 0)
                            {
                                foreach (XmlNode item in node.SelectNodes("item"))
                                {
                                    string crmValues = item.Attributes["crm"].Value;
                                    ItemDePara itemDePara = new ItemDePara();
                                    itemDePara.name = item.Attributes["name"].Value;
                                    itemDePara.dn = item.Attributes["dn"].Value;// != null ? int.Parse(item.Attributes["dn"].Value) : 0;
                                    itemDePara.crm = item.Attributes["crm"].Value != null ? crmValues.Split(',') : new string[] { "" }; //crmValues.Split(',').Select(n => Convert.ToInt64(n)).ToArray() : new long[] { 0 };
                                    deParaDN.itens.Add(itemDePara);
                                }
                            }
                        }

                        dados.crmDeParaDNs.Add(deParaDN);

                    }
                    else
                    {
                        throw new InvalidPluginExecutionException($"De/Para = Parâmetro ou o valor do parâmetro > '{campo}' , não encontrado na entidade {queryDeParaCampo.EntityName}.");
                    }
                }
            }
            #endregion
        }

        private void ValidateModel(Dictionary<string, CrmProperty> jObject, string[] modeloBase, DadosEntidade dados, string parametro)
        {
            bool chaves = true;
            List<string> chavesAusentes = new List<string>();
            foreach (string str in modeloBase)
            {
                if (!dados.entityData.ContainsKey(str))
                {
                    chavesAusentes.Add(str);
                    chaves = false;
                }
            }
            if (!chaves)
            {
                throw new InvalidPluginExecutionException($"A(s) seguinte(s) chave(s): {string.Join(", ", chavesAusentes)}; não foram encontradas no parâmetro '{parametro}'.");
            }

            bool propriedades = true;
            List<string> propriedadesVazias = new List<string>();
            foreach (var x in dados.entityData)
            {
                if (x.Key != "Entidade")
                {
                    if (string.IsNullOrEmpty(x.Value.crmtype) || string.IsNullOrEmpty(x.Value.crmname))
                    {
                        propriedadesVazias.Add(x.Key);
                        propriedades = false;
                    }
                }
            }
            if (!propriedades)
            {
                throw new InvalidPluginExecutionException($"A(s) seguinte(s) chaves(s): {string.Join(", ", propriedadesVazias)}; não possuem a(s) propriedade(s) 'crmname' e/ou 'crmtype' no parâmetro '{parametro}'.");
            }

        }

        /// <summary>
        /// Método para solicitar 
        /// </summary>
        /// <param name="qtdePagina"></param>
        /// <param name="pagina"></param>
        /// <param name="dataModificacao"></param>
        /// <returns></returns>
        public string GetCRMData(int qtdePagina, int pagina, DateTime dataModificacao, string layout)
        {

            //tracingService.Trace("GetCRMData => Processing");
            DadosEntidade dados = new DadosEntidade();
            // Com base no layout validar informações da entidade
            switch (layout)
            {
                case "contas":
                    Conta conta = new Conta();
                    GetEntityInfo(conta.parametroExtrator, conta.parametroExtratorDePara, ref dados, conta.modeloBase);
                    break;
                case "contatos":
                    Contato contato = new Contato();
                    GetEntityInfo(contato.parametroExtrator, contato.parametroExtratorDePara, ref dados, contato.modeloBase);
                    break;
                case "oportunidades":
                    Oportunidade oportunidade = new Oportunidade();
                    GetEntityInfo(oportunidade.parametroExtrator, oportunidade.parametroExtratorDePara, ref dados, oportunidade.modeloBase);
                    break;
                case "produtooportunidades":
                    ProdutoOportunidade produtoOportunidade = new ProdutoOportunidade();
                    GetEntityInfo(produtoOportunidade.parametroExtrator, produtoOportunidade.parametroExtratorDePara, ref dados, produtoOportunidade.modeloBase);
                    break;
                case "propostas":
                    Proposta proposta = new Proposta();
                    GetEntityInfo(proposta.parametroExtrator, proposta.parametroExtratorDePara, ref dados, proposta.modeloBase);
                    break;
                case "produtopropostas":
                    ProdutoProposta produtoProposta = new ProdutoProposta();
                    GetEntityInfo(produtoProposta.parametroExtrator, produtoProposta.parametroExtratorDePara, ref dados, produtoProposta.modeloBase);
                    break;
                case "contratos":
                    Contrato contrato = new Contrato();
                    GetEntityInfo(contrato.parametroExtrator, contrato.parametroExtratorDePara, ref dados, contrato.modeloBase);
                    break;
                case "produtocontratos":
                    ProdutoContrato produtoContrato = new ProdutoContrato();
                    GetEntityInfo(produtoContrato.parametroExtrator, produtoContrato.parametroExtratorDePara, ref dados, produtoContrato.modeloBase);
                    break;
                case "ocorrencias":
                    Ocorrencia ocorrencia = new Ocorrencia();
                    GetEntityInfo(ocorrencia.parametroExtrator, ocorrencia.parametroExtratorDePara, ref dados, ocorrencia.modeloBase);
                    break;
                case "produtos":
                    Produto produto = new Produto();
                    GetEntityInfo(produto.parametroExtrator, produto.parametroExtratorDePara, ref dados, produto.modeloBase);
                    break;
                default:
                    throw new InvalidPluginExecutionException("GetEntityInfo - Layout não definido");
            }

            //tracingService.Trace("GetCRMData => Setting QueryExpression");

            string entityName = dados.entityName;
            ColumnSet entityID = new ColumnSet(dados.entityID);
            ColumnSet entityColumns = new ColumnSet(dados.entityColumns);
            ConditionExpression condition = new ConditionExpression("modifiedon", ConditionOperator.GreaterThan, dataModificacao);
            OrderExpression order = new OrderExpression("createdon", OrderType.Ascending);
            
            
            List<Functions.LinkEntities> linkEntitiesData = new List<Functions.LinkEntities>();
            foreach (KeyValuePair<string, CrmProperty> keyPair in dados.entityData)
            {
                if (keyPair.Value.crmtype == "lookup")
                {
                    Functions.LinkEntities listExistente = linkEntitiesData.Find(key => key.LinkFromAttributeName == keyPair.Value.crmname
                                                                && key.LinkToEntityName == keyPair.Value.crmlookupentityname
                                                                && key.LinkToAttributeName == keyPair.Value.crmlookupto);
                    if (listExistente != null)
                    {
                        listExistente.Columns.Add(keyPair.Value.crmlookupcolumn);
                    }
                    else
                    {
                        Functions.LinkEntities novoLink = new Functions.LinkEntities
                        {
                            LinkFromEntityName = dados.entityName,
                            LinkFromAttributeName = keyPair.Value.crmname,
                            LinkToEntityName = keyPair.Value.crmlookupentityname,
                            LinkToAttributeName = keyPair.Value.crmlookupto,
                            EntityAlias = keyPair.Value.crmlookupentityname
                        };
                        novoLink.Columns = new List<string>();
                        novoLink.Columns.Add(keyPair.Value.crmlookupcolumn);

                        if (keyPair.Value.ismultilookup)
                        {
                            novoLink.Links = new List<Functions.LinkEntities>();
                            foreach (MultiLookup multi in keyPair.Value.multiLookupItens)
                            {
                                Functions.LinkEntities subLink = new Functions.LinkEntities
                                {
                                    LinkFromEntityName = multi.crmfromentityname,
                                    LinkFromAttributeName = multi.crmname,
                                    LinkToEntityName = multi.crmlookupentityname,
                                    LinkToAttributeName = multi.crmlookupto,
                                    EntityAlias = multi.crmlookupentityname
                                };
                                subLink.Columns = new List<string>();
                                subLink.Columns.Add(multi.crmlookupcolumn);
                                novoLink.Links.Add(subLink);
                            }
                        }
                        linkEntitiesData.Add(novoLink);
                    }
                }
            }

            #region Retornar Página Solicitada
            //tracingService.Trace("GetEntityInfo => Retornar Página Solicitada");
            QueryExpression query = new QueryExpression
            {
                EntityName = entityName,
                ColumnSet = entityColumns,
            };
            query.Criteria.AddCondition(condition);
            query.Orders.Add(order);
            query.PageInfo = new PagingInfo();
            query.PageInfo.Count = qtdePagina;
            query.PageInfo.PageNumber = pagina;

            switch (layout)
            {
                case "contatos":
                    FilterExpression filterCtt = new FilterExpression(LogicalOperator.And);
                    filterCtt.Conditions.Add(new ConditionExpression("findes_tipocontato", ConditionOperator.Equal, 482870001));
                    query.Criteria.AddFilter(filterCtt);
                    break;
                case "produtooportunidades":
                    FilterExpression filterPrdOpp = new FilterExpression(LogicalOperator.And);
                    filterPrdOpp.Conditions.Add(new ConditionExpression("isproductoverridden", ConditionOperator.Equal, false));
                    query.Criteria.AddFilter(filterPrdOpp);
                    break;

                case "produtos":
                    FilterExpression filterPrd = new FilterExpression(LogicalOperator.And);
                    filterPrd.Conditions.Add(new ConditionExpression("statuscode", ConditionOperator.Equal, 1));
                    query.Criteria.AddFilter(filterPrd);
                    break;
            }

            foreach (Functions.LinkEntities item in linkEntitiesData)
            {
                LinkEntity linkEntity = new LinkEntity
                {
                    LinkFromEntityName = item.LinkFromEntityName,
                    LinkFromAttributeName = item.LinkFromAttributeName,
                    LinkToEntityName = item.LinkToEntityName,
                    LinkToAttributeName = item.LinkToAttributeName,
                    EntityAlias = item.EntityAlias,
                    Columns = new ColumnSet(item.Columns.ToArray()),
                    JoinOperator = JoinOperator.LeftOuter
                };
                query.LinkEntities.Add(linkEntity);
                if (item.Links != null && item.Links.Count > 0)
                {
                    foreach(Functions.LinkEntities links in item.Links)
                    {
                        LinkEntity linkEntitysub = new LinkEntity
                        {
                            LinkFromEntityName = links.LinkFromEntityName,
                            LinkFromAttributeName = links.LinkFromAttributeName,
                            LinkToEntityName = links.LinkToEntityName,
                            LinkToAttributeName = links.LinkToAttributeName,
                            EntityAlias = links.EntityAlias,
                            Columns = new ColumnSet(links.Columns.ToArray()),
                            JoinOperator = JoinOperator.LeftOuter
                        };
                        query.LinkEntities[query.LinkEntities.Count - 1].LinkEntities.Add(linkEntitysub);
                    }
                }
            }

            EntityCollection results = organizationService.RetrieveMultiple(query);

            // Debug - RETORNO QUERY
            /*
            MemoryStream msDebug = new MemoryStream();
            DataContractJsonSerializer jsonSerializerDebug = new DataContractJsonSerializer(typeof(QueryExpression));
            jsonSerializerDebug.WriteObject(msDebug, query);
            return Encoding.UTF8.GetString(msDebug.ToArray());
            */
            #endregion

            #region Contar Total de Páginas e Total de Registros
            //tracingService.Trace("GetEntityInfo => Contar Total de Páginas e Total de Registros");
            int totalPaginas = 0;
            int totalRegistros = 0;
            QueryExpression queryCount = new QueryExpression
            {
                EntityName = entityName,
                ColumnSet = entityID
            };
            queryCount.Criteria.AddCondition(condition);
            queryCount.Orders.Add(order);
            queryCount.PageInfo = new PagingInfo();
            queryCount.PageInfo.Count = qtdePagina;
            queryCount.PageInfo.PageNumber = 1;

            /*
            foreach (LinkEntity lk in linkEntities)
            {
                queryCount.LinkEntities.Add(lk);
            }
            */

            while (true)
            {
                EntityCollection resultsCount = organizationService.RetrieveMultiple(queryCount);
                // Loop página atual
                if (resultsCount != null)
                {
                    totalPaginas++;
                    foreach (Entity item in resultsCount.Entities)
                    {
                        totalRegistros++;
                    }
                }

                // Próxima Pagina
                if (resultsCount.MoreRecords)
                {
                    queryCount.PageInfo.PageNumber++;
                    queryCount.PageInfo.PagingCookie = resultsCount.PagingCookie;
                }
                else
                {
                    break;
                }
            }
            #endregion

            //tracingService.Trace("GetEntityInfo => DataContractSerializer");
            MemoryStream ms = new MemoryStream();
            // Com base no layout definir o "DataContractSerializer" e instanciar o objeto solicitando o resultado do modelo do layout solicitado
            switch (layout)
            {
                case "contas":
                    Conta conta = new Conta();
                    DataContractJsonSerializer jsonSerializerConta = new DataContractJsonSerializer(typeof(Conta.Resultado));
                    jsonSerializerConta.WriteObject(ms, conta.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "contatos":
                    Contato contatos = new Contato();
                    DataContractJsonSerializer jsonSerializerContato = new DataContractJsonSerializer(typeof(Contato.Resultado));
                    jsonSerializerContato.WriteObject(ms, contatos.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "oportunidades":
                    Oportunidade oportunidades = new Oportunidade();
                    DataContractJsonSerializer jsonSerializerOportunidade = new DataContractJsonSerializer(typeof(Oportunidade.Resultado));
                    jsonSerializerOportunidade.WriteObject(ms, oportunidades.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));

                    break;
                case "produtooportunidades":
                    ProdutoOportunidade produtoOportunidade = new ProdutoOportunidade();
                    DataContractJsonSerializer jsonSerializerProdutoOportunidade = new DataContractJsonSerializer(typeof(ProdutoOportunidade.Resultado));
                    jsonSerializerProdutoOportunidade.WriteObject(ms, produtoOportunidade.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "propostas":
                    Proposta proposta = new Proposta();
                    DataContractJsonSerializer jsonSerializerProposta = new DataContractJsonSerializer(typeof(Proposta.Resultado));
                    jsonSerializerProposta.WriteObject(ms, proposta.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "produtopropostas":
                    ProdutoProposta produtoProposta = new ProdutoProposta();
                    DataContractJsonSerializer jsonSerializerProdutoProposta = new DataContractJsonSerializer(typeof(ProdutoProposta.Resultado));
                    jsonSerializerProdutoProposta.WriteObject(ms, produtoProposta.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "contratos":
                    Contrato contrato = new Contrato();
                    DataContractJsonSerializer jsonSerializerContrato = new DataContractJsonSerializer(typeof(Contrato.Resultado));
                    jsonSerializerContrato.WriteObject(ms, contrato.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "produtocontratos":
                    ProdutoContrato produtoContrato = new ProdutoContrato();
                    DataContractJsonSerializer jsonSerializerProdutoContrato = new DataContractJsonSerializer(typeof(ProdutoContrato.Resultado));
                    jsonSerializerProdutoContrato.WriteObject(ms, produtoContrato.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "ocorrencias":
                    Ocorrencia ocorrencia = new Ocorrencia();
                    DataContractJsonSerializer jsonSerializerOcorrencia = new DataContractJsonSerializer(typeof(Ocorrencia.Resultado));
                    jsonSerializerOcorrencia.WriteObject(ms, ocorrencia.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                case "produtos":
                    Produto produto = new Produto();
                    DataContractJsonSerializer jsonSerializerProduto = new DataContractJsonSerializer(typeof(Produto.Resultado));
                    jsonSerializerProduto.WriteObject(ms, produto.ParseResultado(results, totalPaginas, totalRegistros, pagina, qtdePagina, dados, tracingService));
                    break;
                default:
                    throw new InvalidPluginExecutionException("DataContractJsonSerializer - Layout não definido");
            }
            
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }

    public class Functions
    {
        private ITracingService tracingService;

        public Functions(ITracingService ts)
        {
            tracingService = ts;
        }

        public CrmValueReturn ReturnCrmValue(DadosEntidade dados, Entity entity, string item)
        {
            var fieldValue = "";
            var fieldType = "";
            bool hasField = dados.entityData.Any(field => field.Key == item);
            if (hasField)
            {
                switch (dados.entityData[item].crmtype)
                {
                    case "guid":
                        fieldType = "string";
                        fieldValue = entity.GetAttributeValue<Guid>(dados.entityData[item].crmname).ToString();
                        break;
                    case "entityreference":
                        fieldType = "string";
                        if (entity.Contains(dados.entityData[item].crmname))
                        {
                            if (dados.entityData[item].crmtypefield == "id")
                                fieldValue = entity.GetAttributeValue<EntityReference>(dados.entityData[item].crmname).Id.ToString();
                            else
                                fieldValue = entity.GetAttributeValue<EntityReference>(dados.entityData[item].crmname).Name;
                        }
                        break;
                    case "optionset":
                        fieldType = "int";
                        fieldValue = entity.GetAttributeValue<OptionSetValue>(dados.entityData[item].crmname).Value.ToString();
                        break;
                    case "string":
                        fieldType = "string";
                        fieldValue = entity.GetAttributeValue<string>(dados.entityData[item].crmname);
                        break;
                    case "int":
                    case "integer":
                        fieldType = "int";
                        fieldValue = entity.GetAttributeValue<int>(dados.entityData[item].crmname).ToString("N0");
                        break;
                    case "bool":
                    case "boolean":
                        fieldType = "boolean";
                        fieldValue = entity.GetAttributeValue<bool>(dados.entityData[item].crmname).ToString();
                        break;
                    case "decimal":
                        fieldType = "decimal";
                        fieldValue = entity.GetAttributeValue<decimal>(dados.entityData[item].crmname).ToString("N2");
                        break;
                    case "money":
                        fieldType = "money";
                        fieldValue = entity.GetAttributeValue<Money>(dados.entityData[item].crmname).Value.ToString("N2");
                        break;
                    case "datetime":
                        fieldType = "datetime";
                        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                        var verao = timeZone.IsDaylightSavingTime(entity.GetAttributeValue<DateTime>(dados.entityData[item].crmname));
                        fieldValue = TimeZoneInfo.ConvertTime(entity.GetAttributeValue<DateTime>(dados.entityData[item].crmname), timeZone).ToString("yyyy/MM/dd HH:mm:ss");
                        break;
                    case "lookup":
                        fieldType = "string";
                        string attributeName = "";
                        if (dados.entityData[item].ismultilookup)
                        {
                            foreach (MultiLookup multi in dados.entityData[item].multiLookupItens)
                            {
                                attributeName = multi.crmlookupentityname + "." + multi.crmlookupcolumn;
                            }
                        }
                        else
                        {
                            attributeName = dados.entityData[item].crmlookupentityname + "." + dados.entityData[item].crmlookupcolumn;
                        }

                        //if (fieldAliasValue != null & fieldAliasValue.Value != null)
                        if (entity.Contains(attributeName))
                        {
                            AliasedValue fieldAliasValue = entity.GetAttributeValue<AliasedValue>(attributeName);
                            fieldValue = fieldAliasValue.Value.ToString();
                        }
                        else
                        {
                            fieldValue = null;
                        }
                        break;
                }

                // If value not empy or null
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    string colunaDePara = "";
                    if (dados.entityData[item].crmtype == "lookup")
                    {
                        colunaDePara = dados.entityData[item].crmlookupcolumn;
                        if (dados.entityData[item].ismultilookup)
                        {
                            foreach (MultiLookup multi in dados.entityData[item].multiLookupItens)
                            {
                                colunaDePara = multi.crmlookupcolumn;
                            }
                        }
                    }
                    else
                    {
                        colunaDePara = dados.entityData[item].crmname;
                    }
                    // Verify if this column exist on list before DE/PARA
                    bool inList = dados.crmDeParaDNs.Any(crmItem => crmItem.crmname == colunaDePara);
                    if (inList)
                    {
                        //tracingService.Trace($"DEPARA - Name {entity.LogicalName} - ID {entity.Id} - item {item}");
                        fieldValue = fieldValue.Trim();
                        //tracingService.Trace($"Novo Anterior {fieldValue}");
                        CheckDePara(dados, colunaDePara, ref fieldValue);
                        //tracingService.Trace($"Novo Valor {fieldValue}");
                    }
                }
            }

            return new CrmValueReturn { fieldValue = fieldValue, fieldType = fieldType };
        }

        public void CheckDePara(DadosEntidade dados, string crmField, ref string fieldValue)
        {
            string newFieldValue = "";
            // Tratamentos padrões conhecidos de campos do CRM
            switch (crmField)
            {
                case "stepname":
                    fieldValue = fieldValue.Substring(0, 1);
                    break;
            }
            CrmDeParaDN crmDeParaDN = dados.crmDeParaDNs.Single(item => item.crmname == crmField);

            // Loop Itens
            foreach (ItemDePara itemDePara in crmDeParaDN.itens)
            {
                // Lopp valores
                foreach (string valorCrm in itemDePara.crm)
                {
                    // Se valor do campo = valor do CRM atribuir valor do DN
                    if (fieldValue == valorCrm.ToString())
                    {
                        newFieldValue = itemDePara.dn.ToString();
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(newFieldValue))
                {
                    break;
                }
            }
            fieldValue = newFieldValue;
        }

        public class CrmValueReturn
        {
            public string fieldValue { get; set; }
            public string fieldType { get; set; }
        }

        public class LinkEntities
        {
            public string LinkFromEntityName { get; set; }
            public string LinkFromAttributeName { get; set; }
            public string LinkToEntityName { get; set; }
            public string LinkToAttributeName { get; set; }
            public string EntityAlias { get; set; }
            public List<string> Columns { get; set; }
            public List<LinkEntities> Links { get; set; }
        }

    }
}
