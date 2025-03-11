/*
    2023-12-22 fortune.js © by Heinrich Elsigan
    https://area23.at/js/fortune.js
    https://area23.at/js/fortune.js
*/

var theFortuneForm, fortuneUrl;

function ReloadForm() { // var url = "https://area23.at/cgi/fortune.cgi";
    var delay = 16000;
    setTimeout(function () { window.location.reload(); }, delay);
    return;
}

function VerifyFortuneUrl() {
    const urlWindowLocation = new URL(window.location.toLocaleString());
    fortuneUrl = new URL(urlWindowLocation);
    if (fortuneUrl.indexOf("Fortun") > -1)
        return true;
    return false;
}


function GetFortuneForm() {
    theFortuneForm = null;        
    var possibleForms = ["Area23MasterForm", "form1", "form", "form0", "from2"];
    let _form_Id = "";
    possibleForms.forEach(function (_form_Id) {
        if (theFortuneForm == null)
            theFortuneForm = document.getElementById(_form_Id);        
    });
    if (theFortuneForm == null) {
        var allForms = document.getElementsByTagName("form");
        if (allForms != null && allForms.length > 0)
            theFortuneForm = allForms[0];
    }
    return theFortuneForm;       
}

function FormSubmit() {
    theFortuneForm = GetFortuneForm();
    if (VerifyFortuneUrl() && theFortuneForm != null)
        theFortuneForm.submit();
}
