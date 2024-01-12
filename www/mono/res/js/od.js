/*
    2023-12-20 od.js © by Heinrich Elsigan
    https://darkstar.work/cgi/od.cgi
    https://area23.at/test/cgi/od.html
*/

var theForm;

function GetForm() {
    if (document.getElementById) { theForm = document.getElementById('form1'); }
    else { theForm = document.form1; }
}

function WebForm_TextBoxKeyHandler(event) {
    var target = event.target;
    if ((target == null) || (typeof (target) == "undefined"))
        target = event.srcElement;
    if (event.keyCode == 13) {
        if ((typeof (target) != "undefined") && (target != null)) {
            if (typeof (target.onchange) != "undefined") {
                target.onchange();
                event.cancelBubble = true;
                if (event.stopPropagation) event.stopPropagation();
                return false;
            }
        }
    }
    return true;
}

function FormSubmit() {
    GetForm();
    theForm.submit();
}
