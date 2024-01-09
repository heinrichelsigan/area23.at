<%@ Page Title="QR Code generator (apache2 mod_mono)" Language="C#" MasterPageFile="~/QRMaster.master" AutoEventWireup="true" CodeBehind="Qr.aspx.cs" Inherits="area23.at.www.mono.Qr" %>
<asp:Content ID="ContentQrHead" ContentPlaceHolderID="QrHead" runat="server">
	<title>QR Code generator (apache2 mod_mono)</title>
	<link rel="stylesheet" href="res/area23.at.www.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
</asp:Content>
<asp:Content ID="ContentQrBody" ContentPlaceHolderID="QrBody" runat="server">
	<div align="left" class="contentDiv">
		url:
		<asp:TextBox ID="TextBox_QrUrl" runat="server" ToolTip="enter url www web site" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
            Width="128pt" Height="24pt" TextMode="Url" CssClass="QRTextBox" AutoCompleteType="Homepage">https://area23.at/</asp:TextBox>
	</div>
	<div align="left" class="contentDiv">
		<span class="lefthuge">
			<asp:LinkButton ID="LinkButton_QrUrl" runat="server" ToolTip="Generate QR Code for URL" Text="Gen Url QRCode" OnClick="LinkButton_QrUrl_Click" />
		</span> 
		<span class="righthuge">
			<img id="ImgQR" runat="server" alt="QRCode" height="192" width="192" tooltip="QRCode" src="res/qrsample1.gif" />			
		</span>		
	</div>
	<div align="left" class="contentDiv">
		<span class="lefthuge">
			<asp:Button ID="Button_QrUrl" runat="server" ToolTip="Generate QR Code for URL" Text="Gen Url QRCode" OnClick="Button_QrUrl_Click" />
		</span>
		<span class="righthuge">
			<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/qrsample2.png" BorderStyle="None" Width="192" />
		</span>
	<</div>
</asp:Content>
