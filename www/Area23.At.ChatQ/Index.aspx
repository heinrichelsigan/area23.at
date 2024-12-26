<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Area23.At.ChatQ.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post" enctype="text/plain">
        <div>
            <asp:TextBox ID="TextBoxSubmit" runat="server" TextMode="MultiLine" MaxLength="65536" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="[Enter text to en-/decrypt here]" Text="" Width="480px"></asp:TextBox>
            <br />
            <asp:TextBox ID="TextBoxLastMsg" runat="server" ReadOnly="true" TextMode="MultiLine" MaxLength="65536" Rows="10" Columns="48" ValidateRequestMode="Disabled" ToolTip="[Enter text to en-/decrypt here]" Text="" Width="480px"></asp:TextBox>
            <br />
            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" ToolTip="Submit" OnClick="ButtonSubmit_Click" />
        </div>
    </form>
    <pre id="pre1" runat="server">

    </pre>
</body>
</html>
