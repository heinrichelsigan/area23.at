<%@ Page Title="TicTacToe" Language="C#" MasterPageFile="~/Games/GamesMaster.master" AutoEventWireup="false"  %>
<%@ Import namespace="Area23.At.Framework.Library" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>
<%@ Import namespace="Area23.At.Mono.Util" %>

<script runat="server" language="C#">
   
    void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            
        }
    }

</script>

<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>TicTacToe</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<style>
        body.ticTacToeBody {
            color: white;
            background-color: black;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;
        }

        div.ticTacToe {
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

            div.ticTacToe.img {
                width: 20%;
                min-width: 36pt;
                height: 20%;
                min-height: 27pt;
                padding: 0 0 0 0;
                margin: 0 0 0 0;
                background-repeat: no-repeat;
                background-size: 100% 100%;
            }

            div.ticTacToe.ticTacToeHeader {
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

        div.ticTacToeHeader#headerLeft {
            height: 10%;
            min-height: 45pt;
            width: 27%;
            min-width: 90pt;
            vertical-align: middle;
            font-size: larger;
            text-align: right;
        }

        div.ticTacToeHeader#headerCenter {
            height: 10%;
            min-height: 45pt;
            width: 64%;
            min-width: 297pt;
            vertical-align: middle;
            font-size: medium;
            text-align: center;
        }

        div.ticTacToeHeader.headerImage {
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

        div.ticTacToeHeader#headerRight {
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

        div.ticTacToe.ticTacToeFooter {
            width: 81%;
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

            div.ticTacToe.ticTacToeFooter#spanLeft {
                height: 36pt;
                min-height: 36pt;
                width: 40pt;
                min-width: 40pt;
                vertical-align: middle;
                text-align: left;
            }

            div.ticTacToe.ticTacToeFooter#spanRight {
                height: 36pt;
                min-height: 36pt;
                width: 40pt;
                min-width: 40pt;
                vertical-align: middle;
                text-align: right
            }


        table.ticTacToeTable {
            width: 60%;
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

        tr.ticTacToeTr {
            width: 60%;
            min-width: 360px;
            height: 20%;
            min-height: 27px;
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

        tr.ticTacToeTrUpStreet {
            width: 60%;
            min-width: 360px;
            height: 6%;
            min-height: 27px;            
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

        td.ticTacToeTd {
            width: 20%;
            /* min-width: 36pt; */
            height: 20%;
            min-height: 27pt;           
            /* background-repeat: no-repeat; */
            background-size: 100% 100%;
        }

        img.ticTacToeImage,
        img.ticTacToeWood {
            width: 100%;
            min-width: 36pt;
            height: 100%;
            min-height: 27pt;
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

            img.ticTacToeImage#ticTacToe0,
            img.ticTacToeImage#ticTacToe1,
            img.ticTacToeImage#ticTacToe2,
            img.ticTacToeImage#ticTacToe3 {
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
            ticTacToeInit();
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

        // ticTacToeInitInit will be called on 1st time loading
        function ticTacToeInit() {
            windowCursorKeysHandler();
            ticTacToeInitLoad();
        }

        // ticTacToeInitReStart(repeatLevel) => repeatLevel = true
        function ticTacToeInitReStart(repeatLevel) {
            if (repeatLevel) {
                gameOver = 0;
                window.location.reload();
            } else { // TODO: fix this
                reCreateFrogs();
                frogLoad();
            }
        }

        // ticTacToe loader
        function ticTacToeLoad() {
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
                document.getElementById("headerImg").src = "../res/img/header.png";
                document.getElementById("headerImg").focus();
                document.getElementById("headerImg").blur();
            }
            var frogZero = document.getElementById("frog0");
            if (frogZero != null)
                frogZero.focus();

            setTimeout(function () { frogaLooper(loopTicks, loopDelay) }, loopDelay); // will call function after loopDelay milli seconds.
        }

        // main js looper => keeping game in movement
        function ticTacToeInitLooper(ticks, delay) {

            let leftNotes = document.getElementById("leftNotes");
            let rightNotes = document.getElementById("rightNotes");
            if (leftNotes.innerHTML.length > 1)
                leftNotes.innerHTML = "";
            if (rightNotes.innerHTML.length > 1)
                rightNotes.innerHTML = "";

            currentFrog = getActiveFrog();

            // level completed
            if (ticTacToeCompleted)
            {
                headerImg.src = "../res/img/levelcompleted.gif"                
                headerImg.height = 36;
                level++;
                soundDuration = 3600;
                setTimeout(function () { ticTacToeSound("../res/audio/levelCompleted.mp3") }, 100);
                setTimeout(function () { ticTacToeReStart(false); }, 4000); // will call the function after 8 secs.
                return;
            }
            // game over
            if (currentFrog == null || frogsDied > 3) {
                headerImg.src = "../res/img/gameover.png";
                headerImg.height = 36;
                gameOver = 1;
                soundDuration = 4800;
                setTimeout(function () { ticTacToeSound("../res/audio/frogaGameOver.mp3") }, 100);
                setTimeout(function () { ticTacToeReStart(true); }, 5000); // will call the function after 8 secs.
                return;
            }

            

            loopTicks = ticks + 1;

            // setTimeout(function () { frogaLooper(loopTicks, delay) }, delay); // will call the function after 16 secs.
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
                document.getElementById("aUp").src = "../res/img/a_up.gif";
            }
            else if (jumpDirection.charAt(0) == 'd') { 							// TODO should we let frog drive back to start meadow
                nrY = below(frY);												// down 				
                document.getElementById("aDown").src = "../res/img/a_down.gif";
            }

            if (jumpDirection.charAt(0) == 'r' || jumpDirection.charAt(1) == 'r') {
                nrX = righter(frX);												// right
                document.getElementById("aRight").src = "../res/img/a_right.gif";
            } else if (jumpDirection.charAt(0) == 'l' || jumpDirection.charAt(1) == 'l') {
                nrX = lefter(frX);												// left
                document.getElementById("aLeft").src = "../res/img/a_left.gif";
            }

            // TODO: better use newTd = getNewTdPositionByMoving(car, 'rr');
            var newTd = "td" + nrY + nrX;
            var newFrog = document.getElementById(currentFrogId);

            newFrog.id = currentFrogId;
            newFrog.title = "ActiveFrog";
            newFrog.border = "0";
            newFrog.src = "data:image/gif;base64,R0lGODlhJAAbAMZhAGBmAF5lJ3d5AHyBAGuvAG2xAG+xAG6yAG+yAHCyAHKyAHCzAHKzAHG0AHK0AHC0CHezAHK1AHO1AHO2AHS2AHW2AHO3AHW3AnyvR3W3BXa2DnS4AHO5AHO6AHe5AHS6AHe5AXi5AY2zAHS7AHW7AHW8AHe9AHe+AHi+AHy8C3y8Dni/AH28FH69C36+AHnAAKi0Aa+yAH3CAKm1AILAEoLAF37CFoPAHoDEAITBF37DF4TBGYS9TaizRIPEBITDEoTECYbCH4TEEoTEG4bEGojDIYjDI4jDJIfEHobFH4nELovAWa7BAI3HNY7HN5DJPJLJQK7IAJTLQZTNRpbNRpfNRpnJbpvQTJvRSLbOE5zSSZ3SSr3OEqfXWsrWAKraX6zcYv///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////yH+IkNyZWF0ZWQgIGJ5IGhlQGFyZWEyMy5hdCB3aXRoIEdJTVAAIfkECRIAfwAsAAABACQAGgAAB9SAf4KDhIWGh4iJiouMikpgf2BKjZSEV12XV5WbT00VTU+blDNeC6ZeM6KMAKWnAKqFWn+ygjEKpgsKMYOytI1bfy9/QoMMDbgNDIPEwsCMKH84LwSDuNaDBC/C0IzZ2oTWuITa05Tk5CPhpiPn35QE7OjqCyMl6NSwfwTz+PmF++r6+cPGb+C/gvmMKDQiCGA4fAsZMooY8c+8PxQXLsqoEMg8IBwlKgo5IdyEkJQ4ugjnguOmjD7C+cioKiMFXBRowqK40lTLiv4WurBo6s9PkYgCAQAh+QQJEgB/ACwAAAAAJAAbAAAH2oB/goOEhYaHiImHSmB/YEqKkYpXXZRXkpiGT00VTU+ZoH8zXgulXjOhmACkpgCphlp/sYIxCqULCjGDsbOYW38vf0KDDA23DQyDw8G/kih/OC8Eg7fVgwQvwc+S2NmE1beE2dKZ4+Mj4KUj5t6ZBOvn6QsjJefTr4IE8vf4hvrp/PoR+gcuoMB8+w4WIljNYCgjEI1QSzcookRJFi3+kfcnY8RIHiECkQck5EVFJieAm2AyU0gX4FyEfJjRBzgfHl95pHCLQk58GWGWkqlRYEQXG0v9IXpSocBAACH5BAkSAH8ALAAAAAAkABsAAAemgH+Cg4SFfzxWf1Y8ho2Oj39LVpJLkJaQPDwYmZedhT09GKKgnqUBoaMBpZ6oohg9q4aVlYKutqKDs52VGH+9tbeug729tI/EuMDBv764zI62hMvJys/Q09i3ndnc1LHYsZDg4dfL5OXB543j4Znu1dqH7oyY8/PY9vOO+ffT/O+G/mXCJ3CfwGUC6Rnkh/DfpX8N83mCaMvhqny3JJ5zh4wYwEeBAAA7";
            // newFrog.src = "../res/img/frogActive.gif";
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
                        newFrog.src = "../res/img/frogOnWoodB.gif#" + woodIt;
                        newFrog.setAttribute("idwood", woodId);
                        woodIt = 4;
                        break;
                    }
                    woodIt++;
                }
                // frog dies in river
                if (imgDisApear == null) {
                    frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/img/frogDiesInWaterB.gif", "../res/audio/frogUnderWater.ogg", "died", "Frog died!");
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
                        newFrog.src = "../res/img/frogOnWoodT.gif#" + woodIt;
                        newFrog.setAttribute("idwood", woodId);
                        woodIt = 4;
                        break;
                    }
                    woodIt++;
                }
                // frog dies in river
                if (imgDisApear == null) {
                    frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/img/frogDiesInWaterT.gif", "../res/audio/frogUnderWater.ogg", "died", "Frog died!");
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
                        if ((imgDisApear.src.substr(idaLen - 27) == "../res/img/frogTwiceInHole.gif") ||
                            (imgDisApear.src.substr(idaLen - 22) == "../res/img/frogInHole.gif")) {
                            frogDoubleHole++;
                            woodIt = 4; break;
                        }
                        if (imgDisApear.src.substr(idaLen - 20) == "../res/img/frogHole.png") {
                            woodIt = 4;
                            break;
                        }
                    }
                    woodIt++;
                }

                if (imgDisApear == null) {
                    frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/img/frogDiesInSwamp.gif", "../res/audio/frogInSwamp.ogg", "died", "Frog died!");
                }
                else if (imgDisApear != null) {
                    if (frogDoubleHole >= 1) {
                        newTd = "td" + nrY + lefter(nrX);
                        frogDied = frogInRiverOrSwampOrHole(newFrog, "../res/img/frogDiesInSwamp.gif", "../res/audio/frogInSwamp.ogg", "died", "frog" + frogNr + "@graveyard");
                        imgDisApear = null;
                    }
                    else if (frogDoubleHole < 1) {
                        frogsInHole++;
                        setFrogsInHole(frogsInHole);
                        frogInRiverOrSwampOrHole(newFrog, "../res/img/frogInHole.gif", "../res/audio/frogInHole.ogg", "save", "frog" + frogNr + "@home");
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
        function ticTacToeSound(soundName) {
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


        // get active current frog
        function getActivePlayer() {

            let aFrogIt = 0;
            var aFrogId = "frog0";
            currentFrog = null;

            for (aFrogIt = 0; aFrogIt < 4; aFrogIt++) {

                aFrogId = "frog" + aFrogIt;							// TODO: define frog prefix "frog" as constant
                currentFrog = document.getElementById(aFrogId);

                if (currentFrog != null) {
                    if (currentFrog.title != "ActiveFrog")
                        currentFrog.title = "ActiveFrog";
                    // currentFrog.src = "../res/img/frogActive.gif";
                    fY = rowByTag(currentFrog);
                    fX = columnByTag(currentFrog)
                    return currentFrog;
                }
            }

            return currentFrog;
        }



        function reCreateTicTacToe() {
            // first clear all bottom and top table cells, so that there rest neither frogs nor holes there
            var tdsToClear = [
                "td2a", "td2b", "td2c", "td1a", "td1b", "td1c", "td0a", "td0b", "td0c"];
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
                       
        }

        function setLevel(ticTacToeLevel) {
            document.getElementById("ticTacToeLevelLevel").innerHTML = ticTacToeLevel;
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
    <div class="ticTacToe" align="center">
	    <div class="ticTacToeHeader">
            <span id="headerLeft" align="right" valign="middle">
                <span id="leftNotes"></span>
                <img class="frogsInImg" src="../res/img/frogsLeftCount.png" /><span id="frogsLeft" alt="frogs left">4</span>
                <img class="frogsInImg" src="../res/img/frogsDiedCount.png" /><span id="frogsDied" alt="frogs died">0</span>
                <img class="frogsInImg" src="../res/img/frogsInHoleCount.png" /><span id="frogsInHole" alt="frogs in hole">0</span>
            </span>
		    <span id="headerCenter" align="center" valign="middle">
			    <img class="headerImage" src="../res/img/header.png" id="headerImg" border="0" onclick="restart()" />
		    </span>
		    <span id="headerRight" align="left" valign="middle">
			    &nbsp;<span id="frogsLevel" alt="frogs left">Level</span>
			    <span id="frogaLevel" alt="froga level">0</span>
			    <span id="rightNotes"></span>
		    </span>
	    </div>
	    <table class="ticTacToeTable" border="2" cellpadding="2" cellpadding="2">
		    <tr id="tr2" class="ticTacToeTr">
			    <td id="td2a" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
			    <td id="td2b" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
			    <td id="td2c" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
		    </tr>
		    <tr id="tr1" class="ticTacToeTr">
			    <td id="td1a" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
			    <td id="td1b" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
			    <td id="td1c" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
		    </tr>
            <tr id="tr0" class="ticTacToeTr">
                <td id="td0a" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
                <td id="td0b" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
                <td id="td0c" class="ticTacToeTd" width="20%" height="20%" style="background-color: #efefef; border-color: black; border-top-width: 2px; border-top-style: outset"></td>
            </tr>
	    </table>
	    <div class="ticTacToeFooter">
            <span id="spanLeft" align="left" valign="middle">
                <img id="aLeft" class="ticTacToeImage" src="../res/img/a_left.gif" border="0" onclick="moveFrog('left')" />
            </span>
		    <img id="aUp" class="ticTacToeImage" src="../res/img/a_up.gif" border="0" onclick="moveFrog('up')" />
            <img id="aDown" class="ticTacToeImage" src="../res/img/a_down.gif" border="0" onclick="moveFrog('down')" />
            <span id="spanRight" align="right" valign="middle">
                <img class="ticTacToeImage" id="aRight" src="../res/img/a_right.gif" border="0" onclick="moveFrog('right')" />
            </span>
	    </div>	    
    </div>
</asp:Content>
