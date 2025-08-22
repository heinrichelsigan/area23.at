using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Calc 
{
    /// <summary>
    /// <see href="https://area23.at/mono/app.publish/Calc/MatrixSO.aspx" />
    /// <see href="https://math.stackexchange.com/questions/2201193/creating-link-matrix#comment10956997_2201193" />
    /// <see href="https://pastebin.com/aeh8yAxa" />
    /// </summary>
    public partial class MatrixSO : Util.UIPage
    {
        int[,] MatrixA = new int[16,16];
        int[,] MatrixB = new int[16,16];

        private static readonly object _lock0 = new object(), _lock1 = new object(), _lock2 = new object();

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                Button_ResetMB_0_Click(sender, e);
            }
        }

        protected void Button_ResetMA_0_Click(object sender, EventArgs e)
        {
            ResetMatrixA();
        }

        protected void Button_ResetMB_0_Click(object sender, EventArgs e) 
        {
            ResetMatrixB();
        }

        protected void Button_RandomSetMA_Click(object sender, EventArgs e)
        {
            MatrixA = GetRandomMatrix();
            for (int row = 0; row < 16; row++)
            {
                int rw = 15 - row;
                for (int col = 0; col < 16; col++)
                {
                    Control destCtrl = null;
                    if (((destCtrl = MatrixSOForm.FindControl($"TextBox_m0_{rw:x1}_{col:x1}")) != null) && destCtrl is TextBox m0TextBox)
                        m0TextBox.Text = $"{MatrixA[row, col]}";
                }
            }
        }

        protected void Button_Linked_Algorithm_Click(object sender, EventArgs e)
        {
            LinkAlgorithm();
        }

        protected void ResetMatrixA()
        {
            lock (_lock0)
            {
                for (int row = 0; row < 16; row++)
                {
                    int rw = 15 - row;
                    for (int col = 0; col < 16; col++)
                    {
                        Control m0Ctrl = null;
                        if (((m0Ctrl = MatrixSOForm.FindControl($"TextBox_m0_{rw:x1}_{col:x1}")) != null) && m0Ctrl is TextBox m0TextBox)
                            m0TextBox.Text = "0";
                        MatrixA[row, col] = 0;
                    }
                }
            }
        }

        protected void ResetMatrixB()
        {
            lock (_lock1)
            {
                for (int row = 0; row < 16; row++)
                {
                    int rw = 15 - row;
                    for (int col = 0; col < 16; col++)
                    {
                        Control m1Ctrl = null;
                        if (((m1Ctrl = MatrixSOForm.FindControl($"TextBox_m1_{rw:x1}_{col:x1}")) != null) && m1Ctrl is TextBox m1TextBox)
                            m1TextBox.Text = "0";
                        MatrixB[row, col] = 0;
                    }
                }
            }
        }

        protected int[,] GetMatrixA()
        {
            lock (_lock0)
            {
                for (int row = 0; row < 16; row++)
                {
                    int rw = 15 - row;
                    for (int col = 0; col < 16; col++)
                    {
                        Control sourceCtrl = null;
                        if (((sourceCtrl = MatrixSOForm.FindControl($"TextBox_m0_{rw:x1}_{col:x1}")) != null) && sourceCtrl is TextBox srcTextBox)
                        {
                            MatrixA[row, col] = (srcTextBox.Text == "1") ? 1 : 0;
                        }
                    }
                }
            }
            return MatrixA;
        }

        protected void SetMatrixB(int[,] mb)
        {
            if (mb == null)
                mb = MatrixB;
            lock (_lock1)
            {
                for (int row = 0; row < 16; row++)
                {
                    int rw = 15 - row;
                    for (int col = 0; col < 16; col++)
                    {
                        Control destCtrl = null;
                        if (((destCtrl = MatrixSOForm.FindControl($"TextBox_m1_{rw:x1}_{col:x1}")) != null) && destCtrl is TextBox destTextBox)
                            destTextBox.Text = (mb[row, col]).ToString();
                    }
                }
            }
            return;
        }

        protected int[,] GetRandomMatrix()
        {
            int[,] matrix = new int[16, 16];
            lock (_lock2)
            {
                Random randMatrixA = new Random((DateTime.Now.Second + 1) * (DateTime.Now.Millisecond + 1));
                for (int row = 0; row < 16; row++)
                {
                    for (int col = 0; col < 16; col++)
                    {
                        int l0val = randMatrixA.Next(16);
                        matrix[row, col] = (l0val % 4 == 0) ? 1 : 0;
                    }
                }
            }

            return matrix;
        }

        protected void LinkAlgorithm()
        {
            lock (_lock2)
            {
                MatrixA = GetMatrixA();

                for (int row = 0; row < 16; row++)
                {
                    List<int> linked = new List<int>();
                    for (int col = 0; col < 16; col++)
                    {
                        if (MatrixA[row, col] == 1)
                        {
                            MatrixB[row, col] = 1;
                            linked.Add(col);
                        }

                    }
                    foreach (int link in linked)
                    {
                        for (int col = 0; col < 16; col++)
                        {
                            if (MatrixA[link, col] == 1)
                                MatrixB[row, col] = 1;
                        }
                    }
                }

                SetMatrixB(MatrixB);
            }
        }

    }

}
