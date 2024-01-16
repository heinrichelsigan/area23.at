<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="ByteTransColor.aspx.cs" Inherits="area23.at.www.mono.ByteTransColor" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="res/css/area23.at.www.mono.css" />
    <title>byte trans color image (apache2 mod_mono)</title>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <INPUT id="oFile" type="file" runat="server" NAME="oFile">
    <asp:Button ID="btnUploadTrans" type="submit" Text="Transform" runat="server" OnClick="btnUploadTrans_Click"></asp:button>
    <asp:Button ID="btnUploadRe" type="submit" Text="Reform" runat="server" OnClick="btnUploadRe_Click"></asp:button>
    <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
        <img id="imgIn" runat="server" border="0" alt="Image uploaded" width="600" />
        <br />
        <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
        <br />
        <img id="imgOut" runat="server" border="0" alt="Image transformed" />
     </asp:Panel>
</asp:Content>
