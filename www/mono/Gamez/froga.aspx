<%@ Page Title="froga" Language="C#" MasterPageFile="~/Gamez/GamesMaster.master" AutoEventWireup="true" CodeBehind="froga.aspx.cs" Inherits="Area23.At.Mono.Gamez.froga" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>frogga</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<style>
        body.frogbody {
            color: white;
            background-color: black;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;
        }

        div.froga {
            width: 86%;
            min-width: 387pt;
            height: 84%;
            min-height: 378pt;
            line-height: normal;
            vertical-align: middle;
            color: white;
            background-color: black;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;
        }

            div.froga.img {
                width: 8%;
                min-width: 36pt;
                height: 6%;
                min-height: 27pt;
                padding: 0 0 0 0;
                margin: 0 0 0 0;
                background-repeat: no-repeat;
                background-size: 100% 100%;
            }

            div.froga.frogaHeader {
                width: 90%;
                min-width: 360pt;
                height: 10%;
                min-height: 45pt;
                align-content: center;
                font-size: medium;
                line-height: normal;
                border-color: white;
                color: white;
                background-color: black;
                background-repeat: no-repeat;
            }

        div.frogaHeader#headerLeft {
            height: 10%;
            min-height: 45pt;
            width: 27%;
            min-width: 90pt;
            vertical-align: middle;
            font-size: larger;
            text-align: right;
        }

        div.frogaHeader#headerCenter {
            height: 10%;
            min-height: 45pt;
            width: 64%;
            min-width: 297pt;
            vertical-align: middle;
            font-size: medium;
            text-align: center;
        }

        div.frogaHeader.headerImage {
            width: 60%;
            min-width: 270pt;
            height: 10%;
            min-height: 45pt;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            background-repeat: no-repeat;
            background-size: 100% 100%;
        }

        div.frogaHeader#headerRight {
            height: 10%;
            min-height: 45pt;
            width: 19.11%;
            min-width: 86pt;
            vertical-align: middle;
            font-size: larger;
            text-align: left;
        }


        img.frogsInImg {
            width: 32pt;
            height: 27pt;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;
            background-repeat: no-repeat;
            background-size: 100% 100%;
        }

        span#leftNotes {
            color: whitesmoke;
            width: 5.33%;
            max-width: 24pt;
            font-size: large;
        }

        span#frogsInHole {
            color: greenyellow;
            width: 32pt;
            margin-left: -18pt;
            padding-right: 6pt;
            font-weight: bold;
        }

        span#frogsLeft {
            color: aqua;
            width: 32pt;
            margin-left: -18pt;
            padding-right: 2pt;
            font-size: large;
            /*visibility: hidden; */
        }

        span#frogsDied {
            color: gainsboro;
            width: 32pt;
            font-size: medium;
            font-weight: bold;
            margin-left: -18pt;
            padding-right: 4pt;
        }
           
        span#frogsLevel {
            color: orangered;
            font-size: larger;
        }

        span#frogaLevel {
            color: greenyellow;
            font-size: larger;
        }

        span#leftNotes,
        span#rightNotes {
            color: whitesmoke;
            width: 5.33%;
            max-width: 24pt;
            font-size: large;
        }

        div.froga.frogaFooter {
            width: 80%;
            min-width: 360pt;
            height: 8%;
            min-height: 36pt;
            text-align: center;
            font-size: medium;
            background-color: black;
            align-content: center;
            table-layout: fixed;
            inset-block-start: initial;
            font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', 'Geneva', 'Verdana', 'sans-serif';
        }

            div.froga.frogaFooter#spanLeft {
                height: 36pt;
                min-height: 36pt;
                width: 40pt;
                min-width: 40pt;
                vertical-align: middle;
                text-align: left;
            }

            div.froga.frogaFooter#spanRight {
                height: 36pt;
                min-height: 36pt;
                width: 40pt;
                min-width: 40pt;
                vertical-align: middle;
                text-align: right
            }


        table.frogaTable {
            width: 80%;
            min-width: 360pt;
            height: 60%;
            min-height: 270pt;
            align-content: center;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;
            background-repeat: no-repeat;
        }

        tr.frogaTr {
            width: 80%;
            min-width: 360px;
            height: 6%;
            min-height: 27px;
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

        tr.frogaTrUpStreet {
            width: 80%;
            min-width: 360px;
            height: 6%;
            min-height: 27px;            
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

        td.frogaTd {
            width: 8%;
            /* min-width: 36pt; */
            height: 6%;
            min-height: 27pt;           
            /* background-repeat: no-repeat; */
            background-size: 100% 100%;
        }

        img.frogaImage,
        img.frogaWood {
            width: 100%;
            min-width: 36pt;
            height: 100%;
            min-height: 27pt;
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

            img.frogaImage#frog0,
            img.frogaImage#frog1,
            img.frogaImage#frog2,
            img.frogaImage#frog3 {
                width: 100%;
                min-width: 36pt;
                height: 100%;
                min-height: 27pt;
                background-repeat: no-repeat;
                /* background-size: 100% 100%; */
            }


        img#aUp,
        img#aDown,
        img#aLeft,
        img#aRight {
            width: 32pt;
            min-width: 32pt;
            height: 27pt;
            min-height: 27pt;
            background-repeat: no-repeat;
            background-size: 100% 100%;
        }

	</style>
	<script type="text/javascript">

        var loopDelay = 1625,
            loopTicks = 0,
            soundDuration = 1625;
        var level = 0,
            frogsDied = 0,
            frogsInHole = 0,
            frogHoleMax = 3,
            gameOver = 0;
        var fX, fY;
        var currentFrog, currentFrogId;
        var imgSavedWoodB, imgSavedWoodT;
        window.onload = function () {
            frogInit();
        };

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

        // frogInit will be called on 1st time loading
        function frogInit() {
            windowCursorKeysHandler();
            frogLoad();
        }

        // frogReStart(repeatLevel) => repeatLevel = true
        function frogReStart(repeatLevel) {
            if (repeatLevel) {
                gameOver = 0;
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
            gameOver = 0;

            switch (level) {
                case 0: loopDelay = 1625; break;
                case 1: loopDelay = 1500; break;
                case 2: loopDelay = 1375; break;
                case 3: loopDelay = 1250; break;
                case 4: loopDelay = 1125; break;
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
                document.getElementById("headerImg").src = "../res/gamez/froga/header.png";
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
            if (frogsInHole >= frogHoleMax) {
                if (frogsDied > 0)
                    headerImg.src = "../res/gamez/froga/levelcompleted.gif"
                else
                    headerImg.src = "../res/gamez/froga/levelperfect.gif"
                headerImg.height = 36;
                level++;
                soundDuration = 3600;
                setTimeout(function () { frogSound("../res/gamez/audio/levelCompleted.mp3") }, 100);
                setTimeout(function () { frogReStart(false); }, 4000); // will call the function after 8 secs.
                return;
            }
            // game over
            if (currentFrog == null || frogsDied > 3) {
                headerImg.src = "../res/gamez/froga/gameover.png";
                headerImg.height = 36;
                gameOver = 1;
                soundDuration = 4800;
                setTimeout(function () { frogSound("../res/gamez/audio/frogaGameOver.mp3") }, 100);
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
                car.src = "../res/gamez/froga/car3.gif";

                if (newTd == frogTd) {
                    currentFrog.id = "died" + frogNr;
                    document.getElementById(newTd).removeChild(currentFrog);

                    car.src = "../res/gamez/froga/car3crashed.png"
                    changeImagePlaySound(car, "../res/gamez/froga/car3crashed.png", "../res/gamez/audio/frogCrash.ogg");

                    // currentFrog & currentFrogId will be fetched in setFrogsDied
                    setFrogsDied(++frogsDied);
                    // currentFrog = getActiveFrog();
                    // currentFrogId = getCurrentFrogId(currentFrog)
                }

                document.getElementById(oldTd).removeChild(car);
                document.getElementById(newTd).appendChild(car);
            }

            carCnt = 0;
            for (carCnt = 0; carCnt < 2; carCnt++) {
                carId = "car2" + carCnt;

                car = document.getElementById(carId);
                oldTd = car.getAttribute("cellid");

                newTd = getNewTdPositionByMoving(car, 'rr');

                car.setAttribute("cellid", newTd);
                car.src = "../res/gamez/froga/car2.gif";

                if (newTd == frogTd) {
                    currentFrog.id = "died" + frogNr;
                    document.getElementById(newTd).removeChild(currentFrog);

                    car.src = "../res/gamez/froga/car3crashed.png"
                    changeImagePlaySound(car, "../res/gamez/froga/car2crashed.png", "../res/gamez/audio/frogCrash.ogg");

                    // currentFrog & currentFrogId will be fetched in setFrogsDied
                    setFrogsDied(++frogsDied);
                    // currentFrog = getActiveFrog();
                    // currentFrogId = getCurrentFrogId(currentFrog);
                }

                document.getElementById(oldTd).removeChild(car);
                document.getElementById(newTd).appendChild(car);
            }
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
                    case "person0": walk.src = "../res/gamez/froga/walk2m.gif"; break;
                    case "person1": walk.src = "../res/gamez/froga/walk5m.gif"; break;
                    case "person2": walk.src = "../res/gamez/froga/walk6m.gif"; break;
                    case "person3": walk.src = "../res/gamez/froga/walk3m.gif"; break;
                    default: break;
                }

                if (newTd == frogTd) {
                    currentFrog.id = "died" + frogNr;
                    document.getElementById(newTd).removeChild(currentFrog);

                    walk.src = "../res/gamez/froga/walk0m.gif"
                    changeImagePlaySound(walk, "../res/gamez/froga/walk0m.gif", "../res/gamez/audio/frogJump.ogg");

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
            let checkFrog = (frY == 5 || frY == 6);

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
                document.getElementById("aUp").src = "../res/gamez/froga/arrow/a_up.gif";
            }
            else if (jumpDirection.charAt(0) == 'd') { 							// TODO should we let frog drive back to start meadow
                nrY = below(frY);												// down 				
                document.getElementById("aDown").src = "../res/gamez/froga/arrow/a_down.gif";
            }

            if (jumpDirection.charAt(0) == 'r' || jumpDirection.charAt(1) == 'r') {
                nrX = righter(frX);												// right
                document.getElementById("aRight").src = "../res/gamez/froga/arrow/a_right.gif";
            } else if (jumpDirection.charAt(0) == 'l' || jumpDirection.charAt(1) == 'l') {
                nrX = lefter(frX);												// left
                document.getElementById("aLeft").src = "../res/gamez/froga/arrow/a_left.gif";
            }

            // TODO: better use newTd = getNewTdPositionByMoving(car, 'rr');
            var newTd = "td" + nrY + nrX;
            var newFrog = document.getElementById(currentFrogId);

            newFrog.id = currentFrogId;
            newFrog.title = "ActiveFrog";
            newFrog.border = "0";
            newFrog.src = "data:image/gif;base64,R0lGODlhJAAbAMZhAGBmAF5lJ3d5AHyBAGuvAG2xAG+xAG6yAG+yAHCyAHKyAHCzAHKzAHG0AHK0AHC0CHezAHK1AHO1AHO2AHS2AHW2AHO3AHW3AnyvR3W3BXa2DnS4AHO5AHO6AHe5AHS6AHe5AXi5AY2zAHS7AHW7AHW8AHe9AHe+AHi+AHy8C3y8Dni/AH28FH69C36+AHnAAKi0Aa+yAH3CAKm1AILAEoLAF37CFoPAHoDEAITBF37DF4TBGYS9TaizRIPEBITDEoTECYbCH4TEEoTEG4bEGojDIYjDI4jDJIfEHobFH4nELovAWa7BAI3HNY7HN5DJPJLJQK7IAJTLQZTNRpbNRpfNRpnJbpvQTJvRSLbOE5zSSZ3SSr3OEqfXWsrWAKraX6zcYv///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////yH+IkNyZWF0ZWQgIGJ5IGhlQGFyZWEyMy5hdCB3aXRoIEdJTVAAIfkECRIAfwAsAAABACQAGgAAB9SAf4KDhIWGh4iJiouMikpgf2BKjZSEV12XV5WbT00VTU+blDNeC6ZeM6KMAKWnAKqFWn+ygjEKpgsKMYOytI1bfy9/QoMMDbgNDIPEwsCMKH84LwSDuNaDBC/C0IzZ2oTWuITa05Tk5CPhpiPn35QE7OjqCyMl6NSwfwTz+PmF++r6+cPGb+C/gvmMKDQiCGA4fAsZMooY8c+8PxQXLsqoEMg8IBwlKgo5IdyEkJQ4ugjnguOmjD7C+cioKiMFXBRowqK40lTLiv4WurBo6s9PkYgCAQAh+QQJEgB/ACwAAAAAJAAbAAAH2oB/goOEhYaHiImHSmB/YEqKkYpXXZRXkpiGT00VTU+ZoH8zXgulXjOhmACkpgCphlp/sYIxCqULCjGDsbOYW38vf0KDDA23DQyDw8G/kih/OC8Eg7fVgwQvwc+S2NmE1beE2dKZ4+Mj4KUj5t6ZBOvn6QsjJefTr4IE8vf4hvrp/PoR+gcuoMB8+w4WIljNYCgjEI1QSzcookRJFi3+kfcnY8RIHiECkQck5EVFJieAm2AyU0gX4FyEfJjRBzgfHl95pHCLQk58GWGWkqlRYEQXG0v9IXpSocBAACH5BAkSAH8ALAAAAAAkABsAAAemgH+Cg4SFfzxWf1Y8ho2Oj39LVpJLkJaQPDwYmZedhT09GKKgnqUBoaMBpZ6oohg9q4aVlYKutqKDs52VGH+9tbeug729tI/EuMDBv764zI62hMvJys/Q09i3ndnc1LHYsZDg4dfL5OXB543j4Znu1dqH7oyY8/PY9vOO+ffT/O+G/mXCJ3CfwGUC6Rnkh/DfpX8N83mCaMvhqny3JJ5zh4wYwEeBAAA7";
            // newFrog.src = "../res/gamez/froga/frogActive.gif";
            newFrog.setAttribute("idwood", "");

            var shouldReturn = false;
            if (nrY == 1) {
                var startObjects = ["meadow0b0", "frog0", "frog1", "frog2", "frog3", "meadow0b1"];
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
            if (frY == 5 && (nrY == 4 || nrY == 5 || nrY == 6)) {
                if (imgSavedWoodB != null)
                    imgReApear = imgSavedWoodB;
            }

            // saved top wood image will be restored
            if (frY == 6 && (nrY == 5 || nrY == 6 || nrY == 7)) {
                if (imgSavedWoodT != null)
                    imgReApear = imgSavedWoodT;
            }

            // set frog on bottom wood
            if (nrY == 5 && (frY == 4 || frY == 5 || frY == 6)) {
                woodIt = 0;
                imgDisApear = null;
                while (imgDisApear == null && woodIt < 4) {
                    var woodId = "woodB" + woodIt;
                    imgDisApear = document.getElementById(newTd).children[woodId];
                    if (imgDisApear != null) {
                        if (frY == 5)
                            imgReApear = imgSavedWoodB;
                        imgSavedWoodB = copyImg(imgDisApear);
                        newFrog.src = "../res/gamez/froga/frogOnWoodB.gif#" + woodIt;
                        newFrog.setAttribute("idwood", woodId);
                        woodIt = 4;
                        break;
                    }
                    woodIt++;
                }
                // frog dies in river
                if (imgDisApear == null) {
                    frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/gamez/froga/frogDiesInWaterB.gif", "../res/gamez/audio/frogUnderWater.ogg", "died", "Frog died!");
                }
            }

            // set frog on top wood
            if (nrY == 6 & (frY == 5 || frY == 6 || frY == 7)) {
                woodIt = 0;
                imgDisApear = null;
                while (imgDisApear == null && woodIt < 4) {
                    var woodId = "woodT" + woodIt;
                    imgDisApear = document.getElementById(newTd).children[woodId];
                    if (imgDisApear != null) {
                        if (frY == 6)
                            imgReApear = imgSavedWoodT;
                        imgSavedWoodT = copyImg(imgDisApear);
                        newFrog.src = "../res/gamez/froga/frogOnWoodT.gif#" + woodIt;
                        newFrog.setAttribute("idwood", woodId);
                        woodIt = 4;
                        break;
                    }
                    woodIt++;
                }
                // frog dies in river
                if (imgDisApear == null) {
                    frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/gamez/froga/frogDiesInWaterT.gif", "../res/gamez/audio/frogUnderWater.ogg", "died", "Frog died!");
                }
            }

            if (nrY == 7) {
                woodIt = 0;
                imgDisApear = null;
                while (imgDisApear == null && woodIt < frogHoleMax) {
                    imgDisApear = document.getElementById(newTd).children["hole" + woodIt];
                    if (imgDisApear == null)
                        imgDisApear = document.getElementById(newTd).children["save" + woodIt];
                    if (imgDisApear != null && imgDisApear.src != null) {
                        let idaLen = imgDisApear.src.length;
                        if ((imgDisApear.src.substr(idaLen - 27) == "../res/gamez/froga/frogTwiceInHole.gif") ||
                            (imgDisApear.src.substr(idaLen - 22) == "../res/gamez/froga/frogInHole.gif")) {
                            frogDoubleHole++;
                            woodIt = 4; break;
                        }
                        if (imgDisApear.src.substr(idaLen - 20) == "../res/gamez/froga/frogHole.png") {
                            woodIt = 4;
                            break;
                        }
                    }
                    woodIt++;
                }

                if (imgDisApear == null) {
                    frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/gamez/froga/frogDiesInSwamp.gif", "../res/gamez/audio/frogInSwamp.ogg", "died", "Frog died!");
                }
                else if (imgDisApear != null) {
                    if (frogDoubleHole >= 1) {
                        newTd = "td" + nrY + lefter(nrX);
                        frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/gamez/froga/frogDiesInSwamp.gif", "../res/gamez/audio/frogInSwamp.ogg", "died", "frog" + frogNr + "@graveyard");
                        imgDisApear = null;
                    }
                    else if (frogDoubleHole < 1) {
                        frogsInHole++;
                        setFrogsInHole(frogsInHole);
                        frogInRiverOrSwampOrHole(newFrog, "../res/gamez/froga/frogInHole.gif", "../res/gamez/audio/frogInHole.ogg", "save", "frog" + frogNr + "@home");
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

            if (frY == 6 && (nrY <= 4 || nrY >= 7))
                imgSavedWoodB = null;
            if (frY == 7 && (nrY <= 5 || nrY >= 7))
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
            var move_Y, move_X;

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
                            let moveIdent = _move_Id.substr(0, 4);
                            switch (moveIdent) {
                                case "car2": ++crashCnt;
                                    changeImagePlaySound(move, "../res/gamez/froga/car2crashed.png", "../res/gamez/audio/frogCrash.ogg");
                                    move.src = "../res/gamez/froga/car2crashed.png";
                                    break;
                                case "car3": ++crashCnt;
                                    changeImagePlaySound(move, "../res/gamez/froga/car3crashed.png", "../res/gamez/audio/frogCrash.ogg");
                                    move.src = "../res/gamez/froga/car3crashed.png";
                                    break;
                                case "pers": ++crashCnt;
                                    changeImagePlaySound(move, "../res/gamez/froga/walk0m.gif", "../res/gamez/audio/frogJump.ogg");
                                    move.src = "../res/gamez/froga/walk0m.gif";
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
                    // currentFrog.src = "../res/gamez/froga/frogActive.gif";
                    fY = rowByTag(currentFrog);
                    fX = columnByTag(currentFrog)
                    return currentFrog;
                }
            }

            return currentFrog;
        }

        // get current frog id
        function getCurrentFrogId(aFrog) {
            var aFrog = getActiveFrog();
            let frogsLeftNr = 4 - parseInt(aFrog.id.charAt(4));
            setFrogsLeft(frogsLeftNr);
            return aFrog.id;
        }


        function reCreateFrogs() {
            // first clear all bottom and top table cells, so that there rest neither frogs nor holes there
            var tdsToClear = [
                "td7a", "td7b", "td7c", "td7d", "td7e", "td7f", "td7g", "td7h", "td7i"];
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
            var frogSrc = "../res/gamez/froga/frogSleepy.png";
            var frogCellTd = "td1d";
            switch (parseInt(frogNr)) {
                case 0:
                    frogTitle = "ActiveFrog";
                    frogSrc = "../res/gamez/froga/frogActive.gif"
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
            holeImg.src = "../res/gamez/froga/frogHole.png";
            holeImg.setAttribute("border", 0);
            holeImg.setAttribute("title", "");
            holeImg.setAttribute("class", "frogaImage");
            holeImg.setAttribute("className", "frogaImage");

            var holeCellTd = "td7e";
            switch (parseInt(frogNr)) {
                case 0:
                    holeCellTd = "td7c"; break;
                case 1:
                    holeCellTd = "td7e"; break;
                case 2:
                    holeCellTd = "td7g"; break;
                case 3:
                    holeCellTd = "td7i"; break;
                default:
                    break;
            }

            if (holeCellTd != null) {
                holeImg.setAttribute("cellid", holeCellTd);
                var cellTd = document.getElementById(holeCellTd);
                if (cellTd != null) {
                    cellTd.appendChild(holeImg);
                    if (holeCellTd == "td7i") {
                        try {
                            cellTd.setAttribute("background", "../res/gamez/froga/frogHole.png");
                        } catch (Exception) { }
                    }
                }
            }
        }

        function setFrogsInHole(inHole) {
            var spanInHole = document.getElementById("frogsInHole");
            if (spanInHole != null)
                spanInHole.innerText = inHole;
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
            if (rowUp < 7)	// TODO: add constant here for different froga boards
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
                //    imgOrig.setAttribute("cellid", imgFrom.getAttribute("cellid"));
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

    </script>
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">     
    <noscript>Your browser does not support JavaScript!</noscript>
    <div class="froga" align="center">
	    <div class="frogaHeader">
            <span id="headerLeft" align="right" valign="middle">
                <span id="leftNotes"></span>
                <img class="frogsInImg" src="../res/gamez/froga/frogsLeftCount.png" /><span id="frogsLeft" alt="frogs left">4</span>
                <img class="frogsInImg" src="../res/gamez/froga/frogsDiedCount.png" /><span id="frogsDied" alt="frogs died">0</span>
                <img class="frogsInImg" src="../res/gamez/froga/frogsInHoleCount.png" /><span id="frogsInHole" alt="frogs in hole">0</span>
            </span>
		    <span id="headerCenter" align="center" valign="middle">
			    <img class="headerImage" src="../res/gamez/froga/header.png" id="headerImg" border="0" onclick="restart()" />
		    </span>
		    <span id="headerRight" align="left" valign="middle">
			    &nbsp;<span id="frogsLevel" alt="frogs left">Level</span>
			    <span id="frogaLevel" alt="froga level">0</span>
			    <span id="rightNotes"></span>
		    </span>
	    </div>
	    <table class="frogaTable" border="0" cellpadding="0" cellpadding="0">
		    <tr id="tr7" class="frogaTr">
			    <td id="td7a" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
			    <td id="td7b" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
			    <td id="td7c" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/frogHole.png">
				    <img id="hole0" cellid="td7c" class="frogaImage" src="../res/gamez/froga/frogHole.png" border="0" />
			    </td>
			    <td id="td7d" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
			    <td id="td7e" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/frogHole.png">
				    <img id="hole1" cellid="td7e" class="frogaImage" src="../res/gamez/froga/frogHole.png" border="0" />
			    </td>
			    <td id="td7f" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
			    <td id="td7g" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/frogHole.png">
				    <img id="hole2" cellid="td7g" class="frogaImage" src="../res/gamez/froga/frogHole.png" border="0" />
			    </td>
			    <td id="td7h" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
			    <td id="td7i" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
			    <td id="td7j" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/swamp0t.png"></td>
		    </tr>
		    <tr id="tr6" class="frogaTr">
			    <td id="td6a" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed"></td>
			    <td id="td6b" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed"></td>
			    <td id="td6c" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed">
				    <img id="woodT0" cellid="td6c" class="frogaWood" src="../res/gamez/froga/wood0t.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td6d" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed">
				    <img id="woodT1" cellid="td6d" class="frogaWood" src="../res/gamez/froga/wood0t.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td6e" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed"></td>
			    <td id="td6f" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed"></td>
			    <td id="td6g" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed"></td>
			    <td id="td6h" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed">
				    <img id="woodT2" cellid="td6h" class="frogaWood" src="../res/gamez/froga/wood0t.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td6i" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed">
				    <img id="woodT3" cellid="td6i" class="frogaWood" src="../res/gamez/froga/wood0t.png" width="36" height="6%" border="0" />
			    </td>
			    <td id="td6j" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-top-color: #5b4b93; border-top-width: 2px; border-top-style: dashed"></td>
		    </tr>
		    <tr id="tr5" class="frogaTr">
			    <td id="td5a" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed"></td>
			    <td id="td5b" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed">
				    <img id="woodB0" cellid="td5b" class="frogaWood" src="../res/gamez/froga/wood0b.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td5c" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed">
				    <img id="woodB1" cellid="td5c" class="frogaWood" src="../res/gamez/froga/wood0b.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td5d" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed"></td>
			    <td id="td5e" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed"></td>
			    <td id="td5f" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed"></td>
			    <td id="td5g" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed">
				    <img id="woodB2" cellid="td5g" class="frogaWood" src="../res/gamez/froga/wood0b.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td5h" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed">
				    <img id="woodB3" cellid="td5h" class="frogaWood" src="../res/gamez/froga/wood0b.png" width="8%" height="6%" border="0" />
			    </td>
			    <td id="td5i" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed"></td>
			    <td id="td5j" class="frogaTd" width="8%" height="6%" style="background-color: #8e59ee; border-bottom-color: #5b4b93; border-bottom-width: 2px; border-bottom-style: dashed"></td>
		    </tr>
		    <tr id="tr4" class="frogaTr">
			    <td id="td4a" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png"></td>
			    <td id="td4b" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png">
				    <img id="person0" cellid="td4b" class="frogaImage" src="../res/gamez/froga/walk2m.gif" border="0" />
			    </td>
			    <td id="td4c" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png"></td>
			    <td id="td4d" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png">
				    <img id="person1" cellid="td4d" class="frogaImage" src="../res/gamez/froga/walk5m.gif" border="0" />
			    </td>
			    <td id="td4e" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png"></td>
			    <td id="td4f" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png"></td>
			    <td id="td4g" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png">
				    <img id="person2" cellid="td4g" class="frogaImage" src="../res/gamez/froga/walk6m.gif" border="0" />
			    </td>
			    <td id="td4h" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png"></td>
			    <td id="td4i" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png"></td>
			    <td id="td4j" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/walk1m.png">
				    <img id="person3" cellid="td4j" class="frogaImage" src="../res/gamez/froga/walk3m.gif" border="0" />
			    </td>
		    </tr>

		    <tr id="tr3" class="frogaTrUpStreet">
			    <td id="td3a" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png">
				    <img id="car30" cellid="td3a" class="frogaImage" src="../res/gamez/froga/car3.gif" border="0" />
			    </td>
			    <td id="td3b" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
			    <td id="td3c" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
			    <td id="td3d" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
			    <td id="td3e" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png">
				    <img id="car31" cellid="td3e" class="frogaImage" src="../res/gamez/froga/car3.gif" border="0" />
			    </td>
			    <td id="td3f" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
			    <td id="td3g" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
			    <td id="td3h" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
			    <td id="td3i" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png">
				    <img id="car32" cellid="td3i" class="frogaImage" src="../res/gamez/froga/car3.gif" border="0" />
			    </td>
			    <td id="td3j" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0t.png"></td>
		    </tr>
		    <tr id="tr2" class="frogaTr">
			    <td id="td2a" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2b" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png">
				    <img id="car20" cellid="td2b" class="frogaImage" src="../res/gamez/froga/car2.gif" border="0" />
			    </td>
			    <td id="td2c" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2d" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2e" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2f" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2g" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2h" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png">
				    <img id="car21" cellid="td2h" class="frogaImage" src="../res/gamez/froga/car2.gif" border="0" />
			    </td>
			    <td id="td2i" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
			    <td id="td2j" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/street0b.png"></td>
		    </tr>
		    <tr id="tr1" class="frogaTr">
			    <td id="td1a" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png">
				    <img id="meadow0b0" cellid="td1a" class="frogaImage" src="../res/gamez/froga/meadow0b.png" border="0" />
			    </td>
			    <td id="td1b" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png"></td>
			    <td id="td1c" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png"></td>
			    <td id="td1d" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png">
				    <img id="frog0" cellid="td1d" idwood="" class="frogaImage" src="../res/gamez/froga/frogActive.gif" title="ActiveFrog" border="0" />
			    </td>
			    <td id="td1e" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png">
				    <img id="frog1" cellid="td1e" idwood="" class="frogaImage" src="../res/gamez/froga/frogSleepy.png" border="0" />
			    </td>
			    <td id="td1f" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png">
				    <img id="frog2" cellid="td1f" class="frogaImage" src="../res/gamez/froga/frogSleepy.png" border="0" idwood="" />
			    </td>
			    <td id="td1g" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png">
				    <img id="frog3" cellid="td1g" idwood="" class="frogaImage" src="../res/gamez/froga/frogSleepy.png" border="0" />
			    </td>
			    <td id="td1h" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png"></td>
			    <td id="td1i" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png"></td>
			    <td id="td1j" class="frogaTd" width="8%" height="6%" background="../res/gamez/froga/meadow1b.png">
				    <img id="meadow0b1" cellid="td1j" class="frogaImage" src="../res/gamez/froga/meadow3b.png" border="0" />
			    </td>
		    </tr>
	    </table>
	    <div class="frogaFooter">
            <span id="spanLeft" align="left" valign="middle">
                <img id="aLeft" class="frogaImage" src="../res/gamez/froga/arrow/a_left.gif" border="0" onclick="moveFrog('left')" />
            </span>
		    <img id="aUp" class="frogaImage" src="../res/gamez/froga/arrow/a_up.gif" border="0" onclick="moveFrog('up')" />
            <img id="aDown" class="frogaImage" src="../res/gamez/froga/arrow/a_down.gif" border="0" onclick="moveFrog('down')" />
            <span id="spanRight" align="right" valign="middle">
                <img id="aRight" src="../res/gamez/froga/arrow/a_right.gif" border="0" onclick="moveFrog('right')" />
            </span>
	    </div>	    
    </div>
</asp:Content>
