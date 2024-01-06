/*
	2023-11-05 froga.js by Heinrich Elsigan
*/
var currentFrog;
var currentFrogId;
var currentFrogName;
var fX;
var fY;
var loopTicks = 0;
var loopDelay = 1200;
var frogsDied = 0;
var frogsInWhole = 0;
var level = 0;
var gameOver = 0;
var imgSavedWoodB;
var imgSavedWoodT;

function frogInit() {
	
	frogsDied = 0;
	loopTicks = 0;
	loopDelay = 1200;
	frogsInWhole = 0;
	gameOver = 0;
	document.getElementById("headerImg").src = "img/header.png";
	window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
		if (e.which == 37) {
			frogMove("left");
		}
		if (e.which == 39) {
			frogMove("right");
		}
		if (e.which == 38) {
			frogMove("up");
		}
		if (e.which == 40) {
			frogMove("down");
		}
	};

	setTimeout(function () { frogaLooper(loopTicks, loopDelay) }, loopDelay); // will call the function after 16 secs.
}


function restart() {
	if (gameOver == 1) { 
		recreateFrogs();
		frogInit();
	}
	
}

function frogaLooper(ticks, delay) {

	currentFrog = getActiveFrog();	
	if (frogsInWhole >= 3) {
		headerImg.src = "img/message_levelcompleted.png"
		headerImg.width = 318;
		headerImg.height = 31;
		level++;
		alert("Level " + level + " completed");
		setTimeout(function () { window.location.reload(); }, loopDelay); // will call the function after 16 secs.
		return; 
	}
	if (currentFrog == null) {	
		headerImg.src = "img/message_gameover.png";
		headerImg.width = 318;
		headerImg.height = 31;
		gameOver = 1;

		return;
	}
	currentFrogId = getCurrentFrogId(currentFrog);

	try {
		moveCars();
		moveWalkers();
		moveWoods();
	} catch (exMove) {
		alert(exMove)
	}

	loopTicks = ticks + 1;

	setTimeout(function () { frogaLooper(loopTicks, delay) }, delay); // will call the function after 16 secs.
}


function moveCars() {
	var carId = "car00";
	var car = document.getElementById(carId);
	var oldTd = car.getAttribute("alt");
	var car_Y = rowByTag(car);
	var car_X = rrighter(columnByTag(car));
	var newTd = "td" + car_Y + car_X;

	var frX = columnByTag(currentFrog);
	var frY = parseInt(rowByTag(currentFrog));
	var frogNr = parseInt(currentFrogId.charAt(4));
	var frogTd = "td" + frY + frX;

	car.alt = newTd;
	car.setAttribute("cellid", newTd);	
	car.src = "img/car0.png";

	if (newTd == frogTd) {
		currentFrog.id = "died" + frogNr;
		document.getElementById(newTd).removeChild(currentFrog);
		car.src = "img/car0crashed.png"
		currentFrog = getActiveFrog();
		currentFrogId = getCurrentFrogId(currentFrog);
	}

	document.getElementById(oldTd).removeChild(car);
	document.getElementById(newTd).appendChild(car);	

	carId = "car01";
	car = document.getElementById(carId);
	oldTd = car.getAttribute("alt");
	car_Y = rowByTag(car);
	car_X = rrighter(columnByTag(car));
	newTd = "td" + car_Y + car_X;

	car.alt = newTd;
	car.setAttribute("cellid", newTd);
	car.src = "img/car0.png";

	if (newTd == frogTd) {
		currentFrog.id = "died" + frogNr;
		document.getElementById(newTd).removeChild(currentFrog);
		car.src = "img/car0crashed.png"
		currentFrog = getActiveFrog();
		currentFrogId = getCurrentFrogId(currentFrog);
	}

	document.getElementById(oldTd).removeChild(car);
	document.getElementById(newTd).appendChild(car);
		
	let carCnt = 10;
	for (carCnt = 10; carCnt < 13; carCnt++) {
		carId = "car" + carCnt;

		car = document.getElementById(carId);
		oldTd = car.getAttribute("alt");
		car_Y = rowByTag(car);
		car_X = llefter(columnByTag(car));
		newTd = "td" + car_Y + car_X;

		car.alt = newTd;
		car.setAttribute("cellid", newTd);
		car.src = "img/car1.png";

		if (newTd == frogTd) {
			currentFrog.id = "died" + frogNr;			
			document.getElementById(newTd).removeChild(currentFrog);
			car.src = "img/car1crashed.png"
			currentFrog = getActiveFrog();
			currentFrogId = getCurrentFrogId(currentFrog);
		}

		document.getElementById(oldTd).removeChild(car);
		document.getElementById(newTd).appendChild(car);		
	}
}

function moveWalkers() {
	var walk; 
	var oldTd;
	var walk_Y;
	var walk_X;
	var newTd;
	var walkId;
	let walkCnt = 0;

	var frX = columnByTag(currentFrog);
	var frY = parseInt(rowByTag(currentFrog));
	var frogNr = parseInt(currentFrogId.charAt(4));
	var frogTd = "td" + frY + frX;

	for (walkCnt = 0; walkCnt < 4; walkCnt++) {

		walkId = "person" + walkCnt;
		walk = document.getElementById(walkId);
		oldTd = walk.getAttribute("alt");
		walk_Y = rowByTag(walk);
		walk_X = rrighter(columnByTag(walk));
		newTd = "td" + walk_Y + walk_X;

		walk.alt = newTd;
		walk.setAttribute("cellid", newTd);
		switch (walkId) {
			case "person0": walk.src = "img/walk4m.png"; break;
			case "person1": walk.src = "img/walk5m.png"; break;
			case "person2": walk.src = "img/walk6m.png"; break;
			case "person3": walk.src = "img/walk3m.png"; break;
			default: break;
		}	

		if (newTd == frogTd) {
			currentFrog.id = "died" + frogNr;
			document.getElementById(newTd).removeChild(currentFrog);
			walk.src = "img/walk7m.png"
			currentFrog = getActiveFrog();
			currentFrogId = getCurrentFrogId(currentFrog);
		}

		document.getElementById(oldTd).removeChild(walk);
		document.getElementById(newTd).appendChild(walk);		
	}
}

function moveWoods() {
	var woodId;
	var wood;
	var oldTd;
	var oldTdCell;
	var wood_Y;
	var wood_X;
	var newTd;
	var newWood;
	let checkFrog = 0;
	let woodCnt = 0;

	var newFrog = document.getElementById(currentFrogId);
	var frX = columnByTag(newFrog);
	var frY = parseInt(rowByTag(newFrog));
	if (frY == 6 || frY == 7) {
		checkFrog = 1;
	}

	let frogCnt = 0;
	for (frogCnt = 0; frogCnt < 3; frogCnt++) {
		var deadFrogId = "died" + frogCnt;
		var deadFrog = document.getElementById(deadFrogId);
		if (deadFrog != null) {
			var parentElem = deadFrog.parentElement;
			var parentNode = deadFrog.parentNode;
			if (parentElem != null)
				parentElem.removeChild(deadFrog);
		}
	}

	for (woodCnt = 3; woodCnt >= 0; woodCnt--) {
		woodId = "woodB" + woodCnt;
		wood = document.getElementById(woodId);
		if (wood == null && checkFrog == 1 && newFrog.getAttribute("idwood") == woodId) { 
			wood = newFrog;
		}		
		if (wood != null) {
			oldTd = wood.getAttribute("alt");
			wood_Y = rowByTag(wood);
			wood_X = rrighter(columnByTag(wood));
			newTd = "td" + wood_Y + wood_X;			

			newWood = copyImg(wood);
			newWood.alt = newTd;
			newWood.setAttribute("cellid", newTd);

			if (imgSavedWoodB != null && imgSavedWoodB.id != null && imgSavedWoodB.id == woodId) {
				imgSavedWoodB.alt = newTd;
				imgSavedWoodB.setAttribute("cellid", newTd);
			}

			document.getElementById(newTd).appendChild(newWood);
			document.getElementById(oldTd).removeChild(wood);			
		}
	}

	woodCnt = 0;
	for (woodCnt = 0; woodCnt < 4; woodCnt++) {
		woodId = "woodT" + woodCnt;
		wood = document.getElementById(woodId);
		if (wood == null && checkFrog == 1 && newFrog.getAttribute("idwood") == woodId) { 
			wood = newFrog;
		}
		if (wood != null) {
			oldTd = wood.getAttribute("alt");
			wood_Y = rowByTag(wood);
			wood_X = llefter(columnByTag(wood));
			newTd = "td" + wood_Y + wood_X;

			newWood = copyImg(wood);
			newWood.alt = newTd;
			newWood.setAttribute("cellid", newTd);

			if (imgSavedWoodT != null && imgSavedWoodT.id != null && imgSavedWoodT.id == woodId) {
				imgSavedWoodT.alt = newTd;
				imgSavedWoodT.setAttribute("cellid", newTd);
			}

			try {
				document.getElementById(oldTd).removeChild(wood);
			} catch (ex3) {
				alert("Id: " + woodId + " oldTd: " + oldTd + " newTd: " + newTd + " " + ex3);
			}			

			newWood.alt = newTd;
			newWood.setAttribute("cellid", newTd);
			document.getElementById(newTd).appendChild(newWood);			
		}
	}

}

function frogMove(jumpDir) {

	currentFrog = getActiveFrog();
	currentFrogId = getCurrentFrogId(currentFrog);
	var frogNr = parseInt(currentFrogId.charAt(4));	// TODO		better implementation of frog number

	var additionalRemove;
	var imgDisApear = null;
	var imgReApear = null;

	var frogDied = -1;
	var currentInWhole = 0;

	// var rowColTag = currentFrog.getAttribute("alt");

	var frX = columnByTag(currentFrog);
	var frY = parseInt(rowByTag(currentFrog));
	var oldTd = "td" + frY + frX;
	var oldFrog = document.getElementById(oldTd).children[currentFrogId];

	var nrX = fX;
	var nrY = parseInt(fY);

	if (jumpDir == null || jumpDir.length < 2)
		return;

	if (jumpDir.charAt(0) == 'u') {
		nrY = upper(frY);										// up 	
		document.getElementById("aUp").src = "img/a_up.gif";
	} else if (jumpDir.charAt(0) == 'd') {
		nrY = below(frY);										// down 		
		document.getElementById("aDown").src = "img/a_down.gif";
	}
	if (jumpDir.charAt(0) == 'r' || jumpDir.charAt(1) == 'r') {
		nrX = righter(frX);										// right
		document.getElementById("aRight").src = "img/a_right.gif";
	} else if (jumpDir.charAt(0) == 'l' || jumpDir.charAt(1) == 'l') {
		document.getElementById("aLeft").src = "img/a_left.gif";
		nrX = lefter(frX);										// left
	}

	var newTd = "td" + nrY + nrX;
	var newFrog = document.getElementById(currentFrogId);

	newFrog.id = currentFrogId;
	newFrog.title = "ActiveFrog";
	newFrog.border = "0";
	newFrog.src = "img/frogactive.png";
	newFrog.setAttribute("idwood", "");

	if (nrY == 1) 
		newFrog.src = "img/street3b.png";
	if (nrY == 2)
		newFrog.src = "img/street2t.png";
	if (nrY == 3)
		newFrog.src = "img/meadow2m.png";
	if (nrY == 4)
		newFrog.src = "img/walk2m.png";
	if (nrY == 5)
		newFrog.src = "img/meadow2t.png";

	let woodIt = 0;
	if (frY == 6 && (nrY == 5 || nrY == 6 || nrY == 7)) {
		if (imgSavedWoodB != null) {
			imgReApear = imgSavedWoodB;
		}
	}


	if (frY == 7 && (nrY == 6 || nrY == 7 || nrY == 8)) {
		// imgReApear = null;
		if (imgSavedWoodT != null) {
			imgReApear = imgSavedWoodT;
		}
	}


	if (nrY == 6 && (frY == 5 || frY == 6 || frY == 7)) {
		woodIt = 0;
		imgDisApear = null;
		while (imgDisApear == null && woodIt < 4) {
			var woodId = "woodB" + woodIt;
			imgDisApear = document.getElementById(newTd).children[woodId];
			if (imgDisApear != null) {
				if (frY == 6)
					imgReApear = imgSavedWoodB;
				imgSavedWoodB = copyImg(imgDisApear);
				newFrog.src = "img/wood1b.png#" + woodIt;
				newFrog.setAttribute("idwood", woodId);
				woodIt = 4;
				break;
			}
			else
				imgDisApear = null;

			woodIt++;
		}

		if (imgDisApear == null) {
			newFrog.src = "img/wood4b.gif";
			newFrog.title = "FrogDied";
			newFrog.id = "died" + frogNr;
			frogDied = frogNr;
		}
	}

	if (nrY == 7 && (frY == 6 || frY == 7 || frY == 8)) {
		woodIt = 0;
		imgDisApear = null;
		while (imgDisApear == null && woodIt < 4) {
			var woodId = "woodT" + woodIt;
			imgDisApear = document.getElementById(newTd).children[woodId];
			if (imgDisApear != null) {	
				if (frY == 7)
					imgReApear = imgSavedWoodT;
				imgSavedWoodT = copyImg(imgDisApear);
				newFrog.src = "img/wood1t.png#" + woodIt;
				newFrog.setAttribute("idwood", woodId);
				woodIt = 4;
				break;
			}
			else
				imgDisApear = null;

			woodIt++;
		}

		if (imgDisApear == null) {		
			newFrog.src = "img/wood4t.gif";
			newFrog.title = "FrogDied";
			newFrog.id = "died" + frogNr;
			// TODO: add sound
			frogDied = frogNr;
		}
	}

	if (nrY == 8)
		newFrog.src = "img/meadow2t.png";

	if (nrY == 9) {
		woodIt = 0;
		imgDisApear = null;
		while (imgDisApear == null && woodIt < 3) {

			imgDisApear = document.getElementById(newTd).children["whole" + woodIt];
			if (imgDisApear != null) {
				woodIt = 4;
				break;
			}
			else
				imgDisApear = null;

			woodIt++;
		}
		if (imgDisApear == null) {
			if (nrX.charAt(0) == 'c')
				imgDisApear = document.getElementById("whole0");
			if (nrX.charAt(0) == 'e')
				imgDisApear = document.getElementById("whole1");
			if (nrX.charAt(0) == 'g')
				imgDisApear = document.getElementById("whole2");
		}

		if (imgDisApear != null) {
			currentInWhole++;
			setFrogsInWhole(++frogsInWhole);
			let savedNr = parseInt(newFrog.id.charAt(4));
			newFrog.src = "img/frogend1.png";
			newFrog.id = "save" + frogNr;
			newFrog.title = "FrogInWhole";
		}
	}

	oldTdCell = document.getElementById(oldTd);
	if (oldTdCell != null) {
		// oldFrog = oldTdCell.getElementById(currentFrogId);
		oldFrog = document.getElementById(oldTd).children[currentFrogId];
	}
	if (oldFrog == null)
		oldFrog = document.getElementById(currentFrogId);

	newFrog.alt = newTd;
	newFrog.setAttribute("cellid", newTd);

	if (imgDisApear != null) {
		document.getElementById(newTd).removeChild(imgDisApear);
	}

	if (frogDied < 0)
		frogDied = crashFrog(newTd);
	else
		newFrog.id = "died" + frogNr;

	if (frogDied > -1) {
		frogDied = frogNr;		
		setFrogsDied(++frogsDied);
		currentFrog = getActiveFrog();
		currentFrogId = getCurrentFrogId();
	} else {
		document.getElementById(newTd).appendChild(newFrog);
	}
	

	if (imgReApear != null) {
		document.getElementById(oldTd).appendChild(imgReApear);
	}

	if (frY == 7 && (nrY <= 5 || nrY >= 8))
		imgSavedWoodB = null;
	if (frY == 8 && (nrY <= 6 || nrY >= 9))	
		imgSavedWoodT = null;


}

function crashFrog(tdFrogCell) {


	var moveId, frogNr;
	var move;
	var moveTd, tdFrog;
	var move_Y, move_X;

	currentFrog = getActiveFrog();
	currentFrogId = getCurrentFrogId(currentFrog);
	tdFrog = currentFrog.getAttribute("alt");
	frogNr = parseInt(currentFrogId.charAt(4));

	let crashCnt = 0;
	let moveCnt = 0;
	let _move_Id = "";
	var movers = ["car00", "car01", "car10", "car11", "car12", "car13", "person0", "person1", "person2", "person3"];

	movers.forEach(function (_move_Id) {

		moveId = _move_Id;
		move = document.getElementById(moveId);
		if (move != null) {
			moveTd = move.getAttribute("alt");
			move_Y = rowByTag(move);
			move_X = rrighter(columnByTag(move));
			if (moveTd == tdFrogCell) {				
				try {
					var parentCell = document.getElementById(currentFrogId).parentElement;
					if (parentCell != null) {
						currentFrog.id = "died" + frogNr;
						parentCell.removeChild(currentFrog);
					}
					// document.getElementById(tdFrogCell).removeChild(currentFrog);
				} catch (exFrogCrash1) {
					// alert(exFrogCrash1);
				}
				if (_move_Id.length >= 4) {
					switch (_move_Id.substr(0, 4)) {
						case "car0": move.src = "img/car0crashed.png"; ++crashCnt; break;
						case "car1": move.src = "img/car1crashed.png"; ++crashCnt; break;
						case "pers": move.src = "img/walk7m.png"; ++crashCnt; break;
						default: break;
					}
				}
			}
		}
	});
	return (crashCnt > 0) ? frogNr : -1;
}

function getActiveFrog() {

	let aFrogIt = 0;
	var aFrogId = "frog0";
	currentFrog = null;

	for (aFrogIt = 0; aFrogIt < 4; aFrogIt++) {

		aFrogId = "frog" + aFrogIt;							// TODO: define frog prefix "frog" as constant
		currentFrog = document.getElementById(aFrogId);

		if (currentFrog != null) {
			if (currentFrog.title != "ActiveFrog")
				currentFrog.title = "ActiveFrog";
			// currentFrog.src = "img/frogactive.png";
			fY = rowByTag(currentFrog);
			fX = columnByTag(currentFrog)
			return currentFrog;
		}
	}
	
	return currentFrog;
}

function getCurrentFrogId(aFrog) {
	var aFrog = getActiveFrog();
	let frogsLeftNr = 4 - parseInt(aFrog.id.charAt(4)); 
	setFrogsLeft(frogsLeftNr);
	return aFrog.id;
}


function rowByTag(aVehicle) {
	var altTag = aVehicle.getAttribute("alt");
	if (altTag != null) //  && altTag.length >= 2) 
		return parseInt(altTag.charAt(2));
	// fY = 0;
	return -1;
}

function columnByTag(aVehicle) {
	var altTag = aVehicle.getAttribute("alt");
	if (altTag != null) // && altTag.length >= 2)
		return altTag.charAt(3);
	// fX 
	return 'd';
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


function recreateFrogs() {
	var imgF0 = new Image(36, 27);
	imgF0.id = "frog0";
	imgF0.src = "img/frogactive.png";
	imgF0.alt = "td0d";
	imgF0.setAttribute("border", 0);	
	imgF0.setAttribute("title", "ActiveFrog");
	imgF0.setAttribute("class", "frogaIMG");
	imgF0.setAttribute("className", "frogaIMG");
	imgF0.setAttribute("cellid", "td0d");
	imgF0.setAttribute("idwood", "");
	document.getElementById("td0d").appendChild(imgF0);

	var imgF1 = new Image(36, 27);
	imgF1.id = "frog1";
	imgF1.src = "img/froga.png";
	imgF1.alt = "td0e";
	imgF1.setAttribute("border", 0);
	imgF1.setAttribute("title", "ActiveFrog");
	imgF1.setAttribute("class", "frogaIMG");
	imgF1.setAttribute("className", "frogaIMG");
	imgF1.setAttribute("cellid", "td0e");
	imgF1.setAttribute("idwood", "");
	document.getElementById("td0e").appendChild(imgF1);

	var imgF2 = new Image(36, 27);
	imgF2.id = "frog2";
	imgF2.src = "img/froga.png";
	imgF2.alt = "td0f";
	imgF2.setAttribute("border", 0);
	imgF2.setAttribute("title", "ActiveFrog");
	imgF2.setAttribute("class", "frogaIMG");
	imgF2.setAttribute("className", "frogaIMG");
	imgF2.setAttribute("cellid", "td0f");
	imgF2.setAttribute("idwood", "");
	document.getElementById("td0f").appendChild(imgF2);

	var imgF3 = new Image(36, 27);
	imgF3.id = "frog2";
	imgF3.src = "img/froga.png";
	imgF3.alt = "td0g";
	imgF3.setAttribute("border", 0);
	imgF3.setAttribute("title", "ActiveFrog");
	imgF3.setAttribute("class", "frogaIMG");
	imgF3.setAttribute("className", "frogaIMG");
	imgF3.setAttribute("cellid", "td0g");
	imgF3.setAttribute("idwood", "");
	document.getElementById("td0g").appendChild(imgF3);
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

function upper(row) {
	let rowUp = parseInt(row);
	if (rowUp < 9)	// TODO: add constant here for different froga boards
		rowUp++;
	return rowUp;
}

function below(row) {
	let rowBelow = parseInt(row);
	if (rowBelow > 0)
		rowBelow--;
	return rowBelow;
}

function righter(col) { 
	// TODO: add constant here for different froga boards
	switch (col.charAt(0)) {
		case 'a': return 'b';
		case 'b': return 'c';
		case 'c': return 'd';
		case 'd': return 'e';
		case 'e': return 'f';
		case 'f': return 'g';
		case 'g': return 'h';
		case 'h': return 'i';
		case 'i': return 'j';
		case 'j': return 'j';
		default: break;
	}
	return (col.charAt(0));
}

function rrighter(col) {
	if (col.charAt(0) == 'j')
		return 'a';
	var rightMov = righter(col);
	return rightMov;	
}

function llefter(col) {
	if (col.charAt(0) == 'a')
		return 'j';
	var leftMov = lefter(col);
	return leftMov;
}

function lefter(col) {
	// TODO: add constant here for different froga boa
	switch (col.charAt(0)) {
		case 'a': return 'a';
		case 'b': return 'a';
		case 'c': return 'b';
		case 'd': return 'c';
		case 'e': return 'd';
		case 'f': return 'e';
		case 'g': return 'f';
		case 'h': return 'g';
		case 'i': return 'h';
		case 'j': return 'i';
		default: break;
	}
	return (col.charAt(0));
}

function setFrogsInWhole(inWhole) {
	document.getElementById("frogsInWhole").innerHTML = inWhole;
}

function setFrogsLeft(frogsLeft) {
	document.getElementById("frogsLeft").innerHTML = frogsLeft;
}


function setFrogsDied(frogsDied) {
	document.getElementById("frogsDied").innerHTML = frogsDied;
}