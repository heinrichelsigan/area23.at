<%@ Page Language="C#" Title="basic calculator bc(1)" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="Bc.aspx.cs" Inherits="Area23.At.Mono.Unix.Bc" %>
<asp:Content ID="UnixContentHead" ContentPlaceHolderID="UnixHead" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />    
    <title>basic calculator bc(1)</title>
    <script src="../res/js/bc.js"></script>
    <script async src="../res/js/area23.js"></script>    
    <script language="javascript">
        window.onload = function () {
            bcInit();
        }; 
    </script>
</asp:Content>
<asp:Content ID="UnixContentBody" ContentPlaceHolderID="UnixBody" runat="server">
    <form id="Area23UnixBcForm" runat="server">               
        <asp:TextBox ID="bcText" runat="server" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="BcText_TextChanged"
            TextMode="MultiLine" MaxLength="65536" Columns="80" Rows="24" 
            BackColor="Black" ForeColor="White" BorderWidth="1" BorderColor="#111111" BorderStyle="Outset"
            Text="bc 1.07.1
Copyright 1991-1994, 1997, 1998, 2000, 2004, 2006, 2008, 2012-2017 Free Software Foundation, Inc.
This is free software with ABSOLUTELY NO WARRANTY.
For details type `warranty'.
"            Width="84%" />
        <hr />
        <asp:TextBox ID="bcCurrentOp" runat="server" TextMode="SingleLine" MaxLength="256" Columns="80" ReadOnly="true"></asp:TextBox>
        <pre id="preOut" runat="server">
        </pre>        
        <div class="odDiv">
            <span style="width: 28%; text-align: left;">
                <input type="button" id="inputReset" title="Click to reset bc(1)" value="Reset bc(1)" 
                     OnClientClick="setTimeout(function () { window.location.reload(); }, 100);" /> 
            </span>
            <span style="width:28%; text-align: right;">
                <asp:Button ID="buttonEnter" ClientIDMode="Static" runat="server" Text="[Enter]" CssClass="Hidden" 
                    OnClick="ButtonEnter_Click" />
            </span>
        </div>
    </form>
</asp:Content>
