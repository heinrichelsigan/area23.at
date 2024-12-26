<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.CqrJD.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CqrJd Testform</title>
</head>
<body>
    <form id="form1" runat="server" method="post" enctype="text/plain" submitdisabledcontrols="true" novalidate="novalidate" style="background-color: transparent;">
        <div>
            <div>
                <span style="display: block; border-style: outset; border-width: 1px; border-color: azure; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif; font-size: larger">
                    ServerIPv4: <asp:Literal ID="LiteralServerIPv4" runat="server"></asp:Literal>
                </span>
                <br />
                <span style="display: block; border-style: outset; border-width: 1px; border-color: azure; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif; font-size: larger">
                    ServerIPv6: <asp:Literal ID="LiteralServerIPv6" runat="server"></asp:Literal>
                </span>
                <br />
                <span style="display: block; border-style: outset; border-width: 1px; border-color: blanchedalmond; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif; font-size: larger">
                    ClientIp: <asp:Literal ID="LiteralClientIp" runat="server"></asp:Literal>
                </span>
                <br />
                <span style="display: block; border-style: outset; border-width: 1px; border-color: blanchedalmond; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif; font-size: larger">
                    ServerIp from client: <asp:Literal ID="LiteralFromClient" runat="server"></asp:Literal>
                </span>
            </div>
            <hr />
            <div>
                <asp:TextBox ID="TextBoxEncrypted" runat="server" TextMode="MultiLine" MaxLength="65536" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="TextBox Current Message" Text="" Width="480px"></asp:TextBox>
                <br />
                <asp:TextBox ID="TextBoxDecrypted" runat="server" TextMode="MultiLine" MaxLength="65536" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="TextBox Current Message" Text="" Width="480px"></asp:TextBox>
                <br />
                <asp:TextBox ID="TextBoxLastMsg" runat="server" ReadOnly="true" TextMode="MultiLine" MaxLength="65536" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="TextBox Last Message" Text="" Width="480px"></asp:TextBox>
                <br />
                <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" ToolTip="Submit" OnClick="ButtonSubmit_Click" />
            </div>
            <hr />
            <pre id="preOut" runat="server" style="width: 640px; height: 180px; max-height: 192px; border-block-color: palevioletred;">

            </pre>
            <hr />
            <pre id="preLast" runat="server" style="width: 800px; height: 240px; max-height: 320px; border-block-color: blue;">

            </pre>
        </div>
    </form>
</body>
</html>
