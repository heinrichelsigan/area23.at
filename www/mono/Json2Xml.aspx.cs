using Area23.At;
using Area23.At.Framework.Library.Static;
using System;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Area23.At.Mono
{
    /// <summary>
    /// Json2Xml Xml2Json transfermator
    /// </summary>
    public partial class Json2Xml : System.Web.UI.Page
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
                return;
            }
            this.jsonPreOut.InnerText = "";            
            this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "json 2 xml.";
            try
            {
                outs0 += Framework.Library.Static.Utils.Json2Xml(js0);
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
            string json = Framework.Library.Static.Utils.Xml2Json(xml);
            outs0 += json.Replace(",\"", ",\r\n\"") + "\r\n";
            this.jsonPreOut.InnerText += outs0;
        }



        protected void LinkButtonJsonTreePaths_Click(Object sender, EventArgs e)
        {
            string outi0 = String.Empty;
            string js0 = this.TextBoxJson.Text;

            string outs0 = Framework.Library.Static.Extensions.GetJsonLevelTree(js0);

            if (string.IsNullOrEmpty(js0) || js0.Length < 8)
            {
                this.jsonPreOut.InnerText = "JSON string is null or shorter then 8 characters \r\n";
                // TextBoxOut.Text = "JSON string is null or shorter then 8 characters \r\n";
                return;
            }
            this.jsonPreOut.InnerText = "";
            // TextBoxOut.Text = "";
            this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " json tree paths.";

            
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
            if (Framework.Library.Static.Extensions.IsValidJson(this.TextBoxJson.Text))
            {
                this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " json 2 xml.";
                this.LinkButtonJSON.Visible = true;
                this.LinkButtonJSON.BackColor = System.Drawing.Color.Green;
                this.LinkButtonJsonTreePaths.BackColor = System.Drawing.Color.IndianRed;
                this.LinkButtonXML2Json.BackColor = System.Drawing.Color.IndianRed;
            }
            else if (Framework.Library.Static.Extensions.IsValidXml(this.TextBoxJson.Text))
            {
                this.LiteralDateTime.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " xml 2 json.";
                this.LinkButtonJSON.BackColor = System.Drawing.Color.IndianRed;
                this.LinkButtonJsonTreePaths.BackColor = System.Drawing.Color.IndianRed;
                this.LinkButtonXML2Json.BackColor = System.Drawing.Color.Green;
            }
        }

        

    }
}