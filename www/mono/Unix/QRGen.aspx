<%@ Page Language="C#"  MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="QRGen.aspx.cs" Inherits="Area23.At.Mono.Unix.QRGen" %>
<asp:Content ID="UnixHeadContent" ContentPlaceHolderID="UnixHead" runat="server">
	<title>qrcode url param passed (apache2 mod_mono)</title>
	<link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
</asp:Content>
<asp:Content ID="UnixBodyContent" ContentPlaceHolderID="UnixBody" runat="server">
	<div>
		<a id="aHref" runat="server" href="" title="Redirecting to ...">Redirecting to ...</a>
		<br />
		<img id="ImgQR" runat="server" alt="QRCode" height="244" width="244" tooltip="QRCode" src="../res/img/qrsample.gif" />
	</div>
</asp:Content>