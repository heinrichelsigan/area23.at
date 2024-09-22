<%@ Page Title="image png crypt (apache2 mod_mono)" Language="C#" MasterPageFile="~/Encode/EncodeMaster.master" AutoEventWireup="true" CodeBehind="ImgPngCrypt.aspx.cs" Inherits="Area23.At.Mono.Encode.ImgPngCrypt" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <title>image png crypt (apache2 mod_mono)</title>
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server">
    <form id="ImgPngCryptForm" runat="server" action="ImgPngCrypt.aspx" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">
        <INPUT id="oFile" type="file" runat="server" NAME="oFile">
        <button id="buttonUploadTrans" type="submit" title="Transform" runat="server" OnClick="FormSubmit()">Transform</button>
        <button id="btnUpLoad" type="submit" title="Reform" runat="server" OnClick="FormSubmit()">Reform</button>
        <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
            <img id="imgIn" runat="server" border="0" alt="Image uploaded" width="600" />
            <br />
            <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
            <br />
            <img id="imgOut" runat="server" border="0" alt="Image transformed" />
         </asp:Panel>
    </form>
</asp:Content>
