function addFilterOnContactByAccount(context) {

    var customer = context.getFormContext().getAttribute("customerid").getValue();

    debugger;

    //var aux = Xrm.Page.getAttribute("customerid").getValue()[0];

    if (customer != null) {
        Xrm.Page.getControl("header_process_parentcontactid").addPreSearch(function () { addFilterContact(customer) });
    }
    else {
        Xrm.Page.getControl("header_process_parentcontactid").removePreSearch(function () { addFilterContact(customer) });
    }

}

function addFilterContact(customer) {
    debugger;
    //var account = Xrm.Page.getAttribute("customerid").getValue()[0];
    var accountId = customer[0].id;
    var accountName = customer[0].name;
    var fetchXML = '<filter type="and">' +
        '<condition attribute="parentcustomerid" value="' + accountId.toString() + '" uitype="account" uiname="' + accountName.toString() + '" operator="eq"/>' +
        '</filter>';

    Xrm.Page.getControl("header_process_parentcontactid").addCustomFilter(fetchXML);
}

function addFilterOnCurrentQuote(context) {
    debugger;
    var aux = context.getFormContext();
    var opportunityId = aux.data.entity.getId();
    Xrm.Page.getControl("header_process_findes_propostaatualid").addPreSearch(function () { addFilterQuote(opportunityId); });
}

function addFilterQuote(opportunityId) {
    debugger;
    var fetchXML = '<filter type="and">' +
        '<condition attribute="opportunityid" operator="eq" uitype="opportunity" value="' + opportunityId.toString() + '" />' +
        '</filter>';

    Xrm.Page.getControl("header_process_findes_propostaatualid").addCustomFilter(fetchXML);
}