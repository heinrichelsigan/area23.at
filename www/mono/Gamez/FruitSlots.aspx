<%@ Page Title="" Language="C#" MasterPageFile="~/Gamez/GamesMaster.master" AutoEventWireup="true" CodeBehind="FruitSlots.aspx.cs" Inherits="Area23.At.Mono.Gamez.FruitSlots" %>

<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>FruitSlotMachine</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />	
	<script type="text/javascript">

		var machineStarted = 0; 
		var machineRunning = 0;

		// gets random fruit and random speed
		function getFruitByNumber(slotNumber) {
			var fruit = "";
            var appPath = "../res/gamez/fruitslotmachine/"
			const fruitNum = getRandomInt(1, 4);
			const slotSpeed = (slotNumber < 5) ? slotNumber : getRandomInt(slotNumber, slotNumber + 1); 
			switch (fruitNum) {
				case 1: fruit = appPath + "apple" + slotSpeed + ".gif"; break;
                case 2: fruit = appPath + "cherry" + slotSpeed + ".gif"; break;
                case 3: fruit = appPath + "melon" + slotSpeed + ".gif"; break;
                case 4: fruit = appPath + "orange" + slotSpeed + ".gif"; break;
				default: alert("No fruit availible for fruit number: " + fruitNum); break;
			}		
			return fruit;
		}
		
		// Returns a random integer between min (inclusive) and max (inclusive).	
		function getRandomInt(min, max) {
			min = Math.ceil(min);
			max = Math.floor(max);
			return Math.floor(Math.random() * (max - min + 1)) + min;
		}
		
		// playSound
		function playSound(soundName) {
			var dursec = 2500;		
			document.title = "Fruit Slot Machine";
			let sound = new Audio(soundName);
			sound.autoplay = true;
			sound.loop = false;
		
			setTimeout(function () { 
				try {
					sound.play(); 
				} catch (soundPlayEx) {
					console.log("playSound(soundName = " + soundName + ") throwed exception: " + soundPlayEx);
				}
			}, 100);

			setTimeout(function () {
				sound.loop = false;
				sound.pause();
				sound.autoplay = false;
				sound.currentTime = 0;
				try {
					sound.src = "";
					sound = null;
				} catch (exSnd) {
				}
				soundDuration = 2500;
			}, dursec);
		}

		// playSoundUrl
		function playSoundUrl(soundUrl, etext) {	
			if (soundUrl != null && soundUrl.length > 1) {
				setTimeout(function () { 
					setTextById(0, "spanId", etext);
					playSound(soundUrl) 
				}, 15000);
			}
		}

		function setTextById(mstate, docId, docText) {
			try { 
				machineRunning = mstate;
				if (document.getElementById(docId) != null) { 
					document.getElementById(docId).innerText = docText; 
				} 
			} catch (exText) {
				console.log("setTextById(" + mstate + ", " + docId + ", " + docText + ") throwed exception: " +  exText);
			}
		}


		// starts fruit slot machine
		function slotMachineStart() {		
			var fruitSlotId = "fruitSlot";	
            var fruitImage = "../res/gamez/fruitslotmachine/apple0.gif";
			
			if (machineRunning == 1) {
				try {
                    document.getElementById("machineTurn").src = "../res/gamez/fruitslotmachine/machineReady.gif";
					setTimeout(function() { setTextById(0, "spanId", "machine breaking"); }, 21000);          
				} catch (mRunExc) {
				}
				return; 
			}
			
			if (machineRunning == 0) {	
				setTextById(1, "spanId", "machine started");
				machineStarted++; 
				try {			
                    document.getElementById("machineTurn").src = "../res/gamez/fruitslotmachine/machineStart.gif";				
                    playSound("../res/gamez/audio/coin.wav");	
				} catch (mRunExc) {
				}					

				var ac = 0, cc = 0, mc = 0, oc = 0;
				
				var slcnt = 0;		
				while (slcnt < 4) {			
					fruitSlotId = "fruitSlot" + slcnt;
					try {
						if (document.getElementById(fruitSlotId) != null) {					
							
							let fruitImage = getFruitByNumber(slcnt);
							document.getElementById(fruitSlotId).src = fruitImage;
							document.getElementById(fruitSlotId).alt = "slot" + slcnt + " " + fruitImage;
							
							if (fruitImage.indexOf('apple') > -1) ac++;
							if (fruitImage.indexOf('cherry') > -1) cc++;
							if (fruitImage.indexOf('melon') > -1) mc++;	
							if (fruitImage.indexOf('orange') > -1) oc++;					
						}
					} catch (fruitSlotIterateEx) {
						console.log("fruitSlotMachineStart() iterate " + fruitSlotId + " " + fruitImage + " throwed exception: \n\r" +  fruitSlotIterateEx);
					}
					slcnt++;
				}
								
				var delayInMilliseconds = 1500; //1,5 second
				setTimeout(function() {
					try {
                        document.getElementById("machineTurn").src = "../res/gamez/fruitslotmachine/machineReady.gif";
						setTextById(1, "spanId", "machine running");
					} catch { }
					},  delayInMilliseconds);			
					
				if (ac == 4 || cc == 4 || mc == 4 || oc == 4) {
					var winMsg = "4x - YOU WIN AGAIN!";
					if (ac == 4)
						winMsg = "4x APPLE - SMART WIN, SUPERB!";
					if (cc == 4)
						winMsg = "4x CHERRY - SWEET WIN, MAKE A PICK!";
					if (mc == 4)
						winMsg = "4x MELON - TOPLESS WIN, NO BRA!";
					if (oc == 4)
						winMsg = "4x ORANGE - GREAT WIN, YOU TRUMP!";
                    playSoundUrl("../res/gamez/audio/four4.wav", winMsg);
				}
				else if ((ac == 2 && oc == 2) || (ac == 2 && cc == 2) || (cc == 2 && oc == 2)) 
                    playSoundUrl("../res/gamez/audio/two2.mp3", "2x pairs");
				else  
                    playSoundUrl("../res/gamez/audio/three3.mp3", "No luck, try again fruit slot machine");
			}
		}

		// mouse over slot machine turn image
		function slotMachineTurnOver() {		
			if (machineRunning == 1) {
				try {
					document.getElementById("lblRun").innerText = "slot machine is running";
				} catch { }
				setTimeout(function() { 
					try { 
                        document.getElementById("machineTurn").src = '../res/gamez/fruitslotmachine/machineReady.gif'; 
						document.getElementById("lblRun").innerText = "fruit slot machine";
					} catch { } 
				}, 2500);
				return ;
			}
			if (machineRunning == 0) { 
				try {
                    document.getElementById("machineTurn").src = '../res/gamez/fruitslotmachine/machineOver.gif';
				} catch { }
				if (machineStarted == 0) {	
					try {
						document.getElementById("spanId").innerText = "mouse click to start first time.";
					} catch (exTurnOVer) {
						console.log("slotMachineTurnOver() throwed exception: " + exTurnOVer);
					}		
				}		
			} 
		}

		// mouse out of slot machine turn image
		function slotMachineTurnOut() {
			try {
				document.getElementById("lblRun").innerText = "fruit slot machine";
			} catch { }
			setTimeout(function() {
				try {
                    document.getElementById("machineTurn").src = '../res/gamez/fruitslotmachine/machineReady.gif'; 
				} catch (exTurnOut) {
					console.log("slotMachineTurnOut() throwed exception: " + exTurnOut);
				}
			}, 750);			
		}
			
		// resets fruit slot machine
		function slotMachineReset() {
			try {
				machineRunning = 0;
				machineStarted = 0;
				setTextById(0, "spanId", "fruit slot machine reseted");
                document.getElementById("fruitSlot0").src = "../res/gamez/fruitslotmachine/apple.gif";
                document.getElementById("fruitSlot1").src = "../res/gamez/fruitslotmachine/cherry.gif";
                document.getElementById("fruitSlot2").src = "../res/gamez/fruitslotmachine/melon.gif";
                document.getElementById("fruitSlot3").src = "../res/gamez/fruitslotmachine/orange.gif";
                document.getElementById("machineTurn").src = "../res/gamez/fruitslotmachine/machineReady.gif";			
			} catch (ex) {
			
			}
		}	

    </script>
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server">     
    <noscript>Your browser does not support JavaScript!</noscript>
    <div style="padding: 2px; margin: 2px; border-color: green; border-style: double; border-width: 2px;" align="center">
		<table border="0" width="660px" height="360px" cellpadding="0" cellspacing="0">	
			<tr>
				<td colspan="4"  align="center" id="td1st"><span id="spanId" style='font-size: x-large; font-weight: bold; text-align: center;'> </span></td>
				<td width="180px"> 
					<button id="BtnStart" style='font-size: x-large' onclick="slotMachineStart(); return false;">Start</button>			
				</td>
			</tr>
			<tr height="360px">
				<td width="120px"> 
					<img id="fruitSlot0" src="../res/gamez/fruitslotmachine/apple.gif" width="120px" border="0" />
				</td>
				<td width="120px"> 
					<img id="fruitSlot1" src="../res/gamez/fruitslotmachine/cherry.gif" width="120px" border="0" />
				</td>
				<td width="120px"> 
					<img id="fruitSlot2" src="../res/gamez/fruitslotmachine/melon.gif" width="120px" border="0" />
				</td>		
				<td width="120px"> 
					<img id="fruitSlot3" src="../res/gamez/fruitslotmachine/orange.gif" width="120px" border="0" />
				</td>
				<td width="180px"> 
					<img id="machineTurn" src="../res/gamez/fruitslotmachine/machineReady.gif" width="180px" border="0"
						onmouseover="slotMachineTurnOver(); return false;" 
						onmouseout="slotMachineTurnOut(); return false;" 
						onclick="slotMachineStart(); return false;" />
				</td>
			</tr>
			<tr>
				<td width="120px" colspan="4">&nbsp;</td>
				<td width="180px"> 
					<button id="BtnReset" style='font-size: x-large' onclick="slotMachineReset(); return false;">Reset</button>
				</td>
			</tr>
		</table> 
    </div>
</asp:Content>
