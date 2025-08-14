<%@ Page Title="PdfMerge (apache2 mod_mono)" Language="C#" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" EnableSessionState="true" CodeBehind="PdfMerge.aspx.cs" Inherits="Area23.At.Mono.Unix.PdfMerge" %>
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
    <title>pdf file merge (apache2 mod_mono)</title>    
</asp:Content>
<asp:Content ID="UnixBodyContent" ContentPlaceHolderID="UnixBody" ClientIDMode="Static" runat="server">
    <form id="Area23UnixPdfMerge" runat="server" style="margin-left: 12px;">
        <h2>Upload several pdf's, merge & download</h2>
        <div class="odDiv" style="vertical-align: top; display: block;">                       
            <span class="leftSpan" style="vertical-align: top;">                
                <INPUT id="oFile" type="file" runat="server" ClientIDMode="Static" NAME="oFile" onchange="UploadFile(this)" />
                <asp:Button ID="ButtonUploadID" CommandName="ButtonUploadName" Text="Upload" runat="server" OnClick="ButtonUpload_Click" Style="display: none" />
            </span>
        </div>
        <div style="display: block; clear: both; min-width: 320px; max-width: 540px; min-height: 180px; max-height: 240px;">
            <span class="leftSpan" style="min-width: 320px; max-width: 540px; min-height: 180px; max-height: 240px;">
                <asp:ListBox ID="ListBoxFilesUploaded" runat="server" ClientIDMode="Static" ToolTip="Uploaded files"
                    OnSelectedIndexChanged="ListBoxFilesUploaded_SelectedIndexChanged"  Rows="8" Height="200px" Width="436px" />
            </span>            
        </div>
        <div id="DivButtons" style="clear:both; display: block;">
            <span class="leftSpan">
                <asp:Button ID="ButtonPdfMerge" runat="server" ClientIDMode="Static" ToolTip="Merge all uploaded pdfs to one output pdf file" 
                    OnClick="ButtonPdfMerge_Click" Text="Merge pdfs" />  
            </span>
            <span class="centerSpan">
                
            </span>
            <span class="rightSpan">
                <asp:Button ID="ButtonClear" runat="server" ClientIDMode="Static" OnClick="ButtonClear_Click" Text="Clear" ToolTip="Clear entire form" />                
            </span>
        </div>
        <div id="DivLabel" style="clear:both; display: block;">
            <span class="leftSpan" id="SpanDownload" runat="server" visible="false" style="display: none">
                Downlaod: <a id="aPdfMergeDownload" runat="server" href="#" title="Download merged pdf">Download merged pdf</a>
            </span>            
            <br />
            <asp:Label id="LabelUploadResult" runat="server" ClientIDMode="Static" ToolTip="File succesfully uploaded!" Text="File succesfully uploaded" Visible="false" /> 
        </div>
        <div style="clear:left; display: none;" id="DivObject" runat="server" visible="false">
            <p>&nbsp;</p>
        </div>                        
    </form>
    <div style="clear:both; display: block;">
        &nbsp;
    </div>
</asp:Content>
