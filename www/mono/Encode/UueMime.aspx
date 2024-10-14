<%@ Page Title="Simple uu and base64 en-/decode tool (apache2 mod_mono)" Language="C#" MasterPageFile="~/Encode/EncodeMaster.master" AutoEventWireup="true" CodeBehind="UueMime.aspx.cs" Inherits="Area23.At.Mono.Encode.UueMime"  validateRequest="false" %>
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server">
        <title>Simple uu and base64 en-/decode tool (apache2 mod_mono)</title>
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta name="keywords" content="encode decode uuencode uudecode mime base64 aes encrypt decrypt" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" />
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server">
    <form id="EncodeMasterForm" runat="server" action="UueMime.aspx" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">  
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxSource" runat="server" TextMode="MultiLine" MaxLength="32768" Rows="10" Columns="48" ValidateRequestMode="Disabled" 
                    ToolTip="Enter your text to en-/decode here" Text="[Enter text to en-/decrypt here]" 
                     Width="98%" Height="320px" Style="table-layout: fixed;"></asp:TextBox>
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <pre id="preOut" class="jsonPreOut" runat="server" style="margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 0px hidden white;" />
                <!-- pre id="Pre1"  /-->
                <!-- asp:TextBox ID="TextBoxOut" TextMode="MultiLine" ToolTip="output of json operation" Width="100%" Height="320px" /-->
            </div>
        </div>
        <!-- hr /-->
        <div class="odDiv">
            <span class="leftSpan">
                <asp:DropDownList ID="DropDownList_EncodeType" runat="server">
                    <asp:ListItem Enabled="true" Value="hex16" Selected="false">Hex16</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base16" Selected="false">Base16</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base32" Selected="false">Base32</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base32hex" Selected="false">Base32Hex</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="base64" Selected="true">Base64</asp:ListItem>                    
                    <asp:ListItem Enabled="true" Value="html"  Selected="false">Html</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="url"  Selected="false">Url</asp:ListItem>
                    <asp:ListItem Enabled="true" Value="uu" Selected="false">Uu</asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="centerSpan">
                <INPUT id="oFile" type="file" runat="server" NAME="oFile" onselect="oFile_Submit"  />
                <asp:Button ID="Button_UploadFile_Encode" runat="server" ToolTip="Encode uploaded file" Text="Encode file" OnClick="Button_UploadFile_Encode_Click" />
            </span>
            <span class="centerSpan">
                <asp:LinkButton ID="LinkButton_Encode" runat="server"  ToolTip="Encode text" Text="Encode" OnClick="LinkButton_Encode_Click"></asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="LinkButton_Decode" runat="server" ToolTip="Decode text" Text="Decode" OnClick="LinkButton_Decode_Click" /> &nbsp;
            </span>
            <span class="rightSpan">
                <asp:Button ID="Button_Clear" runat="server" Text="Clear" ToolTip="Clear Form" OnClick="Button_Clear_Click" />
            </span>
        </div>
         
    </form>
</asp:Content>
