﻿using Area23.At.Framework.Core.Net;
using Area23.At.Framework.Core.Net.NameService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class NetAddr : TransparentFormCore
    {
        public NetAddr()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            toolStripMenuItemExit_Click(sender, e);
        }

        private void buttonMacs_Click(object sender, EventArgs e)
        {
            IEnumerable<PhysicalAddress> list = Area23.At.Framework.Core.Net.NetworkAddresses.GetMacAddress();
            listBoxAddrs.Items.Clear();
            foreach (PhysicalAddress addr in list)
            {
                listBoxAddrs.Items.Add(addr);
            }
        }

        private void buttonIpAddr_Click(object sender, EventArgs e)
        {
            IEnumerable<IPAddress> list = NetworkAddresses.GetIpAddresses();
            listBoxAddrs.Items.Clear();
            foreach (IPAddress addr in list)
            {
                listBoxAddrs.Items.Add(addr);
            }
        }

        private void buttonIpHostAddr_Click(object sender, EventArgs e)
        {
            IEnumerable<IPAddress> list = DnsHelper.GetIpAddrsByHostName();
            listBoxAddrs.Items.Clear();
            foreach (IPAddress addr in list)
            {
                listBoxAddrs.Items.Add(addr);
            }
        }

        private void buttonConnectedIPAddrs_Click(object sender, EventArgs e)
        {
            List<IPAddress> list = NetworkAddresses.GetConnectedIpAddresses();
            listBoxAddrs.Items.Clear();
            foreach (IPAddress addr in list)
            {
                listBoxAddrs.Items.Add(addr);
            }
        }
    }
}
