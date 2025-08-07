<%@ Page Language="C#" MasterPageFile="~/Qr/QRMaster.master" AutoEventWireup="true" CodeBehind="ContactQrGenerator.aspx.cs" Inherits="Area23.At.Mono.Qr.ContactQrGenerator" %>
<asp:Content ID="QrHeadContent" ContentPlaceHolderID="QrHead" runat="server" ClientIDMode="Static">
    <title>Contact Qr Generator (apache2 mod_mono)</title>
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
	<script async src="../res/js/area23.js"></script>
	<script>
		window.onload = function () {
			setColorPicker();
		}; 
    </script>
</asp:Content>
<asp:Content ID="QrBodyContent" ContentPlaceHolderID="QrBody" runat="server" ClientIDMode="Static">
	<form id="Form_QRCodeGen" runat="server">
		<table class="qrcTable" border="0" cellpadding="0" cellspacing="0">
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
						AutoCompleteType="HomePhone"  ClientIDMode="Static"
						MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"
						onchange="highLightOnChange('TextBox_Phone')"></asp:TextBox>
				</td>
				<td id="td1c" class="qrcTdRight" width="18%" height="30pt">mobile:</td>
				<td id="td1d" class="qrcTdLeft" width="32%" height="30pt">
					<asp:TextBox ID="TextBox_Mobile" runat="server" ToolTip="enter mobile phone number" 
						AutoCompleteType="Cellular"  ClientIDMode="Static"
						MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxRight"
						onchange="highLightOnChange('TextBox_Mobile')"></asp:TextBox>
				</td>
			</tr>
			<tr id="tr2" class="qrcTr">
				<td id="td2a" class="qrcTdRight" width="18%" height="30pt">e-mail:</td>
				<td id="td2b" class="qrcTdLeft" width="32%" height="30pt">
					<asp:TextBox ID="TextBox_Email" runat="server" ToolTip="enter email address"
						AutoCompleteType="Email"  ClientIDMode="Static"
						MaxLength="136" Width="128pt" Height="24pt" CssClass="QRTextBox"
						onchange="highLightOnChange('TextBox_Email')" ></asp:TextBox>
				</td>
				<td id="td2c" class="qrcTdRight" width="18%" height="30pt">www:</td>
				<td id="td2d" class="qrcTdLeft" width="32%" height="30pt">
					<asp:TextBox ID="TextBox_Web" runat="server" ToolTip="enter www web site"
						AutoCompleteType="Homepage"  ClientIDMode="Static"
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
						<asp:Button id="Button_QRCode" name="Button_QRCode" runat="server" ClientIDMode="Static" ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />
					</span>
				</td>
				<td id="td6d" class="qrcTdLeft" width="32%" height="192pt" rowspan="4">
					<span class="righthuge">
						<img id="ImgQR" runat="server" alt="QRCode" height="244" width="244" tooltip="QRCode" src="../res/img/qrsample.gif" />
					</span>
				</td>
			</tr>
			<tr id="tr7" class="qrcTr">
				<td id="td7a" class="qrcTdRight" width="18%">
					<asp:Label id="labelQrColor" runat="server" ToolTip="Qr color" Text="qr color:" />
				</td>
				<td id="td7b" class="qrcTdLeft" width="32%">
					<input type="color" name="color1" id="color1" onchange="newQrColor(color1.value);" />&nbsp;
					<input id="input_color" ClientIDMode="Static" alt="qr color" runat="server" name="selected_color" type="text" value="" size="7" />																						
				</td>
			</tr>
			<tr id="tr8" class="qrcTr">
				<td id="td8a" class="qrcTdRight" width="18%">
					<asp:Label id="labelBgColor" runat="server" ToolTip="background color" Text="back color:" />
				</td>
				<td id="td8b" class="qrcTdLeft" width="32%">
					<input type="color" name="color0" id="color0" onchange="newBackgroundColor(color0.value);" />&nbsp;
					<input id="input_backcolor" ClientIDMode="Static" alt="background color" runat="server" name="back_color" type="text" value="" size="7" />																
				</td>
			</tr>
			<tr id="tr9" class="qrcTr">
				<td id="td9a" class="qrcTdRight" width="18%">
					<asp:Label id="labelQrMode" runat="server" ToolTip="background color" Text="qr mode:" />
					<asp:DropDownList ClientIDMode="Static" ID="DropDownListQrMode" runat="server" CssClass="QrDropDownList" ToolTip="Select Qr mode" 
						Style="display: inline-block;">
						<asp:ListItem Enabled="true" Selected="False" Value="1" Text="1" />
						<asp:ListItem Enabled="true" Selected="True" Value="2" Text="2" />
						<asp:ListItem Enabled="true" Selected="False" Value="3" Text="3" />
						<asp:ListItem Enabled="true" Selected="False" Value="4" Text="4" />
						<asp:ListItem Enabled="true" Selected="False" Value="6" Text="6" />
						<asp:ListItem Enabled="true" Selected="False" Value="8" Text="8" />
					</asp:DropDownList>
				</td>
				<td id="t9b" class="qrcTdLeft" width="32%">
					<asp:Label id="labelQrLevel" runat="server" ToolTip="background color" Text="qr level:" />
					<asp:DropDownList ClientIDMode="Static" ID="DropDownListQrLevel" runat="server" CssClass="QrDropDownList" ToolTip="Select Qr mode" 
						Style="display: inline-block;">
						<asp:ListItem Enabled="true" Selected="False" Value="-1" Text="Default" />
						<asp:ListItem Enabled="true" Selected="False" Value="0" Text="L" />
						<asp:ListItem Enabled="true" Selected="True" Value="1" Text="M" />
						<asp:ListItem Enabled="true" Selected="False" Value="2" Text="Q" />
						<asp:ListItem Enabled="true" Selected="False" Value="3" Text="H" />
					</asp:DropDownList>		
				</td>
			</tr>
			<tr id="tr10" class="qrcTr">
				<td id="td10a" class="qrcTdRight" width="18%">
					<asp:Label id="labelNote" runat="server" ToolTip="note" Text="note:" Visible="false" Enabled="false" />
					<asp:Label id="labelBirthday" runat="server" ToolTip="birthday" Text="birthday:" Visible="false" Enabled="false" />
				</td>
				<td id="td10b" class="qrcTdLeft" width="32%">
					<asp:TextBox ID="TextBox_Birthday" runat="server" ToolTip="Enter birthday" 
						TextMode="Date" MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"						
						Visible="false" Enabled="false" />	
					<asp:TextBox ID="TextBox_Note" runat="server" ToolTip="Enter personal note" AutoPostBack="False" 						
						MaxLength="128" Width="128pt" Height="24pt" Text="personal note" CssClass="QRTextBoxLeft" AutoCompleteType="None" 
						Visible="false" Enabled="false" />
				</td>
			</tr>
		</table>
		<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
		</div>
	</form>
</asp:Content>
