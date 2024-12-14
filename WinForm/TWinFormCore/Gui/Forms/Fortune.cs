using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.EnDeCoding;
using Area23.At.Framework.Library.Core.Cipher.Symm;
using Area23.At.WinForm.TWinFormCore.Gui.Forms;
using Area23.At.WinForm.TWinFormCore.Gui;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class Fortune : TransparentFormCore
    {
        static object fortuneLock = new object();
        static bool useExec = true;
        static short execTimes = 0;
        protected internal List<string> fortunes = new List<string>();

        public string[] Fortunes { get => fortunes.ToArray(); }

        public Fortune()
        {
            InitializeComponent();
            SetFortune();
        }

        protected void SetFortune()
        {
            textBoxFortune.Text = ExecFortune(false);
        }

        protected string ExecFortune(bool longFortune = true)
        {
            string fortuneResult = string.Empty;
            //if (useExec)
            //{
            //    try
            //    {
            //        ++execTimes; execTimes %= 256;
            //        fortuneResult = (longFortune) ?
            //            ProcessCmd.Execute("/usr/games/fortune", " -a -l ") :
            //            ProcessCmd.Execute("/usr/games/fortune", " -a -i -s  ");
            //        useExec = true;
            //        if (!fortunes.Contains(fortuneResult))
            //            fortunes.Add(fortuneResult);
            //    }
            //    catch (Exception ex)
            //    {
            //        Area23Log.LogStatic(ex);
            //        if (execTimes >= 8) useExec = false;
            //    }
            //}
            if (string.IsNullOrEmpty(fortuneResult))
            {
                lock (fortuneLock)
                {
                    if (fortunes.Count < 1)
                        fortunes = ReadAllFortunes();

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

        public List<string> ReadAllFortunes()
        {
            string fortuneFile = LibPaths.TextDirPath + "fortune.u8";
            string fortuneString = (File.Exists(fortuneFile)) ? File.ReadAllText(fortuneFile) : ResReader.GetAllFortunes();
            string[] sep = { "\r\n%\r\n", "\r\n%", "%\r\n" };
            List<string> forts = new List<string>();
            foreach (string addFortune in fortuneString.Split(sep, StringSplitOptions.RemoveEmptyEntries))
            {
                forts.Add(addFortune);
            }
            return forts;
        }

        private void buttonFortune_Click(object sender, EventArgs e)
        {
            SetFortune();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            toolStripMenuItemExit_Click(sender, e);
        }
    }
}
