using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Calc 
{
    public partial class MatrixMCalc : Util.UIPage
    {
        Stack<string> rpnStack = new Stack<string>();

        private int _textCursor = 0;
        internal int TextCursor
        {
            get => _textCursor;
            set => _textCursor = (value > 0 && value <= 18) ? value : _textCursor;
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
            
            _textCursor = rpnStack.Count;
            if (!IsPostBack)
            {
                Button_ResetMC_Click(sender, e);
            }
        }

        protected void Button_RandomSetMA_Click(object sender, EventArgs e)
        {
            Random randMatrixA = new Random((DateTime.Now.Second + 1) * (DateTime.Now.Millisecond + 1));

            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control destCtrl = null;
                    if (((destCtrl = MatrixMCalcForm.FindControl($"TextBox_m0_{row:x1}_{col:x1}")) != null) && destCtrl is TextBox m0TextBox)
                    {                        
                        long l0val = randMatrixA.Next(16);
                        if (CheckBox_Hex.Checked)
                            m0TextBox.Text = $"{l0val:x1}";
                        else
                        {
                            double d0val = l0val + randMatrixA.NextDouble();
                            m0TextBox.Text = $"{d0val}"; ;
                        }
                    }
                }
            }
        }

        protected void Button_ResetMA_0_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m0Ctrl = null;
                    if (((m0Ctrl = MatrixMCalcForm.FindControl($"TextBox_m0_{row:x1}_{col:x1}")) != null) && m0Ctrl is TextBox m0TextBox)
                    {
                        m0TextBox.Text = "0";
                    }
                }
            }
        }

        protected void Button_ResetMA_1_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m0Ctrl = null;
                    if (((m0Ctrl = MatrixMCalcForm.FindControl($"TextBox_m0_{row:x1}_{col:x1}")) != null) && m0Ctrl is TextBox m0TextBox)
                    {
                        m0TextBox.Text = (col == (0xf - row)) ? "1" : "0";
                    }
                }
            }
        }

        protected void Button_RandomSetMB_Click(object sender, EventArgs e)
        {
            Random rand = new Random((DateTime.Now.Second + 1) * (DateTime.Now.Millisecond + 1));

            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m1Ctrl = null;
                    if (((m1Ctrl = MatrixMCalcForm.FindControl($"TextBox_m1_{row:x1}_{col:x1}")) != null) && m1Ctrl is TextBox m1TextBox)
                    {                       
                        long l1val = rand.Next(16);
                        if (CheckBox_Hex.Checked)
                            m1TextBox.Text = $"{l1val:x1}";
                        else
                        {
                            double d1val = l1val + rand.NextDouble();
                            m1TextBox.Text = $"{d1val}"; 
                        }
                    }
                }
            }
        }


        protected void Button_ResetMC_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m2Ctrl = null;
                    if (((m2Ctrl = MatrixMCalcForm.FindControl($"TextBox_m2_{row:x1}_{col:x1}")) != null) && m2Ctrl is TextBox m2TextBox)
                    {
                        m2TextBox.Text = "0";
                        m2TextBox.Width = 0;
                        m2TextBox.BackColor = (CheckBox_Hex.Checked) ? ColorFrom.FromHtml("#fefefe") : ColorFrom.FromHtml("#dedede");
                    }
                }
            }
        }

        protected void Button_ResetMB_0_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m1Ctrl = null;
                    if (((m1Ctrl = MatrixMCalcForm.FindControl($"TextBox_m1_{row:x1}_{col:x1}")) != null) && m1Ctrl is TextBox m1TextBox)

                        m1TextBox.Text = (col == row) ? "1" : "0";
                }
            }
        }

        protected void Button_ResetMB_1_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m1Ctrl = null;
                    if (((m1Ctrl = MatrixMCalcForm.FindControl($"TextBox_m1_{row:x1}_{col:x1}")) != null) && m1Ctrl is TextBox m1TextBox)

                        m1TextBox.Text = (col == (0xf - row)) ? "1" : "0";
                }
            }
        }


        protected void Button_MatrixAxB_Click(object sender, EventArgs e)
        {
            IFormatProvider provider = new NumberFormatInfo();
            for (int rw = 0; rw < 16; rw++)
            {
                int row = 15 - rw;
                for (int col = 0; col < 16; col++)
                {
                    
                    Control destCtrl = null;                    
                    if (((destCtrl = MatrixMCalcForm.FindControl($"TextBox_m2_{row:x1}_{col:x1}")) != null) && destCtrl is TextBox destTextBox)
                    {
                        long l0scaV = 0, l1scaV = 0, l2scaR = 0;
                        double m0scaV = 0, m1scaV = 0, m2scaR = 0;
                        for (int crossVP = 0; crossVP < 16; crossVP++)
                        {
                            int crossRow = 0xf - crossVP;
                            Control m0Vector = null, m1Vector = null;
                            if (((m0Vector = MatrixMCalcForm.FindControl($"TextBox_m0_{row:x1}_{crossVP:x1}")) != null) && m0Vector is TextBox m0TextBox)
                            {
                                if (CheckBox_Hex.Checked)
                                {
                                    if (!Int64.TryParse(m0TextBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out l0scaV))
                                        l0scaV = 0;
                                }
                                else
                                {
                                    if (!Double.TryParse(m0TextBox.Text, out m0scaV))
                                        m0scaV = 0;
                                }
                            }
                            if (((m1Vector = MatrixMCalcForm.FindControl($"TextBox_m1_{crossRow:x1}_{col:x1}")) != null) && m1Vector is TextBox m1TextBox)
                            {
                                if (CheckBox_Hex.Checked)
                                {
                                    if (!Int64.TryParse(m1TextBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out l1scaV))
                                        l1scaV = 0;
                                }
                                else
                                {
                                    if (!Double.TryParse(m1TextBox.Text, out m1scaV))
                                        m1scaV = 0;
                                }
                            }
                            if (CheckBox_Hex.Checked)
                                l2scaR += (l0scaV * l1scaV);
                            else 
                                m2scaR += (m0scaV * m1scaV);
                        }

                        if (CheckBox_Hex.Checked)
                        {
                            destTextBox.BackColor = (l2scaR == 0) ? ColorFrom.FromHtml("#fafafa") : Color.White;
                            destTextBox.BorderWidth = (l2scaR == 0) ? 0 : 1;
                        }
                        else
                        {
                            destTextBox.BackColor = (m2scaR == 0) ? ColorFrom.FromHtml("#dadada") : Color.White;
                            destTextBox.BorderWidth = (m2scaR == 0) ? 0 : 1;
                        }
                        destTextBox.Text = (CheckBox_Hex.Checked) ? l2scaR.ToString("x") : m2scaR.ToString();
                    }
                }
                // $"TextBox_m2_{row:x1}_{col:x1}"  TextBox_m0_0_0
            }
        }


        protected void Button_MatrixBxA_Click(object sender, EventArgs e)
        {
            IFormatProvider provider = new NumberFormatInfo();
            for (int rw = 0; rw < 16; rw++)
            {
                int row = 15 - rw;
                for (int col = 0; col < 16; col++)
                {
                    Control destCtrl = null;
                    if (((destCtrl = MatrixMCalcForm.FindControl($"TextBox_m2_{row:x1}_{col:x1}")) != null) && destCtrl is TextBox destTextBox)
                    {
                        double m0scaV = 0, m1scaV = 0, m2scaR = 0;
                        long l0scaV = 0, l1scaV = 0, l2scaR = 0;
                        for (int crossVP = 0; crossVP < 16; crossVP++)
                        {
                            int crossRow = 0xf - crossVP;
                            Control m0Vector = null, m1Vector = null;
                            if (((m0Vector = MatrixMCalcForm.FindControl($"TextBox_m1_{row:x1}_{crossVP:x1}")) != null) && m0Vector is TextBox m0TextBox)
                            {
                                if (CheckBox_Hex.Checked)
                                {
                                    if (!Int64.TryParse(m0TextBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out l0scaV))
                                        l0scaV = 0;
                                }
                                else
                                {
                                    if (!Double.TryParse(m0TextBox.Text, out m0scaV))
                                        m0scaV = 0;
                                }                                
                            }
                            if (((m1Vector = MatrixMCalcForm.FindControl($"TextBox_m0_{crossRow:x1}_{col:x1}")) != null) && m1Vector is TextBox m1TextBox)
                            {
                                if (CheckBox_Hex.Checked)
                                {
                                    if (!Int64.TryParse(m1TextBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out l1scaV))
                                        l1scaV = 0;
                                }
                                else
                                {
                                    if (!Double.TryParse(m1TextBox.Text, out m1scaV))
                                        m1scaV = 0;
                                }
                            }
                            if (CheckBox_Hex.Checked)
                                l2scaR += (l0scaV * l1scaV);
                            else
                                m2scaR += (m0scaV * m1scaV);
                        }
                        if (CheckBox_Hex.Checked)
                        {
                            destTextBox.BackColor = (l2scaR == 0) ? ColorFrom.FromHtml("#fafafa") : Color.White;
                            destTextBox.BorderWidth = (l2scaR == 0) ? 0 : 1;
                        }
                        else
                        {
                            destTextBox.BackColor = (m2scaR == 0) ? ColorFrom.FromHtml("#dadada") : Color.White;
                            destTextBox.BorderWidth = (m2scaR == 0) ? 0 : 1;
                        }


                        destTextBox.Text = (CheckBox_Hex.Checked) ? l2scaR.ToString("x") : m2scaR.ToString(); 
                    }
                }
                // $"TextBox_m2_{row:x1}_{col:x1}"  TextBox_m0_0_0
            }
        }

    }
}
