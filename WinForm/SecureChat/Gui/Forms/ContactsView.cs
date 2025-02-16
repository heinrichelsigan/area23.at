using Area23.At.Framework.Core;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.CqrXs;
using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.CqrXs.CqrSrv;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net;
using Area23.At.Framework.Core.Net.IpSocket;
using Area23.At.Framework.Core.Net.WebHttp;
using Area23.At.Framework.Core.Util;
using Area23.At.WinForm.SecureChat.Entities;
using Area23.At.WinForm.SecureChat.Properties;
using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    partial class ContactsView : System.Windows.Forms.Form
    {

        int contactsCount = 0;

        public ContactsView()
        {
            InitializeComponent();                
        }

        private void ContactsView_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            Refresh_DataGrid(sender, e);
            this.Text = $"ContactView shows {contactsCount} contacts";
        }

        internal void Refresh_DataGrid(object sender, EventArgs e)
        {
            Entities.Contact[] contacts;
            this.dataGridContacts.Rows.Clear();
            // DataGridViewRow row = new DataGridViewRow();
            string userCreatorOwner = Environment.UserName;            
            if ((contacts = Entities.Settings.Instance.Contacts.ToArray()) != null)
            {
                contactsCount = contacts.Length;
                foreach (Entities.Contact contact in contacts)
                {
                    this.dataGridContacts.Rows.Add(contact.RowParams);
                }
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


    }
}
