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
                    <asp:ListItem Enabled="true" Value="Cast5" Selected="False">Cast5</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Cast6" Selected="False">Cast6</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Camellia" Selected="False">Camellia</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Gost28147" Selected="False">Gost28147</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Idea" Selected="false">Idea</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Noekeon" Selected="false">Noekeon</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="RC2" Selected="false">RC2</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="RC532" Selected="false">RC532</asp:ListItem>                    
                    <asp:ListItem Enabled="true" Value="RC6" Selected="false">RC6</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Rijndael" Selected="false">Rijndael</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Seed" Selected="false">Seed</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Serpent" Selected="false">Serpent</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Skipjack" Selected="false">Skipjack</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Tea" Selected="false">Tea</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="Tnepres" Selected="false">Tnepres</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="XTea" Selected="false">XTea</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="ZenMatrix" Selected="false">ZenMatrix</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="centerSpan">
                <asp:ImageButton ID="ImageButton_Add" ClientIDMode="Static" runat="server" ImageUrl="res/img/a_right.gif" 
                    OnClick="ImageButton_Add_Click" AlternateText="Add symetric chiffer algorithm"
                    onmouseover="document.getElementById('ImageButton_Add').src='res/img/a_right_hover.gif'"                     
                    onmouseout="document.getElementById('ImageButton_Add').src='res/img/a_right.gif'" />
            </span>
            <span class="centerSpan">
                <asp:TextBox ID="TextBox_Encryption" runat="server" ReadOnly="true" TextMode="SingleLine" Text="" Width="416px" MaxLength="512" />
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_Clear" runat="server" Text="Clear" ToolTip="Clear SymChiffre Pipeline" OnClick="Button_Clear_Click" />
            </span>
        </div>
        <div class="odDiv">
            <span class="leftSpan">
                Secret key:
            </span>
            <span class="centerSpan">                
                <asp:TextBox ID="TextBox_Key" runat="server" ToolTip="Enter your personal email address or secret key here" Text="heinrich.elsigan@area23.at" MaxLength="256" Width="216px" AutoPostBack="true" OnTextChanged="TextBox_Key_TextChanged" />
            </span>
            <span class="centerSpan">
                <span class="textSpan">iv: </span>
                <asp:TextBox ID="TextBox_IV" runat="server" ReadOnly="true" Text="0x000ade1e" MaxLength="128" />
            </span>            
            <span class="rightSpan">
                <asp:Button ID="Button_Reset_KeyIV" runat="server" Text="Reset Key/IV" ToolTip="Reset secret key / iv" OnClick="Button_Reset_KeyIV_Click" />
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
        <asp:TextBox ID="TextBoxSource" runat="server" TextMode="MultiLine" MaxLength="32768" Rows="8" Columns="40" ToolTip="Source Text" Text="[Enter text to en-/decrypt here]"></asp:TextBox>
        <asp:TextBox ID="TextBoxDestionation" runat="server" TextMode="MultiLine" Rows="8" Columns="40" MaxLength="32768" ReadOnly="true" ToolTip="Destination Text"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonEncrypt" runat="server" Text="Encrypt" ToolTip="Encrypt" OnClick="ButtonEncrypt_Click" />
        <asp:Button ID="ButtonDecrypt" runat="server" Text="Decrypt" ToolTip="Decrypt" OnClick="ButtonDecrypt_Click" />        
    </form>
</asp:Content>
