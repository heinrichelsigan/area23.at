using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono
{
    public partial class RpnCalc : RpnBasePage
    {
        Stack<string> rpnStack = new Stack<string>();

        private int _textCursor = 0;
        internal int TextCursor
        {
            get => _textCursor;
            set => _textCursor = (value > 0 && value <= 18) ? value : _textCursor;
        }
        public TextBox CurrentTextBox { get => this.textboxRpn; }

        public string Change_Click_EventCnt
        {
            get => (Session[Constants.CHANGE_CLICK_EVENTCNT] != null) ? 
                (string)Session[Constants.CHANGE_CLICK_EVENTCNT] : string.Empty;
            set => Session[Constants.CHANGE_CLICK_EVENTCNT] = value;
        }
        object bChange_Click_lock = new object();
        object bEnter_Click_lock = new object();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constants.RPN_STACK] != null)
            {
                rpnStack = (Stack<string>)Session[Constants.RPN_STACK];
            }            
            else
            {
                rpnStack = new Stack<string>();
                if (metacursor.Attributes["content"] != null)
                {
                    string s = HttpUtility.HtmlDecode(metacursor.Attributes["content"].ToString());
                }
                
                Session[Constants.RPN_STACK] = rpnStack;
            }
            if (!Page.IsPostBack)
            {
                if (metacursor.Attributes["content"] == null)
                    metacursor.Attributes.Add("content", HttpUtility.HtmlEncode(rpnStack.ToArray().ToString()));
                else
                    metacursor.Attributes["content"] = HttpUtility.HtmlEncode(rpnStack.ToArray().ToString());
            }
            _textCursor = rpnStack.Count;
        }
               
         
        [Obsolete("rpnCalc_Click is obseolte", false)]
        protected void rpnCalc_Click(object sender, EventArgs e)
        {
            object o1 = sender;
            string eName = e.GetType().Name;

        }

        [Obsolete("bDummy_Click is obseolte", false)]
        protected void bDummy_Click(object sender, EventArgs e)
        {
            bEnter_Click(sender, e);
        }


        protected void bBracers_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
        }
        
        protected void bRad_Click(object sender, EventArgs e)
        {
            //TODO: implement it
        }

        protected void bArc_Click(object sender, EventArgs e)
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

        protected void bMath_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            if (!string.IsNullOrEmpty(mathString))
            {
                this.textboxtop.Text = mathString.ToString();
                if (ValidateMath(this.textboxtop.Text))
                {
                    rpnStack.Push(mathString.ToString());
                    TextCursor++;
                    RpnStackToTextBox();
                    SetMetaContent();
                }
                else
                {
                    this.textboxtop.BorderColor = Color.Red;
                    this.textboxRpn.BorderStyle = BorderStyle.Dotted;
                }
            }
        }
        
        protected void bMath2Op_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            if (!string.IsNullOrEmpty(mathString))
            {
                this.textboxtop.Text = mathString.ToString();
                if (ValidateMath2Op(this.textboxtop.Text))
                {
                    rpnStack.Push(mathString.ToString());
                    TextCursor++;
                    RpnStackToTextBox();
                    SetMetaContent();
                }
                else
                {
                    this.textboxtop.BorderColor = Color.Red;
                    this.textboxRpn.BorderStyle = BorderStyle.Dotted;
                }
            }
        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            this.textboxtop.Text = "";
            this.textboxRpn.Text = "";
            this.textbox0.Text = "";
            this.rpnStack.Clear();
            
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
                bEnter_Click_lock = new object();
                if (!string.IsNullOrEmpty(this.textboxtop.Text))
                {
                    this.bEnter_Click(sender, e);
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
                    if (ValidateNumber(this.textboxtop.Text))
                    {
                        TextCursor++;
                        rpnStack.Push(textboxtop.Text);
                        this.textboxtop.Text = string.Empty;
                        RpnStackToTextBox();
                        SetMetaContent();
                        this.Change_Click_EventCnt = rpnStack.Peek();
                    }
                    else
                    {
                        this.textboxtop.BorderColor = Color.Red;
                        this.textboxtop.BorderStyle = BorderStyle.Dashed;
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

                if (ValidateOperator(op))
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
                            result = Math.Sin(n0).ToString();
                            break;
                        case "cos":
                            result = Math.Cos(n0).ToString();
                            break;
                        case "tan":
                            result = Math.Tan(n0).ToString();
                            break;
                        case "cot":
                            result = (Math.Cos(n0) / Math.Sin(n0)).ToString();
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
                            result = Math.Pow(n0, ((double)(1/3))).ToString();
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
        protected bool ValidateMath2Op(string op)
        {
            string[] rpnArr = rpnStack.ToArray();
            if (rpnArr == null || rpnArr.Length < 2)
                return false;
            return (ValidateNumber(rpnArr[rpnArr.Length - 1]) && ValidateNumber(rpnArr[rpnArr.Length - 2]));
        }
        protected bool ValidateMath(string op)
        {
            string[] rpnArr = rpnStack.ToArray();
            return (ValidateNumber(rpnArr[rpnArr.Length - 1]));
        }
        protected bool ValidateNumber(string num)
        {
            if (string.IsNullOrEmpty(num))
                return false;

            string restnum = num.TrimStart('-');
            if (string.IsNullOrEmpty(restnum))
                return false;
            
            string rest = restnum.Trim("0123456789.,ℇπ,".ToArray());
            return (string.IsNullOrEmpty(rest));
        }
        protected bool ValidateOperator(string op)
        {
            if (!string.IsNullOrEmpty(op))
            {
                switch (op)
                {
                    case "*":
                    case "×":
                    case "+":
                    case "-":
                    case "/":
                    case "÷":
                    case "xⁿ":
                    case "^":                                        
                    case "√":
                    case "∛":
                    case "∜":
                    case "‰":
                    case "%":
                    case "|x|":
                    case "2ⁿ":
                    case "10ⁿ":
                    case "!":
                    case "sin":
                    case "cos":
                    case "tan":
                    case "cot":
                    case "x²":
                    case "²":
                    case "x³":
                    case "³":
                    case "1/x":
                    case "log":
                    case "ln":
                    case "ld":
                        return true;
                    default: return false;
                }
            }
            return false;
        }

        #endregion validate rpn

        #region helper

        protected double NumberFromStack(bool peekOnly = false)
        {
            string n = (peekOnly) ? rpnStack.Peek() : rpnStack.Pop();
            if (n == "ℇ") n = Math.E.ToString();
            if (n == "π") n = Math.PI.ToString();
            double d = Double.Parse(n);
            if (d.IsRoundNumber()) ;
            return d;
        }

        protected void SetMetaContent()
        {
            this.textboxtop.BorderColor = Color.Black;
            this.textboxtop.BorderStyle = BorderStyle.None;
            this.textboxRpn.BorderStyle = BorderStyle.None;

            if (metacursor.Attributes["content"] == null)
                metacursor.Attributes.Add("content", HttpUtility.HtmlEncode(rpnStack.ToArray().ToString()));
            else
                metacursor.Attributes["content"] = HttpUtility.HtmlEncode(rpnStack.ToArray().ToString());
        }

        #endregion helper

        protected void RpnStackToTextBox()
        {
            this.textboxRpn.Text = string.Empty;
            rpnStack.ToList().ForEach(x => this.textboxRpn.Text += x.ToString() + "\n");
            Session["rpnStack"] = rpnStack;
            this.textboxtop.Focus();
        }
        


    }
}
