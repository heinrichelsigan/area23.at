/*
    2026-05-10 qr.js needed for color picker settímhs © by Heinrich Elsigan
    https://area23.at/net/res/js/qr.js
    
*/

var buttonQRCode, inputcolor, inputbackcolor, colorpicker, backcolorpicker;

function highLightOnChange(highLightId) {
    if (highLightId != null && document.getElementById(highLightId) != null) {
        if (document.getElementById(highLightId).style.borderStyle == "dotted" ||
            document.getElementById(highLightId).style.borderColor == "red") {
            // do nothing when dotted
        }
        else if (document.getElementById(highLightId).style.borderStyle == "dashed" ||
            document.getElementById(highLightId).style.borderColor == "red") {

            document.getElementById(highLightId).style.borderStyle = "dotted";
        }
        else {
            // set border-width: 1; border-style: dashed
            document.getElementById(highLightId).style.borderColor = "red";
            document.getElementById(highLightId).style.borderStyle = "dashed";
        }
    }
}

function newQrColor(color) {
    colorpicker = document.getElementById("color1");
    if (colorpicker == null)
        colorpicker = document.getElementsByName("color1")[0]

    inputcolor = document.getElementById("input_color");
    if (inputcolor == null)
        inputcolor = document.getElementsByName("input_color")[0]

    try {

        if (inputcolor != null) {
            inputcolor.setAttribute("Text", color);
            inputcolor.setAttribute("qrcolor", color);
            inputcolor.value = color;
            inputcolor.style.borderColor = color;
            inputcolor.style.textColor = color;
            // inputcolor.style.backgroundColor = color;
        }

        buttonQRCode = document.getElementById("Button_QRCode");
        if (buttonQRCode == null) {
            buttonQRCode = document.getElementsByName("Button_QRCode");
        }
        if (buttonQRCode != null) {
            buttonQRCode.setAttribute("qrcolor", color);
            buttonQRCode.style.borderColor = color;
            buttonQRCode.style.textColor = color;
            // buttonQRCode.style.backgroundColor = color;
            // buttonQRCode.setAttribute("BackColor", color);
            // buttonQRCode.setAttribute("ToolTip", color);
            document.getElementById("Button_QRCode").click();
        }

    } catch (exCol) {
        alert("getElementsById('input_color') " + exCol);
    }
}

function newBackgroundColor(bgcolor) {
    
    backcolorpicker = document.getElementById("color0");
    if (backcolorpicker == null)
        backcolorpicker = document.getElementsByName("color0")[0]

    inputbackcolor = document.getElementById("input_backcolor");
    if (inputbackcolor == null)
        inputbackcolor = document.getElementsByName("input_backcolor")[0];

    try {
        
        if (inputbackcolor != null) {
            inputbackcolor.setAttribute("Text", bgcolor);
            inputbackcolor.setAttribute("qrcolor", bgcolor);
            inputbackcolor.value = bgcolor;
            inputbackcolor.style.borderColor = bgcolor;
            // inputbackcolor.style.textColor = bgcolor;
        }

        buttonQRCode = document.getElementById("Button_QRCode");
        if (buttonQRCode == null) {
            buttonQRCode = document.getElementsByName("Button_QRCode");
        }
        if (buttonQRCode != null) {
            buttonQRCode.setAttribute("bgcolor", bgcolor);
            buttonQRCode.style.bgcolor = bgcolor;
            // buttonQRCode.style.backgroundColor = color;
            // buttonQRCode.setAttribute("BackColor", color);
            // buttonQRCode.setAttribute("ToolTip", color);
            document.getElementById("Button_QRCode").click();
        }

    } catch (exCol) {
        alert("getElementsById('input_backcolor') " + exCol);
    }
}

function setColorPicker() {
    initColorPickers();
}

function initColorPickers() {
    colorpicker = document.getElementById("color1");
    if (colorpicker == null)
        colorpicker = document.getElementsByName("color1")[0]

    inputcolor = document.getElementById("input_color");
    if (inputcolor == null)
        inputcolor = document.getElementsByName("input_color")[0]

    if (colorpicker != null && inputcolor != null && inputcolor.value != null && inputcolor.value != "" && inputcolor.value.length >= 6) {
        colorpicker.value = inputcolor.value;
    }

    backcolorpicker = document.getElementById("color0");
    if (backcolorpicker == null)
        backcolorpicker = document.getElementsByName("color0")[0]

    inputbackcolor = document.getElementById("input_backcolor");
    if (inputbackcolor == null)
        inputbackcolor = document.getElementsByName("input_backcolor")[0];

    if (backcolorpicker != null && inputbackcolor != null && inputbackcolor.value != null && inputbackcolor.value != "") {
        // if (backcolorpicker.value == inputbackcolor.value)
        //     return;
        backcolorpicker.value = inputbackcolor.value;
    }
}
