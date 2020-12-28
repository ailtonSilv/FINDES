using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Findes.CustomAction.ExtratorCRM.Model;

namespace Findes.CustomAction.ExtratorCRM
{
    public class ProdutoContrato
    {
        public string parametroExtrator = "ActionExtratorCRM_ProdutoContratos";
        public string parametroExtratorDePara = "ActionExtratorCRM_DePara_ListaItens_ProdutoContratos";
        public string[] modeloBase = { "Entidade", "DataAlteracaoRegistro", "Cnpj", "CodigoContratoRegional", "CodigoProduto", "NomeProduto", "ProdutoNacionalRegional", "QuantidadeProduto", "DescricaoUnidadeMedida", "ValorProduto", "ValorTotalProduto" };
        /* Modelo XML
        -- ActionExtratorCRM_ProdutoContratos
        <xml>
            <Entidade crmname='salesorderdetail' />
            <DataAlteracaoRegistro crmname='modifiedon' crmtype='datetime' />
	        <Cnpj 
		        crmname='salesorderid' crmtype='lookup' 
		        crmlookupentityname='salesorder' 
		        crmlookupto='salesorderid' 
		        crmlookupcolumn='customerid' 
		        ismultilookup='true'>
		        <Multilookup crmname='customerid'
			        crmfromentityname='salesorder' 
			        crmlookupentityname='account' 
			        crmlookupto='accountid' 
			        crmlookupcolumn='findes_cnpj' />
	        </Cnpj>
            <CodigoContratoRegional crmname='salesorderid' crmtype='entityreference' crmtypefield='id' />
            <CodigoProduto crmname='productid' crmtype='entityreference' crmtypefield='id' />
            <NomeProduto crmname='productid' crmtype='entityreference' crmtypefield='name' />
            <ProdutoNacionalRegional crmname='productid' crmtype='lookup' crmlookupentityname='product' crmlookupto='productid' crmlookupcolumn='findes_homologadodn' ismultilookup='false' />	
            <QuantidadeProduto crmname='quantity' crmtype='decimal' />
            <DescricaoUnidadeMedida crmname='uomid' crmtype='entityreference' crmtypefield='name' />
            <ValorProduto crmname='priceperunit' crmtype='money' />
            <ValorTotalProduto crmname='extendedamount' crmtype='money' />
            <CodigoEntidade crmname='findes_coligadaid' crmtype='lookup' crmlookupentityname='findes_coligada' crmlookupto='findes_coligadaid' crmlookupcolumn='findes_codigodn' ismultilookup='false' />
            <NomeCategoria crmname='findes_categoriaid' crmtype='entityreference' crmtypefield='name' />
        </xml>
         * */

        /// <summary>
        /// Trata o resultado no modelo previsto
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="totalPaginas"></param>
        /// <param name="totalRegistros"></param>
        /// <param name="pagina"></param>
        /// <param name="qtdePagina"></param>
        /// <returns>modelo Resultado</returns>
        public Resultado ParseResultado(EntityCollection collection, int totalPaginas, int totalRegistros, int pagina, int qtdePagina, DadosEntidade dados, ITracingService tracingService)
        {
            List<ItemProdutoContrato> listResult = new List<ItemProdutoContrato>();

            Functions funcoes = new Functions(tracingService);

            int x = (pagina * qtdePagina) - qtdePagina;
            foreach (Entity ett in collection.Entities)
            {
                try
                {
                    var item = new ItemProdutoContrato
                    {
                        NumeroLayout = (int)NumeroLayout.ProdutoContrato,
                        NumeroLinhaArquivo = x++,
                        CodigoRegional = (int)CodigoRegional.ES,
                        NomeSistema = "CRM",
                        Excluido = (int)DNSimNao.Não,
                    };
                    item.CodigoEntidade = Int32.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoEntidade").fieldValue);
                    item.DataAlteracaoRegistro = funcoes.ReturnCrmValue(dados, ett, "DataAlteracaoRegistro").fieldValue;
                    item.Cnpj = funcoes.ReturnCrmValue(dados, ett, "Cnpj").fieldValue;
                    item.CodigoContratoRegional = funcoes.ReturnCrmValue(dados, ett, "CodigoContratoRegional").fieldValue;
                    item.CodigoProduto = funcoes.ReturnCrmValue(dados, ett, "CodigoProduto").fieldValue;
                    item.ProdutoNacionalRegional = (funcoes.ReturnCrmValue(dados, ett, "ProdutoNacionalRegional").fieldValue) == "True" ? 1 : 0;
                    item.ValorProduto = double.Parse(funcoes.ReturnCrmValue(dados, ett, "ValorProduto").fieldValue);
                    item.ValorTotalProduto = double.Parse(funcoes.ReturnCrmValue(dados, ett, "ValorTotalProduto").fieldValue);
                    decimal decQtdeProduto = decimal.Parse(funcoes.ReturnCrmValue(dados, ett, "QuantidadeProduto").fieldValue);
                    item.QuantidadeProduto = Convert.ToInt32(Math.Floor(Convert.ToDouble(decQtdeProduto)));
                    item.DescricaoUnidadeMedida = funcoes.ReturnCrmValue(dados, ett, "DescricaoUnidadeMedida").fieldValue;
                    item.NomeCategoria = funcoes.ReturnCrmValue(dados, ett, "NomeCategoria").fieldValue;
                    item.NomeProduto = funcoes.ReturnCrmValue(dados, ett, "NomeProduto").fieldValue;

                    listResult.Add(item);
                }
                catch (Exception ex)
                {
                    tracingService.Trace($"https://sesisenaies.crm2.dynamics.com/main.aspx?etn={ett.LogicalName}&id={ett.Id}&pagetype=entityrecord");
                    tracingService.Trace($"CWF ERROR: {ett.LogicalName} ParseResultado => ID {ett.Id} / Linha {x++} / Exception => {ex.Message} / InnerException => {ex.InnerException} / StackTrace => {ex.StackTrace}");
                    tracingService.Trace("==== End Item. ====");
                }
            }

            var r = new Resultado
            {
                content = listResult,
                number = pagina - 1,
                numberOfElements = collection.Entities.Count,
                totalElements = totalRegistros,
                size = qtdePagina,
                first = pagina == 1 ? true : false,
                last = pagina == totalPaginas ? true : false,
                totalPages = collection.Entities.Count == 0 ? 0 : totalPaginas
            };

            return r;
        }

        /// <summary>
        /// Modelo da Requisição
        /// </summary>
        [DataContract]
        public class Resultado
        {
            /// <summary>
            /// quantidade de registros retornados
            /// </summary>
            [DataMember]
            public int numberOfElements { get; set; }
            /// <summary>
            /// número da página
            /// </summary>
            [DataMember]
            public int number { get; set; }
            /// <summary>
            /// quantidade de registros por página
            /// </summary>
            [DataMember]
            public int size { get; set; }
            /// <summary>
            /// total de páginas
            /// </summary>
            [DataMember]
            public int totalPages { get; set; }
            /// <summary>
            /// total de registros
            /// </summary>
            [DataMember]
            public int totalElements { get; set; }
            /// <summary>
            /// indica se é a última página
            /// </summary>
            [DataMember]
            public bool last { get; set; }
            /// <summary>
            /// indica se é a primeira página
            /// </summary>
            [DataMember]
            public bool first { get; set; }
            [DataMember]
            public List<ItemProdutoContrato> content { get; set; }
        }

        /// <summary>
        /// Modelo da Conta
        /// </summary>
        [DataContract]
        public class ItemProdutoContrato
        {
            /// <summary>
            /// Número do Layout, conforme numeração na coluna Domínio.
            /// </summary>
            /// <remarks>
            /// Tamanho 5
            /// Nulo = Não
            /// Tipo = Integer
            /// Formato = 
            /// </remarks>
            /// <example>
            /// 1 - Conta; 2 - Contato; 3 - Oportunidade; 4 - Produto Oportunidade; 5 - Proposta; 6 - Produto Proposta; 7 - Contrato; 8 - Produto Contrato; 9 - Ocorrência; 10 - Produto.
            /// </example>
            [DataMember(Name = "numeroLayout")]
            public int NumeroLayout { get; set; }
            /// <summary>
            /// Número da Linha no arquivo (sequencial).
            /// </summary>
            /// <remarks>
            /// Tamanho 10
            /// Nulo = Não
            /// Tipo = Integer
            /// </remarks>
            [DataMember(Name = "numeroLinhaArquivo")]
            public int NumeroLinhaArquivo { get; set; }
            /// <summary>
            /// Data de alteração do registro no sistema do regional.
            /// </summary>
            /// <remarks>
            /// Tamanho 26
            /// Nulo = Sim
            /// Tipo = DateTime
            /// Formato = dd/MM/yyyy HH:mm
            /// </remarks>
            [DataMember(Name = "dataAlteracaoRegistro")]
            public string DataAlteracaoRegistro { get; set; }
            /// <summary>
            /// Indica se o registro foi excluído ou não da base de dados do regional, conforme numeração na coluna Domínio.
            /// </summary>
            /// <remarks>
            /// Tamanho 2
            /// Nulo = Não
            /// Tipo = Integer
            /// </remarks>
            /// <example>
            /// 0 - Não; 1 - Sim
            /// </example>
            [DataMember(Name = "excluido")]
            public int Excluido { get; set; }
            /// <summary>
            /// Código da entidade conforme descrito na coluna Domínio.
            /// </summary>
            /// <remarks>
            /// Tamanho 2
            /// Nulo = Sim
            /// Tipo = Integer
            /// </remarks>
            /// <example>
            /// 6- SESI; 5 - SENAI; 4 - IEL; 3 - FEDERACAO; 2 - CNI;
            /// </example>
            [DataMember(Name = "codigoEntidade")]
            public int CodigoEntidade { get; set; }
            /// <summary>
            /// Sigla do departamento regional, conforme numeração na coluna Domínio.
            /// </summary>
            /// <remarks>
            /// Tamanho 2
            /// Nulo = Não  
            /// Tipo = Integer
            /// </remarks>
            /// <example>
            /// 1 - DN; 2 - AC; 3 - AL; 4 - AP; 5 - AM; 6 - BA; 7 - CE; 8 - DF; 9 - ES; 10 - GO; 11 - MA; 12 - MT; 13 - MS; 14 - MG; 15 - PA; 16 - PB; 17 - PR; 18 - PE; 19 - PI; 20 - RJ; 21 - RN; 22 - RS; 23 - RO; 24 - RR; 25 - SC; 26 - SP; 27 - SE; 28 - TO; 29 - CETIQT
            /// </example>
            [DataMember(Name = "codigoRegional")]
            public int CodigoRegional { get; set; }
            /// <summary>
            /// Nome do sistema de origem da informação no regional.
            /// </summary>
            /// <remarks>
            /// Tamanho 30
            /// Nulo = Sim  
            /// Tipo = String
            /// </remarks>
            [DataMember(Name = "nomeSistema")]
            public string NomeSistema { get; set; }

            [DataMember(Name = "cnpj")]
            public string Cnpj { get; set; }

            [DataMember(Name = "codigoContratoRegional")]
            public string CodigoContratoRegional { get; set; }

            [DataMember(Name = "codigoProduto")]
            public string CodigoProduto { get; set; }

            [DataMember(Name = "nomeProduto")]
            public string NomeProduto { get; set; }

            [DataMember(Name = "produtoNacionalRegional")]
            public int ProdutoNacionalRegional { get; set; }

            [DataMember(Name = "valorProduto")]
            public double ValorProduto { get; set; }

            [DataMember(Name = "valorTotalProduto")]
            public double ValorTotalProduto { get; set; }

            [DataMember(Name = "quantidadeProduto")]
            public int? QuantidadeProduto { get; set; }

            [DataMember(Name = "descricaoUnidadeMedida")]
            public string DescricaoUnidadeMedida { get; set; }

            [DataMember(Name = "nomeCategoria")]
            public string NomeCategoria { get; set; }
        }
    }
}
