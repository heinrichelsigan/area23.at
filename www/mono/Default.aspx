<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>apache2 mod_mono examples</title>
</head>

<script runat="server" language="C#">
          
    void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("QRCodeGen.aspx");
    }        

</script>

<body onload="window.location.href='Qrc.aspx'">
    <form id="form1" runat="server">
        <div>
            <hr />
            <div align="left" style="width: 100%; table-layout: fixed; inset-block-start: initial; background-color: #dfdfdf; font-size: small; border-style: none; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;">
                <span style="width:12%; background-color: #bfafcf; vertical-align:middle; text-align: left;" align="left" valign="middle"><a href="Fortune.aspx">fortune</a></span>
                <span style="width:12%; background-color: #afcfbf; vertical-align:middle; text-align: center;" align="center" valign="middle"><a href="json.aspx">json deserializer</a></span>
                <span style="width:12%; background-color: #cfbfaf; vertical-align:middle; text-align: center;" align="center" valign="middle"><a href="OctalDump.aspx">octal dump</a></span>
                <span style="width:64%; background-color: #cfcfcf; vertical-align:middle; text-align: right;" align="right" valign="middle">
                    <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>
                </span>
            </div>
        </div>
    </form>
</body>
</html>

