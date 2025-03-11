/*
	2024-01-15 rpn.js by Heinrich Elsigan
*/

var fX;
var fY;
var textbox0, textboxtop, textboxresult, bequ, benter;
var textcursor = 9;
var metacursor = document.getElementById("metacursor");
textboxtop = document.getElementById("TextBox_Top");
textboxresult = document.getElementById("TextBox_Calc");
bequ = document.getElementById("Bequ");
benter = document.getElementById("BEnter");

if (textboxtop != null) textboxtop.focus();

var keys = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
	"[", "]", "(", ")",
	".", ",", ";", ":",
	"*", "/", "+", "-",
	"²", "³", "°", "^",
	"§", "$", "%", "&",
	"@", "~", "#", "\'",
	"|", "<", ">", "\""];

function ccalcInit() {
	
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

	//window.onload = function (e) {
	//	if (textboxtop != null) textboxtop.focus();
	//}

	window.onkeydown = function (k) {
		bequ = document.getElementById("Bequ");
		benter = document.getElementById("BEnter");
		if (k.which == 10 || k.which == 13) {
			if (textboxtop.innerText != null && textboxtop.innerText.length > 0) {
				if (benter != null) {
					benter.click();
					return;
				}
			}
		}
		if (k.which == 61) {
			if (bequ != null) {
				bequ.click();
				return;
			}
		}
	};
	
}


function captureKey(keyWhich) {
	alert("Key pressed: " + parseInt(keyWhich));

}
