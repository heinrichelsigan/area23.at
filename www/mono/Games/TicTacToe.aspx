﻿<%@ Page Title="TicTacToe" Language="C#" MasterPageFile="~/Games/GamesMaster.master" AutoEventWireup="false"  %>
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



        span#leftNotes {
            color: whitesmoke;
            width: 5.33%;
            max-width: 27px;
            font-size: large;
        }

        span#frogsInHole {
            color: greenyellow;
            width: 32px;
            margin-left: -18pt;
            padding-right: 6pt;
            font-weight: bold;
        }

        span#frogsLeft {
            color: aqua;
            width: 32px;
            margin-left: -18pt;
            padding-right: 2pt;
            font-size: large;
            /*visibility: hidden; */
        }

        span#frogsDied {
            color: gainsboro;
            width: 32px;
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
            max-width: 27px;
            font-size: large;
        }

        div.ticTacToe.ticTacToeFooter#spanLeft {
            min-width: 32px;
            width: 40px;
            min-height: 27px;    
            height: 36px;

            vertical-align: middle;
            text-align: left;
        }

        div.ticTacToe.ticTacToeFooter#spanRight {
            min-width: 32px;
            width: 40px;
            min-height: 27px;    
            height: 36px;
            
            vertical-align: middle;
            text-align: right
        }


        table.ticTacToeTable {
            width: 69%;
            min-width: 345px;
            height: 69%;
            min-height: 345px;  
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            align-content: center;

            background-color: #1f1f1f;
            background-repeat: no-repeat;

            border-style: outset;
            border-spacing: 1px;
            border-width: 1px;
            border-color: blueviolet;            
        }

        tr.ticTacToeTr {
            width: 69%;
            min-width: 360px;
            height: 24%;
            min-height: 120px;

            background-color: #1f1f1f;
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */

            border-color: blueviolet; 
            border-top-width: 1px; 
            border-top-style: outset;

        }

        td.ticTacToeTd {
            width: 20%;
            min-width: 120px; 
            height: 24%;
            min-height: 120px;         
            
            background-color: #1f1f1f;
            background-image: url('../res/img/emptyCellBlueTikTakToe.png');
            /* background-repeat: no-repeat; */
            background-size: 100% 100%;

            border-color: blueviolet; 
            border-top-width: 1px; 
            border-top-style: outset;             
        }

        img.frogsInImg {
            min-width: 32px;
            width: 64px;
            min-height: 27px;            
            height: 54px;         
    
            padding: 0 0 0 0;
            margin: 0 0 0 0;
    
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;

            background-repeat: no-repeat;
            background-size: 100% 100%;
        }


        img.ticTacToeImage,
        img.ticTacToeImage#ticTacToe0,
        img.ticTacToeImage#ticTacToe1,
        img.ticTacToeImage#ticTacToe2,
        img.ticTacToeImage#ticTacToe3 {
            width: 100%;
            min-width: 120px;
            height: 100%;
            min-height: 120px;

            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

        
        
        img#aUp,
        img#aDown,
        img#aLeft,
        img#aRight {
            width: 120px;
            min-width: 120px;
            height: 120px;
            min-height: 120px;
            background-repeat: no-repeat;
            background-size: 100% 100%;
        }

	</style>
	<script type="text/javascript">

        var loopDelay = 1625,
            loopTicks = 0,
            soundDuration = 1625,
            seconds = -1;
        var level = 0,
            androidCount = 5,
            playersCount = 5,
            whoStarts = -1,
            whoNext = -1,
            whoWins = -1,
            gameOver = 0;
        var ticTacToeBoard = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"],
            ticTacToeGame = ["", "", "", "", "", "", "", "", ""],
            ticTacToeAndroid = ["", "", "", "", "", "", "", "", ""],
            ticTacToePlayers = ["", "", "", "", "", "", "", "", ""];

        var imgPlayer, imgSkull, imgComputer, imgAndroid;

        window.onload = function () {
            ticTacToeInit();
        }; 

        // windows cursor key press hanlder
        function windowCursorKeysHandler() {
            window.onkeydown = function (e) { // TODO: pressing two arrow keys at same time
                if (e.which > 13) {
                    if (e.which == 97) {
                        ticMouseOver('a0');
                        tacPlayerSets('a0');
                    }
                    else if (e.which == 98) {
                        ticMouseOver('b0');
                        tacPlayerSets('b0');
                    }
                    else if (e.which == 99) {
                        ticMouseOver('c0');
                        tacPlayerSets('c0');
                    }
                    else if (e.which == 100) {
                        ticMouseOver('a1');
                        tacPlayerSets('a1');
                    }
                    else if (e.which == 101) {
                        ticMouseOver('b1');
                        tacPlayerSets('b1');
                    }
                    else if (e.which == 102) {
                        ticMouseOver('c1');
                        tacPlayerSets('c1');
                    }
                    else if (e.which == 103) {
                        ticMouseOver('a2');
                        tacPlayerSets('a2');
                    }
                    else if (e.which == 104) {
                        ticMouseOver('b2');
                        tacPlayerSets('b2');
                    }
                    else if (e.which == 105) {
                        ticMouseOver('c2');
                        tacPlayerSets('c2');
                    }
                    // alert("e.which = " + e.which);
                }
            };
        }

        // ticTacToeInit will be called on 1st time loading
        function ticTacToeInit() {
            windowCursorKeysHandler();
            ticTacToeLoad();
        }

        // ticTacToeReStart(repeatLevel) => repeatLevel = true
        function ticTacToeReStart(repeatLevel) {
            if (repeatLevel) {
                gameOver = 0;
                window.location.reload();
            } else { // TODO: fix this
                toeReCreate();
                frogLoad();
            }
        }

        // ticTacToeLoad game loader
        function ticTacToeLoad() {

            const now = new Date(Date.now());
            seconds = now.getSeconds();
            loopTicks = 0;
            androidCount = 5;
            playersCount = 5;
            whoStarts = (seconds % 2);
            whoNext = whoStarts,
                whoWins = -1;
            gameOver = 0;
            ticTacToeBoard = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"];
            ticTacToeGame = ["", "", "", "", "", "", "", "", ""];
            ticTacToePlayers = ["", "", "", "", "", "", "", "", ""];
            ticTacToeAndroid = ["", "", "", "", "", "", "", "", ""];
            imgPlayer = getNewImage(1, "");
            imgComputer = getNewImage(0, "");

            switch (level) {
                case 0: loopDelay = 1625; break;
                case 1: loopDelay = 1500; break;
                case 2: loopDelay = 1375; break;
                case 3: loopDelay = 1250; break;
                case 4: loopDelay = 1125; break;
                default: loopDelay = 1000; break;
            }
            setLevel(level);
            setPlayersCounter(playersCount);
            setAndroidCounter(androidCount);

            var headerImg = document.getElementById("headerImg");
            if (headerImg != null) {
                document.getElementById("headerImg").src = "../res/img/headerTicTacToe.png";
                document.getElementById("headerImg").focus();
                document.getElementById("headerImg").blur();
            }

            if (whoStarts == 1) // Player starts
            {
                alert("Player starts new Tic Tac Toe game, click on board or key pad 1-9.");
            }
            else if (whoStarts == 0) // Computer starts
            {
                alert("Computer starts new Tic Tac Toe game,\nafter computers first android set\nclick on board or key pad 1-9 to set your skull.");
                tacComputerSets(loopTicks);                
            }

            // setTimeout(function () { frogaLooper(loopTicks, loopDelay) }, loopDelay); // will call function after loopDelay milli seconds.
        }

        // tacCalculateGamblerArrays => gets computer & players status 
        function tacCalculateGamblerArrays() {
            let toeCnt = -1;
            var ticTacToeTd = null;
            var ticTacToeCellId = null;
            ticTacToeBoard.forEach(function (ticTacToeCellId) {

                toeCnt++;
                ticTacToeTd = document.getElementById(ticTacToeCellId);
                if (ticTacToeTd != null) {

                    var ticAlt = ticTacToeTd.alt;
                    var ticTacToeValue = ticTacToeTd.getAttribute("ticTacToe");
                    if (ticAlt != null && ticAlt.charAt(0) != '\0' && ticTacToeValue != null && ticTacToeValue.charAt(0) != '\0') {

                        if (ticAlt.charAt(0) == 'a') {
                            ticTacToeGame[toeCnt] = 'a';
                            ticTacToeAndroid[toeCnt] = ticTacToeCellId;
                        }
                        if (ticAlt.charAt(0) == 'p') {
                            ticTacToeGame[toeCnt] = 'p';
                            ticTacToePlayers[toeCnt] = ticTacToeCellId;
                        }
                    }
                }
            });
        }

        // toeFinished checks if computer or player already wins that game
        // ticks        game ticks
        // whoPlayed    0 for computer, 1 for player
        function toeFinished(ticks, whoPlayed) {
            
            let leftNotes = document.getElementById("leftNotes");
            let rightNotes = document.getElementById("rightNotes");
            if (leftNotes.innerHTML.length > 1)
                leftNotes.innerHTML = "";
            if (rightNotes.innerHTML.length > 1)
                rightNotes.innerHTML = "";


            tacCalculateGamblerArrays();

            whoWins = toeWhoWins(whoPlayed, (whoPlayed == 0) ? ticTacToeAndroid : ticTacToePlayers);

            // level completed
            if (whoWins == 1) {
                headerImg.src = "../res/img/levelcompleted.gif"
                headerImg.height = 36;
                level++;
                soundDuration = 3600;
                setTimeout(function () { ticSound("../res/audio/levelCompleted.mp3") }, 100);
                setTimeout(function () { ticTacToeReStart(false); }, 4000); // will call the function after 8 secs.
                return;
            }
            else if (whoWins == 0)
            {
                headerImg.src = "../res/img/gameover.png";
                headerImg.height = 36;
                gameOver = 1;
                soundDuration = 4800;
                setTimeout(function () { ticSound("../res/audio/frogaGameOver.mp3") }, 100);
                setTimeout(function () { ticTacToeReStart(true); }, 5000); // will call the function after 8 secs.
                return;
            }

            loopTicks = ticks + 1;

            // setTimeout(function () { frogaLooper(loopTicks, delay) }, delay); // will call the function after 16 secs.
        }

        // checks if somebody already wins
        // whoPlayed    0 for computer, 1 for player
        // gameArray    ticTacToeAndroid for computer, ticTacToePlayers for player
        // return       -1 if game not finished or nobody wins game, 0 if computer won, 1 if player wins game
        function toeWhoWins(whoPlayed, gameArray) {

            if (gameArray == null || gameArray.length != 9)
                return -1;

            let toeWin = -1;
            let ga0 = -1, gb0 = -1, gc0 = -1, ga1 = -1, gb1 = -1, gc1 = -1, ga2 = -1, gb2 = -1, gc2 = -1;
            if (gameArray[0] != null && gameArray[0].charAt(0) != '\0')
                ga0 = 1;
            if (gameArray[1] != null && gameArray[1].charAt(0) != '\0')
                gb0 = 1;
            if (gameArray[2] != null && gameArray[2].charAt(0) != '\0')
                gc0 = 1;
            if (gameArray[3] != null && gameArray[3].charAt(0) != '\0')
                ga1 = 1;
            if (gameArray[4] != null && gameArray[4].charAt(0) != '\0')
                gb1 = 1;
            if (gameArray[5] != null && gameArray[5].charAt(0) != '\0')
                gc1 = 1;
            if (gameArray[6] != null && gameArray[6].charAt(0) != '\0')
                ga2 = 1;
            if (gameArray[7] != null && gameArray[7].charAt(0) != '\0')
                gb2 = 1;
            if (gameArray[8] != null && gameArray[8].charAt(0) != '\0')
                gc2 = 1;

            if ((ga0 == 1 && gb0 == 1 && gc0 == 1) ||
                (ga1 == 1 && gb1 == 1 && gc1 == 1) ||
                (ga2 == 1 && gb2 == 1 && gc2 == 1) ||
                (ga0 == 1 && ga1 == 1 && ga2 == 1) ||
                (gb0 == 1 && gb1 == 1 && gb2 == 1) ||
                (gc0 == 1 && gc1 == 1 && gc2 == 1) ||
                (ga0 == 1 && gb1 == 1 && gc2 == 1) ||
                (gc0 == 1 && gb1 == 1 && ga2 == 1))
                toeWin = whoPlayed;

            return toeWin;
        }


        // clear marked cells 
        function ticClearMarkedCells(tdCellId) {
            var ticTacToeTd = null;
            let ticTacToeCellId = "";
            ticTacToeBoard.forEach(function (ticTacToeCellId) {

                ticTacToeTd = document.getElementById(ticTacToeCellId);

                if (ticTacToeTd != null) {
                    ticTacToeTd.style.backgroundColor = "#1f1f1f";
                    ticTacToeTd.style.borderWidth = 1;
                    ticTacToeTd.style.borderStyle = "outset";
                }
            });

        }


        // ticMouseOver marks on mouse over the blured cell on tic tac toe board
        function ticMouseOver(tdCellId) {

            var cellTicTacToeOver = null;
            ticClearMarkedCells(tdCellId);
            cellTicTacToeOver = document.getElementById(tdCellId);

            if (cellTicTacToeOver != null) {

                cellTicTacToeOver.style.backgroundColor = "#3f3f3f";
                cellTicTacToeOver.style.borderWidth = 2;
                cellTicTacToeOver.style.borderStyle = "solid";
            }
        }

        // ticTacToe player sets skull on board
        function tacPlayerSets(tdCellId) {

            var cellTicTacToe = null;
            var cellAlt = null;
            var setCellTd = document.getElementById(tdCellId);
            if (setCellTd != null) {

                try {
                    if (setCellTd.title != null && setCellTd.title.charAt(0) != '\0')
                        cellAlt = setCellTd.title;
                } catch (exGetTitle) {
                    cellAlt = null;
                }
                try {
                    if (cellAlt == null)
                        cellAlt = setCellTd.getAttribute("title");
                } catch (exGetAlt) {
                    cellAlt = null;
                }
            }

            cellTicTacToe = setCellTd.getAttribute("ticTacToe");
            if (cellTicTacToe != null && cellAlt != null && cellTicTacToe.charAt(0) != '\0' && cellAlt.charAt(0) != '\0') {
                alert("Can't set players skull here!");
            } else {
                imgPlayer = getNewImage(1, tdCellId);
                imgSkull = copyImg(imgPlayer);
                setCellTd.appendChild(imgSkull);
                setCellTd.setAttribute("ticTacToe", imgPlayer.id);
                setCellTd.alt = imgPlayer.id;

                if (playersCount > 0) {
                    playersCount--;
                    setPlayersCounter(playersCount);
                    imgPlayer = getNewImage(1, "");
                }

                toeFinished(loopTicks, 1);
                whoNext = 0;

                tacComputerSets(loopTicks);
            }

            ticClearMarkedCells(tdCellId);
        }

        // ticTacToe computer sets android on board
        function tacComputerSets(ticks) {

            var ticTacToeTd = null;
            let toeCnt = -1;
            var tacCellId = null;
            var toeCellId = null;
            imgComputer = getNewImage(0, "");
            tacCalculateGamblerArrays();

            if (loopTicks < 4 && (ticTacToeGame[4] == null || ticTacToeGame[4].charAt(0) == '\0')) {
                toeCellId = 'b2';
            }

            if (toeCellId == null || toeCellId.charAt(0) == '\0') {
                // stupid ai for computer to find free field
                ticTacToeGame.forEach(function (tacCellId) {
                    toeCnt++;
                    ticTacToeTd = document.getElementById(tacCellId);
                    if (toeCellId == null || toeCellId.charAt(0) == '\0') {
                        if (ticTacToeTd != null && (ticTacToeGame[toeCnt] == null || ticTacToeGame[toeCnt] == "")) {
                            toeCellId = ticTacToeBoard[toeCnt];
                        }
                    }
                });
            }    

            var setCellTd = document.getElementById(toeCellId);
            if (setCellTd != null) {

                imgComputer = getNewImage(0, toeCellId);

                imgAndroid = copyImg(imgComputer);
                setCellTd.appendChild(imgAndroid);
                setCellTd.setAttribute("ticTacToe", imgAndroid.id);
                setCellTd.alt = imgAndroid.id;

                if (androidCount > 0) {
                    androidCount--;
                    setAndroidCounter(androidCount);
                    imgComputer = getNewImage(0, "");
                }

                toeFinished(loopTicks, 0);

                whoNext = 1;
            } else {  // what shell we do with the drunken sailor
                alert("drunken sailor!");
            }
        }

        // set players remaining skull discs to set
        function setPlayersCounter(playersCnt) {
            var spanplayersCounter = document.getElementById("playersCounter");
            if (spanplayersCounter != null)
                spanplayersCounter.innerText = playersCnt;
        }

        // set computer remaining android stickers to set
        function setAndroidCounter(androidCnt) {
            var elemAndroidCounter = document.getElementById("androidCounter");
            if (elemAndroidCounter != null)
                elemAndroidCounter.innerHTML = androidCnt;
        }

        // sets level number inside span id=ticTacToeLevel
        function setLevel(tacToeLevel) {

            var levelSpan = document.getElementById("ticTacToeLevel");

            if (levelSpan != null) {
                levelSpan.innerHTML = tacToeLevel;
            }

        }


        // get a new image to set at next move
        function getNewImage(forWho, aCellId) {

            var ticImg = new Image(32, 27);
            var ticNum = (forWho == 0) ? androidCount : playersCount;
            var ticId = (forWho == 0) ? "a" + ticNum : "p" + ticNum;
            var ticTitle = "";
            var ticImgSrc = (forWho == 0) ? "../res/img/androidSetTicTacToe.gif" : "../res/img/skullSetTicTacToe.gif";
            var cellIdSet = "";

            if (aCellId != null && aCellId.charAt(0) != '\0' && aCellId.length > 1) {

                if ((aCellId.charAt(0) == 'a' || aCellId.charAt(0) == 'b' || aCellId.charAt(0) == 'c') &&
                    (aCellId.charAt(1) == '0' || aCellId.charAt(1) == '1' || aCellId.charAt(1) == '2'))
                    cellIdSet = aCellId;
            }

            if (forWho == 0) {
                ticImgSrc = "../res/img/androidSetTicTacToe.gif";
                ticNum = androidCount;
                ticId = "a" + ticNum;
                ticTitle = "android" + ticNum;
            }
            else {
                ticImgSrc = "../res/img/skullSetTicTacToe.gif";
                ticNum = playersCount;
                ticId = "p" + ticNum;
                ticTitle = "players" + ticNum;
            }

            ticImg.Id = ticId;
            ticImg.alt = ticId;
            ticImg.src = ticImgSrc
            ticImg.setAttribute("border", 0);
            ticImg.setAttribute("title", ticTitle);
            ticImg.setAttribute("class", "ticTacToeImage");
            ticImg.setAttribute("className", "ticTacToeImage");
            ticImg.setAttribute("cellid", cellIdSet);

            return ticImg;
        }

        //// sound and image 
        function ticSound(soundName) {

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
                setTimeout(function () { frogSound(soundToPlay); }, 100);

        }


        // toeReCreate clears are finished tic tac toe board
        function toeReCreate() {
            
            var tableCell = null;
            var childFromTableCell = null;
            var tCellTdId = null;
            var tdsToClear = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"];

            tdsToClear.forEach(function (tCellTdId) {

                tableCell = document.getElementById(tCellTdId);

                if (tableCell != null && tableCell.children != null && tableCell.children.length > 0) {

                    for (tdCellIt = 0; tdCellIt < tableCell.children.length; tdCellIt++) {

                        childFromTableCell = tableCell.children[tdCellIt];
                        if (childFromTableCell != null)
                            tableCell.removeChild(childFromTableCell);
                    }
                }
            });
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

        // copies an image to another
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
                if (imgC.getAttribute("ticTacToe") != null)
                    imgD.setAttribute("ticTacToe", imgC.getAttribute("ticTacToe"));
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
                <span id="ticTacToeLevel">0</span>
                <span id="leftNotes"></span>
                <img class="frogsInImg" src="../res/img/skullTicTacToe.gif" /><span id="playersCounter" alt="players counter">5</span>
                &nbsp;
            </span>
		    <span id="headerCenter" align="center" valign="middle">
			    <img class="headerImage" src="../res/img/headerTicTacToe.png" id="headerImg" border="0" onclick="restart()" />
		    </span>
		    <span id="headerRight" align="left" valign="middle">
			    &nbsp;<img class="frogsInImg" src="../res/img/androidTicTacToe.gif" /><span id="androidCounter" alt="android counter">5</span>
		    </span>
            <span id="rightNotes"></span>
	    </div>
	    <table class="ticTacToeTable" border="1" cellpadding="1" cellpadding="1">
		    <tr id="t2" class="ticTacToeTr">
			    <td id="a2" onmouseover="ticMouseOver('a2'); return false;" onclick="tacPlayerSets('a2'); return false;" class="ticTacToeTd" width="20%" height="24%" title="">
			    </td>
			    <td id="b2" onmouseover="ticMouseOver('b2'); return false;" onclick="tacPlayerSets('b2'); return false;" class="ticTacToeTd" width="20%" height="24%" title="">
			    </td>
			    <td id="c2" onmouseover="ticMouseOver('c2'); return false;" onclick="tacPlayerSets('c2'); return false;" class="ticTacToeTd" width="20%" height="24%" title="">
			    </td>
		    </tr>
		    <tr id="tr1" class="ticTacToeTr">
			    <td id="a1" class="ticTacToeTd" onmouseover="ticMouseOver('a1'); return false;"  onclick="tacPlayerSets('a1'); return false;" width="20%" height="24%" title="">
			    </td>
			    <td id="b1" class="ticTacToeTd" onmouseover="ticMouseOver('b1'); return false;"  onclick="tacPlayerSets('b1'); return false;" width="20%" height="24%" title="">
			    </td>
			    <td id="c1" class="ticTacToeTd" onmouseover="ticMouseOver('c1'); return false;"  onclick="tacPlayerSets('c1'); return false;" width="20%" height="24%" title="">
			    </td>
		    </tr>
            <tr id="tr0" class="ticTacToeTr">
                <td id="a0" class="ticTacToeTd" onmouseover="ticMouseOver('a0'); return false;" onclick="tacPlayerSets('a0'); return false;" width="20%" height="24%" title="">
                </td>
                <td id="b0" class="ticTacToeTd" onmouseover="ticMouseOver('b0'); return false;" onclick="tacPlayerSets('b0'); return false;" width="20%" height="24%" title="">
                </td>
                <td id="c0" class="ticTacToeTd" onmouseover="ticMouseOver('c0'); return false;" onclick="tacPlayerSets('c0'); return false;" width="20%" height="24%" title="">
                </td>
            </tr>
	    </table> 
    </div>
</asp:Content>
