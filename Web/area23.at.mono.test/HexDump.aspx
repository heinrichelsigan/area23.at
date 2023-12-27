﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HexDump.aspx.cs" Inherits="area23.at.mono.test.HexDump" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="https://area23.at/css/od.css" />
    <title>Hex Dump (od) apache2 mod_mono C#</title>
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
        <script>
            window.dataLayer = window.dataLayer || [];
            function gtag() { dataLayer.push(arguments); }
            gtag('js', new Date());

            gtag('config', 'G-01S65129V7');
        </script>
</head>
<body class="odbody">
    <form id="form1" runat="server">
        <div class="odDiv">
            <span class="leftSpan">
                <span class="textSpan">hex width: </span>
                <asp:DropDownList ID="DropDown_HexWidth" runat="server" CssClass="DropDownList" ToolTip="Hexadecimal width" AutoPostBack="True" OnSelectedIndexChanged="HexDump_ParameterChanged">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem Selected="True">8</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="centerSpan">
                <span class="textSpan"> word width: </span>
                    <asp:DropDownList ID="DropDown_WordWidth" runat="server" CssClass="DropDownList" ToolTip="Word with for bytes" AutoPostBack="True" OnSelectedIndexChanged="HexDump_ParameterChanged">
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem Selected="True">32</asp:ListItem>
                    <asp:ListItem>64</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="centerSpan">
                <span class="textSpan"> read bytes: </span>
                    <asp:DropDownList ID="DropDown_ReadBytes" runat="server" CssClass="DropDownList" ToolTip="Bytes to read on octal dump" AutoPostBack="True" OnSelectedIndexChanged="HexDump_ParameterChanged">
                    <asp:ListItem>64</asp:ListItem>
                    <asp:ListItem>128</asp:ListItem>
                    <asp:ListItem>256</asp:ListItem>
                    <asp:ListItem>512</asp:ListItem>
                    <asp:ListItem Selected="True">1024</asp:ListItem>
                    <asp:ListItem>4096</asp:ListItem>
                    <asp:ListItem>16384</asp:ListItem>
                    <asp:ListItem>65536</asp:ListItem>
                    <asp:ListItem>262144</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="rightSpan">
                <span class="textSpan"> seek bytes: </span>
                <asp:TextBox ID="TextBox_Seek" runat="server" ToolTip="seek bytes" MaxLength="8" Width="48pt" Height="24pt" AutoPostBack="True" OnTextChanged="HexDump_ParameterChanged"                    
                    CssClass="ButtonTextBox">0</asp:TextBox>
            </span>
        </div>
        <div class="odDiv">
            <span style="nowrap; width:90%; text-align: left;">
                <asp:RadioButtonList ID="RBL_Radix" runat="server" AutoPostBack="true" ToolTip="Radix format" RepeatDirection="Horizontal" OnSelectedIndexChanged="HexDump_ParameterChanged">
                    <asp:ListItem Selected="False" Value="d">Decimal</asp:ListItem>
                    <asp:ListItem Selected="False" Value="o">Octal</asp:ListItem>
                    <asp:ListItem Selected="False" Value="x">Hex</asp:ListItem>
                    <asp:ListItem Selected="True" Value="n">None</asp:ListItem>
                </asp:RadioButtonList>
            </span>
        </div>
        <div class="odDiv">
            <span style="width:28%; text-align: left;">
                <asp:Button ID="Button_HexDump" runat="server" ToolTip="Click to perform hex dump" Text="hex dump" OnClick="Button_HexDump_Click" />
            </span>
            <span class="smallSpan" style="width:44%; text-align: right;">
                <span class="textSpan">od cmd: </span>
                <asp:TextBox ID="TextBox_OdCmd" runat="server"  CssClass="ButtonTextBox" ToolTip="od shell command"  ReadOnly Width="32%" MaxLength="60" Height="24pt" />
            </span>        
            <span style="width:28%; text-align: right;">
                <asp:DropDownList ID="DropDown_Device" runat="server" CssClass="DropDownList" ToolTip="device name" AutoPostBack="True" OnSelectedIndexChanged="HexDump_ParameterChanged">
                    <asp:ListItem Enabled="true">random</asp:ListItem>
                    <asp:ListItem Enabled="true" Selected="True">urandom</asp:ListItem>
                    <asp:ListItem Enabled="true">zero</asp:ListItem>
                </asp:DropDownList>
            </span>
        </div>
        <hr />
        <pre id="preOut" runat="server">
        </pre>
        <hr />
        <div align="left" class="footerDiv">            
            <span class="footerLeft" align="left" valign="middle"><a href="/cgi/fortune.cgi">fortune</a></span>
			<span class="footerLeftCenter" align="center" valign="middle"><a href="/froga/">froga</a></span>
			<span class="footerCenter" align="center" valign="middle"><a href="/mono/test/HexDump.aspx">hex dump</a></span>			
            <span class="footerCenter" align="center" valign="middle"><a href="/mono/test/QRCode.aspx">hex dump</a></span>	
			<span class="footerRightCenter" align="center" valign="middle"><a href="/mono/SchnapsNet/">schnapsen 66</a></span>
			<span class="footerRight" align="right" valign="middle"><a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a></span>
		</div>
    </form> 
</body>
</html>
