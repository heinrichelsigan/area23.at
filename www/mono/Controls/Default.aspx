<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Area23.At.Mono.Controls.Default" %>
<%@ Register TagPrefix="uc23" TagName="LetterImageControl" Src="~/Controls/LetterImageControl.ascx" %> 
<%@ Register TagPrefix="uc23" TagName="TreeViewControl" Src="~/Controls/TreeViewControl.ascx" %> 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc23:LetterImageControl ID="letterImageControl" runat="server" />
            <uc23:TreeViewControl ID="treeViewControl" runat="server" />
        </div>
    </form>
</body>
</html>
