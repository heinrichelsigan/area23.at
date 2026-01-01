<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="R.aspx.cs" Inherits="Area23.At.Mono.MyIp.R" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server"><title id="title" runat="server" title="My-IPAddr" /></head>
<body>
    <asp:Literal ID="literalUserHost" runat="server"></asp:Literal>
    <br />
    <asp:HyperLink ID="GeoLink" NavigateUrl="https://www.google.com/maps/" runat="server"></asp:HyperLink>
</body>
</html>