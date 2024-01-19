<%@ Page Title="Json Deserialize" Language="C#" MasterPageFile="~/Area23.Master" AutoEventWireup="true"  %>
<%@ Import namespace="Newtonsoft.Json" %>
<%@ Import namespace="Newtonsoft.Json.Linq" %>
<%@ Import namespace="Newtonsoft.Json.Bson" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Linq" %>
<%@ Import namespace="System.Reflection" %>
<%@ Import namespace="System.Web"%>
<%@ Import namespace="System.IO"%>
<%@ Import namespace="System.Diagnostics" %>
<%@ Import namespace="System.Web.UI"  %>
<%@ Import namespace="System.Web.UI.WebControls" %>
<%@ Import namespace="System.Xml" %>
<%@ Import namespace="System.Xml.Linq" %>
<%@ Import namespace="System.Xml.Serialization" %>
<%@ Import namespace="System.Runtime.Serialization" %>
<%@ Import namespace="System.Runtime.Serialization.Json" %>
<%@ Import namespace="Area23.At.Mono.Util" %>

<script runat="server" language="C#">

    const string dlm = "-";
    void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.LiteralDateTime.Text = Constants.DateArea23 + " loading page json 2 xml & json deserialize tree paths sample...";
            this.TextBoxJson.Text = Constants.JSON_SAMPLE;
            this.LinkButtonEmpty.Text = "empty json form";
        }
    }

    void JsonDeserialize_Click(Object sender, EventArgs e)
    {
        string outs0 = String.Empty;
        string outi0 = String.Empty;
        string js0 = this.TextBoxJson.Text;

        if (string.IsNullOrEmpty(js0) || js0.Length < 8)
        {
            this.jsonPreOut.InnerText = "JSON string is null or shorter then 8 characters \r\n";
            // TextBoxOut.Text = "JSON string is null or shorter then 8 characters \r\n";
            return;
        }
        this.jsonPreOut.InnerText = "";
        // TextBoxOut.Text = "";
        this.LiteralDateTime.Text = Constants.DateArea23 + "json 2 xml.";
        try
        {
            var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(
                Encoding.ASCII.GetBytes(js0), new XmlDictionaryReaderQuotas()));
            outs0 += xml + "\r\n";        
        }
        catch (Exception ex0)
        {
            outs0 += String.Format(
                "Exception in JsonConvert.DeserializeObject(jsonString): \r\n\tMessage = {0} \r\n\tException: {1} \r\n",
                ex0.Message, ex0);
        }
        this.jsonPreOut.InnerText += outs0;
        // TextBoxOut.Text += outs0;
    }

    void LinkButtonJsonTreePaths_Click(Object sender, EventArgs e)
    {
        string outs0 = String.Empty;
        string outi0 = String.Empty;
        string js0 = this.TextBoxJson.Text;

        if (string.IsNullOrEmpty(js0) || js0.Length < 8)
        {
            this.jsonPreOut.InnerText = "JSON string is null or shorter then 8 characters \r\n";
            // TextBoxOut.Text = "JSON string is null or shorter then 8 characters \r\n";
            return;
        }
        this.jsonPreOut.InnerText = "";
        // TextBoxOut.Text = "";
        this.LiteralDateTime.Text = Constants.DateArea23 + "json tree paths.";

        try
        {            
            outs0 += String.Format("JSON length = {0} \r\n", js0.Length);
            JObject o0 = (JObject)JsonConvert.DeserializeObject(js0);
            JToken root = o0.Root;
            try
            {
                if (o0 != null)
                {
                    outs0 += GetJsonTreeObject(root, outi0, 0, false);
                }
            }
            catch (Exception ex2)
            {
                outs0 += String.Format("Exception \tMessage = {0} \r\n\tException: {1} \r\n",
                    ex2.Message, ex2);
                throw new ApplicationException("Error when parsing json tree with reflection", ex2);
            }
        }
        catch (Exception ex0)
        {
            outs0 += String.Format(
                "Exception in JsonConvert.DeserializeObject(jsonString): \r\n\tMessage = {0} \r\n\tException: {1} \r\n",
                ex0.Message, ex0);
        }
        this.jsonPreOut.InnerText += outs0;
        // TextBoxOut.Text += outs0;
    }

    void LinkButtonEmpty_Click(object sender, EventArgs e)
    {
        if (this.LinkButtonEmpty.Text == "empty json form")
        {
            this.LiteralDateTime.Text = Constants.DateArea23 + "Clearing json form.";
            this.TextBoxJson.Text = "";
            this.jsonPreOut.InnerText = "";
            // TextBoxOut.Text = "";
            this.LinkButtonEmpty.Text = "use json default sample";
        }
        else if (this.LinkButtonEmpty.Text == "use json default sample") 
        {
            this.LiteralDateTime.Text = Constants.DateArea23 + "Prefilling json form with sample.";
            this.TextBoxJson.Text = Constants.JSON_SAMPLE;
            this.LinkButtonEmpty.Text = "empty json form";
        }            
    }

    string GetJsonTreeObject(JToken o, string outp, int depth, bool html = false)
    {
        int jsninc = 1;
        string NEWLINE = (html) ? "\r\n" : "\r\n";
        string name = string.Empty;
        string depthStr = (depth > 99) ? depth.ToString() : (depth < 10) ? "  " + depth : " " + depth;

        try
        {
            Type type = o.GetType();
            JToken rootToken = o.Root;
            string path = o.Path;

            JContainer parent = (o.Parent != null) ? (JContainer)o.Parent : null;
            if (parent != null && !String.IsNullOrEmpty(path) && parent.Path == path)
                jsninc = 0;
            else
            {
                if (depth == 0)
                    outp += String.Format("{0} \t{1} \r\n", depthStr, type);
                else
                    outp += String.Format("{0} \t{1} \r\n", depthStr, path);
            }

            foreach (JToken jChildToken in o.Children())
            {
                if (jChildToken.HasValues)
                {
                    string oton = string.Empty;
                    outp += GetJsonTreeObject(jChildToken, oton, depth + jsninc);
                }
            }
        }
        catch (Exception ex3)
        {
            outp += "Exception in Reflection ttye.GetFields() or GetProperties(): \r\n";
            outp += String.Format("\tMessage = {0} \r\n\tException: {1} \r\n", ex3.Message, ex3);
        }
        return outp;
    }

</script>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link rel="stylesheet" href="https://area23.at/css/fortune.css" />
    <link rel="stylesheet" href="res/css/area23.at.mono.css" />
    <title>json ser (apache2 mod_mono)</title>
    <script async src="res/js/area23.js"></script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <form id="Area23MasterForm" runat="server">
        <asp:Literal ID="LiteralDateTime" runat="server"></asp:Literal>
        <div class="jsonRow" style="display:block; width:100%;">
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <asp:TextBox ID="TextBoxJson" runat="server" TextMode="MultiLine" ToolTip="Put your JSON string here" Width="98%" Height="320px" 
                    Style="table-layout: fixed;" />
            </div>
            <div class="jsonColumn" style="width:49%; float: left; display: inline-block;">
                <pre id="jsonPreOut" class="jsonPreOut" runat="server" style="margin-top: -12px; height: 332px;max-height: 332px;overflow: auto;" />
                <!-- pre id="Pre1"  /-->
                <!-- asp:TextBox ID="TextBoxOut" TextMode="MultiLine" ToolTip="output of json operation" Width="100%" Height="320px" /-->
            </div>
        </div>
        <!-- hr /-->
        <div align="left" class="jsonButtons" style="clear: both;">
            <asp:LinkButton ID="LinkButtonJSON" runat="server"  ToolTip="deserialize json and serialize it back to xml" Text="json2xml"  OnClick="JsonDeserialize_Click"></asp:LinkButton> &nbsp;
            <asp:LinkButton ID="LinkButtonJsonTreePaths" runat="server" ToolTip="deserialize only json tree paths" Text="json tree paths" OnClick="LinkButtonJsonTreePaths_Click" /> &nbsp; 
            <asp:LinkButton ID="LinkButtonEmpty" runat="server" Text="empty json form" OnClick="LinkButtonEmpty_Click" /> &nbsp;           
        </div>
    </form>
</asp:Content>
