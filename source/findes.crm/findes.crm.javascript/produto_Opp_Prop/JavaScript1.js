function Ocultar_campos_produtoopp_produtoprop() {



    setTimeout(function () {
        var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
        var portifolio = 482870002;
        var customizacao = 482870001;
        var novoProduto = 482870000;

        var solucao = Xrm.Page.getAttribute("findes_solucaoid");
        var linhaAtuacao = Xrm.Page.getAttribute("findes_linhaatuacaoid");
        var categoria = Xrm.Page.getAttribute("findes_categoriaid");
        var familiaProduto = Xrm.Page.getAttribute("findes_familiaprodutoid");

        var solucaoControl  = Xrm.Page.getControl("findes_solucaoid");
        var linhaAtuacaoControl = Xrm.Page.getControl("findes_linhaatuacaoid") ; 
        var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
        var familiaProdutoControl = Xrm.Page.getControl("findes_familiaprodutoid");
        var produtoFilialControl = Xrm.Page.getControl("findes_produtofilialid");

        solucaoControl.setVisible(false);
        linhaAtuacaoControl.setVisible(false);
        categoriaControl.setVisible(false);
        familiaProdutoControl.setVisible(false);
        produtoFilialControl.setVisible(false);

        var selecionarProduto = Xrm.Page.getAttribute("isproductoverridden");
        var unidadeExecutora = Xrm.Page.getAttribute("findes_unidadeexecutoraid");
        var unidade = Xrm.Page.getAttribute("uomid");
        var produtoID = Xrm.Page.getAttribute("productid");
        var nomeNovoProduto = Xrm.Page.getControl("productdescription");
        var tabNovoProduto = Xrm.Page.ui.tabs.get("general").sections.get("general_section_5");
        var tabCustomizacao = Xrm.Page.ui.tabs.get("general").sections.get("general_section_6");
        var tabPortifolio = Xrm.Page.ui.tabs.get("general").sections.get("general_section_4");


        if (tipoSolicitacao === portifolio) {
            //Produto Existente
            selecionarProduto.setValue(false);
            //Tab Novo Produto
            tabNovoProduto.setVisible(false);
            //Tab Customizacao
            tabCustomizacao.setVisible(false);
            //Tab Portifolio
            tabPortifolio.setVisible(true);
            //Adiciona Obrigatoriedade Unidade Executora
            unidadeExecutora.setRequiredLevel("required");
            //Adiciona obrigatoriedade Unidade;
            unidade.setRequiredLevel("required");
            //Adiciona Obrigatoriedade em Produto
            produtoID.setRequiredLevel("required");
            //Adiciona obrigatoriedade em categoria
            categoria.setRequiredLevel("required");
            //Adiciona obrigatoriedade em Solução
            solucao.setRequiredLevel("required");
            //Adiciona obrigatoriedade em Linha Atuação
            linhaAtuacao.setRequiredLevel("required");
            //Adiciona obrigatoriedade Família Produto
            familiaProduto.setRequiredLevel("required");

        } else {
            if (tipoSolicitacao === customizacao) {
                debugger;
                //Remove obrigatoriedade Unidade;
                unidade.setRequiredLevel("none");
                //Remove Obrigatoriedade em Produto
                produtoID.setRequiredLevel("required");
                //Produto Existente
                selecionarProduto.setValue(false);
                //Tab Novo Produto
                tabNovoProduto.setVisible(false);
                //Tab Customizacao
                tabCustomizacao.setVisible(true);
                //Remove obrigatoriedade Unidade Executora
                unidadeExecutora.setRequiredLevel("none");
                //Remove obrigatoriedade em categoria
                categoria.setRequiredLevel("none");
                //Remove obrigatoriedade em Solução
                solucao.setRequiredLevel("none");
                //Remove obrigatoriedade em Linha de Atuação
                linhaAtuacao.setRequiredLevel("none");
                //Remove obrigatriedade em Família do Produto
                familiaProduto.setRequiredLevel("none");
                //Tab Portifolio
                tabPortifolio.setVisible(false);


            } else {
                if (tipoSolicitacao === novoProduto) {
                    //Novo Produto
                    selecionarProduto.setValue(true);
                    //Tab Portifolio
                    tabPortifolio.setVisible(false);
                    //Tab Customizacao
                    tabCustomizacao.setVisible(false);
                    //Tab Novo Produto
                    tabNovoProduto.setVisible(true);
                    //Remove obrigatoriedade Unidade Executora
                    unidadeExecutora.setRequiredLevel("none");
                    //Remove obrigatoriedade Unidade;
                    unidade.setRequiredLevel("none");
                    //Remove Obrigatoriedade em Produto
                    produtoID.setRequiredLevel("none");
                    //Adiciona obrigatoriedade em categoria
                    categoria.setRequiredLevel("required");
                    //Remove obrigatoriedade em Solução
                    solucao.setRequiredLevel("none");
                    //Remove obrigatoriedade em Linha de Atuação
                    linhaAtuacao.setRequiredLevel("none");
                    //Remove obrigatriedade em Família do Produto
                    familiaProduto.setRequiredLevel("none");
                    //Torna visivel o nome do novo produto
                    nomeNovoProduto.setVisible(true);

                }
            }
        }
    }, 800);
}



