function Ocultar_campos_produtoopp_produtoprop() {


    setTimeout(function () {

        var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
        var portfolioCustomizado = 482870003;
        var portifolio = 482870002;
        var customizacao = 482870001;
        var novoProduto = 482870000;
        var portifolioPiloto = 482870004;



        //Tab
        var grid = Xrm.Page.ui.tabs.get("general").sections.get("general_section_4");

        var sectionSENAI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosenai");
        var sectionSESI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosesi");
        sectionSENAI.setVisible(false);
        sectionSESI.setVisible(false);

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


        if (tipoSolicitacao === portifolio || tipoSolicitacao == null) {

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

        if (tipoSolicitacao === portfolioCustomizado) {
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



            produtoPortfolioControl.setVisible(true);
            unidadeControl.setVisible(true);



            novoProdutoControl.setVisible(false);
 

        }

        if (tipoSolicitacao === portifolioPiloto) {
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

            produtoPortfolioControl.setVisible(true);
            unidadeControl.setVisible(true);

            novoProdutoControl.setVisible(false);


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


            case portfolioCustomizado:
                produtoPortfolioControl.setLabel("Produto");
                grid.setLabel("Portfólio Customizado");
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
        }

        clearFilter();
    }, 200);
}


function limparFrm() {

    setTimeout(function () {

        //Default de Definição de Preços
        Xrm.Page.data.entity.attributes.get("quantity").setValue(null);
        Xrm.Page.data.entity.attributes.get("ispriceoverridden").setValue(false);


        //Limpa os valores de Unidades
        Xrm.Page.data.entity.attributes.get("findes_unidadeexecutoraid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_unidadefaturamentoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_localexecucaoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_produtofilialid").setValue(null);
        Xrm.Page.data.entity.attributes.get("uomid").setValue(null);

        //Limpa e esconde a Solução
        Xrm.Page.data.entity.attributes.get("findes_linhaatuacaoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_solucaoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_familiaprodutoid").setValue(null);
        Xrm.Page.data.entity.attributes.get("findes_categoriaid").setValue(null);
        Xrm.Page.data.entity.attributes.get("productid").setValue(null);

        //Limpeza da Coligada(Entidade)
        Xrm.Page.data.entity.attributes.get("findes_coligadaid").setValue(null);

        var sectionSENAI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosenai");
        var sectionSESI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosesi");
        sectionSENAI.setVisible(false);
        sectionSESI.setVisible(false);

    }, 20);


}

function clearFilter() {
    var productControl = Xrm.Page.getControl("productid");
    var categoryControl = Xrm.Page.getControl("findes_categoriaid");
    productControl.removePreSearch(function () { addFilterOnProductByCategory() });
    productControl.removePreSearch(function () { addFilterOnProductByEntity() });
    categoryControl.removePreSearch(function () { addFilterOnCategoryByEntity() });
    categoryControl.removePreSearch(function () { addFilterOnCategoryByProductFamily });
}

function addFilterOnProductByEntityCustom() {
    var productIdControl = Xrm.Page.getControl("productid");
    var aux = Xrm.Page.getAttribute("findes_coligadaid").getValue()[0];
    var id = aux.id;
    var fetch = '<filter type="and">' +
        //'< filter type = "and" >' +
        '<condition attribute="findes_coligadaid" operator="eq" uitype="findes_coligada" value="' + id.toString() + '" />' +
        '<condition attribute="producttypecode" operator="eq" value="11" />' +
        '<condition attribute="findes_tipoportfolio" operator="eq" value="482870001" />' +
        //'<condition attribute="pricelevelid" operator="not-null" />' +
        //'</filter >' +
        '</filter >';

    productIdControl.addCustomFilter(fetch);
}

function addFilterOnProductByCategory() {
    var productIdControl = Xrm.Page.getControl("productid");
    var aux = Xrm.Page.getAttribute("findes_categoriaid").getValue()[0];
    var id = aux.id.toString();
    var fetch = "<filter type='and'>" +
        "<condition attribute='producttypecode' operator='eq' value='11' />" +
        "<condition attribute='parentproductid' operator='eq' uitype='product' value='" + id + "' />" +
        //"<condition attribute='findes_tipoportfolio' operator='eq' value='482870000' />" + //Portifolio
        "</filter>";

    productIdControl.addCustomFilter(fetch);
}

function addFilterOnProductByEntity() {

    var productIdControl = Xrm.Page.getControl("productid");
    var aux = Xrm.Page.getAttribute("findes_coligadaid").getValue()[0];
    var id = aux.id;

    var fetch = '<filter type="and">' +
        //'< filter type="and">' +
        '<condition attribute="findes_coligadaid" operator="eq" uitype="findes_coligada" value="' + id.toString() + '" />' +
        '<condition attribute="producttypecode" operator="eq" value="11" />' +
        '<condition attribute="findes_tipoportfolio" operator="eq" value="482870000" />' +
        '<condition attribute="pricelevelid" operator="not-null" />' +
        //'</filter >' +
        '</filter >';

    productIdControl.addCustomFilter(fetch);
}

function addFilterOnProductByEntityPortfolioPiloto() {
    var productIdControl = Xrm.Page.getControl("productid");
    var aux = Xrm.Page.getAttribute("findes_coligadaid").getValue()[0];
    var id = aux.id;

    var fetch = '<filter type="and">' +
        //'< filter type="and">' +
        '<condition attribute="findes_coligadaid" operator="eq" uitype="findes_coligada" value="' + id.toString() + '" />' +
        '<condition attribute="findes_tipoportfolio" operator="eq" value="482870002" />' +
        //'<condition attribute="pricelevelid" operator="not-null" />' +
        //'</filter >' +
        '</filter >';

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




function setVisibleSectionSesiSenai(context) {
    debugger;
    var sectionSENAI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosenai");
    var sectionSESI = Xrm.Page.ui.tabs.get("general").sections.get("findes_sectionsoliticadaoprodutosesi");

    var tipoSolicitacao = context.getFormContext().getAttribute("findes_tiposolicitacao").getValue();

    var coligada = context.getFormContext().getAttribute("findes_coligadaid").getValue();
    var customizacao = 482870001;
    var novoProduto = 482870000;

    if (tipoSolicitacao === customizacao || tipoSolicitacao === novoProduto) {
        if (coligada != null) {
            if (coligada[0].id.toString() === "{1593E58F-7729-E911-A95B-000D3AC1B1AB}") { //Senai
                sectionSENAI.setVisible(true);
                sectionSESI.setVisible(false);
            } else {
                if (coligada[0].id.toString() === "{1193E58F-7729-E911-A95B-000D3AC1B1AB}") { //Sesi
                    sectionSESI.setVisible(true);
                    sectionSENAI.setVisible(false);
                }
            }

        } else {
            sectionSENAI.setVisible(false);
            sectionSESI.setVisible(false);
        }
    }



}