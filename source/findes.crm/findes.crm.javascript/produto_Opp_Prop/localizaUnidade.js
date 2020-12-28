    function replicar_campos(){


        setTimeout(function(){
                var produtoFilialID = Xrm.Page.getAttribute("findes_produtofilialid").getValue();
                var unidadeExecutora = Xrm.Page.getAttribute("findes_unidadeexecutoraid");
                var unidadeFaturamento = Xrm.Page.getAttribute("findes_unidadefaturamentoid");
                var localExecucao = Xrm.Page.getAttribute("findes_localexecucaoid");

                SDK.REST.retrieveMultipleRecords("findes_produtofilial", "?$select=findes_unidadeatendimentoid&$filter=findes_produtofilialId eq (guid'"+ produtoFilialID[0].id +"')", function(results) {


                    var findes_unidadeatendimentoid = results[0].findes_unidadeatendimentoid;


                    var unidadeAtendimento = new Array();
                    unidadeAtendimento[0] = new Object();
                    unidadeAtendimento[0].id = findes_unidadeatendimentoid.Id;
                    unidadeAtendimento[0].name = findes_unidadeatendimentoid.Name;
                    unidadeAtendimento[0].entityType = "findes_unidadedeatendimento";
                    

                    unidadeExecutora.setValue(unidadeAtendimento);
                    unidadeFaturamento.setValue(unidadeAtendimento);
                    localExecucao.setValue(unidadeAtendimento);
            
                }, function(error) {
                    Xrm.Utility.alertDialog(error.message);
                }, function() {
                    //On Complete - Do Something
                });
        },350);
    }



      