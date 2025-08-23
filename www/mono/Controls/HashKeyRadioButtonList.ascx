<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HashKeyRadioButtonList.ascx.cs" Inherits="Area23.At.Mono.Controls.HashKeyRadioButtonList" %>
<asp:RadioButtonList ID="RadioButtonList_Hash" runat="server" AutoPostBack="true" ToolTip="choose hashing key method" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList_Hash_ParameterChanged"> 
    <asp:ListItem Selected="False" Value="BCrypt">bcrypt key</asp:ListItem>
    <asp:ListItem Selected="True" Value="Hex">hex hash key</asp:ListItem>
    <asp:ListItem Selected="False" Value="MD5">md5 key</asp:ListItem>
    <asp:ListItem Selected="False" Value="OpenBSDCrypt">openbsd crypt</asp:ListItem>
    <asp:ListItem Selected="False" Value="SCrypt">scrypt key</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha1">sha1 key</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha256">sha256 key</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha384">sha384 key</asp:ListItem>
    <asp:ListItem Selected="False" Value="Sha512">sha256 key</asp:ListItem>
</asp:RadioButtonList>                    