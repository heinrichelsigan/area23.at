<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.Settings.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <asp:Table ID="TableSettings" runat="server" BorderStyle="Outset" BorderColor="#c0c0c0" CellPadding="1" CellSpacing="1">
                    <asp:TableHeaderRow>
                        <asp:TableCell BorderStyle="Double" BackColor="Silver" Font-Bold="true">Setting Name</asp:TableCell> 
                        <asp:TableCell BorderStyle="Double" BackColor="Silver" Font-Bold="true">Setting Value</asp:TableCell> 
                    </asp:TableHeaderRow>
                </asp:Table>                
            </div>
            <hr />
            <div>
                <asp:Table ID="TableRuntime" runat="server" BorderStyle="Outset" BorderColor="#c0c0c0" CellPadding="1" CellSpacing="1">
                    <asp:TableHeaderRow>
                        <asp:TableCell BorderStyle="Outset" BackColor="LightGray" Font-Bold="true">Runtime Name</asp:TableCell> 
                        <asp:TableCell BorderStyle="Outset" BackColor="LightGray" Font-Bold="true">Runtime Value</asp:TableCell> 
                    </asp:TableHeaderRow>
                </asp:Table>                
            </div>
            <div id="DivTest0" runat="server" style="background-color: floralwhite; border-width: 1; border-style: dashed">
            </div>
            <div id="DivTest1" runat="server" style="background-color: lightcyan; border-width: 1; border-style: dashed">
            </div>
            <div id="DivTest2" runat="server" style="background-color: lightgray; border-width: 1; border-style: dashed">
            </div>
        </div>
    </form>
</body>
</html>
