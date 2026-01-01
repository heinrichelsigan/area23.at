<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.MyIp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Geolocation FrameSet</title>
    <script type="text/javascript">                

        navigator.geolocation.getCurrentPosition(position => {
            const { latitude, longitude } = position.coords;
            const myElement = document.getElementById("topFrame");
            if (myElement != null) {
                try {
                    myElement.src = "R.aspx?geolat=" + latitude + "&geolong=" + longitude;
                } catch (e) { alert("Exception: " + e); }
                try {
                    myElement.attributes["src"] = "R.aspx?geolat=" + latitude + "&geolong=" + longitude;
                } catch (e) { alert("Exception: " + e); }
                try {
                    myElement.setAttribute("src", "R.aspx?geolat=" + latitude + "&geolong=" + longitude)
                } catch (e) { alert("Exception: " + e); }
            }
        });

        
    </script>
</head>
    <frameset rows="60%, 40%">
        <frame id="topFrame" runat="server" name="RFrame" src="R.aspx" />
        <frame id="ipapiFrame" runat="server" src="https://area23.at/net/R.aspx" />
    </frameset>    
</html>
