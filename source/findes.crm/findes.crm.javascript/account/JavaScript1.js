function contatoComercial() {
    var contatoComercial = 482870001;
    var tipoContato = Xrm.Page.getAttribute("findes_tipocontato").getValue();
    var cpfControl = Xrm.Page.getAttribute("findes_cpf");

    if (tipoContato === contatoComercial) {
        cpfControl.setRequiredLevel("recommended");
    }
    else {
        cpfControl.setRequiredLevel('required');
    }
}

function validacpf(campo) {

    var valorCpf = campo.getEventSource().getValue();
    var cpf = valorCpf;


    var nomeCampo = campo.getEventSource().getName();
    var control = Xrm.Page.getControl(nomeCampo);

    var cpfValido = true;
    
    if (cpf === '' || cpf === null || cpf === "") {
        cpf = null;
        cpfValido = true;
        control.clearNotification('1');
        control.clearNotification();
    }
    

    else {
        cpf = cpf.replace(/[^\d]+/g, '');

        if (cpf.length !== 11 ||
            cpf === "00000000000" ||
            cpf === "11111111111" ||
            cpf === "22222222222" ||
            cpf === "33333333333" ||
            cpf === "44444444444" ||
            cpf === "55555555555" ||
            cpf === "66666666666" ||
            cpf === "77777777777" ||
            cpf === "88888888888" ||
            cpf === "99999999999")
            cpfValido = false;

        add = 0;
        for (i = 0; i < 9; i++)
            add += parseInt(cpf.charAt(i)) * (10 - i);
        rev = 11 - (add % 11);
        if (rev === 10 || rev === 11)
            rev = 0;
        if (rev !== parseInt(cpf.charAt(9)))
            cpfValido = false;

        add = 0;
        for (i = 0; i < 10; i++)
            add += parseInt(cpf.charAt(i)) * (11 - i);
        rev = 11 - (add % 11);
        if (rev === 10 || rev === 11)
            rev = 0;
        if (rev !== parseInt(cpf.charAt(10)))
            cpfValido = false;

        if (!cpfValido) {
            control.setNotification("CPF Inválido",'1');
        }
        else {
            control.clearNotification();

            cpf = cpf.substring(0, 3) + "." + cpf.substring(3, 6) + "." + cpf.substring(6, 9) + "-" + cpf.substring(9, 11);
            campo.getEventSource().setValue(cpf);
        }
    }
}

//ReferenceError: validacpf is not defined at eval (eval at RunHandlerInternal 

function ValidaCep(campo) {

    var cepValido = true;
    var ceep = campo.getEventSource().getValue();
    var cep = ceep;
    var nomeCampo = campo.getEventSource().getName();
    var control = Xrm.Page.getControl(nomeCampo);

    if (!cep || cep === null) cep = "";

    cep = cep.replace(/[^0-9]/g, "");
    if (cep === "") cepValido = false;

    if (cep.length !== 8 ||
        cep === "00000000" ||
        cep === "11111111" ||
        cep === "22222222" ||
        cep === "33333333" ||
        cep === "44444444" ||
        cep === "55555555" ||
        cep === "66666666" ||
        cep === "77777777" ||
        cep === "88888888" ||
        cep === "99999999")
        cepValido = false;

    if (!cepValido)

        control.setNotification("Formato Inválido de Cep");
    else {
        control.clearNotification();

        cep = cep.substr(0, 5) + " - " + cep.substr(5, 8);
        campo.getEventSource().setValue(cep);

    }
}

function ValidaTelefone(campo) {

    var telefoneValido = true;
    var getValorTel = campo.getEventSource().getValue();
    var telefone = getValorTel;
    var nomeCampo = campo.getEventSource().getName();
    var control = Xrm.Page.getControl(nomeCampo);

    if (!telefone || telefone === null) telefone = "";

    telefone = telefone.replace(/[^0-9]/g, "");
    if (telefone === "") telefoneValido = false;

    if (telefone === "00000000000" ||
        telefone === "11111111111" ||
        telefone === "22222222222" ||
        telefone === "33333333333" ||
        telefone === "44444444444" ||
        telefone === "55555555555" ||
        telefone === "66666666666" ||
        telefone === "77777777777" ||
        telefone === "88888888888" ||
        telefone === "99999999999")
        telefoneValido = false;

    if (!telefoneValido) {

        control.setNotification("Formato Inválido de telefone.");
    } else {

        switch (telefone.length) {

            case 10:
                telefone = "(" + telefone.substr(0, 2) + ")" + " " + telefone.substr(2, 4) + "-" + telefone.substr(6, 4);
                control.clearNotification();
                campo.getEventSource().setValue(telefone);
                break;

            case 11:
                telefone = "(" + telefone.substr(0, 2) + ")" + " " + telefone.substr(2, 5) + "-" + telefone.substr(7, 4);
                control.clearNotification();
                campo.getEventSource().setValue(telefone);
                break;

            default:
                control.setNotification("Formato Inválido de telefone.");
                break;
        }
    }
}

function PreencheCamposEndereco() {

    LogradouroPrincipal();
    BairroPrincipal();
    ComplementoPrincipal();
    CepPrincipal();
    EstadoPrincipal();
    MunicipioPrincipal();
    LogradouroPrincipalProp();
    BairroPrincipalProp();
    ComplementoPrincipalProp();
    CepPrincipalProp();
}



function LogradouroPrincipal() {

    var logPrincipal = Xrm.Page.getAttribute("address1_line1").getValue();
    Xrm.Page.getAttribute("address2_line1").setValue(logPrincipal);
}

function LogradouroPrincipalProp() {

    var logPrincipalProp = Xrm.Page.getAttribute("shipto_line1").getValue();
    Xrm.Page.getAttribute("billto_line1").setValue(logPrincipalProp);
}

function BairroPrincipal() {

    var bairroPrincipal = Xrm.Page.getAttribute("address1_line3").getValue();
    Xrm.Page.getAttribute("address2_line3").setValue(bairroPrincipal);
}

function BairroPrincipalProp() {

    var bairroPrincipalProp = Xrm.Page.getAttribute("shipto_line3").getValue();
    Xrm.Page.getAttribute("billto_line3").setValue(bairroPrincipalProp);
}

function ComplementoPrincipal() {

    var complePrincipal = Xrm.Page.getAttribute("address1_line2").getValue();
    Xrm.Page.getAttribute("address2_line2").setValue(complePrincipal);
}

function ComplementoPrincipalProp() {

    var complePrincipalProp = Xrm.Page.getAttribute("shipto_line2").getValue();
    Xrm.Page.getAttribute("billto_line2").setValue(complePrincipalProp);
}

function CepPrincipal() {

    var cepPrincipal = Xrm.Page.getAttribute("address1_postalcode").getValue();
    Xrm.Page.getAttribute("address2_postalcode").setValue(cepPrincipal);
}

function CepPrincipalProp() {

    var cepPrincipalProp = Xrm.Page.getAttribute("shipto_postalcode").getValue();
    Xrm.Page.getAttribute("billto_postalcode").setValue(cepPrincipalProp);
}

function EstadoPrincipal() {

    var estadoPrincipal = Xrm.Page.getAttribute("findes_estadoprincipalid").getValue();
    Xrm.Page.getAttribute("findes_estadopagamentoid").setValue(estadoPrincipal);
}

function MunicipioPrincipal() {

    var municipioPrincipa = Xrm.Page.getAttribute("findes_municipioprincipalid").getValue();
    Xrm.Page.getAttribute("findes_municipiopagamentoid").setValue(municipioPrincipa);
} 