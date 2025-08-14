using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Area23.At.Mono.Calc 
{
    public partial class MatrixVCalc : Util.UIPage
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
        }


        protected void Button_RandomSetVA_Click(object sender, EventArgs e)
        {
            Random rand = new Random((DateTime.Now.Second + 1) * (DateTime.Now.Millisecond + 1));
            for (int col = 0; col < 16; col++)
            {
                Control destCtrl = null;
                if (((destCtrl = MatrixCalcForm.FindControl($"TextBox_{col:x1}_v0")) != null) && destCtrl is TextBox v0TextBox)
                {
                    int v0val = rand.Next(16);
                    v0TextBox.Text = $"{v0val:x1}";
                }
            }
        }

        protected void Button_ResetVA_Click(object sender, EventArgs e)
        {
            for (int col = 0; col < 16; col++)
            {
                Control v0Ctrl = null;
                if (((v0Ctrl = MatrixCalcForm.FindControl($"TextBox_{col:x1}_v0")) != null) && v0Ctrl is TextBox v0TextBox)
                {
                    v0TextBox.Text = "1";
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
                    if (((m1Ctrl = MatrixCalcForm.FindControl($"TextBox_{row:x1}_{col:x1}")) != null) && m1Ctrl is TextBox m1TextBox)
                    {                        
                        int m1val = rand.Next(16);
                        m1TextBox.Text = $"{m1val:x1}";
                    }
                }
            }
        }

        protected void Button_ResetMB_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 16; col++)
                {
                    Control m1Ctrl = null;
                    if (((m1Ctrl = MatrixCalcForm.FindControl($"TextBox_{row:x1}_{col:x1}")) != null) && m1Ctrl is TextBox m1TextBox)
                        m1TextBox.Text = (col == (0xf - row)) ? "1" : "0";                    
                }
            }
        }
        


        protected void Button_VectorAxMatrixB_Click(object sender, EventArgs e)
        {
            for (int rw = 0; rw < 16; rw++)
            {
                int row = 15 - rw;
                for (int col = 0; col < 1; col++)
                {
                    Control destCtrl = null;
                    if (((destCtrl = MatrixCalcForm.FindControl($"TextBox_{row:x1}_vf")) != null) && destCtrl is TextBox destTextBox)
                    {
                        long m0scaV = 0, m1scaV = 0, m2scaR = 0;
                        for (int crossVP = 0; crossVP < 16; crossVP++)
                        {
                            int crossRow = 0xf - crossVP;
                            Control m0Vector = null, m1Vector = null;
                            IFormatProvider provider = new NumberFormatInfo();


                            if (((m0Vector = MatrixCalcForm.FindControl($"TextBox_{row:x1}_v0")) != null) && m0Vector is TextBox m0TextBox)
                            {                               
                                if (!Int64.TryParse(m0TextBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out m0scaV))
                                    m0scaV = 0;
                            }
                            if (((m1Vector = MatrixCalcForm.FindControl($"TextBox_{crossRow:x1}_{col:x1}")) != null) && m1Vector is TextBox m1TextBox)
                            {
                                if (!Int64.TryParse(m1TextBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out m1scaV))
                                    m1scaV = 0;
                            }
                            m2scaR += (m0scaV * m1scaV);
                        }

                        destTextBox.BackColor = (m2scaR == 0) ? ColorFrom.FromHtml("#fafada") : Color.White;
                        destTextBox.BorderWidth = (m2scaR == 0) ? 0 : 1;
                        destTextBox.Text = m2scaR.ToString("x");
                    }
                }
                // $"TextBox_m2_{row:x1}_{col:x1}"  TextBox_m0_0_0
            }
        }


        protected void Button_ResetVectorC_Click(object sender, EventArgs e) 
        {
            for (int col = 0; col < 16; col++)
            {
                Control vfCtrl = null;
                if (((vfCtrl = MatrixCalcForm.FindControl($"TextBox_{col:x1}_vf")) != null) && vfCtrl is TextBox vfTextBox)
                {
                    vfTextBox.Text = "0";
                }
            }
        }

    }
}
