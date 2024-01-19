using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rpnStack"] != null)
            {
                rpnStack = (Stack<string>)Session["rpnStack"];
            }
            if (!Page.IsPostBack)
            {
                if (metacursor.Attributes["content"] == null)
                    metacursor.Attributes.Add("content", TextCursor.ToString());
                else
                    metacursor.Attributes["content"] = TextCursor.ToString();
            }
            _textCursor = (metacursor.Attributes["content"] != null) ? Int32.Parse(metacursor.Attributes["content"]) : _textCursor;
        }

        public TextBox CurrentTextBox { get => this.textboxRpn; }

        protected void rpnCalc_Click(object sender, EventArgs e)
        {
            object o1 = sender;
            string eName = e.GetType().Name;

        }

        protected void bBracers_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
        }

        protected void bDummy_Click(object sender, EventArgs e)
        {
            bEnter_Click(sender, e);
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
            
            string rest = num.Trim("0123456789.,".ToArray());
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
                    case "xⁿ":
                    case "÷":
                    case "^":                    
                    case "√":
                    case "∛":
                    case "∜":
                    case "|x|":
                    case "2ⁿ":
                    case "10ⁿ":
                    case "!":
                    case "sin":
                    case "cos":
                    case "tan":
                    case "x²":
                    case "x³":
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

        protected void bNumber_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.textboxtop.Text += mathString.ToString();
        }

        protected void bEnter_Click(object sender, EventArgs e)
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
                }
                else
                {
                    this.textboxtop.BorderColor = Color.Red;
                    this.textboxtop.BorderStyle = BorderStyle.Dashed;
                }
            }

        }

        protected void SetMetaContent()
        {
            this.textboxtop.BorderColor = Color.Black;
            this.textboxtop.BorderStyle = BorderStyle.None;
            this.textboxRpn.BorderStyle = BorderStyle.None;

            if (metacursor.Attributes["content"] == null)
                metacursor.Attributes.Add("content", TextCursor.ToString());
            else
                metacursor.Attributes["content"] = TextCursor.ToString();
        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            this.textboxtop.Text = "";
            this.textboxRpn.Text = "";
            this.textbox0.Text = "";
            this.rpnStack.Clear();
            
        }

        protected void BEval_Click(object sender, EventArgs e)
        {
            string result = null;
            if (rpnStack.Count >= 2)
            {
                string n0 = null, n1 = null, op = rpnStack.Peek();

                if (ValidateOperator(op))
                {
                    op = rpnStack.Pop();
                    n0 = rpnStack.Pop();
                    if (n0 == "ℇ") n0 = Math.E.ToString();
                    if (n0 == "π") n0 = Math.PI.ToString();

                    switch (op)
                    {
                        case "+":
                            n1 = rpnStack.Pop();
                            if (n1 == "ℇ") n1 = Math.E.ToString();
                            if (n1 == "π") n1 = Math.PI.ToString();
                            result = (Double.Parse(n1) + Double.Parse(n0)).ToString();
                            break;
                        case "-":
                            n1 = rpnStack.Pop();
                            if (n1 == "ℇ") n1 = Math.E.ToString();
                            if (n1 == "π") n1 = Math.PI.ToString();
                            result = (Double.Parse(n1) - Double.Parse(n0)).ToString();
                            break;
                        case "*":
                        case "×":
                            n1 = rpnStack.Pop();
                            if (n1 == "ℇ") n1 = Math.E.ToString();
                            if (n1 == "π") n1 = Math.PI.ToString();
                            result = (Double.Parse(n1) * Double.Parse(n0)).ToString();
                            break;
                        case "/":
                        case "÷":
                            n1 = rpnStack.Pop();
                            if (n1 == "ℇ") n1 = Math.E.ToString();
                            if (n1 == "π") n1 = Math.PI.ToString();
                            result = (Double.Parse(n1) / Double.Parse(n0)).ToString();
                            break;
                        case "^":
                            n1 = rpnStack.Pop();
                            if (n1 == "ℇ") n1 = Math.E.ToString();
                            if (n1 == "π") n1 = Math.PI.ToString();
                            result = (Math.Pow(Double.Parse(n0), Double.Parse(n1))).ToString();
                            break;
                        case "!":
                            long fkCnt = 1;
                            for (fkCnt = 1; fkCnt < Int32.Parse(n0); fkCnt *= fkCnt++) ;
                            result = fkCnt.ToString();
                            break;
                        case "x²":
                        case "²":
                            result = (Double.Parse(n0) * Double.Parse(n0)).ToString();
                            break;
                        case "x³":
                        case "³":
                            result = (Double.Parse(n0) * Double.Parse(n0) * Double.Parse(n0)).ToString();
                            break;
                        case "sin":
                            result = Math.Sin(Double.Parse(n0)).ToString();
                            break;
                        case "cos":
                            result = Math.Cos(Double.Parse(n0)).ToString();
                            break;
                        case "tan":
                            result = Math.Tan(Double.Parse(n0)).ToString();
                            break;
                        case "ln":
                            result = Math.Log(Double.Parse(n0)).ToString();
                            break;
                        case "log10":
                            result = Math.Log10(Double.Parse(n0)).ToString();
                            break;
                        case "ld":
                            result = (Math.Log10(Double.Parse(n0)) / Math.Log10(2)).ToString();
                            break;
                        case "1/x":
                            result = (1 / Double.Parse(n0)).ToString();
                            break;
                        case "√":
                            result = Math.Sqrt(Double.Parse(n0)).ToString();
                            break;
                        case "∛":
                            result = Math.Pow(Double.Parse(n0), ((double)(1/3))).ToString();
                            break;
                        case "∜":
                            result = Math.Pow(Double.Parse(n0), ((double)(1 / 4))).ToString();
                            break;
                        case "2ⁿ":
                            result = Math.Pow(2, Double.Parse(n0)).ToString();
                            break;
                        case "10ⁿ":
                            result = Math.Pow(10, Double.Parse(n0)).ToString();
                            break;
                        case "|x|":
                            result = Math.Abs(Double.Parse(n0)).ToString();
                            break;
                        default:
                            if (n1 != null)
                                rpnStack.Push(n1);
                            if (n0 != null)
                                rpnStack.Push(n0);
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

        protected void RpnStackToTextBox()
        {
            this.textboxRpn.Text = string.Empty;
            rpnStack.ToList().ForEach(x => this.textboxRpn.Text += x.ToString() + "\n");
            Session["rpnStack"] = rpnStack;
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
    }
}
