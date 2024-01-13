﻿/*
    2023-12-22 fortune.js © by Heinrich Elsigan
    https://darkstar.work/js/area23.js
    https://area23.at/js/area23.js
*/

function loadScript(src, asyn, f) {
    var head = document.getElementsByTagName("head")[0];
    var script = document.createElement("script");
    
    if (asyn) { // set async tag in script                
        script.async = true;
    }

    script.src = src; // set src in script
    var done = false;
    script.onload = script.onreadystatechange = function () {
        // attach to both events for cross browser finish detection:
        if (!done && (!this.readyState ||
            this.readyState == "loaded" || this.readyState == "complete")) {
            done = true;
            if (typeof f == 'function') f();
            // cleans up a little memory:
            script.onload = script.onreadystatechange = null;
            head.removeChild(script);
        }
    };
    head.appendChild(script);
}



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

function newBackgroundColor(color) {
    // document.bgColor = color;

    // try {
    //     if (document.getElementById("TextBox_Color") != null) {
    //         let textBoxColorId = document.getElementById("TextBox_Color");
    //         textBoxColorId.title = color;
    //         textBoxColorId.setAttribute("text", color);
    //         textBoxColorId.setAttribute("qrcolor", color);
    //         textBoxColorId.value = color;
    //         textBoxColorId.style.borderColor = color;
    //         // textBoxColorId.style.backgroundColor = color;
    //     }
    // } catch (exCol) {
    //     alert("getElementById('TextBox_Color') " + exCol);
    // }

    try {
        if (document.getElementById("Button_QRCode") != null) {
            var buttonQRCode = document.getElementById("Button_QRCode");
            buttonQRCode.setAttribute("qrcolor", color);
            buttonQRCode.style.borderColor = color;
            // buttonQRCode.style.backgroundColor = color;
            // buttonQRCode.setAttribute("BackColor", color);
            // buttonQRCode.setAttribute("ToolTip", color);
        }

        if (document.getElementById("input_color") != null) {
            var inputcolor = document.getElementById("input_color");
            inputcolor.setAttribute("Text", color);
            inputcolor.setAttribute("qrcolor", color);
            inputcolor.value = color;
            inputcolor.style.borderColor = color;
            inputcolor.style.textColor = color;
            // inputcolor.style.backgroundColor = color;
        }
    } catch (exCol) {
        alert("getElementsById('input_color') " + exCol);
    }
    // try {
    //     if (document.getElementsByName("selected_color") != null) {
    //         var inputcolors = document.getElementsByName("selected_color");
    //         inputcolors.value = color;
    //         inputcolors.setAttribute("text", color);
    //         inputcolors.setAttribute("qrcolor", color);
    // 		inputcolors.style.borderColor = color;
    //         inputcolors.style.textColor = color;
    //         // inputcolors.style.backgroundColor = color;
    //     }
    // } catch (exCol) {
    //     alert("getElementsByName('selected_color') " + exCol);
    // }
}

function setColorPicker() {
    var color1_id = document.getElementById("color1");
    var inputcolor = document.getElementById("input_color");
    if (color1_id != null && inputcolor != null) {
        if (inputcolor.value != null && inputcolor.value != "" && inputcolor.value.length >= 6) {
            color1_id.value = inputcolor.value;
        }
    }
}

setColorPicker();
// example for loading more javascript dynamically from main script by creating script elements in html head.
loadScript('https://area23.at/js/fortune.js', false, function () { console.log('finished loading fortune: ' + GetFortuneForm()); });
loadScript('https://area23.at/js/od.js', false, function () { console.log('finished loading od: ' + GetOdForm()); });
loadScript('https://area23.at/js/gtag.js', true, function () { gTagInit(); console.log('fished loading google tag async script!'); });