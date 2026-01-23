using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.WinCRoach
{
    public class SaySpeach
    {
        private static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }
        private static string[] TMPS = { "temp", "TEMP", "tmp", "TMP" };
        protected internal static readonly Lazy<SaySpeach> lazySaySingleton = new Lazy<SaySpeach>(() => new SaySpeach());
        protected internal SpeechSynthesizer synthesizer;
        private static string saveDir;
        protected internal string[] voices;

        public static SaySpeach Instance { get => lazySaySingleton.Value; }

        protected internal static string[] AppPaths
        {
            get
            {
                List<string> appPathList = new System.Collections.Generic.List<string>();
                string tmpDir = AppDomain.CurrentDomain.BaseDirectory;
                if (tmpDir != null && Directory.Exists(tmpDir))
                    appPathList.Add(tmpDir);

                foreach (string tmpVar in TMPS)
                {
                    tmpDir = Environment.GetEnvironmentVariable(tmpVar);
                    if (tmpDir != null && tmpDir.Length > 1 && Directory.Exists(tmpDir))
                        appPathList.Add(tmpDir);
                }

                return appPathList.ToArray();
            }
        }

        protected internal static String SavePath
        {
            get
            {
                if (saveDir != null && saveDir.Length > 1 && Directory.Exists(saveDir))
                    return saveDir;

                foreach (string appPath in AppPaths)
                {
                    saveDir = appPath;
                    try
                    {
                        saveDir = Path.Combine(appPath, "wavtmp");
                        DirectoryInfo dirInfo = Directory.CreateDirectory(saveDir);
                        if (dirInfo == null || !dirInfo.Exists)
                            throw new IOException(saveDir + " doesn't exist!");
                    }
                    catch (Exception) { saveDir = appPath; }
                    try
                    {
                        string tmpFile = Path.Combine(saveDir, (DateTime.Now.Ticks + ".txt"));
                        File.WriteAllText(tmpFile, DateTime.Now.ToString());
                        if (File.Exists(tmpFile))
                        {
                            File.Delete(tmpFile);
                            return saveDir;
                        }
                    }
                    catch (Exception) { continue; }
                }

                saveDir = "." + SepChar;
                return saveDir;
            }
        }

        protected internal string WaveFileName(string sayText)
        {
            string sayWaveFile = string.Empty;
            foreach (char sayChar in sayText.ToCharArray())
            {
                if ((((int)sayChar) >= ((int)'A') && ((int)sayChar) <= ((int)'Z')) ||
                    (((int)sayChar) >= ((int)'a') && ((int)sayChar) <= ((int)'z')))
                    sayWaveFile += sayChar;
            }
            if (string.IsNullOrEmpty(sayWaveFile))
                sayWaveFile = DateTime.Now.Ticks.ToString();
            return (sayWaveFile + ".wav");
        }


        protected internal SaySpeach()
        {
            synthesizer = new SpeechSynthesizer();
            saveDir = SavePath;
            voices = (voices == null || voices.Length == 0) ? new string[0] : voices;
            voices = addVoices(voices);
            // SpeechAudioFormatInfo sAudioFormatInfo = new SpeechAudioFormatInfo(1, 1, AudioChannel.Stereo);
            string TempPath = "H:\\Temp";
            string wavFile = TempPath + SepChar + WaveFileName(DateTime.Now.Ticks.ToString());
            try
            {
                if (!File.Exists(wavFile))
                    File.Create(wavFile);
                synthesizer.SetOutputToWaveFile(wavFile);
            }
            catch (Exception)
            {
                synthesizer.SetOutputToDefaultAudioDevice();
            }
        }


        protected internal string[] addVoices(string[] voiceArray)
        {
            if (synthesizer == null)
                synthesizer = new SpeechSynthesizer();

            List<string> myVoices = (voiceArray != null && voiceArray.Length > 0) ?
                new List<string>(voiceArray) : new List<string>();

            foreach (var voice in synthesizer.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                if (info != null)
                {
                    if (!myVoices.Contains(info.Name))
                        myVoices.Add(info.Name);
                    if (info.Gender == VoiceGender.Female)
                        synthesizer.SelectVoice(info.Name);
                }
            }

            return myVoices.ToArray();
        }

        public void Say(string say)
        {
            voices = addVoices(voices);

            if (!string.IsNullOrWhiteSpace(say))
            {
                
                synthesizer.Speak(say);

                Prompt promptA = synthesizer.SpeakAsync(say);
            }
        }
    }
}

