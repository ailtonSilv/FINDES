using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Findes.CustomAction.ExtratorCRM.Model;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Findes.CustomAction.ExtratorCRM
{
    public class Conta
    {
        public string parametroExtrator = "ActionExtratorCRM_Contas";
        public string parametroExtratorDePara = "ActionExtratorCRM_DePara_ListaItens_Contas";
        public string[] modeloBase = { "Entidade", "DataAlteracaoRegistro", "Cnpj", "DataCriacaoConta", "RazaoSocial" };
        /* Modelo Xml
        <xml>
	        <Entidade crmname='account' />
	        <DataAlteracaoRegistro crmname='modifiedon' crmtype='datetime' />
	        <Cnpj crmname='findes_cnpj' crmtype='string' />
	        <DataCriacaoConta crmname='createdon' crmtype='datetime' />
	        <RazaoSocial crmname='findes_razaosocial' crmtype='string' />
        </xml>
        */

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
            List<ItemConta> listResult = new List<ItemConta>();

            Functions funcoes = new Functions(tracingService);

            int x = (pagina * qtdePagina) - qtdePagina;
            foreach (Entity ett in collection.Entities)
            {
                try
                {
                    var item = new ItemConta
                    {
                        NumeroLayout = (int)NumeroLayout.Conta,
                        NumeroLinhaArquivo = x++,
                        CodigoRegional = (int)CodigoRegional.ES,
                        NomeSistema = "CRM",
                        Excluido = (int)DNSimNao.Não
                    };
                    item.CodigoEntidade = (int)CodigoEntidade.FEDERACAO;
                    item.DataAlteracaoRegistro = funcoes.ReturnCrmValue(dados, ett, "DataAlteracaoRegistro").fieldValue;
                    item.Cnpj = funcoes.ReturnCrmValue(dados, ett, "Cnpj").fieldValue;
                    item.DataCriacaoConta = funcoes.ReturnCrmValue(dados, ett, "DataCriacaoConta").fieldValue;
                    item.RazaoSocial = funcoes.ReturnCrmValue(dados, ett, "RazaoSocial").fieldValue;
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
            public List<ItemConta> content { get; set; }
        }

        /// <summary>
        /// Modelo da Conta
        /// </summary>
        [DataContract]
        public class ItemConta
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
            public int? CodigoEntidade { get; set; }
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
            /// <summary>
            /// CNPJ 14 com máscara
            /// </summary>
            /// <remarks>
            /// Tamanho 18
            /// Nulo = Não  
            /// Tipo = String
            /// Formato = ##.###.###/####-##
            /// </remarks>
            [DataMember(Name = "cnpj")]
            public string Cnpj { get; set; }
            /// <summary>
            /// Data de Criação da Conta
            /// </summary>
            /// <remarks>
            /// Tamanho 26
            /// Nulo = Sim  
            /// Tipo = DateTime
            /// Formato = dd/MM/yyyy HH:mm
            /// </remarks>
            [DataMember(Name = "dataCriacaoConta")]
            public string DataCriacaoConta { get; set; }
            /// <summary>
            /// Razão Social da empresa com a qual se está interagindo. Caso o CNPJ não conste na base do CRM nacional, ou não esteja na base SIGA, deverá trazer o nome da conta, independente dos demais campos obrigatórios do cadastro de conta
            /// </summary>
            /// <remarks>
            /// Tamanho 300
            /// Nulo = Não  
            /// Tipo = String
            /// </remarks>
            [DataMember(Name = "razaoSocial")]
            public string RazaoSocial { get; set; }
        }
    }
}
