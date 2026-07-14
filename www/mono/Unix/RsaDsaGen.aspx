<%@ Page Language="C#" Title="Rsa Dsa generator (apache2 mod_mono)" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="RsaDsaGen.aspx.cs" Inherits="Area23.At.Mono.Unix.RsaDsaGen" %>
<asp:Content ID="UnixContentHead" ContentPlaceHolderID="UnixHead" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <title>Rsa Dsa generator (apache2 mod_mono)</title>
    <script src="../res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="UnixContentBody" ContentPlaceHolderID="UnixBody" runat="server">
    <form id="Area23UnixRsaDsaGenForm" runat="server">
        <div class="odDiv">
            <span class="leftSpan">
                <asp:RadioButtonList ID="RadioButtonList_Algorithm" runat="server" ToolTip="asymmetric key algorithm"
                    AutoPostBack="True" OnSelectedIndexChanged="Algorithm_Changed" RepeatDirection="Horizontal" CssClass="RadioButtonList">                    
                    <asp:ListItem>Dsa</asp:ListItem>
                    <asp:ListItem Selected="True">Rsa</asp:ListItem>
                </asp:RadioButtonList>
            </span>
            <span class="centerSpan">
                <span class="textSpan">passkey: </span>    
                <asp:TextBox ID="TextBox_PassKey" ClientIDMode="Static" runat="server" ToolTip="Enter domain name or IP address" MaxLength="256" Width="256pt" Height="20pt" 
                    AutoPostBack="True" OnTextChanged="TextBox_PassKey_TextChanged" CssClass="ButtonTextBox" />
            </span>
            <span class="centerSpan">
                <span class="textSpan"> key size: </span>
                <asp:DropDownList ID="DropDown_KeySize" runat="server" ToolTip="KeySize for asymmetric key" 
                        AutoPostBack="True" OnSelectedIndexChanged="KeySize_Changed" CssClass="DropDownList">
                    <asp:ListItem>512</asp:ListItem>
                    <asp:ListItem Selected="True">1024</asp:ListItem>
                    <asp:ListItem>2048</asp:ListItem>
                    <asp:ListItem>4096</asp:ListItem>
                    <asp:ListItem>8192</asp:ListItem>
                </asp:DropDownList>
         </span>
            <span class="rightSpan">
                <asp:Button ID="Button_Gen" ClientIDMode="Static" runat="server" ToolTip="Click to generate keys" Text="generate" OnClick="Button_Gen_Click" Height="20pt" />
            </span>
        </div>        
        <br />
        <hr />
        <asp:Table ID="AspTable" ClientIDMode="Static" runat="server">
            <asp:TableHeaderRow ID="TableHeaderRow" runat="server" ClientIDMode="Static">
                <asp:TableCell ID="TableHeaderCellLeft" runat="server" ClientIDMode="Static"><b>private key</b></asp:TableCell>
                <asp:TableCell ID="TableHeaderCellRight" runat="server" ClientIDMode="Static"><b>public key</b></asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow ID="TableRow" runat="server" ClientIDMode="Static">
                <asp:TableCell ID="TableCellLeft" runat="server" ClientIDMode="Static" style="vertical-align: text-top;" Width="48%"></asp:TableCell>
                <asp:TableCell ID="TableCellRight" runat="server" ClientIDMode="Static" style="vertical-align: text-top;" Width="48%"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>     
    </form>
</asp:Content>
