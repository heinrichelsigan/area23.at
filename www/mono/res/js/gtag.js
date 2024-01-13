/*
    2024-01-14 gtag.js script to load google tag script async
    https://darkstar.work/js/gtag.js
    https://area23.at/js/gtag.js
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

function gtag() { dataLayer.push(arguments); }

function gTagInit() {
    loadScript('https://www.googletagmanager.com/gtag/js?id=G-01S65129V7', true,
        function () {
            window.dataLayer = window.dataLayer || [];
            gtag('js', new Date());
            gtag('config', 'G-01S65129V7');
            console.log('finished loading google script : https://www.googletagmanager.com/gtag/js?id=G-01S65129V7');
        });
}
