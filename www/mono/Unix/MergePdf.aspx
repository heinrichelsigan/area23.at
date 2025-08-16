<%@ Page Title="MergePdf (apache2 mod_mono)" Language="C#" MasterPageFile="~/Unix/UnixMaster.master" AutoEventWireup="true" CodeBehind="MergePdf.aspx.cs" Inherits="Area23.At.Mono.Unix.MergePdf" %>
<asp:Content ID="UnixHeadContent" ContentPlaceHolderID="UnixHead" runat="server" ClientIDMode="Static">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="../res/css/area23.at.mono.css" />
    <script async src="../res/js/area23.js"></script>
    <script type="text/javascript">
        function UploadFile(fileUpload, buttonToClick) {

            if (fileUpload.value != '') {

                if (buttonToClick != null && buttonToClick.value != '' && document.getElementById(buttonToClick) != null) {
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
        <div class="odDiv" style="vertical-align: top; display: block;">                       
            <span class="leftSpan" style="vertical-align: top;">
                <asp:FileUpload ID="FileUploadInput" runat="server" ClientIDMode="Static" ToolTip="Upload file" AllowMultiple="false" />
                <asp:Button ID="UploadID" CommandName="UploadName" Text="Upload" runat="server" OnClick="Upload_Click" Style="display: none" />
            </span>
            <span class="centerSpan">
                <asp:Button ID="ButtonPdfMerge" runat="server" ClientIDMode="Static" ToolTip="Merge all uploaded pdfs to one output pdf file" 
                    OnClick="ButtonPdfMerge_Click" Text="Merge pdfs" />  
            </span>
            <span class="rightSpan">
                <asp:Button ID="ButtonClear" runat="server" ClientIDMode="Static" OnClick="ButtonClear_Click" Text="Clear" ToolTip="Clear entire form" />                
            </span>
        </div>
        <div style="display: block; clear: both;">
            <table>
                <tr>
                    <td rowspan="4">
                        <asp:ListBox ID="ListBoxFilesUploaded" runat="server" ClientIDMode="Static" ToolTip="Uploaded files"
                        OnSelectedIndexChanged="ListBoxFilesUploaded_SelectedIndexChanged"  Rows="8" Height="192px" Width="480px" />
                    </td>
                    <td>
                        <asp:ImageButton ID="ImButtonUp" runat="server" ClientIDMode="Static" ImageUrl="../res/img/arrow/arrow_up.gif" OnClick="ImButtonUp_ArrowUp" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="ImButtonDel" runat="server" ClientIDMode="Static" ImageUrl="../res/img/arrow/close_delete.gif" OnClick="ImButtonDel_Delete" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="ImButtonMerge" runat="server" ClientIDMode="Static" ImageUrl="../res/img/symbol/file_working.gif" OnClick="ImButtonMerge_Merge" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="ImButtonDown" runat="server" ClientIDMode="Static" ImageUrl="../res/img/arrow/arrow_down.gif" OnClick="ImButtonDown_ArrowDown" />
                    </td>
                </tr>
            </table>          
        </div>
        <div id="DivDownload" style="clear:both; display: block;">
            <span class="leftSpan">
                <asp:CheckBox ID="CheckBoxPreview" runat="server" ClientIDMode="Static" Checked="false" Text="Preview" />                
            </span>
            <span class="centerSpan" id="SpanDownload" title="merged.pdf" runat="server" visible="false" >
                Downlaod: <a href="#" title="Download merged pdf" target="_blank">Download merged pdf</a>
            </span>            
        </div>
        <div id="DivLabel" runat="server" style="clear:left; display: block;">                        
            &nbsp;
        </div>
        <asp:Label id="LabelUploadResult" runat="server" ClientIDMode="Static" ToolTip="File succesfully uploaded!" Text="File succesfully uploaded" Visible="false" /> 
        <div style="clear:left; display: block; margin-left: 40px" id="DivObject" runat="server" visible="false">
            <p>&nbsp;</p>
        </div>       
    </form>
    <div style="clear:left; display: block;">
        &nbsp;
    </div>
</asp:Content>
