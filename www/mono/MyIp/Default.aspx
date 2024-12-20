<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="System.Diagnostics"%>
<%@ Import namespace="System.Web.UI"%>
<%@ Import namespace="System.Web.UI.WebControls"%>
<%@ Import namespace="System.Runtime.Serialization"%>


<script runat="server" language="C#">

    const string dlm = "-";
    void Page_Load(object sender, EventArgs e)
    {
		string userHostName;
		string userHostAddr = Request.UserHostAddress;

		literalUserHost.Text = userHostAddr;			

		if (!this.IsPostBack)
        {
            userHostName = Request.UserHostName;
			header.InnerHtml = "<title>" + userHostName + "</title>";
		}

    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server"><title>My-IPAddr</title></head>
<body><asp:Literal ID="literalUserHost" runat="server"></asp:Literal></body>
</html>