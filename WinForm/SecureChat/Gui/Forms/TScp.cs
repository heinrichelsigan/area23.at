using Area23.At.Framework.Library.Core.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    public partial class TScp : TransparentFormCore
    {
        public TScp()
        {
            InitializeComponent();
        }


        protected override void OnCreateControl()
        {
            if (this.comboBoxHosts != null && this.comboBoxHosts.Items != null && this.comboBoxHosts.Items.Count == 0)
            {
                // comboBoxHosts.Items.AddRange(new object[] { "area23.at", "virginia.area23.at", "paris.area23.at", "parisienne.area23.at" });
                this.comboBoxHosts.Items.AddRange(GetItemsComboBoxHosts());
                if (this.comboBoxHosts.Items != null && this.comboBoxHosts.Items.Count > 0)
                    this.comboBoxHosts.SelectedIndex = 0;
            }
            base.OnCreateControl();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            toolStripMenuItemExit_Click(sender, e);
        }





        protected object[] GetItemsComboBoxHosts()
        {
            List<object> entries = new List<object>();
            bool? keyExists = ConfigurationManager.AppSettings?.AllKeys.Contains("SshKeyDirectory");

            if (keyExists.HasValue && keyExists.Value)
            {
                string dirName = ConfigurationManager.AppSettings["SshKeyDirectory"].ToString();
                string newHostStr = string.Empty;

                foreach (string hostStr in GetAllHostsNameByPuttyPrivateKey(dirName))
                {
                    if (hostStr.Contains('\\'))
                        newHostStr = hostStr.Substring(hostStr.LastIndexOf('\\') + 1).Replace(".ppk", "");

                    if (!entries.Contains(newHostStr))
                    {
                        entries.Add(newHostStr);
                        entries.AddRange(NetworkAddresses.GetDnsHostNamesByHostName(newHostStr));
                    }
                }
            }

            return entries.ConvertAll<object>(o => (object)o).ToArray();
        }


        protected string[] GetAllHostsNameByPuttyPrivateKey(string keyDirectory)
        {
            if (string.IsNullOrEmpty(keyDirectory) || !Directory.Exists(keyDirectory))
                return (new string[0]);

            return Directory.GetFiles(keyDirectory, "*.ppk", SearchOption.TopDirectoryOnly);

        }

        private void buttonSelectLeft_Click(object sender, EventArgs e)
        {
            if (openFileDialog == null)
                openFileDialog = new OpenFileDialog();
            string loadDir = string.Empty;
            
            if (string.IsNullOrEmpty(loadDir))
                    loadDir = Environment.GetEnvironmentVariable("TEMP") ?? System.AppDomain.CurrentDomain.BaseDirectory;
            if (loadDir != null)
            {
                openFileDialog.SupportMultiDottedExtensions = true;
                openFileDialog.ShowHiddenFiles = true;
                openFileDialog.Multiselect = false;
                openFileDialog.CheckPathExists = true;
                // openFileDialog.CheckFileExists = true;
                openFileDialog.InitialDirectory = loadDir;
                openFileDialog.RestoreDirectory = true;
            }
            DialogResult diaOpenRes = openFileDialog.ShowDialog();
            if (diaOpenRes == DialogResult.OK || diaOpenRes == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    int lidx = openFileDialog.FileName.LastIndexOf('\\');
                    string parentDir = openFileDialog.FileName.Substring(0, lidx);                
                
                    if (File.Exists(openFileDialog.FileName))
                        this.comboBoxLeft.Items.Add(openFileDialog.FileName.ToString());
                    else if (Directory.Exists(parentDir))
                        this.comboBoxLeft.Items.Add(parentDir);
                    
                }
            }

        }
    }
}
