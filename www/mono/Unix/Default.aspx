<%@ Page Title="some unix cmd line shell tools (apache2 mod_mono)" Language="C#" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.Unix.Default" %>
<asp:Content ID="UnixHeadContent" ContentPlaceHolderID="UnixHead" runat="server" ClientIDMode="Static">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <script src="../res/js/area23.js"></script>
    <script>
		window.onload = function () {
            try {
                SetTimeDigital();
            } catch (exDigiTime) {
                console.log(`Exception ' + exDigiTime + ' on executing function SetTimeDigital() => now calling setDigiTime().`);
                setDigiTime();
            }  
            try {
                InitTimeDigital();
            } catch (exDigiTime) {
                console.log(`Exception ' + exDigiTime + ' on executing function InitTimeDigital() => now calling initDigiTime().`);
                initDigiTime();
            }  
            setTimeout(function () { setDigiTime() }, 900);
        }; 

        function setDigiTime() {

            try {
                InitTimeDigital();
            } catch (exDigiTime) {
                console.log(`Exception on executing function InitTimeDigital() => now calling initDigiTime().`);
                initDigiTime();
            }  
                        
            setTimeout(function () { setDigiTime() }, 900);
        }

        function initDigiTime() {

            var hours, minutes, seconds;
            var digiYear, digiMonth, digiDay, digiTime, digiHours, digiMinutes, digiSeconds;

            const now = new Date(Date.now());
            seconds = now.getSeconds();
            now.getMilliseconds
            digiSeconds = (seconds < 10) ? "0" + seconds : seconds + "";
            minutes = now.getMinutes();
            digiMinutes = (minutes < 10) ? ("0" + minutes) : (minutes + "");
            hours = now.getHours();
            digiHours = (hours < 10) ? ("0" + hours) : (hours + "");

            digiTime = digiHours + ":" + digiMinutes + ":" + digiSeconds;

            var hoursId = "spanHoursId";
            if (document.getElementById(hoursId) != null) {
                document.getElementById(hoursId).innerText = digiHours;
            } else {
                const hourNames = document.getElementsByName("spanHours");
                hoursId = hourNames[0].id;
                if (document.getElementById(hoursId) != null) {
                    document.getElementById(hoursId).innerText = digiHours;
                }
            }

            var minutesId = "spanMinutesId";
            if (document.getElementById(minutesId) != null) {
                document.getElementById(minutesId).innerText = digiMinutes;
            } else {
                const minutesNames = document.getElementsByName("spanMinutes");
                minutesId = minutesNames[0].id;
                if (document.getElementById(minutesId) != null) {
                    document.getElementById(minutesId).innerText = digiMinutes;
                }
            }

            var secondsId = "spanSecondsId";
            if (document.getElementById(secondsId) != null) {
                document.getElementById(secondsId).innerText = digiSeconds;
            } else {
                const secondsNames = document.getElementsByName("spanSeconds");
                secondsId = secondsNames[0].id;
                if (document.getElementById(secondsId) != null) {
                    document.getElementById(secondsId).innerText = digiSeconds;
                }
            }

            if (seconds == 0) {
                if (minutes == 0) {                    
                    ReloadForm();
                    alert("Alert each full hour: " + digiTime);
                    return;
                }                
            }

            console.log(`Digital time: ${digiTime}`);
            return digiTime;
        }


    </script>
    <title>some unix cmd line shell tools (apache2 mod_mono)</title>    
</asp:Content>
<asp:Content ID="UnixBodyContent" ContentPlaceHolderID="UnixBody" ClientIDMode="Static" runat="server">
    <form id="Area23UnixMain" runat="server">
        <div class="digitalclock" id="divDigitalTimeId" runat="server">
            <span 
                class="digitalClockSpan" id="spanHoursId" name="spanHours" runat="server">00</span>:<span 
                class="digitalClockSpan" id="spanMinutesId" name="spanMinutes" runat="server">00</span>:<span 
                    class="digitalClockSpan" id="spanSecondsId" name="spanSeconds" runat="server">00</span>
        </div>
        <hr />
        <h2 id="h2Id" runat="server">unix cmd line app interfaces</h2>
        <ul>
            <li><a href="Bc.aspx">basic calculator bc(1)</a></li>
            <li><a href="Fortune.aspx">fortune</a> <a href="https://area23.at/shared/man/fortune.txt" target="_blank">fortune(6)</a></li>
            <li><a href="HostWhois.aspx" name="HostWhois">hostwhois</a> <a href="https://area23.at/shared/man/whois.txt" target="_blank">whois (1)</a></li>
            <li><a href="OctalDump.aspx">octaldump</a> <a href="https://www.man7.org/linux/man-pages/man1/od.1.html" target="_blank">od(1)</a></li>            
            <li><a href="RsaDsaGen.aspx">generate asymmetric rsa/dsa keys</a> with <a href="https://man.openbsd.org/openssl.1" target="_blank">openssl(1)</a></li>
            <li><a href="PdfMerge.aspx" name="PdfMerge">PdfMerge pdfunite(1)</a></li>
            <li><a href="PdfUnite.aspx" name="PdfUnite">PdfUnite pdfunite(1)</a></li>            
        </ul>
    </form>
</asp:Content>
