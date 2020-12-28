using System;
using System.Collections.Generic;
using System.Text;

namespace Findes.CustomAction.ExtratorCRM.Model
{
    public enum NumeroLayout
    {
        Conta = 1,
        Contato = 2,
        Oportunidade = 3,
        ProdutoOportunidade = 4,
        Proposta = 5,
        ProdutoProposta = 6,
        Contrato = 7,
        ProdutoContrato = 8,
        Ocorrencia = 9,
        Produto = 10
    }

    public enum CodigoEntidade
    {
        GLOBAL = 1,
        CNI = 2,
        FEDERACAO = 3,
        IEL = 4,
        SENAI = 5,
        SESI = 6
    }

    public enum CodigoRegional
    {
        DN = 1,
        AC = 2,
        AL = 3,
        AP = 4,
        AM = 5,
        BA = 6,
        CE = 7,
        DF = 8,
        ES = 9,
        GO = 10,
        MA = 11,
        MT = 12,
        MS = 13,
        MG = 14,
        PA = 15,
        PB = 16,
        PR = 17,
        PE = 18,
        PI = 19,
        RJ = 20,
        RN = 21,
        RS = 22,
        RO = 23,
        RR = 24,
        SC = 25,
        SP = 26,
        SE = 27,
        TO = 28,
        CETIQT = 29
    }

    public enum CodigoEstagioOportunidade
    {
        Qualificar = 1,
        Desenvolver = 2,
        Propor = 3,
        Fechar = 4
    }

    public enum CodigoStatusOportunidade
    {
        Aberta = 1,
        Ganha = 2,
        Perdida = 3
    }

    public enum CodigoMotivoPerdaOportunidade
    {
        Preço = 1,
        Prazo = 2,
        ProdutoInadequado = 3,
        CapacidadedeEntrega = 4,
        Açãodoconcorrenteutros = 5,
        Outros = 6
    }

    public enum DNSimNao
    {
        Não = 0,
        Sim = 1
    }

    public enum CodigoPapelRegional
    {
        Operador = 1,
        CoordenadorTécnico = 2,
        CoordenadorRelacionamento = 3
    }
}
