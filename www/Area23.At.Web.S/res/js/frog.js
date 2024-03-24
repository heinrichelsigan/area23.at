/*
    2023-11-05 froga.js © by Heinrich Elsigan
    2024-01-08 updated
    https://darkstar.work/froga/
    https://area23.at/froga/froga.html
*/

var loopDelay = 1600,
    loopTicks = 0,
    soundDuration = 1625;
var level = 0,
    frogsDied = 0,
    frogsInHole = 0,
    frogHoleMax = 3,
    isGameOver = 0;
var fX, fY;
var currentFrog, currentFrogId;
var imgSavedWoodB, imgSavedWoodT;
var car20, car21, car22, car30, car31, car32;

// windows cursor key press hanlder
function windowCursorKeysHandler() {
    window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
        if (e.which == 37)
            moveFrog("left");
        if (e.which == 39)
            moveFrog("right");
        if (e.which == 38)
            moveFrog("up");
        if (e.which == 40)
            moveFrog("down");
    };
}


// function Level Completed
function levelCompleted() {
    if (frogsInHole >= frogHoleMax) {
        if (frogsDied > 0)
            headerImg.src = "res/img/levelcompleted.gif"
        else
            headerImg.src = "res/img/levelperfect.gif"
        headerImg.height = 36;
        level++;
        soundDuration = 3600;
        setTimeout(function () { frogSound("res/audio/levelCompleted.mp3") }, 100);
        setTimeout(function () { frogReStart(false); }, 4000); // will call the function after 8 secs.

        return true;
    }
    return false;
}

// function Game Over
function gameOver() {
    if (currentFrog == null || frogsDied > 3) {
        headerImg.src = "res/img/gameover.png";
        headerImg.height = 36;
        isGameOver = 1;
        soundDuration = 4800;
        setTimeout(function () { frogSound("res/audio/frogaGameOver.mp3") }, 100);
        setTimeout(function () { frogReStart(true); }, 5000); // will call the function after 8 secs.

        return true;
    }
    return false;
}

// frogInit will be called on 1st time loading
function frogInit() {
    windowCursorKeysHandler();
    frogLoad();
}

// frogReStart(repeatLevel) => repeatLevel = true
function frogReStart(repeatLevel) {
    if (repeatLevel) {
        isGameOver = 0;
        window.location.reload();
    } else { // TODO: fix this		
        reCreateFrogs();
        frogLoad();
    }
}

// frog loader
function frogLoad() {
    loopTicks = 0;
    frogsDied = 0;
    frogsInHole = 0;
    isGameOver = 0;

    switch (level) {
        case 0: loopDelay = 1600; break;
        case 1: loopDelay = 1500; break;
        case 2: loopDelay = 1400; break;
        case 3: loopDelay = 1300; break;
        case 4: loopDelay = 1200; break;
        case 5: loopDelay = 1100; break;
        default: loopDelay = 1000; break;
    }
    setLevel(level);
    setFrogsInHole(frogsInHole);
    setFrogsDied(frogsDied);

    fX = 'd';
    fY = 0;
    imgSavedWoodB = null;
    imgSavedWoodT = null;

    var headerImg = document.getElementById("headerImg");
    if (headerImg != null) {
        document.getElementById("headerImg").src = "res/img/header.png";
        document.getElementById("headerImg").focus();
        document.getElementById("headerImg").blur();
    }
    var frogZero = document.getElementById("frog0");
    if (frogZero != null)
        frogZero.focus();

    setTimeout(function () { frogaLooper(loopTicks, loopDelay) }, loopDelay); // will call function after loopDelay milli seconds.
}

// main js looper => keeping game in movement
function frogaLooper(ticks, delay) {

    let leftNotes = document.getElementById("leftNotes");
    let rightNotes = document.getElementById("rightNotes");
    if (leftNotes.innerHTML.length > 1)
        leftNotes.innerHTML = "";
    if (rightNotes.innerHTML.length > 1)
        rightNotes.innerHTML = "";

    currentFrog = getActiveFrog();

    // level completed
    if (levelCompleted())
        return;
    if (frogsInHole >= frogHoleMax) {
        headerImg.src = (frogsDied > 0) ? "res/img/levelcompleted.gif" :
            "res/img/levelperfect.gif";
        headerImg.height = 36;
        level++;
        soundDuration = 3600;
        setTimeout(function () { frogSound("res/audio/levelCompleted.mp3") }, 100);
        setTimeout(function () { frogReStart(false); }, 4000); // will call the function after 8 secs.
        return;
    }
    // game over
    if (gameOver()) {
        return; 
    }    
    if (currentFrog == null || frogsDied > 3) {
        headerImg.src = "res/img/gameover.png";
        headerImg.height = 36;
        isGameOver = 1;
        soundDuration = 4800;
        setTimeout(function () { frogSound("res/audio/frogaGameOver.mp3") }, 100);
        setTimeout(function () { frogReStart(true); }, 5000); // will call the function after 8 secs.
        return;
    }

    currentFrogId = getCurrentFrogId(currentFrog);

    try {
        moveCars();
        moveWalkers();
        moveWoods();
    } catch (exMove) {
        // alert(exMove)
    }

    loopTicks = ticks + 1;

    setTimeout(function () { frogaLooper(loopTicks, delay) }, delay); // will call the function after 16 secs.
}


function carLooper(carMargin, ticks, delay) {

    if (carMargin <= -16 || ticks != loopTicks)
        return;

    car30.style.marginLeft = carMargin + "px";
    car30.style.marginRight = -carMargin + "px";
    car31.style.marginLeft = carMargin + "px";
    car30.style.marginRight = -carMargin + "px";
    car32.style.marginLeft = carMargin + "px";

    car20.style.marginLeft = -carMargin + "px";
    car21.style.marginLeft = -carMargin + "px";
    car22.style.marginLeft = -carMargin + "px";

}


// move cars in froga game
function moveCars() {
    var car, carId = "car2";
    var oldTd, newTd;
    var frX = columnByTag(currentFrog);
    var frY = parseInt(rowByTag(currentFrog));
    var frogNr = parseInt(currentFrogId.charAt(4));
    var frogTd = "td" + frY + frX;

    carId = "car3";
    let carCnt = 0;
    for (carCnt = 0; carCnt <= 2; carCnt++) {
        carId = "car3" + carCnt;

        car = document.getElementById(carId);
        oldTd = car.getAttribute("cellid");

        newTd = getNewTdPositionByMoving(car, 'll');

        car.setAttribute("cellid", newTd);
        car.src = "res/img/car3.gif";
        car.style.marginLeft = "16px";

        if (newTd == frogTd) {
            currentFrog.id = "died" + frogNr;
            document.getElementById(newTd).removeChild(currentFrog);

            car.src = "res/img/car3crashed.png";
            car.style.marginLeft = "0px";
            changeImagePlaySound(car, "res/img/car3crashed.png", "res/audio/frogCrash.ogg");

            // currentFrog & currentFrogId will be fetched in setFrogsDied
            setFrogsDied(++frogsDied);
            // currentFrog = getActiveFrog();
            // currentFrogId = getCurrentFrogId(currentFrog);
        }

        document.getElementById(oldTd).removeChild(car);
        document.getElementById(newTd).appendChild(car);
    }

    car30 = document.getElementById("car30");
    car31 = document.getElementById("car31");
    car32 = document.getElementById("car32");

    carCnt = 0;
    for (carCnt = 0; carCnt < 2; carCnt++) {
        carId = "car2" + carCnt;

        car = document.getElementById(carId);
        oldTd = car.getAttribute("cellid");

        newTd = getNewTdPositionByMoving(car, 'rr');

        car.setAttribute("cellid", newTd);
        car.src = "res/img/car2.gif";
        car.style.marginLeft = "-16px";

        if (newTd == frogTd) {
            currentFrog.id = "died" + frogNr;
            document.getElementById(newTd).removeChild(currentFrog);

            car.src = "res/img/car2crashed.png";

            changeImagePlaySound(car, "res/img/car2crashed.png", "res/audio/frogCrash.ogg");

            // currentFrog & currentFrogId will be fetched in setFrogsDied
            setFrogsDied(++frogsDied);
            // currentFrog = getActiveFrog();
            // currentFrogId = getCurrentFrogId(currentFrog);
        }

        document.getElementById(oldTd).removeChild(car);
        document.getElementById(newTd).appendChild(car);
    }

    car20 = document.getElementById("car20");
    car20.style.marginLeft = "-16px";
    car21 = document.getElementById("car21");
    car21.style.marginLeft = "-16px";
    car22 = document.getElementById("car22");
    car22.style.marginLeft = "-16px";

    let carDelay = 100;
    let carTicks = loopTicks;
    setTimeout(function () { carLooper(12, carTicks, carDelay) }, 100);
    setTimeout(function () { carLooper(8, carTicks, carDelay) }, 200);
    setTimeout(function () { carLooper(4, carTicks, carDelay) }, 300);
    setTimeout(function () { carLooper(0, carTicks, carDelay) }, 400);
    setTimeout(function () { carLooper(-4, carTicks, carDelay) }, 500);
    setTimeout(function () { carLooper(-8, carTicks, carDelay) }, 600);
    setTimeout(function () { carLooper(-12, carTicks, carDelay) }, 800);
    setTimeout(function () { carLooper(-16, carTicks, carDelay) }, 1000);

}

// move wakjers
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
        oldTd = walk.getAttribute("cellid");
        newTd = getNewTdPositionByMoving(walk, 'rr');

        walk.setAttribute("cellid", newTd);

        switch (walkId) {
            case "person0": walk.src = "res/img/walk2m.gif"; break;
            case "person1": walk.src = "res/img/walk5m.gif"; break;
            case "person2": walk.src = "res/img/walk6m.gif"; break;
            case "person3": walk.src = "res/img/walk3m.gif"; break;
            default: break;
        }

        if (newTd == frogTd) {
            currentFrog.id = "died" + frogNr;
            document.getElementById(newTd).removeChild(currentFrog);

            walk.src = "res/img/walk0m.gif"
            changeImagePlaySound(walk, "res/img/walk0m.gif", "res/audio/frogJump.ogg");

            // currentFrog & currentFrogId will be fetched in setFrogsDied
            setFrogsDied(++frogsDied);
            // currentFrog = getActiveFrog();
            // currentFrogId = getCurrentFrogId(currentFrog);
        }

        document.getElementById(oldTd).removeChild(walk);
        document.getElementById(newTd).appendChild(walk);
    }
}

// move woods on water
function moveWoods() {
    var woodId;
    var wood;
    var oldTd;
    var oldTdCell;
    var wood_Y;
    var wood_X;
    var newTd;
    var newWood;
    let woodCnt = 0;

    var newFrog = document.getElementById(currentFrogId);
    var frX = columnByTag(newFrog);
    var frY = parseInt(rowByTag(newFrog));
    let checkFrog = (frY == 6 || frY == 7);

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
        if (wood == null && checkFrog && newFrog.getAttribute("idwood") == woodId) {
            wood = newFrog;
        }
        if (wood != null) {
            oldTd = wood.getAttribute("cellid");
            newTd = getNewTdPositionByMoving(wood, 'rr');

            newWood = copyImg(wood);
            newWood.setAttribute("cellid", newTd);

            if (imgSavedWoodB != null && imgSavedWoodB.id != null && imgSavedWoodB.id == woodId)
                imgSavedWoodB.setAttribute("cellid", newTd);

            document.getElementById(newTd).appendChild(newWood);
            document.getElementById(oldTd).removeChild(wood);
        }
    }

    woodCnt = 0;
    for (woodCnt = 0; woodCnt < 4; woodCnt++) {
        woodId = "woodT" + woodCnt;
        wood = document.getElementById(woodId);
        if (wood == null && checkFrog && newFrog.getAttribute("idwood") == woodId) {
            wood = newFrog;
        }
        if (wood != null) {
            oldTd = wood.getAttribute("cellid");
            newTd = getNewTdPositionByMoving(wood, 'll');

            newWood = copyImg(wood);
            newWood.setAttribute("cellid", newTd);

            if (imgSavedWoodT != null && imgSavedWoodT.id != null && imgSavedWoodT.id == woodId)
                imgSavedWoodT.setAttribute("cellid", newTd);

            try {
                wood.parentElement.removeChild(wood);
            } catch (ex3) {
                // alert("wood.parentElement.id=" + wood.parentElemen.id + "wood.id=" + wood.id + "\r\nException: " + x3);
            }

            newWood.setAttribute("cellid", newTd);
            document.getElementById(newTd).appendChild(newWood);
        }
    }

}

// move frog => frog jumping handler
function moveFrog(jumpDirection) {

    currentFrog = getActiveFrog();
    currentFrogId = getCurrentFrogId(currentFrog);
    var frogNr = parseInt(currentFrogId.charAt(4));	// TODO		better implementation of frog number

    var imgDisApear = null;
    var imgReApear = null;

    var frogDied = -1;
    var frogCrashed = -1;
    var frogDoubleHole = 0;

    var frX = columnByTag(currentFrog);
    var frY = parseInt(rowByTag(currentFrog));
    var oldTd = "td" + frY + frX;

    var nrX = fX;
    var nrY = parseInt(fY);

    if (jumpDirection == null || jumpDirection.length < 2)
        return;

    if (jumpDirection.charAt(0) == 'u') {
        nrY = upper(frY);												// up 	
        document.getElementById("aUp").src = "res/img/a_up.gif";
    }
    else if (jumpDirection.charAt(0) == 'd') { 							// TODO should we let frog drive back to start meadow
        nrY = below(frY);												// down 				
        document.getElementById("aDown").src = "res/img/a_down.gif";
    }

    if (jumpDirection.charAt(0) == 'r' || jumpDirection.charAt(1) == 'r') {
        nrX = righter(frX);												// right
        document.getElementById("aRight").src = "res/img/a_right.gif";
    } else if (jumpDirection.charAt(0) == 'l' || jumpDirection.charAt(1) == 'l') {
        nrX = lefter(frX);												// left
        document.getElementById("aLeft").src = "res/img/a_left.gif";
    }

    // TODO: better use newTd = getNewTdPositionByMoving(car, 'rr');
    var newTd = "td" + nrY + nrX;
    var newFrog = document.getElementById(currentFrogId);

    newFrog.id = currentFrogId;
    newFrog.title = "ActiveFrog";
    newFrog.border = "0";
    newFrog.src = "res/img/frogJumpLand.gif";
    newFrog.setAttribute("idwood", "");

    var shouldReturn = false;

    if (nrY == 1 || nrY == 4) {
        var startObjects = ["meadow0b0", "frog0", "frog1", "frog2", "frog3", "meadow0b1", "meadow0m0"];
        let _startObj_Id = "";
        var startObj = null;
        startObjects.forEach(function (_startObj_Id) {
            startObj = document.getElementById(newTd).children[_startObj_Id];
            if (startObj != null) {
                shouldReturn = true;
            }
        });
    }

    if (shouldReturn)
        return;

    frogCrashed = crashFrog(newTd);

    // saved bottom wood image will be restored
    let woodIt = 0;
    if (frY == 6 && (nrY == 5 || nrY == 6 || nrY == 7)) {
        if (imgSavedWoodB != null)
            imgReApear = imgSavedWoodB;
    }

    // saved top wood image will be restored
    if (frY == 7 && (nrY == 6 || nrY == 7 || nrY == 8)) {
        if (imgSavedWoodT != null)
            imgReApear = imgSavedWoodT;
    }

    // set frog on bottom wood
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
                newFrog.src = "res/img/frogOnWoodB.gif#" + woodIt;
                newFrog.setAttribute("idwood", woodId);
                woodIt = 4;
                break;
            }
            woodIt++;
        }
        // frog dies in river
        if (imgDisApear == null) {
            frogDied = frogInRiverOrSwampOrHole(newFrog, "res/img/frogDiesInWaterB.gif", "res/audio/frogUnderWater.ogg", "died", "Frog died!");
        }
    }

    // set frog on top wood
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
                newFrog.src = "res/img/frogOnWoodT.gif#" + woodIt;
                newFrog.setAttribute("idwood", woodId);
                woodIt = 4;
                break;
            }
            woodIt++;
        }
        // frog dies in river
        if (imgDisApear == null) {
            frogDied = frogInRiverOrSwampOrHole(newFrog, "res/img/frogDiesInWaterT.gif", "res/audio/frogUnderWater.ogg", "died", "Frog died!");
        }
    }

    if (nrY == 8) {
        woodIt = 0;
        imgDisApear = null;
        while (imgDisApear == null && woodIt < frogHoleMax) {
            imgDisApear = document.getElementById(newTd).children["hole" + woodIt];
            if (imgDisApear == null)
                imgDisApear = document.getElementById(newTd).children["save" + woodIt];
            if (imgDisApear != null && imgDisApear.src != null) {
                let idaLen = imgDisApear.src.length;
                if ((imgDisApear.src.substr(idaLen - 27) == "res/img/frogTwiceInHole.gif") ||
                    (imgDisApear.src.substr(idaLen - 22) == "res/img/frogInHole.gif")) {
                    frogDoubleHole++;
                    woodIt = 4; break;
                }
                if (imgDisApear.src.substr(idaLen - 20) == "res/img/frogHole.png") {
                    woodIt = 4;
                    break;
                }
            }
            woodIt++;
        }

        if (imgDisApear == null) {
            frogDied = frogInRiverOrSwampOrHole(newFrog, "res/img/frogDiesInSwamp.gif", "res/audio/frogInSwamp.ogg", "died", "Frog died!");
        }
        else if (imgDisApear != null) {
            if (frogDoubleHole >= 1) {
                newTd = "td" + nrY + lefter(nrX);
                frogDied = frogInRiverOrSwampOrHole(newFrog, "res/img/frogDiesInSwamp.gif", "res/audio/frogInSwamp.ogg", "died", "frog" + frogNr + "@graveyard");
                imgDisApear = null;
            }
            else if (frogDoubleHole < 1) {
                frogsInHole++;
                setFrogsInHole(frogsInHole);
                frogInRiverOrSwampOrHole(newFrog, "res/img/frogInHole.gif", "res/audio/frogInHole.ogg", "save", "frog" + frogNr + "@home");
            }
        }
    }

    newFrog.setAttribute("cellid", newTd);

    // if (imgDisApear != null) 
    //    document.getElementById(newTd).removeChild(imgDisApear);

    if (frogsInHole >= 3 || (frogCrashed) < 0) {
        if (imgDisApear != null) {
            replaceImg(imgDisApear, newFrog);
            // document.getElementById(newFrog.id).parentElement.removeChild(newFrog);
        }
        else {
            document.getElementById(newTd).appendChild(newFrog);
        }
    } else {
        frogDied = frogCrashed;
        if (imgDisApear != null)
            document.getElementById(newTd).removeChild(imgDisApear);
    }

    if (frogDied > -1)
        setFrogsDied(++frogsDied);

    if (imgReApear != null) {
        document.getElementById(oldTd).appendChild(imgReApear);
    }

    if (frY == 7 && (nrY <= 5 || nrY >= 8))
        imgSavedWoodB = null;
    if (frY == 8 && (nrY <= 6 || nrY >= 8))
        imgSavedWoodT = null;
}


// sound and image 
function frogSound(soundName) {
    var dursec = 1625;
    dursec = parseInt(soundDuration);
    if (dursec < parseInt(loopDelay))
        dursec = parseInt(loopDelay);

    let sound = new Audio(soundName);

    sound.autoplay = true;
    sound.loop = false;

    let leftNotes = document.getElementById("leftNotes");
    let rightNotes = document.getElementById("rightNotes");
    leftNotes.innerHTML = "";
    rightNotes.innerHTML = "";

    setTimeout(function () {
        sound.play();
        leftNotes.innerHTML = "♪ ";
        rightNotes.innerHTML = " ♫";
    }, 100);

    setTimeout(function () {
        leftNotes.innerHTML = " ♪";
        rightNotes.innerHTML = "♫ ";
    }, 800);

    setTimeout(function () {
        leftNotes.innerHTML = "  ";
        rightNotes.innerHTML = "  ";
        sound.loop = false;
        sound.pause();
        sound.autoplay = false;
        sound.currentTime = 0;
        try {
            sound.src = "";
            sound = null;
        } catch (exSnd) {
        }
        soundDuration = parseInt(loopDelay);
    }, dursec);
}


// exchange image & play sound
function changeImagePlaySound(imageToChange, newImageUrl, soundToPlay) {
    if (imageToChange != null)
        imageToChange.src = newImageUrl;

    if (soundToPlay != null && soundToPlay.length > 1)
        setTimeout(function () { frogSound(soundToPlay) }, 100);
}


// frog in river or swamp
function frogInRiverOrSwampOrHole(aFrog, deathImg, deathSound, idPrefix, deathTitle) {
    var frogNr = parseInt(aFrog.id.charAt(4));
    aFrog.src = deathImg;
    aFrog.title = (deathTitle == null) ? "" : deathTitle;
    aFrog.id = idPrefix + frogNr;

    if (deathSound != null && deathSound.length > 1)
        setTimeout(function () { frogSound(deathSound) }, 100);

    return frogNr;
}

// crash frog
function crashFrog(tdFrogCell) {

    var moveId, frogNr;
    var move;
    var moveTd, tdFrog;

    currentFrog = getActiveFrog();
    currentFrogId = getCurrentFrogId(currentFrog);
    tdFrog = currentFrog.getAttribute("cellid");
    frogNr = parseInt(currentFrogId.charAt(4));

    let crashCnt = 0;
    let moveCnt = 0;
    let _move_Id = "";

    var movers = ["car20", "car21", , "car22", "car30", "car31", "car32", "car33", "person0", "person1", "person2", "person3"];

    movers.forEach(function (_move_Id) {

        moveId = _move_Id;
        move = document.getElementById(moveId);
        if (move != null) {
            moveTd = move.getAttribute("cellid");
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
                        case "car2": ++crashCnt;
                            changeImagePlaySound(move, "res/img/car2crashed.png", "res/audio/frogCrash.ogg");
                            move.src = "res/img/car2crashed.png";
                            break;
                        case "car3": ++crashCnt;
                            changeImagePlaySound(move, "res/img/car3crashed.png", "res/audio/frogCrash.ogg");
                            move.src = "res/img/car3crashed.png";
                            break;
                        case "pers": ++crashCnt;
                            changeImagePlaySound(move, "res/img/walk0m.gif", "res/audio/frogJump.ogg");
                            move.src = "res/img/walk0m.gif";
                            break;
                        default: break;
                    }
                }
            }
        }
    });

    return (crashCnt > 0) ? frogNr : -1;
}


// get active current frog
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
            // currentFrog.src = "res/img/frogActive.gif";
            fY = rowByTag(currentFrog);
            fX = columnByTag(currentFrog)
            return currentFrog;
        }
    }

    return currentFrog;
}

// get current frog id
function getCurrentFrogId(aFrog) {
    aFrog = getActiveFrog();
    let frogsLeftNr = 0;
    if (aFrog != null && aFrog.id != null && aFrog.id.length >= 4)
        frogsLeftNr = 4 - parseInt(aFrog.id.charAt(4));
    setFrogsLeft(frogsLeftNr);
    return (aFrog == null || aFrog.id == null) ? -1 : aFrog.id;
}



function reCreateFrogs() {
    // first clear all bottom and top table cells, so that there rest neither frogs nor holes there
    var tdsToClear = [
        // "td1b", "td1c", "td1d", "td1e", "td1f", "td1g", "td1h", "td1i",
        "td8a", "td8b", "td8c", "td8d", "td8e", "td8f", "td8g", "td8h", "td8i"];
    // "td9a", "td9b", "td9c", "td9d", "td9e", "td9f", "td9g", "td9h", "td9i"];
    tdsToClear.forEach(function (tdId) {
        tableCell = document.getElementById(tdId);
        if (tableCell != null && tableCell.children != null && tableCell.children.length > 0) {
            for (tdCellIt = 0; tdCellIt < tableCell.children.length; tdCellIt++) {
                childFromTableCell = tableCell.children[tdCellIt];
                if (childFromTableCell != null)
                    tableCell.removeChild(childFromTableCell);
            }
        }
    });
    var frogsToClear = ["frog0", "frog1", "frog2", "frog3"];
    frogsToClear.forEach(function (frogId) {
        var frogMatch = document.getElementById(frogId);
        if (frogMatch != null && frogMatch.parentElement != null)
            frogMatch.parentElement.removeChild(frogMatch);
    });

    // recreate frog images dynamically
    reCreateNewFrogImage(0);
    reCreateNewFrogImage(1);
    reCreateNewFrogImage(2);
    reCreateNewFrogImage(3);

    // recreate holes images for frog goal dynamically
    reCreateFrogHole(0);
    reCreateFrogHole(1);
    reCreateFrogHole(2);
    reCreateFrogHole(3);

    frogHoleMax = 4;
}


function reCreateNewFrogImage(frogNr) {
    var frogImg = new Image(36, 27);
    var frogTitle = "";
    var frogSrc = "res/img/frogSleepy.png";
    var frogCellTd = "td1d";
    switch (parseInt(frogNr)) {
        case 0:
            frogTitle = "ActiveFrog";
            frogSrc = "res/img/frogActive.gif"
            frogCellTd = "td1d";
            break;
        case 1:
            frogCellTd = "td1e"; break;
        case 2:
            frogCellTd = "td1f"; break;
        case 3:
            frogCellTd = "td1g"; break;
        default:
            break;
    }

    frogImg.id = "frog" + parseInt(frogNr);
    frogImg.alt = "FROG " + parseInt(frogNr);
    frogImg.src = frogSrc
    frogImg.setAttribute("border", 0);
    frogImg.setAttribute("title", frogTitle);
    frogImg.setAttribute("class", "frogaImage");
    frogImg.setAttribute("className", "frogaImage");
    frogImg.setAttribute("cellid", frogCellTd);
    frogImg.setAttribute("idwood", "");

    if (frogCellTd != null && frogImg != null) {
        var cellTd = document.getElementById(frogCellTd);
        if (cellTd != null)
            cellTd.appendChild(frogImg);
    }
}

function reCreateFrogHole(frogNr) {
    var holeImg = new Image(36, 27);
    holeImg.id = "hole" + parseInt(frogNr);
    holeImg.alt = "HOLE " + parseInt(frogNr);
    holeImg.src = "res/img/frogHole.png";
    holeImg.setAttribute("border", 0);
    holeImg.setAttribute("title", "");
    holeImg.setAttribute("class", "frogaImage");
    holeImg.setAttribute("className", "frogaImage");

    var holeCellTd = "td8e";
    switch (parseInt(frogNr)) {
        case 0:
            holeCellTd = "td8c"; break;
        case 1:
            holeCellTd = "td8e"; break;
        case 2:
            holeCellTd = "td8g"; break;
        case 3:
            holeCellTd = "td8i"; break;
        default:
            break;
    }

    if (holeCellTd != null) {
        holeImg.setAttribute("cellid", holeCellTd);
        var cellTd = document.getElementById(holeCellTd);
        if (cellTd != null) {
            cellTd.appendChild(holeImg);
            if (holeCellTd == "td8i") {
                try {
                    cellTd.setAttribute("background", "res/img/frogHole.png");
                } catch (Exception) { }
            }
        }
    }
}


function setFrogsInHole(inHole) {
    var spanInHole = document.getElementById("frogsInHole");
    if (spanInHole != null)
        spanInHole.innerText = inHole;
    // currentFrog = getActiveFrog();
    // currentFrogId = getCurrentFrogId(currentFrog);
}

function setFrogsLeft(frogsLeft) {
    document.getElementById("frogsLeft").innerHTML = frogsLeft;
}

function setFrogsDied(frogsDied) {
    document.getElementById("frogsDied").innerHTML = frogsDied;
    currentFrog = getActiveFrog();
    currentFrogId = getCurrentFrogId(currentFrog);
}

function setLevel(frogLevel) {
    document.getElementById("frogaLevel").innerHTML = frogLevel;
}


function getNewTdPositionByMoving(elemImage, direction) {
    var elem_Y = rowByTag(elemImage);
    var elem_X = columnByTag(elemImage);

    if (direction != null && direction.charAt(0) != '\0') {
        switch (direction.charAt(0)) {
            case 'r': elem_X = rrighter(elem_X); break;
            case 'l': elem_X = llefter(elem_X); break;
            case 'u': elem_Y = upper(elem_Y); break;
            case 'd': elem_Y = below(elem_Y); break;
            default: break;
        }
    }
    return "td" + elem_Y + elem_X;
}

function rowByTag(aVehicle) {
    var cellidTag = aVehicle.getAttribute("cellid");
    if (cellidTag != null) //  && cellidTag.length >= 2) 
        return parseInt(cellidTag.charAt(2));
    // fY = 0;
    return -1;
}

function columnByTag(aVehicle) {
    var cellidtag = aVehicle.getAttribute("cellid");
    if (cellidtag != null) // && cellidtag.length >= 2)
        return cellidtag.charAt(3);
    // fX 
    return 'd';
}


function upper(row) {
    let rowUp = parseInt(row);
    if (rowUp < 8)	// TODO: add constant here for different froga boards
        rowUp++;
    return rowUp;
}

function below(row) {
    let rowBelow = parseInt(row);
    if (rowBelow > 1)
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

// replaces imgOrig with imgFrom, copies all attributes and removes imgFrom from document
function replaceImg(imgOrig, imgFrom) {
    if (imgOrig != null && imgOrig.id != null && imgOrig.src != null &&
        imgFrom != null && imgFrom.id != null && imgFrom.src != null) {
        var tmpId = imgFrom.id;
        imgFrom.id = tmpId + "_1";
        imgOrig.id = tmpId;
        imgOrig.src = imgFrom.src;
        imgOrig.width = imgFrom.width;
        imgOrig.height = imgFrom.height;
        imgOrig.alt = imgFrom.alt;
        if (imgFrom.getAttribute("title") != null)
            imgOrig.setAttribute("title", imgFrom.getAttribute("title"));
        if (imgFrom.getAttribute("className") != null)
            imgOrig.setAttribute("className", imgFrom.getAttribute("className"));
        imgOrig.setAttribute("class", imgFrom.getAttribute("class"));
        // we leave cellid from imgOrig unchanged
        // if (imgFrom.getAttribute("cellid") != null)
        //     imgOrig.setAttribute("cellid", imgFrom.getAttribute("cellid"));
        if (imgFrom.getAttribute("idwood") != null)
            imgOrig.setAttribute("idwood", imgFrom.getAttribute("idwood"));
        imgOrig.setAttribute("border", 0);

        if (document.getElementById(imgFrom.id) != null && document.getElementById(imgFrom.id).parentElement != null) {
            try {
                document.getElementById(imgFrom.id).parentElement.removeChild(imgFrom);
            } catch (e) {
                alert(e);
            }
        }
    }
}       