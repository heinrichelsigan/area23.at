<%@ Page Title="HashKey (apache2 mod_mono)" Language="C#" MasterPageFile="~/Crypt/EncodeMaster.master" AutoEventWireup="true" CodeBehind="HashKey.aspx.cs" Inherits="Area23.At.Mono.Crypt.HashKey"  validateRequest="true" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
        <title>Simple uu and base64 en-/decode tool (apache2 mod_mono)</title>
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta name="keywords" content="encode decode uuencode uudecode mime base64 aes encrypt decrypt" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" />       
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">
    <h2>Enryption method</h2>
    <form id="HashKeyForm" runat="server" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True" style="background-color: transparent;">
        <div style="background-color: transparent; padding-left: 40px; margin-left: 2px;">
            <div class="odDiv">
                <span class="leftSpan" style="width: 60px; min-width: 48px; max-width: 72px">secret&nbsp;key:</span>
                <span class="centerSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    <asp:ImageButton ID="ImageButton_Key" runat="server" ClientIDMode="Static"
                    OnClick="Button_Key_Click" ImageUrl="../res/img/crypt/a_right_key.png" AlternateText="save your user key in session" />
                </span>
                <span class="centerSpan" style="max-width: 400px;">                
                    <asp:TextBox ID="TextBox_Key" runat="server" ClientIDMode="Static" Text="heinrich.elsigan@area23.at"  AutoPostBack="true" 
                        OnTextChanged="Button_Key_Click" ToolTip="Enter your personal email address or secret key here" MaxLength="192" Width="480px" style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 120px; min-width: 108px; max-width: 144px">                    
                    <asp:Button ID="Button_Hash" runat="server" Text="Hash" ClientIDMode="Static"
                        OnClick="Button_Key_Click" ToolTip="bcrypt key and hash" style="min-width: 48px; max-width: 72px" />
                    <asp:Button ID="Button_Clear" runat="server" Text="clear" OnClick="Button_Clear_Click" 
                        ToolTip="Clear SymChiffre Pipeline" style="width: 60px; min-width: 48px; max-width: 72px" />
                </span>
            </div>
            <div class="odDiv">
                <span class="leftSpan" style="width: 60px; min-width: 48px; max-width: 72px">secret key hash:</span>
                <span class="centerSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    &nbsp;
                </span>
                <span class="centerSpan" style="max-width: 600px;">                
                    <asp:TextBox ID="TextBox_BCrypt_Key" runat="server" ClientIDMode="Static" Text=""                        
                        ToolTip="hashed key" MaxLength="192" Width="532px" style="width: 532px;" />
                </span>
            </div>                    
            <div class="odDiv" style="margin-top: 4px">
                <span class="leftSpan" style="white-space: nowrap; width:80%; text-align: left;"
                    <asp:RadioButtonList ID="RadioButtonList_Hash" runat="server" AutoPostBack="true" ToolTip="choose hashing key method" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList_Hash_ParameterChanged"> 
                        <asp:ListItem Selected="False" Value="BCrypt">bcrypt key</asp:ListItem>
                        <asp:ListItem Selected="True" Value="Hex">hex hash key</asp:ListItem>
                        <asp:ListItem Selected="False" Value="MD5">md5 key</asp:ListItem>
                        <asp:ListItem Selected="False" Value="OpenBSDCrypt">openbsd crypt</asp:ListItem>
                        <asp:ListItem Selected="False" Value="SCrypt">scrypt key</asp:ListItem>
                        <asp:ListItem Selected="False" Value="Sha1">sha1 key</asp:ListItem>
                        <asp:ListItem Selected="False" Value="Sha256">sha256 key</asp:ListItem>
                        <asp:ListItem Selected="False" Value="Sha384">sha384 key</asp:ListItem>
                        <asp:ListItem Selected="False" Value="Sha512">sha256 key</asp:ListItem>
                    </asp:RadioButtonList>                    
                </span> 
            </div>
        </div>
        <div id="DivAesImprove" runat="server" style="padding-left: 40px; margin-left: 2px; background-repeat: no-repeat; background-color: transparent;">
            <div class="odDiv">
                <span class="leftSpan">
                    Hint: bcrypt, openbsd crypt and scrypt were implemented by <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>
                </span>
            </div>
        </div>                   
        <hr />   
        <h3>Great thanks to <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>!</h3>
    </form>
</asp:Content>
