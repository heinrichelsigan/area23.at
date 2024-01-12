/*
    2023-12-22 fortune.js © by Heinrich Elsigan
    https://darkstar.work/js/fortune.js
    https://area23.at/js/fortune.js
*/

var theForm;

function ReloadForm() { // var url = "https://darkstar.work/cgi/fortune.cgi";
    var delay = 16000;
    setTimeout(function () { window.location.reload(); }, delay);
    return;
}

function GetForm() {
    if (document.getElementById) { theForm = document.getElementById('form1'); }
    else { theForm = document.form1; }
}

function FormSubmit() {
    GetForm();
    theForm.submit();
}
