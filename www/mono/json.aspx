<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Json.aspx.cs" Inherits="Area23.At.Mono.Json" MasterPageFile="~/Area23.Master" ValidateRequest="false" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>json ser (apache2 mod_mono)</title>
    <script async src="res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="Area23JsonForm" runat="server">
        <asp:Literal ID="LiteralDateTime" runat="server"></asp:Literal>
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxJson" runat="server" TextMode="MultiLine" ToolTip="Put your JSON string here" AutoPostBack="true"  OnTextChanged="TextBoxJson_OnTextChanged"  CausesValidation="false" ValidateRequestMode="Disabled"  Width="98%" Height="320px" 
                    Style="table-layout: fixed;" />
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <pre id="jsonPreOut" class="jsonPreOut" runat="server" style="margin-top: -12px; height: 332px;max-height: 332px;overflow: auto;" />
                <!-- pre id="Pre1"  /-->
                <!-- asp:TextBox ID="TextBoxOut" TextMode="MultiLine" ToolTip="output of json operation" Width="100%" Height="320px" /-->
            </div>
        </div>
        <!-- hr /-->
        <div align="left" class="jsonButtons" style="clear: both;">
            <asp:LinkButton ID="LinkButtonJSON" runat="server"  ToolTip="deserialize json and serialize it back to xml" Text="json2xml"  OnClick="JsonDeserialize_Click"></asp:LinkButton> &nbsp;
            <asp:LinkButton ID="LinkButtonJsonTreePaths" runat="server" ToolTip="deserialize only json tree paths" Text="json tree paths" OnClick="LinkButtonJsonTreePaths_Click" /> &nbsp; 
            <asp:LinkButton ID="LinkButtonXML2Json" runat="server" ToolTip="deserialize xml and serialize it back as json" Text="xml2json" OnClick="Xml2Json_Click"></asp:LinkButton>            
            <asp:LinkButton ID="LinkButtonEmpty" runat="server" Text="empty json/xml source form" OnClick="LinkButtonEmpty_Click" /> &nbsp;           
        </div>
    </form>
</asp:Content>

