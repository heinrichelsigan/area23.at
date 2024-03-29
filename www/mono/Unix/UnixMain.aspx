﻿<%@ Page Title="some unix cmd line shell tools (apache2 mod_mono)" Language="C#" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="UnixMain.aspx.cs" Inherits="Area23.At.Mono.Unix.UnixMain" %>
<asp:Content ID="UnixHeadContent" ContentPlaceHolderID="UnixHead" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <script async src="../res/js/digiclock.js"></script>
    <script>
		window.onload = function () {
            setDigiTime();
		}; 
    </script>
    <title>some unix cmd line shell tools (apache2 mod_mono)</title>    
</asp:Content>
<asp:Content ID="UnixBodyContent" ContentPlaceHolderID="UnixBody" runat="server">
    <form id="Area23UnixMain" runat="server">
        <div class="digitalclock" id="divDigitalTimeId" runat="server">
            <span class="digitalClockSpan" id="spanHoursId" name="spanHours" runat="server">00</span>:
            <span class="digitalClockSpan" id="spanMinutesId" name="spanMinutes" runat="server">00</span>:
            <span class="digitalClockSpan" id="spanSecondsId" name="spanSeconds" runat="server">00</span>
        </div> 
        <hr />
        <h2 id="h2Id" runat="server">unix cmd line app interfaces</h2>
        <ul>
            <li><a href="FortunAsp.aspx">fortune(6)</a></li>
            <li><a href="HexDump.aspx">hex dump od(1)</a></li>
            <li><a href="Bc.aspx">basic calculator bc(1)</a></li>
            <li><a name="netstat">netstat(8)</a></li>
            <li><a name="lsof">lsof(8)</a></li>
        </ul>
    </form>
</asp:Content>
