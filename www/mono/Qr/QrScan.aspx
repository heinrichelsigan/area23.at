<%@ Page Language="C#"  MasterPageFile="~/Qr/QRMaster.master" AutoEventWireup="true" CodeBehind="QrScan.aspx.cs" Inherits="Area23.At.Mono.Qr.QrScan" %>
<asp:Content ID="QrHeadContent" ContentPlaceHolderID="QrHead" runat="server">
	<title>qr code scanner (apache2 mod_mono)</title>
	<link rel="stylesheet" href="../res/css/area23.at.mono.css" />
	<meta name="keywords" content="QR code scanner" />
	<meta name="description" content="https://github.com/heinrichelsigan/area23.at/" />
	<title>QrScan (apache2 mod_mono)</title>
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
<asp:Content ID="QrBodyContent" ContentPlaceHolderID="QrBody" runat="server">
	<form id="Form_QrScant" runat="server" action="QrScan.aspx" method="post" enableviewstate="True" enctype="multipart/form-data" submitdisabledcontrols="True">
        <INPUT id="oFile" type="file" runat="server" ClientIDMode="Static" NAME="oFile" onchange="UploadFile(this, 'ButtonUploadID')" />
        <asp:Button ID="ButtonUploadID" CommandName="ButtonUploadName" Text="Upload" ClientIDMode="Static" runat="server" OnClick="ButtonUpload_Click" Style="display: none" />
        <asp:Panel ID="PanelUploadImage" Visible="False" Runat="server">
            <table width="640px" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td align="left"><a ID="aOrigImg"  runat="server" href="#" title="">Original Image</a></td>
                    <td align="center"><a ID="aNewQr" runat="server" href="#" title="">New generated Qr-Image</a></td>
                </tr>
                <tr>
                    <td align="left"><img id="ImgQrIn" runat="server" border="0" src="#" alt="Image uploaded" width="300" /></td>
                    <td align="center"><img id="ImgQrOut" runat="server"  src="#" border="0" alt="Image uploaded" width="300" /></td>
                </tr>
                <tr>
                    <td align="left" colspan="2"><asp:Label id="LabelUpload" Runat="server"></asp:Label></td>                    
                </tr>
                <tr>
                    <td colspan="2" align="left"><asp:TextBox ID="TextBoxQrDecoded" runat="server" ClientIDMode="Static" 
                        TextMode="MultiLine" ReadOnly="true" Columns="10" Rows="80" Width="480" Height="192"></asp:TextBox></td>
                </tr>
            </table>            
            <br />            
        </asp:Panel>
    </form>
</asp:Content>