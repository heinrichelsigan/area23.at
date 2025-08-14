<%@ Page Language="C#"  MasterPageFile="~/Qr/QRMaster.master" AutoEventWireup="true" CodeBehind="QrRedirect.aspx.cs" Inherits="Area23.At.Mono.Qr.QrRedirect" %>
<asp:Content ID="QrHeadContent" ContentPlaceHolderID="QrHead" runat="server">
	<title>qrcode url param passed (apache2 mod_mono)</title>
	<link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
	<title>QrRedirect (apache2 mod_mono)</title>
</asp:Content>
<asp:Content ID="QrBodyContent" ContentPlaceHolderID="QrBody" runat="server">
	<form id="Form_QrRedirect" runat="server">
		<div>
			<a id="aHref" runat="server" href="" title="Redirecting to ...">Redirecting to ...</a>
			<br />
			<img id="ImgQR" runat="server" alt="QRCode" height="244" width="244" tooltip="QRCode" src="../res/img/qrsample.gif" />
		</div>
	</form>
</asp:Content>