<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="area23.at.mono.rpncalc.Calculator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>RPNCalc.Web</title>    
    <link rel="stylesheet" href="css/rpncalcweb.css" />
	<script type="text/javascript" src="js/rpn.js"></script>
	<meta id="metacursor" runat="server" content="9" />
</head>
<body onload="rpnInit();">
    <form id="form1" runat="server">
        		<div class="rpnDiv" align="center">
			<div class="rpnDivHeader">
				<span id="headerLeft" style="min-height: 36pt; min-width: 60pt; vertical-align: middle; font-size: larger; text-align: right " align="right" valign="middle">
					<span id="spanPreviousValue" runat="server" alt="previous value" style="color:greenyellow">0</span> 
					<span id="spanLastValue" runat="server" alt="last value" style="color:aqua;">4</span>					
					<span id="frogsDied" alt="frogs died" style="color:gainsboro">0</span>
				</span>
				<span style="min-height: 36pt; min-width: 240pt; vertical-align:middle; font-size: medium; text-align: center" align="center" valign="middle">
					<img class="rpnImg" src="img/header.png" id="headerImg" border="0" onclick="restart()" />
				</span>
				<span id="headerRight" style="min-height: 36pt; min-width: 60pt; vertical-align: middle; font-size: larger; text-align: left" align="left" valign="middle">								
					&nbsp;<span id="spanCalcModeLabel" alt="calculator mode">mode</span>
					<span id="spanCalcMode" runat="server" alt="decimal">10</span>
					<span id="rightNotes"></span>
				</span>
			</div>
			<table class="rpnTbl" border="0" cellpadding="0" cellpadding="0">								
				<!--
				<tr id="tr11" class="rpnTr">
					<td class="rpnTd" width="10%" id="td11a"></td>
					<td class="gainsboroTd" width="10%" id="td11b" runat="server" align="center"><asp:Button ID="Bsinh" runat="server" Text="sinh" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td11c" runat="server" align="center"><asp:Button ID="Bcosh" runat="server" Text="cosh" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td11d" runat="server" align="center"><asp:Button ID="Btanh" runat="server" Text="tanh" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td11e" runat="server" align="center">sech</td>
					<td class="gainsboroTd" width="10%" id="td11f" runat="server" align="center">csch</td>
					<td class="gainsboroTd" width="10%" id="td11g" runat="server" align="center"><asp:Button ID="Bcoth" runat="server" Text="coth" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td11h"></td>
					<td class="rpnTd" width="20%" id="td11i" colspan="2"><asp:TextBox ID="textbox11" runat="server"></asp:TextBox></td>
				</tr>
				-->				
				<!-- 
				<tr id="tr10" class="rpnTr">
					<td class="rpnTd" width="10%" id="td10a"></td>
					<td class="gainsboroTd" width="10%" id="td10b" runat="server" align="center"><asp:Button ID="Basin" runat="server" Text="asin" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td10c" runat="server" align="center"><asp:Button ID="Bacos" runat="server" Text="acos" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td10d" runat="server" align="center"><asp:Button ID="Batan" runat="server" Text="atan" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td10e" runat="server" align="center"><asp:Button ID="Basec" runat="server" Text="asec" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td10f" runat="server" align="center"><asp:Button ID="Bacsc" runat="server" Text="acsc" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td10g" runat="server" align="center"><asp:Button ID="Bacot" runat="server" Text="acot" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td10h"></td>
					<td class="rpnTd" width="20%" id="td10i" colspan="2"><asp:TextBox ID="textbox10" runat="server"></asp:TextBox></td>>
				</tr>
				-->				
				<tr id="tr9" class="rpnTr">
					<td class="rpnTd" width="10%" id="td9a"></td>
					<td class="gainsboroTd" width="10%" id="td9b" align="center"><asp:Button ID="Bsin" runat="server" Text="sin" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td9c" align="center"><asp:Button ID="Bcos" runat="server" Text="cos" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td9d" align="center"><asp:Button ID="Btan" runat="server" Text="tan" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td9e" align="center"><asp:Button ID="Bsec" runat="server" Text="sec" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td9f" align="center"><asp:Button ID="Bcsc" runat="server" Text="csc" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td9g" align="center"><asp:Button ID="Bcot" runat="server" Text="cot" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9h"></td>
					<td class="rpnTd" width="20%" id="td9i" colspan="2"><asp:TextBox ID="textbox9" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr8" class="rpnTr">
					<td class="rpnTd" width="10%" id="td8a"></td>
					<td class="gainsboroTd" width="10%" id="td8b" align="center"><asp:Button ID="Babs" runat="server" Text="|x|" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td8c" align="center"><asp:Button ID="Bxpow2" runat="server" Text="x²" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td8d" align="center"><asp:Button ID="B2pown" runat="server" Text="2ⁿ" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td8e" align="center"><asp:Button ID="B10pown" runat="server" Text="10ⁿ" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td8f" align="center"><asp:Button ID="Bpermutation" runat="server" Text="n!" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td8g" align="center"><asp:Button ID="Bxpown" runat="server" Text="xⁿ" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8h"></td>
					<td class="rpnTd" width="20%" id="td8i" colspan="2"><asp:TextBox ID="textbox8" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr7" class="rpnTr">
					<td class="rpnTd" width="10%" id="td7a"></td>
					<td class="gainsboroTd" width="10%" id="td7b" align="center"><asp:Button ID="Blog" runat="server" Text="log" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td7c" align="center"><asp:Button ID="Bln" runat="server" Text="ln" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td7d" align="center"><asp:Button ID="Bld" runat="server" Text="ld" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td7e" align="center"><asp:Button ID="Binverse" runat="server" Text="1/x" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td7f" align="center"><asp:Button ID="Bmod" runat="server" Text="mod" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td7g" align="center"><asp:Button ID="Bexp" runat="server" Text="exp" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7h"></td>
					<td class="rpnTd" width="20%" id="td7i" colspan="2"><asp:TextBox ID="textbox7" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr6" class="rpnTr">
					<td class="rpnTd" width="10%" id="td6a" runat="server" align="center"></td>
					<td class="gainsboroTd" width="10%" id="td6b" align="center"><asp:Button ID="Bsqrt" runat="server" Text="√" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td6c" align="center"><asp:Button ID="Bsqr3" runat="server" Text="∛" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td6d" align="center"><asp:Button ID="Bsqr4" runat="server" Text="∜" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td6e" align="center"><asp:Button ID="Bpercent" runat="server" Text="%" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td6f" align="center"><asp:Button ID="Bpermille" runat="server" Text="‰" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td6g" align="center"></td>
					<td class="rpnTd" width="10%" id="td6h"></td>
					<td class="rpnTd" width="20%" id="td6i" colspan="2"><asp:TextBox ID="textbox6" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr5" class="rpnTr">
					<td class="rpnTd" width="10%" id="td5a"></td>
					<td class="gainsboroTd" width="10%" id="td5b" align="center"><asp:Button ID="Bopen" runat="server" Text="(" OnClick="bBracers_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td5c" align="center"><asp:Button ID="Beuler" runat="server" Text="ℇ" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td5d" align="center"><asp:Button ID="Bpi" runat="server" Text="π" OnClick="bMath_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td5e" align="center"><asp:Button ID="Binfite" runat="server" Text="∞" OnClick="bMath_Click" /></td>					
					<td class="deeppinkTd" width="10%" id="td5f" align="center"><asp:Button ID="Bdivision" runat="server" Text="÷" OnClick="bMath_Click" /></td>
					<td class="palevioletredTd" width="10%" id="td5g" align="center"><asp:Button ID="BClear" runat="server" Text="C" ToolTip="Clear" OnClick="BClear_Click" /></td>
					<td class="rpnTd" width="10%" id="td5h"></td>
					<td class="rpnTd" width="20%" id="td5i" colspan="2"><asp:TextBox ID="textbox5" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr4" class="rpnTr">
					<td class="rpnTd" width="10%" id="td4a"></td>
					<td class="gainsboroTd" width="10%" id="td4b" align="center"><asp:Button ID="Bclose" runat="server" Text=")" OnClick="bBracers_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td4c" align="center"><asp:Button ID="B7" runat="server" Text="7" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td4d" align="center"><asp:Button ID="B8" runat="server" Text="8" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td4e" align="center"><asp:Button ID="B9" runat="server" Text="9" OnClick="bNumber_Click" /></td>
					<td class="deeppinkTd" width="10%" id="td4f" align="center"><asp:Button ID="Bmultiply" runat="server" Text="×" OnClick="bMath_Click" /></td>
					<td class="palevioletredTd" width="10%" id="td4g" align="center"><asp:Button ID="Bdel" runat="server" Text="␡" OnClick="Bdel_Click" /></td>
					<td class="rpnTd" width="10%" id="td4h"></td>
					<td class="rpnTd" width="20%" id="td4i" colspan="2"><asp:TextBox ID="textbox4" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr3" class="rpnTr">
					<td class="rpnTd" width="10%" id="td3a"></td>
					<td class="gainsboroTd" width="10%" id="td3b" align="center"><asp:Button ID="Bbopen" runat="server" Text="[" OnClick="bBracers_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td3c" align="center"><asp:Button ID="B4" runat="server" Text="4" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td3d" align="center"><asp:Button ID="B5" runat="server" Text="5" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td3e" align="center"><asp:Button ID="B6" runat="server" Text="6" OnClick="bNumber_Click" /></td>
					<td class="deeppinkTd" width="10%" id="td3f" align="center"><asp:Button ID="Bminus" runat="server" Text="-" OnClick="bMath_Click" /></td>
					<td class="palevioletredTd" width="10%" id="td3g" align="center"></td>
					<td class="rpnTd" width="10%" id="td3h"></td>
					<td class="rpnTd" width="20%" id="td3i" colspan="2"><asp:TextBox ID="textbox3" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr2" class="rpnTr">
					<td class="rpnTd" width="10%" id="td2a"></td>
					<td class="gainsboroTd" width="10%" id="td2b" align="center"><asp:Button ID="Bbclose" runat="server" Text="]" OnClick="bBracers_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td2c" align="center"><asp:Button ID="B1" runat="server" Text="1" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td2d" align="center"><asp:Button ID="B2" runat="server" Text="2" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td2e" align="center"><asp:Button ID="B3" runat="server" Text="3" OnClick="bNumber_Click" /></td>
					<td class="deeppinkTd" width="10%" id="td2f" align="center"><asp:Button ID="Bplus" runat="server" Text="+" OnClick="bMath_Click" /></td>
					<td class="palevioletredTd" width="10%" id="td2g" align="center"><asp:Button ID="BEnter" runat="server" Text="&#9166;" OnClick="bEnter_Click" /></td>
					<td class="rpnTd" width="10%" id="td2h"></td>
					<td class="rpnTd" width="20%" id="td2i" colspan="2"><asp:TextBox ID="textbox2" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr1" class="rpnTr">
					<td class="rpnTd" width="10%" id="td1a"></td>
					<td class="gainsboroTd" width="10%" id="td1b" align="center"></td>
					<td class="gainsboroTd" width="10%" id="td1c" align="center"><asp:Button ID="BDot" runat="server" Text="." OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td1d" align="center"><asp:Button ID="B0" runat="server" Text="0" OnClick="bNumber_Click" /></td>
					<td class="gainsboroTd" width="10%" id="td1e" align="center"><asp:Button ID="Bcomma" runat="server" Text="," OnClick="bNumber_Click" /></td>
					<td class="deeppinkTd" width="10%" id="td1f" align="center"><asp:Button ID="Bplusminus" runat="server" Text="±" OnClick="bMath_Click" /></td>
					<td class="palevioletredTd" width="10%" id="td1g" align="center"><asp:Button ID="Bequals" runat="server" Text="=" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td1h"></td>
					<td class="rpnTd" width="20%" id="td1i" colspan="2"><asp:TextBox ID="textbox1" runat="server"></asp:TextBox></td>
				</tr>
				<tr id="tr0" class="rpnTr">
					<td class="rpnTd" width="70%" colspan="7" id="td0a"></td>
					<td class="rpnTd" width="10%" id="td0h"></td>
					<td class="rpnTd" width="20%" id="td0i" colspan="2"><asp:TextBox ID="textbox0" runat="server"></asp:TextBox></td>
				</tr>
			</table>
			<div class="footerDiv">
			    <div align="left" style="text-align: left; width: 100%; height: 8%; visibility: inherit; background-color: #bfbfbf; font-size: small; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif">
					<a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="https://github.com/heinrichelsigan" target="_blank">github.com/heinrichelsigan</a>/<a href="https://github.com/heinrichelsigan/rpncalc" target="_blank">rpncalc</a>]            
				</div>
			</div>    
		</div>
    </form>
</body>
</html>
