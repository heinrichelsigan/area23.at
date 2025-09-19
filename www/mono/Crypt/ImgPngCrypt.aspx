<%@ Page Title="image png crypt (apache2 mod_mono)" Language="C#" MasterPageFile="~/Crypt/EncodeMaster.master" AutoEventWireup="true" CodeBehind="ImgPngCrypt.aspx.cs" Inherits="Area23.At.Mono.Crypt.ImgPngCrypt" validateRequest="true" %>
<%@ Register TagPrefix="uc23" TagName="HashKeyRadioButtonList" Src="~/Controls/HashKeyRadioButtonList.ascx" %> 
<asp:Content ID="ContentEncodeHead" ContentPlaceHolderID="EncodeHead" runat="server" ClientIDMode="Static">
        <title>image png crypt (apache2 mod_mono)</title>
        <link rel="stylesheet" href="https://area23.at/css/od.css" />
        <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <meta name="keywords" content="encode decode uuencode uudecode mime base64 aes encrypt decrypt" />
        <meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
        <meta name="author" content="Heinrich Elsigan (he@area23.at)" /> 
     <script type="text/javascript">
         function UploadFile(fileUpload, buttonToClick) {

             if (fileUpload.value != '') {

                 if ((buttonToClick != null || buttonToClick.value != '') && document.getElementById(buttonToClick) != null) {
                     document.getElementById(buttonToClick).click();
                     return;
                 }
                 else {
                     var buttonUploadId = "ButtonUploadID";
                     if (document.getElementById(buttonUploadId) != null) {
                         document.getElementById(buttonUploadId).click();
                         return;
                     }
                     const buttonUploadNames = document.getElementsByName("ButtonUploadName");
                     if (buttonUploadNames != null && buttonUploadNames.length > 0) {
                         buttonUploadId = buttonUploadNames[0].id;
                     }
                     if (document.getElementById(buttonUploadId) != null) {
                         document.getElementById(buttonUploadId).click();
                         return;
                     }
                 }
                 document.getElementById("<%=ButtonUploadID.ClientID %>").click();
             }
         }        
     </script>
</asp:Content>
<asp:Content ID="ContentEncodeBody" ContentPlaceHolderID="EncodeBody" runat="server"  ClientIDMode="Static">
    <form id="ImgPngCryptForm" runat="server" action="ImgPngCrypt.aspx" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">
        <INPUT id="oFile" type="file" runat="server" ClientIDMode="Static" NAME="oFile" onchange="UploadFile(this, 'ButtonUploadID')" />
        <asp:Button ID="ButtonUploadID" CommandName="ButtonUploadName" Text="Upload" ClientIDMode="Static" runat="server" OnClick="ButtonUpload_Click" Style="display: none" />
        <asp:RadioButtonList runat="server" ID="RadioOptions" ClientIDMode="Static">
            <asp:ListItem Text="TransForm" Enabled="true" Selected="True" />
            <asp:ListItem Text="ReForm"  Enabled="true" Selected="False" />
        </asp:RadioButtonList>
        <asp:Panel ID="frmConfirmation" Visible="False" Runat="server">
            <img id="imgIn" runat="server" border="0" alt="Image uploaded" width="600" />
            <br />
            <asp:Label id="lblUploadResult" Runat="server"></asp:Label>
            <br />
            <img id="imgOut" runat="server" border="0" alt="Image transformed" />
         </asp:Panel>
    </form>
</asp:Content>
