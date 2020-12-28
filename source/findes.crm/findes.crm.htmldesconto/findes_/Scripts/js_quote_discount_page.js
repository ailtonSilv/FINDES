document.onreadystatechange = function () {

    if (document.readyState == "complete") {

        window["ENTITY_SET_NAMES"] = window["ENTITY_SET_NAMES"] || JSON.stringify({
            "quotedetail": "quotedetails",
            "quote": "quotes",
            "findes_solicitacaodedesconto": "findes_solicitacaodedescontos"
        });
        window["ENTITY_PRIMARY_KEYS"] = ['{"quote":"quoteid", "systemuser":"systemuserid"}'];

        getDataParam();
    }
}

function getDataParam() {

    var vals = new Array();

    if (location.search != "") {
        vals = location.search.substr(1).split("&");
        for (var i in vals) {
            vals[i] = vals[i].replace(/\+/g, " ").split("=");
        }
        //look for the parameter named 'data'  
        var found = false;
        for (var i in vals) {
            if (vals[i][0].toLowerCase() == "data") {
                parseDataValue(vals[i][1]);
                found = true;
                break;
            }
        }
        if (!found) {
            noParams();
        }
    } else {
        noParams();
    }
}

var proposta;
var produtos = [];
var produtos_alterados = [];
var solicitacao;

function parseDataValue(datavalue) {

    if (datavalue != "") {

        //debugger;
        var urlParams = new URLSearchParams(location.search);
        var dados = new URLSearchParams(urlParams.get("Data"));

        // Recebe valores da proposta
        proposta =
        {
            id: dados.get('id'),
            action: dados.get('act'),
            vl_original: parseFloat(dados.get('vo')),
            vl_total: parseFloat(dados.get('tp')),
            vl_desc_max: parseFloat(dados.get('da')),
            vl_desc_aplicado: parseFloat(dados.get('dp')),
            vl_desc_restante: 0
        };

        // Alterar labels das colunas

        if (proposta.action == "edit") {
            var tableedit = document.getElementById("tabelaProdutos");
            var row = tableedit.getElementsByTagName("tr")[0];
            var th = row.getElementsByTagName("th");
            th[5].innerHTML = "Valor Desconto Solicitado (R$)";
            var botao_desconto_aplicado = document.getElementById("botao_desconto_aplicado");
            botao_desconto_aplicado.style.display = "none";
        }

        // solicitacao de desconto
        solicitacao = {
            id: dados.get('solicitacaoid')
        }

        // Produtos
        var dadosProdutos = true;
        var pIndex = 0;
        while (dadosProdutos) {
            if (proposta.action == "edit") {

                produtos[pIndex] =
                {
                    sequencia: pIndex + 1,
                    id: dados.get("p" + pIndex + "id"),
                    nome: dados.get('p' + pIndex + 'd'),
                    vl_atual: parseFloat(dados.get("p" + pIndex + "f")),
                    vl_margem_aplicado: parseFloat(dados.get("p" + pIndex + "m")),
                    vl_desc_aplicado: parseFloat(dados.get("p" + pIndex + "desc")) - parseFloat(dados.get("p" + pIndex + "descatual")),
                    vl_final: parseFloat(dados.get("p" + pIndex + "f")) - parseFloat(dados.get("p" + pIndex + "descatual")),
                    vl_reinvestimento: parseFloat(dados.get("p" + pIndex + "m")),
                    vl_desc: parseFloat(dados.get("p" + pIndex + "descatual")),
                    vl_desc_porcentagem: parseFloat(parseFloat(dados.get("p" + pIndex + "descatual")) / parseFloat(dados.get("p" + pIndex + "f"))) * 100
                }
            } else {
                produtos[pIndex] =
                {
                    sequencia: pIndex + 1,
                    id: dados.get("p" + pIndex + "id"),
                    nome: dados.get('p' + pIndex + 'd'),
                    vl_atual: parseFloat(dados.get("p" + pIndex + "f")),
                    vl_margem_aplicado: parseFloat(dados.get("p" + pIndex + "m")),
                    vl_desc_aplicado: parseFloat(dados.get("p" + pIndex + "desc")),
                    vl_final: parseFloat(dados.get("p" + pIndex + "f")),
                    vl_reinvestimento: parseFloat(dados.get("p" + pIndex + "m")),
                    vl_desc_porcentagem: 0,
                    vl_desc: 0.00
                }
            }
            pIndex++;

            if (dados.get("p" + pIndex + "id") == null) {
                dadosProdutos = false;
            }
        }

        // MONTA TABELA
        var table = document.getElementById("tabelaProdutos");
        var tBody = document.getElementById("bodyTable");

        for (i in produtos) {

            var tr = document.createElement("tr");
            tr.name = produtos[i].id;
            tr.id = i;

            var td0 = document.createElement("td");
            setText(td0, produtos[i].sequencia);
            tr.appendChild(td0);

            var td1 = document.createElement("td");
            setText(td1, produtos[i].nome);
            tr.appendChild(td1);

            var td2 = document.createElement("td");
            setText(td2, produtos[i].vl_atual.toLocaleString('pt-br', { minimumFractionDigits: 2 }));
            tr.appendChild(td2);

            var td7 = document.createElement("td");
            var input2 = document.createElement("input");
            input2.setAttribute("type", "text");
            input2.setAttribute("id", "reInvestiment" + i);
            input2.setAttribute("style", "border:1px solid #000000; width:60%;");
            input2.value = produtos[i].vl_reinvestimento;
            input2.addEventListener("change", setReInvestiment);
            td7.appendChild(input2);
            tr.appendChild(td7);

            var td3 = document.createElement("td");
            var input0 = document.createElement("input");
            input0.setAttribute("type", "text");
            input0.setAttribute("id", "percentDiscount" + i);
            input0.setAttribute("style", "border:1px solid #000000; width:60%;");
            input0.value = 0;
            input0.addEventListener("change", setPercentDiscount);
            td3.appendChild(input0);
            tr.appendChild(td3);

            var td4 = document.createElement("td");
            var input1 = document.createElement("input");
            input1.setAttribute("type", "text");
            input1.setAttribute("id", "moneyDiscount" + i);
            input1.setAttribute("style", "border:1px solid #000000; width:60%;");
            input1.value = produtos[i].vl_desc;
            input1.addEventListener("change", setMoneyDiscount);
            td4.appendChild(input1);
            tr.appendChild(td4);

            var td5 = document.createElement("td");

            setText(td5, produtos[i].vl_desc_aplicado.toLocaleString('pt-br', { minimumFractionDigits: 2 }));
            tr.appendChild(td5);

            var td6 = document.createElement("td");
            setText(td6, produtos[i].vl_final.toLocaleString('pt-br', { minimumFractionDigits: 2 }));
            tr.appendChild(td6);

            tBody.appendChild(tr);
        }

        table.appendChild(tBody);
        calculateValues(produtos);

    } else {
        noParams();
    }
}

function confirmDiscount() {

    var resposta;
    if (proposta.action == "edit") {
        resposta = confirm("Deseja Alterar a Solicitação de Desconto?");
    } else {
        resposta = confirm("Deseja Aplicar o Desconto?");
    }
    if (resposta == true) {

        updateQuoteDetail();

    } else {
        // Fechar
    }
}

function closeWindow() {
    window.open('', '_parent', '');
    window.close();
}

function setMoneyDiscount(event) {

    // id da Linha da tabela
    var id = event.target.parentNode.parentNode.id;

    // Celulas da linha do Produto
    var celula_produto_vl_atual = document.getElementById(id).children[3];
    var celula_produto_vl_desc_porcentagem = document.getElementById(id).children[4].children[0];
    var celula_produto_vl_desc = document.getElementById(id).children[5].children[0];
    var celula_produto_vl_final = document.getElementById(id).children[6];

    // Desconto
    var _aux_vl_desc = celula_produto_vl_desc.value;
    _aux_vl_desc = _aux_vl_desc.replace(",", ".");
    if (_aux_vl_desc > produtos[id].vl_atual) { _aux_vl_desc = produtos[id].vl_atual; }
    _aux_vl_desc = formatMoney(_aux_vl_desc);

    // Margem
    var _aux_vl_desc_porcentagem = (_aux_vl_desc / produtos[id].vl_atual) * 100;

    // Valor Final
    _aux_vl_final = produtos[id].vl_atual - _aux_vl_desc;

    // Atualiza Produto
    produtos[id].vl_desc_porcentagem = parseFloat(_aux_vl_desc_porcentagem);
    produtos[id].vl_desc = parseFloat(_aux_vl_desc);
    produtos[id].vl_final = parseFloat(_aux_vl_final);

    // Produtos alterados
    produtos_alterados[id] = produtos[id];
    calculateValues(produtos);
}

function setReInvestiment(event) {
    debugger;
    // id da Linha da tabela
    var id = event.target.parentNode.parentNode.id;

    var celula_produto_vl_reinvestimento = document.getElementById(id).children[3].children[0];

    var _aux_vl_reinvestimento = celula_produto_vl_reinvestimento.value;
    if (_aux_vl_reinvestimento > 100) {
        _aux_vl_reinvestimento = 100;
        document.getElementById(id).children[3].children[0].value = _aux_vl_reinvestimento;
    }
    if (_aux_vl_reinvestimento == "") {
        _aux_vl_reinvestimento = 0;
        document.getElementById(id).children[3].children[0].value = _aux_vl_reinvestimento;
    }
    produtos[id].vl_reinvestimento = _aux_vl_reinvestimento;
}

function setPercentDiscount(event) {
    //debugger;
    // id da Linha da tabela
    var id = event.target.parentNode.parentNode.id;
    var idInput = event.target.id;

    // Celulas da linha do Produto
    /*
    var celula_produto_vl_atual = document.getElementById(id).children[3];
    var celula_produto_vl_desc_porcentagem = document.getElementById(id).children[4].children[0];
    var celula_produto_vl_desc = document.getElementById(id).children[5].children[0];
    var celula_produto_vl_final = document.getElementById(id).children[6];
    */
    var celula_produto_vl_desc_porcentagem = document.getElementById(idInput);

    // Margem
    
    var _aux_vl_desc_porcentagem = celula_produto_vl_desc_porcentagem.value;
    _aux_vl_desc_porcentagem = _aux_vl_desc_porcentagem.replace(",", ".");
    _aux_vl_desc_porcentagem = parseFloat(_aux_vl_desc_porcentagem);
    if (_aux_vl_desc_porcentagem > 100) { _aux_vl_desc_porcentagem = 100; }
    _aux_vl_desc_porcentagem = formatMoney(_aux_vl_desc_porcentagem);
    // Desconto
    var _aux_vl_desc = produtos[id].vl_atual * _aux_vl_desc_porcentagem / 100;
    // Valor Final
    var _aux_vl_final = produtos[id].vl_atual - _aux_vl_desc;

    // Atualiza Objeto Produto 
    produtos[id].vl_desc_porcentagem = parseFloat(_aux_vl_desc_porcentagem);
    produtos[id].vl_desc = parseFloat(_aux_vl_desc);
    produtos[id].vl_final = parseFloat(_aux_vl_final);

    // Insere o produto para atualizacao
    produtos_alterados[id] = produtos[id];
    calculateValues(produtos);
}

function calculateValues(_produtos) {
    //debugger;
    // Celulas do Cabeçalho
    var celula_proposta_vl_original = document.getElementById("ValorOriginalProposta");
    var celula_proposta_vl_total = document.getElementById("TotalProposta");
    var celula_proposta_vl_desc_restante = document.getElementById("DescontoPermitido");
    var celula_proposta_vl_desc_aplicado = document.getElementById("DescontoAplicado");

    var _aux_total_desc = 0;
    var _aux_total_prop = 0;
    var _aux_desc_permitido = 0;

    for (i in _produtos) {
        var linha = document.getElementById(i);
        
        linha.children[4].children[0].value = _produtos[i].vl_desc_porcentagem.toLocaleString('pt-br', { minimumFractionDigits: 2 });
        linha.children[5].children[0].value = _produtos[i].vl_desc.toLocaleString('pt-br', { minimumFractionDigits: 2 });
        linha.children[7].textContent = _produtos[i].vl_final.toLocaleString('pt-br', { minimumFractionDigits: 2 });
        
        _aux_total_desc += (_produtos[i].vl_desc_aplicado + _produtos[i].vl_desc);
        _aux_total_prop += _produtos[i].vl_final;
    }

    // Proposta
    proposta.vl_desc_aplicado = parseFloat(_aux_total_desc);
    proposta.vl_total = parseFloat(_aux_total_prop);
    //
    _aux_desc_permitido = proposta.vl_desc_max - proposta.vl_desc_aplicado;
    proposta.vl_desc_restante = parseFloat(_aux_desc_permitido);

    // Atualiza totais 
    celula_proposta_vl_original.textContent = "R$ " + proposta.vl_original.toLocaleString('pt-br', { minimumFractionDigits: 2 });
    celula_proposta_vl_total.textContent = "R$ " + proposta.vl_total.toLocaleString('pt-br', { minimumFractionDigits: 2 });
    celula_proposta_vl_desc_restante.textContent = "R$ " + proposta.vl_desc_restante.toLocaleString('pt-br', { minimumFractionDigits: 2 });
    celula_proposta_vl_desc_aplicado.textContent = "R$ " + proposta.vl_desc_aplicado.toLocaleString('pt-br', { minimumFractionDigits: 2 });

    validacoes();
}

function validacoes() {

    // Alerta Visualmente o usuário sobre valores não permitidos
    var item_desc_permitido = document.getElementById("botao_desconto_permitido");
    var item_desc_aplicado = document.getElementById("botao_desconto_aplicado");
    var item_proposta_vl_total = document.getElementById("botao_total_proposta");

    // SE o Desconto Aplicado for MAIOR que o Desconto Permitido
    if (proposta.vl_desc_aplicado > proposta.vl_desc_max) {

        item_desc_aplicado.style.backgroundColor = "#ff4949";
        item_desc_aplicado.children[0].style.color = "#ffffff";
        item_desc_aplicado.children[1].style.color = "#ffffff";
        pageMsg("O valor limite de sua alçada foi excedido.", "warning", 5);

    } else {
        document.getElementById("botao_desconto_aplicado").removeAttribute("style");
        document.getElementById("botao_desconto_aplicado").children[0].removeAttribute("style");
        document.getElementById("botao_desconto_aplicado").children[1].removeAttribute("style");
    }

    // SE o Valor da Proposta for MENOR ou IGUAL a Zero
    if (proposta.vl_total <= 0) {
        document.getElementById("botao_total_proposta").style.backgroundColor = "#ff4949";
        document.getElementById("botao_total_proposta").children[0].style.color = "#ffffff";
        document.getElementById("botao_total_proposta").children[1].style.color = "#ffffff";

    } else {
        document.getElementById("botao_total_proposta").removeAttribute("style");
        document.getElementById("botao_total_proposta").children[0].removeAttribute("style");
        document.getElementById("botao_total_proposta").children[1].removeAttribute("style");
    }
    // SE o Desconto Restante ACABAR ou Zerar
    if (proposta.vl_desc_restante <= 0) {
        document.getElementById("botao_desconto_permitido").style.backgroundColor = "#ff4949";
        document.getElementById("botao_desconto_permitido").children[0].style.color = "#ffffff";
        document.getElementById("botao_desconto_permitido").children[1].style.color = "#ffffff";

    } else {
        document.getElementById("botao_desconto_permitido").removeAttribute("style");
        document.getElementById("botao_desconto_permitido").children[0].removeAttribute("style");
        document.getElementById("botao_desconto_permitido").children[1].removeAttribute("style");
    }
}

function updateQuoteDetail() {

    if (produtos_alterados.length > 0) {

        var requests = [];
        for (i in produtos_alterados) {

            requests[requests.length] = {
                getMetadata: function () {
                    return {
                        boundParameter: undefined,
                        parameterTypes: {},
                        operationType: 2,
                        operationName: "Update"
                    };
                },
                etn: "quotedetail",
                id: produtos_alterados[i].id,
                payload: {
                    "findes_valortotaldescontoalcada": produtos_alterados[i].vl_desc,
                    "findes_valorsomadescontoalcada": (produtos_alterados[i].vl_desc_aplicado + produtos_alterados[i].vl_desc),
                    "findes_margemlucratividade": produtos_alterados[i].vl_reinvestimento,
                    "findes_descontorecusado": false
                }
            };
        }

        parent.Xrm.WebApi.executeMultiple(requests).then(
            function success(result) {
                updateQuote();
            },
            function (error) {
                console.log(error.message);
                alert("Erro atualizar os produtos da cotação: " + error.message);
            }
        );

    } else {

        alert("Nenhum produto foi alterado");
        closeWindow();
    }
}

function updateQuote() {

    var data =
    {
        "findes_valortotaldescontoalcadajaaplicado": proposta.vl_desc_aplicado,
        "findes_valortotaldescontoalcadapermitido": proposta.vl_desc_max
    }

    parent.Xrm.WebApi.updateRecord("quote", proposta.id, data).then(
        function success(result) {

            if (proposta.action == "edit") {
                editDiscount();

            } else if (proposta.action == "add") {
                applyDiscount();
            }
        },
        function (error) {
            console.log(error.message);
            alert("Erro ao atualizar proposta:" + error.message);
        }
    );
}

function editDiscount() {

    var data =
    {
        "findes_valortotaldescontoaplicado": proposta.vl_desc_aplicado
    }

    parent.Xrm.WebApi.updateRecord("findes_solicitacaodedesconto", solicitacao.id, data).then(
        function success(result) {
            alert("Solicitação de Desconto alterada!");
            window.opener.location.reload();
            window.close();

        },
        function (error) {
            console.log(error.message);
            alert("Erro ao alterar Solicitação de Desconto:" + error.message);
        }
    );
}

function applyDiscount() {

    var request = {
        getMetadata: function () {
            return {
                boundParameter: "entity",
                parameterTypes: {
                    "entity": {
                        "typeName": "mscrm.quote",
                        "structuralProperty": 5
                    },
                    "userReference": {
                        "typeName": "mscrm.systemuser",
                        "structuralProperty": 5
                    }
                },
                operationType: 0,
                operationName: "findes_ActionAplicarDesconto"
            };
        },
        userReference: {
            id: parent.Xrm.Utility.getGlobalContext().userSettings.userId,
            entityType: "systemuser"
        },
        entity: {
            id: proposta.id,
            entityType: "quote"
        }
    };

    parent.Xrm.WebApi.execute(request).then(
        function success(result) {
            // Informa o usuário SE o desconto foi aplicado OU foi para aprovação
            if (proposta.vl_desc_aplicado > proposta.vl_desc_max) {
                alert("Desconto enviado para Aprovação");
                closeWindow();
            } else {
                alert("Desconto Aplicado!");
                closeWindow();
            }
        },
        function (error) {
            console.log(error.message);
            alert("Erro ao Aplicar Desconto: " + error.message);
            closeWindow();
        }
    );
}

function formatMoney(moneyValue) {

    moneyValue = moneyValue.toString().replace(/[^\d\.]/g, '');
    if (moneyValue == '') { moneyValue = 0; }

    return moneyValue;
}

// Mensagens
function setText(element, text) {

    if (typeof element.innerText != "undefined") {
        element.innerText = text;
    } else {
        element.textContent = text;
    }
}

function noParams() {

    var message = document.createElement("p");
    setText(message, "No data parameter was passed to this page");
    document.getElementById("error_msg").appendChild(message);
}

function pageMsg(textoMsg, tipoMsg, tempoMsg) {

    tempoMsg = tempoMsg * 1000;
    var page_msg = document.getElementById("page_msg");
    var p = document.createElement("p");
    p.className = tipoMsg;
    setText(p, textoMsg);

    if (page_msg.hasChildNodes()) {
        var item = page_msg.childNodes[0];
        item.replaceChild(page_msg, item.childNodes[0]);
    } else {
        page_msg.appendChild(p);
    }
    setTimeout(function () {
        page_msg.removeChild(page_msg.firstChild);
    }, tempoMsg);
}

function formatReal(int) {
    var tmp = int + '';
    tmp = tmp.replace(/([0-9]{2})$/g, ",$1");
    if (tmp.length > 6)
        tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

    return tmp;
}