﻿<%@ Page Title="Simple uu and base64 en-/decode tool (apache2 mod_mono)" Language="C#" MasterPageFile="~/Encode/EncodeMaster.master" AutoEventWireup="true" CodeBehind="AesImprove.aspx.cs" Inherits="Area23.At.Mono.Encode.AesImprove"  validateRequest="false" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
        <title>Simple uu and base64 en-/decode tool (apache2 mod_mono)</title>
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta name="keywords" content="encode decode uuencode uudecode mime base64 aes encrypt decrypt" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" />
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">
    <form id="AesImproveForm" runat="server" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">
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
                    <asp:ListItem Enabled="true" Value="Rijndael" Selected="false">Rijndael</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="RC2" Selected="false">RC2</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="RC532" Selected="false">RC532</asp:ListItem>                
                    <asp:ListItem Enabled="true" Value="RC6" Selected="false">RC6</asp:ListItem>
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
                <asp:ImageButton ID="ImageButton_Add" ClientIDMode="Static" runat="server" ImageUrl="../res/img/a_right.gif" 
                    OnClick="ImageButton_Add_Click" AlternateText="Add symetric chiffer algorithm"
                    onmouseover="document.getElementById('ImageButton_Add').src='../res/img/a_right_hover.gif'"                     
                    onmouseout="document.getElementById('ImageButton_Add').src='../res/img/a_right.gif'" />
            </span>
            <span class="centerSpan">
                <asp:TextBox ID="TextBox_Encryption" runat="server" ReadOnly="true"  TextMode="SingleLine" Text="" Width="416px" MaxLength="512" />
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_Clear" runat="server" Text="Clear CryptPipe" ToolTip="Clear SymChiffre Pipeline" OnClick="Button_Clear_Click" />
            </span>
        </div>
        <div class="odDiv">
            <span class="leftSpan">&nbsp;User&nbsp;&nbsp;Secret:
            </span>
            <span class="centerSpan" style="max-width: 72px">
                <asp:ImageButton ID="ImageButton_Key" ClientIDMode="Static" runat="server" ImageUrl="../res/img/a_right_key.png" 
                OnClick="Button_Reset_KeyIV_Click" AlternateText="Key for symmetric cipher algorithm" />
            </span>
            <span class="centerSpan">               
               <asp:TextBox ID="TextBox_Key" runat="server" ToolTip="Enter your personal email address or secret key here" Text="heinrich.elsigan@area23.at" MaxLength="256" Width="416px" AutoPostBack="true" OnTextChanged="TextBox_Key_TextChanged" />
            </span>
            <span class="rightSpan">
                <asp:DropDownList ID="DropDownList_Encoding" runat="server">
                    <asp:ListItem Enabled="true" Value="hex16" Selected="false">Hex16</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base16" Selected="false">Base16</asp:ListItem>                
                    <asp:ListItem Enabled="true" Value="base32" Selected="false">Base32</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base32hex" Selected="false">Base32Hex</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base64" Selected="true">Base64</asp:ListItem>                                
                    <asp:ListItem Enabled="true" Value="uu" Selected="False">Uu</asp:ListItem>
                </asp:DropDownList>
            </span>
        </div>
        <div class="odDiv">
            <span class="leftSpan">
                <span class="textSpan">&nbsp;Key hash  iv: </span>
            </span>           
            <span class="centerSpan" style="max-width: 72px">
                <asp:ImageButton ID="ImageButton_Hash" ClientIDMode="Static" runat="server" ImageUrl="../res/img/a_hash.png" 
                    OnClick="Button_Reset_KeyIV_Click" AlternateText="Hash generated from key" />
            </span>
            <span class="centerSpan">    
                &nbsp;<asp:TextBox ID="TextBox_IV" runat="server" ReadOnly="true" Text="0x000ade1e" MaxLength="192"  Width="416px" />
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_Reset_KeyIV" runat="server" Text="Reset Key/IV" ToolTip="Reset secret key / iv" OnClick="Button_Reset_KeyIV_Click" />
            </span>
        </div>
        <hr />
        <h3>To encrypt with valid private key / iv, see <a href="CoolCrypt.aspx" target="_blank">CoolCrypt.aspx</a>!</h3>
        <h2>En-/Decrypt file</h2>
        <INPUT id="oFile" type="file" runat="server" NAME="oFile" />
        <asp:Button ID="ButtonEncryptFile" runat="server" ToolTip="Encrypt file" OnClick="ButtonEncryptFile_Click" Text="Encrypt file" />
        <asp:Button ID="ButtonDecryptFile" runat="server" ToolTip="Decrypt file" OnClick="ButtonDecryptFile_Click" Text="Decrypt file" />        
        <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
            <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
            <br />
            <a id="aTransFormed" runat="server" alt="Transformed File" href="../res/fortune.u8">
                <img id="imgOut" runat="server" border="0" alt="File transformed" src="../res/img/file.png" /></a>
         </asp:Panel>
        <h2>En-/Decrypt text</h2>
        <asp:TextBox ID="TextBoxSource" runat="server" TextMode="MultiLine" MaxLength="32768" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="Source Text" Text="[Enter text to en-/decrypt here]" Width="480px"></asp:TextBox>
        <asp:TextBox ID="TextBoxDestionation" runat="server" TextMode="MultiLine" Rows="10" Columns="48" MaxLength="32768" ReadOnly="true" ToolTip="Destination Text" Width="468px"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonEncrypt" runat="server" Text="Encrypt" ToolTip="Encrypt" OnClick="ButtonEncrypt_Click" />
        <asp:Button ID="ButtonDecrypt" runat="server" Text="Decrypt" ToolTip="Decrypt" OnClick="ButtonDecrypt_Click" />   
        <hr />
        <h3>Great thanks to <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>!</h3>
    </form>
</asp:Content>
