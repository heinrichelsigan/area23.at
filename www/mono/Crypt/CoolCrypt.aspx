﻿<%@ Page Title="Simple uu and base64 en-/decode tool (apache2 mod_mono)" Language="C#" MasterPageFile="~/Crypt/EncodeMaster.master" AutoEventWireup="true" CodeBehind="CoolCrypt.aspx.cs" Inherits="Area23.At.Mono.Crypt.CoolCrypt"  validateRequest="false" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
        <title>Simple uu and base64 en-/decode tool (apache2 mod_mono)</title>
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta name="keywords" content="encode decode uuencode uudecode mime base64 aes encrypt decrypt" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" />
        <script type="text/javascript">

            function changeCryptBackgroundFile() {
                var divAes = document.getElementById("DivAesImprove");
                if (divAes != null) {
                    divAes.setAttribute("style", "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesBGFile.gif'); background-repeat: no-repeat; background-color: transparent;");
                    divAes.style.backgroundImage = "url('../res/img/AesBGFile.gif')";
                }
            }

            function changeCryptBackgroundText() {
                var divAes = document.getElementById("DivAesImprove");
                if (divAes != null) {
                    divAes.setAttribute("style", "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesBGText.gif'); background-repeat: no-repeat; background-color: transparent;");
                    divAes.style.backgroundImage = "url('../res/img/AesBGText.gif')";
                }
            }

        </script>
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">
    <h2>Enryption method</h2>
    <form id="CoolCryptForm" runat="server" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True" style="background-color: transparent;">
        <div style="background-color: transparent; padding: 0 0 0 0; margin: 0 0 0 0;">
            <div class="odDiv">
                <span class="leftSpan">&nbsp;User&nbsp;&nbsp;Secret:
                </span>
                <span class="centerSpan" style="max-width: 72px">
                    <asp:ImageButton ID="ImageButton_Key" ClientIDMode="Static" runat="server" ImageUrl="../res/img/a_right_key.png" 
                    OnClick="ImageButton_Key_Click" AlternateText="Key for symmetric cipher algorithm" />
                </span>
                <span class="centerSpan">               
                    <asp:TextBox ID="TextBox_Key" runat="server" Text="heinrich.elsigan@area23.at" AutoPostBack="true" OnTextChanged="TextBox_Key_TextChanged" 
                        ToolTip="Enter your personal email address or secret key here" MaxLength="192" Width="480px" style="width: 480px;" />
                </span>
                <span class="rightSpan">
                    <asp:Button ID="Button_Clear" runat="server" Text="Clear CryptPipe" OnClick="Button_Clear_Click" 
                        ToolTip="Clear SymChiffre Pipeline" style="width: 156px;" />
                </span>
            </div>    
            <div class="odDiv">
                <span class="textSpan">&nbsp;Key hash  iv: </span>
                <span class="centerSpan" style="max-width: 72px">
                    <asp:ImageButton ID="ImageButton_Hash" ClientIDMode="Static" runat="server" ImageUrl="../res/img/a_hash.png" 
                        OnClick="Button_Reset_KeyIV_Click" AlternateText="Hash generated from key" />
                </span>
                <span class="centerSpan">    
                    &nbsp;<asp:TextBox ID="TextBox_IV" runat="server" ReadOnly="true" Text="0x000ade1e"
                        MaxLength="192"  Width="480px"  style="width: 480px;" />
                </span>
                <span class="rightSpan">
                    <asp:Button ID="Button_Reset_KeyIV" runat="server" Text="Reset Key/IV" OnClick="Button_Reset_KeyIV_Click" 
                        ToolTip="Reset secret key / iv" style="width: 156px;" />
                </span>
            </div>
        </div>
        <div id="DivAesImprove" runat="server" style="padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;">
        
            <div class="odDiv">
                <span class="leftSpan">                  
                    <asp:DropDownList ID="DropDownList_Zip" runat="server" style="width: 72px;">
                        <asp:ListItem Enabled="true" Value="None" Selected="true">None</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Z7" Selected="false">7Zip</asp:ListItem>                
                        <asp:ListItem Enabled="true" Value="BZip2" Selected="false">BZip2</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="GZip" Selected="false">GZip</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Zip" Selected="false">Zip</asp:ListItem>         
                    </asp:DropDownList>
                    &rArr;
                </span>
                <span class="centerSpan">                    
                    <asp:DropDownList ID="DropDownList_Cipher" runat="server" style="width: 84px;">
                        <asp:ListItem Enabled="true" Value="None" Selected="true">None</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="DES3" Selected="false">3DES</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="FISH2" Selected="false">2FISH</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="FISH3" Selected="false">3FISH</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="AES" Selected="false">AES</asp:ListItem>              
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
                        <asp:ListItem Enabled="true" Value="SkipJack" Selected="false">SkipJack</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Tea" Selected="false">Tea</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Tnepres" Selected="false">Tnepres</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="XTea" Selected="false">XTea</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="ZenMatrix" Selected="false">ZenMatrix</asp:ListItem>
                    </asp:DropDownList>
                </span>
                <span class="centerSpan">
                    <asp:ImageButton ID="ImageButton_Add" ClientIDMode="Static" runat="server" ImageUrl="../res/img/AddAesArrow.gif" 
                        OnClick="ImageButton_Add_Click" AlternateText="Add symetric chiffer algorithm"
                        onmouseover="document.getElementById('ImageButton_Add').src='../res/img/AddAesArrowHover.gif'"                     
                        onmouseout="document.getElementById('ImageButton_Add').src='../res/img/AddAesArrow.gif'" />
                </span>
                <span class="centerSpan">
                    <asp:TextBox ID="TextBox_Encryption" runat="server" ReadOnly="true"  TextMode="SingleLine" MaxLength="512" 
                        Width="400px"  style="width: 400px;" />
                        &rArr;
                </span>
                <span class="rightSpan">
                     <asp:DropDownList ID="DropDownList_Encoding" runat="server" AutoPostBack="true" 
                         OnSelectedIndexChanged="DropDownList_Encoding_SelectedIndexChanged" style="width: 84px;">
                        <asp:ListItem Enabled="true" Value="None" Selected="false">None</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Base16" Selected="false">Base16</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Base32" Selected="false">Base32</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Hex32" Selected="false">Hex32</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Base64" Selected="true">Base64</asp:ListItem>                                
                        <asp:ListItem Enabled="true" Value="Uu" Selected="False">Uu</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Hex16" Selected="false">Hex16</asp:ListItem>
                    </asp:DropDownList>
                </span>
            </div>
            <hr />                
            <h3>En-/Decrypt file</h3>
            <div class="odDiv" style="vertical-align: top;">                       
                <span class="leftSpan" style="vertical-align: top;">
                    <INPUT id="oFile" type="file" runat="server" NAME="oFile" /> 
                </span>
                <span class="centerSpan" style="max-width: 72px; vertical-align: top;">
                    <asp:Button ID="ButtonEncryptFile" runat="server" ToolTip="Encrypt file" OnClientClick="changeCryptBackgroundFile();" OnClick="ButtonEncryptFile_Click" Text="Encrypt file" />
                </span>
                <span class="centerSpan" style="vertical-align: top;">     
                    <asp:CheckBox ID="CheckBoxEncode" runat="server" ToolTip="Encode file (e.g. hex16, base64, uu) after encryption" Text="encode file" Checked="true" />
                </span>
                <span class="rightSpan" style="vertical-align: top;">     
                    <asp:Button ID="ButtonDecryptFile" runat="server" ToolTip="Decrypt file" OnClientClick="changeCryptBackgroundFile();" OnClick="ButtonDecryptFile_Click" Text="Decrypt file" />  
                </span>
            </div>     
            <div class="odDiv">                       
                <span id="SpanLeftFile" runat="server" class="leftSpan" style="vertical-align: top;" visible="false">
                    <a id="aUploaded" runat="server" alt="Uploaded File" href="../res/img/file.png">
                        <img id="img1" runat="server" border="0" alt="" src="../res/img/file.png" />
                    </a>
                </span>
                <span id="SpanLabel" runat="server" class="centerSpan" visible="False">
                    <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
                </span>
                <span class="centerSpan">&nbsp;</span>
                <span id="SpanRightFile" runat="server" class="rightSpan" style="vertical-align: top;" visible="false">
                    <a id="aTransFormed" runat="server" alt="Transformed File" href="../res/fortune.u8">
                        <img id="imgOut" runat="server" border="0" alt="File transformed" src="../res/img/file.png" />
                    </a>
                </span>                
            </div>
            <br />
            <h3>En-/Decrypt text</h3>
            <div style="background-color: transparent; padding: 0 0 0 0; margin: 0 0 0 0;">                
                <asp:TextBox ID="TextBoxSource" runat="server" TextMode="MultiLine" MaxLength="32768" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="Source Text" Text="[Enter text to en-/decrypt here]" Width="480px"></asp:TextBox>
                <asp:TextBox ID="TextBoxDestionation" runat="server" TextMode="MultiLine" Rows="10" Columns="48" MaxLength="32768" ReadOnly="true" ToolTip="Destination Text" Width="468px"></asp:TextBox>
                <br />
                <asp:Button ID="ButtonEncrypt" runat="server" Text="Encrypt" ToolTip="Encrypt" OnClientClick="changeCryptBackgroundText()" OnClick="ButtonEncrypt_Click" />
                <asp:Button ID="ButtonDecrypt" runat="server" Text="Decrypt" ToolTip="Decrypt" OnClientClick="changeCryptBackgroundText();" OnClick="ButtonDecrypt_Click" />   
            </div>
        </div>
        <hr />   
        <h3>Great thanks to <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>!</h3>
    </form>
</asp:Content>
