using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.EnDeCoding;
using Area23.At.Framework.Library.Core.Cipher.Symm;
using Area23.At.WinForm.SecureChat.Gui.Forms;
using Area23.At.WinForm.SecureChat.Gui;
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
using Area23.At.Framework.Library.Core.Net;
using System.Net;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    public partial class SecureChat : TransparentFormCore
    {
        protected string savedFile = string.Empty;
        protected string loadDir = string.Empty;

        public SecureChat()
        {
            InitializeComponent();
        }

        public SecureChat(bool encodeOnly = false, bool dragNDrop = false)
        {
            InitializeComponent();
            if (encodeOnly)
            {
                this.ComboBox_RemoteEndPoint.Enabled = false;
            }
            else
            {
                string usrMailKey = (!string.IsNullOrEmpty(this.richTextBoxChat.Text)) ? this.richTextBoxChat.Text : Constants.AUTHOR_EMAIL;                
                ComboBox_RemoteEndPoint.SelectedIndex = 0;
            }

            ComboBox_LocalEndPoint.SelectedIndex = 3;
        }

        private void SecureChat_Load(object sender, EventArgs e)
        {
            List<IPAddress> list = NetworkAddresses.GetConnectedIpAddresses();
            
            int mchecked = 0;
            foreach (IPAddress addr in list)
            {
                if (addr != null) 
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(addr.AddressFamily + " " + addr.ToString(), null, null, addr.ToString());
                    if (mchecked++ == 0)
                        item.Checked = true;
                    else item.Checked = false;

                    this.toolStripMenuView.DropDownItems.Add(item);
                }
            }
        }


        /// <summary>
        /// buttonEncode_Click fired when ButtonEncrypt for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Encode_Click(object sender, EventArgs e)
        {
            // frmConfirmation.Visible = false;

            string usrMailKey = (!string.IsNullOrEmpty(this.richTextBoxChat.Text)) ? this.richTextBoxChat.Text : Constants.AUTHOR_EMAIL;
            
        }


        /// <summary>
        /// Button_Decode_Click fired when Button_Decode for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Decode_Click(object sender, EventArgs e)
        {
            // frmConfirmation.Visible = false;
            string usrMailKey = Constants.AUTHOR_EMAIL;
            
        }

        private void toolStripMenuItemOld_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemNew_Click(object sender, EventArgs e)
        {
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
            if (System.Windows.Forms.Application.OpenForms.Count < 2)
            {
                AppCloseAllFormsExit();
                return;
            }
            try
            {
                this.Close();
            }
            catch (Exception exFormClose)
            {
                Area23Log.LogStatic(exFormClose);
            }
            try
            {
                this.Dispose(true);
            }
            catch (Exception exFormDispose)
            {
                Area23Log.LogStatic(exFormDispose);
            }

            return;

        }

        protected internal void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            AppCloseAllFormsExit();
            return;
        }

        protected internal void menuViewMenuCrypItemEnDeCode_Click(object sender, EventArgs e)
        {

        }

        protected internal void menuViewMenuCryptItemCrypt_Click(object sender, EventArgs e)
        {

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

                var badge = new TransparentBadge($"File {fileName} saved to directory {saveDir}.");
                badge.Show();

                // TODO: outsource in Badge;
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

        }

        private void menuViewMenuUnixItemNetAddr_Click(object sender, EventArgs e)
        {

        }

        private void menuViewMenuICrypttemEnDeCode_Click(object sender, EventArgs e)
        {


        }

        private void menuViewMenuUnixItemScp_Click(object sender, EventArgs e)
        {

        }

        private void menuViewMenuUnixItemSecureChat_Click(object sender, EventArgs e)
        {

        }



        public virtual void AppCloseAllFormsExit()
        {
            for (int frmidx = 0; frmidx < System.Windows.Forms.Application.OpenForms.Count; frmidx++)
            {
                try
                {
                    Form? form = System.Windows.Forms.Application.OpenForms[frmidx];
                    if (form != null && form.Name != this.Name)
                    {
                        form.Close();
                        form.Dispose();
                    }
                }
                catch (Exception exForm)
                {
                    Area23Log.LogStatic(exForm);
                }
            }
            try
            {
                this.Close();
            }
            catch (Exception exFormClose)
            {
                Area23Log.LogStatic(exFormClose);
            }
            try
            {
                this.Dispose(true);
            }
            catch (Exception exFormDispose)
            {
                Area23Log.LogStatic(exFormDispose);
            }

            Exception[] exceptions = new Exception[0];
            try
            {
                Program.ReleaseCloseDisposeMutex(Program.PMutec, out exceptions);
            }
            catch (Exception ex)
            {
                foreach (Exception exMutex in exceptions)
                    Area23Log.LogStatic(exMutex);
                Area23Log.LogStatic(ex);
            }

            Application.ExitThread();
            Dispose();
            Application.Exit();
            Environment.Exit(0);

        }


        private void Button_HashIv_Click(object sender, EventArgs e)
        {
            string usrMailKey = Constants.AUTHOR_EMAIL;
            
        }

        
    }
}
