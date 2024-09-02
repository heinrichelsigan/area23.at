using Area23.At.Framework.Library;
using System;
using System.Collections.Generic;
using System.IO;

namespace Area23.At.Mono.Unix
{
    public partial class Bc : System.Web.UI.Page
    {
        // int lines = 4;
        // static Random random;
        string lastLine = "";
        object bcLock = new object();
        private static readonly bool USE_UNIX = (Path.DirectorySeparatorChar == '/') ? true : false;
        private readonly string BC_CMD_PATH = (USE_UNIX) ? "/usr/local/bin/bccmd.sh" : LibPaths.AdditionalBinDir + "bccmd.bat";
        const string BC_CMD = "bc";             
        Stack<string> bcStack = new Stack<string>();

        public DateTime Change_Click_EventDate
        {
            get => (Session[Constants.CHANGE_CLICK_EVENTCNT] != null) ?
                (DateTime)Session[Constants.CHANGE_CLICK_EVENTCNT] : DateTime.MinValue;
            set => Session[Constants.CHANGE_CLICK_EVENTCNT] = value;
        }

        /// <summary>
        /// Page_Load event triggered in normal lifecycle of asp.net classic page OnLoad
        /// </summary>
        /// <param name="sender"><see cref="object">object sender</see></param>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
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
        protected void InitBcText(bool fullReset = false)
        {            
            lastLine = "";
            bcStack.Clear();
            if (fullReset)
            {
                this.TextBox_BcOut.Text = Constants.BC_START_MSG;
            }
        }


        public void TextBox_BcOut_TextChanged(object sender, EventArgs e)
        {
            string sndr = sender.ToString();
            TimeSpan t0 = DateTime.UtcNow.Subtract(this.Change_Click_EventDate);
            if (t0.TotalMilliseconds >= 1024 || t0.Seconds >= 2)
            {
                string currentLastLine = GetLastLineFromBcText();
                if (currentLastLine != lastLine)
                {
                    lock (bcLock)
                    {
                        lastLine = currentLastLine;
                        Perform_BasicCalculator();
                        this.Change_Click_EventDate = DateTime.UtcNow;
                    }
                }
            }
        }


        /// <summary>
        /// GetLastLineFromBcText() - fetch last entered line in <see cref="bcText"/> by user
        /// </summary>
        /// <returns>lastline from <see cref="System.Web.UI.WebControls.TextBox">TextBox bcText</see></returns>
        protected string GetLastLineFromBcText()
        {
            char[] sep = "\r\n".ToCharArray();
            string[] bcStrings = this.TextBox_BcOut.Text.Split(sep);
            foreach (string bcStr in bcStrings)
            {
                if (!string.IsNullOrEmpty(bcStr) && !bcStack.Contains(bcStr) &&
                    !bcStr.StartsWith("bc 1.07.1") && !bcStr.ToLower().Contains("free software") && !bcStr.ToLower().Contains("warranty"))
                {
                    if (!string.IsNullOrEmpty(bcStr))
                        bcStack.Push(bcStr);
                }
            }

            this.TextBox_BcOp.Text = (bcStack.Count > 0) ? bcStack.Peek() : "";
            return this.TextBox_BcOp.Text;
        }


        /// <summary>
        /// Process classical od cmd
        /// </summary>
        /// <param name="filepath">od cmd filepath</param>
        /// <param name="args">od arguments passed to od</param>
        /// <returns>output of od cmd</returns>
        protected string Process_Bc(
            string filepath = BC_CMD,
            string args = "")
        {
            return ProcessCmd.Execute(filepath, args, false);
        }


        /// <summary>
        /// Perform_BasicCalculator()
        /// </summary>
        /// <param name="bcStr"><see cref="string"/> bcStr passed to bc(1)</param>
        protected void Perform_BasicCalculator(string bcStr = null)
        {
            if (string.IsNullOrEmpty(bcStr))
                bcStr = GetLastLineFromBcText();

            Area23Log.LogStatic(" Executing: " + BC_CMD_PATH + " " + bcStr);
            try
            {                
                string bcCmd = BC_CMD_PATH;                
                string bcOutPut = Process_Bc(bcCmd, bcStr).Trim("\r\n".ToCharArray());
                this.TextBox_BcOut.Text += this.TextBox_BcOut.Text.EndsWith("\r\n") ? "" : "\r\n";
                this.TextBox_BcOut.Text += bcOutPut + "\r\n";
                this.TextBox_BcResult.Text = bcOutPut;
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }
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
            TimeSpan t0 = DateTime.UtcNow.Subtract(this.Change_Click_EventDate);
            if (t0.TotalMilliseconds >= 1024 || t0.Seconds >= 2)
            {
                string currentLastLine = GetLastLineFromBcText();
                if (currentLastLine != lastLine)
                {
                    lock (bcLock)
                    {
                        lastLine = currentLastLine;
                        Perform_BasicCalculator(currentLastLine);
                        this.Change_Click_EventDate = DateTime.UtcNow;
                    }
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
            InitBcText(true);
        }

    }
}