<%@ Page Title="JokerDicePoker" Language="C#" MasterPageFile="~/Gamez/GamesMaster.master" AutoEventWireup="true" CodeBehind="JokerDice.aspx.cs" Inherits="Area23.At.Mono.Gamez.JokerDice" %>

<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>JokerDicePoker</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">     
    <form id="Form_JokerDice" runat="server">
        <div>
        <asp:Table ID="TablePC" runat="server">
            <asp:TableRow ID="TableRowP" runat="server">
                <asp:TableCell ID="TableCellP0" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Literal ID="Literal_P0" runat="server" Text="Player" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP1" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageP1" runat="server" ImageUrl="~/res/img/symbol/Joker.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP2" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageP2" runat="server"  ImageUrl="~/res/img/symbol/Ace.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP3" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageP3" runat="server"  ImageUrl="~/res/img/symbol/King.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP4" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageP4" runat="server"  ImageUrl="~/res/img/symbol/Queen.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP5" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageP5" runat="server" ImageUrl="~/res/img/symbol/Jack.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellP6" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageP6" runat="server" ImageUrl="~/res/img/symbol/Ten.png" Width="122px" Height="122px" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRowC" runat="server">
                <asp:TableCell ID="TableCellC0" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Literal ID="Literal_C0" runat="server" Text="Computer" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC1" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC1" runat="server" ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC2" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC2" runat="server"  ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC3" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC3" runat="server"  ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC4" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC4" runat="server"  ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC5" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC5" runat="server" ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
                <asp:TableCell ID="TableCellC6" runat="server" Width="12%" Style="vertical-align: text-top;">
                    <asp:Image ID="ImageC6" runat="server" ImageUrl="~/res/img/symbol/EmptyDice.png" Width="122px" Height="122px" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </div>
    </form>
</asp:Content>
