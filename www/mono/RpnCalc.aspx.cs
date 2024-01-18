using Area23.At.Mono.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono
{
    public partial class RpnCalc : RpnBasePage
    {
        private int _textCursor = 9;
        internal int TextCursor
        {
            get => _textCursor;
            set => _textCursor = (value > 0 && value <= 9) ? value : _textCursor;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (metacursor.Attributes["content"] == null)
                    metacursor.Attributes.Add("content", TextCursor.ToString());
                else
                    metacursor.Attributes["content"] = TextCursor.ToString();
            }
            _textCursor = (metacursor.Attributes["content"] != null) ? Int32.Parse(metacursor.Attributes["content"]) : _textCursor;
        }

        public TextBox FindTextBox(int cursor = -1)
        {
            if (cursor < 1 || cursor > 9)
            {
                _textCursor = (metacursor.Attributes["content"] != null) ? Int32.Parse(metacursor.Attributes["content"]) : _textCursor;
                cursor = TextCursor;
            }
            string findTxtBx = "textbox" + cursor;
            var control = ((Area23)(this.Master)).MasterBody.FindControl(findTxtBx);
            if (control is TextBox)
                return (TextBox)control;
            return null;
        }

        public TextBox CurrentTextBox { get => FindTextBox(); }

        protected void checkBoxRpnCalc_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rpnCalc_Click(object sender, EventArgs e)
        {
            object o1 = sender;
            string eName = e.GetType().Name;

        }

        protected void bBracers_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
        }

        protected void bMath_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            if (!string.IsNullOrEmpty(mathString))
            {
                this.CurrentTextBox.Text += mathString.ToString();
                TextCursor--;
                if (metacursor.Attributes["content"] == null)
                    metacursor.Attributes.Add("content", TextCursor.ToString());
                else
                    metacursor.Attributes["content"] = TextCursor.ToString();
            }
        }

        protected void bSin_Click(object sender, EventArgs e)
        {

        }

        protected void bNumber_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.CurrentTextBox.Text += mathString.ToString();
        }

        protected void bEnter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.CurrentTextBox.Text))
            {
                TextCursor--;
                if (metacursor.Attributes["content"] == null)
                    metacursor.Attributes.Add("content", TextCursor.ToString());
                else
                    metacursor.Attributes["content"] = TextCursor.ToString();
            }

        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            this.textbox0.Text = "";
            this.textbox1.Text = "";
            this.textbox2.Text = "";
            this.textbox3.Text = "";
            this.textbox4.Text = "";
            this.textbox5.Text = "";
            this.textbox7.Text = "";
            this.textbox8.Text = "";
            this.textbox9.Text = "";
        }

        protected void Bdel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.CurrentTextBox.Text) && CurrentTextBox.Text.Length > 0)
            {
                this.CurrentTextBox.Text = CurrentTextBox.Text.Substring(0, CurrentTextBox.Text.Length - 1);
            }
        }
    }
}
