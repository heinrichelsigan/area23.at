<%@ Page Language="C#"  MasterPageFile="~/Qr/QRMaster.master" AutoEventWireup="true" CodeBehind="S.aspx.cs" Inherits="Area23.At.Mono.Qr.S" %>
<asp:Content ID="QrSHeadContent" ContentPlaceHolderID="QrHead" runat="server">
	<title>QrUrl Shortener (apache2 mod_mono)</title>
	<link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="keywords" content="QR Url shorten tiny small uri" />
	<meta name="author" content="Heinrich Elsigan" />
	<meta name="description" content="https://github.com/heinrichelsigan/area23.at/tree/main/www/Area23.At.Www.S" />
	<script async src="../res/js/area23.js"></script>
	<script>
		window.onload = function () {
			// initColorPickers();
            colorpicker = document.getElementById("color1");
            if (colorpicker == null)
                colorpicker = document.getElementsByName("color1")[0]

            inputcolor = document.getElementById("input_color");
            if (inputcolor == null)
                inputcolor = document.getElementsByName("input_color")[0]

            if (colorpicker != null && inputcolor != null && inputcolor.value != null && inputcolor.value != "" && inputcolor.value.length >= 6) {
                colorpicker.value = inputcolor.value;
            }

            backcolorpicker = document.getElementById("color0");
            if (backcolorpicker == null)
                backcolorpicker = document.getElementsByName("color0")[0]

            inputbackcolor = document.getElementById("input_backcolor");
            if (inputbackcolor == null)
                inputbackcolor = document.getElementsByName("input_backcolor")[0];

            if (backcolorpicker != null && inputbackcolor != null && inputbackcolor.value != null && inputbackcolor.value != "") {
                // if (backcolorpicker.value == inputbackcolor.value)
                //     return;
                backcolorpicker.value = inputbackcolor.value;
            }
        }; 
    </script>
</asp:Content>
<asp:Content ID="QrSBodyContent" ContentPlaceHolderID="QrBody" runat="server">
	<form id="formQrS" runat="server">
		<div align="left" class="contentDiv">
			<asp:TextBox ID="TextBox_UrlLong" runat="server" ToolTip="enter url www web site" AutoPostBack="True" OnTextChanged="TextBox_UrlLong_TextChanged"
				Width="75%" Height="24pt" AutoCompleteType="Homepage" CssClass="QRTextBoxSmall" />&nbsp;
			<asp:Button id="Button_QRCode" name="Button_QRCode" runat="server" ClientIDMode="Static" 
				ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />	
		</div>
		<div class="contentDiv">
			<span class="leftSpan">
				<input type="color" name="color1" id="color1" onchange="newQrColor(color1.value);" />
				&nbsp;
				<input id="input_color" alt="qr color" runat="server" name="input_color" type="text" value="" size="7" />						
			</span>
			<span class="centerSpan">
				<input type="color" name="color0" id="color0" onchange="newBackgroundColor(color0.value);" />
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
			<asp:TextBox ID="TextBoxShortenUrl" runat="server" Width="75%" Height="24pt" ToolTip="shorten utl" Enabled="false" Visible="false" CssClass="QRTextBoxSmall" />&nbsp;
		</div>
		<div align="left" class="contentDiv">
			<a id="HrefShort" runat="server" class="Url_Short" href="#" target="_blank" name="UrlShort" visible="false"></a>				
		</div>
		<div align="left" class="contentDiv">
			<span class="lefthuge" id="hugeLeftId" runat="server">
				<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/img/qrsample2.png" Visible="false" BorderStyle="None" BackColor="Transparent" />
			</span>
			<span class="righthuge">
				<img id="imQrInverse" runat="server" src="~/res/img/qrsample1.png" alt="Qr Code" visible="false" style="border-style: none; background-color: transparent" />
			</span>
		</div>		
		<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
		</div>
	</form> 
</asp:Content>