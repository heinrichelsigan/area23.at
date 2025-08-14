<%@ Page Title="MergePdf (apache2 mod_mono)" Language="C#" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="MergePdf.aspx.cs" Inherits="Area23.At.Mono.Unix.MergePdf" %>
<asp:Content ID="UnixHeadContent" ContentPlaceHolderID="UnixHead" runat="server" ClientIDMode="Static">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <script async src="../res/js/area23.js"></script>
    <script type="text/javascript">
        function UploadFile(fileUpload, buttonToClick) {

            if (fileUpload.value != '') {

                if ((buttonToClick != null || buttonToClick.value != '') && document.getElementById(buttonToClick) != null) {
                    document.getElementById(buttonToClick).click();
                    return;
                }
                else {
                    var buttonUploadId = "UploadID";
                    if (document.getElementById(buttonUploadId) != null) {
                        document.getElementById(buttonUploadId).click();
                        return;
                    }
                    const buttonUploadNames = document.getElementsByName("UploadName");
                    if (buttonUploadNames != null && buttonUploadNames.length > 0) {
                        buttonUploadId = buttonUploadNames[0].id;
                    }
                    if (document.getElementById(buttonUploadId) != null) {
                        document.getElementById(buttonUploadId).click();
                        return;
                    }
                }
                document.getElementById("<%=UploadID.ClientID %>").click();
            }
        }        
    </script>
    <title>pdf's file merge (apache2 mod_mono)</title>    
</asp:Content>
<asp:Content ID="UnixBodyContent" ContentPlaceHolderID="UnixBody" ClientIDMode="Static" runat="server">
    <form id="MergePdfForm" runat="server">
        <h2>Upload several pdf's, merge & download</h2>
        <div class="odDiv" style="vertical-align: top; display: block; height: 240px;">                       
            <span class="leftSpan" style="vertical-align: top;">
                <asp:FileUpload ID="FileUploadInput" runat="server" ClientIDMode="Static" ToolTip="Upload file"  />
                <asp:Button ID="UploadID" CommandName="UploadName" Text="Upload" runat="server" OnClick="Upload_Click" Style="display: none" />
            </span>
            <span class="centerSpan" style="max-width: 72px; vertical-align: top;">
                <asp:ListBox ID="ListBoxFilesUploaded" runat="server" ClientIDMode="Static" ToolTip="Uploaded files" OnSelectedIndexChanged="ListBoxFilesUploaded_SelectedIndexChanged"  Rows="8" Height="220px" Width="220px" />
            </span>
            <span class="centerSpan" style="vertical-align: top;">     
                <asp:Button ID="ButtonPdfMerge" runat="server" ClientIDMode="Static" ToolTip="Merge all uploaded pdfs to one output pdf file" OnClick="ButtonPdfMerge_Click" Text="Merge pdf's" />  
            </span>
            <span class="rightSpan" style="vertical-align: top;">                 
                <a id="aPdfMergeDownload" runat="server" href="#" title="Download merged pdf">Download merged pdf</a>
            </span>
        </div>
        <div id="DivLabel" runat="server" style="clear:left; display: block;">
            <asp:Label id="LabelUploadResult" runat="server" ClientIDMode="Static" ToolTip="File succesfully uploaded!" Text="File succesfully uploaded" Visible="false" /> 
        </div>
        <div style="clear:left; display: block; margin-left: 40px" id="DivObject" runat="server" visible="false">
            <p>&nbsp;</p>
        </div>        
        <asp:Button ID="ButtonClear" runat="server"  ClientIDMode="Static" OnClick="ButtonClear_Click" Text="Clear" ToolTip="Clear entire form" />
    </form>
    <div style="clear:left; display: block;">
        &nbsp;
    </div>
</asp:Content>
