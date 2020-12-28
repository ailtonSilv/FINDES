function localizarArvoreProduto() {


    setTimeout(function () {





        var prodId = Xrm.Page.getAttribute("productid").getValue()[0].id.toString();


        var validador = Xrm.Page.getAttribute("findes_validador");
        validador.setValue(true);
        var solucaoControl = Xrm.Page.getControl("findes_solucaoid");
        var linha_atuacaoControl = Xrm.Page.getControl("findes_linhaatuacaoid");
        var familia_produtoControl = Xrm.Page.getControl("findes_familiaprodutoid");
        var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
        var produtoFilialControl = Xrm.Page.getControl("findes_produtofilialid");


        //categoria        
        SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + prodId + "')", function (results) {

            var categoriaID = results[0].ParentProductId;


            var array = new Array();
            array[0] = new Object();
            array[0].id = categoriaID.Id;
            array[0].name = categoriaID.Name;
            array[0].entityType = "product";

            var categoria = Xrm.Page.getAttribute("findes_categoriaid");
            categoria.setValue(array);

            //familia do produto
            SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + categoriaID.Id.toString() + "')", function (results) {

                var familiaProdutoID = results[0].ParentProductId;

                var array2 = new Array();
                array2[0] = new Object();
                array2[0].id = familiaProdutoID.Id;
                array2[0].name = familiaProdutoID.Name;
                array2[0].entityType = "product";

                var familiaProduto = Xrm.Page.getAttribute("findes_familiaprodutoid");
                familiaProduto.setValue(array2);



                //Linha de Atuação
                SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + familiaProdutoID.Id.toString() + "')", function (results) {
                    var linhaAtuacaoID = results[0].ParentProductId;

                    var array3 = new Array();
                    array3[0] = new Object();
                    array3[0].id = linhaAtuacaoID.Id;
                    array3[0].name = linhaAtuacaoID.Name;
                    array3[0].entityType = "product";

                    var linhaAtuacao = Xrm.Page.getAttribute("findes_linhaatuacaoid");
                    linhaAtuacao.setValue(array3);

                    //Solução
                    SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + linhaAtuacaoID.Id.toString() + "')", function (results) {
                        var solucaoID = results[0].ParentProductId;

                        var array4 = new Array();
                        array4[0] = new Object();
                        array4[0].id = solucaoID.Id;
                        array4[0].name = solucaoID.Name;
                        array4[0].entityType = "product";

                        var solucao = Xrm.Page.getAttribute("findes_solucaoid");
                        solucao.setValue(array4);


                        //debugger;
                        var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
                        var portifolio = 482870002;
                        var portifolioCustomizado = 482870003;
                        var portifolioPiloto = 482870004;

                        if (tipoSolicitacao === portifolio || tipoSolicitacao === portifolioCustomizado || tipoSolicitacao === portifolioPiloto) {
                            solucaoControl.setVisible(true);
                            linha_atuacaoControl.setVisible(true);
                            familia_produtoControl.setVisible(true);
                            categoriaControl.setVisible(true);
                            produtoFilialControl.setVisible(true);
                        }



                    }, function (error) {
                        Xrm.Utility.alertDialog(error.message);
                    }, function () {
                        //On Complete - Do Something
                    });
                }, function (error) {
                    Xrm.Utility.alertDialog(error.message);
                }, function () {
                    //On Complete - Do Something
                });

            }, function (error) {
                Xrm.Utility.alertDialog(error.message);
            }, function () {
                //On Complete - Do Something
            });

        }, function (error) {
            Xrm.Utility.alertDialog(error.message);
        }, function () {
            //On Complete - Do Something
        });

    }, 350);
} 


function localizaArvoreCategoria() {


    setTimeout(function () {

        debugger;
        var tipoSolicitacao = Xrm.Page.getAttribute("findes_tiposolicitacao").getValue();
        var novoProduto = 482870000;

        if (tipoSolicitacao === novoProduto) {

            var categoriaID = Xrm.Page.getAttribute("findes_categoriaid").getValue()[0].id.toString();

            //if (!categoriaID == null) {
                var validador = Xrm.Page.getAttribute("findes_validador");
                validador.setValue(true);
                var solucaoControl = Xrm.Page.getControl("findes_solucaoid");
                var linha_atuacaoControl = Xrm.Page.getControl("findes_linhaatuacaoid");
                var familia_produtoControl = Xrm.Page.getControl("findes_familiaprodutoid");
                var categoriaControl = Xrm.Page.getControl("findes_categoriaid");
                var produtoFilialControl = Xrm.Page.getControl("findes_produtofilialid");

                SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + categoriaID + "')", function (results) {

                    var familiaProdutoID = results[0].ParentProductId;

                    var array2 = new Array();
                    array2[0] = new Object();
                    array2[0].id = familiaProdutoID.Id;
                    array2[0].name = familiaProdutoID.Name;
                    array2[0].entityType = "product";

                    var familiaProduto = Xrm.Page.getAttribute("findes_familiaprodutoid");
                    familiaProduto.setValue(array2);

                    //Linha de Atuação
                    SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + familiaProdutoID.Id.toString() + "')", function (results) {
                        var linhaAtuacaoID = results[0].ParentProductId;

                        var array3 = new Array();
                        array3[0] = new Object();
                        array3[0].id = linhaAtuacaoID.Id;
                        array3[0].name = linhaAtuacaoID.Name;
                        array3[0].entityType = "product";

                        var linhaAtuacao = Xrm.Page.getAttribute("findes_linhaatuacaoid");
                        linhaAtuacao.setValue(array3);

                        //Solução
                        SDK.REST.retrieveMultipleRecords("Product", "?$select=ParentProductId&$filter=ProductId eq (guid'" + linhaAtuacaoID.Id.toString() + "')", function (results) {
                            var solucaoID = results[0].ParentProductId;

                            var array4 = new Array();
                            array4[0] = new Object();
                            array4[0].id = solucaoID.Id;
                            array4[0].name = solucaoID.Name;
                            array4[0].entityType = "product";

                            var solucao = Xrm.Page.getAttribute("findes_solucaoid");
                            solucao.setValue(array4);

                            //solucaoControl.setVisible(true);
                            //linha_atuacaoControl.setVisible(true);
                            //familia_produtoControl.setVisible(true);
                            //categoriaControl.setVisible(true);
                            //produtoFilialControl.setVisible(true);


                        }, function (error) {
                            Xrm.Utility.alertDialog(error.message);
                        }, function () {
                            //On Complete - Do Something
                        });
                    }, function (error) {
                        Xrm.Utility.alertDialog(error.message);
                    }, function () {
                        //On Complete - Do Something
                    });

                }, function (error) {
                    Xrm.Utility.alertDialog(error.message);
                }, function () {
                    //On Complete - Do Something
                });
            //}

           
        }     
        

    }, 350);
}