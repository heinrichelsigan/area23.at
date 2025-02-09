<%@ Page Title="ZenMatrixVisualize" Language="C#" MasterPageFile="~/Crypt/EncodeMaster.master" AutoEventWireup="true" CodeBehind="ZenMatrixVisualize.aspx.cs" Inherits="Area23.At.Mono.Crypt.ZenMatrixVisualize"  validateRequest="false" %>
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
    <form id="ZenMatrixVisualizeForm" runat="server" method="post" enableviewstate="True"  style="background-color: transparent;">
        <div style="background-color: transparent; padding-left: 40px; margin-left: 2px;">
            <div class="odDiv">
                <span class="leftSpan" style="width: 72px">
                    <asp:Button ID="Button_Key" runat="server" Text="Key " ClientIDMode="Static"
                        OnClick="Button_Key_Click" ToolTip="save your user key in session" style="max-width: 72px" />
                </span>
                <span class="centerSpan" style="width: 72px">&nbsp&nbsp;Secret&nbsp;key:&nbsp;</span>
                <span class="centerSpan" style="width: 72px"><asp:ImageButton ID="ImageButton_Key" runat="server"  
                    ClientIDMode="Static" ImageUrl="../res/img/a_right_key.png" 
                    AlternateText="save your user key in session" /></span>
                <span class="centerSpan" style="max-width: 400px;">                
                    <asp:TextBox ID="TextBox_Key" runat="server" Text="heinrich.elsigan@area23.at"                        
                        ToolTip="Enter your personal email address or secret key here" MaxLength="192" Width="480px" style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 72px">
                    <asp:Button ID="Button_Clear" runat="server" Text="clear" OnClick="Button_Clear_Click" 
                        ToolTip="Clear SymChiffre Pipeline" style="max-width: 72px" />
                </span>
            </div>    
            <div class="odDiv">
                <span class="leftSpan" style="width: 72px">
                    <asp:Button ID="Button_Hash" runat="server" Text="hash" ClientIDMode="Static"
                        OnClick="Button_Hash_Click" ToolTip="save your user key in session" style="max-width: 72px" />      
                </span>
                <span class="centerSpan" style="width: 72px">Key&nbsp;hash/iv:&nbsp;</span>
                <span class="centerSpan" style="width: 72px"><asp:ImageButton ID="ImageButton_Hash" runat="server"  
                    OnClick="Button_Hash_Click" ClientIDMode="Static" ImageUrl="../res/img/a_hash.png" 
                    AlternateText="Generate new hash from key" /></span>                
                <span class="centerSpan" style="max-width: 400px;"><asp:TextBox ID="TextBox_IV" runat="server" 
                    ToolTip="key generated hash" ReadOnly="true" Text="" MaxLength="192"  Width="480px"  style="width: 480px;" />
                </span>
                <span class="rightSpan" style="width: 72px">
                    <asp:Button ID="Button_Reset_KeyIV" runat="server" Text="reset" ClientIDMode="Static" 
                        OnClick="Button_Reset_KeyIV_Click" ToolTip="Reset secret key / iv" style="max-width: 72px" />
                </span>
            </div>
        </div>
        <div id="DivAesImprove" runat="server" style="padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;">
        
            <div class="odDiv">
                <span class="leftSpan" style="width: 72px;">                      
                    <asp:DropDownList ID="DropDownList_Zip" runat="server" Enabled="false" style="width: 64px;">
                        <asp:ListItem Enabled="true" Value="None" Selected="true">None</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Z7" Selected="false">7Zip</asp:ListItem>                
                        <asp:ListItem Enabled="true" Value="BZip2" Selected="false">BZip2</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="GZip" Selected="false">GZip</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Zip" Selected="false">Zip</asp:ListItem>         
                    </asp:DropDownList>
                    &rArr;
                </span>
                <span class="centerSpan" style="width: 72px;">                    
                    <asp:DropDownList ID="DropDownList_Cipher" runat="server" Enabled="false" style="width: 72px;">
                        <asp:ListItem Enabled="true" Value="Aes" Selected="false">Aes</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="BlowFish" Selected="false">BlowFish</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Fish2" Selected="false">Fish2</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Fish3" Selected="false">Fish3</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Camellia" Selected="false">Camellia</asp:ListItem>              
                        <asp:ListItem Enabled="true" Value="Cast5" Selected="False">Cast5</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Cast6" Selected="False">Cast6</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Des3" Selected="False">Des3</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Gost28147" Selected="False">Gost28147</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Idea" Selected="false">Idea</asp:ListItem>                        
                        <asp:ListItem Enabled="true" Value="Noekeon" Selected="false">Noekeon</asp:ListItem>                        
                        <asp:ListItem Enabled="true" Value="RC2" Selected="false">RC2</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="RC532" Selected="false">RC532</asp:ListItem>   
                        <asp:ListItem Enabled="true" Value="RC564" Selected="false">RC564</asp:ListItem> 
                        <asp:ListItem Enabled="true" Value="RC6" Selected="false">RC6</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Rsa" Selected="false">Rsa</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Seed" Selected="false">Seed</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Serpent" Selected="false">Serpent</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="SkipJack" Selected="false">SkipJack</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Tea" Selected="false">Tea</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Tnepres" Selected="false">Tnepres</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="XTea" Selected="false">XTea</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="ZenMatrix" Selected="true">ZenMatrix</asp:ListItem>
                    </asp:DropDownList>
                </span>
                <span class="centerSpan" style="width: 72px;"> 
                    <asp:ImageButton ID="ImageButton_Add" ClientIDMode="Static" Enabled="false"  runat="server" ImageUrl="../res/img/AddAesArrow.gif" 
                        AlternateText="Add symetric chiffer algorithm" />
                </span>
                <span class="centerSpan" style="max-width: 400px;">
                    <asp:TextBox ID="TextBox_Encryption" runat="server" ReadOnly="true" TextMode="SingleLine" MaxLength="512"
                        Width="400px" Style="width: 400px;" />ZenMatrix;
                        &rArr;
                </span>
                <span class="rightSpan">
                     <asp:DropDownList ID="DropDownList_Encoding" runat="server" Enabled="false" style="width: 84px;">
                        <asp:ListItem Enabled="true" Value="None" Selected="false">None</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Base16" Selected="false">Base16</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Hex16" Selected="false">Hex16</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Base32" Selected="false">Base32</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Hex32" Selected="false">Hex32</asp:ListItem>
                        <asp:ListItem Enabled="true" Value="Base64" Selected="true">Base64</asp:ListItem>                                
                        <asp:ListItem Enabled="true" Value="Uu" Selected="False">Uu</asp:ListItem>
                    </asp:DropDownList>
                </span>
            </div>
            <div class="odDiv">
                <span class="leftSpan" style="width: 72px">
                   ZenMatrix Visualize Testform
                </span>
                <span class="centerSpan" style="width: 72px">&nbsp;</span>
                <span class="centerSpan" style="width: 72px"></span>                
                <span class="centerSpan" style="max-width: 400px;">
                </span>
                <span class="rightSpan" style="width: 72px">
                </span>
            </div>
            <br />                
            <div style="width: 320px; width: 420px; height: 260px; min-height: 224px">
                <pre id="zenmatrix" runat="server" style="width: 320px; width: 420px; height: 260px; min-height: 224px">
| 0 |=> | 0 1 2 3 4 5 6 7 |<br />
| 1 |=> | 1 |
| 2 |=> | 2 |
| 3 |=> | 3 |
| 4 |=> | 4 |
| 5 |=> | 5 |                        
| 5 |=> | 6 |
| 7 |=> | 5 |
                    </pre>             
            </div>
            <br />            
        </div>
        <hr />   
        <h3>Great thanks to <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>!</h3>
    </form>
</asp:Content>
