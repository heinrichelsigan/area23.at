using Area23.At.Mono.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;

namespace Area23.At.Mono.Unix
{
    public partial class Bc : System.Web.UI.Page
    {
        char radix = 'n';
        int hexWidth = 8;
        int wordWidth = 32;
        long seekBytes = 0;
        long readBytes = 1024;
        string device = "urandom";
        static Random random;
        string lastLine = null;
        object bcLock = new object();
        const string bcCmdPath = "echo {0} | bc";
        const string bcSuidCmdPath = "/usr/bin/echo {0} | /usr/bin/bc";
        Stack<string> bcStack = new Stack<string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            // if (!Page.IsPostBack)

            InitBcText();
        }

        protected void Button_ResetBc_Click(object sender, EventArgs e)
        {
            InitBcText();
        }

        protected void InitBcText()
        {
            this.bcText.Text = "bc 1.07.1\r\nCopyright 1991-1994, 1997, 1998, 2000, 2004, 2006, 2008, 2012-2017 Free Software Foundation, Inc.\r\nThis is free software with ABSOLUTELY NO WARRANTY.\r\nFor details type `warranty'.\r\n";
            lastLine = null;
            bcStack.Clear();
        }

        protected void BcText_KeyPress(object sender, EventArgs e)
        {
            string sndr = sender.ToString();
            string currentLastLine = GetLastLineFromBcText();
            lock (bcLock)
            {
                if (currentLastLine != lastLine)
                {
                    lastLine = currentLastLine;
                    Perform_BasicCalculator();
                }
            }
        }

        protected string GetLastLineFromBcText()
        {            
            char[] sep = "\r\n".ToCharArray();
            foreach (string bcStr in this.bcText.Text.Split(sep))
            {
                if (!string.IsNullOrEmpty(bcStr) && !bcStack.Contains(bcStr))
                    bcStack.Push(bcStr);
            }
            bcCurrentOp.Text = bcStack.Peek();
            return bcCurrentOp.Text;
        }

        /// <summary>
        /// Perform_Hexdump()
        /// </summary>
        protected void Perform_BasicCalculator()
        {
            try
            {
                string bcCmd = String.Format(bcCmdPath, GetLastLineFromBcText());
                string bcOutPut = Process_Bc(bcSuidCmdPath, "");
                bcText.Text += "\r\n" + bcOutPut + "\r\n";
                preOut.InnerText = "\r\n" + bcOutPut + "\r\n";
            } 
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }
        }

        /// <summary>
        /// Process classical od cmd
        /// </summary>
        /// <param name="filepath">od cmd filepath</param>
        /// <param name="args">od arguments passed to od</param>
        /// <returns>output of od cmd</returns>
        protected string Process_Bc(
            string filepath = bcSuidCmdPath,
            string args = "")
        {
            return ProcessCmd.Execute(filepath, args);
        }



        //protected void SaveSlot_Leave(object sender, EventArgs e)
        //{
        //    if (sender is TextBox myText && myText.ReadOnly == false)
        //    {
        //        myText.ForeColor = SupuColors.Instance.SlotText;
        //        if (String.IsNullOrWhiteSpace(myText.Text))
        //        {
        //            myText.Text = myText.Tag.ToString().Replace(".supu", "");
        //        }
        //    }        
        //}
    }
}