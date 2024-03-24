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
        <asp:TextBox ID="TextBox_BcOut" runat="server" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="TextBox_BcOut_TextChanged"
            TextMode="MultiLine" MaxLength="65536" Columns="80" Rows="24" 
            BackColor="Black" ForeColor="White" BorderWidth="1" BorderColor="#111111" BorderStyle="Outset"
            Text="bc 1.07.1
Copyright 1991-1994, 1997, 1998, 2000, 2004, 2006, 2008, 2012-2017 Free Software Foundation, Inc.
This is free software with ABSOLUTELY NO WARRANTY.
For details type `warranty'.
"            Width="84%" />
        <hr />
        <div class="odDiv">
            <span style="width: 20%; text-align: left; word-break: keep-all;  line-break: unset; nowrap;">
                <asp:Button ID="ButtonEnter" ClientIDMode="Static" runat="server" Text="[Enter]" ToolTip="[Basic Calculate]" OnClick="ButtonEnter_Click" />
            </span>
            <span style="width: 20%; text-align: left; word-break: keep-all;  line-break: unset; nowrap;">
                <asp:Button ID="ButtonReset" ClientIDMode="Static" runat="server" Text="Reset bc(1)" ToolTip="Click to reset bc(1)" OnClick="ButtonReset_Click" />            
            </span>
            <span style="width: 40%; text-align: center; word-break: keep-all;  line-break: unset; nowrap;">
                <asp:TextBox ID="TextBox_BcOp" runat="server" TextMode="SingleLine" MaxLength="256" Columns="40" ReadOnly="true"></asp:TextBox>
            </span>
            <span style="width: 16%; text-align: right; word-break: keep-all;  line-break: unset; nowrap;">
                <asp:TextBox ID="TextBox_BcResult" runat="server" TextMode="SingleLine" MaxLength="16" Columns="16" ReadOnly="true"></asp:TextBox>
            </span>
        </div>
    </form>
</asp:Content>
