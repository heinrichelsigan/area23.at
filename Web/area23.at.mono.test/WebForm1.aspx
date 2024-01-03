<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="area23.at.mono.test.WebForm1" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="res/area23.at.mono.test.css" />
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
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="bodydiv" runat="server">
	<table class="qrcTable" border="0" cellpadding="0" cellpadding="0">
		<tr id="tr0" class="qrcTr">
			<td id="td0a" class="qrcTdRight" width="18%" height="30pt">first name:</td>
			<td id="td0b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_FirstName" runat="server" ToolTip="enter first name" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_FirstName')" 
					MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
			<td id="td0c" class="qrcTdRight" width="18%" height="30pt">last name:</td>
			<td id="td0d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_LastName" runat="server" ToolTip="enter last name" MaxLength="84" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_LastName')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr1" class="qrcTr">
			<td id="td1a" class="qrcTdRight" width="18%" height="30pt">phone:</td>
			<td id="td1b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Phone" runat="server" ToolTip="enter phone number" MaxLength="84"  
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Phone')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
			<td id="td1c" class="qrcTdRight" width="18%" height="30pt">mobile:</td>
			<td id="td1d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Mobile" runat="server" ToolTip="enter mobile phone number" MaxLength="84"  
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Mobile')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr2" class="qrcTr">
			<td id="td2a" class="qrcTdRight" width="18%" height="30pt">e-mail:</td>
			<td id="td2b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Email" runat="server" ToolTip="enter email address" MaxLength="136"  
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Email')" 
					Width="128pt" Height="24pt" CssClass="QRTextBox"></asp:TextBox>
			</td>
			<td id="td2c" class="qrcTdRight" width="18%" height="30pt">www:</td>
			<td id="td2d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Web" runat="server" ToolTip="enter www web site" MaxLength="144"  
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Web')" 
					Width="136pt" Height="24pt" CssClass="QRTextBox"></asp:TextBox>
			</td>
		</tr>				
		<tr id="tr3" class="qrcTr">
			<td id="td3a" class="qrcTdRight" width="18%" height="30pt">country:</td>
			<td id="td3b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Coutry" runat="server" ToolTip="Enter country here" MaxLength="84"  
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Coutry')" 					
					Width="128pt" Height="24pt" CssClass="ButtonTextBox">Austria</asp:TextBox>
			</td>
			<td id="td3c" class="qrcTdRight" width="18%" height="30pt">region:</td>
			<td id="td3d" class="qrcTdLeft" width="32%" height="30pt">
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
			<td id="td4a" class="qrcTdRight" width="18%" height="30pt">city:</td>
			<td id="td4b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_City" runat="server" ToolTip="Enter city name" MaxLength="84"  Width="128pt" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_City')" 
					Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
			<td id="td4c" class="qrcTdRight" width="18%" height="30pt">zip code:</td>
			<td id="td4d" class="qrcTdLeft" width="325%" height="30pt">
				<asp:TextBox ID="TextBox_ZipCode" runat="server" ToolTip="Enter postal zip code" MaxLength="64"  
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_ZipCode')" 
					Width="60pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr5" class="qrcTr">
			<td id="td5a" class="qrcTdRight" width="18%" height="30pt">street:</td>
			<td id="td5b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Street" runat="server" ToolTip="Enter street name" MaxLength="128" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Street')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
			<td id="td5c" class="qrcTdRight" width="18%" height="30pt">house nr:</td>
			<td id="td5d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_StreetNr" runat="server" ToolTip="Enter street number" MaxLength="32" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_StreetNr')" 							
					Width="48pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr6" class="qrcTr">
			<td id="td6a" class="qrcTdRight" width="18%">
				<asp:Label id="labelOrg" runat="server" ToolTip="organisation" Text="org"></asp:Label>:
			</td>
			<td id="td6b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Org" runat="server" ToolTip="Enter organisation name" MaxLength="128" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Org')" 
						Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
			<td id="td6c" class="qrcTdRight" width="18%" height="192pt" rowspan="4">
				<span class="lefthuge">
					<asp:Button ID="Button_QRCode" runat="server" ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />
				</span>
			</td>
			<td id="td6d" class="qrcTdLeft" width="32%" height="192pt" rowspan="4">
				<span class="righthuge">
					<img id="ImgQR" runat="server" alt="QRCode" height="192" width="192" tooltip="QRCode" src="res/qrsample1.gif" />
				</span>
			</td>
		</tr>
		<tr id="tr7" class="qrcTr">
			<td id="td7a" class="qrcTdRight" width="18%">
				<asp:Label id="labelOrgTitle" runat="server" ToolTip="organisation titel" Text="org title"></asp:Label>:						
			</td>
			<td id="td7b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_OrgTitle" runat="server" ToolTip="Enter organisation title" MaxLength="128" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
						onchange="highLightOnChange('TextBox_OrgTitle')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>							
			</td>
		</tr>
		<tr id="tr8" class="qrcTr">
			<td id="td8a" class="qrcTdRight" width="18%">
				<asp:Label id="labelNote" runat="server" ToolTip="note" Text="note"></asp:Label>:
			</td>
			<td id="td8b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Note" runat="server" ToolTip="Enter personal note" MaxLength="128" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Note')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr9" class="qrcTr">
			<td id="td9a" class="qrcTdRight" width="18%">
				<asp:Label id="labelBirthday" runat="server" ToolTip="birthday" Text="birthday"></asp:Label>:
			</td>
			<td id="td9b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Birthday" runat="server" ToolTip="Enter birthday" MaxLength="128" 
					onkeypress="if (QRCodeGen_TextBoxKeyHandler(event) == false) return false;" 
					onchange="highLightOnChange('TextBox_Birthday')" 
					Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
			</td>
		</tr>
	</table>
	<div class="hugeqr">
		
		
	</div>
	<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
	</div>    
</asp:Content>
