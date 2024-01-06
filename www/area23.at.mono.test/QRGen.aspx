<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRGen.aspx.cs" Inherits="area23.at.mono.test.QRGen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <a id="aHref" runat="server" href="" title="Redirecting to ...">Redirecting to ...</a>
            <br />
            <img id="ImgQR" runat="server" alt="QRCode" height="192" width="192" tooltip="QRCode" src="res/qrsample1.gif" />
        </div>
    </form>
</body>
</html>
