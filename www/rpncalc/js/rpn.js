/*
	2024-01-15 rpn.js by Heinrich Elsigan
*/

var fX;
var fY;
var textbox0, textbox1, textbox2, textbox3, textbox4, textbox5, textbox6, textbox7, textbox8, textbox9, textbox10;
var textcursor = 9;
var metacursor = document.getElementById("metacursor");
var bEnter = document.getElementById("BEnter");
textbox0 = document.getElementById("texbox0");
textbox1 = document.getElementById("texbox1");
textbox2 = document.getElementById("texbox2");
textbox3 = document.getElementById("texbox3");
textbox4 = document.getElementById("texbox4");
textbox5 = document.getElementById("texbox5");
textbox6 = document.getElementById("texbox6");
textbox7 = document.getElementById("texbox7");
textbox8 = document.getElementById("texbox8");
textbox9 = document.getElementById("texbox9");
if (textbox0 != null) textbox0.focus();

var keys = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
	"[", "]", "(", ")",
	".", ",", ";", ":",
	"*", "/", "+", "-",
	"²", "³", "°", "^", 
	"§", "$", "%", "&", 
	"@", "~", "#", "\'",
	"|", "<", ">", "\""];

function rpnInit() {
	
	
	document.getElementById("headerImg").src = "img/header.png";
	if (metacursor == null)
		metacursor = document.getElementById("metacursor");
	if (metacursor != null) {
		let metacontent = metacursor.getAttribute("content");
		if (metacontent != null)
			textcursor = parseInt(metacontent);
	}
	let currentTextBox = document.getElementById("textbox" + textcursor);


	window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
		if (e.which == 96 || e.which == 48) {
			document.getElementById("B0").click();
			return;
		}
		if (e.which == 97 || e.which == 49) {
			document.getElementById("B1").click();
			return;
		}
		if (e.which == 98 || e.which == 50) {
			document.getElementById("B2").click();
			return;
		}
		if (e.which == 99 || e.which == 51) {
			document.getElementById("B3").click();
			return;
		}
		if (e.which == 100 || e.which == 52) {
			document.getElementById("B4").click();
			return;
		}
		if (e.which == 101 || e.which == 53) {
			document.getElementById("B5").click();
			return;
		}
		if (e.which == 102 || e.which == 54) {
			document.getElementById("B6").click();
			return; 
		}
		if (e.which == 103 || e.which == 55) {
			document.getElementById("B7").click();
			return;
		}
		if (e.which == 104 || e.which == 56) {
			document.getElementById("B8").click();
			return;
		}
		if (e.which == 105 || e.which == 57) {
			document.getElementById("B9").click();
			return;
		}
		if (e.which == 10 || e.which == 13) {
			bEnter = document.getElementById("BEnter");
			if (bEnter != null) {
				bEnter.click();
				return;
			}
		}

		if (e.which >= 10 && e.which <= 15) 
			captureKey(e.which);

		if (e.which >= 19 && e.which <= 36)
			captureKey(e.which);

		if (e.which >= 37 && e.which <= 40)
			captureKey(e.which);
	
		if (e.which >= 40 && e.which <= 128) {
			captureKey(e.which);
		}

		if (e.which > 128 && e.which < 256) {
			captureKey(e.which);
		}
	};	
}


function captureKey(keyWhich) {
	alert("Key pressed: " + parseInt(keyWhich));

}


function cloneObj(obj) {
	var copy;
	if (obj instanceof Object) {
		copy = {};
		for (var attr in obj) {
			if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
		}
		return copy;
	}
}


function copyImg(imgC) {
	var imgD = new Image();
	if (imgC != null && imgC.id != null) {
		imgD.id = imgC.id;
		imgD.src = imgC.src;
		imgD.width = imgC.width;
		imgD.height = imgC.height;
		imgD.alt = imgC.alt;
		// imgD.title = imgC.title;
		// imgD.className = imgC.className;
		// imgD.setAttribute("alt", imgC.getAttribute("alt"));
		if (imgC.getAttribute("title") != null)
			imgD.setAttribute("title", imgC.getAttribute("title"));
		if (imgC.getAttribute("className") != null)
			imgD.setAttribute("className", imgC.getAttribute("className"));
		imgD.setAttribute("class", imgC.getAttribute("class"));
		if (imgC.getAttribute("cellid") != null)
			imgD.setAttribute("cellid", imgC.getAttribute("cellid"));
		if (imgC.getAttribute("idwood") != null)
			imgD.setAttribute("idwood", imgC.getAttribute("idwood"));
		imgD.setAttribute("border", 0);		

	}
	return imgD;
}
