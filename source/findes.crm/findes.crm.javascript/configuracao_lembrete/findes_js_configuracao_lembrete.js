var Findes = window.Findes || {};

Findes.DurationChange = function (executionContext) {
    var formContext = executionContext.getFormContext();
    if (formContext.getAttribute('findes_datageracaotarefa').getValue() && formContext.getAttribute('findes_periodotarefa').getValue()) {
        formContext.getAttribute('findes_dataproximageracao').setValue(formContext.getAttribute('findes_datageracaotarefa').getValue().setMinutes(formContext.getAttribute('findes_datageracaotarefa').getValue().getMinutes() + formContext.getAttribute('findes_periodotarefa').getValue()));
        formContext.getAttribute('findes_dataproximageracao').setSubmitMode('always');
    }
};