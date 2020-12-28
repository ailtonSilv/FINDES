function visibilidadeCamposPortfolio(){
    debugger;
    //setTimeout(function () {

    try {
        var prodId = Xrm.Page.getAttribute("productid").getValue()[0].id.toString();
        var solucao = Xrm.Page.getAttribute("findes_solucaoid").getValue()[0].id.toString();
        //var solucao = Xrm.Page.getAttribute("findes_solucaoid").value();
        var solucaoControl = Xrm.Page.getControl("findes_solucaoid");
        var linha_atuacao = Xrm.Page.getAttribute("findes_linhaatuacaoid").getValue()[0].id.toString();
        var linha_atuacaoControl = Xrm.Page.getControl("findes_linhaatuacaoid");
        var familia_produto = Xrm.Page.getAttribute("findes_familiaprodutoid").getValue()[0].id.toString();
        var familia_produtoControl = Xrm.Page.getControl("findes_familiaprodutoid");
        var categoria = Xrm.Page.getAttribute("findes_categoriaid").getValue()[0].id.toString();
        var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
        var produtoFilial = Xrm.Page.getAttribute("findes_produtofilialid").getValue()[0].id.toString();
        var produtoFilialControl = Xrm.Page.getControl("findes_produtofilialid");


          

    } catch (error) {

        

            Xrm.Page.getControl("findes_validador").setVisible(true);
            var validador = Xrm.Page.getAttribute("findes_validador").getValue();
            Xrm.Page.getControl("findes_validador").setVisible(false);


            if(validador===true){


                var prodId = Xrm.Page.getAttribute("productid");
                var solucao = Xrm.Page.getAttribute("findes_solucaoid");
                var linha_atuacao = Xrm.Page.getAttribute("findes_linhaatuacaoid");
                var familia_produto = Xrm.Page.getAttribute("findes_familiaprodutoid");
                var categoria = Xrm.Page.getAttribute("findes_categoriaid");
                var produtoFilial = Xrm.Page.getAttribute("findes_produtofilialid");
                var unidade_executora = Xrm.Page.getAttribute("findes_unidadeexecutoraid");
                var unidade_faturamento = Xrm.Page.getAttribute("findes_unidadefaturamentoid");
                var local_execucao = Xrm.Page.getAttribute("findes_localexecucaoid");

                var solucaoControl = Xrm.Page.getControl("findes_solucaoid");
                var linha_atuacaoControl = Xrm.Page.getControl("findes_linhaatuacaoid");
                var familia_produtoControl = Xrm.Page.getControl("findes_familiaprodutoid");
                var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
                var produtoFilialControl = Xrm.Page.getControl("findes_produtofilialid");
                var unidade_executoraControl = Xrm.Page.getControl("findes_unidadeexecutoraid");
                var unidade_faturamentoControl = Xrm.Page.getControl("findes_unidadefaturamentoid");
                var local_execucaoControl = Xrm.Page.getControl("findes_localexecucaoid");


                solucao.setValue(null);
                linha_atuacao.setValue(null);
                familia_produto.setValue(null);
                categoria.setValue(null);
                produtoFilial.setValue(null);
                prodId.setValue(null);
                unidade_executora.setValue(null);
                unidade_faturamento.setValue(null);
                local_execucao.setValue(null);

                solucaoControl.clearNotification();
                linha_atuacaoControl.clearNotification();
                familia_produtoControl.clearNotification();
                categoriaControl.clearNotification();
                produtoFilialControl.clearNotification();

                unidade_executoraControl.setVisible(false);
                unidade_faturamentoControl.setVisible(false);
                local_execucaoControl.setVisible(false);


                Xrm.Page.getControl("findes_validador").setVisible(true);
                Xrm.Page.getAttribute("findes_validador").setValue(false);
                Xrm.Page.getControl("findes_validador").setVisible(false);


                //produtoFilialControl.setVisible(false);
                //solucaoControl.setVisible(false);
                //linha_atuacaoControl.setVisible(false);
                //familia_produtoControl.setVisible(false);
                //categoriaControl.setVisible(false);
            }

        


    }

        
    //},600);
}