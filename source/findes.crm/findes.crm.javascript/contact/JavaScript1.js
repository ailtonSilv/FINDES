function validacnpj(campo) {

    var valorCnpj = campo.getEventSource().getValue();
    var vCNPJ = valorCnpj;

    var nomeCampo = campo.getEventSource().getName();
    var control = Xrm.Page.getControl(nomeCampo);

    if (!vCNPJ || vCNPJ === null) vCNPJ = "";
    vCNPJ = vCNPJ.replace(/[^\d]/g, '');

    var b = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    var cnpjValido = true;

    if (vCNPJ.length !== 14)
        cnpjValido = false;

    if (/0{14}/.test(vCNPJ))
        cnpjValido = false;

    for (var i = 0, n = 0; i < 12; n += vCNPJ[i] * b[++i]);
    if (vCNPJ[12] !== (((n %= 11) < 2) ? 0 : 11 - n))
        cnpjValido = false;

    for (var j = 0, m = 0; j <= 12; m += vCNPJ[i] * b[i++]);
    if (vCNPJ[13] !== (((m %= 11) < 2) ? 0 : 11 - m))
        cnpjValido = false;

    if (!cnpjValido)
        control.setNotification("CNPJ Inválido");
    else {
        control.clearNotification();
        vCNPJ = vCNPJ.substring(0, 2) + "." + vCNPJ.substring(2, 5) + "." + vCNPJ.substring(5, 8) + "/" + vCNPJ.substring(8, 12) + "-" + vCNPJ.substring(12, 14);
        campo.getEventSource().setValue(vCNPJ);
    }
}

function ValidaTelefone(campo) {

    var telefoneValido = true;
    var getValorTel = campo.getEventSource().getValue();
    var telefone = getValorTel;
    var nomeCampo = campo.getEventSource().getName();
    var control = Xrm.Page.getControl(nomeCampo);

    if (telefone === null || telefone === "") {
        telefone = null;
        telefoneValido = true;
        control.clearNotification();
    }
    else {

        telefone = telefone.replace(/[^0-9]/g, "");

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
        
}

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


function obrigatoriedadeCPF() {

    var contatoComercial = 482870001;
    var perfil = Xrm.Page.getAttribute("findes_tipocontato").getValue();

    var email = Xrm.Page.getAttribute("emailaddress1");
    var telefoneComercial = Xrm.Page.getAttribute("telephone1");
    var cpf = Xrm.Page.getAttribute("findes_cpf");
    

    if (perfil === contatoComercial) {
        cpf.setRequiredLevel("none");
        telefoneComercial.setRequiredLevel("required");
        email.setRequiredLevel("required");
    } else {
        cpf.setRequiredLevel("required");
        telefoneComercial.setRequiredLevel("none");
        email.setRequiredLevel("none");
    }
}