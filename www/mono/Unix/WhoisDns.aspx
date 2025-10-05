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
                <asp:TextBox ID="TextBox_HostName" ClientIDMode="Static" runat="server" ToolTip="Enter domain name or IP address" MaxLength="256" Width="256pt" Height="21pt" 
                    AutoPostBack="True" OnTextChanged="Button_WhoisDns_TextChanged" CssClass="ButtonTextBox">area23.at</asp:TextBox>                
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_WhoisDns" ClientIDMode="Static" runat="server" ToolTip="Click to perform whois and dns lookup" Text="whois/dns" OnClick="Button_WhoisDns_Click" />
            </span>
        </div>
        <div class="odDiv">
            <span class="leftSpan">
                <span class="textSpan">whois cmd: </span>            
            </span>
            <span class="centerSpan">
                <asp:TextBox ID="TextBox_Whois" ClientIDMode="Static" runat="server" ReadOnly="true" ToolTip="whois command" MaxLength="128" Width="192pt" Height="21pt" AutoPostBack="True"  CssClass="ButtonTextBox" />
            </span>
            <span class="centerSpan">
                <span class="textSpan">host cmd: </span>            
            </span>
            <span class="rightSpan">
                <asp:TextBox ID="TextBox_Host" ClientIDMode="Static" runat="server" ReadOnly="true" ToolTip="host command" MaxLength="128" Width="192pt" Height="21pt" AutoPostBack="True" CssClass="ButtonTextBox" />
            </span>
        </div>
        <hr />
        <pre id="preOut" runat="server">
        </pre>
    </form>
</asp:Content>
