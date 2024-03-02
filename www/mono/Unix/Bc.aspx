<%@ Page Language="C#" Title="basic calculator bc(1)" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="Bc.aspx.cs" Inherits="Area23.At.Mono.Unix.Bc" %>
<asp:Content ID="UnixContentHead" ContentPlaceHolderID="UnixHead" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <title>basic calculator bc(1)</title>
    <script async src="../res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="UnixContentBody" ContentPlaceHolderID="UnixBody" runat="server">
    <form id="Area23MasterForm" runat="server">       
        <asp:TextBox ID="bcCurrentOp" runat="server" TextMode="SingleLine" MaxLength="256" Columns="80" ReadOnly="true"></asp:TextBox>
        <asp:TextBox ID="bcText" runat="server" TextMode="MultiLine" MaxLength="65536" Columns="80" Rows="24" 
            BackColor="Black" ForeColor="White" BorderWidth="1" BorderColor="#111111" BorderStyle="Outset" 
        Width="84%" AutoPostBack="true" OnTextChanged="BcText_KeyPress"></asp:TextBox> 
        <pre id="preOut" runat="server">
        </pre>
        <hr />
        <div class="odDiv">
            <span style="width:28%; text-align: left;">
                <asp:Button ID="ButtonReset" runat="server" ToolTip="Click to reset bc(1)" Text="Reset bc(1)" OnClick="Button_ResetBc_Click" />
            </span>
        </div>
    </form>
</asp:Content>
