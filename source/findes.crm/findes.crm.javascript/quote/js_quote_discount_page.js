document.onreadystatechange = function () {

    if (document.readyState == "complete") {

        window["ENTITY_SET_NAMES"] = window["ENTITY_SET_NAMES"] || JSON.stringify({
            "quotedetail": "quotedetails",
            "quote": "quotes"
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
var produto = [];
var produtos_alterados = [];

function parseDataValue(datavalue) {

    if (datavalue != "") {

        var vals = new Array();
        vals = decodeURIComponent(datavalue).split("&");

        for (var i in vals) {
            vals[i] = vals[i].replace(/\+/g, " ").split("=");
        }

        debugger;
        // Recebe valores da proposta
        proposta = {
            id: vals[0][1],
            vl_total: parseFloat(vals[1][1]),
            vl_desc_max: parseFloat(vals[2][1]),
            vl_desc_restante: parseFloat(vals[2][1]),
            vl_desc_aplicado: parseFloat(vals[3][1])
        };

        vals.shift();
        vals.shift();
        vals.shift();
        vals.shift();

        // Recebe produtos da proposta
        for (i in vals) {
            produto[produto.length] = {
                sequencia: parseInt([i]) + 1,
                id: vals[4][1],
                nome: vals[0][1],
                vl_atual: parseFloat(vals[1][1]),
                vl_margem: parseFloat(vals[2][1]),
                vl_desc: parseFloat(vals[3][1]),
                vl_final: parseFloat(vals[1][1] - vals[3][1])
            }
            for (var i = 0; i < 5; i++) {
                vals.shift();
            }
        }

        // MONTA TABELA
        var table = document.getElementById("tabelaProdutos");
        var tBody = document.getElementById("bodyTable");

        for (i in produto) {
            var tr = document.createElement("tr");
            tr.id = produto[i].id;

            var td = document.createElement("td");
            setText(td, produto[i].sequencia);
            tr.appendChild(td);

            td = document.createElement("td");
            setText(td, produto[i].nome);
            tr.appendChild(td);

            td = document.createElement("td");
            setText(td, produto[i].vl_atual.toFixed(2));
            tr.appendChild(td);

            td = document.createElement("td");
            setText(td, produto[i].vl_margem.toFixed(2));
            tr.appendChild(td);

            td = document.createElement("td");
            var input = document.createElement("input");
            input.id = i;
            input.pattern = "[0-9]+([\,.][0-9]+)?";
            input.value = produto[i].vl_desc.toFixed(2);
            input.addEventListener("keyup", setDiscount);
            td.appendChild(input);
            tr.appendChild(td);

            td = document.createElement("td");
            setText(td, produto[i].vl_final.toFixed(2));
            tr.appendChild(td);

            tBody.appendChild(tr);
        }
        table.appendChild(tBody);

        load_values();
        validacoes();

    } else {
        noParams();
    }
}

function noParams() {

    var message = document.createElement("p");
    setText(message, "No data parameter was passed to this page");
    document.getElementById("error_msg").appendChild(message);
}

//Added for cross browser support.  
function setText(element, text) {

    if (typeof element.innerText != "undefined") {
        element.innerText = text;
    } else {
        element.textContent = text;
    }
}

function setDiscount(event) {

    debugger;
    // index do Produto alterado 
    var id = parseInt(event.target.id);

    // celulas da linha
    var celulas = document.getElementById(produto[id].id).children;

    // Desconto atualizado
    var _aux_vl_desc = document.getElementById(id).value;
    _aux_vl_desc = (_aux_vl_desc.length > 0) ? parseFloat(_aux_vl_desc) : 0;
    _aux_vl_desc = isFinite(_aux_vl_desc) ? parseFloat(_aux_vl_desc) : 0;

    // Valor Final do produto Atualizado
    var _aux_vl_final = produto[id].vl_atual - _aux_vl_desc;

    // atualiza objeto produto 
    produto[id].vl_desc = parseFloat(_aux_vl_desc);
    produto[id].vl_final = parseFloat(_aux_vl_final);

    // Produtos alterados
    produtos_alterados[id] = produto[id];

    // Calcula Total da proposta e descontos 
    CalculaTotais();
    validacoes();
}

function CalculaTotais() {

    // Calcula Total da proposta e descontos 
    var _total_descontos = 0;
    var _total_proposta = 0;

    for (i in produto) {
        _total_descontos += produto[i].vl_desc;
        _total_proposta += produto[i].vl_final;
    }
    proposta.vl_desc_aplicado = parseFloat(_total_descontos);
    proposta.vl_total = parseFloat(_total_proposta);

    var _desconto_permitido = proposta.vl_desc_max - proposta.vl_desc_aplicado;
    proposta.vl_desc_restante = parseFloat(_desconto_permitido);

    // Atualiza Linha Proposta HTML
    document.getElementById("TotalProposta").textContent = "R$ " + proposta.vl_total.toFixed(2);
    document.getElementById("DescontoPermitido").textContent = "R$ " + proposta.vl_desc_restante.toFixed(2);
    document.getElementById("DescontoAplicado").textContent = "R$ " + proposta.vl_desc_aplicado.toFixed(2);
}

function load_values() {

    // Calcula Total da proposta e descontos 
    var _total_descontos = 0;

    for (i in produto) {
        _total_descontos += produto[i].vl_desc;
    }
    proposta.vl_desc_aplicado = parseFloat(_total_descontos);

    var _desconto_permitido = proposta.vl_desc_max - proposta.vl_desc_aplicado;
    proposta.vl_desc_restante = parseFloat(_desconto_permitido);

    // Atualiza Linha Proposta HTML
    document.getElementById("TotalProposta").textContent = "R$ " + proposta.vl_total.toFixed(2);
    document.getElementById("DescontoPermitido").textContent = "R$ " + proposta.vl_desc_restante.toFixed(2);
    document.getElementById("DescontoAplicado").textContent = "R$ " + proposta.vl_desc_aplicado.toFixed(2);
}

// Alerta Visualmente o usuário sobre valores não permitidos
function validacoes() {

    // SE o Desconto Aplicado for MAIOR que o Desconto Permitido
    if (proposta.vl_desc_aplicado > proposta.vl_desc_max) {
        document.getElementById("botao_desconto_aplicado").style.backgroundColor = "#ff4949";
        document.getElementById("botao_desconto_aplicado").children[0].style.color = "#ffffff";
        document.getElementById("botao_desconto_aplicado").children[1].style.color = "#ffffff";
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

function confirmDiscount() {

    var resposta = confirm("Deseja Aplicar o Desconto?");
    if (resposta == true) {
        updateQuoteDetail();
    } else {
        // Fechar
    }
}

function closeWindow() {
    //
    window.open('', '_parent', '');
    window.close();
}

function updateQuoteDetail() {

    //debugger;


    // Produtos da Cotacao
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
            payload: { "findes_valortotaldescontoalcada": produtos_alterados[i].vl_desc }
        };
    }

    parent.Xrm.WebApi.executeMultiple(requests).then(
        function success(result) {
            //alert("Registros Atualizados!");
            updateQuote();
        },
        function (error) {
            console.log(error.message)
            alert("Erro atualizar os produtos da cotação");
        }
    );
}



function updateQuote() {
    parent.Xrm.WebApi.retrieveRecord("quotedetails", produto[0].id, "?$select=quoteid").then(
        function success(result) {
            // Recupera ID proposta
            proposta.id = result._quoteid_value;

            var data =
            {
                "findes_valortotaldescontoalcadajaaplicado": proposta.vl_desc_aplicado,
                "findes_valortotaldescontoalcadapermitido": proposta.vl_desc_max
            }

            parent.Xrm.WebApi.updateRecord("quote", proposta.id, data).then(
                function success(result) {

                    if (produtos_alterados.length < 1) {
                        alert("Nenhum produto foi alterado");
                        closeWindow();
                    } else {
                        applyDiscount();
                    }

                },
                function (error) {
                    console.log(error.message)
                    alert("Erro ao atualizar proposta");
                }
            );

        },
        function (error) {
            console.log(error.message)
            alert("Erro ao obter ID da proposta");
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
            console.log(error.message)
            alert("Erro ao Aplicar Desconto");
            closeWindow();
        }
    );

}

// 
function get_quote_details(quote_id) {

    parent.Xrm.WebApi.retrieveRecord("quotedetails", "?$select=quotedetailid,productid,extendedamount,findes_margemlucratividade,findes_valortotaldescontoalcada&$filter=_quoteid_value eq '" + quote_id + "'").then(
        function success(result) {

            var _produtos = [];
            for (var i = 0; i < result.entities.length; i++) {

                _produto[_produto.length] = {
                    sequencia: parseInt([i]) + 1,
                    id: result.entities[i].quotedetailid,
                    nome: result.entities[i].productid.name,
                    vl_atual: result.entities[i].extendedamount.toFixed(2),
                    vl_margem: result[i].entities.findes_margemlucratividade.toFixed(2),
                    vl_desc: result[i].entities.findes_valortotaldescontoalcada.toFixed(2),
                    vl_final: result[i].entities.extendedamount.toFixed(2)
                }

            }
            return _produtos;
        },
        function (error) {
            console.log(error.message)
            alert("Erro ao obter os produtos da Proposta");
            return null;
        }
    );
}

function get_quote(proposta_id) {

    parent.Xrm.WebApi.retrieveRecord("quotes", proposta_id, "?$select=quoteid,totalamount,findes_valortotaldescontoalcadapermitido,findes_valortotaldescontoalcadajaaplicado").then(
        function success(result) {

            var _proposta = {
                id: result.quoteid,
                vl_total: result.totalamount.toFixed(2),
                Vl_desc_max: result.findes_valortotaldescontoalcadapermitido.toFixed(2),
                vl_desc_restante: result.findes_valortotaldescontoalcadapermitido.toFixed(2) - result.findes_valortotaldescontoalcadajaaplicado.toFixed(2),
                vl_desc_aplicado: result.findes_valortotaldescontoalcadajaaplicado.toFixed(2)
            };
            return _proposta;
        },
        function (error) {
            console.log(error.message)
            alert("Erro ao obter dados da Proposta");
            return null;
        }
    );
}
