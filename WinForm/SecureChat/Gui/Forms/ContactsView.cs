using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.Crypt.Cipher;
using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Core.Crypt.CqrJd;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Net;
using Area23.At.Framework.Library.Core.Net.IpSocket;
using Area23.At.Framework.Library.Core.Net.WebHttp;
using Area23.At.Framework.Library.Core.Util;
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
