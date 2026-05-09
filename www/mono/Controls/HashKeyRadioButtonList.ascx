<%@ Control Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="HashKeyRadioButtonList.ascx.cs" Inherits="Area23.At.Mono.Controls.HashKeyRadioButtonList" %>
<asp:RadioButtonList ID="RadioButtonList_Hash" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" ToolTip="choose hashing key method" OnSelectedIndexChanged="RadioButtonList_Hash_ParameterChanged">
</asp:RadioButtonList>
<!--
    BCrypt, Blake2xs, CShake, Dstu7564, Hex, MD5, OpenBSDCrypt, Oct,
    RipeMD256, SCrypt, Sha1, Sha256, TupleHash, Whirlpool
    
-->