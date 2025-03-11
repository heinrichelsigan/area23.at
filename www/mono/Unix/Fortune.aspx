<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="Area23.At.Framework.Library" %>
<%@ Import namespace="Area23.At.Framework.Library.Static" %>
<%@ Import namespace="Newtonsoft.Json" %>
<%@ Import namespace="Newtonsoft.Json.Linq" %>
<%@ Import namespace="Newtonsoft.Json.Bson" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web" %>
<%@ Import namespace="System.Diagnostics" %>
<%@ Import namespace="Area23.At.Mono.Util" %>
<%@ Import namespace="System.Web.UI" %>
<%@ Import namespace="System.Web.UI.WebControls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <noscript>
        <meta http-equiv="refresh" content="20; url=/net/Unix/Fortune.aspx" />
    </noscript>
    <link rel="stylesheet" href="/css/fortune.css" />
    <title>Fortune Mono WebApi</title>
    <script async src="../res/js/area23.js"></script>
</head>

<script runat="server" language="C#">

    void Page_Load(object sender, EventArgs e)
    {
        SetFortune();
        Response.Redirect("FortunAsp.aspx");
    }

    void ButtonHidden_Click(object sender, EventArgs e)
    {
        SetFortune();
    }

    void SetFortune()
    {
        if (Constants.FortuneBool) LiteralFortune.Text = ExecFortune(false);
        else PreFortune.InnerText =  ExecFortune(true);
    }

    string ExecFortune(bool longFortune = true)
    {
        return (longFortune) ?
            ProcessCmd.Execute("/usr/games/fortune", " -a -l ") :
            ProcessCmd.Execute("/usr/games/fortune", "-o -s  ");
    }

</script>
<body onload="reloadFortune(); return false;">
    <form id="form1" runat="server">
        <div class="fortuneDiv" align="left">
            <asp:Literal ID="LiteralFortune" runat="server"></asp:Literal>
        </div>
        <hr />
        <pre id="PreFortune" runat="server" style="text-align: left; border-style:none; background-color='#bfbfbf'; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif"></pre>
        <hr />
        <div align="left" class="footerDiv">            
            <span class="footerLeft" align="left" valign="middle"><a href="/cgi/fortune.cgi">fortune</a></span>
	        <span class="footerLeftCenter" align="center" valign="middle"><a href="/froga/">froga</a></span>
	        <span class="footerCenter" align="center" valign="middle"><a href="/mono/test/HexDump.aspx">hex dump</a></span>			
            <span class="footerCenter" align="center" valign="middle"><a href="/mono/test/Qr/Qrc.aspx">qrcode gen</a></span>	
	        <span class="footerRightCenter" align="center" valign="middle"><a href="/mono/test/json.aspx">json (de-)serializer</a></span>
	        <span class="footerRight" align="right" valign="middle">
                <a href="mailto:he@area23.at">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="http://blog.area23.at">blog.</a>]<a href="https://area23.at">area23.at</a>
	        </span>
        </div>
        <div align="right" style="height: 3pt; width: 5pt; font-size: xx-small; border-style: none; text-decoration-color: #f7f7f7; color: #f9f9f9; background-color: #fbfbfb;">
            <asp:Button ID="ButtonHidden" runat="server" Text="_" BorderStyle="None" BorderColor="#fbfbfb" BackColor="#fbfbfb" ForeColor="#fbfbfb" BorderWidth="0"
                 Width="5" Height="3" OnClick="ButtonHidden_Click"
                style="height: 3pt; width: 5pt; font-size: xx-small; text-decoration-color: #f7f7f7; color: #f9f9f9; background-color: #fbfbfb;" />
        </div>        
    </form>
</body>
</html>
