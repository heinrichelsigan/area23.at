<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.ChatQ.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>area23.at/q/ - another chatq</title>
	<link rel="stylesheet" href="res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="author" content="Heinrich Elsigan" />
	<meta name="description" content="https://github.com/heinrich.elsigan/" />
</head>
<body>
    <form id="form1" runat="server">		
		<a href="~/res/img/qrsample2.png" runat="server" id="HrefShort">
			<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/img/qrsample2.png" Visible="false" BorderStyle="None" BackColor="Transparent" />
		</a>
	</form> 
</body>
</html>
