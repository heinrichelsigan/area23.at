﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true" CodeBehind="FortunAsp.aspx.cs" Inherits="Area23.At.Mono.FortunAsp" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="refresh" content="16; url=https://area23.at/mono/test/FortunAsp.aspx" />    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>fortune (apache2 mod_mono)</title>    
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="Area23MasterForm" runat="server">
        <div class="fortuneDiv" align="left">
            <asp:Literal ID="LiteralFortune" runat="server"></asp:Literal>
        </div>
        <hr />
        <pre id="PreFortune" runat="server" style="text-align: left; border-style: none; background-color: #bfbfbf; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif"></pre>
    </form>
</asp:Content>
