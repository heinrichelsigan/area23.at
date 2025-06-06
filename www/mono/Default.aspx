﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.Default" %>
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
├── <span style="color: blue">unix</span>
│   ├── <a href="Unix/Default.aspx" tatget="top">unix cmd tools</a>
│   ├── <a href="Unix/FortunAsp.aspx" tatget="top">fortune(6)</a>
│   ├── <a href="Unix/HexDump.aspx" tatget="top">hexdump od(1)</a>
│   └── <a href="Unix/Bc.aspx" tatget="top">basic calc bc(1)</a>
│
├── <span style="color: blue">qr</span>
│   ├── <a href="Qr/QRCodeGen.aspx" tatget="top">qr contact</a>
│   ├── <a href="Qr/Qrc.aspx" tatget="top">qr prefilled</a>
│   └── <a href="Qr/Qr.aspx" tatget="top">qrcode generic</a>
│
├── <a href="Json.aspx" tatget="top">json/xml ser</a>
│
├── <span style="color: blue">crypt</span>
│   ├── <a href="Crypt/AesImprove.aspx" tatget="top">aes pipeline</a>
│   ├── <a href="Crypt/CoolCrypt.aspx" tatget="top">cool crypt</a>
│   ├── <a href="Crypt/ImgPngCrypt.aspx" tatget="top">grfx img crypt</a>
│   ├── <a href="Crypt/UueMime.aspx" tatget="top">uu base64 code</a>
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

