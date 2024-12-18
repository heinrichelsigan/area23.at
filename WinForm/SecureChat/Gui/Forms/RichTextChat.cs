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
using Area23.At.Framework.Library.Core.Net.WebHttp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Area23.At.WinForm.SecureChat.Properties;
using System.Runtime.CompilerServices;
using System.IO;
using Area23.At.Framework.Library.Core.Util;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    public partial class RichTextChat : Form
    {
        protected string savedFile = string.Empty;
        protected string loadDir = string.Empty;

        public RichTextChat()
        {
            InitializeComponent();
            MemoryStream ms = new MemoryStream(Properties.Resources.a_hash);
            buttonSecretKey.Image = new System.Drawing.Bitmap(ms);
            buttonHashIv.Image = new System.Drawing.Bitmap(ms);
            ms.Close();
        }


        private void RichTextChat_Load(object sender, EventArgs e)
        {
            if (Entities.Settings.Load() == null)
            {
                var badge = new TransparentBadge($"Error reading Settings from {LibPaths.AppDirPath + Constants.JSON_SETTINGS_FILE}.");
                badge.Show();
            }
            if (Entities.Settings.Instance == null || Entities.Settings.Instance.MyContact == null)
            {
                menuItemMyContact_Click(sender, e);
            }

            if (Entities.Settings.Instance != null && Entities.Settings.Instance.MyContact != null && !string.IsNullOrEmpty(Entities.Settings.Instance.MyContact.ImageBase64))
            {
                this.pictureBoxYou.Image = Entities.Settings.Instance.MyContact.ImageBase64.Base64ToImage();

            }

            List<IPAddress> addresses = new List<IPAddress>();
            string[] proxieStrs = Resources.Proxies.Split(";,".ToCharArray());
            List<string> proxyList = new List<string>();
            foreach (string str in proxieStrs)
            {
                try
                {
                    IPAddress ip = IPAddress.Parse(str);
                    addresses.Add(ip);
                    proxyList.Add(ip.ToString());
                }
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }
            }

            List<IPAddress> list = NetworkAddresses.GetConnectedIpAddresses(addresses);

            List<string> myIpStrList = new List<string>();
            int mchecked = 0;
            this.menuItemIPv6Secure.Checked = false;
            foreach (IPAddress addr in list)
            {
                if (addr != null)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(addr.AddressFamily + " " + addr.ToString(), null, null, addr.ToString());
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        this.menuItemIPv6Secure.Checked = true;
                    }
                    myIpStrList.Add(addr.ToString());
                    if (mchecked++ == 0)
                        item.Checked = true;
                    else item.Checked = false;

                    this.menuIItemMyIps.DropDownItems.Add(item);
                }
            }

            foreach (IPAddress addrProxy in addresses)
            {
                if (addrProxy != null)
                {
                    if (addrProxy.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ||
                        (addrProxy.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && this.menuItemIPv6Secure.Checked))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(addrProxy.AddressFamily + " " + addrProxy.ToString(), null, null, addrProxy.ToString());
                        this.menuItemProxyServers.DropDownItems.Add(item);
                    }
                }
            }

            if (Entities.Settings.Instance != null)
            {
                Entities.Settings.Instance.Proxies = proxyList;
                Entities.Settings.Instance.MyIPs = myIpStrList;
                Entities.Settings.Save(Entities.Settings.Instance);
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


        private void Button_SecretKey_Click(object sender, EventArgs e)
        {
            string richText = this.richTextBoxChat.Text;

            string? secretKey = Entities.Settings.Instance?.MyContact?.Email;

            string url = "https://area23.at/net/R.aspx";
            WebClient webclient = WebClientRequest.GetWebClient(url, secretKey ?? "wabiwabi");
            string wabiwabi = webclient.DownloadString(url);


            this.richTextBoxOneView.Text += wabiwabi + "\n";

        }

        private void Button_AddToPipeline_Click(object sender, EventArgs e)
        {
            string? secretKey = Entities.Settings.Instance?.MyContact?.Email;
            string url = "https://area23.at/net/R.aspx";
            Uri uri = new Uri(url);
            HttpClient httpClientR = HttpClientRequest.GetHttpClient(url, secretKey ?? "wabiwabi");
            ConfiguredTaskAwaitable<HttpResponseMessage> respTask =
                httpClientR.GetAsync(uri).ConfigureAwait(false);
        }

        private void Button_HashIv_Click(object sender, EventArgs e)
        {
            string url = "https://area23.at/net/R.aspx";
            Uri uri = new Uri(url);
            HttpClient httpClientR = HttpClientRequest.GetHttpClient(url, Encoding.UTF8);
            Task<HttpResponseMessage> respTask = httpClientR.GetAsync(uri);

        }



        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            openFileDialog = openFileDialog ?? new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                MessageBox.Show($"FileName: {openFileDialog.FileName} init directory: {openFileDialog.InitialDirectory}", $"{Text} type {openFileDialog.GetType()}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void toolStripMenuItemLoad_Click(object sender, EventArgs e)
        {
            openFileDialog = openFileDialog ?? new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            DialogResult res = openFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                MessageBox.Show($"FileName: {openFileDialog.FileName} init directory: {openFileDialog.InitialDirectory}", $"{Text} type {openFileDialog.GetType()}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            TransparentDialog dialog = new TransparentDialog();
            dialog.ShowDialog();
        }

        protected internal void toolStripMenuItemInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Text} type {this.GetType()} Information MessageBox.", $"{Text} type {this.GetType()}", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // var badge = new TransparentBadge($"File {fileName} saved to directory {saveDir}.");
                // badge.Show();
            }

            return (saveFileDialog != null && saveFileDialog.FileName != null && File.Exists(saveFileDialog.FileName)) ? saveFileDialog.FileName : null;
        }




        private void menuItemMyContact_Click(object sender, EventArgs e)
        {
            ContactSettings contactSettings = new ContactSettings("My Contact Info", 0);
            contactSettings.ShowInTaskbar = true;
            contactSettings.ShowDialog();

            if (Entities.Settings.Instance.MyContact != null)
            {
                string base64image = Entities.Settings.Instance.MyContact.ImageBase64 ?? string.Empty;
                if (!string.IsNullOrEmpty(base64image))
                    this.pictureBoxYou.Image = base64image.Base64ToImage();

                // var badge = new TransparentBadge("My contact added!");
                // badge.ShowDialog();

            }

        }

        private void menuItemAddContact_Click(object sender, EventArgs e)
        {
            ContactSettings contactSettings = new ContactSettings("Add Contact Info", 1);
            contactSettings.ShowInTaskbar = true;
            contactSettings.ShowDialog();

            // var badge = new TransparentBadge("My contact added!");
            // badge.ShowDialog();
        }

        private void menuItemSend_Click(object sender, EventArgs e)
        {

        }

        private void menuViewItemTopBottom_Click(object sender, EventArgs e)
        {
            menuViewItemLeftRíght.Checked = false;
            menuViewItemTopBottom.Checked = true;
            menuIViewItem1View.Checked = false;

            

        }

        private void menuViewItemLeftRíght_Click(object sender, EventArgs e)
        {
            menuViewItemLeftRíght.Checked = true;
            menuViewItemTopBottom.Checked = false;
            menuIViewItem1View.Checked = false;

        }

        private void menuIViewItem1View_Click(object sender, EventArgs e)
        {
            menuViewItemLeftRíght.Checked = false;
            menuViewItemTopBottom.Checked = false;
            menuIViewItem1View.Checked = true;
        }



        private void menuFileItemExit_Click(object sender, EventArgs e)
        {
            AppCloseAllFormsExit();
        }

        /// <summary>
        /// AppCloseAllFormsExit closes all open forms and exit
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public virtual void AppCloseAllFormsExit()
        {
            if (!Entities.Settings.Save(Entities.Settings.Instance))
            {
                throw new ApplicationException("Cannot persist settings for SecureChat!");
            }

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

    }

}
