var Findes = window.Findes || {};

Findes.CreateOrder = function (executionContext) {
    debugger;
    var formContext = executionContext.getFormContext();
    if (formContext.data.getIsDirty()) {
        formContext.ui.setFormNotification('Existem alterações não salvas no formulário. Salve as alteraçòes antes de gerar a proposta.', 'INFO', 'gerarPropostaAlert');
        window.setTimeout(function () {
            formContext.ui.clearFormNotification('gerarPropostaAlert');
        }, 1000);
        return;
    }
    formContext.getAttribute("findes_ultimapropostagerada").setValue(new Date());
    formContext.getAttribute('findes_ultimapropostagerada').setSubmitMode('always');
    formContext.getAttribute("findes_gerarproposta").setValue(true);
    formContext.getAttribute('findes_gerarproposta').setSubmitMode('always');
    formContext.data.save();
};