<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.Settings.Default" %>
<%@ Register TagPrefix="uc23" TagName="LoginControl" Src="~/Controls/LoginControl.ascx" %> 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Settings</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc23:LoginControl ID="LoginPanel" runat="server" />
            <div id="DivOuter" runat="server" style="display: block; border-style: outset; border-width: 2px; border-color: azure; max-width: 90%; width: 80%; font-size: medium; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;  padding: 2px 2px 2px 2px; margin: 2px 2px 2px 2px;">
                <div>
                    <asp:Table ID="TableSettings" runat="server" BorderStyle="Outset" BorderColor="#c0c0c0" CellPadding="1" CellSpacing="1">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell BorderStyle="Double" BackColor="Silver" Font-Bold="true">Setting Name</asp:TableHeaderCell>
                            <asp:TableHeaderCell BorderStyle="Double" BackColor="Silver" Font-Bold="true">Setting Value</asp:TableHeaderCell>
                        </asp:TableHeaderRow>                    
                    </asp:Table>                
                </div>
                <hr />
                <div>
                    <asp:Table ID="TableRuntime" runat="server" BorderStyle="Outset" BorderColor="#c0c0c0" CellPadding="1" CellSpacing="1">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell BorderStyle="Outset" BackColor="LightGray" Font-Bold="true">Name</asp:TableHeaderCell>
                            <asp:TableHeaderCell BorderStyle="Outset" BackColor="LightGray" Font-Bold="true">Value</asp:TableHeaderCell>
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
        </div>
    </form>
</body>
</html>
