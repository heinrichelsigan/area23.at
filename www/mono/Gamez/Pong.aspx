<%@ Page Title="" Language="C#" MasterPageFile="~/Gamez/GamesMaster.master" AutoEventWireup="true" CodeBehind="Pong.aspx.cs" Inherits="Area23.At.Mono.Gamez.Pong" %>

<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Pong</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<style>
        body.pongBody {
            color: white;
            background-color: black;
            padding: 0 0 0 0;
            margin: 0 0 0 0;
            border-style: none;
            border-spacing: 0pt;
            border-width: 0pt;
            border-color: transparent;
        }

        div.pong {
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

            div.pong.img {
                width: 20%;
                min-width: 36px;
                height: 20%;
                min-height: 27px;
                padding: 0 0 0 0;
                margin: 0 0 0 0;
                background-repeat: no-repeat;
                background-size: 100% 100%;
            }

            div.pong.pongHeader {
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

        div.pongHeader#headerLeft {
            height: 10%;
            min-height: 40px;
            width: 27%;
            min-width: 90px;
            vertical-align: middle;
            font-size: larger;
            text-align: right;
        }

        div.pongHeader#headerCenter {
            height: 10%;
            min-height: 45pt;
            width: 64%;
            min-width: 297pt;
            vertical-align: middle;
            font-size: medium;
            text-align: center;
        }

        div.pongHeader.headerImage {
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

        div.pongHeader#headerRight {
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

        div.pong.pongFooter#spanLeft {
            min-width: 32px;
            width: 40px;
            min-height: 27px;    
            height: 36px;

            vertical-align: middle;
            text-align: left;
        }

        div.pong.pongFooter#spanRight {
            min-width: 32px;
            width: 40px;
            min-height: 27px;    
            height: 36px;
            
            vertical-align: middle;
            text-align: right
        }


        table.pongTable {
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

        tr.pongTr {
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

        td.pongTd {
            width: 23%;
            min-width: 92px; 
            height: 20%;
            min-height: 80px;         
            
            background-color: #1f1f1f;
            /* background-image: url('../res/gamez/pong/emptyCellBlueTikTakToe.png'); */
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

        td.pongTd.img.pongImage, 
        td.pongTd.pongImage, 
        img.pongImage,
        img.pongImage#pong0,
        img.pongImage#pong1,
        img.pongImage#pong2,
        img.pongImage#pong3 {
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
        var pongBoard = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"],
            pongGame = ["0", "0", "0", "0", "0", "0", "0", "0", "0"],
            pongAndroid = [0, 0, 0, 0, 0, 0, 0, 0, 0],
            pongPlayers = [0, 0, 0, 0, 0, 0, 0, 0, 0];

        var imgPlayer, imgSkull, imgComputer, imgAndroid;

        window.onload = function () {
            pongInit();
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

        // pongInit will be called on 1st time loading
        function pongInit() {
            windowCursorKeysHandler();
            const now = new Date(Date.now());
            seconds = now.getSeconds();
            whoStarts = (seconds % 2) + 1;
            pongLoad(whoStarts);
        }

        // pongReStart 
        // repeatLevel  true ... restart at level 0, false ... continue at next level
        function pongReStart(repeatLevel) {
            if (repeatLevel) {
                gameOver = 0;
                window.location.reload();
            } else { // TODO: fix this
                toeReCreate();

                if (whoStarts == 1)
                    whoStarts = 2;
                else if (whoStarts == 2)
                    whoStarts = 1;

                pongLoad(whoStarts);
            }
        }

        // pongLoad    => pongLoad game loader
        // starts   1 for computer, 2 for player
        function pongLoad(starts) {

            loopTicks = 0;
            androidCount = 5;
            playersCount = 5;
            whoNext = whoStarts;
            whoWins = -1;
            gameOver = 0;

            pongBoard = ["a0", "b0", "c0", "a1", "b1", "c1", "a2", "b2", "c2"];
            pongGame = ["0", "0", "0", "0", "0", "0", "0", "0", "0"];
            pongPlayers = [0, 0, 0, 0, 0, 0, 0, 0, 0];
            pongAndroid = [0, 0, 0, 0, 0, 0, 0, 0, 0];

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
                    document.getElementById("headerImg").src = "../res/gamez/pong/headerPongPlayer.gif";
                    document.getElementById("headerImg").focus();
                    document.getElementById("headerImg").blur();
                }
            }
            else if (whoStarts == 1) { // Computer starts           
                if (headerImg != null) {
                    document.getElementById("headerImg").src = "../res/gamez/pong/headerPongComputer.gif";
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
            var pongTd = null;
            var pongCellId = null;
            pongBoard.forEach(function (pongCellId) {

                toeCnt++;
                pongTd = document.getElementById(pongCellId);
                if (pongTd != null) {

                    var ticAlt = pongTd.alt;
                    var pongValue = pongTd.getAttribute("pong");
                    if (ticAlt != null && ticAlt.length > 0 && pongValue != null && pongValue.length > 0) {

                        if (ticAlt.charAt(0) == 'a') {
                            pongGame[toeCnt] = "a";
                            pongAndroid[toeCnt] = 1;
                        }
                        if (ticAlt.charAt(0) == 'p') {
                            pongGame[toeCnt] = "p";
                            pongPlayers[toeCnt] = 2;
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
                whoPlayedArray = pongAndroid;
            else if (whoPlayed == 2)
                whoPlayedArray = pongPlayers;

            whoWins = toeWhoWins(whoPlayed, whoPlayedArray);

            // level completed
            if (whoWins == 2) {
                headerImg.src = "../res/gamez/pong/levelcompleted.gif"
                headerImg.height = 36;
                level++;
                soundDuration = 3600;
                setTimeout(function () { ticSound("../res/gamez/audio/levelCompleted.mp3") }, 100);
                setTimeout(function () { pongReStart(false); }, 4000); // will call the function after 8 secs.
                return whoWins;
            }
            else if (whoWins == 1) {
                headerImg.src = "../res/gamez/pong/gameover.png";
                headerImg.height = 36;
                gameOver = 1;
                soundDuration = 4800;
                setTimeout(function () { ticSound("../res/gamez/audio/frogaGameOver.mp3") }, 100);
                setTimeout(function () { pongReStart(true); }, 5000); // will call the function after 8 secs.
                return whoWins;
            }
            if (whoWins == 0) {
                headerImg.src = "../res/gamez/pong/pongDraw.gif"
                headerImg.height = 36;
                level++;
                soundDuration = 5000;
                setTimeout(function () { ticSound("../res/gamez/audio/aDraw.m4a") }, 100);
                setTimeout(function () { pongReStart(false); }, 6000); // will call the function after 8 secs.
                return whoWins;
            }

            loopTicks = ticks + 1;

            // setTimeout(function () { frogaLooper(loopTicks, delay) }, delay); // will call the function after 16 secs.
            return whoWins;
        }

        // checks if somebody already wins
        // whoPlayed    0 for computer, 1 for player
        // gameArray    pongAndroid for computer, pongPlayers for player
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
                tacTCellId = pongBoard[toeCnt];
                toeTCellTd = document.getElementById(tacTCellId);
                if (toeTCellTd != null) {
                    tValue = toeTCellTd.getAttribute("pong");
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
                if (pongGame[toeCnt].charAt(0) == matchCh) {
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
                if (pongGame[toeCnt].charAt(0) != '0') {
                    gdraw++;
                }
                else if (pongGame[toeCnt].charAt(0) == '0') {
                    g0 = 0;
                }
            }

            if (whoPlayed == 1) {
                for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                    if (pongAndroid[toeCnt] > 0) {
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
                    if (pongPlayers[toeCnt] > 0) {
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
                //    pongGame[6] + "\t" + pongGame[7] + "\t" + pongGame[8] + "\n" +
                //    pongGame[3] + "\t" + pongGame[4] + "\t" + pongGame[5] + "\n" +
                //    pongGame[0] + "\t" + pongGame[1] + "\t" + pongGame[2] + "\n");

            }

            return toeWin;
        }


        // clear marked cells 
        function ticClearMarkedCells(tdCellId) {
            var pongTd = null;
            let pongCellId = "";
            pongBoard.forEach(function (pongCellId) {

                pongTd = document.getElementById(pongCellId);

                if (pongTd != null) {
                    pongTd.style.backgroundColor = "#1f1f1f";
                    pongTd.style.borderWidth = 1;
                    pongTd.style.borderStyle = "outset";
                }
            });
        }

        // mark cells 
        // cellsToMark  array of cells to mark
        function markCells(cellsToMark) {

            var pongTd = null;
            var pongCellId = "";
            let cMarkCnt = 0;
            if (cellsToMark != null) {

                try {
                    for (cMarkCnt = 0; cMarkCnt < 3; cMarkCnt++) {

                        pongCellId = cellsToMark[cMarkCnt];
                        pongTd = document.getElementById(pongCellId);

                        if (pongTd != null) {
                            // alert("making cell id " + pongCellId + "!");
                            pongTd.style.backgroundColor = "#3f3f3f";
                            pongTd.style.borderWidth = 2;
                            pongTd.style.borderStyle = "solid";
                        }
                    }
                } catch (cellsToMarkException) {
                    alert("Exception on maring cells: " + cellsToMarkException);
                }
            }
        }


        // ticMouseOver marks on mouse over the blured cell on tic tac toe board
        function ticMouseOver(tdCellId) {

            var cellPongOver = null;
            ticClearMarkedCells(tdCellId);
            cellPongOver = document.getElementById(tdCellId);

            if (cellPongOver != null) {

                cellPongOver.style.backgroundColor = "#3f3f3f";
                cellPongOver.style.borderWidth = 2;
                cellPongOver.style.borderStyle = "solid";
            }
        }

        // tacPlayerSets
        //          pong player sets skull on board
        // tdCellId id of table cell td
        function tacPlayerSets(tdCellId) {

            var cellPong = null;
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

            cellPong = playCellTd.getAttribute("pong");
            if (cellPong != null && cellAlt != null && cellPong.length > 0 && cellAlt.length > 0) {
                alert("Can't set players skull here!");
            } else {

                cleanTableCell(playCellTd);

                imgPlayer = getNewImage(2, tdCellId);
                imgSkull = copyImg(imgPlayer);
                playCellTd.appendChild(imgSkull);
                playCellTd.setAttribute("pong", imgPlayer.id);
                playCellTd.alt = imgPlayer.id;
                tacNum = mapCellTdToArrayIndex(tdCellId);

                pongPlayers[tacNum] = 1;
                pongGame[tacNum] = "p";

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
        //          pong computer sets android on board
        // ticks    game ticks
        function tacComputerSets(ticks) {

            var pongTd = null;
            let toeCnt = -1;
            var tacCellId = null;
            var toeCellId = null;
            var toeTCell = null;
            imgComputer = getNewImage(1, "");
            tacCalculateGamblerArrays();

            if (loopTicks < 4 && (pongGame[4] != null && pongGame[4].charAt(0) == '0'))
                toeCellId = "b1";

            if (toeCellId == null || toeCellId.charAt(0) != 'b') {
                // stupid ai for computer to find free field
                if (pongGame[0].charAt(0) == 'a' && pongGame[4].charAt(0) == 'a' && pongGame[8].charAt(0) == '0')
                    toeCellId = pongBoard[8];
                else if (pongGame[0].charAt(0) == '0' && pongGame[4].charAt(0) == 'a' && pongGame[8].charAt(0) == 'a')
                    toeCellId = pongBoard[0];
                else if (pongGame[0].charAt(0) == 'a' && pongGame[4].charAt(0) == '0' && pongGame[8].charAt(0) == 'a')
                    toeCellId = pongBoard[4];
                else if (pongGame[3].charAt(0) == 'a' && pongGame[4].charAt(0) == 'a' && pongGame[6].charAt(0) == '0')
                    toeCellId = pongBoard[6];
                else if (pongGame[3].charAt(0) == '0' && pongGame[4].charAt(0) == 'a' && pongGame[6].charAt(0) == 'a')
                    toeCellId = pongBoard[3];
                else if (pongGame[3].charAt(0) == 'a' && pongGame[4].charAt(0) == '0' && pongGame[6].charAt(0) == 'a')
                    toeCellId = pongBoard[4];
                else if (pongGame[0].charAt(0) == 'a' && pongGame[1].charAt(0) == 'a' && pongGame[2].charAt(0) == '0')
                    toeCellId = pongBoard[2];
                else if (pongGame[0].charAt(0) == '0' && pongGame[1].charAt(0) == 'a' && pongGame[2].charAt(0) == 'a')
                    toeCellId = pongBoard[0];
                else if (pongGame[0].charAt(0) == 'a' && pongGame[1].charAt(0) == '0' && pongGame[2].charAt(0) == 'a')
                    toeCellId = pongBoard[1];
                else if (pongGame[3].charAt(0) == 'a' && pongGame[4].charAt(0) == 'a' && pongGame[5].charAt(0) == '0')
                    toeCellId = pongBoard[5];
                else if (pongGame[3].charAt(0) == '0' && pongGame[4].charAt(0) == 'a' && pongGame[5].charAt(0) == 'a')
                    toeCellId = pongBoard[3];
                else if (pongGame[6].charAt(0) == 'a' && pongGame[7].charAt(0) == 'a' && pongGame[8].charAt(0) == '0')
                    toeCellId = pongBoard[8];
                else if (pongGame[6].charAt(0) == 'a' && pongGame[7].charAt(0) == '0' && pongGame[8].charAt(0) == 'a')
                    toeCellId = pongBoard[7];
                else if (pongGame[6].charAt(0) == '0' && pongGame[7].charAt(0) == 'a' && pongGame[8].charAt(0) == 'a')
                    toeCellId = pongBoard[6];
                else if (pongGame[0].charAt(0) == 'a' && pongGame[3].charAt(0) == 'a' && pongGame[6].charAt(0) == '0')
                    toeCellId = pongBoard[6];
                else if (pongGame[0].charAt(0) == 'a' && pongGame[3].charAt(0) == '0' && pongGame[6].charAt(0) == 'a')
                    toeCellId = pongBoard[3];
                else if (pongGame[0].charAt(0) == '0' && pongGame[3].charAt(0) == 'a' && pongGame[6].charAt(0) == 'a')
                    toeCellId = pongBoard[0];
                else if (pongGame[1].charAt(0) == 'a' && pongGame[4].charAt(0) == 'a' && pongGame[7].charAt(0) == '0')
                    toeCellId = pongBoard[7];
                else if (pongGame[1].charAt(0) == '0' && pongGame[4].charAt(0) == 'a' && pongGame[7].charAt(0) == 'a')
                    toeCellId = pongBoard[1];
                else if (pongGame[1].charAt(0) == 'a' && pongGame[4].charAt(0) == '0' && pongGame[7].charAt(0) == 'a')
                    toeCellId = pongBoard[4];
                else if (pongGame[2].charAt(0) == 'a' && pongGame[5].charAt(0) == 'a' && pongGame[8].charAt(0) == '0')
                    toeCellId = pongBoard[8];
                else if (pongGame[2].charAt(0) == '0' && pongGame[5].charAt(0) == 'a' && pongGame[8].charAt(0) == 'a')
                    toeCellId = pongBoard[8];
                else if (pongGame[2].charAt(0) == 'a' && pongGame[5].charAt(0) == '0' && pongGame[8].charAt(0) == 'a')
                    toeCellId = pongBoard[5];

                if (toeCellId == null || (toeCellId.charAt(0) != 'a' && toeCellId.charAt(0) != 'b' && toeCellId.charAt(0) != 'c')) {

                    if (pongGame[0].charAt(0) == 'p' && pongGame[4].charAt(0) == 'p' && pongGame[8].charAt(0) == '0')
                        toeCellId = pongBoard[8];
                    else if (pongGame[0].charAt(0) == '0' && pongGame[4].charAt(0) == 'p' && pongGame[8].charAt(0) == 'p')
                        toeCellId = pongBoard[0];
                    else if (pongGame[0].charAt(0) == 'p' && pongGame[4].charAt(0) == '0' && pongGame[8].charAt(0) == 'p')
                        toeCellId = pongBoard[4];
                    else if (pongGame[3].charAt(0) == 'p' && pongGame[4].charAt(0) == 'p' && pongGame[6].charAt(0) == '0')
                        toeCellId = pongBoard[6];
                    else if (pongGame[3].charAt(0) == '0' && pongGame[4].charAt(0) == 'p' && pongGame[6].charAt(0) == 'p')
                        toeCellId = pongBoard[3];
                    else if (pongGame[3].charAt(0) == 'p' && pongGame[4].charAt(0) == '0' && pongGame[6].charAt(0) == 'p')
                        toeCellId = pongBoard[4];
                    else if (pongGame[0].charAt(0) == 'p' && pongGame[1].charAt(0) == 'p' && pongGame[2].charAt(0) == '0')
                        toeCellId = pongBoard[2];
                    else if (pongGame[0].charAt(0) == '0' && pongGame[1].charAt(0) == 'p' && pongGame[2].charAt(0) == 'p')
                        toeCellId = pongBoard[0];
                    else if (pongGame[0].charAt(0) == 'p' && pongGame[1].charAt(0) == '0' && pongGame[2].charAt(0) == 'p')
                        toeCellId = pongBoard[1];
                    else if (pongGame[3].charAt(0) == 'p' && pongGame[4].charAt(0) == 'p' && pongGame[5].charAt(0) == '0')
                        toeCellId = pongBoard[5];
                    else if (pongGame[3].charAt(0) == '0' && pongGame[4].charAt(0) == 'p' && pongGame[5].charAt(0) == 'p')
                        toeCellId = pongBoard[3];
                    else if (pongGame[6].charAt(0) == 'p' && pongGame[7].charAt(0) == 'p' && pongGame[8].charAt(0) == '0')
                        toeCellId = pongBoard[8];
                    else if (pongGame[6].charAt(0) == 'p' && pongGame[7].charAt(0) == '0' && pongGame[8].charAt(0) == 'p')
                        toeCellId = pongBoard[7];
                    else if (pongGame[6].charAt(0) == '0' && pongGame[7].charAt(0) == 'p' && pongGame[8].charAt(0) == 'p')
                        toeCellId = pongBoard[6];
                    else if (pongGame[0].charAt(0) == 'p' && pongGame[3].charAt(0) == 'p' && pongGame[6].charAt(0) == '0')
                        toeCellId = pongBoard[6];
                    else if (pongGame[0].charAt(0) == 'p' && pongGame[3].charAt(0) == '0' && pongGame[6].charAt(0) == 'p')
                        toeCellId = pongBoard[3];
                    else if (pongGame[0].charAt(0) == '0' && pongGame[3].charAt(0) == 'p' && pongGame[6].charAt(0) == 'p')
                        toeCellId = pongBoard[0];
                    else if (pongGame[1].charAt(0) == 'p' && pongGame[4].charAt(0) == 'p' && pongGame[7].charAt(0) == '0')
                        toeCellId = pongBoard[7];
                    else if (pongGame[1].charAt(0) == '0' && pongGame[4].charAt(0) == 'p' && pongGame[7].charAt(0) == 'p')
                        toeCellId = pongBoard[1];
                    else if (pongGame[1].charAt(0) == 'p' && pongGame[4].charAt(0) == '0' && pongGame[7].charAt(0) == 'p')
                        toeCellId = pongBoard[4];
                    else if (pongGame[2].charAt(0) == 'p' && pongGame[5].charAt(0) == 'p' && pongGame[8].charAt(0) == '0')
                        toeCellId = pongBoard[8];
                    else if (pongGame[2].charAt(0) == '0' && pongGame[5].charAt(0) == 'p' && pongGame[8].charAt(0) == 'p')
                        toeCellId = pongBoard[8];
                    else if (pongGame[2].charAt(0) == 'p' && pongGame[5].charAt(0) == '0' && pongGame[8].charAt(0) == 'p')
                        toeCellId = pongBoard[5];
                }

                for (toeCnt = 0; toeCnt < 9; toeCnt++) {
                    if (toeCellId == null && pongGame[toeCnt] == "0" && pongGame[toeCnt].charAt(0) != 'a' && pongGame[toeCnt].charAt(0) != 'p') {
                        toeCellId = pongBoard[toeCnt];
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
                    droidCellTd.setAttribute("pong", imgAndroid.id);
                    droidCellTd.alt = imgAndroid.id;
                    tacNum = mapCellTdToArrayIndex(toeCellId);

                    pongAndroid[tacNum] = 1;
                    pongGame[tacNum] = "a";


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

        // sets level number inside span id=pongLevel
        function setLevel(tacToeLevel) {

            var levelSpan = document.getElementById("pongLevel");

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
            var pongImgSrc;
            var cellIdSet = "";

            if (forWho == 1) {
                pongNum = androidCount;
                pongId = "a" + pongNum;
                pongImgSrc = "../res/gamez/pong/androidSetPong.gif";
            } else if (forWho == 2) {
                pongNum = playersCount;
                pongId = "p" + pongNum;
                pongImgSrc = "../res/gamez/pong/skullSetPong.gif";
            }

            if (aCellId != null && aCellId.charAt(0) != '\0' && aCellId.length > 1) {

                if ((aCellId.charAt(0) == 'a' || aCellId.charAt(0) == 'b' || aCellId.charAt(0) == 'c') &&
                    (aCellId.charAt(1) == '0' || aCellId.charAt(1) == '1' || aCellId.charAt(1) == '2'))
                    cellIdSet = aCellId;
            }

            if (forWho == 1) {
                pongImgSrc = "../res/gamez/pong/androidSetPong.gif";
                pongNum = androidCount;
                pongId = "a" + pongNum;
                pongTitle = "android" + pongNum;
            }
            else if (forWho == 2) {
                pongImgSrc = "../res/gamez/pong/skullSetPong.gif";
                pongNum = playersCount;
                pongId = "p" + pongNum;
                pongTitle = "players" + pongNum;
            }

            pongImg.Id = pongId;
            pongImg.alt = pongId;
            pongImg.src = pongImgSrc
            pongImg.setAttribute("border", 0);
            pongImg.setAttribute("title", pongTitle);
            pongImg.setAttribute("class", "pongImage");
            pongImg.setAttribute("className", "pongImage");
            pongImg.setAttribute("cellid", cellIdSet);

            return pongImg;
        }

        // getEmptyImage  
        //          gets a new image to set at next move
        // aCellId  table cell id (identifier)
        function getEmptyImage(aCellId) {

            var tacImg = new Image(23, 20);
            var tacNum = -1;
            var tacImgSrc = "../res/gamez/pong/emptyCellPong.gif";
            var tacId = "";

            if (aCellId != null) {

                tacNum = mapCellTdToArrayIndex(aCellId);
                tacId = aCellId;
            }

            tacImg.Id = tacId;
            tacImg.src = tacImgSrc;
            tacImg.alt = "";
            tacImg.setAttribute("border", 0);
            tacImg.setAttribute("class", "pongImage");
            tacImg.setAttribute("className", "pongImage");

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


        // pongSound     plays soundName
        // soundName    sound name to play
        function pongSound(soundName) {

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
        //          clears are finished pong tac toe board
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
                tableCell.setAttribute("pong", "");
                tableCell.alt = "";
                tableCell.style.backgroundColor = "#1f1f1f";
                tableCell.style.borderWidth = 1;
                tableCell.style.borderStyle = "outset";
                cleanTableCell(tableCell);
            });

            let tCnt = -1;
            for (tCnt = 0; tCnt < 9; tCnt++) {

                nCellTdId = pongBoard[tCnt];
                nTableCell = document.getElementById(nCellTdId)

                if (nTableCell != null) {
                    nImg = getEmptyImage(nCellTdId);
                    nTableCell.appendChild(nImg);
                    nTableCell.setAttribute("pong", "");
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
                if (imgSource.getAttribute("pong") != null)
                    imgDestination.setAttribute("pong", imgSource.getAttribute("pong"));

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
                if (imgFrom.getAttribute("pong") != null)
                    imgOrig.setAttribute("pong", imgFrom.getAttribute("pong"));
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
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server">     
    <noscript>Your browser does not support JavaScript!</noscript>
    <div class="pong" align="center">
	    <div class="pongHeader">
            <span id="headerLeft" align="right" valign="middle">
                <span id="pongLevel">0</span>
                <span id="leftNotes"></span>
                <img class="frogsInImg" src="../res/gamez/pong/skullPong.gif" /><span id="playersCounter" alt="players counter">5</span>
                &nbsp;
            </span>
		    <span id="headerCenter" align="center" valign="middle">
			    <img class="headerImage" src="../res/gamez/pong/headerPong.png" id="headerImg" border="0" onclick="restart()" />
		    </span>
		    <span id="headerRight" align="left" valign="middle">
			    &nbsp;<img class="frogsInImg" src="../res/gamez/pong/androidPong.gif" /><span id="androidCounter" alt="android counter">5</span>
		    </span>
            <span id="rightNotes"></span>
	    </div>
	    <table class="pongTable" border="1" cellpadding="1" cellpadding="1">
		    <tr id="t2" class="pongTr">
			    <td id="a2" class="pongTd" onmouseover="pongMouseOver('a2'); return false;" onclick="tacPlayerSets('a2'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="pongImage" id="a20" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
			    <td id="b2" class="pongTd" onmouseover="pongMouseOver('b2'); return false;" onclick="tacPlayerSets('b2'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="pongImage" id="b20" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
			    <td id="c2" class="pongTd" onmouseover="pongMouseOver('c2'); return false;" onclick="tacPlayerSets('c2'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="pongImage" id="c20" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
		    </tr>
		    <tr id="t1" class="pongTr">
			    <td id="a1" class="pongTd" onmouseover="pongMouseOver('a1'); return false;"  onclick="tacPlayerSets('a1'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="pongImage" id="a10" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
			    <td id="b1" class="pongTd" onmouseover="pongMouseOver('b1'); return false;"  onclick="tacPlayerSets('b1'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="pongImage" id="b10" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
			    <td id="c1" class="pongTd" onmouseover="pongMouseOver('c1'); return false;"  onclick="tacPlayerSets('c1'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
			        <img class="pongImage" id="c10" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
		    </tr>
            <tr id="t0" class="pongTr">
                <td id="a0" class="pongTd" onmouseover="pongMouseOver('a0'); return false;" onclick="tacPlayerSets('a0'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
                    <img class="pongImage" id="a00" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
                <td id="b0" class="pongTd" onmouseover="pongMouseOver('b0'); return false;" onclick="tacPlayerSets('b0'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
                    <img class="pongImage" id="b00" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
                <td id="c0" class="pongTd" onmouseover="ticMouseOver('c0'); return false;" onclick="tacPlayerSets('c0'); return false;" width="20%" height="24%" title="" align="center" valign="middle">
                    <img class="pongImage" id="c00" src='../res/gamez/pong/emptyCellPong.gif' border="0" />
                </td>
            </tr>
	    </table> 
    </div>
</asp:Content>
