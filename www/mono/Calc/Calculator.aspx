<%@ Page Language="C#"  MasterPageFile="~/Calc/Calc.master" Title="Web Calculator" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="Area23.At.Mono.Calc.Calculator" %>
<asp:Content ID="CalcHeadContent" ContentPlaceHolderID="CalcHead" runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta id="metacursor" runat="server" content="" />
	<meta id="metarad" runat="server" content="DEG" />
	<meta id="metaarc" runat="server" content="" />
	<meta name="keywords" content="calculators calc rpn" />
    <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
	<title>Web Calculator</title>
	<link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<link rel="stylesheet" href="../res/css/rpncalcweb.css" />	
	<script async src="../res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="CalcBodyContent" ContentPlaceHolderID="CalcBody" runat="server" ClientIDMode="Static">
	<form id="CalcForm" runat="server" action="Calculator.aspx">
	
		<table class="rpnTbl" border="0">
			<tr id="trA" class="rpnTr">
				<td id="tdAa" width="12%" align="left" class="rpnTd"></td>
				<td id="tdAb" width="60%" align="center" class="rpnTd" colspan="6"><asp:TextBox ID="textboxtop" ClientIDMode="Static" runat="server" Columns="30" AutoPostBack="true" OnTextChanged="bChange_Click" Font-Names="Courier New" /></td>
				<td id="tdAh" width="12%" align="right" class="rpnTd"></td>				
			</tr>
			<tr id="tr9" class="rpnTr">
				<td id="td9a" width="12%" align="center" class="rpnTd"></td>
				<td id="td9b" width="12%" align="center" class="azureTd"><asp:Button ID="Bsin" ClientIDMode="Static" runat="server" Text="sin" OnClick="bMath_Click" /></td>
				<td id="td9c" width="12%" align="center" class="azureTd"><asp:Button ID="Bcos" ClientIDMode="Static" runat="server" Text="cos" OnClick="bMath_Click" /></td>
				<td id="td9d" width="12%" align="center" class="azureTd"><asp:Button ID="Btan" ClientIDMode="Static" runat="server" Text="tan" OnClick="bMath_Click" /></td>
				<td id="td9e" width="12%" align="center" class="azureTd"><asp:Button ID="Bcot" ClientIDMode="Static" runat="server" Text="cot" OnClick="bMath_Click" /></td>
				<td id="td9f" width="12%" align="center" class="azureTd"></td>
				<td id="td9g" width="12%" align="center" class="salmonTd"></td>
				<td id="td9h" width="12%" align="center" class="rpnTd"></td>
			</tr>
			<tr id="tr8" class="rpnTr">
				<td id="td8a" width="12%" align="center" class="rpnTd"></td>
				<td id="td8b" width="12%" align="center" class="azureTd"><asp:Button ID="Bpow2" runat="server" ClientIDMode="Static" Text="x²" OnClick="bMath_Click" AccessKey="²" /></td>
				<td id="td8c" width="12%" align="center" class="azureTd"><asp:Button ID="Bpow3" runat="server" ClientIDMode="Static" Text="x³" AccessKey="³"  OnClick="bMath_Click" /></td>
				<td id="td8d" width="12%" align="center" class="azureTd"><asp:Button ID="B2pown" runat="server" ClientIDMode="Static" Text="2ⁿ" OnClick="bMath_Click" /></td>
				<td id="td8e" width="12%" align="center" class="azureTd"><asp:Button ID="B10pown" runat="server" ClientIDMode="Static" Text="10ⁿ" OnClick="bMath_Click" /></td>
				<td id="td8f" width="12%" align="center" class="mistyroseTd"><asp:Button ID="Bxpown" runat="server" Text="xⁿ" OnClick="bMath2Op_Click" /></td>
				<td id="td8g" width="12%" align="center" class="funckeyId"><asp:Button ID="Brad" ClientIDMode="Static" runat="server" Text="DEG" OnClick="bRad_Click" /></td>
				<td id="td8h" width="12%" align="center" class="rpnTd"></td>					
			</tr>
			<tr id="tr7" class="rpnTr">
				<td id="td7a" width="12%" class="rpnTd"></td>
				<td id="td7b" width="12%" align="center" class="azureTd"><asp:Button ID="Binverse" runat="server" ClientIDMode="Static" Text="1/x" OnClick="bMath_Click" /></td>
				<td id="td7c" width="12%" align="center" class="azureTd"><asp:Button ID="Bln" runat="server" ClientIDMode="Static" Text="ln" OnClick="bMath_Click" /></td>
				<td id="td7d" width="12%" align="center" class="azureTd"><asp:Button ID="Bld" runat="server" ClientIDMode="Static" Text="ld" OnClick="bMath_Click" /></td>d
				<td id="td7e" width="12%" align="center" class="azureTd"><asp:Button ID="Blog" runat="server" ClientIDMode="Static" Text="log" OnClick="bMath_Click" /></td>
				<td id="td7f" width="12%" align="center" class="mistyroseTd"><asp:Button ID="Blogxy" runat="server" ClientIDMode="Static" Text="log&#x2095;&#x1d482;" OnClick="bMath2Op_Click" /></td>
				<td id="td7g" width="12%" align="center" class="funckeyId"><asp:Button ID="Barc" ClientIDMode="Static" runat="server" Text="ARC" OnClick="bArc_Click" /></td>
				<td id="td7h" width="12%" align="center" class="rpnTd"></td>
			</tr>
			<tr id="tr6" class="rpnTr">
				<td class="rpnTd" width="12%" id="td6a" runat="server" align="center"></td>
				<td class="azureTd" width="12%" id="td6b" align="center"><asp:Button ID="Bsqrt" runat="server" ClientIDMode="Static" Text="√" OnClick="bMath_Click" /></td>
				<td class="azureTd" width="12%" id="td6c" align="center"><asp:Button ID="Bsqr3" runat="server" ClientIDMode="Static" Text="∛" OnClick="bMath_Click" /></td>
				<td class="azureTd" width="12%" id="td6d" align="center"><asp:Button ID="Bsqr4" runat="server" ClientIDMode="Static" Text="∜" OnClick="bMath_Click" /></td>
				<td class="azureTd" width="12%" id="td6e" align="center"></td>
				<td class="mistyroseTd" width="12%" id="td6f" align="center"><asp:Button ID="Bsqrti" runat="server" ClientIDMode="Static" Text="ⁱ√" OnClick="bMath_Click" /></td>
				<td class="salmonTd" width="12%" id="td6g" align="center"></td>
				<td class="rpnTd" width="12%" id="td6h"></td>
			</tr>
			<tr id="tr5" class="rpnTr">
				<td class="rpnTd" width="12%" id="td5a"></td>
				<td class="azureTd" width="12%" id="td5b" align="center"><asp:Button ID="Bpercent" runat="server" ClientIDMode="Static" Text="%" OnClick="bMath_Click" /></td>
				<td class="azureTd" width="12%" id="td5c" align="center"><asp:Button ID="Bpermille" runat="server" ClientIDMode="Static" Text="‰" OnClick="bMath_Click" /></td>
				<td class="azureTd" width="12%" id="td5d" align="center"><asp:Button ID="Babs" runat="server" ClientIDMode="Static" Text="|x|" AccessKey="|" OnClick="bMath_Click" /></td>
				<td class="azureTd" width="12%" id="td5e" align="center"><asp:Button ID="Bpermutation" runat="server" ClientIDMode="Static" Text="n!" OnClick="bMath_Click" AccessKey="!" /></td>
				<td class="mistyroseTd" width="12%" id="td5f" align="center"><asp:Button ID="Bmod" runat="server" ClientIDMode="Static" Text="mod" OnClick="bMath2Op_Click" /></td>
				<td class="funckeyId" width="12%" id="td5g" align="center"><asp:Button ID="Bmodus" ClientIDMode="Static" runat="server" Text="md10" OnClick="bModus_Click" /></td>
				<td class="rpnTd" width="12%" id="td5h"></td>
			</tr>
			<tr id="tr4" class="rpnTr">
				<td class="rpnTd" width="12%" id="td4a"></td>
				<td class="gainsboroTd" width="12%" id="td4b" align="center"><asp:Button ID="Bopen" runat="server" ClientIDMode="Static" Text="(" OnClick="bBracers_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td4c" align="center"><asp:Button ID="Beuler" runat="server" Text="ℇ" ClientIDMode="Static" OnClick="bPiE_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td4d" align="center"><asp:Button ID="Bpi" runat="server" ClientIDMode="Static" Text="π" OnClick="bPiE_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td4e" align="center"><asp:Button ID="Binfite" runat="server" ClientIDMode="Static" Text="∞" OnClick="bInfinite_Click" /></td>
				<td class="mistyroseTd" width="12%" id="td4f" align="center"><asp:Button ID="Bdivision" runat="server" Text="÷" OnClick="bMath2Op_Click" ClientIDMode="Static" /></td>
				<td class="salmonTd" width="12%" id="td4g" align="center"></td>
				<td class="rpnTd" width="12%" id="td4h"></td>
			</tr>
			<tr id="tr3" class="rpnTr">
				<td class="rpnTd" width="12%" id="td3a"></td>
				<td class="gainsboroTd" width="12%" id="td3b" align="center"><asp:Button ID="Bclose" runat="server" Text=")" OnClick="bBracers_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td3c" align="center"><asp:Button ID="B7" runat="server" Text="7" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td3d" align="center"><asp:Button ID="B8" runat="server" Text="8" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td3e" align="center"><asp:Button ID="B9" runat="server" Text="9" OnClick="bNumber_Click" /></td>
				<td class="mistyroseTd" width="12%" id="td3f" align="center"><asp:Button ID="Bmultiply" runat="server" Text="×" OnClick="bMath2Op_Click" /></td>
				<td class="salmonTd" width="12%" id="td3g" align="center"><asp:Button ID="Bdel" runat="server" Text="␡" OnClick="Bdel_Click" /></td>
				<td class="rpnTd" width="12%" id="td3h"></td>
			</tr>
			<tr id="tr2" class="rpnTr">
				<td class="rpnTd" width="12%" id="td2a"></td>
				<td class="gainsboroTd" width="12%" id="td2b" align="center"><asp:Button ID="Bbopen" runat="server" Text="[" OnClick="bBracers_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td2c" align="center"><asp:Button ID="B4" runat="server" Text="4" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td2d" align="center"><asp:Button ID="B5" runat="server" Text="5" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td2e" align="center"><asp:Button ID="B6" runat="server" Text="6" OnClick="bNumber_Click" /></td>
				<td class="mistyroseTd" width="12%" id="td2f" align="center"><asp:Button ID="Bminus" runat="server" Text="-" OnClick="bMath2Op_Click" /></td>
				<td class="salmonTd" width="12%" id="td2g" align="center"><asp:Button ID="BClear" runat="server" ClientIDMode="Static" Text="C" ToolTip="Clear" OnClick="BClear_Click" /></td>
				<td class="rpnTd" width="12%" id="td2h"></td>
			</tr>
			<tr id="tr1" class="rpnTr">
				<td class="rpnTd" width="12%" id="td1a"></td>
				<td class="gainsboroTd" width="12%" id="td1b" align="center"><asp:Button ID="Bbclose" runat="server" Text="]" OnClick="bBracers_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td1c" align="center"><asp:Button ID="B1" runat="server" Text="1" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td1d" align="center"><asp:Button ID="B2" runat="server" Text="2" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td1e" align="center"><asp:Button ID="B3" runat="server" Text="3" OnClick="bNumber_Click" /></td>
				<td class="mistyroseTd" width="12%" id="td1f" align="center"><asp:Button ID="Bplus" runat="server" Text="+" OnClick="bMath2Op_Click" /></td>
				<td class="salmonTd" width="12%" id="td1g" align="center"><asp:Button ID="Bequ" runat="server" Text="=" OnClick="BEval_Click" ClientIDMode="Static" /></td>
				<td class="rpnTd" width="12%" id="td1h"></td>
			</tr>
			<tr id="tr0" class="rpnTr">
				<td class="rpnTd" width="12%" id="td0a"></td>
				<td class="gainsboroTd" width="12%" id="td0b" align="center"></td>
				<td class="gainsboroTd" width="12%" id="td0c" align="center"><asp:Button ID="BDot" runat="server" Text="." OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td0d" align="center"><asp:Button ID="B0" runat="server" Text="0" OnClick="bNumber_Click" /></td>
				<td class="gainsboroTd" width="12%" id="td0e" align="center"><asp:Button ID="Bcomma" runat="server" Text="," OnClick="bNumber_Click" /></td>
				<td class="azureTd" width="12%" id="td0f" align="center"><asp:Button ID="Bplusminus" ClientIDMode="Static" runat="server" Text="±" OnClick="bMath_Click" /></td>
				<td class="salmonTd" width="12%" id="td0g" align="center"><asp:Button ID="BEnter" ClientIDMode="Static" runat="server" Text="&#9166;" OnClick="bEnter_Click" /></td>
				<td class="rpnTd" width="12%" id="td0h"></td>
			</tr>
		</table> 
	</form>
</asp:Content>