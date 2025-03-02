using Area23.At.WinForm.TWinFormCore.Gui.TForms;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Static;
using System.Data;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class TransparentFormCore : System.Windows.Forms.Form
    {
        TransparentBadge? badge;

        public string TFormType
        {
            get => this.GetType().ToString();
        }

        public TransparentFormCore()
        {
            InitializeComponent();
        }

        public TransparentFormCore(string name) : this()
        {
            if (Program.tFormUniqueNames.Contains(name))
                name += "_" + DateTime.Now.Area23DateTimeWithMillis();

            Program.tFormUniqueNames.Add(name);
            this.Text = name;
            this.Name = name;
        }

        private void toolStripMenuItemOld_Click(object sender, EventArgs e)
        {
            if (Program.tWinFormOld == null)
            {
                Program.tWinFormOld = new TWinForm();
                Program.tWinFormOld.Show();
            }
            else
            {
                Program.tWinFormOld.BringToFront();
                Program.tWinFormOld.Focus();
            }
        }

        private void toolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            Program.tFormsNew = Program.tFormsNew ?? new List<System.Windows.Forms.Form>();
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 16)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TransparentFormCore tCoreFormNew = new TransparentFormCore($"TCoreForm{formsCount + 1}");
            Program.tFormsNew.Add(tCoreFormNew);
            tCoreFormNew.Show();
            tCoreFormNew.BringToFront();
            tCoreFormNew.Focus();
        }


        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            openFileDialog = openFileDialog ?? new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                MessageBox.Show($"FileName: {openFileDialog.FileName} init directory: {openFileDialog.InitialDirectory}", $"{Text} type {TFormType}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void toolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            openFileDialog = openFileDialog ?? new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            DialogResult res = openFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                MessageBox.Show($"FileName: {openFileDialog.FileName} init directory: {openFileDialog.InitialDirectory}", $"{Text} type {TFormType}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            TransparentDialog dialog = new TransparentDialog();
            dialog.ShowDialog();
        }

        protected internal void toolStripMenuItemInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Text} type {TFormType} Information MessageBox.", $"{Text} type {TFormType}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected internal void toolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            Program.tFormsNew = Program.tFormsNew ?? new List<System.Windows.Forms.Form>();
            if (Program.tFormsNew.Count > 1)
            {
                var tList = Program.tFormsNew.Where(x => (x.Name == Name));
                System.Windows.Forms.Form tFormCoreInList = (tList == null) ? this : tList.FirstOrDefault();
                try
                {
                    Program.tFormsNew.Remove(item: tFormCoreInList);
                }
                catch
                {
                }
                this.Close();
                try
                {
                    this.Dispose(true);
                }
                catch { }
                return;
            }

        }

        protected internal void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            foreach (var t in Program.tFormsNew)
            {
                try
                {
                    t.Close();
                }
                catch { }
                try { t.Dispose(); } catch { }
            }
            if (Program.tWinFormOld != null)
            {
                try
                {
                    Program.tWinFormOld.Close();
                }
                catch { }
                try { Program.tWinFormOld.Dispose(); } catch { }
            }

            try
            {
                Program.ReleaseCloseDisposeMutex();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

            Application.ExitThread();
            Dispose();
            Application.Exit();
            Environment.Exit(0);
        }

        protected internal void menuViewMenuCrypItemEnDeCode_Click(object sender, EventArgs e)
        {
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 16)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EnDeCodingForm endecodeForm = new EnDeCodingForm(true, false);
            Program.tFormsNew.Add(endecodeForm);
            endecodeForm.Show();
            endecodeForm.BringToFront();
            endecodeForm.Focus();
        }

        protected internal void menuViewMenuCryptItemCrypt_Click(object sender, EventArgs e)
        {
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 16)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EnDeCodingForm endecodeForm = new EnDeCodingForm(false, false);
            Program.tFormsNew.Add(endecodeForm);
            endecodeForm.Show();
            endecodeForm.BringToFront();
            endecodeForm.Focus();
        }

        protected internal virtual void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SafeFileName();
        }

        protected virtual byte[] OpenCryptFileDialog(ref string loadDir)
        {
            if (openFileDialog == null)
                openFileDialog = new OpenFileDialog();
            byte[] fileBytes;
            if (string.IsNullOrEmpty(loadDir))
                loadDir = Environment.GetEnvironmentVariable("TEMP") ?? System.AppDomain.CurrentDomain.BaseDirectory;
            if (loadDir != null)
            {
                openFileDialog.InitialDirectory = loadDir;
                openFileDialog.RestoreDirectory = true;
            }
            DialogResult diaOpenRes = openFileDialog.ShowDialog();
            if (diaOpenRes == DialogResult.OK || diaOpenRes == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName) && File.Exists(openFileDialog.FileName))
                {
                    loadDir = Path.GetDirectoryName(openFileDialog.FileName) ?? System.AppDomain.CurrentDomain.BaseDirectory;
                    fileBytes = File.ReadAllBytes(openFileDialog.FileName);
                    return fileBytes;
                }
            }

            fileBytes = new byte[0];
            return fileBytes;
        }

        protected virtual string SafeFileName(string? filePath = "", byte[]? content = null)
        {
            string? saveDir = Environment.GetEnvironmentVariable("TEMP");
            string ext = ".hex";
            string fileName = DateTime.Now.Area23DateTimeWithSeconds() + ext;
            if (!string.IsNullOrEmpty(filePath))
            {
                fileName = System.IO.Path.GetFileName(filePath);
                saveDir = System.IO.Path.GetDirectoryName(filePath);
                ext = System.IO.Path.GetExtension(filePath);
            }

            if (saveDir != null)
            {
                saveFileDialog.InitialDirectory = saveDir;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.DefaultExt = ext;
            }
            saveFileDialog.FileName = fileName;
            DialogResult diaRes = saveFileDialog.ShowDialog();
            if (diaRes == DialogResult.OK || diaRes == DialogResult.Yes)
            {
                if (content != null && content.Length > 0)
                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, content);

                badge = new TransparentBadge($"File {fileName} saved to directory {saveDir}.");
                badge.Show();
                Point pt = badge.DesktopLocation;

                System.Timers.Timer timer = new System.Timers.Timer { Interval = 1000 };
                System.Timers.Timer timerDispose = new System.Timers.Timer { Interval = 3000 };
                timer.Elapsed += (s, en) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            badge.SetDesktopLocation(pt.X, pt.Y - (i * 2));
                            Thread.Sleep(200);
                        }
                    }));
                    timer.Stop(); // Stop the timer(otherwise keeps on calling)
                };

                timerDispose.Elapsed += (s, en) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if (badge != null)
                        {
                            badge.Close();
                            badge.Dispose();
                        }
                    }));
                    timerDispose.Stop(); // Stop the timer(otherwise keeps on calling)
                };

                timer.Start(); // Starts the show autosave timer after 2,5 sec
                timerDispose.Start(); // Starts the DisposePictureMessage timer after 4sec
            }

            return (saveFileDialog != null && saveFileDialog.FileName != null && File.Exists(saveFileDialog.FileName)) ? saveFileDialog.FileName : null;
        }

        protected internal void menuViewMenuUnixItemFortnune_Click(object sender, EventArgs e)
        {
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 16)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Fortune fortune = new Fortune();
            Program.tFormsNew.Add(fortune);
            fortune.Show();
            fortune.BringToFront();
            fortune.Focus();
        }

        private void menuViewMenuUnixItemNetAddr_Click(object sender, EventArgs e)
        {
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 16)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NetAddr netAddr = new NetAddr();
            Program.tFormsNew.Add(netAddr);
            netAddr.Show();
            netAddr.BringToFront();
            netAddr.Focus();
        }

        private void menuViewMenuICrypttemEnDeCode_Click(object sender, EventArgs e)
        {


        }

        private void menuViewMenuUnixItemScp_Click(object sender, EventArgs e)
        {
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 10)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TScp tscp = new TScp();
            Program.tFormsNew.Add(tscp);
            tscp.Show();
            tscp.BringToFront();
            tscp.Focus();
        }

        private void menuViewMenuUnixItemSecureChat_Click(object sender, EventArgs e)
        {
            int formsCount = Program.tFormsNew.Count;
            if (formsCount > 10)
            {
                MessageBox.Show($"Already {formsCount} instances of {TFormType} currently running!", $"{Program.progName}: maximum reached!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SecureChat schat = new SecureChat();
            Program.tFormsNew.Add(schat);
            schat.Show();
            schat.BringToFront();
            schat.Focus();
        }

        private void menuItemSerializerText_Click(object sender, EventArgs e)
        {
            (new SerializeTest()).Show();

        }
    }
}
