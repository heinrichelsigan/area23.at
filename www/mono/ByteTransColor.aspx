<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="ByteTransColor.aspx.cs" Inherits="area23.at.www.mono.ByteTransColor" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="res/area23.at.www.mono.css" />
    <title>Byte Trans Color Image (apache2 mod_mono)</title>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <INPUT id="oFile" type="file" runat="server" NAME="oFile">
    <asp:Button ID="btnUpload" type="submit" Text="Upload" runat="server" OnClick="btnUpload_Click"></asp:button>
    <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
        <img id="imgIn" runat="server" border="0" alt="Image uploaded" />
        <br />
        <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
        <br />
        <img id="imgOut" runat="server" border="0" alt="Image transformed" />        
     </asp:Panel>
</asp:Content>
