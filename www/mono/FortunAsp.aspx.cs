using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono
{
    public partial class FortunAsp : System.Web.UI.Page
    {
        static object fortuneLock;

        public FortunAsp()
        {
                fortuneLock = new object();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFortune();
        }

        protected void ButtonHidden_Click(object sender, EventArgs e)
        {
            SetFortune();
        }

        protected void SetFortune()
        {
            LiteralFortune.Text = HttpUtility.HtmlEncode(ExecFortune(false));
            PreFortune.InnerText = ExecFortune(true);
        }

        protected string ExecFortune(bool longFortune = true)
        {
            string fortuneResult = string.Empty;
            try
            {
                fortuneResult = (longFortune) ?
                    ProcessCmd.Execute("/usr/games/fortune", " -a -l ") :
                    ProcessCmd.Execute("/usr/games/fortune", "-o -s  ");
            } 
            catch (Exception)
            {
                lock (fortuneLock)
                {
                    string[] filenames = { "res" + Paths.SepChar + "fortune.u8", Paths.OutDir + Paths.SepChar + "fortune.u8", "fortune.u8", "Properties" + Paths.SepChar + "fortune.u8" };
                    int fp = 0;
                    while (!File.Exists(filenames[fp]))
                        ++fp;

                    string[] sep = { "\r\n%\r\n", "\r\n%", "%\r\n" };
                    string[] allFortunes = File.ReadAllText(filenames[fp]).Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    Random rand = new Random(DateTime.UtcNow.Millisecond);
                    int nowFortune = rand.Next(allFortunes.Length);
                    while (longFortune ^ allFortunes[nowFortune].Contains("\n"))
                    {
                        ++nowFortune;
                        nowFortune %= allFortunes.Length;
                    }
                    fortuneResult = allFortunes[nowFortune];
                }
            }
            finally { } // nothing todo here
            return fortuneResult;
        }
    }
}