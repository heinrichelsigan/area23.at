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
    <form id="form1" runat="server" method="post" enctype="text/plain">
		<asp:TextBox ID="TextBoxSubmit" runat="server" TextMode="MultiLine" MaxLength="32768" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="[Enter text to en-/decrypt here]" Text="" Width="480px"></asp:TextBox>
		<br />
		<asp:Button ID="ButtonSubmit" runat="server" Text="Submit" ToolTip="Submit" OnClick="ButtonSubmit_Click" />
	</form> 
</body>
</html>
