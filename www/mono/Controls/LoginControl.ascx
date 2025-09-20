<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="Area23.At.Mono.Controls.LoginControl" %>
<div>
    <div id="DivLoginBox" runat="server" style="display: block; border-style: outset; border-width: 2px; border-color: azure; max-width: 640px; width: 444px; font-size: larger; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;  padding: 4px 4px 2px 2px; margin: 4px 4px 2px 2px;">
        <span style="display: block; border-style: outset; border-width: 1px; border-color: blanchedalmond;">
            &#x1F464; Username: <asp:TextBox ID="TextBoxUserName" runat="server" AutoPostBack="false" ToolTip="Enter username here" 
                Width="240px" MaxLength="32" Height="25px"></asp:TextBox>
        </span>
        <span style="display: block; border-style: outset; border-width: 1px; border-color: blanchedalmond;">
            &#128273;&nbsp; Password: <asp:TextBox ID="TextBoxPassword" runat="server" AutoPostBack="false" TextMode="Password" ToolTip="Enter secret password here" 
                Width="240px" MaxLength="32" Height="25px"></asp:TextBox>
        </span>
        <span style="display: block; border-style: outset; border-width: 1px; border-color: blanchedalmond;">
            <asp:CheckBox ID="CheckBoxKeepSignIn" runat="server" ToolTip="check to keep sign in" Text="Keep sign in" Checked="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ButtonLogin" runat="server" Text="Login" ToolTip="Login" OnClick="ButtonLogin_Click"  Font-Size="Large" Font-Names="Geneva, Verdana, sans-serif" BorderStyle="Outset"  />
        </span>                
    </div>    
    <pre id="preOut" runat="server" style="font-size:small; width: 640px; height: 180px; max-height: 192px; border-block-color: palevioletred;">&nbsp;</pre>
</div>

