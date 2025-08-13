<%@ Page Title="PdfMerge (apache2 mod_mono)" Language="C#" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" EnableSessionState="true" CodeBehind="PdfMerge.aspx.cs" Inherits="Area23.At.Mono.Unix.PdfMerge" %>
<asp:Content ID="UnixHeadContent" ContentPlaceHolderID="UnixHead" runat="server" ClientIDMode="Static">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <script async src="../res/js/area23.js"></script>
    <title>pdf file merge (apache2 mod_mono)</title>    
</asp:Content>
<asp:Content ID="UnixBodyContent" ContentPlaceHolderID="UnixBody" ClientIDMode="Static" runat="server">
    <form id="Area23UnixPdfMerge" runat="server">
        <h2>Upload several Pdf's, merge & download</h2>
        <div class="odDiv" style="vertical-align: top;">                       
            <span class="leftSpan" style="vertical-align: top;">
                <INPUT id="oFile" type="file" runat="server" ClientIDMode="Static" NAME="oFile" /> 
            </span>
            <span class="centerSpan" style="max-width: 72px; vertical-align: top;">
                <asp:ListBox ID="ListBoxFilesUploaded" runat="server" ClientIDMode="Static" ToolTip="Uploaded files" OnSelectedIndexChanged="ListBoxFilesUploaded_SelectedIndexChanged"  Rows="8" Height="220px" Width="220px" />
            </span>
            <span class="centerSpan" style="vertical-align: top;">     
                <asp:Button ID="ButtonPdfMerge" runat="server" ClientIDMode="Static" ToolTip="Merge all uploaded pdfs to one output pdf file" OnClick="ButtonPdfMerge_Click" Text="Merge pdf's" />  
            </span>
            <span class="rightSpan" style="vertical-align: top;"> 
                <asp:Label id="LabelUploadResult" runat="server" ClientIDMode="Static" ToolTip="File succesfully uploaded!" Text="File succesfully uploaded" Visible="false" /> 
                <a id="aPdfMergeDownload" runat="server" href="#" onclick="javascript:Alert('Upload pdfs first, then merge, then download');" title="Download merged pdf">Download merged pdf</a>
            </span>
        </div>    
        <div class="odDiv" id="DivObject" runat="server" visible="false">
            <object data="data:application/pdf;base64,<%= Base64Mime %>" type='application/pdf' width="640px" height="480px">
                <p>Unable to display type application/pdf</p>
            </object>
        </div>        
    </form>
</asp:Content>
