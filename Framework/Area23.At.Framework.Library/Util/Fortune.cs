using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.IO;

namespace Area23.At.Framework.Library.Util
{
    public static class Fortune
    {
        private static List<string> fortunes = new List<string>();
        static int execTimes = 0;        
        static readonly object fortuneLock = new object();
        static string[] Fortunes { get => fortunes.ToArray(); }

        public static string ExecFortune()
        {
            string fortuneResult = string.Empty;
            lock (fortuneLock)
            {
                if (fortunes.Count < 1)
                    ReadAllFortunes();
            }

            try
            {
                fortuneResult = ProcessCmd.Execute("/usr/games/fortune", " -a ");
                if (!fortunes.Contains(fortuneResult))
                    fortunes.Add(fortuneResult);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            if (string.IsNullOrEmpty(fortuneResult))
            {
                lock (fortunes)
                {
                    Random rand = new Random(DateTime.UtcNow.Millisecond);
                    int nowFortune = rand.Next(Fortunes.Length);
                    if (Fortunes[nowFortune].Contains("\n"))
                    {
                        ++nowFortune;
                        nowFortune %= Fortunes.Length;
                    }
                    fortuneResult = Fortunes[nowFortune];
                }
            }

            return fortuneResult;
        }

        public static string[] ReadAllFortunes()
        {
            string fortuneFile = LibPaths.TextDirPath + "fortune.u8";
            string fortuneString = (File.Exists(fortuneFile)) ? File.ReadAllText(fortuneFile) : ResReader.GetAllFortunes();
            string[] sep = { "\r\n%\r\n", "\r\n%", "%\r\n" };

            foreach (string addFortune in fortuneString.Split(sep, StringSplitOptions.RemoveEmptyEntries))
            {
                if (fortunes.Contains(addFortune)) continue;
                fortunes.Add(addFortune);
            }

            return fortunes.ToArray();
        }

    }
}
