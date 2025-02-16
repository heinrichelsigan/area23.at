using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Ink;
using System.Windows.Media.Animation;

namespace Area23.At.Mono.Calc 
{
    public partial class CCalc : Util.UIPage
    {
        Stack<string> rpnStack = new Stack<string>();
        object _lock = new object();

        private int _textCursor = 0;
        internal int TextCursor
        {
            get => _textCursor;
            set => _textCursor = (value > 0 && value <= 18) ? value : _textCursor;
        }
        public TextBox CurrentTextBox { get => this.TextBox_Top; }

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
                this.CurrentTextBox.Focus();
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
            this.CurrentTextBox.Text += mathString.ToString();
        }

        protected void bPlusMinus_Click(object sender, EventArgs e)
        {
            if (this.CurrentTextBox.Text.StartsWith("-"))
                this.CurrentTextBox.Text = this.CurrentTextBox.Text.TrimStart("-".ToCharArray());
            else
                if (!string.IsNullOrEmpty(this.CurrentTextBox.Text) || 
                    this.CurrentTextBox.Text != "0" || this.CurrentTextBox.Text != "0,0")
                    this.CurrentTextBox.Text = "-" + this.CurrentTextBox.Text;
        }

        protected void bPiE_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.CurrentTextBox.Text = mathString;
            TextCursor++;
            rpnStack.Push(CurrentTextBox.Text);            
            RpnStackToTextBox();
            SetMetaContent();
            this.CurrentTextBox.Text = string.Empty;
        }


        protected void bInfinite_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.CurrentTextBox.Text = mathString;
            TextCursor++;
            rpnStack.Push(CurrentTextBox.Text);            
            RpnStackToTextBox();
            SetMetaContent();
            this.CurrentTextBox.Text = string.Empty;
        }


        protected void bMath_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text :
                (sender is TextBox) ? ((TextBox)sender).Text : "";

            if (mathString == "|x|")
                mathString = "abs";

            if (!string.IsNullOrEmpty(mathString))
            {
                this.CurrentTextBox.Text = mathString.ToString();

                string newElem = this.CurrentTextBox.Text;
                string newTerm = this.TextBox_Calc.Text + newElem;
                CalcTerm term = null;
                bool validated = true;
                string unvalidateReasion = ".";

                if (ValidateMathOp1(mathString) == RPNType.MathOp1)
                {
                    try
                    {                        
                        term = new CalcTerm(newTerm);
                        validated = true;
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogStatic(ex);
                        unvalidateReasion = ": " + ex.Message;
                        validated = false;
                    }

                    if (validated)
                    {
                        rpnStack.Push(mathString.ToString());
                        TextCursor++;
                        RpnStackToTextBox();
                        SetMetaContent();
                        CurrentTextBox.Text = string.Empty;
                        this.Change_Click_EventDate = DateTime.UtcNow;
                        return;
                    }
                }
                
                if (!validated)
                {
                    this.CurrentTextBox.BorderColor = Color.Red;
                    this.CurrentTextBox.BorderStyle = BorderStyle.Dashed;
                    this.CurrentTextBox.BorderWidth = 1;
                    this.CurrentTextBox.ToolTip = "Unary math op " + this.CurrentTextBox.Text + " invalid: " + unvalidateReasion;
                }
            }
        }

        protected void bMath2Op_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text :
                (sender is TextBox) ? ((TextBox)sender).Text : "";
            
            if (mathString == "log&#x2095;a")
                mathString = "bloga";


            if (!string.IsNullOrEmpty(mathString))
            {
                this.CurrentTextBox.Text = mathString.ToString();

                string newElem = this.CurrentTextBox.Text;
                string newTerm = this.TextBox_Calc.Text + newElem;
                CalcTerm term = null;
                bool validated = true;
                string unvalidateReasion = ".";

                if (ValidateMathOp2(this.CurrentTextBox.Text) == RPNType.MathOp2)
                {
                    try
                    {
                        term = new CalcTerm(newTerm);
                        validated = true;
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogStatic(ex);
                        unvalidateReasion = ": " + ex.Message;
                        validated = false;
                    }

                    if (validated)
                    {
                        rpnStack.Push(mathString.ToString());
                        TextCursor++;
                        RpnStackToTextBox();
                        SetMetaContent();
                        CurrentTextBox.Text = string.Empty;
                        return;
                    }
                }
                if (!validated)
                {
                    this.CurrentTextBox.BorderColor = Color.Red;
                    this.CurrentTextBox.BorderStyle = BorderStyle.Dotted;
                    this.CurrentTextBox.ToolTip = "Binary math op " + mathString + " is invalid in this context" + unvalidateReasion;
                    this.CurrentTextBox.BorderStyle = BorderStyle.Dashed;
                    this.CurrentTextBox.BorderColor = Color.Red;
                    this.CurrentTextBox.BorderWidth = 1;
                }
            }
        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            this.rpnStack.Clear();
            TextCursor = 0;
            SetMetaContent();
            this.CurrentTextBox.Text = "";
            this.TextBox_Top.Text = "";
            this.TextBox_Calc.Text = "";
        }

        protected void Bdel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.CurrentTextBox.Text) && CurrentTextBox.Text.Length > 0)
            {
                this.CurrentTextBox.Text = string.Empty;
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
                else
                {
                    this.CurrentTextBox.Text = string.Empty;
                    this.TextBox_Calc.Text = string.Empty;
                }
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
                    if (!string.IsNullOrEmpty(this.CurrentTextBox.Text))
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
                this.CurrentTextBox.Text = CurrentTextBox.Text.TrimStart(" ".ToArray()).TrimEnd(" ".ToArray());
                if (!string.IsNullOrEmpty(this.CurrentTextBox.Text))
                {
                    string newElem = this.CurrentTextBox.Text;
                    string newTerm = this.TextBox_Calc.Text + newElem;
                    CalcTerm term = null;

                    if (ValidateAll(newElem) != RPNType.Invalid)
                    {
                        try
                        {
                            term = new CalcTerm(newTerm);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                            this.CurrentTextBox.BorderColor = Color.Red;
                            this.CurrentTextBox.BorderStyle = BorderStyle.Dotted;
                            this.CurrentTextBox.ToolTip = "Math op " + this.CurrentTextBox.Text + " invalid: " +ex.Message;
                            return;
                        }

                        if (rpnStack != null && rpnStack.Count > 0 && rpnStack.Peek().Equals(CurrentTextBox.Text.ToString()))
                        {
                            ; // work arround do nothing
                            RpnStackToTextBox();
                            SetMetaContent();
                            CurrentTextBox.Text = string.Empty;
                            this.Change_Click_EventDate = DateTime.UtcNow;
                        }
                        else
                        {
                            rpnStack.Push(CurrentTextBox.Text.ToString());
                            TextCursor++;
                            RpnStackToTextBox();
                            SetMetaContent();
                            CurrentTextBox.Text = string.Empty;
                            this.Change_Click_EventDate = DateTime.UtcNow;
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
                string op = rpnStack.Reverse().First();
                string stackTermStr = string.Empty;
                CalcTerm stackTerm = null, textTerm = null;

                if ((ValidateOperator(op) == RPNType.MathOp1) || (ValidateAll(op) == RPNType.Number))
                {
                    while (rpnStack.Count > 1)
                    {
                        stackTermStr = rpnStack.ReverseToString<string>();
                        stackTerm = new CalcTerm(stackTermStr);
                        textTerm = new CalcTerm(TextBox_Calc.Text);
                        stackTerm.EvaluateTerms(_currentRad);
                        this.TextBox_Calc.Text = stackTerm.ToString();
                        rpnStack.Clear();
                        TextCursor = 0;
                        foreach (var selem in stackTerm.sterms)
                            rpnStack.Push(selem.Elem);

                    }

                    RpnStackToTextBox();
                    SetMetaContent();

                    if (rpnStack.Count == 1)
                    {
                        this.TextBox_Calc.BorderColor = Color.Green;
                        this.TextBox_Calc.BorderStyle = BorderStyle.Groove;
                        this.TextBox_Calc.BorderWidth = 1;
                        this.TextBox_Calc.ToolTip = "Result is " + result;
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
            if (d.IsRoundNumber()) { ; }
            return d;
        }

        protected void SetMetaContent()
        {
            string jsonSerializeRpnStack = Newtonsoft.Json.JsonConvert.SerializeObject(rpnStack);
            if (metacursor.Attributes["content"] == null)
                metacursor.Attributes.Add("content", HttpUtility.HtmlEncode(jsonSerializeRpnStack));
            else
                metacursor.Attributes["content"] = HttpUtility.HtmlEncode(jsonSerializeRpnStack);

            this.CurrentTextBox.BorderColor = Color.Black;
            this.CurrentTextBox.BorderStyle = BorderStyle.None;
            this.CurrentTextBox.BorderWidth = 0;
            this.CurrentTextBox.ToolTip = "";
            this.TextBox_Calc.BorderStyle = BorderStyle.None;
            this.TextBox_Calc.BorderWidth = 0;
            this.TextBox_Calc.BorderColor = Color.Black;
            string rpnText = rpnStack.ReverseToString<string>();
            lock (_lock)
            {
                this.TextBox_Calc.ReadOnly = false;
                this.TextBox_Calc.Text = rpnText;
            }
            this.TextBox_Calc.ReadOnly = true;
        }

        #endregion helper

        protected void RpnStackToTextBox()
        {
            // this.textboxRpn.Text = string.Empty;
            string rpnText = rpnStack.ReverseToString<string>();
            lock (_lock)
            {
                this.TextBox_Calc.ReadOnly = false;
                this.TextBox_Calc.Text = rpnText;
            }
            Session["rpnStack"] = rpnStack;
            this.TextBox_Calc.ReadOnly = true;
            this.CurrentTextBox.Focus();
        }

    }
}
