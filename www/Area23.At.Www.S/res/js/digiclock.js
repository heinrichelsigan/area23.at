/*
    2024-03-02 digiclock.js © by Heinrich Elsigan
    https://darkstar.work/js/digiclock.js
    https://area23.at/js/digiclock.js
*/

var hours, minutes, seconds;
var digiYear, digiMonth, digiDay, digiTime, digiHours, digiMinutes, digiSeconds;

function ReloadForm() { // var url = "https://darkstar.work/cgi/fortune.cgi";
    var delay = 100;
    setTimeout(function () { window.location.reload(); }, delay);
    return;
}

function SetDigiTime() {
    initDigitalTime();
    setTimeout(function () { setDigiTime() }, 900);
}

function initDigitalTime() {
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
