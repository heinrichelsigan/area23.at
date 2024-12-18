using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Win32Api;
using Area23.At.Mono.Unix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Unix
{
    public partial class FortunAsp : System.Web.UI.Page
    {
        static object fortuneLock;
        static bool useExec = true;
        static short execTimes = 0;
        protected internal List<string> fortunes = new List<string>();

        public string[] Fortunes { get => fortunes.ToArray(); }

        public FortunAsp()
        {
            fortuneLock = new object();
            fortunes = new List<string>();
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
            if (useExec) 
            {
                try
                {
                    ++execTimes; execTimes %= 256;
                    fortuneResult = (longFortune) ?
                        ProcessCmd.Execute("/usr/games/fortune", " -a -l ") :
                        ProcessCmd.Execute("/usr/games/fortune", " -a -i -s  ");
                    useExec = true;
                    if (!fortunes.Contains(fortuneResult))
                        fortunes.Add(fortuneResult);
                }
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                    if (execTimes >= 8) useExec = false;
                }
            }
            if (string.IsNullOrEmpty(fortuneResult))
            {
                lock (fortuneLock)
                {
                    if (fortunes.Count < 1)
                        ReadAllFortunes();

                    Random rand = new Random(DateTime.UtcNow.Millisecond);
                    int nowFortune = rand.Next(Fortunes.Length);
                    while (longFortune ^ Fortunes[nowFortune].Contains("\n"))
                    {
                        ++nowFortune;
                        nowFortune %= Fortunes.Length;
                    }
                    fortuneResult = Fortunes[nowFortune];
                }
            }

            return fortuneResult;
        }

        public void ReadAllFortunes()
        {
            string fortuneFile = LibPaths.TextDirPath + "fortune.u8";
            string fortuneString = (File.Exists(fortuneFile)) ? File.ReadAllText(fortuneFile) : ResReader.GetAllFortunes();
            string[] sep = { "\r\n%\r\n", "\r\n%", "%\r\n" };
            fortunes = new List<string>();
            foreach (string addFortune in fortuneString.Split(sep, StringSplitOptions.RemoveEmptyEntries))
            {
                fortunes.Add(addFortune);
            }            
        }
    }
}