<%@ Page Title="Simple char, hex, dec, oct, bin mapper (apache2 mod_mono)"  Language="C#" MasterPageFile="~/Crypt/EncodeMaster.master" AutoEventWireup="true" CodeBehind="CharHexDecOctBin.aspx.cs" Inherits="Area23.At.Mono.Crypt.CharHexDecOctBin"  validateRequest="true" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
        <title>Simple char, hex, dec, oct, bin mapper (apache2 mod_mono)</title>
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta name="keywords" content="char character hex hexadecimal decimal dec octal oct binary bin" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" />
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" ClientIDMode="Static" runat="server">
    <form id="EncodeMasterForm" runat="server"  enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">  
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                Char
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxChar" runat="server" MaxLength="2" 
                    ToolTip="Enter character here" TextMode="SingleLine" Width="95%" Height="60px" 
                    Text="" AutoPostBack="true" OnTextChanged="MapCharHexDecOctBin"></asp:TextBox>
            </div>
        </div>
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                &nbsp;Hex
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxHex" runat="server" MaxLength="2" 
                    ToolTip="enter hexadecimal here" TextMode="SingleLine" Width="95%" Height="60px" 
                    Text="" AutoPostBack="true" OnTextChanged="MapCharHexDecOctBin"></asp:TextBox>
            </div>
        </div>
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                &nbsp;Dec
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxDec" runat="server" MaxLength="3" 
                    ToolTip="enter decimal here" TextMode="SingleLine" Width="95%" Height="60px" 
                    Text="" AutoPostBack="true" OnTextChanged="MapCharHexDecOctBin"></asp:TextBox>
            </div>
        </div>
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                &nbsp;Oct
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxOct" runat="server" MaxLength="3" 
                    ToolTip="enter octal here" TextMode="SingleLine" Width="95%" Height="60px" 
                    Text="" AutoPostBack="true" OnTextChanged="MapCharHexDecOctBin" />
            </div>
        </div>
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                &nbsp;Bin
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxBin" runat="server" MaxLength="3" 
                    ToolTip="enter dual bin here" TextMode="SingleLine" Width="95%" Height="60px" 
                    Text="" AutoPostBack="true" OnTextChanged="MapCharHexDecOctBin" />
            </div>
        </div>
        <!-- hr /-->
        <div class="odDiv">
            <span class="leftSpan">
                <asp:Button ID="Button_Clear" runat="server" Text="Clear" ToolTip="Clear Form" OnClick="Button_Clear_Click" />
            </span>
            <span class="centerSpan">
                &nbsp;
            </span>
            <span class="centerSpan">                
                &nbsp;
            </span>
            <span class="rightSpan">
                &nbsp;
            </span>
        </div>         
    </form>
</asp:Content>
