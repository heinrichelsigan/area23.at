using Area23.At.Framework.Library.Static;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;

namespace Area23.At.Mono
{
    public partial class Json : System.Web.UI.Page
    {
        const string dlm = "-";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.LiteralDateTime.Text = Constants.DateArea23 + " loading page json 2 xml & json deserialize tree paths sample...";
                if (Constants.RandomBool)
                    this.TextBoxJson.Text = Constants.JSON_SAMPLE;
                else
                    this.TextBoxJson.Text = Constants.XML_SAMPLE;

                this.LinkButtonEmpty.Text = "empty json form";
            }

            TextBoxJson_OnTextChanged(sender, e);

        }

        protected void JsonDeserialize_Click(Object sender, EventArgs e)
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
            this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "json 2 xml.";
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
        }

        protected void Xml2Json_Click(Object sender, EventArgs e)
        {
            string outs0 = String.Empty;
            string outi0 = String.Empty;
            string xml = this.TextBoxJson.Text;

            if (string.IsNullOrEmpty(xml) || xml.Length < 8)
            {
                this.jsonPreOut.InnerText = "XML string is null or shorter then 8 characters \r\n";
                return;
            }

            this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " xml 2 json.";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);

            outs0 += json.Replace(",\"", ",\r\n\"") + "\r\n";
            this.jsonPreOut.InnerText += outs0;
        }



        protected void LinkButtonJsonTreePaths_Click(Object sender, EventArgs e)
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
            this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " json tree paths.";

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

        protected void LinkButtonEmpty_Click(object sender, EventArgs e)
        {
            if (this.LinkButtonEmpty.Text == "empty json form")
            {
                this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " clearing json form.";
                this.TextBoxJson.Text = "";
                this.jsonPreOut.InnerText = "";
                // TextBoxOut.Text = "";
                this.LinkButtonEmpty.Text = "use json default sample";
            }
            else if (this.LinkButtonEmpty.Text == "use json default sample")
            {
                this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " prefilling json form with sample.";
                this.TextBoxJson.Text = Constants.JSON_SAMPLE;
                this.LinkButtonEmpty.Text = "empty json form";
            }
        }

        protected void TextBoxJson_OnTextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxJson.Text.IsValidJson())
            {
                this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " json 2 xml.";
                this.LinkButtonJSON.Visible = true;
                this.LinkButtonJSON.BackColor = System.Drawing.Color.Green;
                this.LinkButtonJsonTreePaths.BackColor = System.Drawing.Color.IndianRed;
                this.LinkButtonXML2Json.BackColor = System.Drawing.Color.IndianRed;
            }
            else if (this.TextBoxJson.Text.IsValidXml())
            {
                this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " xml 2 json.";
                this.LinkButtonJSON.BackColor = System.Drawing.Color.IndianRed;
                this.LinkButtonJsonTreePaths.BackColor = System.Drawing.Color.IndianRed;
                this.LinkButtonXML2Json.BackColor = System.Drawing.Color.Green;
            }
        }

        protected string GetJsonTreeObject(JToken o, string outp, int depth, bool html = false)
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

    }
}