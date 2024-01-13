<%@ Page Language="C#" MasterPageFile="~/QRMaster.master" AutoEventWireup="true" CodeBehind="QRCodeGen.aspx.cs" Inherits="area23.at.www.mono.QRCodeGen" %>
<asp:Content ID="QrHeadContent" ContentPlaceHolderID="QrHead" runat="server" ClientIDMode="Static">
    <title>qr contact (apache2 mod_mono)</title>
    <link rel="stylesheet" href="res/area23.at.www.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
	<script type="text/javascript">

        function highLightOnChange(highLightId) {
            if (highLightId != null && document.getElementById(highLightId) != null) {
                if (document.getElementById(highLightId).style.borderStyle == "dotted" ||
                    document.getElementById(highLightId).style.borderColor == "red") {
                    // do nothing when dotted
				}
                else if (document.getElementById(highLightId).style.borderStyle == "dashed" ||
						document.getElementById(highLightId).style.borderColor == "red") {

					document.getElementById(highLightId).style.borderStyle = "dotted";
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
<asp:Content ID="QrBodyContent" ContentPlaceHolderID="QrBody" runat="server" ClientIDMode="Static">
	<table class="qrcTable" border="0" cellpadding="0" cellpadding="0">
		<tr id="tr0" class="qrcTr">
			<td id="td0a" class="qrcTdRight" width="18%" height="30pt">first name:</td>
			<td id="td0b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_FirstName" ClientIDMode="Static" runat="server" ToolTip="enter first name"
					AutoCompleteType="FirstName"
                    MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft" 					
                    onchange="highLightOnChange('TextBox_FirstName')"></asp:TextBox>
			</td>
			<td id="td0c" class="qrcTdRight" width="18%" height="30pt">last name:</td>
			<td id="td0d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_LastName" runat="server" ToolTip="enter last name" 
					AutoCompleteType="LastName"  ClientIDMode="Static"
					MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxRight"
					onchange="highLightOnChange('TextBox_LastName')"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr1" class="qrcTr">
			<td id="td1a" class="qrcTdRight" width="18%" height="30pt">phone:</td>
			<td id="td1b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Phone" runat="server" ToolTip="enter phone number"  
					TextMode="Phone" AutoCompleteType="HomePhone"  ClientIDMode="Static"
					MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
					onchange="highLightOnChange('TextBox_Phone')"></asp:TextBox>
			</td>
			<td id="td1c" class="qrcTdRight" width="18%" height="30pt">mobile:</td>
			<td id="td1d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Mobile" runat="server" ToolTip="enter mobile phone number" 
					TextMode="Phone" AutoCompleteType="Cellular"  ClientIDMode="Static"
					MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxRight"
					onchange="highLightOnChange('TextBox_Mobile')"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr2" class="qrcTr">
			<td id="td2a" class="qrcTdRight" width="18%" height="30pt">e-mail:</td>
			<td id="td2b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Email" runat="server" ToolTip="enter email address"
					TextMode="Email" AutoCompleteType="Email"  ClientIDMode="Static"
					MaxLength="136" Width="128pt" Height="24pt" CssClass="QRTextBox"
					onchange="highLightOnChange('TextBox_Email')" ></asp:TextBox>
			</td>
			<td id="td2c" class="qrcTdRight" width="18%" height="30pt">www:</td>
			<td id="td2d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Web" runat="server" ToolTip="enter www web site"
					TextMode="Url" AutoCompleteType="Homepage"  ClientIDMode="Static"
					MaxLength="144" Width="136pt" Height="24pt" CssClass="QRTextBox"
					onchange="highLightOnChange('TextBox_Web')"></asp:TextBox>
			</td>
		</tr>				
		<tr id="tr3" class="qrcTr">
			<td id="td3a" class="qrcTdRight" width="18%" height="30pt">country:</td>
			<td id="td3b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Coutry" runat="server" ToolTip="Enter country here"
					TextMode="SingleLine" AutoCompleteType="HomeState"   ClientIDMode="Static"
					MaxLength="84" Width="128pt" Height="24pt" CssClass="ButtonTextBox"
					onchange="highLightOnChange('TextBox_Coutry')">Austria</asp:TextBox>
			</td> 
			<td id="td3c" class="qrcTdRight" width="18%" height="30pt">region:</td>
			<td id="td3d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Region" runat="server" ToolTip="Enter region here"
					TextMode="SingleLine" AutoCompleteType="HomeCountryRegion"   ClientIDMode="Static"
					MaxLength="84" Width="128pt" Height="24pt" CssClass="ButtonTextBox"
					onchange="highLightOnChange('TextBox_Region')"></asp:TextBox>				
			</td>
		</tr>
		<tr id="tr4" class="qrcTr">
			<td id="td4a" class="qrcTdRight" width="18%" height="30pt">city:</td>
			<td id="td4b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_City" runat="server" ToolTip="Enter city name" 
					AutoCompleteType="HomeCity"  ClientIDMode="Static" 
					MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
					onchange="highLightOnChange('TextBox_City')"></asp:TextBox>
			</td>
			<td id="td4c" class="qrcTdRight" width="18%" height="30pt">zip code:</td>
			<td id="td4d" class="qrcTdLeft" width="325%" height="30pt">
				<asp:TextBox ID="TextBox_ZipCode" runat="server" ToolTip="Enter postal zip code" 
					AutoCompleteType="HomeZipCode"  ClientIDMode="Static"
					MaxLength="64" Width="60pt" Height="24pt" CssClass="QRTextBoxRight"
					onchange="highLightOnChange('TextBox_ZipCode')"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr5" class="qrcTr">
			<td id="td5a" class="qrcTdRight" width="18%" height="30pt">street:</td>
			<td id="td5b" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Street" runat="server" ToolTip="Enter street name" 
					AutoCompleteType="HomeStreetAddress"   ClientIDMode="Static"
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"					
					onchange="highLightOnChange('TextBox_Street')" ></asp:TextBox>
			</td>
			<td id="td5c" class="qrcTdRight" width="18%" height="30pt">house nr:</td>
			<td id="td5d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_StreetNr" runat="server" ToolTip="Enter street number" 
					TextMode="Number"  ClientIDMode="Static"
					MaxLength="32" Width="48pt" Height="24pt" CssClass="QRTextBoxRight"
					onchange="highLightOnChange('TextBox_StreetNr')"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr6" class="qrcTr">
			<td id="td6a" class="qrcTdRight" width="18%">
				<asp:Label id="labelOrg" runat="server" ToolTip="organisation" Text="org"></asp:Label>:
			</td>
			<td id="td6b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Org" runat="server" ToolTip="Enter organisation name" 
					AutoCompleteType="Department"  ClientIDMode="Static"
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
					onchange="highLightOnChange('TextBox_Org')"></asp:TextBox>
			</td>
			<td id="td6c" class="qrcTdRight" width="18%" height="192pt" rowspan="4">
				<span class="lefthuge">
                    <asp:TextBox ID="TextBox_Color" runat="server" ClientIDMode="Static" TextMode="Color" Text="#8c1157" />
					<br />
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
				<asp:TextBox ID="TextBox_OrgTitle" runat="server" ToolTip="Enter organisation title" 
					AutoCompleteType="Department"
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
					onchange="highLightOnChange('TextBox_OrgTitle')"></asp:TextBox>							
			</td>
		</tr>
		<tr id="tr8" class="qrcTr">
			<td id="td8a" class="qrcTdRight" width="18%">
				<asp:Label id="labelNote" runat="server" ToolTip="note" Text="note"></asp:Label>:
			</td>
			<td id="td8b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Note" runat="server" ToolTip="Enter personal note" 
					AutoCompleteType="Notes"
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
					onchange="highLightOnChange('TextBox_Note')"></asp:TextBox>
			</td>
		</tr>
		<tr id="tr9" class="qrcTr">
			<td id="td9a" class="qrcTdRight" width="18%">
				<asp:Label id="labelBirthday" runat="server" ToolTip="birthday" Text="birthday"></asp:Label>:
			</td>
			<td id="td9b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Birthday" runat="server" ToolTip="Enter birthday" 
					TextMode="Date"
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
					onchange="highLightOnChange('TextBox_Birthday')"></asp:TextBox>
			</td>
		</tr>
	</table>
	<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
	</div>
</asp:Content>
