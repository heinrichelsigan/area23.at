<%@ Page Language="C#" Title="hex dump (apache2 mod_mono)" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="WhoisDns.aspx.cs" Inherits="Area23.At.Mono.Unix.WhoisDns" %>
<asp:Content ID="UnixContentHead" ContentPlaceHolderID="UnixHead" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <title>whois dns (apache2 mod_mono)</title>
    <script async src="../res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="UnixContentBody" ContentPlaceHolderID="UnixBody" runat="server">
    <form id="Area23UnixWhoisDns" runat="server">
        <div class="odDiv">
            <span class="leftSpan">
                <span class="textSpan">host name: </span>            
            </span>
            <span class="centerSpan">
                <asp:TextBox ID="TextBox_HostName" ClientIDMode="Static" runat="server" ToolTip="Enter domain name or IP address" MaxLength="256" Width="256pt" Height="20pt" 
                    AutoPostBack="True" OnTextChanged="Button_WhoisDns_TextChanged" CssClass="ButtonTextBox">area23.at</asp:TextBox>                
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_WhoisDns" ClientIDMode="Static" runat="server" ToolTip="Click to perform whois and dns lookup" Text="whois/dns" OnClick="Button_WhoisDns_Click" Height="20pt" />
            </span>
        </div>        
        <hr />
        <asp:Table ID="AspTable" ClientIDMode="Static" runat="server">
            <asp:TableHeaderRow ID="TableHeaderRow" runat="server" ClientIDMode="Static">
                <asp:TableCell ID="TableHeaderCellLeft" runat="server" ClientIDMode="Static"><b>whois</b></asp:TableCell>
                <asp:TableCell ID="TableHeaderCellRight" runat="server" ClientIDMode="Static"><b>dns host</b></asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow ID="TableRow" runat="server" ClientIDMode="Static">
                <asp:TableCell ID="TableCellLeft" runat="server" ClientIDMode="Static" style="vertical-align: text-top;" Width="48%"></asp:TableCell>
                <asp:TableCell ID="TableCellRight" runat="server" ClientIDMode="Static" style="vertical-align: text-top;" Width="48%"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>     
    </form>
</asp:Content>
