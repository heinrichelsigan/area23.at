<%@ Page Language="C#"  MasterPageFile="~/QRMaster.master" AutoEventWireup="true" CodeBehind="Qrc.aspx.cs" Inherits="area23.at.www.mono.Qrc" %>
<asp:Content ID="QrHeadContent" ContentPlaceHolderID="QrHead" runat="server">
	<title>qr contact prefilled (apache2 mod_mono)</title>
	<link rel="stylesheet" href="res/area23.at.www.mono.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
	<script async src="res/js/area23.js"></script>
	<script>
		window.onload = function () {
			setColorPicker();
        }; 
    </script>
</asp:Content>
<asp:Content ID="QrBodyContent" ContentPlaceHolderID="QrBody" runat="server">
	<table class="qrcTable" border="0" cellpadding="0" cellpadding="0">
		<tr id="tr0" class="qrcTr">
			<td id="td0a" class="qrcTdRight" width="18%" height="30pt">first name:</td>
			<td id="td0b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_FirstName" runat="server" ToolTip="enter first name" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="84" Width="128pt" Height="24pt" Text="Heinrich" CssClass="QRTextBoxLeft" AutoCompleteType="FirstName" />
			</td>
			<td id="td0c" class="qrcTdRight" width="18%" height="30pt">last name:</td>
			<td id="td0d" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_LastName" runat="server" ToolTip="enter last name" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="84" Width="128pt" Height="24pt" Text="Elsigan" CssClass="QRTextBoxRight" AutoCompleteType="LastName" />
			</td>
		</tr>
		<tr id="tr1" class="qrcTr">
			<td id="td1a" class="qrcTdRight" width="18%" height="30pt">phone:</td>
			<td id="td1b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_Phone" runat="server" ToolTip="enter phone number" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft" AutoCompleteType="HomePhone" />
			</td>
			<td id="td1c" class="qrcTdRight" width="18%" height="30pt">mobile:</td>
			<td id="td1d" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_Mobile" runat="server" ToolTip="enter mobile phone number" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxRight" 
					TextMode="Phone" AutoCompleteType="Cellular" Text="+436507527928" />
			</td>
		</tr>
		<tr id="tr2" class="qrcTr">
			<td id="td2a" class="qrcTdRight" width="18%" height="30pt">e-mail:</td>
			<td id="td2b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_Email" runat="server" ToolTip="enter email address" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBox" 
					TextMode="Email" AutoCompleteType="Email" Text="office.area23@gmail.com" />
			</td>
			<td id="td2c" class="qrcTdRight" width="18%" height="30pt">www:</td>
			<td id="td2d" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_Web" runat="server" ToolTip="enter www web site" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="128" Width="128pt" Height="24pt"  CssClass="QRTextBox" AutoCompleteType="Homepage" Text="https://area23.at/" />
			</td>
		</tr>				
		<tr id="tr3" class="qrcTr">
			<td id="td3a" class="qrcTdRight" width="18%" height="30pt">country:</td>
			<td id="td3b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_Coutry" runat="server" ToolTip="Enter country here" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="84" Width="128pt" Height="24pt"  CssClass="ButtonTextBox" AutoCompleteType="HomeState" Text="Austria" />
			</td>
			<td id="td3c" class="qrcTdRight" width="18%" height="30pt">region:</td>
			<td id="td3d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_Region" runat="server" ToolTip="Enter region here" 
					OnTextChanged="QRCode_ParameterChanged" AutoCompleteType="HomeCountryRegion" 
					MaxLength="84" Width="128pt" Height="24pt" CssClass="ButtonTextBox" Text="Europe" />
			</td>
		</tr>
		<tr id="tr4" class="qrcTr">
			<td id="td4a" class="qrcTdRight" width="18%" height="30pt">city:</td>
			<td id="td4b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_City" runat="server" ToolTip="Enter city name" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="84" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft" AutoCompleteType="HomeCity" Text="Vienna" />
			</td>
			<td id="td4c" class="qrcTdRight" width="18%" height="30pt">zip code:</td>
			<td id="td4d" class="qrcTdLeft" width="325%" height="30pt">
                <asp:TextBox ID="TextBox_ZipCode" runat="server" ToolTip="Enter postal zip code" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="64" Width="60pt" Height="24pt" CssClass="QRTextBoxRight" AutoCompleteType="HomeZipCode" Text="1040" />
			</td>
		</tr>
		<tr id="tr5" class="qrcTr">
			<td id="td5a" class="qrcTdRight" width="18%" height="30pt">street:</td>
			<td id="td5b" class="qrcTdLeft" width="32%" height="30pt">
                <asp:TextBox ID="TextBox_Street" runat="server" ToolTip="Enter street name" AutoPostBack="True" 
					OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="128" Width="128pt" Height="24pt" 
					CssClass="QRTextBoxLeft" AutoCompleteType="HomeStreetAddress" Text="Theresianumgasse" />
			</td>
			<td id="td5c" class="qrcTdRight" width="18%" height="30pt">house nr:</td>
			<td id="td5d" class="qrcTdLeft" width="32%" height="30pt">
				<asp:TextBox ID="TextBox_StreetNr" runat="server" ToolTip="Enter street number" AutoPostBack="True" 
					OnTextChanged="QRCode_ParameterChanged" 
					MaxLength="32"  Width="48pt" Height="24pt" CssClass="QRTextBoxRight" Text="6" />
			</td>
		</tr>
		<tr id="tr6" class="qrcTr">
			<td id="td6a" class="qrcTdRight" width="18%">
				<asp:Label id="labelOrg" runat="server" ToolTip="organisation" Text="org" />:
			</td>
			<td id="td6b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Org" runat="server" ToolTip="Enter organisation name" AutoPostBack="True" 
					OnTextChanged="QRCode_ParameterChanged" 
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft" Text="area23.at" />
			</td>
			<td id="td6c" class="qrcTdRight" width="18%" height="192pt" rowspan="4">
				<span class="lefthuge">
					<input type="color" name="color1" id="color1" onchange="newBackgroundColor(color1.value);" />&nbsp;
					<input id="input_color" ClientIDMode="Static" runat="server" name="selected_color" type="text" value="" size="7"  />	
					<br />
					<asp:Button id="Button_QRCode" name="Button_QRCode" runat="server" ClientIDMode="Static" 
						ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />
				</span>
			</td>
			<td id="td6d" class="qrcTdLeft" width="32%" height="192pt"  rowspan="4">
				<span class="righthuge">
					<img id="ImgQR" runat="server" alt="QRCode" height="192" width="192" tooltip="QRCode" src="res/qrsample1.gif" />
				</span>
			</td>
		</tr>
		<tr id="tr7" class="qrcTr">
			<td id="td7a" class="qrcTdRight" width="18%">
				<asp:Label id="labelOrgTitle" runat="server" ToolTip="organisation titel" Text="org title" />:						
			</td>
			<td id="td7b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_OrgTitle" runat="server" ToolTip="Enter organisation title" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged" 
					MaxLength="128" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft" Text="FreeLancer" />
			</td>
		</tr>
		<tr id="tr8" class="qrcTr">
			<td id="td8a" class="qrcTdRight" width="18%">
				<asp:Label id="labelNote" runat="server" ToolTip="note" Text="note" />:
			</td>
			<td id="td8b" class="qrcTdLeft" width="32%">
                <asp:TextBox ID="TextBox_Note" runat="server" ToolTip="Enter personal note" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"
                    MaxLength="128" Width="128pt" Height="24pt" Text="personal note" CssClass="QRTextBoxLeft" AutoCompleteType="Notes"  />
			</td>
		</tr>
		<tr id="tr9" class="qrcTr">
			<td id="td9a" class="qrcTdRight" width="18%">
				<asp:Label id="labelBirthday" runat="server" ToolTip="birthday" Text="birthday" />:
			</td>
			<td id="td9b" class="qrcTdLeft" width="32%">
				<asp:TextBox ID="TextBox_Birthday" runat="server" ToolTip="Enter birthday" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged" 
					MaxLength="128" Width="128pt" Height="24pt" TextMode="Date" CssClass="QRTextBoxLeft" Text="22/02/1973" />
			</td>
		</tr>
	</table>			
	<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
	</div>
</asp:Content>
