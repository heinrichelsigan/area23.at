﻿<%@ Page Title="TicTacToe" Language="C#" MasterPageFile="~/Gamez/GamesMaster.master" AutoEventWireup="true" CodeBehind="TicTacToe.aspx.cs" Inherits="Area23.At.Mono.Gamez.TicTacToe" %>

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
            width: 96%;
            min-width: 384px;
            height: 84%;
            min-height: 336px;
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
                min-width: 36px;
                height: 20%;
                min-height: 27px;
                padding: 0 0 0 0;
                margin: 0 0 0 0;
                background-repeat: no-repeat;
                background-size: 100% 100%;
            }

            div.ticTacToe.ticTacToeHeader {
                width: 90%;
                min-width: 360px;
                height: 10%;
                min-height: 40px;
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
            min-height: 40px;
            width: 27%;
            min-width: 90px;
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
            min-width: 240px;
            height: 10%;
            min-height: 40px;
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
            min-height: 40px;
            width: 19.11%;
            min-width: 86px;
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
            min-width: 276px;
            height: 60%;
            min-height: 240px;
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
            min-width: 276px;
            height: 20%;
            min-height: 80px;

            background-color: #1f1f1f;
            background-repeat: no-repeat;
            /* background-size: 100% 100%; */

            border-color: blueviolet; 
            border-top-width: 1px; 
            border-top-style: outset;

        }

        td.ticTacToeTd {
            width: 23%;
            min-width: 92px; 
            height: 20%;
            min-height: 80px;         
            
            background-color: #1f1f1f;
            /* background-image: url('../res/img/emptyCellBlueTikTakToe.png'); */
            /* background-repeat: no-repeat; */
            background-size: 100% 100%;

            border-color: blueviolet; 
            border-top-width: 1px; 
            border-top-style: outset;             
        }

        img.frogsInImg {
            min-width: 23px;
            width: 46px;
            min-height: 20px;            
            height: 40px;         
    
            padding: 0 0 0 0;
            margin: 0 0 0 0;
    
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;

            background-repeat: no-repeat;
            background-size: 100% 100%;
        }

        td.ticTacToeTd.img.ticTacToeImage, 
        td.ticTacToeTd.ticTacToeImage, 
        img.ticTacToeImage,
        img.ticTacToeImage#ticTacToe0,
        img.ticTacToeImage#ticTacToe1,
        img.ticTacToeImage#ticTacToe2,
        img.ticTacToeImage#ticTacToe3 {
            width: 100%;            
            min-width: 92px;
            max-width: 276px;
            height: 100%;
            min-height: 80px;
            max-height: 240px;

            background-repeat: no-repeat;
            /* background-size: 100% 100%; */
        }

        
        
        img#aUp,
        img#aDown,
        img#aLeft,
        img#aRight {
            width: 120px;
            min-width: 92px;
            height: 115px;
            min-height: 80px;
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
            ticTacToeGame = ["0", "0", "0", "0", "0", "0", "0", "0", "0"],
            ticTacToeAndroid = [0, 0, 0, 0, 0, 0, 0, 0, 0],
            ticTacToePlayers = [0, 0, 0, 0, 0, 0, 0, 0, 0];

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
            const now = new Date(Date.now());
            seconds = now.getSeconds();
            whoStarts = (seconds % 2) + 1;
            ticTacToeLoad(whoStarts);
        }

        // ticTacToeReStart 
        // repeatLevel  true ... restart at level 0, false ... continue at next level
        function ticTacToeReStart(repeatLevel) {
            if (repeatLevel) {
                gameOver = 0;
                window.location.reload();
            } else { // TODO: fix this
                toeReCreate();

                if (whoStarts == 1)
                    whoStarts = 2;
                else if (whoStarts == 2)
                    whoStarts = 1;

                ticTacToeLoad(whoStarts);
            }
        }

        // ticTacToeLoad    => ticTacToeLoad game loader
        // starts   1 for computer, 2 for player
        function ticTacToeLoad(starts) {

            loopTicks = 0;
            androidCount = 5;
            playersCount = 5;
            whoNext = whoStarts;
            whoWins = -1;
            gameOver = 0;

            ticTacToeBoard = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"];
            ticTacToeGame = ["0", "0", "0", "0", "0", "0", "0", "0", "0"];
            ticTacToePlayers = [0, 0, 0, 0, 0, 0, 0, 0, 0];
            ticTacToeAndroid = [0, 0, 0, 0, 0, 0, 0, 0, 0];

            imgPlayer = getNewImage(2, "");
            imgComputer = getNewImage(1, "");

            switch (level) {
                case 0: loopDelay = 1625; break;
                case 1: loopDelay = 1500; break;
                default: loopDelay = 1250; break;
            }
            setLevel(level);
            setPlayersCounter(playersCount);
            setAndroidCounter(androidCount);

            var headerImg = document.getElementById("headerImg");

            if (whoStarts == 2) { // Player starts
                if (headerImg != null) {
                    document.getElementById("headerImg").src = "../res/img/headerTicTacToePlayer.gif";
                    document.getElementById("headerImg").focus();
                    document.getElementById("headerImg").blur();
                }
            }
            else if (whoStarts == 1) { // Computer starts           
                if (headerImg != null) {
                    document.getElementById("headerImg").src = "../res/img/headerTicTacToeComputer.gif";
                    document.getElementById("headerImg").focus();
                    document.getElementById("headerImg").blur();
                }
                // alert("Computer starts new Tic Tac Toe game,\nafter computers first android set\nclick on board or key pad 1-9 to set your skull.");
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
                    if (ticAlt != null && ticAlt.length > 0 && ticTacToeValue != null && ticTacToeValue.length > 0) {

                        if (ticAlt.charAt(0) == 'a') {
                            ticTacToeGame[toeCnt] = "a";
                            ticTacToeAndroid[toeCnt] = 1;
                        }
                        if (ticAlt.charAt(0) == 'p') {
                            ticTacToeGame[toeCnt] = "p";
                            ticTacToePlayers[toeCnt] = 2;
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

            var whoPlayedArray = null;
            if (whoPlayed == 1)
                whoPlayedArray = ticTacToeAndroid;
            else if (whoPlayed == 2)
                whoPlayedArray = ticTacToePlayers;

            whoWins = toeWhoWins(whoPlayed, whoPlayedArray);

            // level completed
            if (whoWins == 2) {
                headerImg.src = "../res/img/levelcompleted.gif"
                headerImg.height = 36;
                level++;
                soundDuration = 3600;
                setTimeout(function () { ticSound("../res/audio/levelCompleted.mp3") }, 100);
                setTimeout(function () { ticTacToeReStart(false); }, 4000); // will call the function after 8 secs.
                return whoWins;
            }
            else if (whoWins == 1) {
                headerImg.src = "../res/img/gameover.png";
                headerImg.height = 36;
                gameOver = 1;
                soundDuration = 4800;
                setTimeout(function () { ticSound("../res/audio/frogaGameOver.mp3") }, 100);
                setTimeout(function () { ticTacToeReStart(true); }, 5000); // will call the function after 8 secs.
                return whoWins;
            }
            if (whoWins == 0) {
                headerImg.src = "../res/img/ticTacToeDraw.gif"
                headerImg.height = 36;
                level++;
                soundDuration = 5000;
                setTimeout(function () { ticSound("../res/audio/aDraw.m4a") }, 100);
                setTimeout(function () { ticTacToeReStart(false); }, 6000); // will call the function after 8 secs.
                return whoWins;
            }

            loopTicks = ticks + 1;

            // setTimeout(function () { frogaLooper(loopTicks, delay) }, delay); // will call the function after 16 secs.
            return whoWins;
        }

        // checks if somebody already wins
        // whoPlayed    0 for computer, 1 for player
        // gameArray    ticTacToeAndroid for computer, ticTacToePlayers for player
        // return       -1 if game not finished or nobody wins game, 0 if computer won, 1 if player wins game
        function toeWhoWins(whoPlayed, gameArray) {

            if (gameArray == null || gameArray.length != 9)
                return -1;

            var tacTCellId = null;
            var toeTCellTd = null;
            var tValue = null;
            var matchCh = '';
            if (whoPlayed == 1)
                matchCh = 'a';
            else if (whoPlayed == 2)
                matchCh = 'p';

            let toeWin = -1;
            let toeCnt = -1;
            let g0 = -1, gdraw = -1, ga0 = -1, gb0 = -1, gc0 = -1, ga1 = -1, gb1 = -1, gc1 = -1, ga2 = -1, gb2 = -1, gc2 = -1;

            for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                tacTCellId = ticTacToeBoard[toeCnt];
                toeTCellTd = document.getElementById(tacTCellId);
                if (toeTCellTd != null) {
                    tValue = toeTCellTd.getAttribute("ticTacToe");
                    if (tValue != null && tValue.charAt(0) == matchCh) {
                        alert(tacTCellId + ":\t" + tValue);
                        if (toeCnt == 0)
                            ga0 = 1;
                        else if (toeCnt == 1)
                            gb0 = 1;
                        else if (toeCnt == 2)
                            gc0 = 1;
                        else if (toeCnt == 3)
                            ga1 = 1;
                        else if (toeCnt == 4)
                            gb1 = 1;
                        else if (toeCnt == 5)
                            gc1 = 1;
                        else if (toeCnt == 6)
                            ga2 = 1;
                        else if (toeCnt == 7)
                            gb2 = 1;
                        else if (toeCnt == 8)
                            gc2 = 1;
                    }
                }
            }

            for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                if (ticTacToeGame[toeCnt].charAt(0) == matchCh) {
                    if (toeCnt == 0)
                        ga0 = 1;
                    else if (toeCnt == 1)
                        gb0 = 1;
                    else if (toeCnt == 2)
                        gc0 = 1;
                    else if (toeCnt == 3)
                        ga1 = 1;
                    else if (toeCnt == 4)
                        gb1 = 1;
                    else if (toeCnt == 5)
                        gc1 = 1;
                    else if (toeCnt == 6)
                        ga2 = 1;
                    else if (toeCnt == 7)
                        gb2 = 1;
                    else if (toeCnt == 8)
                        gc2 = 1;
                }
            }

            for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                if (ticTacToeGame[toeCnt].charAt(0) != '0') {
                    gdraw++;
                }
                else if (ticTacToeGame[toeCnt].charAt(0) == '0') {
                    g0 = 0;
                }
            }

            if (whoPlayed == 1) {
                for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                    if (ticTacToeAndroid[toeCnt] > 0) {
                        if (toeCnt == 0)
                            ga0 = 1;
                        else if (toeCnt == 1)
                            gb0 = 1;
                        else if (toeCnt == 2)
                            gc0 = 1;
                        else if (toeCnt == 3)
                            ga1 = 1;
                        else if (toeCnt == 4)
                            gb1 = 1;
                        else if (toeCnt == 5)
                            gc1 = 1;
                        else if (toeCnt == 6)
                            ga2 = 1;
                        else if (toeCnt == 7)
                            gb2 = 1;
                        else if (toeCnt == 8)
                            gc2 = 1;
                    }
                }
            }
            if (whoPlayed == 2) {
                for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                    if (ticTacToePlayers[toeCnt] > 0) {
                        if (toeCnt == 0)
                            ga0 = 1;
                        else if (toeCnt == 1)
                            gb0 = 1;
                        else if (toeCnt == 2)
                            gc0 = 1;
                        else if (toeCnt == 3)
                            ga1 = 1;
                        else if (toeCnt == 4)
                            gb1 = 1;
                        else if (toeCnt == 5)
                            gc1 = 1;
                        else if (toeCnt == 6)
                            ga2 = 1;
                        else if (toeCnt == 7)
                            gb2 = 1;
                        else if (toeCnt == 8)
                            gc2 = 1;
                    }
                }
            }

            if (gdraw >= 8 && g0 < 0)
                toeWin = 0;

            if ((ga0 == 1 && gb0 == 1 && gc0 == 1) ||
                (ga1 == 1 && gb1 == 1 && gc1 == 1) ||
                (ga2 == 1 && gb2 == 1 && gc2 == 1) ||
                (ga0 == 1 && ga1 == 1 && ga2 == 1) ||
                (gb0 == 1 && gb1 == 1 && gb2 == 1) ||
                (gc0 == 1 && gc1 == 1 && gc2 == 1) ||
                (ga0 == 1 && gb1 == 1 && gc2 == 1) ||
                (ga2 == 1 && gb1 == 1 && gc0 == 1)) {

                toeWin = whoPlayed;

                var arrayToMark = null;
                if (ga0 == 1 && gb0 == 1 && gc0 == 1)
                    arrayToMark = ["a0", "b0", "c0"];
                else if (ga1 == 1 && gb1 == 1 && gc1 == 1)
                    arrayToMark = ["a1", "b1", "c1"];
                else if (ga2 == 1 && gb2 == 1 && gc2 == 1)
                    arrayToMark = ["a2", "b2", "c2"];
                else if (ga0 == 1 && ga1 == 1 && ga2 == 1)
                    arrayToMark = ["a0", "a1", "a2"];
                else if (gb0 == 1 && gb1 == 1 && gb2 == 1)
                    arrayToMark = ["b0", "b1", "b2"];
                else if (gc0 == 1 && gc1 == 1 && gc2 == 1)
                    arrayToMark = ["c0", "c1", "c2"];
                else if (ga0 == 1 && gb1 == 1 && gc2 == 1)
                    arrayToMark = ["a0", "b1", "c2"];
                else if (gc0 == 1 && gb1 == 1 && ga2 == 1)
                    arrayToMark = ["a2", "b1", "c0"];

                if (arrayToMark != null) {
                    markCells(arrayToMark);
                }

                //alert(ga2 + "\t" + gb2 + "\t" + gc2 + "\n" + ga1 + "\t" + gb1 + "\t" + gc1 + "\n" + ga0 + "\t" + gb0 + "\t" + gc0 + "\n\n" +
                //    ticTacToeGame[6] + "\t" + ticTacToeGame[7] + "\t" + ticTacToeGame[8] + "\n" +
                //    ticTacToeGame[3] + "\t" + ticTacToeGame[4] + "\t" + ticTacToeGame[5] + "\n" +
                //    ticTacToeGame[0] + "\t" + ticTacToeGame[1] + "\t" + ticTacToeGame[2] + "\n");

            }

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

        // mark cells 
        // cellsToMark  array of cells to mark
        function markCells(cellsToMark) {

            var ticTacToeTd = null;
            var ticTacToeCellId = "";
            let cMarkCnt = 0;
            if (cellsToMark != null) {

                try {
                    for (cMarkCnt = 0; cMarkCnt < 3; cMarkCnt++) {

                        ticTacToeCellId = cellsToMark[cMarkCnt];
                        ticTacToeTd = document.getElementById(ticTacToeCellId);

                        if (ticTacToeTd != null) {
                            // alert("making cell id " + ticTacToeCellId + "!");
                            ticTacToeTd.style.backgroundColor = "#3f3f3f";
                            ticTacToeTd.style.borderWidth = 2;
                            ticTacToeTd.style.borderStyle = "solid";
                        }
                    }
                } catch (cellsToMarkException) {
                    alert("Exception on maring cells: " + cellsToMarkException);
                }
            }
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

        // tacPlayerSets
        //          ticTacToe player sets skull on board
        // tdCellId id of table cell td
        function tacPlayerSets(tdCellId) {

            var cellTicTacToe = null;
            var tacNum = -1;
            var cellAlt = null;
            var playCellTd = document.getElementById(tdCellId);
            if (playCellTd != null) {

                try {
                    if (playCellTd.alt != null && playCellTd.alt != "" && playCellTd.alt.length > 0)
                        cellAlt = playCellTd.alt;
                } catch (exGetTitle) {
                    cellAlt = null;
                }
                try {
                    if (cellAlt == null)
                        cellAlt = playCellTd.getAttribute("alt");
                } catch (exGetAlt) {
                    cellAlt = null;
                }
            }

            cellTicTacToe = playCellTd.getAttribute("ticTacToe");
            if (cellTicTacToe != null && cellAlt != null && cellTicTacToe.length > 0 && cellAlt.length > 0) {
                alert("Can't set players skull here!");
            } else {

                cleanTableCell(playCellTd);

                imgPlayer = getNewImage(2, tdCellId);
                imgSkull = copyImg(imgPlayer);
                playCellTd.appendChild(imgSkull);
                playCellTd.setAttribute("ticTacToe", imgPlayer.id);
                playCellTd.alt = imgPlayer.id;
                tacNum = mapCellTdToArrayIndex(tdCellId);

                ticTacToePlayers[tacNum] = 1;
                ticTacToeGame[tacNum] = "p";

                if (playersCount > 0) {
                    playersCount--;
                    setPlayersCounter(playersCount);
                    imgPlayer = getNewImage(2, "");
                }


                if (toeFinished(loopTicks, 2) > -1) {
                    // TODO: mark winning cells
                    return;
                }

                whoNext = 1;
                tacComputerSets(loopTicks);
            }

            ticClearMarkedCells(tdCellId);
        }

        // tacComputerSets
        //          ticTacToe computer sets android on board
        // ticks    game ticks
        function tacComputerSets(ticks) {

            var ticTacToeTd = null;
            let toeCnt = -1;
            var tacCellId = null;
            var toeCellId = null;
            var toeTCell = null;
            imgComputer = getNewImage(1, "");
            tacCalculateGamblerArrays();

            if (loopTicks < 4 && (ticTacToeGame[4] != null && ticTacToeGame[4].charAt(0) == '0'))
                toeCellId = "b1";

            if (toeCellId == null || toeCellId.charAt(0) != 'b') {
                // stupid ai for computer to find free field
                if (ticTacToeGame[0].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[8].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[8];
                else if (ticTacToeGame[0].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[8].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[0];
                else if (ticTacToeGame[0].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == '0' && ticTacToeGame[8].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[4];
                else if (ticTacToeGame[3].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[6].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[6];
                else if (ticTacToeGame[3].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[6].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[3];
                else if (ticTacToeGame[3].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == '0' && ticTacToeGame[6].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[4];
                else if (ticTacToeGame[0].charAt(0) == 'a' && ticTacToeGame[1].charAt(0) == 'a' && ticTacToeGame[2].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[2];
                else if (ticTacToeGame[0].charAt(0) == '0' && ticTacToeGame[1].charAt(0) == 'a' && ticTacToeGame[2].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[0];
                else if (ticTacToeGame[0].charAt(0) == 'a' && ticTacToeGame[1].charAt(0) == '0' && ticTacToeGame[2].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[1];
                else if (ticTacToeGame[3].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[5].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[5];
                else if (ticTacToeGame[3].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[5].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[3];
                else if (ticTacToeGame[6].charAt(0) == 'a' && ticTacToeGame[7].charAt(0) == 'a' && ticTacToeGame[8].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[8];
                else if (ticTacToeGame[6].charAt(0) == 'a' && ticTacToeGame[7].charAt(0) == '0' && ticTacToeGame[8].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[7];
                else if (ticTacToeGame[6].charAt(0) == '0' && ticTacToeGame[7].charAt(0) == 'a' && ticTacToeGame[8].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[6];
                else if (ticTacToeGame[0].charAt(0) == 'a' && ticTacToeGame[3].charAt(0) == 'a' && ticTacToeGame[6].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[6];
                else if (ticTacToeGame[0].charAt(0) == 'a' && ticTacToeGame[3].charAt(0) == '0' && ticTacToeGame[6].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[3];
                else if (ticTacToeGame[0].charAt(0) == '0' && ticTacToeGame[3].charAt(0) == 'a' && ticTacToeGame[6].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[0];
                else if (ticTacToeGame[1].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[7].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[7];
                else if (ticTacToeGame[1].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'a' && ticTacToeGame[7].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[1];
                else if (ticTacToeGame[1].charAt(0) == 'a' && ticTacToeGame[4].charAt(0) == '0' && ticTacToeGame[7].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[4];
                else if (ticTacToeGame[2].charAt(0) == 'a' && ticTacToeGame[5].charAt(0) == 'a' && ticTacToeGame[8].charAt(0) == '0')
                    toeCellId = ticTacToeBoard[8];
                else if (ticTacToeGame[2].charAt(0) == '0' && ticTacToeGame[5].charAt(0) == 'a' && ticTacToeGame[8].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[8];
                else if (ticTacToeGame[2].charAt(0) == 'a' && ticTacToeGame[5].charAt(0) == '0' && ticTacToeGame[8].charAt(0) == 'a')
                    toeCellId = ticTacToeBoard[5];

                if (toeCellId == null || (toeCellId.charAt(0) != 'a' && toeCellId.charAt(0) != 'b' && toeCellId.charAt(0) != 'c')) {

                    if (ticTacToeGame[0].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[8].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[8];
                    else if (ticTacToeGame[0].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[8].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[0];
                    else if (ticTacToeGame[0].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == '0' && ticTacToeGame[8].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[4];
                    else if (ticTacToeGame[3].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[6].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[6];
                    else if (ticTacToeGame[3].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[6].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[3];
                    else if (ticTacToeGame[3].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == '0' && ticTacToeGame[6].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[4];
                    else if (ticTacToeGame[0].charAt(0) == 'p' && ticTacToeGame[1].charAt(0) == 'p' && ticTacToeGame[2].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[2];
                    else if (ticTacToeGame[0].charAt(0) == '0' && ticTacToeGame[1].charAt(0) == 'p' && ticTacToeGame[2].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[0];
                    else if (ticTacToeGame[0].charAt(0) == 'p' && ticTacToeGame[1].charAt(0) == '0' && ticTacToeGame[2].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[1];
                    else if (ticTacToeGame[3].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[5].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[5];
                    else if (ticTacToeGame[3].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[5].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[3];
                    else if (ticTacToeGame[6].charAt(0) == 'p' && ticTacToeGame[7].charAt(0) == 'p' && ticTacToeGame[8].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[8];
                    else if (ticTacToeGame[6].charAt(0) == 'p' && ticTacToeGame[7].charAt(0) == '0' && ticTacToeGame[8].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[7];
                    else if (ticTacToeGame[6].charAt(0) == '0' && ticTacToeGame[7].charAt(0) == 'p' && ticTacToeGame[8].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[6];
                    else if (ticTacToeGame[0].charAt(0) == 'p' && ticTacToeGame[3].charAt(0) == 'p' && ticTacToeGame[6].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[6];
                    else if (ticTacToeGame[0].charAt(0) == 'p' && ticTacToeGame[3].charAt(0) == '0' && ticTacToeGame[6].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[3];
                    else if (ticTacToeGame[0].charAt(0) == '0' && ticTacToeGame[3].charAt(0) == 'p' && ticTacToeGame[6].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[0];
                    else if (ticTacToeGame[1].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[7].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[7];
                    else if (ticTacToeGame[1].charAt(0) == '0' && ticTacToeGame[4].charAt(0) == 'p' && ticTacToeGame[7].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[1];
                    else if (ticTacToeGame[1].charAt(0) == 'p' && ticTacToeGame[4].charAt(0) == '0' && ticTacToeGame[7].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[4];
                    else if (ticTacToeGame[2].charAt(0) == 'p' && ticTacToeGame[5].charAt(0) == 'p' && ticTacToeGame[8].charAt(0) == '0')
                        toeCellId = ticTacToeBoard[8];
                    else if (ticTacToeGame[2].charAt(0) == '0' && ticTacToeGame[5].charAt(0) == 'p' && ticTacToeGame[8].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[8];
                    else if (ticTacToeGame[2].charAt(0) == 'p' && ticTacToeGame[5].charAt(0) == '0' && ticTacToeGame[8].charAt(0) == 'p')
                        toeCellId = ticTacToeBoard[5];
                }

                for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                    if (toeCellId == null && ticTacToeGame[toeCnt] == "0" && ticTacToeGame[toeCnt].charAt(0) != 'a' && ticTacToeGame[toeCnt].charAt(0) != 'p') {
                        toeCellId = ticTacToeBoard[toeCnt];
                    }
                }


            }

            if (toeCellId != null) {

                var droidCellTd = document.getElementById(toeCellId);
                if (droidCellTd != null) {

                    cleanTableCell(droidCellTd);

                    imgComputer = getNewImage(1, toeCellId);

                    imgAndroid = copyImg(imgComputer);
                    droidCellTd.appendChild(imgAndroid);
                    droidCellTd.setAttribute("ticTacToe", imgAndroid.id);
                    droidCellTd.alt = imgAndroid.id;
                    tacNum = mapCellTdToArrayIndex(toeCellId);

                    ticTacToeAndroid[tacNum] = 1;
                    ticTacToeGame[tacNum] = "a";


                    if (androidCount > 0) {
                        androidCount--;
                        setAndroidCounter(androidCount);
                        imgComputer = getNewImage(1, "");
                    }

                    if (toeFinished(loopTicks, 1) > 0) {
                        return;
                    }

                    whoNext = 2;
                }
                else  // what shell we do with the drunken sailor
                    alert("droidCellTd is NULL!\nWhat shell we do with the drunken sailor?");
            }
            else  // what shell we do with the drunken sailor
                alert("toeCellId is NULL!\nWhat shell we do with the drunken sailor?");
        }

        // setPlayersCounter
        //          set players remaining skull discs to set
        // playersCnt   remaining player stones to set
        function setPlayersCounter(playersCnt) {
            var spanplayersCounter = document.getElementById("playersCounter");
            if (spanplayersCounter != null)
                spanplayersCounter.innerText = playersCnt;
        }

        // setAndroidCounter
        //          set computer remaining android stickers to set
        // androidCnt   remaining android to set   
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


        // getNewImage  
        //          gets a new image to set at next move
        // forWho   1 ... computer, 2 ... player
        // aCellId  table cell id (identifier)
        function getNewImage(forWho, aCellId) {

            var ticImg = new Image(23, 20);
            var ticNum;
            var ticId;
            var ticTitle = "";
            var ticImgSrc;
            var cellIdSet = "";

            if (forWho == 1) {
                ticNum = androidCount;
                ticId = "a" + ticNum;
                ticImgSrc = "../res/img/androidSetTicTacToe.gif";
            } else if (forWho == 2) {
                ticNum = playersCount;
                ticId = "p" + ticNum;
                ticImgSrc = "../res/img/skullSetTicTacToe.gif";
            }

            if (aCellId != null && aCellId.charAt(0) != '\0' && aCellId.length > 1) {

                if ((aCellId.charAt(0) == 'a' || aCellId.charAt(0) == 'b' || aCellId.charAt(0) == 'c') &&
                    (aCellId.charAt(1) == '0' || aCellId.charAt(1) == '1' || aCellId.charAt(1) == '2'))
                    cellIdSet = aCellId;
            }

            if (forWho == 1) {
                ticImgSrc = "../res/img/androidSetTicTacToe.gif";
                ticNum = androidCount;
                ticId = "a" + ticNum;
                ticTitle = "android" + ticNum;
            }
            else if (forWho == 2) {
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

        // getEmptyImage  
        //          gets a new image to set at next move
        // aCellId  table cell id (identifier)
        function getEmptyImage(aCellId) {

            var tacImg = new Image(23, 20);
            var tacNum = -1;
            var tacImgSrc = "../res/img/emptyCellTicTacToe.gif";
            var tacId = "";

            if (aCellId != null) {

                tacNum = mapCellTdToArrayIndex(aCellId);
                tacId = aCellId;
            }

            tacImg.Id = tacId;
            tacImg.src = tacImgSrc;
            tacImg.alt = "";
            tacImg.setAttribute("border", 0);
            tacImg.setAttribute("class", "ticTacToeImage");
            tacImg.setAttribute("className", "ticTacToeImage");

            return tacImg;
        }


        // mapCellTdToArrayIndex    
        //              maps table cell identifier to array index
        // anyCellTd    table cell identifier
        function mapCellTdToArrayIndex(anyCellTd) {
            var tacNum = -1;
            if (anyCellTd != null) {
                if (anyCellTd.charAt(0) == 'a')
                    tacNum = 0;
                if (anyCellTd.charAt(0) == 'b')
                    tacNum = 1;
                if (anyCellTd.charAt(0) == 'c')
                    tacNum = 2;
                if (anyCellTd.charAt(1) == '1')
                    tacNum = tacNum + 3;
                if (anyCellTd.charAt(1) == '2')
                    tacNum = tacNum + 6;
            }

            return tacNum;
        }

        // mapArrayIndexToCellTd 
        //          maps array index to table cell id(identifier)
        // arrIdx   array index
        function mapArrayIndexToCellTd(arrIdx) {
            var retCellTd = null;
            switch (arrIdx) {
                case 0: retCellTd = "a0"; break;
                case 1: retCellTd = "b0"; break;
                case 2: retCellTd = "c0"; break;
                case 3: retCellTd = "a1"; break;
                case 4: retCellTd = "b1"; break;
                case 5: retCellTd = "c1"; break;
                case 6: retCellTd = "a2"; break;
                case 7: retCellTd = "b2"; break;
                case 8: retCellTd = "c2"; break;
                default: retCellTd = null; break;
            }
            return retCellTd;
        }


        // ticSound     plays soundName
        // soundName    sound name to play
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

        // cleanTableCell 
        //          removes cilds from table cell td
        // aTCell   specified table cell td
        function cleanTableCell(aTCell) {

            if (aTCell != null && aTCell.children != null && aTCell.children.length > 0) {

                var tdCellIt = 0;
                for (tdCellIt = 0; tdCellIt < aTCell.children.length; tdCellIt++) {

                    var childFromTCell = aTCell.children[tdCellIt];
                    if (childFromTCell != null)
                        aTCell.removeChild(childFromTCell);
                }
            }
        }


        // toeReCreate 
        //          clears are finished tic tac toe board
        function toeReCreate() {

            var tableCell = null;
            var childFromTableCell = null;
            var tCellTdId = null;
            var nCellTdId = null;
            var nTableCell = null;
            var nImg = null;
            var tdsToClear = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"];

            tdsToClear.forEach(function (tCellTdId) {

                tableCell = document.getElementById(tCellTdId);
                tableCell.setAttribute("ticTacToe", "");
                tableCell.alt = "";
                tableCell.style.backgroundColor = "#1f1f1f";
                tableCell.style.borderWidth = 1;
                tableCell.style.borderStyle = "outset";
                cleanTableCell(tableCell);
            });

            let tCnt = -1;
            for (tCnt = 0; tCnt < 9; tCnt++) {

                nCellTdId = ticTacToeBoard[tCnt];
                nTableCell = document.getElementById(nCellTdId)

                if (nTableCell != null) {
                    nImg = getEmptyImage(nCellTdId);
                    nTableCell.appendChild(nImg);
                    nTableCell.setAttribute("ticTacToe", "");
                    nTableCell.alt = "";
                    nTableCell.style.backgroundColor = "#1f1f1f";
                    nTableCell.style.borderWidth = 1;
                    nTableCell.style.borderStyle = "outset";
                }
            }

        }


        // clone    simple clones an object
        // obj      object to clone
        // returns  cloned object
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

        // copyImg
        //          copies an image to another
        // imgSource    source image
        // returns      destination image copied from source image
        function copyImg(imgSource) {

            var imgDestination = new Image();

            if (imgSource != null && imgSource.id != null) {

                imgDestination.id = imgSource.id;
                imgDestination.src = imgSource.src;
                imgDestination.width = imgSource.width;
                imgDestination.height = imgSource.height;

                if (imgSource.alt != null)
                    imgDestination.alt = imgSource.alt;

                if (imgSource.getAttribute("title") != null)
                    imgDestination.setAttribute("title", imgSource.getAttribute("title"));
                if (imgSource.getAttribute("className") != null)
                    imgDestination.setAttribute("className", imgSource.getAttribute("className"));
                if (imgSource.getAttribute("class") != null)
                    imgDestination.setAttribute("class", imgSource.getAttribute("class"));
                if (imgSource.getAttribute("cellid") != null)
                    imgDestination.setAttribute("cellid", imgSource.getAttribute("cellid"));
                if (imgSource.getAttribute("ticTacToe") != null)
                    imgDestination.setAttribute("ticTacToe", imgSource.getAttribute("ticTacToe"));

                if (imgSource.getAttribute("border") != null)
                    imgDestination.setAttribute("border", imgSource.getAttribute("border"));
                else
                    imgDestination.setAttribute("border", 0);

            }

            return imgDestination;
        }

        // replaceImg
        //      replaces imgOrig with imgFrom, copies all attributes and removes imgFrom from document
        // imgOrig  original image
        // imgForm  image that will replace origImage
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
                if (imgFrom.getAttribute("ticTacToe") != null)
                    imgOrig.setAttribute("ticTacToe", imgFrom.getAttribute("ticTacToe"));
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
			    <td id="a2" class="ticTacToeTd" onmouseover="ticMouseOver('a2'); return false;" onclick="tacPlayerSets('a2'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="ticTacToeImage" id="a20" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
			    <td id="b2" class="ticTacToeTd" onmouseover="ticMouseOver('b2'); return false;" onclick="tacPlayerSets('b2'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="ticTacToeImage" id="b20" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
			    <td id="c2" class="ticTacToeTd" onmouseover="ticMouseOver('c2'); return false;" onclick="tacPlayerSets('c2'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="ticTacToeImage" id="c20" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
		    </tr>
		    <tr id="t1" class="ticTacToeTr">
			    <td id="a1" class="ticTacToeTd" onmouseover="ticMouseOver('a1'); return false;"  onclick="tacPlayerSets('a1'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="ticTacToeImage" id="a10" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
			    <td id="b1" class="ticTacToeTd" onmouseover="ticMouseOver('b1'); return false;"  onclick="tacPlayerSets('b1'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="ticTacToeImage" id="b10" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
			    <td id="c1" class="ticTacToeTd" onmouseover="ticMouseOver('c1'); return false;"  onclick="tacPlayerSets('c1'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="ticTacToeImage" id="c10" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
		    </tr>
            <tr id="t0" class="ticTacToeTr">
                <td id="a0" class="ticTacToeTd" onmouseover="ticMouseOver('a0'); return false;" onclick="tacPlayerSets('a0'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
                    <img class="ticTacToeImage" id="a00" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
                <td id="b0" class="ticTacToeTd" onmouseover="ticMouseOver('b0'); return false;" onclick="tacPlayerSets('b0'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
                    <img class="ticTacToeImage" id="b00" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
                <td id="c0" class="ticTacToeTd" onmouseover="ticMouseOver('c0'); return false;" onclick="tacPlayerSets('c0'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
                    <img class="ticTacToeImage" id="c00" src='../res/img/emptyCellTicTacToe.gif' border="0" />
                </td>
            </tr>
	    </table> 
    </div>
</asp:Content>
