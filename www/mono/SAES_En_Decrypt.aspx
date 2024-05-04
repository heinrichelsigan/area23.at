<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="SAES_En_Decrypt.aspx.cs" Inherits="Area23.At.Mono.SAES_En_Decrypt" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>byte trans color image (apache2 mod_mono)</title>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="Area23SAESDeEncryptForm" runat="server" action="SAES_En_Decrypt.aspx" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">
        <h2>Enryption method</h2>
        <asp:CheckBoxList ID="CheckBoxListEncDeCryption" runat="server">
            <asp:ListItem Text="2FISH" Enabled="false" Value="2FISH" Selected="False"></asp:ListItem>
            <asp:ListItem Text="3DES" Enabled="true" Value="3DES" Selected="true"></asp:ListItem>
            <asp:ListItem Text="AES" Enabled="true" Value="AES" Selected="true"></asp:ListItem>
            <asp:ListItem Text="Serpent" Enabled="false" Value="Serpent" Selected="False"></asp:ListItem>
        </asp:CheckBoxList>
        <h2>En-/Decrypt file</h2>
        <INPUT id="oFile" type="file" runat="server" NAME="oFile" />
        <asp:Button ID="ButtonEncryptFile" runat="server" ToolTip="Encrypt file" OnClick="ButtonEncryptFile_Click" Text="Encrypt file" />
        <asp:Button ID="ButtonDecryptFile" runat="server" ToolTip="Decrypt file" OnClick="ButtonDecryptFile_Click" Text="Decrypt file" />        
        <h2>En-/Decrypt text</h2>
        <asp:TextBox ID="TextBoxSource" runat="server" TextMode="MultiLine" MaxLength="65536" Rows="5" Columns="36"></asp:TextBox>
        <asp:TextBox ID="TextBoxDestionation" runat="server" TextMode="MultiLine" Rows="5" Columns="36" MaxLength="65536"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonEncrypt" runat="server" Text="Encrypt" ToolTip="Encrypt" OnClick="ButtonEncrypt_Click" />
        <asp:Button ID="ButtonDecrypt" runat="server" Text="Decrypt" ToolTip="Decrypt" OnClick="ButtonDecrypt_Click" />
        <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
            <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
            <br />
            <a id="aTransFormed" runat="server" alt="Transformed File" href="res/fortune.u8">
                <img id="imgOut" runat="server" border="0" alt="File transformed" src="res/img/file.png" /></a>
         </asp:Panel>
    </form>
</asp:Content>
