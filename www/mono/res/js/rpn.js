/*
	2024-01-15 rpn.js by Heinrich Elsigan
*/

var fX;
var fY;
var textbox0, textboxtop, textboxRpn;
var textcursor = 9;
var metacursor = document.getElementById("metacursor");
var bEnter = document.getElementById("BEnter");
textbox0 = document.getElementById("texbox0");
textboxtop = document.getElementById("texboxtop");
textboxRpn = document.getElementById("texboxRpn");

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

	document.getElementById("headerImg").src = "res/img/rpnHeader.png";
	if (metacursor == null)
		metacursor = document.getElementById("metacursor");
	if (metacursor != null) {
		let metacontent = metacursor.getAttribute("content");
		if (metacontent != null)
			textcursor = parseInt(metacontent);
		else
			metacursor.setAttribute("content", textcursor);
	} 
	if (textboxtop == null) 
		textboxtop = document.getElementById("textboxtop"); 
	if (textboxtop == null) {
		alert("textboxtop is null: document.getElementById('textbotop');");
		return;
	}

	window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
		if (e.which == 96 || e.which == 48) {
			textboxtop.innerText += "0";
			return;
		}
		if (e.which == 97 || e.which == 49) {
			textboxtop.innerText += "1";
			return;
		}
		if (e.which == 98 || e.which == 50) {
			textboxtop.innerText += "2";
			return;
		}
		if (e.which == 99 || e.which == 51) {
			textboxtop.innerText += "3";
			return;
		}
		if (e.which == 100 || e.which == 52) {
			textboxtop.innerText += "4";
			return;
		}
		if (e.which == 101 || e.which == 53) {
			textboxtop.innerText += "5";
			return;
		}
		if (e.which == 102 || e.which == 54) {
			textboxtop.innerText += "6";
			return;
		}
		if (e.which == 103 || e.which == 55) {
			textboxtop.innerText += "7";
			return;
		}
		if (e.which == 104 || e.which == 56) {
			textboxtop.innerText += "8";
			return;
		}
		if (e.which == 105 || e.which == 57) {
			textboxtop.innerText += "9";
			return;
		}
		if (e.which == 10 || e.which == 13) {
			if (textboxtop.innerText != null && textboxtop.innerText.length > 0) {
				if (metacursor == null)
					metacursor = document.getElementById("metacursor");
				if (metacursor != null)  
					metacursor.setAttribute("content", --textcursor);
			}
			bEnter = document.getElementById("BEnter");
			if (bEnter != null) {
				bEnter.click();
				return;
			}
		}

		// if (e.which >= 10 && e.which <= 15)
		//  	captureKey(e.which);

		// if (e.which >= 19 && e.which <= 36)
		// 	captureKey(e.which);

		// if (e.which >= 37 && e.which <= 40)
		// 	captureKey(e.which);

		// if (e.which >= 40 && e.which <= 128) {
		// 	captureKey(e.which);
		// }

		// if (e.which > 128 && e.which < 256) {
		// 	captureKey(e.which);
		// }
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
