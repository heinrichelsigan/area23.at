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
                    divAes.setAttribute("style", "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/crypt/AesBGFile.gif'); background-repeat: no-repeat; background-color: transparent;");
                    divAes.style.backgroundImage = "url('../res/img/crypt/AesBGFile.gif')";
                }
            }

            function changeCryptBackgroundText() {
                var divAes = document.getElementById("DivAesImprove");
                if (divAes != null) {
                    divAes.setAttribute("style", "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/crypt/AesBGText.gif'); background-repeat: no-repeat; background-color: transparent;");
                    divAes.style.backgroundImage = "url('../res/img/crypt/AesBGText.gif')";
                }
            }

        </script>
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server" ClientIDMode="Static">
    <h2>Enryption method</h2>
    <form id="ZenMatrixVisualizeForm" runat="server" method="post" enableviewstate="True"  style="background-color: transparent;">
        <div style="background-color: transparent; padding-left: 40px; margin-left: 2px;">
            <div class="odDiv">
                <span class="centerSpan" style="width: 72px">&nbsp&nbsp;Secret&nbsp;key:&nbsp;</span>
                <span class="centerSpan" style="width: 72px"><asp:ImageButton ID="ImageButton_Key" runat="server"  
                    ClientIDMode="Static" ImageUrl="../res/img/crypt/a_right_key.png" 
                    AlternateText="save your user key in session" /></span>
                <span class="centerSpan" style="max-width: 400px;">                
                    <asp:TextBox ID="TextBox_Key" runat="server" Text="heinrich.elsigan@gmail.com"  AutoPostBack="true" 
                        OnTextChanged="TextBox_Key_TextChanged"                       
                        ToolTip="Enter your personal email address or secret key here" MaxLength="256" Width="520px" style="width: 520px;" />
                </span>
                <span class="rightSpan" style="width: 72px">
                    <asp:Button ID="Button_Clear" runat="server" Text="clear" OnClick="Button_Clear_Click" 
                        ToolTip="Clear SymChiffre Pipeline" style="max-width: 72px" />
                </span>
				<span class="rightSpan" style="width: 72px">
					<asp:CheckBox ID="CheckBox_FullSymmetric" runat="server" ClientIDMode="Static" Text="full fymmetric"
					 ToolTip="FullSymmetric ZenMatrix"  OnCheckedChanged="CheckBox_FullSymmetric_OnCheckedChanged" Checked="true" AutoPostBack="true" style="max-width: 72px" />
				</span>
            </div>    
            <div class="odDiv">
                <span class="centerSpan" style="width: 72px">Key&nbsp;hash/iv:&nbsp;</span>
                <span class="centerSpan" style="width: 72px"><asp:ImageButton ID="ImageButton_Hash" runat="server"  
                    OnClick="Button_Hash_Click" ClientIDMode="Static" ImageUrl="../res/img/crypt/a_hash.png" 
                    AlternateText="Generate new hash from key" /></span>                
                <span class="centerSpan" style="max-width: 400px;"><asp:TextBox ID="TextBox_IV" runat="server" 
                    ToolTip="key generated hash" ReadOnly="true" Text="" MaxLength="256"  Width="520px"  style="width: 520px;" />
                </span>
                <span class="rightSpan" style="width: 72px">   
					<asp:TextBox ID="TextBoxPermutation" runat="server" 
						ToolTip="ZenMatrix permutation key" ReadOnly="true" Text="" MaxLength="128"  Width="120px"  style="width: 120px;" />
                </span>
            </div>
			<div class="odDiv" style="margin-top: 4px">
                <span class="leftSpan" style="white-space: nowrap; width:92%; text-align: left;">
                    <asp:RadioButtonList ID="RadioButtonList_Hash" runat="server" AutoPostBack="true" ToolTip="choose hashing key method" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList_Hash_ParameterChanged"> 
						<asp:ListItem Selected="False" Value="BCrypt">bcrypt</asp:ListItem>
						<asp:ListItem Selected="False" Value="Blake2xs">blake2xs</asp:ListItem>
						<asp:ListItem Selected="False" Value="CShake">cshake</asp:ListItem>
						<asp:ListItem Selected="False" Value="Dstu7564">dstu7564</asp:ListItem>
						<asp:ListItem Selected="True" Value="Hex">hex hash</asp:ListItem>
						<asp:ListItem Selected="False" Value="MD5">md5</asp:ListItem>
						<asp:ListItem Selected="False" Value="OpenBSDCrypt">openbsd crypt</asp:ListItem>
						<asp:ListItem Selected="False" Value="Oct">octal</asp:ListItem>
						<asp:ListItem Selected="False" Value="RipeMD256">ripemd256</asp:ListItem>
						<asp:ListItem Selected="False" Value="SCrypt">scrypt</asp:ListItem>
						<asp:ListItem Selected="False" Value="Sha1">sha1 key</asp:ListItem>
						<asp:ListItem Selected="False" Value="Sha256">sha256</asp:ListItem>
						<asp:ListItem Selected="False" Value="TupleHash">tuplehash</asp:ListItem>
						<asp:ListItem Selected="False" Value="Whirlpool">whirlpool</asp:ListItem>
                    </asp:RadioButtonList>                 
                </span>                
                <span class="centerSpan" style="margin-left: 20px; max-width: 800px; min-width: 720px;">
                    &nbsp;
                </span>
            </div>        
		</div>
		<br />                
        <div style="width: 800px; height: 600px; min-height: 480px">
			<table class="rpnTbl" border="0" cellpadding="0" cellspacing="0" runat="server" id="MatrixTable" width="49%" height="56%">
				<tr class="rpnTr" runat="server" id="tr_f" width="49%" height="3%">					
					<td class="rpnTd" colspan=2 width="6%" id="td_f_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_f_0" runat="server" align="center"><asp:TextBox ID="TextBox_f_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_1" runat="server" align="center"><asp:TextBox ID="TextBox_f_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_2" runat="server" align="center"><asp:TextBox ID="TextBox_f_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_3" runat="server" align="center"><asp:TextBox ID="TextBox_f_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_4" runat="server" align="center"><asp:TextBox ID="TextBox_f_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_5" runat="server" align="center"><asp:TextBox ID="TextBox_f_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_6" runat="server" align="center"><asp:TextBox ID="TextBox_f_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_7" runat="server" align="center"><asp:TextBox ID="TextBox_f_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_8" runat="server" align="center"><asp:TextBox ID="TextBox_f_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_9" runat="server" align="center"><asp:TextBox ID="TextBox_f_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_a" runat="server" align="center"><asp:TextBox ID="TextBox_f_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_b" runat="server" align="center"><asp:TextBox ID="TextBox_f_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_c" runat="server" align="center"><asp:TextBox ID="TextBox_f_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_d" runat="server" align="center"><asp:TextBox ID="TextBox_f_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_e" runat="server" align="center"><asp:TextBox ID="TextBox_f_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_f_f" runat="server" align="center"><asp:TextBox ID="TextBox_f_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_f_sf" runat="server" align="right">x0&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_f_vf" runat="server" align="center"><asp:TextBox ID="TextBox_f_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_e" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_e_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_e_0" runat="server" align="center"><asp:TextBox ID="TextBox_e_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_1" runat="server" align="center"><asp:TextBox ID="TextBox_e_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_2" runat="server" align="center"><asp:TextBox ID="TextBox_e_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_3" runat="server" align="center"><asp:TextBox ID="TextBox_e_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_4" runat="server" align="center"><asp:TextBox ID="TextBox_e_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_5" runat="server" align="center"><asp:TextBox ID="TextBox_e_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_6" runat="server" align="center"><asp:TextBox ID="TextBox_e_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_7" runat="server" align="center"><asp:TextBox ID="TextBox_e_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_8" runat="server" align="center"><asp:TextBox ID="TextBox_e_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_9" runat="server" align="center"><asp:TextBox ID="TextBox_e_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_a" runat="server" align="center"><asp:TextBox ID="TextBox_e_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_b" runat="server" align="center"><asp:TextBox ID="TextBox_e_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_c" runat="server" align="center"><asp:TextBox ID="TextBox_e_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_d" runat="server" align="center"><asp:TextBox ID="TextBox_e_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_e" runat="server" align="center"><asp:TextBox ID="TextBox_e_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_e_f" runat="server" align="center"><asp:TextBox ID="TextBox_e_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_e_sf" runat="server" align="right">x1&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_e_vf" runat="server" align="center"><asp:TextBox ID="TextBox_e_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_d" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_d_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_d_0" runat="server" align="center"><asp:TextBox ID="TextBox_d_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_1" runat="server" align="center"><asp:TextBox ID="TextBox_d_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_2" runat="server" align="center"><asp:TextBox ID="TextBox_d_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_3" runat="server" align="center"><asp:TextBox ID="TextBox_d_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_4" runat="server" align="center"><asp:TextBox ID="TextBox_d_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_5" runat="server" align="center"><asp:TextBox ID="TextBox_d_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_6" runat="server" align="center"><asp:TextBox ID="TextBox_d_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_7" runat="server" align="center"><asp:TextBox ID="TextBox_d_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_8" runat="server" align="center"><asp:TextBox ID="TextBox_d_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_9" runat="server" align="center"><asp:TextBox ID="TextBox_d_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_a" runat="server" align="center"><asp:TextBox ID="TextBox_d_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_b" runat="server" align="center"><asp:TextBox ID="TextBox_d_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_c" runat="server" align="center"><asp:TextBox ID="TextBox_d_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_d" runat="server" align="center"><asp:TextBox ID="TextBox_d_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_e" runat="server" align="center"><asp:TextBox ID="TextBox_d_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_d_f" runat="server" align="center"><asp:TextBox ID="TextBox_d_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_d_sf" runat="server" align="right">x2&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_d_vf" runat="server" align="center"><asp:TextBox ID="TextBox_d_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_c" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_c_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_c_0" runat="server" align="center"><asp:TextBox ID="TextBox_c_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_1" runat="server" align="center"><asp:TextBox ID="TextBox_c_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_2" runat="server" align="center"><asp:TextBox ID="TextBox_c_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_3" runat="server" align="center"><asp:TextBox ID="TextBox_c_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_4" runat="server" align="center"><asp:TextBox ID="TextBox_c_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_5" runat="server" align="center"><asp:TextBox ID="TextBox_c_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_6" runat="server" align="center"><asp:TextBox ID="TextBox_c_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_7" runat="server" align="center"><asp:TextBox ID="TextBox_c_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_8" runat="server" align="center"><asp:TextBox ID="TextBox_c_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_9" runat="server" align="center"><asp:TextBox ID="TextBox_c_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_a" runat="server" align="center"><asp:TextBox ID="TextBox_c_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_b" runat="server" align="center"><asp:TextBox ID="TextBox_c_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_c" runat="server" align="center"><asp:TextBox ID="TextBox_c_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_d" runat="server" align="center"><asp:TextBox ID="TextBox_c_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_e" runat="server" align="center"><asp:TextBox ID="TextBox_c_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_c_f" runat="server" align="center"><asp:TextBox ID="TextBox_c_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_c_sf" runat="server" align="right">x3&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="tdc_vf" runat="server" align="center"><asp:TextBox ID="TextBox_c_vf" runat="server" ClientIDMode="Static"  Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_b" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_b_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_b_0" runat="server" align="center"><asp:TextBox ID="TextBox_b_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_1" runat="server" align="center"><asp:TextBox ID="TextBox_b_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_2" runat="server" align="center"><asp:TextBox ID="TextBox_b_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_3" runat="server" align="center"><asp:TextBox ID="TextBox_b_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_4" runat="server" align="center"><asp:TextBox ID="TextBox_b_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_5" runat="server" align="center"><asp:TextBox ID="TextBox_b_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_6" runat="server" align="center"><asp:TextBox ID="TextBox_b_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_7" runat="server" align="center"><asp:TextBox ID="TextBox_b_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_8" runat="server" align="center"><asp:TextBox ID="TextBox_b_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_9" runat="server" align="center"><asp:TextBox ID="TextBox_b_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_a" runat="server" align="center"><asp:TextBox ID="TextBox_b_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_b" runat="server" align="center"><asp:TextBox ID="TextBox_b_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_c" runat="server" align="center"><asp:TextBox ID="TextBox_b_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_d" runat="server" align="center"><asp:TextBox ID="TextBox_b_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_e" runat="server" align="center"><asp:TextBox ID="TextBox_b_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_b_f" runat="server" align="center"><asp:TextBox ID="TextBox_b_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_b_sf" runat="server" align="right">x4&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_b_vf" runat="server" align="center"><asp:TextBox ID="TextBox_b_vf" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>					
				</tr>
				<tr class="rpnTr" runat="server" id="tr_a" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_a_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_a_0" runat="server" align="center"><asp:TextBox ID="TextBox_a_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_1" runat="server" align="center"><asp:TextBox ID="TextBox_a_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_2" runat="server" align="center"><asp:TextBox ID="TextBox_a_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_3" runat="server" align="center"><asp:TextBox ID="TextBox_a_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_4" runat="server" align="center"><asp:TextBox ID="TextBox_a_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_5" runat="server" align="center"><asp:TextBox ID="TextBox_a_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_6" runat="server" align="center"><asp:TextBox ID="TextBox_a_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_7" runat="server" align="center"><asp:TextBox ID="TextBox_a_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_8" runat="server" align="center"><asp:TextBox ID="TextBox_a_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_9" runat="server" align="center"><asp:TextBox ID="TextBox_a_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_a" runat="server" align="center"><asp:TextBox ID="TextBox_a_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_b" runat="server" align="center"><asp:TextBox ID="TextBox_a_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_c" runat="server" align="center"><asp:TextBox ID="TextBox_a_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_d" runat="server" align="center"><asp:TextBox ID="TextBox_a_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_e" runat="server" align="center"><asp:TextBox ID="TextBox_a_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_a_f" runat="server" align="center"><asp:TextBox ID="TextBox_a_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_a_sf" runat="server" align="right">x5&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_a_vf" runat="server" align="center"><asp:TextBox ID="TextBox_a_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_9" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_9_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_9_0" runat="server" align="center"><asp:TextBox ID="TextBox_9_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_1" runat="server" align="center"><asp:TextBox ID="TextBox_9_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_2" runat="server" align="center"><asp:TextBox ID="TextBox_9_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_3" runat="server" align="center"><asp:TextBox ID="TextBox_9_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_4" runat="server" align="center"><asp:TextBox ID="TextBox_9_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_5" runat="server" align="center"><asp:TextBox ID="TextBox_9_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_6" runat="server" align="center"><asp:TextBox ID="TextBox_9_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_7" runat="server" align="center"><asp:TextBox ID="TextBox_9_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_8" runat="server" align="center"><asp:TextBox ID="TextBox_9_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_9" runat="server" align="center"><asp:TextBox ID="TextBox_9_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_a" runat="server" align="center"><asp:TextBox ID="TextBox_9_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_b" runat="server" align="center"><asp:TextBox ID="TextBox_9_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_c" runat="server" align="center"><asp:TextBox ID="TextBox_9_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_d" runat="server" align="center"><asp:TextBox ID="TextBox_9_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_e" runat="server" align="center"><asp:TextBox ID="TextBox_9_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_9_f" runat="server" align="center"><asp:TextBox ID="TextBox_9_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_9_sf" runat="server" align="right">x6&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_9_vf" runat="server" align="center"><asp:TextBox ID="TextBox_9_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_8" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_8_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_8_0" runat="server" align="center"><asp:TextBox ID="TextBox_8_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_1" runat="server" align="center"><asp:TextBox ID="TextBox_8_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_2" runat="server" align="center"><asp:TextBox ID="TextBox_8_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_3" runat="server" align="center"><asp:TextBox ID="TextBox_8_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_4" runat="server" align="center"><asp:TextBox ID="TextBox_8_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_5" runat="server" align="center"><asp:TextBox ID="TextBox_8_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_6" runat="server" align="center"><asp:TextBox ID="TextBox_8_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_7" runat="server" align="center"><asp:TextBox ID="TextBox_8_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_8" runat="server" align="center"><asp:TextBox ID="TextBox_8_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_9" runat="server" align="center"><asp:TextBox ID="TextBox_8_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_a" runat="server" align="center"><asp:TextBox ID="TextBox_8_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_b" runat="server" align="center"><asp:TextBox ID="TextBox_8_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_c" runat="server" align="center"><asp:TextBox ID="TextBox_8_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_d" runat="server" align="center"><asp:TextBox ID="TextBox_8_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_e" runat="server" align="center"><asp:TextBox ID="TextBox_8_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_8_f" runat="server" align="center"><asp:TextBox ID="TextBox_8_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_8_sf" runat="server" align="right">x7&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_8_vf" runat="server" align="center"><asp:TextBox ID="TextBox_8_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_7" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_7_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_7_0" runat="server" align="center"><asp:TextBox ID="TextBox_7_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_1" runat="server" align="center"><asp:TextBox ID="TextBox_7_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_2" runat="server" align="center"><asp:TextBox ID="TextBox_7_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_3" runat="server" align="center"><asp:TextBox ID="TextBox_7_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_4" runat="server" align="center"><asp:TextBox ID="TextBox_7_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_5" runat="server" align="center"><asp:TextBox ID="TextBox_7_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_6" runat="server" align="center"><asp:TextBox ID="TextBox_7_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_7" runat="server" align="center"><asp:TextBox ID="TextBox_7_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_8" runat="server" align="center"><asp:TextBox ID="TextBox_7_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_9" runat="server" align="center"><asp:TextBox ID="TextBox_7_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_a" runat="server" align="center"><asp:TextBox ID="TextBox_7_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_b" runat="server" align="center"><asp:TextBox ID="TextBox_7_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_c" runat="server" align="center"><asp:TextBox ID="TextBox_7_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_d" runat="server" align="center"><asp:TextBox ID="TextBox_7_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_e" runat="server" align="center"><asp:TextBox ID="TextBox_7_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_7_f" runat="server" align="center"><asp:TextBox ID="TextBox_7_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_7_sf" runat="server" align="right">x8&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_7_vf" runat="server" align="center"><asp:TextBox ID="TextBox_7_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_6" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_6_v1" runat="server" align="center">&nbsp;</td>		
					<td class="azureTd" width="3%" id="td_6_0" runat="server" align="center"><asp:TextBox ID="TextBox_6_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_1" runat="server" align="center"><asp:TextBox ID="TextBox_6_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_2" runat="server" align="center"><asp:TextBox ID="TextBox_6_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_3" runat="server" align="center"><asp:TextBox ID="TextBox_6_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_4" runat="server" align="center"><asp:TextBox ID="TextBox_6_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_5" runat="server" align="center"><asp:TextBox ID="TextBox_6_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_6" runat="server" align="center"><asp:TextBox ID="TextBox_6_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_7" runat="server" align="center"><asp:TextBox ID="TextBox_6_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_8" runat="server" align="center"><asp:TextBox ID="TextBox_6_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_9" runat="server" align="center"><asp:TextBox ID="TextBox_6_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_a" runat="server" align="center"><asp:TextBox ID="TextBox_6_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_b" runat="server" align="center"><asp:TextBox ID="TextBox_6_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_c" runat="server" align="center"><asp:TextBox ID="TextBox_6_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_d" runat="server" align="center"><asp:TextBox ID="TextBox_6_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_e" runat="server" align="center"><asp:TextBox ID="TextBox_6_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_6_f" runat="server" align="center"><asp:TextBox ID="TextBox_6_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_6_sf" runat="server" align="right">x9&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_6_vf" runat="server" align="center"><asp:TextBox ID="TextBox_6_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_5" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_5_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_5_0" runat="server" align="center"><asp:TextBox ID="TextBox_5_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_1" runat="server" align="center"><asp:TextBox ID="TextBox_5_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_2" runat="server" align="center"><asp:TextBox ID="TextBox_5_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_3" runat="server" align="center"><asp:TextBox ID="TextBox_5_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_4" runat="server" align="center"><asp:TextBox ID="TextBox_5_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_5" runat="server" align="center"><asp:TextBox ID="TextBox_5_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_6" runat="server" align="center"><asp:TextBox ID="TextBox_5_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_7" runat="server" align="center"><asp:TextBox ID="TextBox_5_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_8" runat="server" align="center"><asp:TextBox ID="TextBox_5_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_9" runat="server" align="center"><asp:TextBox ID="TextBox_5_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_a" runat="server" align="center"><asp:TextBox ID="TextBox_5_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_b" runat="server" align="center"><asp:TextBox ID="TextBox_5_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_c" runat="server" align="center"><asp:TextBox ID="TextBox_5_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_d" runat="server" align="center"><asp:TextBox ID="TextBox_5_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_e" runat="server" align="center"><asp:TextBox ID="TextBox_5_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_5_f" runat="server" align="center"><asp:TextBox ID="TextBox_5_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_5_sf" runat="server" align="right">xa&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_v_f5" runat="server" align="center"><asp:TextBox ID="TextBox_5_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_4" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_4_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_4_0" runat="server" align="center"><asp:TextBox ID="TextBox_4_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_1" runat="server" align="center"><asp:TextBox ID="TextBox_4_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_2" runat="server" align="center"><asp:TextBox ID="TextBox_4_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_3" runat="server" align="center"><asp:TextBox ID="TextBox_4_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_4" runat="server" align="center"><asp:TextBox ID="TextBox_4_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_5" runat="server" align="center"><asp:TextBox ID="TextBox_4_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_6" runat="server" align="center"><asp:TextBox ID="TextBox_4_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_7" runat="server" align="center"><asp:TextBox ID="TextBox_4_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_8" runat="server" align="center"><asp:TextBox ID="TextBox_4_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_9" runat="server" align="center"><asp:TextBox ID="TextBox_4_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_a" runat="server" align="center"><asp:TextBox ID="TextBox_4_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_b" runat="server" align="center"><asp:TextBox ID="TextBox_4_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_c" runat="server" align="center"><asp:TextBox ID="TextBox_4_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_d" runat="server" align="center"><asp:TextBox ID="TextBox_4_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_e" runat="server" align="center"><asp:TextBox ID="TextBox_4_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_4_f" runat="server" align="center"><asp:TextBox ID="TextBox_4_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_4_sf" runat="server" align="right">xb&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_4_vf" runat="server" align="center"><asp:TextBox ID="TextBox_4_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_3" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_3_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_3_0" runat="server" align="center"><asp:TextBox ID="TextBox_3_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_1" runat="server" align="center"><asp:TextBox ID="TextBox_3_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_2" runat="server" align="center"><asp:TextBox ID="TextBox_3_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_3" runat="server" align="center"><asp:TextBox ID="TextBox_3_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_4" runat="server" align="center"><asp:TextBox ID="TextBox_3_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_5" runat="server" align="center"><asp:TextBox ID="TextBox_3_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_6" runat="server" align="center"><asp:TextBox ID="TextBox_3_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_7" runat="server" align="center"><asp:TextBox ID="TextBox_3_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_8" runat="server" align="center"><asp:TextBox ID="TextBox_3_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_9" runat="server" align="center"><asp:TextBox ID="TextBox_3_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_a" runat="server" align="center"><asp:TextBox ID="TextBox_3_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_b" runat="server" align="center"><asp:TextBox ID="TextBox_3_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_c" runat="server" align="center"><asp:TextBox ID="TextBox_3_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_d" runat="server" align="center"><asp:TextBox ID="TextBox_3_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_e" runat="server" align="center"><asp:TextBox ID="TextBox_3_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_3_f" runat="server" align="center"><asp:TextBox ID="TextBox_3_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_3_sf" runat="server" align="right">xc&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_3_vf" runat="server" align="center"><asp:TextBox ID="TextBox_3_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_2" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_2_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_2_0" runat="server" align="center"><asp:TextBox ID="TextBox_2_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_1" runat="server" align="center"><asp:TextBox ID="TextBox_2_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_2" runat="server" align="center"><asp:TextBox ID="TextBox_2_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_3" runat="server" align="center"><asp:TextBox ID="TextBox_2_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_4" runat="server" align="center"><asp:TextBox ID="TextBox_2_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_5" runat="server" align="center"><asp:TextBox ID="TextBox_2_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_6" runat="server" align="center"><asp:TextBox ID="TextBox_2_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_7" runat="server" align="center"><asp:TextBox ID="TextBox_2_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_8" runat="server" align="center"><asp:TextBox ID="TextBox_2_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_9" runat="server" align="center"><asp:TextBox ID="TextBox_2_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_a" runat="server" align="center"><asp:TextBox ID="TextBox_2_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_b" runat="server" align="center"><asp:TextBox ID="TextBox_2_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_c" runat="server" align="center"><asp:TextBox ID="TextBox_2_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_d" runat="server" align="center"><asp:TextBox ID="TextBox_2_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_e" runat="server" align="center"><asp:TextBox ID="TextBox_2_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_2_f" runat="server" align="center"><asp:TextBox ID="TextBox_2_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_2_sf" runat="server" align="right">xd&nbsp;&#x21D2;</td>						
					<td class="azureTd" width="3%" id="td_2_vf" runat="server" align="center"><asp:TextBox ID="TextBox_2_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_1" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_1_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_1_0" runat="server" align="center"><asp:TextBox ID="TextBox_1_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_1" runat="server" align="center"><asp:TextBox ID="TextBox_1_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_2" runat="server" align="center"><asp:TextBox ID="TextBox_1_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_3" runat="server" align="center"><asp:TextBox ID="TextBox_1_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_4" runat="server" align="center"><asp:TextBox ID="TextBox_1_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_5" runat="server" align="center"><asp:TextBox ID="TextBox_1_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_6" runat="server" align="center"><asp:TextBox ID="TextBox_1_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_7" runat="server" align="center"><asp:TextBox ID="TextBox_1_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_8" runat="server" align="center"><asp:TextBox ID="TextBox_1_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_9" runat="server" align="center"><asp:TextBox ID="TextBox_1_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_a" runat="server" align="center"><asp:TextBox ID="TextBox_1_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_b" runat="server" align="center"><asp:TextBox ID="TextBox_1_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_c" runat="server" align="center"><asp:TextBox ID="TextBox_1_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_d" runat="server" align="center"><asp:TextBox ID="TextBox_1_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_e" runat="server" align="center"><asp:TextBox ID="TextBox_1_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_1_f" runat="server" align="center"><asp:TextBox ID="TextBox_1_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_1_sf" runat="server" align="right">xe&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_1_vf" runat="server" align="center"><asp:TextBox ID="TextBox_1_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
				<tr class="rpnTr" runat="server" id="tr_0" width="49%" height="3%">
					<td class="rpnTd" colspan=2 width="6%" id="td_0_v1" runat="server" align="center">&nbsp;</td>	
					<td class="azureTd" width="3%" id="td_0_0" runat="server" align="center"><asp:TextBox ID="TextBox_0_0" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_1" runat="server" align="center"><asp:TextBox ID="TextBox_0_1" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_2" runat="server" align="center"><asp:TextBox ID="TextBox_0_2" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_3" runat="server" align="center"><asp:TextBox ID="TextBox_0_3" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_4" runat="server" align="center"><asp:TextBox ID="TextBox_0_4" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_5" runat="server" align="center"><asp:TextBox ID="TextBox_0_5" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_6" runat="server" align="center"><asp:TextBox ID="TextBox_0_6" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_7" runat="server" align="center"><asp:TextBox ID="TextBox_0_7" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_8" runat="server" align="center"><asp:TextBox ID="TextBox_0_8" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_9" runat="server" align="center"><asp:TextBox ID="TextBox_0_9" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_a" runat="server" align="center"><asp:TextBox ID="TextBox_0_a" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_b" runat="server" align="center"><asp:TextBox ID="TextBox_0_b" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_c" runat="server" align="center"><asp:TextBox ID="TextBox_0_c" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_d" runat="server" align="center"><asp:TextBox ID="TextBox_0_d" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_e" runat="server" align="center"><asp:TextBox ID="TextBox_0_e" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="azureTd" width="3%" id="td_0_f" runat="server" align="center"><asp:TextBox ID="TextBox_0_f" runat="server" ClientIDMode="Static" Text="0"  style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt"  /></td>
					<td class="rpnTd" colspan=2 width="6%" id="td_0_sf" runat="server" align="right">xf&nbsp;&#x21D2;</td>	
					<td class="azureTd" width="3%" id="td_0_vf" runat="server" align="center"><asp:TextBox ID="TextBox_0_vf" runat="server" ClientIDMode="Static" Text="0" style="width: 24pt; height: 24pt; min-width: 16pt; max-width: 36pt" /></td>
				</tr>
			</table>
        </div>            
		<br />
    </form>
	<div style="clear: both">		
		<h3>Great thanks to <a href="https://www.bouncycastle.org/download/bouncy-castle-c/" target="_blank">bouncycastle.org</a>!</h3>			
		<br />
	</div>
</asp:Content>
