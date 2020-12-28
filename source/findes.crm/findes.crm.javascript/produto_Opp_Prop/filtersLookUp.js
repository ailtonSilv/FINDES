function limparSolucao() {

    var solucao = Xrm.Page.data.entity.attributes.get("findes_solucaoid").getValue();

    if (solucao == null) {


        Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
        Xrm.Page.data.entity.attributes.get("uomid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);

        Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
        Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
        Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);

        Xrm.Page.data.entity.attributes.get("findes_linhaatuacaoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_categoriaid").setValue(null);
        Xrm.Page.data.entity.attributes.get("productid").setValue(null);

        setTimeout(function () {
            Xrm.Page.getControl("findes_solucaoid").setFocus();
        }, 100);

    }

}

function limparLinhaAtuacao() {
    var linhaAtuacao = Xrm.Page.data.entity.attributes.get("findes_linhaatuacaoid").getValue();

    if (linhaAtuacao == null) {


        Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
        Xrm.Page.data.entity.attributes.get("uomid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);

        Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
        Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
        Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);

        Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_categoriaid").setValue(null);
        Xrm.Page.data.entity.attributes.get("productid").setValue(null);

        setTimeout(function () {
            Xrm.Page.getControl("findes_linhaatuacaoid").setFocus();
        }, 100);
    }
}

function limparFamiliaProduto() {
    setTimeout(function () {
        var familiaProduto = Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid").getValue();
        if (familiaProduto != null) {
            var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
            //categoriaControl.removePreSearch(function () { addFilterOnCategoryByProductFamily() });
            categoriaControl.removePreSearch(function () { addFilterOnCategoryByEntity });
            setTimeout(function () {
                categoriaControl.addPreSearch(function () { addFilterOnCategoryByProductFamily() });
            }, 100);

        } else {


            Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
            Xrm.Page.data.entity.attributes.get("uomid").setValue(null);
            Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
            Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
            Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);

            Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
            Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
            Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);

            Xrm.Page.data.entity.attributes.get("findes_categoriaid").setValue(null);
            Xrm.Page.data.entity.attributes.get("productid").setValue(null);

            setTimeout(function () {
                Xrm.Page.getControl("findes_familiaprodutoid").setFocus();
            }, 10);
        }

    }, 100);
}

function limparCategoria() {

    var validador = Xrm.Page.data.entity.attributes.get("findes_validador");




    if (validador.getValue() == true) {
        var categoriaId = Xrm.Page.data.entity.attributes.get("findes_categoriaid").getValue();
        if (categoriaId != null) {
            var productIdControl = Xrm.Page.getControl("productid");
            productIdControl.removePreSearch(function () { addFilterOnProductByEntity() });
            //productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
            setTimeout(function () {
                productIdControl.addPreSearch(function () { addFilterOnProductByCategory() });
            }, 100);

        } else {
            var familiaProduto = Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid").getValue();
            if (!familiaProduto == null) {
                var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
                categoriaControl.removePreSearch(function () { addFilterOnCategoryByEntity() });
                //categoriaControl.removePreSearch(function () { addFilterOnCategoryByProductFamily() });
                setTimeout(function () {
                    categoriaControl.addPreSearch(function () { addFilterOnCategoryByProductFamily() });
                }, 100);

            }

        }

    } else {
        var entidade = Xrm.Page.data.entity.attributes.get("findes_coligadaid").getValue();
        if (entidade != null) {
            var productIdControl = Xrm.Page.getControl("productid");
            productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
            setTimeout(function () {
                productIdControl.addPreSearch(function () { addFilterOnProductByEntity() });
            }, 100);

        }
    }

    Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
    Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
    Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);


    Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
    Xrm.Page.data.entity.attributes.get("uomid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);

    var produto = Xrm.Page.data.entity.attributes.get("productid");
    produto.setValue(null);

    setTimeout(function () {
        Xrm.Page.getControl("findes_categoriaid").setFocus();
    }, 100);



}

function limparProduto() {
    var produto = Xrm.Page.data.entity.attributes.get("productid").getValue()
    if (produto == null) {


        Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);

        Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
        Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
        Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);
        Xrm.Page.getControl("findes_produtofilialid").setVisible(false);




        setTimeout(function () {
            Xrm.Page.getControl("productid").setFocus();
        }, 50);

    }
}

function setFilter_Entity() {
    var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
    var portifolio = 482870002;
    var customizacao = 482870001;
    var novoProduto = 482870000;
    var portifolioCustom = 482870003;
    var portifolioPiloto = 482870004;

    if (tipoSolicitacao === portifolio || tipoSolicitacao === portifolioCustom || tipoSolicitacao === portifolioPiloto) {
        var entidade = Xrm.Page.data.entity.attributes.get("findes_coligadaid").getValue();
        if (entidade != null) {
            var productIdControl = Xrm.Page.getControl("productid");
            productIdControl.removePreSearch(function () { addFilterOnProductByEntity() });
            productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
            setTimeout(function () {
                productIdControl.addPreSearch(function () { addFilterOnProductByEntity() });
            }, 100);

        } else {
            var productIdControl = Xrm.Page.getControl("productid");
            productIdControl.removePreSearch(function () { addFilterOnProductByEntity() });
            productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
            Xrm.Page.data.entity.attributes.get("productid").setValue(null);
            defaultFrm();
        }
    }

    if (tipoSolicitacao === novoProduto) {
        var entidade = Xrm.Page.data.entity.attributes.get("findes_coligadaid").getValue();
        if (entidade != null) {
            var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
            categoriaControl.removePreSearch(function () { addFilterOnCategoryByProductFamily() });
            //categoriaControl.removePreSearch(function () { addFilterOnCategoryByEntity() });
            setTimeout(function () {
                categoriaControl.addPreSearch(function () { addFilterOnCategoryByEntity() });
            }, 100);

        } else {
            var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
            categoriaControl.removePreSearch(function () { addFilterOnCategoryByProductFamily() });
            categoriaControl.removePreSearch(function () { addFilterOnCategoryByEntity() });
            defaultFrm();

        }

    }

    if (tipoSolicitacao === customizacao) {
        var entidade = Xrm.Page.data.entity.attributes.get("findes_coligadaid").getValue();
        if (entidade != null) {
            var productIdControl = Xrm.Page.getControl("productid");
            productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
            //productIdControl.removePreSearch(function () { addFilterOnProductByEntity });
            setTimeout(function () {
                productIdControl.addPreSearch(function () { addFilterOnProductByEntity() });
            }, 100);

        } else {
            var productIdControl = Xrm.Page.getControl("productid");
            productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
            productIdControl.removePreSearch(function () { addFilterOnProductByEntity() });
            Xrm.Page.data.entity.attributes.get("productid").setValue(null);
            defaultFrm();

        }
    }

}


function clearAllFilter() {

    debugger;
    Xrm.Page.data.entity.removeOnSave(clear());

}
function clear() {
    debugger;
    var productIdControl = Xrm.Page.getControl("productid");
    var categoriaControl = Xrm.Page.getControl("findes_categoriaid");

    productIdControl.removePreSearch(function () { addFilterOnProductByCategory() });
    productIdControl.removePreSearch(function () { addFilterOnProductByEntity() });

    categoriaControl.removePreSearch(function () { addFilterOnCategoryByProductFamily() });
    categoriaControl.removePreSearch(function () { addFilterOnCategoryByEntity() });

    Xrm.Page.getControl("findes_validador").setVisible(true);
    Xrm.Page.data.entity.attributes.get("findes_validador").setValue(false);
    Xrm.Page.getControl("findes_validador").setVisible(false);
}


function defaultFrm() {
    //Limpa e esconde a Solução
    limparSolucao();
    Xrm.Page.getControl("findes_categoriaid").setVisible(false);
    Xrm.Page.getControl("findes_familiaprodutoid").setVisible(false);
    Xrm.Page.getControl("findes_linhaatuacaoid").setVisible(false);
    Xrm.Page.getControl("findes_solucaoid").setVisible(false);


    //Tipo da solicitação
    Xrm.Page.data.entity.attributes.get("findes_tiposolicitacao").setValue(482870002);

    //Limpa os valores de Unidades
    Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
    Xrm.Page.data.entity.attributes.get("uomid").setValue(null);

    //Default de Definição de Preços
    Xrm.Page.data.entity.attributes.get("quantity").setValue(null);
    Xrm.Page.data.entity.attributes.get("ispriceoverridden").setValue(false);

    //Controle Unidade
    Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
    Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
    Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);
    Xrm.Page.getControl("findes_produtofilialid").setVisible(false);

    //Validaro Default
    Xrm.Page.getControl("findes_validador").setVisible(true);
    Xrm.Page.data.entity.attributes.get("findes_validador").setValue(false);
    Xrm.Page.getControl("findes_validador").setVisible(false);

    var sectionSENAI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosenai");
    var sectionSESI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosesi");
    sectionSENAI.setVisible(false);
    sectionSESI.setVisible(false);
    clearAllFilter();

    Oculta_produtooportunidade();
}



function addFilterOnProductByCategory() {
    var productIdControl = Xrm.Page.getControl("productid");
    var aux = Xrm.Page.getAttribute("findes_categoriaid").getValue()[0];
    var id = aux.id.toString();
    var fetch = "<filter type='and'>" +
        "<condition attribute='producttypecode' operator='eq' value='11' />" +
        "<condition attribute='parentproductid' operator='eq' uitype='product' value='" + id + "' />" +
        "</filter>";

    productIdControl.addCustomFilter(fetch);
}
function addFilterOnProductByEntity() {

    var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
    var portifolio = 482870002;
    var portifolioCustom = 482870003;
    var portifolioPiloto = 482870004;
    var novaCustomizacao = 482870001;
    var p;

    if (tipoSolicitacao === portifolio) {
        p = 482870000; //Portfólio
    }
    if (tipoSolicitacao === portifolioPiloto) {
        p = 482870002;//Portfólio Temporário
    }
    if (tipoSolicitacao === portifolioCustom) {
        p = 482870001; //Customizado
    }


    var productIdControl = Xrm.Page.getControl("productid");
    var aux = Xrm.Page.getAttribute("findes_coligadaid").getValue()[0];
    var id = aux.id;
    var fetch;
    if (tipoSolicitacao === novaCustomizacao) {
        fetch = '<filter type="and">' +
            '<condition attribute="findes_coligadaid" operator="eq" uitype="findes_coligada" value="' + id.toString() + '" />' +
            '<condition attribute="producttypecode" operator="eq" value="11" />' +
            '</filter>';
    } else {
        fetch = '<filter type="and">' +
            '<condition attribute="findes_coligadaid" operator="eq" uitype="findes_coligada" value="' + id.toString() + '" />' +
            '<condition attribute="producttypecode" operator="eq" value="11" />' +
            '<condition attribute="findes_tipoportfolio" value="' + p.toString() + '" operator="eq"/>' +
            '</filter>';
    }


    productIdControl.addCustomFilter(fetch);
}

function addFilterOnCategoryByProductFamily() {
    var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
    var id = Xrm.Page.getAttribute("findes_familiaprodutoid").getValue()[0].id.toString();

    var fetch = "<filter type='and'>" +
        "<condition attribute='producttypecode' operator='eq' value='9' />" +
        "<condition attribute='parentproductid' operator='eq' uitype='product' value='" + id + "' />" +
        "</filter>";
    categoriaControl.addCustomFilter(fetch);
}

function addFilterOnCategoryByEntity() {

    var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
    var aux = Xrm.Page.getAttribute("findes_coligadaid").getValue()[0];
    var name = aux.name.toString();
    var id = aux.id.toString();

    var fetch = '<filter type="and">' +
        '<condition attribute="findes_coligadaid" operator="eq" uitype="findes_coligada" value="' + id + '"  />' +
        '<condition attribute="producttypecode" operator="eq" value="9" />' +
        '</filter>';
    categoriaControl.addCustomFilter(fetch);
}


function Oculta_produtooportunidade() {

    Xrm.Page.data.entity.attributes.get("findes_linhaatuacaoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_solucaoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_categoriaid").setValue(null);
    Xrm.Page.data.entity.attributes.get("productid").setValue(null);
    Xrm.Page.getControl("findes_categoriaid").setVisible(false);
    Xrm.Page.getControl("findes_familiaprodutoid").setVisible(false);
    Xrm.Page.getControl("findes_linhaatuacaoid").setVisible(false);
    Xrm.Page.getControl("findes_solucaoid").setVisible(false);


    //Limpa os valores de Unidades
    Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);
    Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
    Xrm.Page.data.entity.attributes.get("uomid").setValue(null);

    //Default de Definição de Preços
    Xrm.Page.data.entity.attributes.get("quantity").setValue(null);
    Xrm.Page.data.entity.attributes.get("ispriceoverridden").setValue(false);


    Xrm.Page.getControl("findes_unidadeexecutoraid").setVisible(false);
    Xrm.Page.getControl("findes_unidadefaturamentoid").setVisible(false);
    Xrm.Page.getControl("findes_localexecucaoid").setVisible(false);
    Xrm.Page.getControl("findes_produtofilialid").setVisible(false);




    var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
    var portifolio = 482870002;
    var customizacao = 482870001;
    var novoProduto = 482870000;
    var portifolioCustom = 482870003;
    var portifolioPiloto = 482870004;
    //Tab
    var grid = Xrm.Page.ui.tabs.get("general").sections.get("general_section_4");

    //var sectionSENAI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosenai");
    //var sectionSESI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosesi");
    //sectionSENAI.setVisible(false);
    //sectionSESI.setVisible(false);

    //Campos Portfólio Controle
    var solucaoControl = Xrm.Page.getControl("findes_solucaoid");
    var linhaAtuacaoControl = Xrm.Page.getControl("findes_linhaatuacaoid");
    var categoriaPortifolioControl = Xrm.Page.getControl("findes_categoriaid");
    var familiaProdutoControl = Xrm.Page.getControl("findes_familiaprodutoid");
    var produtoFilialControl = Xrm.Page.getControl("findes_produtofilialid");
    var produtoPortfolioControl = Xrm.Page.getControl("productid");
    var unidadeControl = Xrm.Page.getControl("uomid");
    var unidadeExecutoraControl = Xrm.Page.getControl("findes_unidadeexecutoraid");
    var unidadeFaturamentoControl = Xrm.Page.getControl("findes_unidadefaturamentoid");
    var localExecucaoControl = Xrm.Page.getControl("findes_localexecucaoid");



    //Campos Novo Produto
    var novoProdutoControl = Xrm.Page.getControl("productdescription");
    var categoriaNovoProdutoContol = Xrm.Page.getControl("findes_categoriaid");

    //Campos Customização
    var produtoCustomizacaoControl = Xrm.Page.getControl("productid");

    if (tipoSolicitacao === portifolio || tipoSolicitacao === portifolioCustom || tipoSolicitacao === portifolioPiloto) {

        //Exibe básico do portfólio
        var solucaoValue = Xrm.Page.data.entity.attributes.get("findes_solucaoid");
        var linhaAtuacaoValue = Xrm.Page.data.entity.attributes.get("findes_linhaatuacaoid");
        var familiaProdutoValue = Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid");
        var categoriaValue = Xrm.Page.data.entity.attributes.get("findes_categoriaid");
        var produtoValue = Xrm.Page.data.entity.attributes.get("productid");
        var produtoFilialValue = Xrm.Page.data.entity.attributes.get("findes_produtofilialid");

        if (solucaoValue.getValue() != null) {
            solucaoControl.setVisible(true);
        } else {
            solucaoControl.setVisible(false);
        }

        if (linhaAtuacaoValue.getValue() != null) {
            linhaAtuacaoControl.setVisible(true);
        } else {
            linhaAtuacaoControl.setVisible(false);
        }

        if (familiaProdutoValue.getValue() != null) {
            familiaProdutoControl.setVisible(true);
        } else {
            familiaProdutoControl.setVisible(false);
        }

        if (categoriaValue.getValue() != null) {
            categoriaPortifolioControl.setVisible(true);
        } else {
            categoriaPortifolioControl.setVisible(false);
        }

        if (produtoValue.getValue() != null) {
            produtoPortfolioControl.setVisible(true);
        } else {
            produtoPortfolioControl.setVisible(false);
        }

        if (produtoFilialValue.getValue() != null) {
            produtoFilialControl.setVisible(true);
        } else {
            produtoFilialControl.setVisible(false);
        }

        produtoPortfolioControl.setVisible(true);
        unidadeControl.setVisible(true);



        //Esconde Novo Produto
        novoProdutoControl.setVisible(false);

        //Esconde Customização


    }

    if (tipoSolicitacao === novoProduto) {
        //Exibe campos novo produto
        novoProdutoControl.setVisible(true);
        categoriaPortifolioControl.setVisible(true);
        categoriaNovoProdutoContol.setVisible(true);

        //Esconte Portfólio exceto categoria
        solucaoControl.setVisible(false);
        linhaAtuacaoControl.setVisible(false);
        familiaProdutoControl.setVisible(false);
        produtoPortfolioControl.setVisible(false);
        produtoFilialControl.setVisible(false);
        unidadeControl.setVisible(false);
        unidadeExecutoraControl.setVisible(false);
        unidadeFaturamentoControl.setVisible(false);
        localExecucaoControl.setVisible(false);

        //Esconde Customização
        produtoCustomizacaoControl.setVisible(false);
    }


    if (tipoSolicitacao === customizacao) {
        //Exibe customização
        produtoCustomizacaoControl.setVisible(true);

        //Esconte Portfólio
        solucaoControl.setVisible(false);
        linhaAtuacaoControl.setVisible(false);
        familiaProdutoControl.setVisible(false);
        categoriaPortifolioControl.setVisible(false);
        produtoFilialControl.setVisible(false);
        unidadeControl.setVisible(false);
        unidadeExecutoraControl.setVisible(false);
        unidadeFaturamentoControl.setVisible(false);
        localExecucaoControl.setVisible(false);

        //Esconde Novo Produto
        novoProdutoControl.setVisible(false);
        categoriaNovoProdutoContol.setVisible(false);
    }

    var solucao = Xrm.Page.getAttribute("findes_solucaoid");
    var linhaAtuacao = Xrm.Page.getAttribute("findes_linhaatuacaoid");
    var categoria = Xrm.Page.getAttribute("findes_categoriaid");
    var familiaProduto = Xrm.Page.getAttribute("findes_familiaprodutoid");
    var unidade = Xrm.Page.getAttribute("uomid");
    var unidadeExecutora = Xrm.Page.getAttribute("findes_unidadeexecutoraid");
    var selecionarProduto = Xrm.Page.getAttribute("isproductoverridden");
    var produto = Xrm.Page.getAttribute("productid");



    switch (tipoSolicitacao) {
        case portifolio:
            produtoPortfolioControl.setLabel("Produto");
            grid.setLabel("Portfólio");
            categoriaPortifolioControl.setLabel("4 - Categoria");

            unidade.setRequiredLevel("required");
            produto.setRequiredLevel("required");
            categoria.setRequiredLevel("required");
            solucao.setRequiredLevel("required");
            linhaAtuacao.setRequiredLevel("required");
            familiaProduto.setRequiredLevel("required");
            unidadeExecutora.setRequiredLevel("required");

            selecionarProduto.setValue(false);

            break;


        case customizacao:
            produtoCustomizacaoControl.setLabel("Produto Customizado");
            grid.setLabel("Customização");

            unidade.setRequiredLevel("none");
            produto.setRequiredLevel("required");
            categoria.setRequiredLevel("none");
            solucao.setRequiredLevel("none");
            linhaAtuacao.setRequiredLevel("none");
            familiaProduto.setRequiredLevel("none");
            unidadeExecutora.setRequiredLevel("none");

            selecionarProduto.setValue(false);

            break;


        case novoProduto:
            categoriaPortifolioControl.setLabel("Categoria");
            grid.setLabel("Novo Produto");

            unidade.setRequiredLevel("none");
            produto.setRequiredLevel("none");
            categoria.setRequiredLevel("required");
            solucao.setRequiredLevel("none");
            linhaAtuacao.setRequiredLevel("none");
            familiaProduto.setRequiredLevel("none");
            unidadeExecutora.setRequiredLevel("none");

            selecionarProduto.setValue(true);

            break;
    }



}


function clearFilter() {
    var productControl = Xrm.Page.getControl("productid");
    var categoryControl = Xrm.Page.getControl("findes_categoriaid");
    productControl.removePreSearch(function () { addFilterOnProductByCategory() });
    productControl.removePreSearch(function () { addFilterOnProductByEntity });
    categoryControl.removePreSearch(function () { addFilterOnCategoryByEntity() });
    categoryControl.removePreSearch(function () { addFilterOnCategoryByProductFamily });

    Xrm.Page.getControl("findes_validador").setVisible(true);
    Xrm.Page.getAttribute("findes_validador").setValue(false);
    Xrm.Page.getControl("findes_validador").setVisible(false);
}