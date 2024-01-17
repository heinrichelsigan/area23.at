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
		else
			metacursor.setAttribute("content", textcursor);
	}
	let currentTextBox = document.getElementById("textbox" + textcursor);
	if (currentTextBox == null) {
		alert("currentTextBox is null: document.getElementById('textbox'" + textcursor + ");");
		return;
	}


	window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
		if (e.which == 96 || e.which == 48) {
			currentTextBox.innerText += "0";
			return;
		}
		if (e.which == 97 || e.which == 49) {
			currentTextBox.innerText += "1";
			return;
		}
		if (e.which == 98 || e.which == 50) {
			currentTextBox.innerText += "2";
			return;
		}
		if (e.which == 99 || e.which == 51) {
			currentTextBox.innerText += "3";
			return;
		}
		if (e.which == 100 || e.which == 52) {
			currentTextBox.innerText += "4";
			return;
		}
		if (e.which == 101 || e.which == 53) {
			currentTextBox.innerText += "5";
			return;
		}
		if (e.which == 102 || e.which == 54) {
			currentTextBox.innerText += "6";
			return;
		}
		if (e.which == 103 || e.which == 55) {
			currentTextBox.innerText += "7";
			return;
		}
		if (e.which == 104 || e.which == 56) {
			currentTextBox.innerText += "8";
			return;
		}
		if (e.which == 105 || e.which == 57) {
			currentTextBox.innerText += "9";
			return;
		}
		if (e.which == 10 || e.which == 13) {
			if (currentTextBox.innerText != null && currentTextBox.innerText.length > 0) {
				if (metacursor == null)
					metacursor = document.getElementById("metacursor");
				if (metacursor != null) {
					metacursor.setAttribute("content", --textcursor);
				}
			}
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
