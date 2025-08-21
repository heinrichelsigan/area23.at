<%@ Page Title="BCrypt (apache2 mod_mono)" Language="C#" MasterPageFile="~/Crypt/EncodeMaster.master" AutoEventWireup="true" CodeBehind="BCrypt.aspx.cs" Inherits="Area23.At.Mono.Crypt.BCrypt"  validateRequest="true" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
        <title>Simple uu and base64 en-/decode tool (apache2 mod_mono)</title>
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta name="keywords" content="encode decode uuencode uudecode mime base64 aes encrypt decrypt" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" />       
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">
    <h2>Enryption method</h2>
    <form id="CoolCryptForm" runat="server" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True" style="background-color: transparent;">
        <div style="background-color: transparent; padding-left: 40px; margin-left: 2px;">
            <div class="odDiv">
                <span class="leftSpan" style="width: 60px; min-width: 48px; max-width: 72px">secret&nbsp;key:</span>
                <span class="centerSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    <asp:ImageButton ID="ImageButton_Key" runat="server" ClientIDMode="Static"
                    OnClick="Button_Key_Click" ImageUrl="../res/img/crypt/a_right_key.png" AlternateText="save your user key in session" />
                </span>
                <span class="centerSpan" style="max-width: 400px;">                
                    <asp:TextBox ID="TextBox_Key" runat="server" ClientIDMode="Static" Text="heinrich.elsigan@area23.at"                        
                        ToolTip="Enter your personal email address or secret key here" MaxLength="192" Width="480px" style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    <asp:Button ID="Button_Clear" runat="server" Text="clear" OnClick="Button_Clear_Click" 
                        ToolTip="Clear SymChiffre Pipeline" style="width: 60px; min-width: 48px; max-width: 72px" />
                </span>
            </div>    
            <div class="odDiv">
                <span class="leftSpan" style="width: 60px; min-width: 48px; max-width: 72px">key&nbsp;hash:</span>
                <span class="centerSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    <asp:ImageButton ID="ImageButton_Hash" runat="server" ClientIDMode="Static"
                    OnClick="Button_Hash_Click" ImageUrl="../res/img/crypt/a_hash.png" AlternateText="generate new hash from key" />
                </span>                
                <span class="centerSpan" style="max-width: 400px;"><asp:TextBox ID="TextBox_IV" runat="server" ClientIDMode="Static"
                    ToolTip="key generated hash" ReadOnly="true" Text="" MaxLength="192"  Width="480px"  style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 80px; min-width: 72px; max-width: 84px">
                    &nbsp;
                </span>
            </div>
            <div class="odDiv">
                <span class="leftSpan" style="width: 60px; min-width: 48px; max-width: 72px">bcrypt&nbsp;key:</span>
                <span class="centerSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                <asp:Button ID="Button_BCrypt" runat="server" Text="bcrypt" ClientIDMode="Static"
                    OnClick="Button_BCrypt_Click" ToolTip="bcrypt key and hash" style="min-width: 48px; max-width: 72px" />
                </span>
                <span class="centerSpan" style="max-width: 400px;">                
                    <asp:TextBox ID="TextBox_BCrypt_Key" runat="server" ClientIDMode="Static" Text=""                        
                        ToolTip="BCrypted key" MaxLength="192" Width="480px" style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    &nbsp; 
                </span>
            </div>    
            <div class="odDiv">
                <span class="leftSpan" style="width: 60px; min-width: 48px; max-width: 72px">crypt&nbsp;hash:</span>
                <span class="centerSpan" style="width: 60px; min-width: 48px; max-width: 72px">
                    <asp:Button ID="Button_BCrypt_Hash" runat="server" Text="hash" ClientIDMode="Static" 
                        OnClick="Button_BCrypt_Hash_Click" ToolTip="calculates an iv hash" style="min-width: 48px; max-width: 72px" />
                </span>                
                <span class="centerSpan" style="max-width: 400px;"><asp:TextBox ID="TextBox_BCrypt_Hash" runat="server" ClientIDMode="Static"
                    ToolTip="bcrypted key generated hash" ReadOnly="true" Text="" MaxLength="192"  Width="480px"  style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 80px; min-width: 72px; max-width: 84px">
                    &nbsp;
                </span>
            </div>
                
        </div>
        <div id="DivAesImprove" runat="server" style="padding-left: 40px; margin-left: 2px; background-repeat: no-repeat; background-color: transparent;">
            <div class="odDiv">
                <span class="leftSpan" style="width: 72px">
                    Hint: bcrypt is implemented via <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>
                </span>
                <span class="centerSpan" style="width: 72px">&nbsp;</span>
                <span class="centerSpan" style="width: 72px"></span>                
                <span class="centerSpan" style="max-width: 400px;">
                </span>
                <span class="rightSpan" style="width: 72px">
                </span>
            </div>
        </div>
        <hr />   
        <h3>Great thanks to <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>!</h3>
    </form>
</asp:Content>
