<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Web.S.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>area23.at/s/ - another url shortner</title>
	<link rel="stylesheet" href="res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
	<script async src="res/js/area23.js"></script>
</head>
<body>
    <form id="form1" runat="server">
		<div align="left" class="contentDiv">
			<asp:LinkButton ID="LinkButton_UrlShorten" runat="server" CssClass="QrLinkButton" ToolTip="Enter url to shorten" 
				OnClick="LinkButton_UrlShorten_Click" Text="Shorten Url" Style="display: inline-block;">Shorten Url</asp:LinkButton>:
			<asp:TextBox ID="TextBox_UrlLong" runat="server" ToolTip="enter url www web site" AutoPostBack="True" OnTextChanged="TextBox_UrlLong_TextChanged"
				Width="128px" Height="24pt" AutoCompleteType="Homepage" CssClass="QRTextBoxLarge" />
		</div>
		<div align="left" class="contentDiv">
			<asp:LinkButton ID="LinkButton_UrlQr" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code shorten Url" 
				OnClick="LinkButton_UrlQr_Click" Text="Generate QrCode from Shorten Url" Style="display: inline-block;">Generate QrCode from Shorten Url</asp:LinkButton>:
			<asp:TextBox ID="TextBox_UrlShort" runat="server" ToolTip="shorten url" AutoPostBack="True" OnTextChanged="TextBox_UrlShort_TextChanged"
				Width="128px" Height="24pt" CssClass="QRTextBoxLarge"></asp:TextBox>
		</div>		
		<div align="left" class="contentDiv">
			<span class="lefthuge">				
				<input type="color" name="color1" id="color1" onchange="newQrColor(color1.value);" />&nbsp;
				<input id="input_color" alt="qr color" runat="server" name="selected_color" type="text" value="" size="7" />										
				<br />
				<input type="color" name="color0" id="color0" onchange="newBackgroundColor(color0.value);" />&nbsp;
				<input id="input_backcolor" alt="background color" runat="server" name="input_backcolor" type="text" value="" size="7" />										
				<br />
				<asp:Button id="Button_QRCode" name="Button_QRCode" runat="server" ClientIDMode="Static" 
					ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />				
			</span>
			<span class="righthuge">
				<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/qrsample2.png" Visible="false" BorderStyle="None" BackColor="Transparent" />
			</span>		
		</div>
		<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
		</div>
	</form> 
</body>
</html>
