﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Www.U.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>area23.at/s/ - another url shortner</title>
	<link rel="stylesheet" href="res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="author" content="Heinrich Elsigan" />
	<meta name="description" content="https://github.com/heinrich.elsigan/" />
	<script async src="res/js/area23.js"></script>
	<script>
		window.onload = function () {
			setColorPicker();
        }; 
    </script>
</head>
<body>
    <form id="form1" runat="server">
		<div align="left" class="contentDiv">
			<asp:TextBox ID="TextBox_UrlLong" runat="server" ToolTip="enter url www web site" AutoPostBack="True" OnTextChanged="TextBox_UrlLong_TextChanged"
				Width="75%" Height="24pt" AutoCompleteType="Homepage" CssClass="QRTextBoxSmall" />&nbsp;
			<asp:Button id="Button_QRCode" name="Button_QRCode" runat="server" ClientIDMode="Static" 
				ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />	
		</div>
		<div class="contentDiv">
			<span class="leftSpan">
				<span class="textSpan"><input type="color" name="color1" id="color1" onchange="newQrColor(color1.value);" /></span>
				&nbsp;
				<input id="input_color" alt="qr color" runat="server" name="selected_color" type="text" value="" size="7" />						
			</span>
			<span class="centerSpan">
				<span class="textSpan"><input type="color" name="color0" id="color0" onchange="newBackgroundColor(color0.value);" /></span>
				<input id="input_backcolor" alt="background color" runat="server" name="input_backcolor" type="text" value="" size="7" />				
			</span>
			<span class="centerSpan">
				<span class="textSpan">qr mode: </span>
				<asp:DropDownList ID="DropDown_QrMode" runat="server" ToolTip="qr mode" AutoPostBack="True" OnSelectedIndexChanged="DropDown_QrMode_Changed" CssClass="DropDownList">
					<asp:ListItem>L</asp:ListItem>
					<asp:ListItem>M</asp:ListItem>				
					<asp:ListItem Selected="True">Q</asp:ListItem>
					<asp:ListItem>H</asp:ListItem>
				</asp:DropDownList>
			</span>
			<span class="rightSpan">
				<span class="textSpan"> pixel per unit: </span>
				<asp:DropDownList ID="DropDown_PixelPerUnit" runat="server" ToolTip="pixel / unit in qr image" AutoPostBack="True" OnSelectedIndexChanged="DropDown_PixelPerUnit_Changed" CssClass="DropDownList">
					<asp:ListItem>1</asp:ListItem>
					<asp:ListItem>2</asp:ListItem>
					<asp:ListItem>3</asp:ListItem>
					<asp:ListItem Selected="True">4</asp:ListItem>
					<asp:ListItem>6</asp:ListItem>
					<asp:ListItem>8</asp:ListItem>
				</asp:DropDownList>
			</span>
		</div>
		<div align="left" class="contentDiv">
			<span class="lefthuge" id="hugeLeftId" runat="server">
				<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/img/qrsample2.png" Visible="false" BorderStyle="None" BackColor="Transparent" />
			</span>
			<span class="righthuge">
				<a id="HrefShort" runat="server" class="Url_Short" href="#" target="_blank" name="UrlShort" visible="false"></a>				
			</span>
		</div>		
		<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
		</div>
	</form> 
</body>
</html>
