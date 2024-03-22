<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="ByteTransColor.aspx.cs" Inherits="Area23.At.Mono.ByteTransColor" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>byte trans color image (apache2 mod_mono)</title>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="Area23TransColorForm" runat="server" action="ByteTransColor.aspx" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">
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
