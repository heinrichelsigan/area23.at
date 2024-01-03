<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Qrc.aspx.cs" Inherits="area23.at.mono.test.Qrc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Qrc (QRCode generator) apache2 mod_mono</title>
    <link rel="stylesheet" href="res/area23.at.mono.test.css" />
	<meta name="keywords" content="QR code generator" />
	<meta name="description" content="https://github.com/codebude/QRCoder/" />
</head>
<body>
    <form id="form1" runat="server">
        <div>    
			<table class="qrcTable" border="0" cellpadding="0" cellpadding="0">
				<tr id="tr0" class="qrcTr">
					<td id="td0a" class="qrcTdRight" width="18%" height="30pt">first name:</td>
					<td id="td0b" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_FirstName" runat="server" ToolTip="enter first name" MaxLength="84" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged" Text="Heinrich" Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
					</td>
					<td id="td0c" class="qrcTdRight" width="18%" height="30pt">last name:</td>
					<td id="td0d" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_LastName" runat="server" ToolTip="enter last name" MaxLength="84" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged" Text="Elsigan" Width="128pt" Height="24pt" CssClass="QRTextBoxRight"></asp:TextBox>
					</td>
				</tr>
				<tr id="tr1" class="qrcTr">
					<td id="td1a" class="qrcTdRight" width="18%" height="30pt">phone:</td>
					<td id="td1b" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_Phone" runat="server" ToolTip="enter phone number" MaxLength="84" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft"></asp:TextBox>
					</td>
					<td id="td1c" class="qrcTdRight" width="18%" height="30pt">mobile:</td>
					<td id="td1d" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_Mobile" runat="server" ToolTip="enter mobile phone number" MaxLength="84" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxRight">+436507527928</asp:TextBox>
					</td>
				</tr>
				<tr id="tr2" class="qrcTr">
					<td id="td2a" class="qrcTdRight" width="18%" height="30pt">e-mail:</td>
					<td id="td2b" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_Email" runat="server" ToolTip="enter email address" MaxLength="128" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBox">office.area23@gmail.com</asp:TextBox>
					</td>
					<td id="td2c" class="qrcTdRight" width="18%" height="30pt">www:</td>
					<td id="td2d" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_Web" runat="server" ToolTip="enter www web site" MaxLength="128" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBox">https://area23.at/</asp:TextBox>
					</td>
				</tr>				
				<tr id="tr3" class="qrcTr">
					<td id="td3a" class="qrcTdRight" width="18%" height="30pt">country:</td>
					<td id="td3b" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_Coutry" runat="server" ToolTip="Enter country here" MaxLength="84" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="ButtonTextBox">Austria</asp:TextBox>
					</td>
					<td id="td3c" class="qrcTdRight" width="18%" height="30pt">region:</td>
					<td id="td3d" class="qrcTdLeft" width="32%" height="30pt">
						<asp:DropDownList ID="DropDown_Country" runat="server" CssClass="DropDownList" ToolTip="Choose country" AutoPostBack="True" OnSelectedIndexChanged="QRCode_ParameterChanged">
							<asp:ListItem>Africa</asp:ListItem>
							<asp:ListItem>All America</asp:ListItem>
							<asp:ListItem>North America</asp:ListItem>
							<asp:ListItem>Canada</asp:ListItem>	
							<asp:ListItem>United States of America</asp:ListItem>
							<asp:ListItem>Middle America</asp:ListItem>
							<asp:ListItem>Mexico</asp:ListItem>
							<asp:ListItem>South America</asp:ListItem>
							<asp:ListItem>Argentinia</asp:ListItem>
							<asp:ListItem>Brazil</asp:ListItem>
							<asp:ListItem>Chile</asp:ListItem>	
							<asp:ListItem>Europe</asp:ListItem>	
							<asp:ListItem Selected="True">Austria</asp:ListItem>	    
							<asp:ListItem>France</asp:ListItem>
							<asp:ListItem>Germany</asp:ListItem>
							<asp:ListItem>Great Britain</asp:ListItem>
							<asp:ListItem>United Kingdom</asp:ListItem>
							<asp:ListItem>Middle East</asp:ListItem>
							<asp:ListItem>Iran</asp:ListItem>
							<asp:ListItem>Israel</asp:ListItem>	
							<asp:ListItem>Saudi Arabia</asp:ListItem>
							<asp:ListItem>United Arab Emirates</asp:ListItem>
							<asp:ListItem>Russian federation</asp:ListItem>
							<asp:ListItem>Russia</asp:ListItem>
							<asp:ListItem>Belarus</asp:ListItem>
							<asp:ListItem>Bangladesh</asp:ListItem>
							<asp:ListItem>India</asp:ListItem>
							<asp:ListItem>Pakistan</asp:ListItem>		
							<asp:ListItem>Australia</asp:ListItem>    
							<asp:ListItem>Eurasia</asp:ListItem>
							<asp:ListItem>Asia</asp:ListItem>	
							<asp:ListItem>China</asp:ListItem>	
							<asp:ListItem>Japan</asp:ListItem>
							<asp:ListItem>Korea</asp:ListItem>
							<asp:ListItem>Oceania</asp:ListItem>	
							<asp:ListItem>Antarctica</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr id="tr4" class="qrcTr">
					<td id="td4a" class="qrcTdRight" width="18%" height="30pt">city:</td>
					<td id="td4b" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_City" runat="server" ToolTip="Enter city name" MaxLength="84" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft">Vienna</asp:TextBox>
					</td>
					<td id="td4c" class="qrcTdRight" width="18%" height="30pt">zip code:</td>
					<td id="td4d" class="qrcTdLeft" width="325%" height="30pt">
						<asp:TextBox ID="TextBox_ZipCode" runat="server" ToolTip="Enter postal zip code" MaxLength="64" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="60pt" Height="24pt" CssClass="QRTextBoxRight">1040</asp:TextBox>
					</td>
				</tr>
				<tr id="tr5" class="qrcTr">
					<td id="td5a" class="qrcTdRight" width="18%" height="30pt">street:</td>
					<td id="td5b" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_Street" runat="server" ToolTip="Enter street name" MaxLength="128" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft">Theresianumgasse</asp:TextBox>
					</td>
					<td id="td5c" class="qrcTdRight" width="18%" height="30pt">house nr:</td>
					<td id="td5d" class="qrcTdLeft" width="32%" height="30pt">
						<asp:TextBox ID="TextBox_StreetNr" runat="server" ToolTip="Enter street number" MaxLength="32" AutoPostBack="True" OnTextChanged="QRCode_ParameterChanged"  Width="48pt" Height="24pt" CssClass="QRTextBoxRight">6</asp:TextBox>
					</td>
				</tr>
				<tr id="tr6" class="qrcTr">
					<td id="td6a" class="qrcTdRight" width="18%">
						<asp:Label id="labelOrg" runat="server" ToolTip="organisation" Text="org"></asp:Label>:
					</td>
					<td id="td6b" class="qrcTdLeft" width="32%">
						<asp:TextBox ID="TextBox_Org" runat="server" ToolTip="Enter organisation name" MaxLength="128" AutoPostBack="True" 
							OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft">area23.at</asp:TextBox>						
					</td>
					<td id="td6c" class="qrcTdRight" width="18%" height="192pt" rowspan="4">
						<span class="lefthuge">
							<asp:Button ID="Button_QRCode" runat="server" ToolTip="Click to generate QRCode" Text="generate QRCode" OnClick="Button_QRCode_Click" />
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
						<asp:Label id="labelOrgTitle" runat="server" ToolTip="organisation titel" Text="org title"></asp:Label>:						
					</td>
					<td id="td7b" class="qrcTdLeft" width="32%">
						<asp:TextBox ID="TextBox_OrgTitle" runat="server" ToolTip="Enter organisation title" MaxLength="128" AutoPostBack="True" 
							OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft">NGO</asp:TextBox>
					</td>
				</tr>
				<tr id="tr8" class="qrcTr">
					<td id="td8a" class="qrcTdRight" width="18%">
						<asp:Label id="labelNote" runat="server" ToolTip="note" Text="note"></asp:Label>:
					</td>
					<td id="td8b" class="qrcTdLeft" width="32%">
						<asp:TextBox ID="TextBox_Note" runat="server" ToolTip="Enter personal note" MaxLength="128" AutoPostBack="True" 
							OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft">personal note</asp:TextBox>
					</td>
				</tr>
				<tr id="tr9" class="qrcTr">
					<td id="td9a" class="qrcTdRight" width="18%">
						<asp:Label id="labelBirthday" runat="server" ToolTip="birthday" Text="birthday"></asp:Label>:
					</td>
					<td id="td9b" class="qrcTdLeft" width="32%">
						<asp:TextBox ID="TextBox_Birthday" runat="server" ToolTip="Enter birthday" MaxLength="128" AutoPostBack="True" 
							OnTextChanged="QRCode_ParameterChanged"  Width="128pt" Height="24pt" CssClass="QRTextBoxLeft">22.02.1973</asp:TextBox>
					</td>
				</tr>
			</table>			
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
