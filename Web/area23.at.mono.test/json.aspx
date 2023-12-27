<%@ Page Language="C#" AutoEventWireup="true" %>
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
<%@ Import namespace="System.Xml.Serialization" %>
<%@ Import namespace="System.Runtime.Serialization" %>
<%@ Import namespace="System.Runtime.Serialization.Json" %>
<%@ Import namespace="JsonDeserializer.Util" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Deserialize JSON</title>
    </head>

    <script runat="server" language="C#">

        const string dlm = "-";
        void Page_Load(object sender, EventArgs e)
        {
            this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy") + dlm + DateTime.UtcNow.ToString("MM") + dlm + DateTime.UtcNow.ToString("dd") + " " +
                DateTime.UtcNow.ToShortTimeString() + ": ";

            if (!this.IsPostBack)
            {
                this.TextBoxJson.Text = Constants.JsonSample;
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
                preOut.InnerText = "JSON string is null or shorter then 8 characters \r\n";
                return;
            }
            preOut.InnerText = "";

            try
            {
                var xml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(
                    Encoding.ASCII.GetBytes(js0), new XmlDictionaryReaderQuotas()));
                outs0 += xml + "\r\n";

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
            preOut.InnerText += outs0;
        }

        void LinkButtonEmpty_Click(object sender, EventArgs e)
        {
            if (this.LinkButtonEmpty.Text == "empty json form")
            {
                this.TextBoxJson.Text = "";
                preOut.InnerText = "";
                this.LinkButtonEmpty.Text = "use json default sample";
            }
            else if (this.LinkButtonEmpty.Text == "use json default sample") 
            {
                this.TextBoxJson.Text = Constants.JsonSample;
                this.LinkButtonEmpty.Text = "empty json form";
            }            
        }

        string GetJsonTreeObject(JToken o, string outp, int depth, bool html = false)
        {
            int jsninc = 1;
            string NEWLINE = (html) ? "<br />\r\n" : "\r\n";
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

    <body>
        <form id="form1" runat="server">
            <div align="left" style="width: 100%; padding-top: 8px; table-layout: fixed; inset-block-start: initial; background-color: #dfdfdf; font-size: large; border-style: none; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;">
                <span style="width:25%; font-size: large; background-color: #bfafcf; vertical-align:middle; text-align: left;" align="left" valign="middle"><a href="Fortune.aspx">fortune</a></span>
                <span style="width:25%; font-size: large; background-color: #afcfbf; vertical-align:middle; text-align: center;" align="center" valign="middle"><a href="json.aspx">json deserializer</a></span>
                <span style="width:25%; font-size: large; background-color: #cfbfaf; vertical-align:middle; text-align: center;" align="center" valign="middle"><a href="OctalDump.aspx">octal dump</a></span>
                <span style="width:25%; font-size: large; background-color: #dfdfdf; vertical-align:middle; text-align: right;" align="right" valign="middle">
                    &nbsp;
                </span>
            </div>
            <div align="left" style="text-align: left; padding-top: 12px; padding-bottom: 8px; background-color: #dfdfdf; width: 100%; table-layout: fixed; inset-block-start: initial; font-size: large; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', 'Geneva', 'Verdana', 'sans-serif'">
                <asp:Literal ID="LiteralDateTime" runat="server"></asp:Literal> &nbsp; 
                <asp:LinkButton ID="LinkButtonJSON" runat="server"  ToolTip="deserialize only paths in json" Text="deserialize json form"  OnClick="JsonDeserialize_Click"></asp:LinkButton> &nbsp;
                <asp:LinkButton ID="LinkButtonEmpty" runat="server" Text="empty json form" OnClick="LinkButtonEmpty_Click" /> &nbsp;                
            </div>
            <div style="width: 100%; table-layout: fixed; inset-block-start: initial; font-size: large; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', 'Geneva', 'Verdana', 'sans-serif'">
                <asp:TextBox ID="TextBoxJson" runat="server" TextMode="MultiLine" ToolTip="Put your JSON string here" Width="100%" Height="320px"></asp:TextBox>
            </div>
            <pre id="preOut" runat="server">
            </pre>
            <hr />
            <div align="left" style="width: 100%; table-layout: fixed; inset-block-start: initial; background-color: #dfdfdf; font-size: medium; border-style: none; font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;">
                <span style="width:12%; background-color: #bfafcf; vertical-align:middle; text-align: left;" align="left" valign="middle"><a href="Fortune.aspx">fortune</a></span>
                <span style="width:12%; background-color: #afcfbf; vertical-align:middle; text-align: center;" align="center" valign="middle"><a href="json.aspx">json deserializer</a></span>
                <span style="width:12%; background-color: #cfbfaf; vertical-align:middle; text-align: center;" align="center" valign="middle"><a href="OctalDump.aspx">octal dump</a></span>
                <span style="width:64%; background-color: #cfcfcf; vertical-align:middle; text-align: right;" align="right" valign="middle">
                    <a href="mailto:root@darkstar.work">Heinrich Elsigan</a>, GNU General Public License 2.0, [<a href="http://blog.darkstar.work">blog.</a>]<a href="https://darkstar.work">darkstar.work</a>
                </span>
            </div>
        </form>
    </body>
</html>
