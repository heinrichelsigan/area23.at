<%@ Page Title="QR Code generator (apache2 mod_mono)" Language="C#" MasterPageFile="~/QRMaster.master" AutoEventWireup="true" CodeBehind="Qr.aspx.cs" Inherits="area23.at.www.mono.Qr" %>
<asp:Content ID="ContentQrHead" ContentPlaceHolderID="QrHead" runat="server">
	<title>QR Code generator (apache2 mod_mono)</title>
	<link rel="stylesheet" href="res/area23.at.www.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
</asp:Content>
<asp:Content ID="ContentQrBody" ContentPlaceHolderID="QrBody" runat="server">
	<div align="left" class="contentDiv">
		<asp:LinkButton ID="LinkButton_QrString" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code from string" 
			OnClick="LinkButton_QrString_Click" Text="Generate QrCode from Text">Generate QrCode from Text</asp:LinkButton>:
		<asp:TextBox ID="TextBox_QrString" runat="server" ToolTip="enter text here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
            Width="128px" Height="24pt" TextMode="SingleLine" CssClass="QRTextBoxLarge"></asp:TextBox>
	</div>
	<div align="left" class="contentDiv">
		<asp:LinkButton ID="LinkButton_QrUrl" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code for URL" OnClick="LinkButton_QrUrl_Click" Text="Generate QrCode from Url">
		Generate QrCode from  Url</asp:LinkButton>:
		<asp:TextBox ID="TextBox_QrUrl" runat="server" ToolTip="enter url www web site" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
            Width="128px" Height="24pt" TextMode="Url" AutoCompleteType="Homepage" CssClass="QRTextBoxLarge"></asp:TextBox>
	</div>
	<div align="left" class="contentDiv">
		<asp:LinkButton ID="LinkButton_QrPhone" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code for phone numner" OnClick="LinkButton_QrPhone_Click" 
			Text="Generate QrCode for Phone">Generate QrCode for Phone</asp:LinkButton>:
		<asp:TextBox ID="TextBox_QrPhone" runat="server" ToolTip="enter phone number here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
            Width="128px" Height="24pt" AutoCompleteType="Cellular" TextMode="Phone" CssClass="QRTextBoxLarge"></asp:TextBox>
	</div>
	<div align="left" class="contentDiv">
		<asp:LinkButton ID="LinkButton_QrIBAN" runat="server" CssClass="QrLinkButton" ToolTip="Generate QR Code for IBAN" OnClick="LinkButton_QrIBAN_Click" Text="Generate QrCode for Phone">
			Generate QrCode for IBAN</asp:LinkButton>:
		<asp:TextBox ID="TextBox_IBAN" runat="server" ToolTip="enter international bank account number (IBAN) here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
            MaxLength="32" Width="156px" Height="24pt" CssClass="QRTextBoxIBAN"></asp:TextBox>
		BIC: <asp:TextBox ID="TextBox_BIC" runat="server" ToolTip="enter bank identifier code here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
				MaxLength="12" Width="120px" Height="24pt" CssClass="QRTextBoxBIC"></asp:TextBox>
		Accout Name: <asp:TextBox ID="TextBox_AccountName" runat="server" ToolTip="enter account name identifier" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
				MaxLength="64" Width="240px" Height="24pt" TextMode="SingleLine" CssClass="QRTextBoxName"></asp:TextBox>
	</div>
	<div align="left" class="contentDiv">
		<span class="lefthuge">
			<img id="ImgQR" runat="server" alt="QRCode" height="192" width="192" tooltip="QRCode" visible="true" src="res/qrsample1.gif" />			
		</span>
		<span class="righthuge">
			<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/qrsample2.png" Visible="false" BorderStyle="None" Width="192" />
		</span>		
	</div>

</asp:Content>
