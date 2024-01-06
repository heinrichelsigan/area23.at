<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="FortunAsp.aspx.cs" Inherits="area23.at.mono.test.FortunAsp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <noscript>
        <meta http-equiv="refresh" content="16; url=https://darkstar.work/mono/fortune/" />
    </noscript>
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="res/area23.at.mono.test.css" />
    <title>Fortune Mono WebApi</title>
    <!-- Google tag (gtag.js) -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-01S65129V7"></script>
    <script>
            window.dataLayer = window.dataLayer || [];
            function gtag(){dataLayer.push(arguments);}
            gtag('js', new Date());

            gtag('config', 'G-01S65129V7');
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodydiv" runat="server">
    <div class="fortuneDiv" align="left">
        <asp:Literal ID="LiteralFortune" runat="server"></asp:Literal>
    </div>
    <hr />
    <pre id="PreFortune" runat="server" style="text-align: left; border-style:none; background-color='#bfbfbf'; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif"></pre>
</asp:Content>
