using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Area23.At.Test.MarriageRisk.ConstEnum;
using Area23.At.Test.MarriageRisk.Models;

namespace Area23.At.Test.MarriageRisk
{
    public partial class MarriageRisk : Area23BasePage
    {        
        long errNum = 0; // Errors Ticker
        volatile byte psaychange = 0;

        public void InitSchnaps()
        {
            InitURLBase();

            
        }

        public void RefreshGlobalVariableSession()
        {
            
            this.Context.Session[Constants.APPNAME] = globalVariable;

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (globalVariable == null)
            {
                if (!Page.IsPostBack)
                {
                    InitSchnaps();
                }

                if (this.Context.Session[Constants.APPNAME] == null)
                {
                    string initMsg = "New connection started from " + Request.UserHostAddress + " " + Request.UserHostName + " with " + Request.UserAgent + "!";
                    Log(initMsg);
                    Log("AppPath=" + HttpContext.Current.Request.ApplicationPath + " logging to " + LogFile);
                    globalVariable = new Models.GlobalAppSettings(this.Context, this.Session);
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }                     
            
        }


        void PrintMsg(String msg)
        {
            preOut.InnerText = msg;            
        }

        void ErrHandler(Exception myErr)
        {
            preOut.InnerText += "\r\nCRITICAL ERROR #" + (++errNum);
            preOut.InnerText += "\nMessage: " + myErr.Message;
            preOut.InnerText += "\nString: " + myErr.ToString();
            preOut.InnerText += "\nLmessage: " + myErr.StackTrace + "\n";
        }

        /// <summary>
        /// setTextMessage shows a new Toast dynamic message
        /// </summary>
        /// <param name="textMsg">text to display</param>
        void SetTextMessage(string textMsg)
        {
            string msgSet = string.IsNullOrWhiteSpace(textMsg) ? "" : textMsg;

            Log(msgSet);
        }

        public void Help_Click(object sender, EventArgs e)
        {
            preOut.InnerHtml = "-------------------------------------------------------------------------\n";
            preOut.InnerText += JavaResReader.GetValue("help_text", globalVariable.TwoLetterISOLanguageName) + "\n";
            preOut.InnerHtml += "-------------------------------------------------------------------------\n";
        }

    }
}