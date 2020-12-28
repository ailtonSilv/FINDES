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
    public class Ocorrencia
    {
        public string parametroExtrator = "ActionExtratorCRM_Ocorrencias";
        public string parametroExtratorDePara = "ActionExtratorCRM_DePara_ListaItens_Ocorrencias";
        public string[] modeloBase = { "Entidade", "DataAlteracaoRegistro", "CodigoOcorrencia", "CodigoTipoOcorrencia", "NomeOcorrencia", "DescricaoOcorrencia", "CodigoStatusOcorrencia", "DataAberturaOcorrencia", "DataFechamentoOcorrencia", "CodigoOrigemOcorrencia", "Cnpj", "Cpf", "NomePessoaFisica", "CargoPessoaFisica", "EmailPessoaFisica", "TelefonePessoaFisica", "NomeResponsavel" };
        /* Modelo XML
        -- ActionExtratorCRM_Ocorrencias
        <xml>
	        <Entidade crmname='incident' />
	        <DataAlteracaoRegistro crmname='modifiedon' crmtype='datetime' />
	        <CodigoOcorrencia crmname='incidentid' crmtype='guid' />
	        <CodigoTipoOcorrencia crmname='createdon' crmtype='datetime' />
	        <NomeOcorrencia crmname='title'  crmtype='string' />
	        <DescricaoOcorrencia crmname='description' crmtype='string' />
	        <CodigoStatusOcorrencia crmname='statecode' crmtype='optionset' />
	        <DataAberturaOcorrencia crmname='createdon' crmtype='datetime' />
	        <DataFechamentoOcorrencia crmname='modifiedon' crmtype='datetime' />
	        <CodigoOrigemOcorrencia crmname='statuscode' crmtype='optionset' />
	        <Cnpj crmname='customerid' crmtype='lookup' 
		        crmlookupentityname='account' 
		        crmlookupto='accountid' 
		        crmlookupcolumn='findes_cnpj' 
		        ismultilookup='false' />
	        <Cpf crmname='customerid' crmtype='lookup' 
		        crmlookupentityname='account' 
		        crmlookupto='accountid' 
		        crmlookupcolumn='findes_cpf' 
		        ismultilookup='false' />
	        <NomePessoaFisica crmname='findes_contatopagamentoid' crmtype='lookup' 
		        crmlookupentityname='contact' 
		        crmlookupto='contactid' 
		        crmlookupcolumn='fullname' 
		        ismultilookup='false' />
	        <CargoPessoaFisica crmname='findes_contatopagamentoid' crmtype='lookup' 
		        crmlookupentityname='contact' 
		        crmlookupto='contactid' 
		        crmlookupcolumn='jobtitle' 
		        ismultilookup='false' />
	        <EmailPessoaFisica crmname='findes_contatopagamentoid' crmtype='lookup' 
		        crmlookupentityname='contact' 
		        crmlookupto='contactid' 
		        crmlookupcolumn='emailaddress1' 
		        ismultilookup='false' />
	        <TelefonePessoaFisica crmname='findes_contatopagamentoid' crmtype='lookup' 
		        crmlookupentityname='contact' 
		        crmlookupto='contactid' 
		        crmlookupcolumn='telephone1' 
		        ismultilookup='false' />
	        <NomeResponsavel crmname='ownerid' crmtype='entityreference' crmtypefield='name' />
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
            List<ItemOcorrencia> listResult = new List<ItemOcorrencia>();

            Functions funcoes = new Functions(tracingService);

            int x = (pagina * qtdePagina) - qtdePagina;
            foreach (Entity ett in collection.Entities)
            {
                try
                {
                    var item = new ItemOcorrencia
                    {
                        NumeroLayout = (int)NumeroLayout.Ocorrencia,
                        NumeroLinhaArquivo = x++,
                        CodigoRegional = (int)CodigoRegional.ES,
                        NomeSistema = "CRM",
                        Excluido = (int)DNSimNao.Não,
                    };
                    item.CodigoEntidade = int.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoEntidade").fieldValue);
                    item.DataAlteracaoRegistro = funcoes.ReturnCrmValue(dados, ett, "DataAlteracaoRegistro").fieldValue;
                    item.CodigoOcorrencia = funcoes.ReturnCrmValue(dados, ett, "CodigoOcorrencia").fieldValue;
                    item.CodigoTipoOcorrencia = int.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoTipoOcorrencia").fieldValue);
                    item.NomeOcorrencia = funcoes.ReturnCrmValue(dados, ett, "NomeOcorrencia").fieldValue;
                    item.DescricaoOcorrencia = funcoes.ReturnCrmValue(dados, ett, "DescricaoOcorrencia").fieldValue;
                    if (!string.IsNullOrEmpty(item.DescricaoOcorrencia) && item.DescricaoOcorrencia.Length > 1000) { item.DescricaoOcorrencia = item.DescricaoOcorrencia.Substring(0, 1000); }
                    item.CodigoStatusOcorrencia = int.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoStatusOcorrencia").fieldValue);
                    item.DataAberturaOcorrencia = funcoes.ReturnCrmValue(dados, ett, "DataAberturaOcorrencia").fieldValue;
                    item.DataFechamentoOcorrencia = funcoes.ReturnCrmValue(dados, ett, "DataFechamentoOcorrencia").fieldValue;
                    item.CodigoOrigemOcorrencia = int.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoOrigemOcorrencia").fieldValue);
                    item.Cnpj = funcoes.ReturnCrmValue(dados, ett, "Cnpj").fieldValue;
                    item.Cpf = funcoes.ReturnCrmValue(dados, ett, "Cpf").fieldValue;
                    item.NomePessoaFisica = funcoes.ReturnCrmValue(dados, ett, "NomePessoaFisica").fieldValue;
                    item.CargoPessoaFisica = funcoes.ReturnCrmValue(dados, ett, "CargoPessoaFisica").fieldValue;
                    item.EmailPessoaFisica = funcoes.ReturnCrmValue(dados, ett, "EmailPessoaFisica").fieldValue;
                    item.TelefonePessoaFisica = funcoes.ReturnCrmValue(dados, ett, "TelefonePessoaFisica").fieldValue;
                    item.NomeResponsavel = funcoes.ReturnCrmValue(dados, ett, "NomeResponsavel").fieldValue;

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
            public List<ItemOcorrencia> content { get; set; }
        }

        /// <summary>
        /// Modelo da Conta
        /// </summary>
        [DataContract]
        public class ItemOcorrencia
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

            [DataMember(Name = "codigoOcorrencia")]
            public string CodigoOcorrencia { get; set; }

            [DataMember(Name = "codigoTipoOcorrencia")]
            public int CodigoTipoOcorrencia { get; set; }

            [DataMember(Name = "nomeOcorrencia")]
            public string NomeOcorrencia { get; set; }

            [DataMember(Name = "descricaoOcorrencia")]
            public string DescricaoOcorrencia { get; set; }

            [DataMember(Name = "codigoStatusOcorrencia")]
            public int CodigoStatusOcorrencia { get; set; }

            [DataMember(Name = "dataAberturaOcorrencia")]
            public string DataAberturaOcorrencia { get; set; }

            [DataMember(Name = "dataFechamentoOcorrencia")]
            public string DataFechamentoOcorrencia { get; set; }

            [DataMember(Name = "codigoOrigemOcorrencia")]
            public int CodigoOrigemOcorrencia { get; set; }

            [DataMember(Name = "cnpj")]
            public string Cnpj { get; set; }

            [DataMember(Name = "cpf")]
            public string Cpf { get; set; }

            [DataMember(Name = "nomePessoaFisica")]
            public string NomePessoaFisica { get; set; }

            [DataMember(Name = "cargoPessoaFisica")]
            public string CargoPessoaFisica { get; set; }

            [DataMember(Name = "emailPessoaFisica")]
            public string EmailPessoaFisica { get; set; }

            [DataMember(Name = "telefonePessoaFisica")]
            public string TelefonePessoaFisica { get; set; }

            [DataMember(Name = "nomeResponsavel")]
            public string NomeResponsavel { get; set; }
        }
    }
}
