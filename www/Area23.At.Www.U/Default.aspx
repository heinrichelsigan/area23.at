<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Www.U.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>area23.at/s/ - another url shortner</title>
	<link rel="stylesheet" href="res/css/area23.at.mono.css" />
	<meta name="keywords" content="Utf8 symbol emoji search" />
	<meta name="author" content="Heinrich Elsigan" />
	<meta name="description" content="https://github.com/heinrich.elsigan/" />
	<script async src="res/js/area23.js"></script>
	<script>
		window.onload = function () {
			setColorPicker();
        }; 
    </script>
</head>
<body>
    <form id="form1" runat="server">
		<div align="left" class="contentDiv">
			<span class="leftSpan">
				<span class="textSpan">symbol search: </span>
				<asp:Textbox id="TextBox_Search" runat="server"  ToolTip="search symbol here" AutoPostBack="True" OnTextChanged="TextBox_Search_TextChanged" Width="192pt" Height="24pt" CssClass="QRTextBoxSmall" />&nbsp;
			</span>
			<span class="centerSpan">
				<span class="textSpan">symbol list: </span>
				<asp:DropDownList ID="DropDown_Symbol" runat="server" ToolTip="symbols" AutoPostBack="True" OnSelectedIndexChanged="DropDown_Symbol_Changed" CssClass="DropDownList" 
			</span>
			<span class="rightSpan">			
				<asp:Button id="Button_Search" name="Button_Search" runat="server" ClientIDMode="Static" 
					ToolTip="Search symbol" Text="Search" OnClick="Button_Search_Click" />	
			</span>
		</div>
		<div class="contentDiv">
			<span class="leftSpan">
				<span class="textSpan"><input type="color" name="color1" id="color1" onchange="newQrColor(color1.value);" /></span>
				&nbsp;
				<input id="input_color" alt="qr color" runat="server" name="selected_color" type="text" value="" size="7" />						
			</span>
			<span class="centerSpan">
				<span class="textSpan"><input type="color" name="color0" id="color0" onchange="newBackgroundColor(color0.value);" /></span>
				<input id="input_backcolor" alt="background color" runat="server" name="input_backcolor" type="text" value="" size="7" />				
			</span>
			<span class="centerSpan">
				<span class="textSpan">qr mode: </span>
				<asp:DropDownList ID="DropDown_QrMode" runat="server" ToolTip="qr mode" AutoPostBack="True" OnSelectedIndexChanged="DropDown_QrMode_Changed" CssClass="DropDownList">
					<asp:ListItem>L</asp:ListItem>
					<asp:ListItem>M</asp:ListItem>				
					<asp:ListItem Selected="True">Q</asp:ListItem>
					<asp:ListItem>H</asp:ListItem>
				</asp:DropDownList>
			</span>
			<span class="rightSpan">
				<span class="textSpan"> pixel per unit: </span>
				<asp:DropDownList ID="DropDown_PixelPerUnit" runat="server" ToolTip="pixel / unit in qr image" AutoPostBack="True" OnSelectedIndexChanged="DropDown_PixelPerUnit_Changed" CssClass="DropDownList">
					<asp:ListItem>1</asp:ListItem>
					<asp:ListItem>2</asp:ListItem>
					<asp:ListItem>3</asp:ListItem>
					<asp:ListItem Selected="True">4</asp:ListItem>
					<asp:ListItem>6</asp:ListItem>
					<asp:ListItem>8</asp:ListItem>
				</asp:DropDownList>
			</span>
		</div>
		<div align="left" class="contentDiv">			
			<span class="leftSpan">
				Symbol: <asp:Literal id="Literal_Symbol" runat="server" />
			</span>
			<span class="cemtertSpan">
				Number: <asp:TextBox id="TextBox_Number" runat="server" MaxLength="8" Text="" TextMode="Number" />
			</span>
			<span class="cemtertRight">
				Number: <asp:TextBox id="TextBox_Name" runat="server" MaxLength="32" Text="" />
			</span>
		</div>
		<div align="left" class="contentDiv">			
			<span class="leftSpan">
				CodeSymbol  <asp:Literal id="Literal_CodeSymbol" runat="server" />
			</span>
			<span class="cemtertSpan">
				HexCodeHtml: <asp:Literal id="Literal_HexCodeHtml" runat="server" />
			</span>
			<span class="cemtertRight">
				CodeHtml: <asp:Literal id="Literal_CodeHtml" runat="server" />
			</span>
		</div>
		<div align="left" class="contentDiv">
			<span class="lefthuge" id="hugeLeftId" runat="server">
				<asp:Image ID="ImageQr" runat="server" ImageUrl="~/res/img/qrsample2.png" Visible="false" BorderStyle="None" BackColor="Transparent" />
			</span>
			<span class="righthuge">
				<a id="HrefShort" runat="server" class="Url_Short" href="#" target="_blank" name="UrlShort" visible="false"></a>				
			</span>
		</div>		
		<div id="ErrorDiv" runat="server" class="footerDiv" visible="false">
		</div>
	</form> 
</body>
</html>
