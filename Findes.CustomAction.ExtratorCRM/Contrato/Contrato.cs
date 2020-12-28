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
    public class Contrato
    {
        public string parametroExtrator = "ActionExtratorCRM_Contratos";
        public string parametroExtratorDePara = "ActionExtratorCRM_DePara_ListaItens_Contratos";
        public string[] modeloBase = { "Entidade", "DataAlteracaoRegistro", "CodigoContratoRegional", "DataContrato", "DescricaoContrato", "CodigoTipoContrato", "DataInicioContrato", "DataFimContrato", "CodigoStatusContrato", "CodigoMotivoCancelamentoSuspensaoContrato", "Cnpj", "NomePessoaFisica", "CargoPessoaFisica", "EmailPessoaFisica", "TelefonePessoaFisica", "NomeResponsavel", "ValorTotalContrato", "CodigoOportunidadeOrigem" };
        /* Modelo XML
        -- ActionExtratorCRM_Contratos
        <xml>
	        <Entidade crmname='salesorder' />
	        <DataAlteracaoRegistro crmname='modifiedon' crmtype='datetime' />
	        <CodigoContratoRegional crmname='salesorderid' crmtype='guid' />
	        <DataContrato crmname='createdon' crmtype='datetime' />
	        <DescricaoContrato crmname='description'  crmtype='string' />
	        <CodigoTipoContrato crmname='statecode' crmtype='optionset' />
	        <DataInicioContrato crmname='findes_iniciocontrato' crmtype='datetime' />
	        <DataFimContrato crmname='findes_terminocontrato' crmtype='datetime' />
	        <CodigoStatusContrato crmname='statecode' crmtype='optionset' />
	        <CodigoMotivoCancelamentoSuspensaoContrato crmname='statuscode' crmtype='optionset' />
	        <Cnpj crmname='customerid' crmtype='lookup' 
		        crmlookupentityname='account' 
		        crmlookupto='accountid' 
		        crmlookupcolumn='findes_cnpj' 
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
	        <ValorTotalContrato crmname='totalamount' crmtype='money' />
	        <CodigoOportunidadeOrigem crmname='opportunityid' crmtype='entityreference' crmtypefield='id' />
        </xml>

        > ActionExtratorCRM_DePara_ListaItens_Contrato:
        AX4B_valor (ActionExtratorCRM_DePara_Contrato.statecode,ActionExtratorCRM_DePara_Contrato.statuscode)

        -- ActionExtratorCRM_DePara_Contrato.statecode
        <xml>
            <DeParaEntidade crmname='salesorder' />
            <Campo dnname='CodigoStatusContrato' crmname='statecode' />
            <Valores>
                <item name='Ativo' dn='1' crm='0' />
                <item name='Suspenso' dn='2' crm='' />
                <item name='Renovado' dn='3' crm='' />
                <item name='Cancelado' dn='4' crm='2' />
                <item name='Aditivado' dn='5' crm='' />
                <item name='Expirado' dn='6' crm='' />
            </Valores>
        </xml>

        -- ActionExtratorCRM_DePara_Contrato.statuscode
        <xml>
	        <DeParaEntidade crmname='salesorder' />
	        <Campo dnname='CodigoMotivoCancelamentoSuspensaoContrato' crmname='statuscode' />
	        <Valores>
		        <item name='Preço' dn='1' crm='' />
		        <item name='Prazo' dn='2' crm='' />
		        <item name='Produto Inadequado' dn='3' crm='' />
		        <item name='Cancelado' dn='4' crm='482870000' />
		        <item name='Capacidade de Entrega' dn='5' crm='' />
		        <item name='Ação do concorrente' dn='6' crm='' />
		        <item name='Outros' dn='7' crm='' />
            </Valores>
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
            List<ItemContrato> listResult = new List<ItemContrato>();

            Functions funcoes = new Functions(tracingService);

            int x = (pagina * qtdePagina) - qtdePagina;
            foreach (Entity ett in collection.Entities)
            {
                try
                {
                    var item = new ItemContrato
                    {
                        NumeroLayout = (int)NumeroLayout.Contrato,
                        NumeroLinhaArquivo = x++,
                        CodigoRegional = (int)CodigoRegional.ES,
                        NomeSistema = "CRM",
                        Excluido = (int)DNSimNao.Não,
                        NegociacaoBaseNacional = (int)DNSimNao.Não,
                        CodigoPapelRegional = (int)CodigoPapelRegional.Operador,
                        OperadorAC = (int)DNSimNao.Não,
                        OperadorAL = (int)DNSimNao.Não,
                        OperadorAM = (int)DNSimNao.Não,
                        OperadorAP = (int)DNSimNao.Não,
                        OperadorBA = (int)DNSimNao.Não,
                        OperadorCE = (int)DNSimNao.Não,
                        OperadorDF = (int)DNSimNao.Não,
                        OperadorES = (int)DNSimNao.Sim,
                        OperadorGO = (int)DNSimNao.Não,
                        OperadorMA = (int)DNSimNao.Não,
                        OperadorMG = (int)DNSimNao.Não,
                        OperadorMS = (int)DNSimNao.Não,
                        OperadorMT = (int)DNSimNao.Não,
                        OperadorPA = (int)DNSimNao.Não,
                        OperadorPB = (int)DNSimNao.Não,
                        OperadorPE = (int)DNSimNao.Não,
                        OperadorPI = (int)DNSimNao.Não,
                        OperadorPR = (int)DNSimNao.Não,
                        OperadorRJ = (int)DNSimNao.Não,
                        OperadorRN = (int)DNSimNao.Não,
                        OperadorRO = (int)DNSimNao.Não,
                        OperadorRR = (int)DNSimNao.Não,
                        OperadorRS = (int)DNSimNao.Não,
                        OperadorSC = (int)DNSimNao.Não,
                        OperadorSE = (int)DNSimNao.Não,
                        OperadorSP = (int)DNSimNao.Não,
                        OperadorTO = (int)DNSimNao.Não
                    };
                    item.CodigoEntidade = Int32.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoEntidade").fieldValue);
                    item.DataAlteracaoRegistro = funcoes.ReturnCrmValue(dados, ett, "DataAlteracaoRegistro").fieldValue;
                    item.CodigoContratoRegional = funcoes.ReturnCrmValue(dados, ett, "CodigoContratoRegional").fieldValue;
                    item.DataContrato = funcoes.ReturnCrmValue(dados, ett, "DataContrato").fieldValue;
                    item.DescricaoContrato = funcoes.ReturnCrmValue(dados, ett, "DescricaoContrato").fieldValue;
                    item.CodigoTipoContrato = int.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoTipoContrato").fieldValue);
                    item.DataInicioContrato = funcoes.ReturnCrmValue(dados, ett, "DataInicioContrato").fieldValue;
                    item.DataFimContrato = funcoes.ReturnCrmValue(dados, ett, "DataFimContrato").fieldValue;
                    item.CodigoStatusContrato = int.Parse(funcoes.ReturnCrmValue(dados, ett, "CodigoStatusContrato").fieldValue);
                    if (item.CodigoStatusContrato == 2 || item.CodigoStatusContrato == 4)
                    {
                        string strCodigoMotivoCancelamentoSuspensaoContrato = funcoes.ReturnCrmValue(dados, ett, "CodigoMotivoCancelamentoSuspensaoContrato").fieldValue;
                        if (!string.IsNullOrEmpty(strCodigoMotivoCancelamentoSuspensaoContrato)) { item.CodigoMotivoCancelamentoSuspensaoContrato = int.Parse(strCodigoMotivoCancelamentoSuspensaoContrato); }
                    }
                    else { item.CodigoMotivoCancelamentoSuspensaoContrato = null; }

                    item.Cnpj = funcoes.ReturnCrmValue(dados, ett, "Cnpj").fieldValue;
                    item.NomePessoaFisica = funcoes.ReturnCrmValue(dados, ett, "NomePessoaFisica").fieldValue;
                    item.CargoPessoaFisica = funcoes.ReturnCrmValue(dados, ett, "CargoPessoaFisica").fieldValue;
                    item.EmailPessoaFisica = funcoes.ReturnCrmValue(dados, ett, "EmailPessoaFisica").fieldValue;
                    item.TelefonePessoaFisica = funcoes.ReturnCrmValue(dados, ett, "TelefonePessoaFisica").fieldValue;
                    item.NomeResponsavel = funcoes.ReturnCrmValue(dados, ett, "NomeResponsavel").fieldValue;
                    item.ValorTotalContrato = double.Parse(funcoes.ReturnCrmValue(dados, ett, "ValorTotalContrato").fieldValue);
                    item.CodigoOportunidadeOrigem = funcoes.ReturnCrmValue(dados, ett, "CodigoOportunidadeOrigem").fieldValue;
                    item.CodigoPropostaOrigem = funcoes.ReturnCrmValue(dados, ett, "CodigoPropostaOrigem").fieldValue;

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
            public List<ItemContrato> content { get; set; }
        }

        /// <summary>
        /// Modelo da Conta
        /// </summary>
        [DataContract]
        public class ItemContrato
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

            [DataMember(Name = "codigoContratoRegional")]
            public string CodigoContratoRegional { get; set; }

            [DataMember(Name = "dataContrato")]
            public string DataContrato { get; set; }

            [DataMember(Name = "descricaoContrato")]
            public string DescricaoContrato { get; set; }

            [DataMember(Name = "codigoTipoContrato")]
            public int CodigoTipoContrato { get; set; }

            [DataMember(Name = "dataInicioContrato")]
            public string DataInicioContrato { get; set; }

            [DataMember(Name = "dataFimContrato")]
            public string DataFimContrato { get; set; }

            [DataMember(Name = "codigoStatusContrato")]
            public int CodigoStatusContrato { get; set; }

            [DataMember(Name = "codigoMotivoCancelamentoSuspensaoContrato")]
            public int? CodigoMotivoCancelamentoSuspensaoContrato { get; set; }

            [DataMember(Name = "cnpj")]
            public string Cnpj { get; set; }

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

            [DataMember(Name = "valorTotalContrato")]
            public double ValorTotalContrato { get; set; }
            
            [DataMember(Name = "negociacaoBaseNacional")]
            public int NegociacaoBaseNacional { get; set; }

            [DataMember(Name = "codigoPapelRegional")]
            public int CodigoPapelRegional { get; set; }

            [DataMember(Name = "codigoOportunidadeOrigem")]
            public string CodigoOportunidadeOrigem { get; set; }

            [DataMember(Name = "codigoPropostaOrigem")]
            public string CodigoPropostaOrigem { get; set; }

            [DataMember(Name = "operadorAC")]
            public int OperadorAC { get; set; }

            [DataMember(Name = "operadorAL")]
            public int OperadorAL { get; set; }

            [DataMember(Name = "operadorAP")]
            public int OperadorAP { get; set; }

            [DataMember(Name = "operadorAM")]
            public int OperadorAM { get; set; }

            [DataMember(Name = "operadorBA")]
            public int OperadorBA { get; set; }

            [DataMember(Name = "operadorCE")]
            public int OperadorCE { get; set; }

            [DataMember(Name = "operadorDF")]
            public int OperadorDF { get; set; }

            [DataMember(Name = "operadorES")]
            public int OperadorES { get; set; }

            [DataMember(Name = "operadorGO")]
            public int OperadorGO { get; set; }

            [DataMember(Name = "operadorMA")]
            public int OperadorMA { get; set; }

            [DataMember(Name = "operadorMT")]
            public int OperadorMT { get; set; }

            [DataMember(Name = "operadorMS")]
            public int OperadorMS { get; set; }

            [DataMember(Name = "operadorMG")]
            public int OperadorMG { get; set; }

            [DataMember(Name = "operadorPA")]
            public int OperadorPA { get; set; }

            [DataMember(Name = "operadorPB")]
            public int OperadorPB { get; set; }

            [DataMember(Name = "operadorPR")]
            public int OperadorPR { get; set; }

            [DataMember(Name = "operadorPE")]
            public int OperadorPE { get; set; }

            [DataMember(Name = "operadorPI")]
            public int OperadorPI { get; set; }

            [DataMember(Name = "operadorRJ")]
            public int OperadorRJ { get; set; }

            [DataMember(Name = "operadorRN")]
            public int OperadorRN { get; set; }

            [DataMember(Name = "operadorRS")]
            public int OperadorRS { get; set; }

            [DataMember(Name = "operadorRO")]
            public int OperadorRO { get; set; }

            [DataMember(Name = "operadorRR")]
            public int OperadorRR { get; set; }

            [DataMember(Name = "operadorSC")]
            public int OperadorSC { get; set; }

            [DataMember(Name = "operadorSP")]
            public int OperadorSP { get; set; }

            [DataMember(Name = "operadorSE")]
            public int OperadorSE { get; set; }

            [DataMember(Name = "operadorTO")]
            public int OperadorTO { get; set; }
        }
    }
}
