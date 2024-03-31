/*
	2024-03-03 bc.js by Heinrich Elsigan
*/

var lines = 4;
var textBoxBcOut = document.getElementById("TextBox_BcOut");
var textBoxBcOp = document.getElementById("TextBox_BcOp");
var textBoxBcResult = document.getElementById("TextBox_BcResult");
var buttonEnter = document.getElementById("ButtonEnter");
var buttonReset = document.getElementById("ButtonReset");

if (textBoxBcOut != null) textBoxBcOut.focus();

var keys = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
	"[", "]", "(", ")",
	".", ",", ";", ":",
	"*", "/", "+", "-",
	"²", "³", "°", "^",
	"§", "$", "%", "&",
	"@", "~", "#", "\'",
	"|", "<", ">", "\""];

function bcInit() {
	// alert("bcInit()");
	if (textBoxBcOp == null)
		textBoxBcOp = document.getElementById("TextBox_BcOp");
	if (textBoxBcOut == null)
		textBoxBcOut = document.getElementById("TextBox_BcOut");
	if (textBoxBcOut != null) { 			
		textBoxBcOut.focus();
		textBoxBcOut.selectionStart = textBoxBcOut.innerHTML.length - 1;
		textBoxBcOut.selectionEnd = textBoxBcOut.innerHTML.length;
		try {
			setCaretToPos(document.getElementById("TextBox_BcOut"), (textBoxBcOut.innerHTML.length - 1));
		} catch (ePos) {
			console.log(`Exception: ${ePos}`);
		}
		var cRow = textBoxBcOut.value.split("\n").length - 1;
		if (cRow < 4) cRow = 4;
		setTextCursor(document.getElementById("TextBox_BcOut"), textBoxBcOut.value, cRow, 0);
	}

	//if (textBoxBcResult == null)
	//	textBoxBcResult = document.getElementById("TextBox_BcResult");
	//if (buttonReset == null)
	//	buttonReset = document.getElementById("ButtonReset");
	//if (buttonEnter == null)
	//	buttonEnter = document.getElementById("ButtonEnter");
			
	// window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
		//if (e.which == 96 || e.which == 48) {
		//	textBoxBcOut.innerHTML += "0";
		//	return;
		//}
		//if (e.which == 97 || e.which == 49) {
		//	textBoxBcOut.innerHTML += "1";
		//	return;
		//}
		//if (e.which == 98 || e.which == 50) {
		//	textBoxBcOut.innerHTML += "2";
		//	return;
		//}
		//if (e.which == 99 || e.which == 51) {
		//	textBoxBcOut.innerHTML += "3";
		//	return;
		//}
		//if (e.which == 100 || e.which == 52) {
		//	textBoxBcOut.innerHTML += "4";
		//	return;
		//}
		//if (e.which == 101 || e.which == 53) {
		//	textBoxBcOut.innerHTML += "5";
		//	return;
		//}
		//if (e.which == 102 || e.which == 54) {
		//	textBoxBcOut.innerHTML += "6";
		//	return;
		//}
		//if (e.which == 103 || e.which == 55) {
		//	textBoxBcOut.innerHTML += "7";
		//	return;
		//}
		//if (e.which == 104 || e.which == 56) {
		//	textBoxBcOut.innerHTML += "8";
		//	return;
		//}
		//if (e.which == 105 || e.which == 57) {
		//	textBoxBcOut.innerHTML += "9";
		//	return;
		//}
		// if (e.which == 10 || e.which == 13) {
		// 	if ((textBoxBcOut.innerHTML != null && textBoxBcOut.innerHTML.length > 0) ||
		// 		(textBoxBcOut.value != null && textBoxBcOut.value.length > 0)) {
		// 		console.log(`Enter pressed; text=${textBoxBcOut.innerHTML}`);
		// 	}
		// 	buttonEnter = document.getElementById("buttonEnter");
		// 	if (buttonEnter != null) {
		// 		buttonEnter.click();
		// 		return;
		// 	}
		// }

		//if (e.which >= 19 && e.which < 48)
		// 	captureKey(e.which);

		//if (e.which >= 58 && e.which < 96)
		// 	captureKey(e.which);

		//if (e.which >= 106 && e.which < 128)
		//	captureKey(e.which);
	// };
}


function captureKey(keyWhich) {
	alert("Key pressed: " + parseInt(keyWhich));

}

function setSelectionRange(input, selectionStart, selectionEnd) {
	if (input.setSelectionRange) {
		input.focus();
		input.setSelectionRange(selectionStart, selectionEnd);
	}
	else if (input.createTextRange) {
		var range = input.createTextRange();
		range.collapse(true);
		range.moveEnd('character', selectionEnd);
		range.moveStart('character', selectionStart);
		range.select();
	}
}

function setCaretToPos(input, pos) {
	setSelectionRange(input, pos, pos);
}

// https://stackoverflow.com/questions/34968174/set-text-cursor-position-in-a-textarea
// @brief: set cursor inside _input_ at position (column,row)
// @input: input DOM element. E.g. a textarea
// @content: textual content inside the DOM element
// @param row: starts a 0
// @param column: starts at 0    
function setTextCursor(input, content, row, column) {
	// search row times: 
	var pos = 0;
	var prevPos = 0;
	for (var i = 0; (i < row) && (pos != -1); ++i) {
		prevPos = pos;
		pos = content.indexOf("\n", pos + 1);
	}


	// if we can't go as much down as we want,
	//  go as far as worked
	if (-1 == pos) { pos = prevPos; }

	if (0 != row)
		++pos; // one for the linebreak

	// prevent cursor from going beyond the current line
	var lineEndPos = content.indexOf("\n", pos + 1);

	if ((-1 != lineEndPos) &&
		(column > lineEndPos - pos)) {
		// go *only* to the end of the current line
		pos = lineEndPos;
	} else {
		// act as usual
		pos += column
	}

	setSelectionRange(input, pos, pos);
}
