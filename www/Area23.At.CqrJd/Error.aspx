<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Area23.At.CqrJd.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
    <title>Server Error</title>
    <link rev="made" href="mailto:mailadmin@area23.at">
    <style type="text/css"><!--/*--><![CDATA[/*><!--*/ 
        body { color: #000000; background-color: #FFFFFF; }
        a:link { color: #0000CC; }
        p, address {margin-left: 3em;}
        span {font-size: smaller;}
    /*]]>*/--></style>
</head>
<body>
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
        <pre id="PreException" runat="server" visible="false" style="background-color: #dfcfef; color: #1111dd; padding: 2 2 2 2; margin: 2 2 2 2;"></pre>
    </div>

</body>
</html>
