<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web" %>
<%@ Import namespace="System.Web.UI" %>
<%@ Import namespace="System.Web.UI.WebControls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>Octal Dump (od) apache2 mod_mono C#</title>
    <script async src="res/js/area23.js"></script>
</head>

<script runat="server" language="C#">
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("HexDump.aspx");
        Response.End();
    }
    
</script>

<body>
    <form method="post" action="/cgi/od.cgi" id="form1" runat="server">
        <div class="odDiv">
            <span class="leftSpan">
                <span class="textSpan">hex width: </span>
                <select name="DropDown_HexWidth" onchange="FormSubmit()" id="DropDown_HexWidth" title="Hexadecimal width" class="DropDownList">
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="4">4</option>
                    <option selected="selected" value="8">8</option>
                </select>
            </span>
            <span class="centerSpan">
                <span class="textSpan"> word width: </span>
                <select name="DropDown_WordWidth" onchange="FormSubmit()" id="DropDown_WordWidth" title="Word with for bytes" class="DropDownList">
                    <option value="4">4</option>
                    <option value="8">8</option>
                    <option value="16">16</option>
                    <option selected="selected" value="32">32</option>
                    <option value="64">64</option>
                </select>
            </span>
            <span class="centerSpan">
                <span class="textSpan"> read bytes: </span>
                <select name="DropDown_ReadBytes" onchange="FormSubmit()" id="DropDown_ReadBytes" title="Bytes to read on octal dump" class="DropDownList">
                    <option value="64">64</option>
                    <option value="128">128</option>
                    <option value="256">256</option>
                    <option value="512">512</option>
                    <option selected="selected" value="1024">1024</option>
                    <option value="4096">4096</option>
                    <option value="16384">16384</option>
                    <option value="65536">65536</option>
                    <option value="262144">262144</option>

                </select>
            </span>
            <span class="rightSpan">
                <span class="textSpan"> seek bytes: </span>
                <input type="text" value="0" maxlength="8" onkeypress="if (WebForm_TextBoxKeyHandler(event) == false) return false;" onchange="setTimeout(64, FormSubmit())" name="TextBox_Seek" id="TextBox_Seek" title="seek bytes" class="ButtonTextBox" style="height:24pt;width:48pt;" />
            </span>
        </div>
        <div class="odDiv">
            <span style="white-space: nowrap; width:90%; text-align: left;">
                <table id="RBL_Radix" title="Radix format">
                    <tr>
                        <td><input id="RBL_Radix_0" type="radio" name="RBL_Radix" value="d" onclick="FormSubmit()" /><label for="RBL_Radix_0">Decimal</label></td>
                        <td><input id="RBL_Radix_1" type="radio" name="RBL_Radix" value="o" onclick="FormSubmit()" /><label for="RBL_Radix_1">Octal</label></td>
                        <td><input id="RBL_Radix_2" type="radio" name="RBL_Radix" value="x" onclick="FormSubmit()" /><label for="RBL_Radix_2">Hex</label></td>
                        <td><input id="RBL_Radix_3" type="radio" name="RBL_Radix" value="n" checked="checked" onclick="FormSubmit()" /><label for="RBL_Radix_3">None</label></td>
                    </tr>
                </table>
            </span>
        </div>
        <div class="odDiv">
            <span style="width:28%; text-align: left;">
                <input type="submit" name="Button_OctalDump" value="octal dump" id="Button_OctalDump" title="Click to perform octal dump" />
            </span>
            <span class="smallSpan" style="width:44%; text-align: right;">
                <span class="textSpan">od cmd: </span>
                <input type="text" value="od -A n -t x8z -w32 -v -j 0 -N 1024 /dev/urandom" maxlength="60" readonly="ReadOnly" name="TextBox_OdCmd" id="TextBox_OdCmd" title="od shell command" class="ButtonTextBox" style="height:24pt;width:32%;" />
            </span>
            <span style="width:28%; text-align: right;">
                <select name="DropDown_Device" onchange="FormSubmit()" id="DropDown_Device" title="device name" class="DropDownList">
                    <option value="random">random</option>
                    <option selected="selected" value="urandom">urandom</option>
                    <option value="zero">zero</option>

                </select>
            </span>
        </div>
        <hr />
        <pre id="preOut">7cb71b7207b131c5 9ffe10310c2f4d4a b040636972bcc946 ccda0100c7ff1dfd  &gt;.1..r..|JM/.1...F..ric@.........&lt;
 f6460b08da50583d d1bf2392578e229c 9d0b77c06688ee65 84328d1a2b0ca86e  &gt;=XP...F..&quot;.W.#..e..f.w..n..+..2.&lt;
 d50b488c2a2d4645 ebe38ebfe670af21 ccaff6eab1859932 37843a6f7fc47472  &gt;EF-*.H..!.p.....2.......rt..o:.7&lt;
 5b3c94532237631a 8f8528b0a539b143 31711bdb3a98837f 395f43686d94a7fb  &gt;.c7&quot;S.&lt;[C.9..(.....:..q1...mhC_9&lt;
 780cf3cb2d37244b 2746dc322f430ca9 9fdadd42100ec09c aefde8033f73270e  &gt;K$7-...x..C/2.F&#39;....B....&#39;s?....&lt;
 fad5898a1e5d8f16 112d25892e98810c 37cf174b49787bde 4d15b7831c8012a0  &gt;..]..........%-..{xIK..7.......M&lt;
</pre>
        <hr />
        <div align="left" class="footerDiv">
            <span style="width:12%; text-align: left;" align="left" valign="middle"><a href="/test/cgi/fortune.html">fortune</a></span>
            <span style="width:12%; text-align: center;" align="center" valign="middle"><a href="/mono/json/">json deserializer</a></span>
            <span style="width:12%; text-align: center;" align="center" valign="middle"><a href="/test/cgi/od.html">octal dump</a></span>
            <span style="width:64%; text-align: right;" align="right" valign="middle">
                <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>
            </span>
        </div>
    </form>
</body>
</html>