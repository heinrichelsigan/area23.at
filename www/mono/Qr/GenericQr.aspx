<%@ Page Title="QR Code generator (apache2 mod_mono)" Language="C#" MasterPageFile="~/Qr/QRMaster.master" AutoEventWireup="true" CodeBehind="GenericQr.aspx.cs" Inherits="Area23.At.Mono.Qr.GenericQr" %>
<asp:Content ID="ContentQrHead" ContentPlaceHolderID="QrHead" runat="server">
	<title>Generic Qr Generator (apache2 mod_mono)</title>
	<link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<meta name="keywords" content="generic qr code generator" />
	<meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
	<script type="text/javascript" src="../res/js/qr.js"></script>
	<script type="text/javascript">

        var buttonQRCode, inputcolor, inputbackcolor, colorpicker, backcolorpicker;

        window.onload = function () {
            try {
                setColorPicker();
                return;
            } catch (ex) {
                console.log("Exception: " + ex);
            }
            //try {
            //    setColorPick();
            //} catch (ex) {
            //    console.log("Exception: " + ex);
            //}
        };

        function setColorPick() {
            colorpicker = document.getElementById("color1");
            inputcolor = document.getElementById("input_color");
            if (colorpicker != null && inputcolor != null && inputcolor.value != null && inputcolor.value != "" && inputcolor.value.length >= 6) {
                if (colorpicker.value == inputcolor.value) {
                    ;
                }
                else
                    colorpicker.value = inputcolor.value;
            }
            backcolorpicker = document.getElementById("color0");
            inputbackcolor = document.getElementById("input_backcolor");
            if (backcolorpicker != null && inputbackcolor != null && inputbackcolor.value != null && inputbackcolor.value.length >= 6) {
                // if (backcolorpicker.value == inputbackcolor.value)
                //     return;
                backcolorpicker.value = inputbackcolor.value;
            }
        }


        function newQrColor(color) {
            try {
                if (document.getElementById("input_color") != null) {
                    inputcolor = document.getElementById("input_color");
                    inputcolor.setAttribute("Text", color);
                    inputcolor.setAttribute("qrcolor", color);
                    inputcolor.value = color;
                    inputcolor.style.borderColor = color;
                    inputcolor.style.textColor = color;
                }

                if (document.getElementById("Button_QRCode") != null) {
                    buttonQRCode = document.getElementById("Button_QRCode");
                    buttonQRCode.setAttribute("qrcolor", color);
                    buttonQRCode.style.borderColor = color;
                    buttonQRCode.style.textColor = color;
                    document.getElementById("Button_QRCode").click();
                }
            } catch (exCol) {
                console.log("newQrColor(color = " + color + ") throwed at getElementsById('input_color') " + exCol);
            }
        }

        function newBackgroundColor(bgcolor) { // document.bgColor = color;            
            try {
                if (document.getElementById("input_backcolor") != null) {
                    inputbackcolor = document.getElementById("input_backcolor");
                    inputbackcolor.setAttribute("Text", bgcolor);
                    inputbackcolor.setAttribute("qrcolor", bgcolor);
                    inputbackcolor.value = bgcolor;
                    inputbackcolor.style.borderColor = bgcolor;
                }

                if (document.getElementById("Button_QRCode") != null) {
                    buttonQRCode = document.getElementById("Button_QRCode");
                    buttonQRCode.setAttribute("bgcolor", bgcolor);
                    buttonQRCode.style.bgcolor = bgcolor;
                    document.getElementById("Button_QRCode").click();
                }
            } catch (exCol) {
                console.log("newBackgroundColor(bgcolor = " + bgcolor +
                    ") throwed at getElementsById('input_backcolor') " + exCol);
            }
        }


    </script>
</asp:Content>
<asp:Content ID="ContentQrBody" ContentPlaceHolderID="QrBody" runat="server">
	<form id="Form_Qr" runat="server">
		<div align="left" class="contentDiv">
			<asp:DropDownList ClientIDMode="Static" ID="DropDownListQrMode" runat="server" CssClass="QrDropDownList" ToolTip="Select Qr mode" 
				Style="display: inline-block;">
				<asp:ListItem Enabled="true" Selected="False" Value="1" Text="1" />
				<asp:ListItem Enabled="true" Selected="False" Value="2" Text="2" />
				<asp:ListItem Enabled="true" Selected="False" Value="3" Text="3" />
				<asp:ListItem Enabled="true" Selected="True" Value="4" Text="4" />
				<asp:ListItem Enabled="true" Selected="False" Value="6" Text="6" />
				<asp:ListItem Enabled="true" Selected="False" Value="8" Text="8" />
			</asp:DropDownList>
			<asp:DropDownList ClientIDMode="Static" ID="DropDownListQrLevel" runat="server" CssClass="QrDropDownList" ToolTip="Select Qr mode" 
				Style="display: inline-block;">
				<asp:ListItem Enabled="true" Selected="True" Value="-1" Text="Default" />
				<asp:ListItem Enabled="true" Selected="False" Value="0" Text="L" />
				<asp:ListItem Enabled="true" Selected="False" Value="1" Text="M" />
				<asp:ListItem Enabled="true" Selected="False" Value="2" Text="Q" />
				<asp:ListItem Enabled="true" Selected="False" Value="3" Text="H" />
			</asp:DropDownList>			
		</div>
		<div align="left" class="contentDiv">
			<asp:LinkButton ID="LinkButton_QrString" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code from string" 
				OnClick="LinkButton_QrString_Click" Text="Generate QrCode from Text" Style="display: inline-block;">Generate QrCode from Text</asp:LinkButton>:
			<asp:TextBox ID="TextBox_QrString" runat="server" ToolTip="enter text here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
				Width="128px" Height="24pt" TextMode="SingleLine" CssClass="QRTextBoxLarge"></asp:TextBox>
		</div>
		<div align="left" class="contentDiv">
			<asp:LinkButton ID="LinkButton_QrUrl" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code for URL" 
				OnClick="LinkButton_QrUrl_Click" Text="Generate QrCode from Url" Style="display: inline-block;">
			Generate QrCode from  Url</asp:LinkButton>:
			<asp:TextBox ID="TextBox_QrUrl" runat="server" ToolTip="enter url www web site" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
				Width="128px" Height="24pt" AutoCompleteType="Homepage" CssClass="QRTextBoxLarge" />
		</div>
		<div align="left" class="contentDiv">
			<asp:LinkButton ID="LinkButton_QrPhone" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code for phone numner" 
				OnClick="LinkButton_QrPhone_Click" Text="Generate QrCode for Phone" Style="display: inline-block;">Generate QrCode for Phone</asp:LinkButton>:
			<asp:TextBox ID="TextBox_QrPhone" runat="server" ToolTip="enter phone number here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
				Width="128px" Height="24pt" AutoCompleteType="Cellular" CssClass="QRTextBoxLarge"></asp:TextBox>
		</div>
		<div align="left" class="contentDiv">
			<asp:LinkButton ID="LinkButton_QrIBAN" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code for IBAN" 
				OnClick="LinkButton_QrIBAN_Click" Text="Generate QrCode for Phone"  Style="display: inline-block;">Generate QrCode for IBAN</asp:LinkButton>:
			<asp:TextBox ID="TextBox_IBAN" runat="server" ToolTip="enter international bank account number (IBAN) here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
				MaxLength="32" Width="180px" Height="24pt" CssClass="QRTextBoxIBAN"></asp:TextBox>
			BIC: <asp:TextBox ID="TextBox_BIC" runat="server" ToolTip="enter bank identifier code here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
					MaxLength="12" Width="120px" Height="24pt" CssClass="QRTextBoxBIC"></asp:TextBox><br />
			Accout Name: <asp:TextBox ID="TextBox_AccountName" runat="server" ToolTip="enter account name identifier" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
					MaxLength="64" Width="228px" Height="24pt" CssClass="QRTextBoxName"></asp:TextBox>&nbsp;
			Reason: <asp:TextBox ID="TextBox_Reason" runat="server" ToolTip="enter transfer reason here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
					MaxLength="64" Width="216px" Height="24pt" CssClass="QRTextBoxName"></asp:TextBox>
		</div>
		<div align="left" class="contentDiv">
			<span class="lefthuge">				
				<input type="color" name="color1" id="color1" onchange="newQrColor(color1.value);" />&nbsp;
				<input id="input_color" ClientIDMode="Static" alt="qr color" runat="server" name="input_color" type="text" value="" size="7" />										
				<br />
				<input type="color" name="color0" id="color0" onchange="newBackgroundColor(color0.value);" />&nbsp;
				<input id="input_backcolor" ClientIDMode="Static" alt="background color" runat="server" name="input_backcolor" type="text" value="" size="7" />										
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
</asp:Content>
