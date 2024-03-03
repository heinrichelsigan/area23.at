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
        int lines = 4;
        static Random random;
        string lastLine = "";
        object bcLock = new object();
        const string bcCmdPath = "echo {0} | bc";
        const string bcSuidCmdPath = "/usr/bin/echo {0} | /usr/bin/bc";
        Stack<string> bcStack = new Stack<string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            // if (!Page.IsPostBack)

            InitBcText();
        }

        /// <summary>
        /// InitBcText - resets <see cref="bcText">bcText</see>.Text to inital string,
        /// sets <see cref="lastLine"/> = null and
        /// clears <see cref="Stack{string}"/> <see cref="bcStack">bcStack</see>
        /// </summary>
        protected void InitBcText()
        {            
            lastLine = "";
            bcStack.Clear();
            // this.bcText.Focus();                                  
        }

        public void BcText_TextChanged(object sender, EventArgs e)
        {
            string sndr = sender.ToString();
            string currentLastLine = GetLastLineFromBcText();
            
            if (currentLastLine != lastLine)
            {
                lock (bcLock)
                {
                    lastLine = currentLastLine;
                    Perform_BasicCalculator();
                }
            }
        }

        /// <summary>
        /// ButtonReset_Click - resets the bcText.Text by calling <see cref="InitBcText"/>
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            InitBcText();
        }

        /// <summary>
        /// ButtonEnter_Click => Event fired, when hidden ButtonEnter is clicked (also clicked by javascript)
        /// Last <see cref="bcText"/> line will be fetched by <see cref="GetLastLineFromBcText()"/> and
        /// if last fetched line != <see cref="lastLine"/> an atomic spinlock 
        ///   sets <see cref="lastline"/> to current last line 
        ///   executes basic calculator line engine by calling <see cref="Perform_BasicCalculator()"/>
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        public void ButtonEnter_Click(object sender, EventArgs e)
        {
            string currentLastLine = GetLastLineFromBcText();
            if (currentLastLine != lastLine)
            {
                lock (bcLock)
                {
                    lastLine = currentLastLine;
                    Perform_BasicCalculator();
                }
            }
        }

        /// <summary>
        /// GetLastLineFromBcText() - fetch last entered line in <see cref="bcText"/> by user
        /// </summary>
        /// <returns></returns>
        protected string GetLastLineFromBcText()
        {            
            char[] sep = "\r\n".ToCharArray();
            string[] bcStrings = this.bcText.Text.Split(sep);
            foreach (string bcStr in bcStrings)
            {
                if (!string.IsNullOrEmpty(bcStr) && !bcStack.Contains(bcStr) &&
                    !bcStr.StartsWith("bc 1.07.1") && !bcStr.ToLower().Contains("free software") && !bcStr.ToLower().Contains("warranty"))
                {
                    bcStack.Push(bcStr);
                }
            }
            bcCurrentOp.Text = (bcStack.Count > 0) ? bcStack.Peek() : "";
            return bcCurrentOp.Text;
        }

        /// <summary>
        /// Perform_BasicCalculator()
        /// </summary>
        /// <param name="bcStr"><see cref="string"/> bcStr passed to bc(1)</param>
        protected void Perform_BasicCalculator(string bcStr = null)
        {
            if (string.IsNullOrEmpty(bcStr))
                bcStr = GetLastLineFromBcText();
            try
            {
                string bcCmd = String.Format(bcSuidCmdPath, bcStr);
                string bcOutPut = Process_Bc(bcCmd, "");
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