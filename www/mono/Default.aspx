<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <title>Area23.At.Mono (test examples)</title> 
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>json ser (apache2 mod_mono)</title>
    <script async src="res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
<pre style="background-color: seashell; margin: 12px 12px 4px 4px; padding: 2px 2px 2px 2px;">
<a href="Default.aspx" tatget="top">/</a>
├── <span style="color: blue"><a href="Unix/Default.aspx" tatget="top">unix cmd tools</a></span>
│   ├── <a href="Unix/FortunAsp.aspx" tatget="top">fortunes</a> <a href="https://area23.at/shared/fortune.txt" target="_blank">fortune(6)</a>
│   ├── <a href="Unix/HexDump.aspx" tatget="top">hexdump</a> <a href="https://www.man7.org/linux/man-pages/man1/od.1.html" target="_blank">od(1)</a>
│   ├── <a href="Unix/Bc.aspx" tatget="top">basic calc</a> <a href="https://linux.die.net/man/1/bc" target="_blank">bc(1)</a>
│   ├── <a href="Unix/MergePdf.aspx" tatget="top">merge pdf</a> <a href="https://wiki.ubuntuusers.de/poppler-utils/" target="_blank">pdfunite (1)</a>
│   └── <a href="Unix/PdfMerge.aspx" tatget="top">pdf merge</a> <a href="https://wiki.ubuntuusers.de/poppler-utils/" target="_blank">pdfunite (1)</a>
│
├── <span style="color: blue">qr</span>
│   ├── <a href="Qr/ContactQrGenerator.aspx" tatget="top">qr contact generator</a>
│   ├── <a href="Qr/ContactPrefilled.aspx" tatget="top">contact prefilled</a>
│   ├── <a href="Qr/GenericQr.aspx" tatget="top">generic qr (generator)</a>
│   └── <a href="Qr/QrRedirect.aspx" tatget="top">qr redirect</a>
│
├── <a href="Json.aspx" tatget="top">json/xml ser</a>
│
├── <span style="color: blue">crypt</span>
│   ├── <a href="Crypt/CoolCrypt.aspx" tatget="top">cool crypt</a>
│   ├── <a href="Crypt/AesImprove.aspx" tatget="top">aes pipeline</a>
│   ├── <a href="Crypt/ImgPngCrypt.aspx" tatget="top">grfx img crypt</a>
│   ├── <a href="Crypt/BCrypt.aspx" tatget="top">bcrypt</a>
│   └── <a href="Crypt/ZenMatrixVisualize.aspx" tatget="top">visualize zen matrix</a>
│
├── <span style="color: blue">calc</span>
│   ├── <a href="Calc/CCalc.aspx" tatget="top">ccalc</a>
│   ├── <a href="Calc/RpnCalc.aspx" tatget="top">rpn calc</a>
│   ├── <a href="Calc/MatrixVCalc.aspx" tatget="top">matrix x vector</a>
│   └── <a href="Calc/MatrixMCalc.aspx" tatget="top">atrix x matrix</a>
│
└── <span style="color: blue">gamez</span>
    ├── <a href="Gamez/froga.aspx" tatget="top">frogA</a>
    ├── <a href="Gamez/frogb.aspx" tatget="top">frogB</a>
    └── <a href="Gamez/TicTacToe.aspx" tatget="top">tic tac toee</a>
</pre>
</asp:Content>

