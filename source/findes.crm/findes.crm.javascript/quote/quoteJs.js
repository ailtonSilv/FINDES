var Findes = window.Findes || { _namespace: true };

var windowOptions = { openInNewWindow: true, width: 1050 };

Findes.Quote = {
    AproveDiscount: function (executionContext) {
        debugger;
        var formContext = executionContext.getFormContext();
        var entityId = formContext.data.entity.getId();
        var userReferenceId = Xrm.Utility.getGlobalContext().userSettings.userId;
        userReferenceId = userReferenceId.substring(1, userReferenceId.length - 1);

        var today = new Date();
        today.setHours(today.getHours() + 3);
        var m = today.getMonth().toString();
        if (m.length === 1) { m = "0" + m; }
        var d = today.getDate().toString();
        if (d.length === 1) { d = "0" + d; }
        var h = today.getHours().toString();
        if (h.length === 1) { h = "0" + h; }
        var min = today.getMinutes().toString();
        if (min.length === 1) { min = "0" + min; }
        var s = today.getSeconds().toString();
        if (s.length === 1) { s = "0" + s; }
        var dateUTC = today.getFullYear() + "-" + m + "-" + d + "T" + h + ":" + min + ":" + s + "Z";

        var data =
        {
            "findes_aprovadorid_findes_solicitacaodedesconto@odata.bind": "/systemusers(" + userReferenceId + ")",
            "statecode": 1,
            "statuscode": 2,
            "scheduledend": dateUTC
        };

        Xrm.Utility.showProgressIndicator("Aprovando o desconto solicitado...");

        Xrm.WebApi.updateRecord("findes_solicitacaodedescontos", entityId.substring(1, entityId.length - 1), data).then(
            function success(result) {
                console.log("Solicitação Aprovada com sucesso.");
                Xrm.Utility.closeProgressIndicator();
                Xrm.Page.data.refresh(true);
            },
            function (error) {
                console.log(error.message);
                Xrm.Utility.closeProgressIndicator();
                formContext.ui.setFormNotification(error.message, 'ERROR', 'discountError');
            }
        );
    },

    ReproveDiscount: function (executionContext) {
        debugger;
        var formContext = executionContext.getFormContext();
        var entityId = formContext.data.entity.getId();
        var userReferenceId = Xrm.Utility.getGlobalContext().userSettings.userId;
        userReferenceId = userReferenceId.substring(1, userReferenceId.length - 1);

        var today = new Date();
        today.setHours(today.getHours() + 3);
        var m = today.getMonth().toString();
        if (m.length === 1) { m = "0" + m; }
        var d = today.getDate().toString();
        if (d.length === 1) { d = "0" + d; }
        var h = today.getHours().toString();
        if (h.length === 1) { h = "0" + h; }
        var min = today.getMinutes().toString();
        if (min.length === 1) { min = "0" + min; }
        var s = today.getSeconds().toString();
        if (s.length === 1) { s = "0" + s; }
        var dateUTC = today.getFullYear() + "-" + m + "-" + d + "T" + h + ":" + min + ":" + s + "Z";

        var data =
        {
            "findes_aprovadorid_findes_solicitacaodedesconto@odata.bind": "/systemusers(" + userReferenceId + ")",
            "statecode": 1,
            "statuscode": 482870001,
            "scheduledend": dateUTC
        }

        Xrm.Utility.showProgressIndicator("Reprovando o desconto solicitado...");

        Xrm.WebApi.updateRecord("findes_solicitacaodedescontos", entityId.substring(1, entityId.length - 1), data).then(
            function success(result) {
                console.log("Solicitação Reprovada com sucesso.");
                Xrm.Utility.closeProgressIndicator();
                Xrm.Page.data.refresh(true);

            },
            function (error) {
                console.log(error.message);
                Xrm.Utility.closeProgressIndicator();
                formContext.ui.setFormNotification(error.message, 'ERROR', 'discountError');
            }
        );
    },

    EditDiscountPage: function (executionContext) {
        debugger;
        var formContext = executionContext.getFormContext();
        var quoteId = formContext.getAttribute("regardingobjectid").getValue()[0].id;
        var solicitacaoId = formContext.data.entity.getId();

        Xrm.Utility.showProgressIndicator("Carregando dados para a página de descontos...");
        Xrm.WebApi.online.execute({
            actionType: "edit&solicitacaoid=" + solicitacaoId.substring(1, solicitacaoId.length - 1),
            getMetadata: function () {
                return {
                    boundParameter: "entity",
                    parameterTypes: {
                        "actionType": {
                            "typeName": "Edm.String",
                            "structuralProperty": 1
                        },
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
                    operationName: "findes_ActionGeraUrlTelaDesconto"
                };
            },
            userReference: {
                id: Xrm.Utility.getGlobalContext().userSettings.userId,
                entityType: "systemuser"
            },
            entity: {
                id: quoteId.substring(1, quoteId.length - 1),
                entityType: "quote"
            }
        }).then(
            function (result) {
                debugger;
                Xrm.Utility.closeProgressIndicator();
                if (result.ok) {
                    Xrm.Navigation.openWebResource("findes_html_quote_discount_page", windowOptions, encodeURIComponent(JSON.parse(result.responseText).discountUrl));
                }
                else
                    Xrm.Navigation.openErrorDialog({ message: "Houve um erro ao tentar abrir a página com as informações sobre o desconto de alçada. Para mais detalhes, baixe o log ou entre em contato com o administrador.", details: JSON.stringify(result) });
            },
            function (error) {
                debugger;
                Xrm.Utility.closeProgressIndicator();
                if (error.message === 'Não foi possivel encontrar o desconto vinculado ao time do usuário do contexto.') {
                    executionContext.getFormContext().ui.setFormNotification(error.message, 'ERROR', 'discountError');
                }
                else
                    Xrm.Navigation.openErrorDialog({ message: "Houve um erro ao tentar abrir a página com as informações sobre o desconto de alçada. Para mais detalhes, baixe o log ou entre em contato com o administrador.", details: JSON.stringify(error) });
            }
        );
    },
    LoadDiscountPage: function (executionContext) {
        debugger;
        entityId = executionContext.getFormContext().data.entity.getId();
        Xrm.Utility.showProgressIndicator("Carregando dados para a página de descontos...");
        Xrm.WebApi.online.execute({
            actionType: "add",
            getMetadata: function () {
                return {
                    boundParameter: "entity",
                    parameterTypes: {
                        "actionType": {
                            "typeName": "Edm.String",
                            "structuralProperty": 1
                        },
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
                    operationName: "findes_ActionGeraUrlTelaDesconto"
                };
            },
            userReference: {
                id: Xrm.Utility.getGlobalContext().userSettings.userId,
                entityType: "systemuser"
            },
            entity: {
                id: entityId.substring(1, entityId.length - 1),
                entityType: "quote"
            }
        }).then(
            function (result) {
                debugger;
                Xrm.Utility.closeProgressIndicator();
                if (result.ok) {
                    Xrm.Navigation.openWebResource("findes_html_quote_discount_page", windowOptions, encodeURIComponent(JSON.parse(result.responseText).discountUrl));
                }
                else
                    Xrm.Navigation.openErrorDialog({ message: "Houve um erro ao tentar abrir a página com as informações sobre o desconto de alçada. Para mais detalhes, baixe o log ou entre em contato com o administrador.", details: JSON.stringify(result) });
            },
            function (error) {
                debugger;
                Xrm.Utility.closeProgressIndicator();
                if (error.message === 'Não foi possivel encontrar o desconto vinculado ao time do usuário do contexto.') {
                    executionContext.getFormContext().ui.setFormNotification(error.message, 'ERROR', 'discountError');
                }
                else
                    Xrm.Navigation.openErrorDialog({ message: "Houve um erro ao tentar abrir a página com as informações sobre o desconto de alçada. Para mais detalhes, baixe o log ou entre em contato com o administrador.", details: JSON.stringify(error) });
            }
        );
    },
    ApplyDiscount: function (executionContext) {
        debugger;
        entityId = executionContext.getFormContext().data.entity.getId();
        Xrm.Utility.showProgressIndicator("Aplicando Desconto...")
        Xrm.WebApi.online.execute({
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
                id: Xrm.Utility.getGlobalContext().userSettings.userId,
                entityType: "systemuser"
            },
            entity: {
                id: entityId.substring(1, entityId.length - 1),
                entityType: "quote"
            }
        }).then(
            function (result) {
                debugger;
                Xrm.Utility.closeProgressIndicator();
                if (result.ok) {
                    return;
                }
                else
                    Xrm.Navigation.openErrorDialog({ message: "Houve um erro ao tentar abrir a página com as informações sobre o desconto de alçada. Para mais detalhes, baixe o log ou entre em contato com o administrador.", details: JSON.stringify(result) });
            },
            function (error) {
                debugger;
                Xrm.Utility.closeProgressIndicator();
                Xrm.Navigation.openErrorDialog({ message: "Houve um erro ao tentar abrir a página com as informações sobre o desconto de alçada. Para mais detalhes, baixe o log ou entre em contato com o administrador.", details: JSON.stringify(error) })
            }
        );
    },
    _namespace: true
}