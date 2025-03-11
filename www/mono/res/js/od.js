/*
    2023-12-20 od.js © by Heinrich Elsigan
    https://area23.at/cgi/od.cgi
    https://area23.at/test/cgi/od.html
*/

var odForm, odUrl;

function VerifyOdUrl() {
    const urlWindowLocation = new URL(window.location.toLocaleString());
    odUrl = new URL(urlWindowLocation);
    if ((odUrl.indexOf("od") > -1) ||
        (odUrl.indexOf("Octal") > -1) ||
        (odUrl.indexOf("HexDump") > -1))
            return true;
    return false;
}


function GetOdForm() {
    odForm = null;
    var possibleForms = ["Area23MasterForm", "form1", "form", "form0", "from2"];
    let _form_Id = "";

    possibleForms.forEach(function (_form_Id) {
        if (odForm == null)
            odForm = document.getElementById(_form_Id);
    });
    if (odForm == null) {
        var allForms = document.getElementsByTagName("form");
        if (allForms != null && allForms.length > 0)
            odForm = allForms[0];
    }
    return odForm;
}

function OdFormSubmit() {
    odForm = GetOdForm();
    if (odForm != null && VerifyOdUrl())
        odForm.submit();
}

function FormSubmit() { OdFormSubmit(); }
