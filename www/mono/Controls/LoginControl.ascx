<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="Area23.At.Mono.Controls.LoginControl" %>
<div style="clear: both; display: block; border-width 2px; border-style: outset; border-color: azure; word-wrap: break-word; unicode-bidi: isolate; position: relative;"> 
    <asp:TreeView ID="TreeViewNavigation" runat="server">
        <RootNodeStyle BorderWidth="1" BorderColor="DarkBlue" BorderStyle="Groove"  BackColor="Transparent" />
        <HoverNodeStyle BackColor="AliceBlue" />
        <LeafNodeStyle BorderStyle="Dotted" BorderWidth="1"  />
        <NodeStyle BorderColor="Turquoise" BorderWidth="1" BorderStyle="Dashed"  BackColor="Transparent" />
        <SelectedNodeStyle BorderColor="OrangeRed" BorderWidth="2" BorderStyle="Double" BackColor="WhiteSmoke" />
        <Nodes>
            <asp:TreeNode Text="/" Expanded="true" ToolTip="/">
        
                <asp:TreeNode Text="unix" ToolTip="unix cmd tools">
                    <asp:TreeNode Text="unix cmd tools" ToolTip="node 1 leaf 1" ShowCheckBox="true" Checked="true"></asp:TreeNode>
                    <asp:TreeNode Text="fortune(6)" ToolTip="fortune(6)" ShowCheckBox="true" Checked="false"></asp:TreeNode>
                    <asp:TreeNode Text="leaf0.1.3" ToolTip="node 1 leaf 3" ShowCheckBox="true" Checked="false"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="node0.2" ToolTip="node 2">                
                    <asp:TreeNode Text="leaf0.2.1" ToolTip="node 2 leaf 1" ShowCheckBox="true" Checked="false"></asp:TreeNode>
                    <asp:TreeNode Text="leaf0.2.2" ToolTip="node 2 leaf 2" ShowCheckBox="true" Checked="true"></asp:TreeNode>
                    <asp:TreeNode Text="leaf0.2.3" ToolTip="node 2 leaf 3" ShowCheckBox="true" Checked="false"></asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="node0.3" ToolTip="node 3">     
                    <asp:TreeNode Text="node0.3.1" ToolTip="node 3 subnode 1">
                        <asp:TreeNode Text="leaf0.3.1.1" ToolTip="node 3 subnode 1 leaf 1" ShowCheckBox="true" Checked="true"></asp:TreeNode>
                    </asp:TreeNode>
                </asp:TreeNode>
            </asp:TreeNode>    
        </Nodes>
    </asp:TreeView>
</div>
