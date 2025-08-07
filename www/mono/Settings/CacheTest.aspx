<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CacheTest.aspx.cs" Inherits="Area23.At.Mono.Settings.CacheTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cache Test</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="word-wrap:unset">
                <asp:Label ID="Label_CacheVariant" runat="server" ClientIDMode="Static" Text="cache variant:" ToolTip="choose cache variant to test" />
                <asp:DropDownList ID="DropDownList_CacheVariant" runat="server" ClientIDMode="Static">
                    <asp:ListItem Enabled="true"  Selected="True" Text="AppDomain" Value="AppDomain" />
                    <asp:ListItem Enabled="true" Selected="False" Text="ApplicationState" Value="ApplicationState" />
                    <asp:ListItem Enabled="true" Selected="False" Text="JsonFile" Value="JsonFile" />
                    <asp:ListItem Enabled="true" Selected="False" Text="Redis" Value="Redis" />
                </asp:DropDownList>
                <asp:Literal ID="Literal_Iterations" runat="server" ClientIDMode="Static" Text="iterations:" />
                <asp:DropDownList ID="DropDownList_Iterations" runat="server" ClientIDMode="Static">
                    <asp:ListItem Enabled="true" Selected="False" Text="16" Value="16" />
                    <asp:ListItem Enabled="true" Selected="False" Text="32" Value="32" />
                    <asp:ListItem Enabled="true" Selected="True" Text="64" Value="64" />
                    <asp:ListItem Enabled="true" Selected="False" Text="128" Value="128" />
                    <asp:ListItem Enabled="true" Selected="False" Text="256" Value="256" />
                    <asp:ListItem Enabled="true" Selected="False" Text="512" Value="512" />
                    <asp:ListItem Enabled="true" Selected="False" Text="1024" Value="1024" />
                </asp:DropDownList>
                <asp:Button ID="Button_TestCache" runat="server" ClientIDMode="Static" Text="Test Cache" ToolTip="Click here to start cache test" Enabled="true" Visible="true" OnClick="Button_TestCache_Click" />
            </div>
            <div>
                <asp:CheckBoxList ID="CheckBoxList_TestType" runat="server" ClientIDMode="Static" ToolTip="cache tests">
                    <asp:ListItem Enabled="true" Selected="True" Text="Serial" Value="Serial" />
                    <asp:ListItem Enabled="true" Selected="True" Text="Parallel" Value="Parallel" />
                </asp:CheckBoxList>
            </div>
            
            <hr />
            <div id="DivSettingsTable" runat="server" visible="false">
                <asp:Table ID="TableSettings" runat="server" BorderStyle="Outset" BorderColor="#c0c0c0" CellPadding="1" CellSpacing="1" Visible="false">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell BorderStyle="Double" BackColor="Silver" Font-Bold="true">Setting Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell BorderStyle="Double" BackColor="Silver" Font-Bold="true">Setting Value</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>                
            </div>
            <hr />
            <div>
                <asp:Table ID="TableCacheTest" runat="server" BorderStyle="Outset" BorderColor="#c0c0c0" CellPadding="1" CellSpacing="1">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell BorderStyle="Outset" BackColor="LightGray" Font-Bold="true">Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell BorderStyle="Outset" BackColor="LightGray" Font-Bold="true">Value</asp:TableHeaderCell>
                    </asp:TableHeaderRow>                
                </asp:Table>                
            </div>
            <hr />
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
