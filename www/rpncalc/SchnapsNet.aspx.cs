using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using area23.at.mono.rpncalc.ConstEnum;
using area23.at.mono.rpncalc.Models;

namespace area23.at.mono.rpncalc
{
    public partial class SchnapsNet : Area23BasePage
    {        
        long errNum = 0; // Errors Ticker
        int ccard = -1; // Computers Card played        
        volatile byte psaychange = 0;

        // static String emptyJarStr = "/schnapsen/cardpics/e.gif";
        // static String backJarStr =  "/schnapsen/cardpics/verdeckt.gif";
        // static String notJarStr =   "/schnapsen/cardpics/n0.gif";
        // static String talonJarStr = "/schnapsen/cardpics/t.gif";
        // Thread t0;

        public void InitSchnaps()
        {
            InitURLBase();

            // preOut.InnerText = "";
            // tMsg.Enabled = false;

            // im0.ImageUrl = emptyURL.ToString();
            // im1.ImageUrl = emptyURL.ToString();
            // im2.ImageUrl = emptyURL.ToString();
            // im3.ImageUrl = emptyURL.ToString();
            // im4.ImageUrl = emptyURL.ToString();

            // imOut0.ImageUrl = emptyURL.ToString();
            // imOut20.ImageUrl = emptyURL.ToString();
            // imOut1.ImageUrl = emptyURL.ToString();
            // imOut21.ImageUrl = emptyURL.ToString();
            // imTalon.ImageUrl = emptyTalonUri.ToString();
            // imTalon.Visible = true;
            // imAtou10.ImageUrl = emptyURL.ToString();

            // bMerge.Text = ResReader.GetValueFromKey("bStart_text", Locale.TwoLetterISOLanguageName);
            // bStop.Text = ResReader.GetValueFromKey("bStop_text", Locale.TwoLetterISOLanguageName);
            // bStop.Enabled = false;
            // bStop.Visible = false;
            // b20b.Text = ResReader.GetValueFromKey("b20b_text", Locale.TwoLetterISOLanguageName);
            // b20b.Enabled = false;
            // b20a.Text = ResReader.GetValueFromKey("b20a_text", Locale.TwoLetterISOLanguageName);
            // b20a.Enabled = false;

            // bChange.Text = ResReader.GetValueFromKey("bChange_text", Locale.TwoLetterISOLanguageName);
            // bChange.Enabled = false;


            // bContinue.Text = ResReader.GetValueFromKey("bContinue_text", Locale.TwoLetterISOLanguageName);
            // bContinue.Enabled = true;

            // bHelp.Text = ResReader.GetValueFromKey("bHelp_text", Locale.TwoLetterISOLanguageName);
            // bHelp.ToolTip = ResReader.GetValueFromKey("bHelp_text", Locale.TwoLetterISOLanguageName);

            // tRest.Enabled = false;
            // tRest.Text = ResReader.GetValueFromKey("tRest_text", Locale.TwoLetterISOLanguageName);            
            // lRest.Text = ResReader.GetValueFromKey("sRest", Locale.TwoLetterISOLanguageName);

            // this.imOut20.ToolTip = ResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            // this.imOut21.ToolTip = ResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);
            // this.imMerge11.ToolTip = ResReader.GetValueFromKey("imageMerge_ToolTip", Locale.TwoLetterISOLanguageName);


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
                    // aTournement = new Tournament();
                    // globalVariable.Tournement = aTournement;
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }

           
        }



        void PrintMsg()
        {
            // preOut.InnerText = "" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        void ErrHandler(Exception myErr)
        {
            // preOut.InnerText += "\r\nCRITICAL ERROR #" + (++errNum);
            // preOut.InnerText += "\nMessage: " + myErr.Message;
            // preOut.InnerText += "\nString: " + myErr.ToString();
            // preOut.InnerText += "\nLmessage: " + myErr.StackTrace + "\n";
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
}
}