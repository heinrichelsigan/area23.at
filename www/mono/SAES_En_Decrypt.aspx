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
        <div class="odDiv">
            <span class="leftSpan">
                <asp:DropDownList ID="DropDownList_SymChiffer" runat="server">
                    <asp:ListItem Enabled="true" Value="3DES" Selected="false">3DES</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="2FISH" Selected="false">2FISH</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="3FISH" Selected="false">3FISH</asp:ListItem>                    
                    <asp:ListItem Enabled="true" Value="AES" Selected="true">AES</asp:ListItem>                     
                    <asp:ListItem Enabled="true" Value="Camellia" Selected="False">Camellia</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Idea" Selected="false">Idea</asp:ListItem>                     
                    <asp:ListItem Enabled="true" Value="Rijndael" Selected="false">Rijndael</asp:ListItem>                    
                    <asp:ListItem Enabled="true" Value="Serpent" Selected="false">Serpent</asp:ListItem>                    
                    <asp:ListItem Enabled="true" Value="Tea" Selected="false">Tea</asp:ListItem>      
                    <asp:ListItem Enabled="true" Value="Tnepres" Selected="false">Tnepres</asp:ListItem>                    
                    <asp:ListItem Enabled="true" Value="XTea" Selected="false">XTea</asp:ListItem>      
                </asp:DropDownList>
            </span>
            <span class="centerSpan">
                <asp:ImageButton ID="ImageButton_Add" ClientIDMode="Static" runat="server" ImageUrl="res/img/a_right.gif" 
                    OnClick="ImageButton_Add_Click" AlternateText="Add symetric chiffer algorithm"
                    onmouseover="document.getElementById('ImageButton_Add').src='res/img/a_right_hover.gif'"                     
                    onmouseout="document.getElementById('ImageButton_Add').src='res/img/a_right.gif'" />
            </span>
            <span class="centerSpan">
                <asp:TextBox ID="TextBox_Encryption" runat="server" ReadOnly="true" TextMode="SingleLine" Text="" />
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_Clear" runat="server" Text="Clear" ToolTip="Clear SymChiffre Pipeline" OnClick="Button_Clear_Click" />
            </span>
        </div>
        <div class="odDiv">
            <span class="leftSpan">
                [future enabled]
            </span>
            <span class="centerSpan">
                <span class="textSpan">key: </span>
                <asp:TextBox ID="TextBox_Key" runat="server" Enabled="false" Text="0x106311o7edebab51" />
            </span>
            <span class="centerSpan">
                <span class="textSpan">iv: </span>
                <asp:TextBox ID="TextBox_IV" runat="server" Enabled="false" Text="0x000ade1e" />
            </span>            
            <span class="rightSpan">
                <asp:Button ID="Button_Reset_KeyIV" runat="server" Text="Reset Key/IV" Enabled="false" ToolTip="Reset secret key / iv" OnClick="Button_Reset_KeyIV_Click" />
            </span>
        </div>
        <h2>En-/Decrypt file</h2>
        <INPUT id="oFile" type="file" runat="server" NAME="oFile" />
        <asp:Button ID="ButtonEncryptFile" runat="server" ToolTip="Encrypt file" OnClick="ButtonEncryptFile_Click" Text="Encrypt file" />
        <asp:Button ID="ButtonDecryptFile" runat="server" ToolTip="Decrypt file" OnClick="ButtonDecryptFile_Click" Text="Decrypt file" />        
        <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
            <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
            <br />
            <a id="aTransFormed" runat="server" alt="Transformed File" href="res/fortune.u8">
                <img id="imgOut" runat="server" border="0" alt="File transformed" src="res/img/file.png" /></a>
         </asp:Panel>
        <h2>En-/Decrypt text</h2>
        <asp:TextBox ID="TextBoxSource" runat="server" TextMode="MultiLine" MaxLength="32768" Rows="5" Columns="36" ToolTip="Transformed Text"></asp:TextBox>
        <asp:TextBox ID="TextBoxDestionation" runat="server" TextMode="MultiLine" Rows="5" Columns="36" MaxLength="16384" ToolTip="Destination Text"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonEncrypt" runat="server" Text="Encrypt" ToolTip="Encrypt" OnClick="ButtonEncrypt_Click" />
        <asp:Button ID="ButtonDecrypt" runat="server" Text="Decrypt" ToolTip="Decrypt" OnClick="ButtonDecrypt_Click" />        
    </form>
</asp:Content>
