<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCodeGen.aspx.cs" Inherits="area23.at.mono.test.QRCodeGen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QRCode apache2 mod_mono generator</title>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
	<script type="text/javascript">

		function QRCodeGen_TextBoxKeyHandler(event) {
			var target = event.target;
			if ((target == null) || (typeof (target) == "undefined")) target = event.srcElement;
			if (event.keyCode == 13) {
				if ((typeof (target) != "undefined") && (target != null)) {
					if (typeof (target.onchange) != "undefined") {
						target.onchange();
						event.cancelBubble = true;
						if (event.stopPropagation) event.stopPropagation();
						return false;
					}
				}
			}
			return true;
		}

		function highLightOnChange(highLightId) { 
			if (highLightId != null && document.getElementById(highLightId) != null) {
				if (document.getElementById(highLightId).style.borderStyle == "dotted" ||
					document.getElementById(highLightId).style.borderColor == "red") {
					// do nothing when dotted
				}
				else {
					// set border-width: 1; border-style: dashed
					document.getElementById(highLightId).style.borderColor = "red";
					document.getElementById(highLightId).style.borderStyle = "dashed";
				}
			}
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>    
			<table class="qrcTable" border="0" cellpadding="0" cellpadding="0">
				<tr id="tr0" class="qrcTr">
					<td id="td0a" class="qrcTdRight" width="25%" height="30pt">first name:</td>
					<td id="td0b" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_FirstName" runat="server" ToolTip="enter first name" 
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_FirstName')" 
							MaxLength="84" Width="84pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
					</td>
					<td id="td0c" class="qrcTdRight" width="25%" height="30pt">last name:</td>
					<td id="td0d" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_LastName" runat="server" ToolTip="enter last name" MaxLength="84" 
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_LastName')" 
							Width="84pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
					</td>
				</tr>
				<tr id="tr1" class="qrcTr">
					<td id="td1a" class="qrcTdRight" width="25%" height="30pt">phone:</td>
					<td id="td1b" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_Phone" runat="server" ToolTip="enter phone number" MaxLength="84"  
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_Phone')" 
							Width="84pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
					</td>
					<td id="td1c" class="qrcTdRight" width="25%" height="30pt">mobile:</td>
					<td id="td1d" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_Mobile" runat="server" ToolTip="enter mobile phone number" MaxLength="84"  
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_Mobile')" 
							Width="84pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
					</td>
				</tr>
				<tr id="tr2" class="qrcTr">
					<td id="td2a" class="qrcTdRight" width="25%" height="30pt">e-mail:</td>
					<td id="td2b" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_Email" runat="server" ToolTip="enter email address" MaxLength="128"  
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_Email')" 
							Width="108pt" Height="24pt" CssClass="QRTextBox"></asp:TextBox>
					</td>
					<td id="td2c" class="qrcTdRight" width="25%" height="30pt">www:</td>
					<td id="td2d" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_Web" runat="server" ToolTip="enter www web site" MaxLength="128"  
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_Web')" 
							Width="108pt" Height="24pt" CssClass="QRTextBox"></asp:TextBox>
					</td>
				</tr>				
				<tr id="tr3" class="qrcTr">
					<td id="td3a" class="qrcTdRight" width="25%" height="30pt">country:</td>
					<td id="td3b" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_Coutry" runat="server" ToolTip="Enter country here" MaxLength="84"  
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_Coutry')" 					
							Width="84pt" Height="24pt" CssClass="ButtonTextBox">Austria</asp:TextBox>
					</td>
					<td id="td3c" class="qrcTdRight" width="25%" height="30pt">region:</td>
					<td id="td3d" class="qrcTdLeft" width="25%" height="30pt">
						<asp:DropDownList ID="DropDown_Country" runat="server" CssClass="DropDownList" onchange="highLightOnChange('DropDown_Country')" ToolTip="Choose country">
							<asp:ListItem>Africa</asp:ListItem>
							<asp:ListItem>All America</asp:ListItem>
							<asp:ListItem> North America</asp:ListItem>
							<asp:ListItem>  Canada</asp:ListItem>	
							<asp:ListItem>  United States of America</asp:ListItem>
							<asp:ListItem> Middle America</asp:ListItem>
							<asp:ListItem>  Mexico</asp:ListItem>
							<asp:ListItem> South America</asp:ListItem>
							<asp:ListItem>  Argentinia</asp:ListItem>
							<asp:ListItem>  Brazil</asp:ListItem>
							<asp:ListItem>  Chile</asp:ListItem>	
							<asp:ListItem Selected="True">Europe</asp:ListItem>   
							<asp:ListItem>  France</asp:ListItem>
							<asp:ListItem>  Germany</asp:ListItem>
							<asp:ListItem>  Great Britain</asp:ListItem>
							<asp:ListItem>  Switzerland</asp:ListItem>
							<asp:ListItem>  United Kingdom</asp:ListItem>
							<asp:ListItem>Middle East</asp:ListItem>
							<asp:ListItem>  Iran</asp:ListItem>
							<asp:ListItem>  Israel</asp:ListItem>	
							<asp:ListItem>  Saudi Arabia</asp:ListItem>
							<asp:ListItem>  United Arab Emirates</asp:ListItem>
							<asp:ListItem>Russian federation</asp:ListItem>
							<asp:ListItem>  Russia</asp:ListItem>
							<asp:ListItem>  White Russia</asp:ListItem>
							<asp:ListItem>  Tazikistan</asp:ListItem>
							<asp:ListItem>  Kasachistan</asp:ListItem>
							<asp:ListItem>  Uzbekistan</asp:ListItem>	
							<asp:ListItem>Hindu</asp:ListItem>
							<asp:ListItem>  Bangladesh</asp:ListItem>
							<asp:ListItem>  India</asp:ListItem>
							<asp:ListItem>  Pakistan</asp:ListItem>		
							<asp:ListItem>Australia</asp:ListItem>    
							<asp:ListItem>Eurasia</asp:ListItem>
							<asp:ListItem>Asia</asp:ListItem>	
							<asp:ListItem> China</asp:ListItem>	
							<asp:ListItem> Japan</asp:ListItem>
							<asp:ListItem> Korea</asp:ListItem>
							<asp:ListItem>Oceania</asp:ListItem>	
							<asp:ListItem>Antarctica (south polar region)</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr id="tr4" class="qrcTr">
					<td id="td4a" class="qrcTdRight" width="25%" height="30pt">city:</td>
					<td id="td4b" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_City" runat="server" ToolTip="Enter city name" MaxLength="84"  Width="84pt" 
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_City')" 
							Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
					</td>
					<td id="td4c" class="qrcTdRight" width="25%" height="30pt">zip code:</td>
					<td id="td4d" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_ZipCode" runat="server" ToolTip="Enter postal zip code" MaxLength="64"  
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_ZipCode')" 
							Width="60pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
					</td>
				</tr>
				<tr id="tr6" class="qrcTr">
					<td id="td5a" class="qrcTdRight" width="25%" height="30pt">street:</td>
					<td id="td5b" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_Street" runat="server" ToolTip="Enter street name" MaxLength="128" 
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_Street')" 
							Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
					</td>
					<td id="td5c" class="qrcTdRight" width="25%" height="30pt">house nr:</td>
					<td id="td5d" class="qrcTdLeft" width="25%" height="30pt">
						<asp:TextBox ID="TextBox_StreetNr" runat="server" ToolTip="Enter street number" MaxLength="32" 
							onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
							onchange="highLightOnChange('TextBox_StreetNr')" 							
							Width="48pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
					</td>
				</tr>
			</table>
			<div class="hugeqr">
				<span class="lefthuge">
					<asp:Button ID="Button_QRCode" runat="server" ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />
				</span>
				<span class="righthuge">
					<img id="ImgQR" runat="server" alt="QRCode" height="192" width="192" tooltip="QRCode" src="image1.gif" />
				</span>
			</div>
			<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
			</div>
            <hr />
            <div align="left" class="footerDiv">            
                <span class="footerLeft" align="left" valign="middle"><a href="/cgi/fortune.cgi">fortune</a></span>
	            <span class="footerLeftCenter" align="center" valign="middle"><a href="/froga/">froga</a></span>
	            <span class="footerCenter" align="center" valign="middle"><a href="/mono/test/HexDump.aspx">hex dump</a></span>			
                <span class="footerCenter" align="center" valign="middle"><a href="/mono/test/Qrc.aspx">qrcode gen</a></span>	
	            <span class="footerRightCenter" align="center" valign="middle"><a href="/mono/SchnapsNet/">schnapsen 66</a></span>
	            <span class="footerRight" align="right" valign="middle"><a href="mailto:he@area23.at">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a></span>
            </div>
        </div>
    </form>
</body>
</html>
