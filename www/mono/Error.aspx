<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Area23.At.Mono.Error" MasterPageFile="~/Area23.Master" ValidateRequest="false" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>Server Error (apache2 mod_mono)</title>
    <script async src="res/js/area23.js"></script>
    <link rel="made" href="mailto:mailadmin@area23.at" />
    <style type="text/css"><!--/*--><![CDATA[/*><!--*/ 
        body { color: #000000; background-color: #FFFFFF; }
        a:link { color: #0000CC; }
        p, address {margin-left: 3em;}
        span {font-size: smaller;}
    /*]]>*/--></style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="Area23ErrorForm" runat="server">
        <p>

    The server has an error.
  

    If you want to try again, please go back.

  
</p>
<p>
If you think this is a fatal server error, please contact
the <a href="mailto:mailadmin@area23.at">webmaster</a>.

</p>

<h2>Error 500</h2>
<address>
  <a href="/">heinrichelsigan.area23.at</a><br>
  <span>Apache/2.4.58 (Ubuntu)</span>
</address>
   
    <div id="DivException" runat="server" visible="false">
        <pre id="PreException" runat="server" visible="false" style="background-color: #dfcfef; color: #1111dd; padding: 2px 2px 2px 2px; margin: 2px 2px 2px 2px;"></pre>
    </div>

    </form>
</asp:Content>

