﻿using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.Util;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Crypt.Cipher;
using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
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
using Area23.At.WinForm.SecureChat.Entities;
using Area23.At.WinForm.SecureChat.Util;
using System.Reflection;
using Area23.At.Framework.Library.Core.Crypt;
using Area23.At.Framework.Library.Core.Crypt.Cipher;
using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using NLog.Config;
using Area23.At.Framework.Library.Core.Crypt.CqrJd;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    public partial class SecureChat : Form
    {
        protected string savedFile = string.Empty;
        protected string loadDir = string.Empty;

        private static IPAddress? serverIpAddress;

        internal static IPAddress? ServerIpAddress
        {
            get
            {
                if (serverIpAddress != null)
                    return serverIpAddress;

                // TODO: change it
                IEnumerable<IPAddress> list = NetworkAddresses.GetIpAddrsByHostName("area23.at");
                foreach (IPAddress ip in list)
                {
                    foreach (string sip in Settings.Instance.Proxies)
                    {
                        if (IPAddress.Parse(sip).Equals(ip))
                        {
                            serverIpAddress = ip;
                            return serverIpAddress;
                        }
                    }
                }
                return null;                
            }
        }

        private static IPAddress? externalIPAddress;

        internal static IPAddress? ExternalIpAddress
        {
            get
            {
                if (externalIPAddress != null)
                    return externalIPAddress;

                string hexs = DeEnCoder.KeyToHex(Constants.BC_START_MSG);
                externalIPAddress = WebClientRequest.ClientIpFromArea23("https://area23.at/net/R.aspx", hexs);
                return externalIPAddress;
            }
        }

        public SecureChat()
        {
            InitializeComponent();
            // Resources.
            // MemoryStream ms = new MemoryStream(Properties.Resources.a_hash);
            // buttonSecretKey.Image = new System.Drawing.Bitmap(ms);
            // buttonHashIv.Image = new System.Drawing.Bitmap(ms);
            // ms.Close();
        }


        private async void SecureChat_Load(object sender, EventArgs e)
        {
            if (Entities.Settings.Load() == null)
            {
                var badge = new TransparentBadge($"Error reading Settings from {LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE}.");
                badge.Show();
            }
            if (Entities.Settings.Instance == null || Entities.Settings.Instance.MyContact == null)
            {
                menuItemMyContact_Click(sender, e);
            }

            await SetupNetwork();

            if (Entities.Settings.Instance != null && Entities.Settings.Instance.MyContact != null && !string.IsNullOrEmpty(Entities.Settings.Instance.MyContact.ImageBase64))
            {
                Bitmap? bmp = (Bitmap?)Entities.Settings.Instance.MyContact.ImageBase64.Base64ToImage();
                if (bmp != null)
                    this.pictureBoxYou.Image = bmp;

            }

        }

        private void menuItemClear_Click(object sender, EventArgs e)
        {
            
            this.TextBoxDestionation.Clear();
            this.TextBoxSource.Clear();
            this.richTextBoxChat.Clear();
        }

        private void menuItemSend_Click(object sender, EventArgs e)
        {
            string myServerKey = ExternalIpAddress?.ToString() + ServerIpAddress?.ToString();

            // TODO: test case later
            CqrServerMsg serverMessage = new CqrServerMsg(myServerKey);
            string encrypted = serverMessage.CqrMessage(this.richTextBoxChat.Text);
            this.TextBoxSource.Text = encrypted + "\n";
            string decrypted = serverMessage.NCqrMessage(encrypted);
            this.TextBoxDestionation.Text = decrypted + "\n";
        }

        private void Button_SecretKey_Click(object sender, EventArgs e)
        {
            string richText = this.richTextBoxChat.Text;

            string wabiwabi = string.Empty;
            if (ExternalIpAddress != null)
            {
                wabiwabi = ExternalIpAddress.ToString() + "\n";
            }

            this.TextBoxDestionation.Text += wabiwabi;
            this.richTextBoxOneView.Text += wabiwabi;

        }

        private void menuItemRefresh_Click(object sender, EventArgs e)
        {
            byte[] b0 = ExternalIpAddress.ToExternalBytes();
            Version? assVersion = Assembly.GetExecutingAssembly().GetName().Version;
            byte[] b1 = (assVersion != null) ? assVersion.ToVersionBytes() : new byte[2] { 0x02, 0x18 };
            string privKey = ExternalIpAddress?.ToString() + ServerIpAddress?.ToString();
            string iv = Constants.BC_START_MSG;
            byte[] keyBytes = CryptHelper.GetUserKeyBytes(privKey, iv, 16);

            ZenMatrix.ZenMatrixGenWithBytes(keyBytes, true);
            TextBoxDestionation.Text = "| 0 | => | ";
            foreach (sbyte sb in ZenMatrix.PermKeyHash)
            {
                TextBoxDestionation.Text += sb.ToString("x1") + " ";
            }
            TextBoxDestionation.Text += "| \r\n";
            for (int zeni = 1; zeni < ZenMatrix.PermKeyHash.Count; zeni++)
            {
                sbyte sb = (sbyte)ZenMatrix.PermKeyHash.ElementAt(zeni);
                TextBoxDestionation.Text += "| " + zeni.ToString("x1") + " | => | " + sb.ToString("x1") + " | " + "\r\n";
            }
            // this.TextBoxDestionation.Text += ZenMatrix.EncryptString(this.richTextBoxChat.Text) + "\n";


        }

        private void Button_AddToPipeline_Click(object sender, EventArgs e)
        {
            string? secretKey = Entities.Settings.Instance?.MyContact?.Email;
            IPAddress? myClientIp = HttpClientRequest.GetClientIP();

            string wabiwabi = (myClientIp != null) ? myClientIp.ToString() : string.Empty;
            this.TextBoxDestionation.Text += wabiwabi + "\n";
            this.richTextBoxOneView.Text += wabiwabi + "\n";
        }

        private void Button_HashIv_Click(object sender, EventArgs e)
        {
            string url = "https://area23.at/net/R.aspx";
            Uri uri = new Uri(url);
            HttpClient httpClientR = HttpClientRequest.GetHttpClient(url, Encoding.UTF8);
            Task<HttpResponseMessage> respTask = httpClientR.GetAsync(uri);

        }


        #region Contacts

        private void menuItemMyContact_Click(object sender, EventArgs e)
        {
            ContactSettings contactSettings = new ContactSettings("My Contact Info", 0);
            contactSettings.ShowInTaskbar = true;
            contactSettings.ShowDialog();

            if (Settings.Instance.MyContact != null)
            {
                string base64image = Settings.Instance.MyContact.ImageBase64 ?? string.Empty;

                Bitmap? bmp;
                byte[] bytes = Base64.Decode(base64image);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    bmp = new Bitmap(ms);
                }
                if (bmp != null)
                    this.pictureBoxYou.Image = bmp;


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

        #endregion Contacts


        #region SplitChatWindowLayout

        /// <summary>
        /// MenuView_ItemTopBottom_Click occures, when user clicks on Top-Bottom in chat app
        /// shows top bottom view of chat líke ancient talk/talks
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void MenuView_ItemTopBottom_Click(object sender, EventArgs e)
        {
            menuViewItemLeftRíght.Checked = false;
            menuViewItemTopBottom.Checked = true;
            menuViewItem1View.Checked = false;

            splitContainer.Visible = true;
            panelCenter.Visible = false;

            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.Panel1MinSize = 200;
            splitContainer.Panel2MinSize = 200;
            splitContainer.SplitterDistance = 226;
            splitContainer.SplitterIncrement = 8;
            splitContainer.SplitterWidth = 8;
            splitContainer.MinimumSize = new System.Drawing.Size(600, 400);

        }

        /// <summary>
        /// MenuView_ItemLeftRíght_Click occurs, when user clicks on View->Left-Right in chat app
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void MenuView_ItemLeftRíght_Click(object sender, EventArgs e)
        {
            menuViewItemLeftRíght.Checked = true;
            menuViewItemTopBottom.Checked = false;
            menuViewItem1View.Checked = false;

            splitContainer.Visible = true;
            panelCenter.Visible = false;

            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.Panel1MinSize = 300;
            splitContainer.Panel2MinSize = 300;
            splitContainer.SplitterDistance = 336;
            splitContainer.SplitterIncrement = 8;
            splitContainer.SplitterWidth = 8;
            splitContainer.MinimumSize = new System.Drawing.Size(600, 400);
        }

        /// <summary>
        /// MenuView_Item1View_Click, occurs, when user clicks on 1-View in chat app
        /// shows only a single rich textbox instead of sender and receiver view
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void MenuView_Item1View_Click(object sender, EventArgs e)
        {
            menuViewItemLeftRíght.Checked = false;
            menuViewItemTopBottom.Checked = false;
            menuViewItem1View.Checked = true;

            splitContainer.Visible = false;
            panelCenter.Visible = true;
            richTextBoxOneView.Visible = true;
        }

        #endregion SplitChatWindowLayout


        internal async Task SetupNetwork()
        {

            List<IPAddress> addresses = new List<IPAddress>();
            string[] proxyStrs = Resources.Proxies.Split(";,".ToCharArray());
            foreach (string proxyStr in proxyStrs)
            {
                try
                {
                    IPAddress ip = IPAddress.Parse(proxyStr);
                    addresses.Add(ip);

                }
                catch (Exception ex)
                {
                    CqrException.LastException = ex;
                    Area23Log.LogStatic(ex);
                }
            }
            string[] proxyNameStrs = Resources.ProxyNames.Split(";,".ToCharArray());
            List<string> proxyList = new List<string>();
            foreach (string proxyStr in proxyNameStrs)
            {
                try
                {
                    foreach (var netIp in NetworkAddresses.GetIpAddrsByHostName(proxyStr))
                        if (!addresses.Contains(netIp))
                            addresses.Add(netIp);
                }
                catch (Exception ex)
                {
                    CqrException.LastException = ex;
                    Area23Log.LogStatic(ex);
                }
            }

            
            var ips = await NetworkAddresses.GetConnectedIpAddressesAsync(addresses);
            List<IPAddress> list = new List<IPAddress>(ips);

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

            ToolStripMenuItem extIpItem = new ToolStripMenuItem(ExternalIpAddress.AddressFamily + " " + ExternalIpAddress.ToString(), null, null, ExternalIpAddress.ToString());
            extIpItem.Checked = true;
            extIpItem.Enabled = false;
            this.menuItemExternalIp.DropDownItems.Add(extIpItem);

            foreach (IPAddress addrProxy in addresses)
            {
                if (addrProxy != null)
                {
                    proxyList.Add(addrProxy.ToString());
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



        #region LoadSaveChatContent

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

        #endregion LoadSaveChatContent


        #region HelpAboutInfo

        private void MenuItemHelp_Click(object sender, EventArgs e)
        {
            // TODO: implement it
        }

        private void MenuItemAbout_Click(object sender, EventArgs e)
        {
            TransparentDialog dialog = new TransparentDialog();
            dialog.ShowDialog();
        }

        protected internal void MenuItemInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Text} type {this.GetType()} Information MessageBox.", $"{Text} type {this.GetType()}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion HelpAboutInfo


        #region closeForm

        /// <summary>
        /// Closes Form, if this is the last form of application, then executes <see cref="AppCloseAllFormsExit"/>
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">FormClosingEventArgs e</param>
        private void formClose_Click(object sender, FormClosingEventArgs e)
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
                CqrException.LastException = exFormClose;
                Area23Log.LogStatic(exFormClose);
            }
            try
            {
                this.Dispose(true);
            }
            catch (Exception exFormDispose)
            {
                CqrException.LastException = exFormDispose;
                Area23Log.LogStatic(exFormDispose);
            }

            return;

        }

        /// <summary>
        /// menuFileItemExit_Click is fired, when selecting exit menu 
        /// and will nevertheless close all forms and exits application
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void menuFileItemExit_Click(object sender, EventArgs e)
        {
            AppCloseAllFormsExit();
        }

        /// <summary>
        /// AppCloseAllFormsExit closes all open forms and exit and finally unlocks Mutex
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public virtual void AppCloseAllFormsExit()
        {
            string settingsNotSavedReason = string.Empty;
            try
            {
                if (!Entities.Settings.Save(null))
                    settingsNotSavedReason = "Unknown reason!";
            }
            catch (Exception exSetSave)
            {
                Area23Log.LogStatic(exSetSave);
                settingsNotSavedReason = exSetSave.Message;
            }

            if (!string.IsNullOrEmpty(settingsNotSavedReason))
                MessageBox.Show(settingsNotSavedReason, "Couldn't save chat settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            int openForms = System.Windows.Forms.Application.OpenForms.Count;
            if (openForms <= 1)
            {
                try
                {
                    this.Close();
                    this.Dispose();
                }
                catch (Exception exForm)
                {
                    CqrException.LastException = exForm;
                    Area23Log.LogStatic(exForm);
                }
            }
            else
            {
                for (int frmidx = 0; frmidx < System.Windows.Forms.Application.OpenForms.Count; frmidx++)
                {
                    try
                    {
                        Form? form = System.Windows.Forms.Application.OpenForms[frmidx];
                        if (form != null)
                        {
                            form.Close();
                            form.Dispose();
                        }
                    }
                    catch (Exception exForm)
                    {
                        CqrException.LastException = exForm;
                        Area23Log.LogStatic(exForm);
                    }
                }

            }

            try
            {
                Program.ReleaseCloseDisposeMutex(Program.PMutec);
            }
            catch (Exception ex)
            {
                CqrException.LastException = ex;
                Area23Log.LogStatic(ex);
            }

            Application.ExitThread();
            Dispose();
            Application.Exit();
            Environment.Exit(0);

        }

        #endregion closeForm


    }

}
