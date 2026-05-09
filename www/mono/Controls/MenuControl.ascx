<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs" Inherits="Area23.At.Mono.Controls.MenuControl" %>
<div id="rowqr" runat="server" class="headercard" align="left">  
    <asp:Repeater ID="RepeaterLink" runat="server" ClientIDMode="Static" ItemType="Area23.At.Mono.Controls.HeaderLink">
        <ItemTemplate>
            <span id='<%# ((Area23.At.Mono.Controls.HeaderLink)Container.DataItem).DivId %>' 
                class='<%# ((Area23.At.Mono.Controls.HeaderLink)Container.DataItem).DivCss %>' align="center" valign="middle">
                <a id='<%# ((Area23.At.Mono.Controls.HeaderLink)Container.DataItem).UId %>' 
                    href='<%# ((Area23.At.Mono.Controls.HeaderLink)Container.DataItem).UHref %>'>
                    <%# ((Area23.At.Mono.Controls.HeaderLink)Container.DataItem).UTitle %></a>
            </span>
        </ItemTemplate>
    </asp:Repeater>
</div>