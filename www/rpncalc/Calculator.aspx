<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="area23.at.mono.rpncalc.Calculator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>RPNCalc.Web</title>    
    <link rel="stylesheet" href="css/rpncalc.css" />
	<script type="text/javascript" src="js/froga.js"></script>
</head>
<body>
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
				
				<tr id="tr11" class="rpnTr">
					<td class="rpnTd" width="100%" id="td11a" colspan="10">
						<asp:TextBox ID="textBoxbResult" runat="server" Width="100%" BorderStyle="Outset"></asp:TextBox>
					</td>
				</tr>	
				
				<!--
				<tr id="tr10" class="rpnTr">
					<td class="rpnTd" width="10%" id="td10a"></td>
					<td class="rpnTd" width="10%" id="td10b" runat="server" align="center"><asp:Button ID="Bsinh" runat="server" Text="sinh" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td10c" runat="server" align="center"><asp:Button ID="Bcosh" runat="server" Text="cosh" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td10d" runat="server" align="center"><asp:Button ID="Btanh" runat="server" Text="tanh" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td10e" runat="server" align="center">sech</td>
					<td class="rpnTd" width="10%" id="td10f" runat="server" align="center">csch</td>
					<td class="rpnTd" width="10%" id="td10g" runat="server" align="center"><asp:Button ID="Bcoth" runat="server" Text="coth" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td10h"></td>
					<td class="rpnTd" width="10%" id="td10i" colspan="2"><asp:TextBox ID="textBox10" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				-->				
				<!-- 
				<tr id="tr9" class="rpnTr">
					<td class="rpnTd" width="10%" id="td9a"></td>
					<td class="rpnTd" width="10%" id="td9b" runat="server" align="center"><asp:Button ID="Basin" runat="server" Text="asin" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9c" runat="server" align="center"><asp:Button ID="Bacos" runat="server" Text="acos" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9d" runat="server" align="center"><asp:Button ID="Batan" runat="server" Text="atan" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9e" runat="server" align="center"><asp:Button ID="Basec" runat="server" Text="asec" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9f" runat="server" align="center"><asp:Button ID="Bacsc" runat="server" Text="acsc" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9g" runat="server" align="center"><asp:Button ID="Bacot" runat="server" Text="acot" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td9h"></td>
					<td class="rpnTd" width="10%" id="td9i" colspan="2"><asp:TextBox ID="textBox9" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>>
				</tr>
				-->				
				<tr id="tr8" class="rpnTr">
					<td class="rpnTd" width="10%" id="td8a"></td>
					<td class="rpnTd" width="10%" id="td8b" align="center" style="background-color: gainsboro"><asp:Button ID="Bsin" runat="server" Text="sin" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8c" align="center" style="background-color: gainsboro"><asp:Button ID="Bcos" runat="server" Text="cos" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8d" align="center" style="background-color: gainsboro"><asp:Button ID="Btan" runat="server" Text="tan" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8e" align="center" style="background-color: gainsboro"><asp:Button ID="Bsec" runat="server" Text="sec" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8f" align="center" style="background-color: gainsboro"><asp:Button ID="Bcsc" runat="server" Text="csc" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8g" align="center" style="background-color: gainsboro"><asp:Button ID="Bcot" runat="server" Text="cot" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td8h"></td>
					<td class="rpnTd" width="10%" id="td8i" colspan="2"><asp:TextBox ID="textBox8" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr7" class="rpnTr">
					<td class="rpnTd" width="10%" id="td7a"></td>
					<td class="rpnTd" width="10%" id="td7b" align="center" style="background-color: gainsboro"><asp:Button ID="Babs" runat="server" Text="|x|" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7c" align="center" style="background-color: gainsboro"><asp:Button ID="Bxpow2" runat="server" Text="x²" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7d" align="center" style="background-color: gainsboro"><asp:Button ID="B2pown" runat="server" Text="2ⁿ" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7e" align="center" style="background-color: gainsboro"><asp:Button ID="B10pown" runat="server" Text="10ⁿ" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7f" align="center" style="background-color: gainsboro"><asp:Button ID="Bpermutation" runat="server" Text="n!" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7g" align="center" style="background-color: gainsboro"><asp:Button ID="Bxpown" runat="server" Text="xⁿ" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td7h"></td>
					<td class="rpnTd" width="10%" id="td7i" colspan="2"><asp:TextBox ID="textBox7" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr6" class="rpnTr">
					<td class="rpnTd" width="10%" id="td6a"></td>
					<td class="rpnTd" width="10%" id="td6b" align="center" style="background-color: gainsboro"><asp:Button ID="Blog" runat="server" Text="log" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td6c" align="center" style="background-color: gainsboro"><asp:Button ID="Bln" runat="server" Text="ln" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td6d" align="center" style="background-color: gainsboro"><asp:Button ID="Bld" runat="server" Text="ld" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td6e" align="center" style="background-color: gainsboro"><asp:Button ID="Binverse" runat="server" Text="1/x" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td6f" align="center" style="background-color: gainsboro"><asp:Button ID="Bmod" runat="server" Text="mod" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td6g" align="center" style="background-color: gainsboro"><asp:Button ID="Bexp" runat="server" Text="exp" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td6h"></td>
					<td class="rpnTd" width="10%" id="td6i" colspan="2"><asp:TextBox ID="textBox6" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr5" class="rpnTr">
					<td class="rpnTd" width="10%" id="td5a" runat="server" align="center"></td>
					<td class="rpnTd" width="10%" id="td5b" align="center" style="background-color: gainsboro"><asp:Button ID="Bsqrt" runat="server" Text="√" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td5c" align="center" style="background-color: gainsboro"><asp:Button ID="Bsqr3" runat="server" Text="∛" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td5d" align="center" style="background-color: gainsboro"><asp:Button ID="Bsqr4" runat="server" Text="∜" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td5e" align="center" style="background-color: gainsboro"><asp:Button ID="Bpercent" runat="server" Text="%" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td5f" align="center" style="background-color: gainsboro"><asp:Button ID="Bpermille" runat="server" Text="‰" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td5g" align="center" style="background-color: gainsboro"></td>
					<td class="rpnTd" width="10%" id="td5h"></td>
					<td class="rpnTd" width="10%" id="td5i" colspan="2"><asp:TextBox ID="textBox5" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr4" class="rpnTr">
					<td class="rpnTd" width="10%" id="td4a"></td>
					<td class="rpnTd" width="10%" id="td4b" align="center" style="background-color: gainsboro"><asp:Button ID="Bopen" runat="server" Text="(" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td4c" align="center" style="background-color: gainsboro"><asp:Button ID="Beuler" runat="server" Text="ℇ" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td4d" align="center" style="background-color: gainsboro"><asp:Button ID="Bpi" runat="server" Text="π" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td4e" align="center" style="background-color: gainsboro"><asp:Button ID="Binfite" runat="server" Text="∞" OnClick="bMath_Click" /></td>					
					<td class="rpnTd" width="10%" id="td4f" align="center" style="background-color: deeppink"><asp:Button ID="Bdivision" runat="server" Text="÷" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td4g" align="center" style="background-color: palevioletred"><asp:Button ID="BClear" runat="server" Text="C" ToolTip="Clear" OnClick="BClear_Click" /></td>
					<td class="rpnTd" width="10%" id="td4h"></td>
					<td class="rpnTd" width="10%" id="td4i" colspan="2"><asp:TextBox ID="textBox4" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr3" class="rpnTr">
					<td class="rpnTd" width="10%" id="td3a"></td>
					<td class="rpnTd" width="10%" id="td3b" align="center" style="background-color: gainsboro"><asp:Button ID="Bclose" runat="server" Text=")" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td3c" align="center" style="background-color: gainsboro"><asp:Button ID="B7" runat="server" Text="7" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td3d" align="center" style="background-color: gainsboro"><asp:Button ID="B8" runat="server" Text="8" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td3e" align="center" style="background-color: gainsboro"><asp:Button ID="B9" runat="server" Text="9" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td3f" align="center" style="background-color: deeppink"><asp:Button ID="Bmultiply" runat="server" Text="×" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td3g" align="center" style="background-color: palevioletred"></td>
					<td class="rpnTd" width="10%" id="td3h"></td>
					<td class="rpnTd" width="10%" id="td3i" colspan="2"><asp:TextBox ID="textBox3" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr2" class="rpnTr">
					<td class="rpnTd" width="10%" id="td2a"></td>
					<td class="rpnTd" width="10%" id="td2b" align="center" style="background-color: gainsboro"><asp:Button ID="Bbopen" runat="server" Text="[" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td2c" align="center" style="background-color: gainsboro"><asp:Button ID="B4" runat="server" Text="4" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td2d" align="center" style="background-color: gainsboro"><asp:Button ID="B5" runat="server" Text="5" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td2e" align="center" style="background-color: gainsboro"><asp:Button ID="B6" runat="server" Text="6" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td2f" align="center" style="background-color: deeppink"><asp:Button ID="Bminus" runat="server" Text="-" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td2g" align="center" style="background-color: palevioletred"></td>
					<td class="rpnTd" width="10%" id="td2h"></td>
					<td class="rpnTd" width="10%" id="td2i" colspan="2"><asp:TextBox ID="textBox2" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr1" class="rpnTr">
					<td class="rpnTd" width="10%" id="td1a"></td>
					<td class="rpnTd" width="10%" id="td1b" align="center" style="background-color: gainsboro"><asp:Button ID="Bbclose" runat="server" Text="]" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td1c" align="center" style="background-color: gainsboro"><asp:Button ID="B1" runat="server" Text="1" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td1d" align="center" style="background-color: gainsboro"><asp:Button ID="B2" runat="server" Text="2" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td1e" align="center" style="background-color: gainsboro"><asp:Button ID="B3" runat="server" Text="3" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td1f" align="center" style="background-color: deeppink"><asp:Button ID="Bplus" runat="server" Text="+" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td1g" align="center" style="background-color: palevioletred"></td>
					<td class="rpnTd" width="10%" id="td1h"></td>
					<td class="rpnTd" width="10%" id="td1i" colspan="2"><asp:TextBox ID="textBox1" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
				</tr>
				<tr id="tr0" class="rpnTr">
					<td class="rpnTd" width="10%" id="td0a"></td>
					<td class="rpnTd" width="10%" id="td0b" align="center" style="background-color: gainsboro"></td>
					<td class="rpnTd" width="10%" id="td0c" align="center" style="background-color: gainsboro"><asp:Button ID="Bplusminus" runat="server" Text="±" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td0d" align="center" style="background-color: gainsboro"><asp:Button ID="B0" runat="server" Text="0" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td0e" align="center" style="background-color: gainsboro"><asp:Button ID="Bcomma" runat="server" Text="," OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td0f" align="center" style="background-color: deeppink"><asp:Button ID="Bequals" runat="server" Text="=" OnClick="bMath_Click" /></td>
					<td class="rpnTd" width="10%" id="td0g" align="center" style="background-color: palevioletred"><asp:Button ID="Bdel" runat="server" Text="␡" OnClick="Bdel_Click" /></td>
					<td class="rpnTd" width="10%" id="td0h"></td>
					<td class="rpnTd" width="10%" id="td0i" colspan="2"><asp:TextBox ID="textBox0" runat="server" Width="20%" BorderStyle="Outset"></asp:TextBox></td>
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
