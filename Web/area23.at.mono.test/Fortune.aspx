<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="Newtonsoft.Json" %>
<%@ Import namespace="Newtonsoft.Json.Linq" %>
<%@ Import namespace="Newtonsoft.Json.Bson" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web" %>
<%@ Import namespace="System.Diagnostics" %>
<%@ Import namespace="area23.at.mono.test.Util" %>
<%@ Import namespace="System.Web.UI" %>
<%@ Import namespace="System.Web.UI.WebControls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <noscript>
        <meta http-equiv="refresh" content="16; url=https://darkstar.work/mono/fortune/" />
    </noscript>
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <title>Fortune Mono WebApi</title>
        <!-- Google tag (gtag.js) -->
        <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
        <script>
                window.dataLayer = window.dataLayer || [];
                function gtag(){dataLayer.push(arguments);}
                gtag('js', new Date());

                gtag('config', 'G-01S65129V7');
        </script>
        <script type="text/javascript">
            function reloadFortune() {

                var url = "https://darkstar.work/mono/fortune/";
                var delay = 16000;

                if (document.getElementById("ButtonHidden") == null) {
                    setTimeout(function () { window.location.href = "https://darkstar.work/mono/fortune/"; }, delay); // will call the function after 8 secs.
                    return;
                }

                setTimeout(
                    function () {
                        // alert("document.getElementById(\"ButtonHidden\").innerText = " + document.getElementById("ButtonHidden").innerText + " !");
                        document.getElementById("ButtonHidden").click();
                    },
                    delay); // will call the function after 16 secs.
            }
        </script>
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
            <span class="footerCenter" align="center" valign="middle"><a href="/mono/test/Qrc.aspx">qrcode gen</a></span>	
	        <span class="footerRightCenter" align="center" valign="middle"><a href="/mono/test/json.aspx">json (de-)serializer</a></span>
	        <span class="footerRight" align="right" valign="middle"><a href="mailto:he@area23.at">Heinrich Elsigan</a>, GNU General Public License 3.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a></span>
        </div>
        <div align="right" style="height: 3pt; width: 5pt; font-size: xx-small; border-style: none; text-decoration-color: #f7f7f7; color: #f9f9f9; background-color: #fbfbfb;">
            <asp:Button ID="ButtonHidden" runat="server" Text="_" BorderStyle="None" BorderColor="#fbfbfb" BackColor="#fbfbfb" ForeColor="#fbfbfb" BorderWidth="0"
                 Width="5" Height="3" OnClick="ButtonHidden_Click"
                style="height: 3pt; width: 5pt; font-size: xx-small; text-decoration-color: #f7f7f7; color: #f9f9f9; background-color: #fbfbfb;" />
        </div>        
    </form>
</body>
</html>
