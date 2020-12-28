function Ocultar_campos_produtoopp_produtoprop() {

    var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
    var portifolio = 482870002;
    var customizacao = 482870001;
    var novoProduto = 482870000;
    var selecionarProduto = Xrm.Page.getAttribute("isproductoverridden");
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
    }

    if (tipoSolicitacao === customizacao) {
        //Produto Existente
        selecionarProduto.setValue(false);
        //Tab Portifolio
        tabPortifolio.setVisible(false);
        //Tab Novo Produto
        tabNovoProduto.setVisible(false);
        //Tab Customizacao
        tabCustomizacao.setVisible(true);
        
    }

    if (tipoSolicitacao === novoProduto) {
        //Novo Produto
        selecionarProduto.setValue(true);
        //Tab Portifolio
        tabPortifolio.setVisible(false);
        //Tab Customizacao
        tabCustomizacao.setVisible(false);
        //Tab Novo Produto
        tabNovoProduto.setVisible(true);
    }
}