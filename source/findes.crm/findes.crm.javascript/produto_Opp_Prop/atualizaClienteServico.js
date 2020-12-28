var Findes = window.Findes || {};

Findes.AtualizarClienteServico = function (executionContext, environment) {
    var formContext = executionContext.getFormContext();

    if (formContext.getAttribute('findes_clienteservicoid').getValue() === null)
    {
        var entityName = null;
        var entityIDname = null;
        var filter = null;
        switch (environment) {
            case "opportunity":
                entityName = "opportunity";
                entityIDname = "opportunityid";
                filter = "?$select=_customerid_value";
                break;
            case "salesorderdetail":
                entityName = "salesorder";
                entityIDname = "salesorderid";
                filter = "?$select=_customerid_value";
                break;
            case "quotedetail":
                entityName = "quote";
                entityIDname = "quoteid";
                filter = "?$select=_customerid_value";
                break;
        }

        var item = formContext.getAttribute(entityIDname).getValue();
        var itemId = item[0].id;

        if (formContext.getAttribute('findes_clienteservicoid').getValue() === null) {
            //debugger;
            Xrm.WebApi.retrieveRecord(entityName, itemId, filter).then(
                function success(result) {
                    //debugger;
                    console.log("Retrieved values: Customer: " + result._customerid_value);

                    var lookup = new Array();
                    lookup[0] = new Object();
                    lookup[0].id = result["_customerid_value"];
                    lookup[0].name = result["_customerid_value@OData.Community.Display.V1.FormattedValue"];
                    lookup[0].entityType = result["_customerid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];

                    formContext.getAttribute('findes_clienteservicoid').setValue(lookup);
                    formContext.getAttribute('findes_clienteservicoid').setSubmitMode('always');
                },
                function (error) {
                    console.log(error.message);
                }
            );
        }
    }
};