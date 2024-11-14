/*
    2024-11-15 area23.js merged with  digiclock.js © by Heinrich Elsigan
    https://darkstar.work/js/area23.js
    https://area23.at/js/area23.js
*/

var hours, minutes, seconds;
var digiYear, digiMonth, digiDay, digiTime, digiHours, digiMinutes, digiSeconds;
var buttonQRCode, inputcolor, inputbackcolor, colorpicker, backcolorpicker;


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


function ReloadUnixForm() { // var url = "https://darkstar.work/cgi/fortune.cgi";
    var delay = 100;
    setTimeout(function () { window.location.reload(); }, delay);
    return;
}

function SetTimeDigital() {
    initDigitalTime();
    setTimeout(function () { setDigiTime() }, 900);
}

function InitTimeDigital() {
    const now = new Date(Date.now());
    seconds = now.getSeconds();
    digiSeconds = (seconds < 10) ? "0" + seconds : seconds + "";
    minutes = now.getMinutes();
    digiMinutes = (minutes < 10) ? ("0" + minutes) : (minutes + "");
    hours = now.getHours();
    digiHours = (hours < 10) ? " " + hours : hours + "";

    digiTime = digiHours + ":" + digiMinutes + ":" + digiSeconds;

    document.getElementById("spanHoursId").innerText = digiHours;
    document.getElementById("spanMinutesId").innerText = digiMinutes;
    document.getElementById("spanSecondsId").innerText = digiSeconds;

    if (seconds == 0) {
        if (minutes == 0) {
            ReloadForm();
            return;
        }
        alert("Digital time: " + digiTime);
    }

    console.log(`Digital time: ${digiTime}`);

    return digiTime;
}


function newQrColor(color) {

    try {
        if (document.getElementById("Button_QRCode") != null) {
            buttonQRCode = document.getElementById("Button_QRCode");
            buttonQRCode.setAttribute("qrcolor", color);
            buttonQRCode.style.borderColor = color;
            // buttonQRCode.style.backgroundColor = color;
            // buttonQRCode.setAttribute("BackColor", color);
            // buttonQRCode.setAttribute("ToolTip", color);
        }

        if (document.getElementById("input_color") != null) {
            inputcolor = document.getElementById("input_color");
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
}

function newBackgroundColor(bgcolor) {
    // document.bgColor = color;
    try {       
        if (document.getElementById("input_backcolor") != null) {
            inputbackcolor = document.getElementById("input_backcolor");
            inputbackcolor.setAttribute("Text", bgcolor);
            inputbackcolor.setAttribute("qrcolor", bgcolor);
            inputbackcolor.value = bgcolor;
            inputbackcolor.style.borderColor = bgcolor;
            // inputbackcolor.style.textColor = bgcolor;
        }
    } catch (exCol) {
        alert("getElementsById('input_backcolor') " + exCol);
    }
}
function setColorPicker() {
    colorpicker = document.getElementById("color1");
    inputcolor = document.getElementById("input_color");
    if (colorpicker != null && inputcolor != null && inputcolor.value != null && inputcolor.value != "" && inputcolor.value.length >= 6) {
        if (colorpicker.value == inputcolor.value) {
            ;
        }
        else  
            colorpicker.value = inputcolor.value;
    }
    backcolorpicker = document.getElementById("color0");
    inputbackcolor = document.getElementById("input_backcolor");
    if (backcolorpicker != null && inputbackcolor != null && inputbackcolor.value != null && inputbackcolor.value.length >= 6) {
        // if (backcolorpicker.value == inputbackcolor.value)
        //     return;
        backcolorpicker.value = inputbackcolor.value;
    }
}


// setColorPicker();
// example for loading more javascript dynamically from main script by creating script elements in html head.
loadScript('https://area23.at/js/fortune.js', false, function () { console.log('finished loading fortune: ' + GetFortuneForm()); });
loadScript('https://area23.at/js/od.js', false, function () { console.log('finished loading od: ' + GetOdForm()); });
