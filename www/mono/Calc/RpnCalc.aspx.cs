﻿using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Calc 
{
    public partial class RpnCalc : Util.UIPage
    {
        Stack<string> rpnStack = new Stack<string>();

        private int _textCursor = 0;
        internal int TextCursor
        {
            get => _textCursor;
            set => _textCursor = (value > 0 && value <= 18) ? value : _textCursor;
        }
        public TextBox CurrentTextBox { get => this.textboxRpn; }

        private RPNRad _currentRad = RPNRad.RAD;
        public string CurrentRad
        {
            get
            {
                switch (this.metarad.Attributes["content"])
                {
                    case "RAD": _currentRad = RPNRad.RAD; return RPNRad.RAD.ToString();
                    case "GRD": _currentRad = RPNRad.GRD; return RPNRad.GRD.ToString();
                    case "DEG":
                    default: _currentRad = RPNRad.DEG; return RPNRad.DEG.ToString();
                }
            }
            set
            {
                if (Enum.TryParse<RPNRad>(value, out _currentRad))
                {
                    switch (_currentRad)
                    {
                        case RPNRad.GRD:
                            this.metarad.Attributes["content"] = RPNRad.GRD.ToString();
                            this.Brad.Text = RPNRad.GRD.ToString();
                            this.Brad.BackColor = Color.FromKnownColor(KnownColor.ControlLightLight);
                            break;
                        case RPNRad.RAD:
                            this.metarad.Attributes["content"] = RPNRad.RAD.ToString();
                            this.Brad.Text = RPNRad.RAD.ToString();
                            this.Brad.BackColor = Color.FromKnownColor(KnownColor.ButtonShadow);
                            break;
                        case RPNRad.DEG:
                        default:
                            this.metarad.Attributes["content"] = RPNRad.DEG.ToString();
                            this.Brad.Text = RPNRad.DEG.ToString();
                            this.Brad.BackColor = Color.FromKnownColor(KnownColor.ButtonHighlight);
                            break;
                    }
                }
            }
        }

        public string CurrentArc
        {
            get => this.metaarc.Attributes["content"];
            set
            {
                if (value.ToUpper() == "ARC")
                {
                    this.metaarc.Attributes["content"] = "ARC";
                    this.Barc.BackColor = Color.FromKnownColor(KnownColor.ButtonShadow);
                    this.Barc.Text = "arc";
                    this.Bsin.Text = "asin";
                    this.Bcos.Text = "acos";
                    this.Btan.Text = "atan";
                    this.Bcot.Text = "acot";
                }
                else
                {
                    this.metaarc.Attributes["content"] = "";
                    this.Barc.BackColor = Color.FromKnownColor(KnownColor.ButtonHighlight);
                    this.Barc.Text = "ARC";
                    this.Bsin.Text = "sin";
                    this.Bcos.Text = "cos";
                    this.Btan.Text = "tan";
                    this.Bcot.Text = "cot";
                }
            }
        }

        public DateTime Change_Click_EventDate
        {
            get => (Session[Constants.CHANGE_CLICK_EVENTCNT] != null) ?
                (DateTime)Session[Constants.CHANGE_CLICK_EVENTCNT] : DateTime.MinValue;
            set => Session[Constants.CHANGE_CLICK_EVENTCNT] = value;
        }
        object bChange_Click_lock = new object();
        object bEnter_Click_lock = new object();

        protected void Page_Load(object sender, EventArgs e)
        {
            string jsonSerRpnStack = string.Empty;
            if (Session[Constants.RPN_STACK] != null)
            {
                rpnStack = (Stack<string>)Session[Constants.RPN_STACK];
            }
            else
            {
                rpnStack = new Stack<string>();
                if (metacursor.Attributes["content"] != null && !string.IsNullOrEmpty(metacursor.Attributes["content"]))
                {
                    jsonSerRpnStack = HttpUtility.HtmlDecode(metacursor.Attributes["content"].ToString());
                    rpnStack = JsonConvert.DeserializeObject<Stack<string>>(jsonSerRpnStack);
                }

                Session[Constants.RPN_STACK] = rpnStack;
            }
            if (!Page.IsPostBack)
            {
                jsonSerRpnStack = JsonConvert.SerializeObject(rpnStack);
                if (metacursor.Attributes["content"] == null)
                    metacursor.Attributes.Add("content", HttpUtility.HtmlEncode(jsonSerRpnStack));
                else
                    metacursor.Attributes["content"] = HttpUtility.HtmlEncode(jsonSerRpnStack);
                this.textboxtop.Focus();
            }
            if (!string.IsNullOrEmpty(this.metarad.Attributes["content"]))
            {
                Brad.Text = CurrentRad;
                string curRad = Brad.Text;
                CurrentRad = curRad;
            }
            if (!string.IsNullOrEmpty(this.metaarc.Attributes["content"]) &&
                this.metaarc.Attributes["content"].ToUpper() == "ARC")
            {
                this.CurrentArc = "ARC";
            }
            else
                this.CurrentArc = "";

            _textCursor = rpnStack.Count;
        }


        [Obsolete("rpnCalc_Click is obseolte", false)]
        protected void rpnCalc_Click(object sender, EventArgs e)
        {
            object o1 = sender;
            string eName = e.GetType().Name;

        }

        protected void bBracers_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
        }

        protected void bRad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.metarad.Attributes["content"]))
            {
                switch (this.metarad.Attributes["content"])
                {
                    case "RAD":
                        this.metarad.Attributes["content"] = RPNRad.GRD.ToString();
                        CurrentRad = RPNRad.GRD.ToString();
                        break;
                    case "GRD":
                        this.metarad.Attributes["content"] = RPNRad.DEG.ToString();
                        CurrentRad = RPNRad.DEG.ToString();
                        break;
                    case "DEG":
                    default:
                        this.metarad.Attributes["content"] = RPNRad.RAD.ToString();
                        CurrentRad = RPNRad.RAD.ToString();
                        break;
                }
            }
        }

        protected void bArc_Click(object sender, EventArgs e)
        {
            switch (this.metaarc.Attributes["content"])
            {
                case "":
                    this.CurrentArc = "ARC";
                    break;
                case "ARC":
                default:
                    this.CurrentArc = "";
                    break;
            }
        }

        protected void bModus_Click(object sender, EventArgs e)
        {
            //TODO: implement it
        }

        protected void bNumber_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.textboxtop.Text += mathString.ToString();
        }

        protected void bPiE_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.textboxtop.Text = mathString;
            TextCursor++;
            rpnStack.Push(textboxtop.Text);
            this.textboxtop.Text = string.Empty;
            RpnStackToTextBox();
            SetMetaContent();
        }


        protected void bInfinite_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.textboxtop.Text = mathString;
            TextCursor++;
            rpnStack.Push(textboxtop.Text);
            this.textboxtop.Text = string.Empty;
            RpnStackToTextBox();
            SetMetaContent();
        }


        protected void bMath_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text :
                (sender is TextBox) ? ((TextBox)sender).Text : "";

            if (!string.IsNullOrEmpty(mathString))
            {
                this.textboxtop.Text = mathString.ToString();
                if (ValidateMathOp1(mathString) == RPNType.MathOp1)
                {
                    rpnStack.Push(mathString.ToString());
                    TextCursor++;
                    RpnStackToTextBox();
                    SetMetaContent();
                    textboxtop.Text = string.Empty;
                }
                else
                {
                    this.textboxtop.BorderColor = Color.Red;
                    this.textboxtop.BorderStyle = BorderStyle.Dotted;
                    this.textboxtop.ToolTip = "Math op " + mathString + " requires at least 1 numbers at top of stack";
                    this.textboxRpn.BorderStyle = BorderStyle.Dashed;
                    this.textboxRpn.BorderColor = Color.Red;
                    this.textboxRpn.BorderWidth = 1;
                }
            }
        }

        protected void bMath2Op_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text :
                (sender is TextBox) ? ((TextBox)sender).Text : "";

            if (!string.IsNullOrEmpty(mathString))
            {
                this.textboxtop.Text = mathString.ToString();
                if (ValidateMathOp2(this.textboxtop.Text) == RPNType.MathOp2)
                {
                    rpnStack.Push(mathString.ToString());
                    TextCursor++;
                    RpnStackToTextBox();
                    SetMetaContent();
                    textboxtop.Text = string.Empty;
                }
                else
                {
                    this.textboxtop.BorderColor = Color.Red;
                    this.textboxtop.BorderStyle = BorderStyle.Dotted;
                    this.textboxtop.ToolTip = "Math op " + mathString + " requires at least 2 numbers at top of stack";
                    this.textboxRpn.BorderStyle = BorderStyle.Dashed;
                    this.textboxRpn.BorderColor = Color.Red;
                    this.textboxRpn.BorderWidth = 1;
                }
            }
        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            this.rpnStack.Clear();
            SetMetaContent();
            this.textboxtop.Text = "";
            this.textboxRpn.Text = "";
            this.textbox0.Text = "";
        }

        protected void Bdel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textboxtop.Text) && textboxtop.Text.Length > 0)
            {
                this.textboxtop.Text = string.Empty;
            }
            else
            {
                if (rpnStack.Count > 0)
                {
                    rpnStack.Pop();
                    TextCursor--;
                    RpnStackToTextBox();
                    SetMetaContent();
                }
                else this.textboxRpn.Text = string.Empty;
            }

        }

        protected void bChange_Click(object sender, EventArgs e)
        {
            lock (bChange_Click_lock)
            {
                TimeSpan t0 = DateTime.UtcNow.Subtract(this.Change_Click_EventDate);
                if (t0.TotalMilliseconds >= 1024 || t0.Seconds >= 2)
                {
                    string x = t0.Ticks.ToString();
                    if (!string.IsNullOrEmpty(this.textboxtop.Text))
                    {
                        this.bEnter_Click(sender, e);
                    }
                }
            }
        }

        protected void bEnter_Click(object sender, EventArgs e)
        {
            lock (bEnter_Click_lock)
            {
                this.textboxtop.Text = textboxtop.Text.TrimStart(" ".ToArray()).TrimEnd(" ".ToArray());
                if (!string.IsNullOrEmpty(this.textboxtop.Text))
                {
                    if (ValidateNumber(this.textboxtop.Text) == RPNType.Number)
                    {
                        rpnStack.Push(textboxtop.Text.ToString());
                        TextCursor++;
                        RpnStackToTextBox();
                        SetMetaContent();
                        textboxtop.Text = string.Empty;
                        this.Change_Click_EventDate = DateTime.UtcNow;
                    }
                    else
                    {
                        RPNType rpnT = ValidateOperator(this.textboxtop.Text);
                        if (rpnT != RPNType.Invalid)
                        {
                            if (rpnT == RPNType.MathOp1)
                                bMath_Click(sender, e);
                            if (rpnT == RPNType.MathOp2)
                                bMath2Op_Click(sender, e);
                        }
                        else
                        {
                            this.textboxtop.BorderColor = Color.Red;
                            this.textboxtop.BorderStyle = BorderStyle.Dotted;
                            this.textboxtop.ToolTip = "Math op " + this.textboxtop.Text + " unknown";
                        }
                    }
                }
            }
        }

        protected void BEval_Click(object sender, EventArgs e)
        {
            string result = null;
            if (rpnStack.Count >= 2)
            {
                double n0 = double.NaN, n1 = double.NaN;
                string op = rpnStack.Peek();

                if (ValidateOperator(op) != RPNType.Invalid)
                {
                    op = rpnStack.Pop();
                    n0 = NumberFromStack();

                    switch (op)
                    {
                        case "+":
                            n1 = NumberFromStack();
                            result = (n1 + n0).ToString();
                            break;
                        case "-":
                            n1 = NumberFromStack();
                            result = (n1 - n0).ToString();
                            break;
                        case "*":
                        case "×":
                            n1 = NumberFromStack();
                            result = (n1 * n0).ToString();
                            break;
                        case "/":
                        case "÷":
                            n1 = NumberFromStack();
                            result = (n1 / n0).ToString();
                            break;
                        case "^":
                        case "xⁿ":
                            n1 = NumberFromStack();
                            result = (Math.Pow(n1, n0)).ToString();
                            break;
                        case "mod":
                            n1 = NumberFromStack();
                            result = (n1 % n0).ToString();
                            break;
                        case "ⁱ√":
                        case "sqrti":
                            n1 = NumberFromStack();
                            Math.Pow(n0, ((double)(1 / n1))).ToString();
                            break;
                        case "logₕ𝒂":
                        case "log&#x2095;a":
                        case "bloga":
                            n1 = NumberFromStack();
                            result = (Math.Log10(n0) / Math.Log10(n1)).ToString();
                            break;
                        case "!":
                            long fkCnt = 1;
                            for (fkCnt = 1; fkCnt < (n0.ToLong()); fkCnt *= fkCnt++) ;
                            result = fkCnt.ToString();
                            break;
                        case "x²":
                        case "²":
                            result = (n0 * n0).ToString();
                            break;
                        case "x³":
                        case "³":
                            result = (n0 * n0 * n0).ToString();
                            break;
                        case "sin":
                            if (this.CurrentRad == "GRD")
                                n0 = n0 * Math.PI / 200;
                            else if (this.CurrentRad == "DEG")
                                n0 = n0 * Math.PI / 180;
                            result = Math.Sin(n0).ToString();
                            break;
                        case "cos":
                            if (this.CurrentRad == "GRD")
                                n0 = (n0 * Math.PI) / 200;
                            else if (this.CurrentRad == "DEG")
                                n0 = (n0 * Math.PI) / 180;
                            result = Math.Cos(n0).ToString();
                            break;
                        case "tan":
                            if (this.CurrentRad == "GRD")
                                n0 = (n0 * Math.PI) / 200;
                            else if (this.CurrentRad == "DEG")
                                n0 = (n0 * Math.PI) / 180;
                            result = Math.Tan(n0).ToString();
                            break;
                        case "cot":
                            if (this.CurrentRad == "GRD")
                                n0 = (n0 * Math.PI) / 200;
                            else if (this.CurrentRad == "DEG")
                                n0 = (n0 * Math.PI) / 180;
                            result = (1 / Math.Tan(n0)).ToString();
                            break;
                        case "asin":
                            double nAsinResult = Math.Asin(n0);
                            if (this.CurrentRad == "GRD")
                                nAsinResult = (nAsinResult * 200) / Math.PI;
                            if (this.CurrentRad == "DEG")
                                nAsinResult = (nAsinResult * 180) / Math.PI;
                            result = nAsinResult.ToString();
                            break;
                        case "acos":
                            double nAcosResult = Math.Acos(n0);
                            if (this.CurrentRad == "GRD")
                                nAcosResult = (nAcosResult * 200) / Math.PI;
                            if (this.CurrentRad == "DEG")
                                nAcosResult = (nAcosResult * 180) / Math.PI;
                            result = nAcosResult.ToString();
                            break;
                        case "atan":
                            double nAtanResult = Math.Atan(n0);
                            if (this.CurrentRad == "GRD")
                                nAtanResult = (nAtanResult * 200) / Math.PI;
                            if (this.CurrentRad == "DEG")
                                nAtanResult = (nAtanResult * 180) / Math.PI;
                            result = nAtanResult.ToString();
                            break;
                        case "acot":
                            double nAcotResult = (Math.Atan(1 / n0));
                            if (this.CurrentRad == "GRD")
                                nAcotResult = (nAcotResult * 200) / Math.PI;
                            if (this.CurrentRad == "DEG")
                                nAcotResult = (nAcotResult * 180) / Math.PI;
                            result = nAcotResult.ToString();
                            break;
                        case "ln":
                            result = Math.Log(n0).ToString();
                            break;
                        case "log10":
                            result = Math.Log10(n0).ToString();
                            break;
                        case "ld":
                            result = (Math.Log10(n0) / Math.Log10(2)).ToString();
                            break;
                        case "1/x":
                            result = (1 / n0).ToString();
                            break;
                        case "√":
                            result = Math.Sqrt(n0).ToString();
                            break;
                        case "∛":
                            result = Math.Pow(n0, ((double)(1 / 3))).ToString();
                            break;
                        case "∜":
                            result = Math.Pow(n0, ((double)(1 / 4))).ToString();
                            break;
                        case "2ⁿ":
                            result = Math.Pow(2, n0).ToString();
                            break;
                        case "10ⁿ":
                            result = Math.Pow(10, n0).ToString();
                            break;
                        case "|x|":
                            result = Math.Abs(n0).ToString();
                            break;
                        case "%":
                            result = ((double)(n0 / 100)).ToString();
                            break;
                        case "‰":
                            result = ((double)(n0 / 1000)).ToString();
                            break;
                        case "±":
                            result = ((double)(0 - n0)).ToString();
                            break;
                        default:
                            if (!n1.IsNan())
                                rpnStack.Push(n1.ToString());
                            if (!n0.IsNan())
                                rpnStack.Push(n0.ToString());
                            rpnStack.Push(op);
                            break;
                    }
                    if (!string.IsNullOrEmpty(result))
                    {
                        rpnStack.Push(result);
                        this.textbox0.Text = result;
                        RpnStackToTextBox();
                        SetMetaContent();
                        this.textboxtop.Text = string.Empty;
                    }
                }
            }
        }


        #region validate rpn
        protected RPNType ValidateMathOp2(string op)
        {
            RPNType rpnT = RPNType.Invalid;
            string[] rpnArr = rpnStack.ToArray();
            if (rpnArr == null || rpnArr.Length < 2)
                return rpnT;
            if (ValidateNumber(rpnArr[0]) == RPNType.Number
                && ValidateNumber(rpnArr[1]) == RPNType.Number)
            {
                rpnT = ValidateAll(op);
            }
            return rpnT;
        }

        protected RPNType ValidateMathOp1(string op)
        {
            RPNType rpnT = RPNType.Invalid;
            string[] rpnArr = rpnStack.ToArray();
            if (rpnArr == null || rpnArr.Length < 1)
                return rpnT;
            if (ValidateNumber(rpnArr[0]) == RPNType.Number)
                rpnT = ValidateAll(op);
            return rpnT;
        }

        protected RPNType ValidateNumber(string num)
        {
            if (string.IsNullOrEmpty(num))
                return RPNType.Invalid;

            string restnum = num.TrimStart('-');
            if (string.IsNullOrEmpty(restnum))
                return RPNType.Invalid;

            string rest = restnum.Trim("0123456789.,ℇπ,".ToArray());
            return (string.IsNullOrEmpty(rest)) ? RPNType.Number : RPNType.Invalid;
        }

        protected RPNType ValidateOperator(string op)
        {
            RPNType rpnTy = RPNType.Invalid;
            rpnTy = ValidateAll(op);
            switch (rpnTy)
            {
                case RPNType.MathOp1:
                    return rpnTy;
                case RPNType.MathOp2:
                    return rpnTy;
                default:
                    break;
            }
            return RPNType.Invalid;
        }

        protected RPNType ValidateAll(string op)
        {
            RPNType rpnType = RPNType.Invalid;
            switch (op)
            {
                case "+":
                case "-":
                case "*":
                case "×":
                case "/":
                case "÷":
                case "^":
                case "xⁿ":
                case "mod":
                case "ⁱ√":
                case "sqrti":
                case "logₕ𝒂":
                case "log&#x2095;a":
                case "bloga":
                    rpnType = RPNType.MathOp2;
                    break;
                case "±":
                case "x²":
                case "²":
                case "x³":
                case "³":
                case "2ⁿ":
                case "10ⁿ":
                case "!":
                case "ln":
                case "log":
                case "log10":
                case "ld":
                case "1/x":
                case "√":
                case "∛":
                case "∜":
                case "abs":
                case "|x|":
                case "%":
                case "‰":
                case "sin":
                case "cos":
                case "tan":
                case "cot":
                case "asin":
                case "acos":
                case "atan":
                case "acot":
                    rpnType = RPNType.MathOp1;
                    break;
                default:
                    rpnType = ValidateNumber(op);
                    break;
            }
            return rpnType;
        }

        #endregion validate rpn

        #region helper

        protected double NumberFromStack(bool peekOnly = false)
        {
            string n = (peekOnly) ? rpnStack.Peek() : rpnStack.Pop();
            if (n == "ℇ") n = Math.E.ToString();
            if (n == "π") n = Math.PI.ToString();
            if (n == "∞") n = Int64.MaxValue.ToString();
            double d = Double.Parse(n);
            if (d.IsRoundNumber()) {; }
            return d;
        }

        protected void SetMetaContent()
        {
            string jsonSerializeRpnStack = Newtonsoft.Json.JsonConvert.SerializeObject(rpnStack);
            if (metacursor.Attributes["content"] == null)
                metacursor.Attributes.Add("content", HttpUtility.HtmlEncode(jsonSerializeRpnStack));
            else
                metacursor.Attributes["content"] = HttpUtility.HtmlEncode(jsonSerializeRpnStack);

            this.textboxtop.BorderColor = Color.Black;
            this.textboxtop.BorderStyle = BorderStyle.None;
            this.textboxtop.BorderWidth = 0;
            this.textboxtop.ToolTip = "";
            this.textboxRpn.BorderStyle = BorderStyle.None;
            this.textboxRpn.BorderWidth = 0;
            this.textboxRpn.BorderColor = Color.Black;
            string rpnText = string.Empty;
            rpnStack.ToList().ForEach(x => rpnText += x.ToString() + "\n");
            this.textboxRpn.Text = rpnText;
        }

        #endregion helper

        protected void RpnStackToTextBox()
        {
            // this.textboxRpn.Text = string.Empty;
            string rpnText = string.Empty;
            rpnStack.ToList().ForEach(x => rpnText += x.ToString() + "\n");
            textboxRpn.Text = rpnText;
            Session["rpnStack"] = rpnStack;
            this.textboxtop.Focus();
        }

    }
}
