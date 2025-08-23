<%@ Control Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="HashKeyRadioButtonList.ascx.cs" Inherits="Area23.At.Mono.Controls.HashKeyRadioButtonList" %>
<asp:RadioButtonList ID="RadioButtonList_Hash" runat="server" AutoPostBack="true" ToolTip="choose hashing key method" OnSelectedIndexChanged="RadioButtonList_Hash_ParameterChanged"> 
    <asp:ListItem Selected="False" Value="BCrypt">bcrypt</asp:ListItem>
    <asp:ListItem Selected="True" Value="Hex">hex hash</asp:ListItem>
    <asp:ListItem Selected="False" Value="MD5">md5</asp:ListItem>
    <asp:ListItem Selected="False" Value="OpenBSDCrypt">openbsd crypt</asp:ListItem>
    <asp:ListItem Selected="False" Value="SCrypt">scrypt</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha1">sha1 key</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha256">sha256</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha384">sha384</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha512">sha512</asp:ListItem>
</asp:RadioButtonList>                