
using Area23.At.Framework.Core.Net.WebHttp;
using System.Net;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{

    partial class RichTextChat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            menuCQRChat = new ToolStripMenuItem();
            menuFileItemOpen = new ToolStripMenuItem();
            menuFileItemSave = new ToolStripMenuItem();
            menuFileSeparator = new ToolStripSeparator();
            menuCqrChatItemPerist = new ToolStripMenuItem();
            meuFileToExitSperator = new ToolStripSeparator();
            menuFileItemExit = new ToolStripMenuItem();
            menuView = new ToolStripMenuItem();
            menuViewItemLeftRíght = new ToolStripMenuItem();
            menuViewItemTopBottom = new ToolStripMenuItem();
            menuViewItem1View = new ToolStripMenuItem();
            menuNetwork = new ToolStripMenuItem();
            menuIItemMyIps = new ToolStripMenuItem();
            menuItemExternalIp = new ToolStripMenuItem();
            menuItemFriendIp = new ToolStripMenuItem();
            menuItempComboBoxFriendIp = new ToolStripComboBox();
            menuItemProxyServers = new ToolStripMenuItem();
            menuIPsSeparator = new ToolStripSeparator();
            menuItemIPv6Secure = new ToolStripMenuItem();
            toolMenuConnect = new ToolStripMenuItem();
            menuConnectItemFriend = new ToolStripMenuItem();
            menuConnectSeparator = new ToolStripSeparator();
            menuConnectComboBoxIps = new ToolStripComboBox();
            menuConnectSeparatorLast = new ToolStripSeparator();
            menuConnectItemLoopback = new ToolStripMenuItem();
            menuCommands = new ToolStripMenuItem();
            menuItemSend = new ToolStripMenuItem();
            menuItemAttach = new ToolStripMenuItem();
            menuCommandsSeperator = new ToolStripSeparator();
            menuItemRefresh = new ToolStripMenuItem();
            menuItemClear = new ToolStripMenuItem();
            menuContacts = new ToolStripMenuItem();
            menuContactsItemMe = new ToolStripMenuItem();
            menuContactsItemAdd = new ToolStripMenuItem();
            menuContactsItemView = new ToolStripMenuItem();
            menuContactsSeparetor = new ToolStripSeparator();
            menuContactstemImport = new ToolStripMenuItem();
            menuContactstemExport = new ToolStripMenuItem();
            menuQuestionMark = new ToolStripMenuItem();
            menuItemHelp = new ToolStripMenuItem();
            menuItemInfo = new ToolStripMenuItem();
            menuItemAbout = new ToolStripMenuItem();
            menuEdit = new ToolStripMenuItem();
            menuEditItemCut = new ToolStripMenuItem();
            menuEditItemCopy = new ToolStripMenuItem();
            menuIEdittemPaste = new ToolStripMenuItem();
            menuEditItemSelectAll = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            statusStrip = new StatusStrip();
            toolStripProgressBar = new ToolStripProgressBar();
            toolStripStatusLabel = new ToolStripStatusLabel();
            SplitChatView = new SplitContainer();
            TextBoxSource = new TextBox();
            TextBoxDestionation = new TextBox();
            PictureBoxYou = new PictureBox();
            ButtonKey = new Button();
            PanelEnCodeCrypt = new Panel();
            ButtonCheck = new Button();
            ComboBoxSecretKey = new ComboBox();
            ComboBoxIpContact = new ComboBox();
            TextBoxPipe = new TextBox();
            RichTextBoxChat = new RichTextBox();
            PictureBoxPartner = new PictureBox();
            PanelDestination = new Panel();
            GroupBoxLinks = new Controls.GroupBoxLinkLabels();
            PanelCenter = new Panel();
            RichTextBoxOneView = new RichTextBox();
            PanelBottom = new Panel();
            buttonAttach = new Button();
            button1 = new Button();
            buttonClear = new Button();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitChatView).BeginInit();
            SplitChatView.Panel1.SuspendLayout();
            SplitChatView.Panel2.SuspendLayout();
            SplitChatView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxYou).BeginInit();
            PanelEnCodeCrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxPartner).BeginInit();
            PanelDestination.SuspendLayout();
            PanelCenter.SuspendLayout();
            PanelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.AllowItemReorder = true;
            menuStrip.BackColor = SystemColors.MenuBar;
            menuStrip.Font = new Font("Lucida Sans Unicode", 10F);
            menuStrip.GripStyle = ToolStripGripStyle.Visible;
            menuStrip.Items.AddRange(new ToolStripItem[] { menuCQRChat, menuView, menuNetwork, toolMenuConnect, menuCommands, menuContacts, menuQuestionMark });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.RenderMode = ToolStripRenderMode.System;
            menuStrip.Size = new Size(976, 25);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip";
            // 
            // menuCQRChat
            // 
            menuCQRChat.BackColor = SystemColors.MenuBar;
            menuCQRChat.DropDownItems.AddRange(new ToolStripItem[] { menuFileItemOpen, menuFileItemSave, menuFileSeparator, menuCqrChatItemPerist, meuFileToExitSperator, menuFileItemExit });
            menuCQRChat.ForeColor = SystemColors.MenuText;
            menuCQRChat.Name = "menuCQRChat";
            menuCQRChat.Padding = new Padding(3, 0, 3, 0);
            menuCQRChat.ShortcutKeys = Keys.Alt | Keys.F;
            menuCQRChat.Size = new Size(73, 21);
            menuCQRChat.Text = "cqr chat";
            // 
            // menuFileItemOpen
            // 
            menuFileItemOpen.AutoToolTip = true;
            menuFileItemOpen.BackColor = SystemColors.MenuBar;
            menuFileItemOpen.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemOpen.ForeColor = SystemColors.MenuText;
            menuFileItemOpen.Margin = new Padding(1);
            menuFileItemOpen.Name = "menuFileItemOpen";
            menuFileItemOpen.ShortcutKeys = Keys.Control | Keys.I;
            menuFileItemOpen.Size = new Size(214, 22);
            menuFileItemOpen.Text = "import chats";
            menuFileItemOpen.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemOpen.ToolTipText = "imports saved chats from a file";
            menuFileItemOpen.Click += toolStripMenuItemLoad_Click;
            // 
            // menuFileItemSave
            // 
            menuFileItemSave.AutoToolTip = true;
            menuFileItemSave.BackColor = SystemColors.MenuBar;
            menuFileItemSave.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemSave.ForeColor = SystemColors.MenuText;
            menuFileItemSave.Margin = new Padding(1);
            menuFileItemSave.Name = "menuFileItemSave";
            menuFileItemSave.ShortcutKeys = Keys.Control | Keys.E;
            menuFileItemSave.Size = new Size(214, 22);
            menuFileItemSave.Text = "export chats";
            menuFileItemSave.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemSave.ToolTipText = "saves chats on local harddisk not on server";
            menuFileItemSave.Click += toolStripMenuItemSave_Click;
            // 
            // menuFileSeparator
            // 
            menuFileSeparator.BackColor = SystemColors.MenuBar;
            menuFileSeparator.ForeColor = SystemColors.MenuText;
            menuFileSeparator.Margin = new Padding(1);
            menuFileSeparator.Name = "menuFileSeparator";
            menuFileSeparator.Size = new Size(211, 6);
            // 
            // menuCqrChatItemPerist
            // 
            menuCqrChatItemPerist.AutoToolTip = true;
            menuCqrChatItemPerist.BackColor = SystemColors.MenuHighlight;
            menuCqrChatItemPerist.BackgroundImageLayout = ImageLayout.Center;
            menuCqrChatItemPerist.Checked = true;
            menuCqrChatItemPerist.CheckState = CheckState.Checked;
            menuCqrChatItemPerist.ForeColor = SystemColors.MenuText;
            menuCqrChatItemPerist.Margin = new Padding(1);
            menuCqrChatItemPerist.Name = "menuCqrChatItemPerist";
            menuCqrChatItemPerist.ShortcutKeys = Keys.Control | Keys.P;
            menuCqrChatItemPerist.Size = new Size(214, 22);
            menuCqrChatItemPerist.Text = "persist chats";
            menuCqrChatItemPerist.TextImageRelation = TextImageRelation.TextAboveImage;
            menuCqrChatItemPerist.ToolTipText = "if cheked chats will be saved on local harddisk automatically, so that you can reimport elsewhere";
            // 
            // meuFileToExitSperator
            // 
            meuFileToExitSperator.BackColor = SystemColors.MenuBar;
            meuFileToExitSperator.ForeColor = SystemColors.MenuText;
            meuFileToExitSperator.Margin = new Padding(1);
            meuFileToExitSperator.Name = "meuFileToExitSperator";
            meuFileToExitSperator.Size = new Size(211, 6);
            // 
            // menuFileItemExit
            // 
            menuFileItemExit.BackColor = SystemColors.MenuBar;
            menuFileItemExit.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemExit.ForeColor = SystemColors.MenuText;
            menuFileItemExit.Name = "menuFileItemExit";
            menuFileItemExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuFileItemExit.Size = new Size(214, 22);
            menuFileItemExit.Text = "exit";
            menuFileItemExit.ToolTipText = "exit chat application";
            menuFileItemExit.Click += MenuFileItemExit_Click;
            // 
            // menuView
            // 
            menuView.BackColor = SystemColors.MenuBar;
            menuView.BackgroundImageLayout = ImageLayout.None;
            menuView.DropDownItems.AddRange(new ToolStripItem[] { menuViewItemLeftRíght, menuViewItemTopBottom, menuViewItem1View });
            menuView.ForeColor = SystemColors.MenuText;
            menuView.ImageScaling = ToolStripItemImageScaling.None;
            menuView.Name = "menuView";
            menuView.Padding = new Padding(3, 0, 3, 0);
            menuView.ShortcutKeys = Keys.Alt | Keys.V;
            menuView.Size = new Size(48, 21);
            menuView.Text = "view";
            // 
            // menuViewItemLeftRíght
            // 
            menuViewItemLeftRíght.BackColor = SystemColors.MenuBar;
            menuViewItemLeftRíght.Checked = true;
            menuViewItemLeftRíght.CheckState = CheckState.Checked;
            menuViewItemLeftRíght.Name = "menuViewItemLeftRíght";
            menuViewItemLeftRíght.ShortcutKeys = Keys.Alt | Keys.L;
            menuViewItemLeftRíght.Size = new Size(204, 22);
            menuViewItemLeftRíght.Text = "left-ríght";
            menuViewItemLeftRíght.ToolTipText = "shows a left->right splitted chat view";
            menuViewItemLeftRíght.Click += MenuView_ItemLeftRíght_Click;
            // 
            // menuViewItemTopBottom
            // 
            menuViewItemTopBottom.BackColor = SystemColors.MenuBar;
            menuViewItemTopBottom.Name = "menuViewItemTopBottom";
            menuViewItemTopBottom.ShortcutKeys = Keys.Alt | Keys.T;
            menuViewItemTopBottom.Size = new Size(204, 22);
            menuViewItemTopBottom.Text = "top-bottom";
            menuViewItemTopBottom.ToolTipText = "top-bottom shows a top->bottom splitted chat view";
            menuViewItemTopBottom.Click += MenuView_ItemTopBottom_Click;
            // 
            // menuViewItem1View
            // 
            menuViewItem1View.BackColor = SystemColors.MenuBar;
            menuViewItem1View.Name = "menuViewItem1View";
            menuViewItem1View.ShortcutKeys = Keys.Alt | Keys.D1;
            menuViewItem1View.Size = new Size(204, 22);
            menuViewItem1View.Text = "1-view";
            menuViewItem1View.ToolTipText = "displays a single box chat view";
            menuViewItem1View.Click += MenuView_Item1View_Click;
            // 
            // menuNetwork
            // 
            menuNetwork.BackColor = SystemColors.MenuBar;
            menuNetwork.DropDownItems.AddRange(new ToolStripItem[] { menuIItemMyIps, menuItemFriendIp, menuItemProxyServers, menuIPsSeparator, menuItemIPv6Secure });
            menuNetwork.Name = "menuNetwork";
            menuNetwork.ShortcutKeys = Keys.Alt | Keys.N;
            menuNetwork.Size = new Size(76, 21);
            menuNetwork.Text = "network";
            menuNetwork.ToolTipText = "network provides ip addresses & secure things";
            // 
            // menuIItemMyIps
            // 
            menuIItemMyIps.BackColor = SystemColors.MenuBar;
            menuIItemMyIps.DropDownItems.AddRange(new ToolStripItem[] { menuItemExternalIp });
            menuIItemMyIps.Name = "menuIItemMyIps";
            menuIItemMyIps.Size = new Size(177, 22);
            menuIItemMyIps.Text = "my ip's";
            // 
            // menuItemExternalIp
            // 
            menuItemExternalIp.BackColor = SystemColors.MenuBar;
            menuItemExternalIp.Name = "menuItemExternalIp";
            menuItemExternalIp.ShortcutKeys = Keys.Alt | Keys.E;
            menuItemExternalIp.Size = new Size(206, 22);
            menuItemExternalIp.Text = "External Ip's";
            // 
            // menuItemFriendIp
            // 
            menuItemFriendIp.BackColor = SystemColors.MenuBar;
            menuItemFriendIp.DropDownItems.AddRange(new ToolStripItem[] { menuItempComboBoxFriendIp });
            menuItemFriendIp.Name = "menuItemFriendIp";
            menuItemFriendIp.Size = new Size(177, 22);
            menuItemFriendIp.Text = "friend ip's";
            menuItemFriendIp.ToolTipText = "You can enter here directly friend ip's, if your connection is free of SNAT/DNAT";
            // 
            // menuItempComboBoxFriendIp
            // 
            menuItempComboBoxFriendIp.BackColor = SystemColors.ControlLightLight;
            menuItempComboBoxFriendIp.Name = "menuItempComboBoxFriendIp";
            menuItempComboBoxFriendIp.Size = new Size(121, 23);
            // 
            // menuItemProxyServers
            // 
            menuItemProxyServers.BackColor = SystemColors.MenuBar;
            menuItemProxyServers.Name = "menuItemProxyServers";
            menuItemProxyServers.Size = new Size(177, 22);
            menuItemProxyServers.Text = "proxies";
            menuItemProxyServers.ToolTipText = "proxies are needed mainly to connect to people, where no endpoint to endpoint ip connection is possible";
            // 
            // menuIPsSeparator
            // 
            menuIPsSeparator.BackColor = SystemColors.MenuBar;
            menuIPsSeparator.ForeColor = SystemColors.ActiveBorder;
            menuIPsSeparator.Name = "menuIPsSeparator";
            menuIPsSeparator.Size = new Size(174, 6);
            // 
            // menuItemIPv6Secure
            // 
            menuItemIPv6Secure.BackColor = SystemColors.MenuBar;
            menuItemIPv6Secure.Name = "menuItemIPv6Secure";
            menuItemIPv6Secure.ShortcutKeys = Keys.Control | Keys.D6;
            menuItemIPv6Secure.Size = new Size(177, 22);
            menuItemIPv6Secure.Text = "ip6 cqr";
            menuItemIPv6Secure.ToolTipText = "you can check it only, when you have an ipv6 address and you want to chat only to partners, where ip6 connect is possible";
            // 
            // toolMenuConnect
            // 
            toolMenuConnect.BackColor = SystemColors.MenuBar;
            toolMenuConnect.DropDownItems.AddRange(new ToolStripItem[] { menuConnectItemFriend, menuConnectSeparator, menuConnectComboBoxIps, menuConnectSeparatorLast, menuConnectItemLoopback });
            toolMenuConnect.Name = "toolMenuConnect";
            toolMenuConnect.Size = new Size(74, 21);
            toolMenuConnect.Text = "connect";
            toolMenuConnect.ToolTipText = "connects directly to an peered ip address or contact over a proxy";
            // 
            // menuConnectItemFriend
            // 
            menuConnectItemFriend.BackColor = SystemColors.Menu;
            menuConnectItemFriend.Name = "menuConnectItemFriend";
            menuConnectItemFriend.ShortcutKeys = Keys.Control | Keys.F;
            menuConnectItemFriend.Size = new Size(275, 22);
            menuConnectItemFriend.Text = "friend (over proxy)";
            menuConnectItemFriend.ToolTipText = "connects to a friend over proxy server";
            // 
            // menuConnectSeparator
            // 
            menuConnectSeparator.Name = "menuConnectSeparator";
            menuConnectSeparator.Size = new Size(272, 6);
            // 
            // menuConnectComboBoxIps
            // 
            menuConnectComboBoxIps.Name = "menuConnectComboBoxIps";
            menuConnectComboBoxIps.Size = new Size(144, 23);
            // 
            // menuConnectSeparatorLast
            // 
            menuConnectSeparatorLast.Name = "menuConnectSeparatorLast";
            menuConnectSeparatorLast.Size = new Size(272, 6);
            // 
            // menuConnectItemLoopback
            // 
            menuConnectItemLoopback.BackColor = SystemColors.Menu;
            menuConnectItemLoopback.Name = "menuConnectItemLoopback";
            menuConnectItemLoopback.ShortcutKeys = Keys.Control | Keys.D0;
            menuConnectItemLoopback.Size = new Size(275, 22);
            menuConnectItemLoopback.Text = "me myself (loopback)";
            menuConnectItemLoopback.ToolTipText = "import contacts from address book";
            // 
            // menuCommands
            // 
            menuCommands.BackColor = SystemColors.MenuBar;
            menuCommands.DropDownItems.AddRange(new ToolStripItem[] { menuItemSend, menuItemAttach, menuCommandsSeperator, menuItemRefresh, menuItemClear });
            menuCommands.Name = "menuCommands";
            menuCommands.Size = new Size(128, 21);
            menuCommands.Text = "chat commands";
            menuCommands.ToolTipText = "possible chat commands";
            // 
            // menuItemSend
            // 
            menuItemSend.BackColor = SystemColors.MenuBar;
            menuItemSend.Name = "menuItemSend";
            menuItemSend.ShortcutKeys = Keys.Control | Keys.S;
            menuItemSend.Size = new Size(178, 22);
            menuItemSend.Text = "send";
            menuItemSend.ToolTipText = "sends a message";
            menuItemSend.Click += MenuItemSend_Click;
            // 
            // menuItemAttach
            // 
            menuItemAttach.BackColor = SystemColors.MenuBar;
            menuItemAttach.Name = "menuItemAttach";
            menuItemAttach.ShortcutKeys = Keys.Control | Keys.A;
            menuItemAttach.Size = new Size(178, 22);
            menuItemAttach.Text = "attach";
            menuItemAttach.ToolTipText = "attaches file to send; in prototype only  file extension image audio and video is supported";
            menuItemAttach.Click += MenuItemAttach_Click;
            // 
            // menuCommandsSeperator
            // 
            menuCommandsSeperator.BackColor = SystemColors.MenuBar;
            menuCommandsSeperator.Name = "menuCommandsSeperator";
            menuCommandsSeperator.Size = new Size(175, 6);
            // 
            // menuItemRefresh
            // 
            menuItemRefresh.BackColor = SystemColors.MenuBar;
            menuItemRefresh.Name = "menuItemRefresh";
            menuItemRefresh.ShortcutKeys = Keys.Control | Keys.R;
            menuItemRefresh.Size = new Size(178, 22);
            menuItemRefresh.Text = "refresh";
            menuItemRefresh.ToolTipText = "refreshes, when the terminal is flushed";
            menuItemRefresh.Click += MenuItemRefresh_Click;
            // 
            // menuItemClear
            // 
            menuItemClear.BackColor = SystemColors.MenuBar;
            menuItemClear.Name = "menuItemClear";
            menuItemClear.ShortcutKeys = Keys.Control | Keys.Delete;
            menuItemClear.Size = new Size(178, 22);
            menuItemClear.Text = "clear";
            menuItemClear.ToolTipText = "clears completey all chat windows";
            menuItemClear.Click += MenuItemClear_Click;
            // 
            // menuContacts
            // 
            menuContacts.BackColor = SystemColors.MenuBar;
            menuContacts.DropDownItems.AddRange(new ToolStripItem[] { menuContactsItemMe, menuContactsItemAdd, menuContactsItemView, menuContactsSeparetor, menuContactstemImport, menuContactstemExport });
            menuContacts.Name = "menuContacts";
            menuContacts.Size = new Size(77, 21);
            menuContacts.Text = "contacts";
            // 
            // menuContactsItemMe
            // 
            menuContactsItemMe.BackColor = SystemColors.Menu;
            menuContactsItemMe.Name = "menuContactsItemMe";
            menuContactsItemMe.ShortcutKeys = Keys.Alt | Keys.M;
            menuContactsItemMe.Size = new Size(233, 22);
            menuContactsItemMe.Text = "me myself mine";
            menuContactsItemMe.ToolTipText = "edits my contact";
            menuContactsItemMe.Click += MenuItemMyContact_Click;
            // 
            // menuContactsItemAdd
            // 
            menuContactsItemAdd.BackColor = SystemColors.Menu;
            menuContactsItemAdd.Name = "menuContactsItemAdd";
            menuContactsItemAdd.ShortcutKeys = Keys.Alt | Keys.A;
            menuContactsItemAdd.Size = new Size(233, 22);
            menuContactsItemAdd.Text = "add contact";
            menuContactsItemAdd.ToolTipText = "adds a friend contact to cqr chat";
            menuContactsItemAdd.Click += MenuItemAddContact_Click;
            // 
            // menuContactsItemView
            // 
            menuContactsItemView.BackColor = SystemColors.Menu;
            menuContactsItemView.Name = "menuContactsItemView";
            menuContactsItemView.ShortcutKeys = Keys.Alt | Keys.V;
            menuContactsItemView.Size = new Size(233, 22);
            menuContactsItemView.Text = "view contacts";
            menuContactsItemView.ToolTipText = "view all added and imported contacts";
            menuContactsItemView.Click += MenuContactsItemView_Click;
            // 
            // menuContactsSeparetor
            // 
            menuContactsSeparetor.Name = "menuContactsSeparetor";
            menuContactsSeparetor.Size = new Size(230, 6);
            // 
            // menuContactstemImport
            // 
            menuContactstemImport.BackColor = SystemColors.Menu;
            menuContactstemImport.Name = "menuContactstemImport";
            menuContactstemImport.ShortcutKeys = Keys.Alt | Keys.I;
            menuContactstemImport.Size = new Size(233, 22);
            menuContactstemImport.Text = "import contacts";
            menuContactstemImport.ToolTipText = "import contacts from address book";
            menuContactstemImport.Click += MenuContactstemImport_Click;
            // 
            // menuContactstemExport
            // 
            menuContactstemExport.BackColor = SystemColors.Menu;
            menuContactstemExport.Name = "menuContactstemExport";
            menuContactstemExport.ShortcutKeys = Keys.Alt | Keys.E;
            menuContactstemExport.Size = new Size(233, 22);
            menuContactstemExport.Text = "export contacts";
            menuContactstemExport.ToolTipText = "export contacts to a json file";
            // 
            // menuQuestionMark
            // 
            menuQuestionMark.BackColor = SystemColors.MenuBar;
            menuQuestionMark.BackgroundImageLayout = ImageLayout.None;
            menuQuestionMark.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuQuestionMark.DropDownItems.AddRange(new ToolStripItem[] { menuItemHelp, menuItemInfo, menuItemAbout });
            menuQuestionMark.ForeColor = SystemColors.MenuText;
            menuQuestionMark.ImageScaling = ToolStripItemImageScaling.None;
            menuQuestionMark.Name = "menuQuestionMark";
            menuQuestionMark.Padding = new Padding(3, 0, 3, 0);
            menuQuestionMark.ShortcutKeys = Keys.Alt | Keys.F7;
            menuQuestionMark.Size = new Size(24, 21);
            menuQuestionMark.Text = "?";
            // 
            // menuItemHelp
            // 
            menuItemHelp.BackColor = SystemColors.MenuBar;
            menuItemHelp.BackgroundImageLayout = ImageLayout.None;
            menuItemHelp.ForeColor = SystemColors.MenuText;
            menuItemHelp.Name = "menuItemHelp";
            menuItemHelp.Padding = new Padding(0, 2, 0, 2);
            menuItemHelp.ShortcutKeys = Keys.Control | Keys.F1;
            menuItemHelp.Size = new Size(167, 24);
            menuItemHelp.Text = "help";
            menuItemHelp.ToolTipText = "displays help";
            menuItemHelp.Click += MenuItemHelp_Click;
            // 
            // menuItemInfo
            // 
            menuItemInfo.BackColor = SystemColors.MenuBar;
            menuItemInfo.BackgroundImageLayout = ImageLayout.None;
            menuItemInfo.ForeColor = SystemColors.MenuText;
            menuItemInfo.Name = "menuItemInfo";
            menuItemInfo.Size = new Size(167, 22);
            menuItemInfo.Text = "info";
            menuItemInfo.TextImageRelation = TextImageRelation.TextAboveImage;
            menuItemInfo.ToolTipText = "displays a tiny message box with version info";
            menuItemInfo.Click += MenuItemInfo_Click;
            // 
            // menuItemAbout
            // 
            menuItemAbout.BackColor = SystemColors.MenuBar;
            menuItemAbout.BackgroundImageLayout = ImageLayout.None;
            menuItemAbout.ForeColor = SystemColors.MenuText;
            menuItemAbout.Name = "menuItemAbout";
            menuItemAbout.Padding = new Padding(0, 2, 0, 2);
            menuItemAbout.Size = new Size(167, 24);
            menuItemAbout.Text = "about";
            menuItemAbout.TextImageRelation = TextImageRelation.TextAboveImage;
            menuItemAbout.ToolTipText = "displays a large modal dialog with version info and  copy left info";
            menuItemAbout.Click += MenuItemAbout_Click;
            // 
            // menuEdit
            // 
            menuEdit.BackColor = SystemColors.MenuBar;
            menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuEditItemCut, menuEditItemCopy, menuIEdittemPaste, menuEditItemSelectAll });
            menuEdit.Enabled = false;
            menuEdit.Name = "menuEdit";
            menuEdit.Size = new Size(46, 21);
            menuEdit.Text = "Edit";
            // 
            // menuEditItemCut
            // 
            menuEditItemCut.BackColor = SystemColors.Menu;
            menuEditItemCut.Enabled = false;
            menuEditItemCut.Name = "menuEditItemCut";
            menuEditItemCut.ShortcutKeys = Keys.Control | Keys.X;
            menuEditItemCut.Size = new Size(164, 22);
            menuEditItemCut.Text = "Cat ✂";
            // 
            // menuEditItemCopy
            // 
            menuEditItemCopy.BackColor = SystemColors.Menu;
            menuEditItemCopy.Enabled = false;
            menuEditItemCopy.Name = "menuEditItemCopy";
            menuEditItemCopy.ShortcutKeys = Keys.Control | Keys.C;
            menuEditItemCopy.Size = new Size(164, 22);
            menuEditItemCopy.Text = "Copy";
            // 
            // menuIEdittemPaste
            // 
            menuIEdittemPaste.BackColor = SystemColors.Menu;
            menuIEdittemPaste.Enabled = false;
            menuIEdittemPaste.Name = "menuIEdittemPaste";
            menuIEdittemPaste.ShortcutKeys = Keys.Control | Keys.V;
            menuIEdittemPaste.Size = new Size(164, 22);
            menuIEdittemPaste.Text = "Paste";
            // 
            // menuEditItemSelectAll
            // 
            menuEditItemSelectAll.BackColor = SystemColors.Menu;
            menuEditItemSelectAll.Enabled = false;
            menuEditItemSelectAll.Name = "menuEditItemSelectAll";
            menuEditItemSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            menuEditItemSelectAll.Size = new Size(164, 22);
            menuEditItemSelectAll.Text = "Select All";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            openFileDialog.Title = "OpenFileDialog";
            // 
            // saveFileDialog
            // 
            saveFileDialog.InitialDirectory = "C:\\Windows\\Temp";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.ShowHiddenFiles = true;
            saveFileDialog.SupportMultiDottedExtensions = true;
            saveFileDialog.Title = "Save File";
            // 
            // statusStrip
            // 
            statusStrip.GripMargin = new Padding(1);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripProgressBar, toolStripStatusLabel });
            statusStrip.Location = new Point(0, 689);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(976, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Margin = new Padding(1);
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(300, 20);
            toolStripProgressBar.Step = 4;
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Margin = new Padding(0, 2, 0, 1);
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(659, 19);
            toolStripStatusLabel.Spring = true;
            toolStripStatusLabel.Text = "Status";
            // 
            // SplitChatView
            // 
            SplitChatView.BackColor = SystemColors.ControlLight;
            SplitChatView.IsSplitterFixed = true;
            SplitChatView.Location = new Point(0, 0);
            SplitChatView.Margin = new Padding(0);
            SplitChatView.MaximumSize = new Size(800, 600);
            SplitChatView.MinimumSize = new Size(600, 400);
            SplitChatView.Name = "SplitChatView";
            // 
            // SplitChatView.Panel1
            // 
            SplitChatView.Panel1.AllowDrop = true;
            SplitChatView.Panel1.BackgroundImageLayout = ImageLayout.None;
            SplitChatView.Panel1.Controls.Add(TextBoxSource);
            SplitChatView.Panel1MinSize = 300;
            // 
            // SplitChatView.Panel2
            // 
            SplitChatView.Panel2.BackgroundImageLayout = ImageLayout.None;
            SplitChatView.Panel2.Controls.Add(TextBoxDestionation);
            SplitChatView.Panel2MinSize = 300;
            SplitChatView.Size = new Size(800, 460);
            SplitChatView.SplitterDistance = 396;
            SplitChatView.SplitterIncrement = 8;
            SplitChatView.SplitterWidth = 8;
            SplitChatView.TabIndex = 31;
            SplitChatView.TabStop = false;
            // 
            // TextBoxSource
            // 
            TextBoxSource.BackColor = SystemColors.GradientActiveCaption;
            TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSource.Dock = DockStyle.Fill;
            TextBoxSource.Font = new Font("Lucida Sans Unicode", 10F);
            TextBoxSource.Location = new Point(0, 0);
            TextBoxSource.Margin = new Padding(1);
            TextBoxSource.MaxLength = 65536;
            TextBoxSource.Multiline = true;
            TextBoxSource.Name = "TextBoxSource";
            TextBoxSource.ScrollBars = ScrollBars.Both;
            TextBoxSource.Size = new Size(396, 460);
            TextBoxSource.TabIndex = 32;
            // 
            // TextBoxDestionation
            // 
            TextBoxDestionation.BackColor = SystemColors.GradientInactiveCaption;
            TextBoxDestionation.BorderStyle = BorderStyle.FixedSingle;
            TextBoxDestionation.Dock = DockStyle.Fill;
            TextBoxDestionation.Font = new Font("Lucida Sans Unicode", 10F);
            TextBoxDestionation.Location = new Point(0, 0);
            TextBoxDestionation.Margin = new Padding(1);
            TextBoxDestionation.MaxLength = 65536;
            TextBoxDestionation.Multiline = true;
            TextBoxDestionation.Name = "TextBoxDestionation";
            TextBoxDestionation.ScrollBars = ScrollBars.Both;
            TextBoxDestionation.Size = new Size(396, 460);
            TextBoxDestionation.TabIndex = 33;
            // 
            // PictureBoxYou
            // 
            PictureBoxYou.BackgroundImageLayout = ImageLayout.Stretch;
            PictureBoxYou.Location = new Point(2, 1);
            PictureBoxYou.Margin = new Padding(1);
            PictureBoxYou.Name = "PictureBoxYou";
            PictureBoxYou.Padding = new Padding(1);
            PictureBoxYou.Size = new Size(148, 148);
            PictureBoxYou.TabIndex = 71;
            PictureBoxYou.TabStop = false;
            // 
            // ButtonKey
            // 
            ButtonKey.BackColor = SystemColors.ButtonHighlight;
            ButtonKey.BackgroundImageLayout = ImageLayout.Center;
            ButtonKey.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            ButtonKey.ForeColor = SystemColors.ActiveCaptionText;
            ButtonKey.Image = Properties.Resources.a_right_key;
            ButtonKey.Location = new Point(254, 4);
            ButtonKey.Margin = new Padding(1);
            ButtonKey.Name = "ButtonKey";
            ButtonKey.Padding = new Padding(1);
            ButtonKey.Size = new Size(40, 27);
            ButtonKey.TabIndex = 12;
            ButtonKey.UseVisualStyleBackColor = false;
            ButtonKey.Click += ButtonKey_Click;
            // 
            // PanelEnCodeCrypt
            // 
            PanelEnCodeCrypt.BackColor = SystemColors.ActiveCaption;
            PanelEnCodeCrypt.Controls.Add(ButtonCheck);
            PanelEnCodeCrypt.Controls.Add(ComboBoxSecretKey);
            PanelEnCodeCrypt.Controls.Add(ComboBoxIpContact);
            PanelEnCodeCrypt.Controls.Add(TextBoxPipe);
            PanelEnCodeCrypt.Controls.Add(ButtonKey);
            PanelEnCodeCrypt.ForeColor = SystemColors.WindowText;
            PanelEnCodeCrypt.Location = new Point(0, 28);
            PanelEnCodeCrypt.Margin = new Padding(0);
            PanelEnCodeCrypt.Name = "PanelEnCodeCrypt";
            PanelEnCodeCrypt.Size = new Size(976, 36);
            PanelEnCodeCrypt.TabIndex = 10;
            // 
            // ButtonCheck
            // 
            ButtonCheck.BackColor = SystemColors.ButtonHighlight;
            ButtonCheck.BackgroundImageLayout = ImageLayout.Center;
            ButtonCheck.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            ButtonCheck.ForeColor = SystemColors.ActiveCaptionText;
            ButtonCheck.Image = Properties.Resources.a_right_key;
            ButtonCheck.Location = new Point(768, 4);
            ButtonCheck.Margin = new Padding(1);
            ButtonCheck.Name = "ButtonCheck";
            ButtonCheck.Padding = new Padding(1);
            ButtonCheck.Size = new Size(40, 27);
            ButtonCheck.TabIndex = 17;
            ButtonCheck.UseVisualStyleBackColor = false;
            // 
            // ComboBoxSecretKey
            // 
            ComboBoxSecretKey.BackColor = SystemColors.ControlLightLight;
            ComboBoxSecretKey.ForeColor = SystemColors.ControlText;
            ComboBoxSecretKey.FormattingEnabled = true;
            ComboBoxSecretKey.Location = new Point(10, 6);
            ComboBoxSecretKey.Margin = new Padding(1);
            ComboBoxSecretKey.Name = "ComboBoxSecretKey";
            ComboBoxSecretKey.Size = new Size(237, 24);
            ComboBoxSecretKey.TabIndex = 11;
            ComboBoxSecretKey.Text = "[enter secret key here]";
            ComboBoxSecretKey.SelectedIndexChanged += ComboBoxSecretKey_SelectedIndexChanged;
            ComboBoxSecretKey.TextUpdate += ComboBoxSecretKey_TextUpdate;
            ComboBoxSecretKey.Leave += ComboBoxSecretKey_FocusLeave;
            // 
            // ComboBoxIpContact
            // 
            ComboBoxIpContact.BackColor = SystemColors.ControlLightLight;
            ComboBoxIpContact.Font = new Font("Lucida Sans Unicode", 10F);
            ComboBoxIpContact.ForeColor = SystemColors.ControlText;
            ComboBoxIpContact.FormattingEnabled = true;
            ComboBoxIpContact.Location = new Point(412, 6);
            ComboBoxIpContact.Margin = new Padding(1);
            ComboBoxIpContact.Name = "ComboBoxIpContact";
            ComboBoxIpContact.Size = new Size(348, 24);
            ComboBoxIpContact.TabIndex = 15;
            ComboBoxIpContact.Text = "[enter peer IPv4 or IPv6 for directly connect]";
            ComboBoxIpContact.SelectedIndexChanged += ComboBoxIpContact_SelectedIndexChanged;
            ComboBoxIpContact.TextUpdate += ComboBoxIpContact_TextUpdate;
            ComboBoxIpContact.Leave += ComboBoxIpContact_FocusLeave;
            // 
            // TextBoxPipe
            // 
            TextBoxPipe.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextBoxPipe.HideSelection = false;
            TextBoxPipe.Location = new Point(304, 5);
            TextBoxPipe.Margin = new Padding(1);
            TextBoxPipe.Name = "TextBoxPipe";
            TextBoxPipe.ReadOnly = true;
            TextBoxPipe.Size = new Size(100, 26);
            TextBoxPipe.TabIndex = 13;
            // 
            // RichTextBoxChat
            // 
            RichTextBoxChat.BorderStyle = BorderStyle.FixedSingle;
            RichTextBoxChat.ForeColor = SystemColors.WindowText;
            RichTextBoxChat.Location = new Point(0, 0);
            RichTextBoxChat.Margin = new Padding(1);
            RichTextBoxChat.Name = "RichTextBoxChat";
            RichTextBoxChat.Size = new Size(824, 136);
            RichTextBoxChat.TabIndex = 41;
            RichTextBoxChat.Text = "";
            // 
            // PictureBoxPartner
            // 
            PictureBoxPartner.Location = new Point(2, 366);
            PictureBoxPartner.Margin = new Padding(1);
            PictureBoxPartner.Name = "PictureBoxPartner";
            PictureBoxPartner.Padding = new Padding(1);
            PictureBoxPartner.Size = new Size(148, 148);
            PictureBoxPartner.TabIndex = 72;
            PictureBoxPartner.TabStop = false;
            // 
            // PanelDestination
            // 
            PanelDestination.BackColor = SystemColors.ActiveCaption;
            PanelDestination.Controls.Add(GroupBoxLinks);
            PanelDestination.Controls.Add(PictureBoxYou);
            PanelDestination.Controls.Add(PictureBoxPartner);
            PanelDestination.ForeColor = SystemColors.ActiveCaptionText;
            PanelDestination.Location = new Point(824, 32);
            PanelDestination.Margin = new Padding(0);
            PanelDestination.Name = "PanelDestination";
            PanelDestination.Size = new Size(152, 515);
            PanelDestination.TabIndex = 70;
            // 
            // GroupBoxLinks
            // 
            GroupBoxLinks.BackColor = SystemColors.ControlLightLight;
            GroupBoxLinks.Font = new Font("Lucida Sans Unicode", 9F);
            GroupBoxLinks.Location = new Point(1, 151);
            GroupBoxLinks.Margin = new Padding(1);
            GroupBoxLinks.Name = "GroupBoxLinks";
            GroupBoxLinks.Padding = new Padding(1);
            GroupBoxLinks.Size = new Size(151, 213);
            GroupBoxLinks.TabIndex = 80;
            GroupBoxLinks.TabStop = false;
            GroupBoxLinks.Text = "Attachments";
            // 
            // PanelCenter
            // 
            PanelCenter.Controls.Add(SplitChatView);
            PanelCenter.Controls.Add(RichTextBoxOneView);
            PanelCenter.Location = new Point(8, 72);
            PanelCenter.Margin = new Padding(0);
            PanelCenter.Name = "PanelCenter";
            PanelCenter.Size = new Size(800, 460);
            PanelCenter.TabIndex = 30;
            // 
            // RichTextBoxOneView
            // 
            RichTextBoxOneView.Dock = DockStyle.Fill;
            RichTextBoxOneView.Location = new Point(0, 0);
            RichTextBoxOneView.Margin = new Padding(2);
            RichTextBoxOneView.Name = "RichTextBoxOneView";
            RichTextBoxOneView.Size = new Size(800, 460);
            RichTextBoxOneView.TabIndex = 36;
            RichTextBoxOneView.Text = "";
            RichTextBoxOneView.Visible = false;
            // 
            // PanelBottom
            // 
            PanelBottom.BackColor = SystemColors.ControlLight;
            PanelBottom.Controls.Add(RichTextBoxChat);
            PanelBottom.ForeColor = SystemColors.ActiveCaptionText;
            PanelBottom.Location = new Point(0, 549);
            PanelBottom.Margin = new Padding(1);
            PanelBottom.Name = "PanelBottom";
            PanelBottom.Size = new Size(824, 136);
            PanelBottom.TabIndex = 40;
            // 
            // buttonAttach
            // 
            buttonAttach.Location = new Point(827, 552);
            buttonAttach.Margin = new Padding(2);
            buttonAttach.Name = "buttonAttach";
            buttonAttach.Padding = new Padding(1);
            buttonAttach.Size = new Size(144, 40);
            buttonAttach.TabIndex = 82;
            buttonAttach.Text = "Attach";
            buttonAttach.UseVisualStyleBackColor = true;
            buttonAttach.Click += buttonAttach_Click;
            // 
            // button1
            // 
            button1.Location = new Point(827, 598);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Padding = new Padding(1);
            button1.Size = new Size(144, 40);
            button1.TabIndex = 83;
            button1.Text = "Send";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(827, 644);
            buttonClear.Margin = new Padding(2);
            buttonClear.Name = "buttonClear";
            buttonClear.Padding = new Padding(1);
            buttonClear.Size = new Size(144, 40);
            buttonClear.TabIndex = 84;
            buttonClear.Text = "Clear";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += buttonClear_Click;
            // 
            // RichTextChat
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(976, 711);
            Controls.Add(buttonClear);
            Controls.Add(button1);
            Controls.Add(buttonAttach);
            Controls.Add(PanelCenter);
            Controls.Add(PanelDestination);
            Controls.Add(PanelBottom);
            Controls.Add(PanelEnCodeCrypt);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Font = new Font("Lucida Sans Unicode", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            Name = "RichTextChat";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "RichTextChat";
            FormClosing += FormClose_Click;
            Load += RichTextChat_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            SplitChatView.Panel1.ResumeLayout(false);
            SplitChatView.Panel1.PerformLayout();
            SplitChatView.Panel2.ResumeLayout(false);
            SplitChatView.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SplitChatView).EndInit();
            SplitChatView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PictureBoxYou).EndInit();
            PanelEnCodeCrypt.ResumeLayout(false);
            PanelEnCodeCrypt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxPartner).EndInit();
            PanelDestination.ResumeLayout(false);
            PanelCenter.ResumeLayout(false);
            PanelBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Panel PanelEnCodeCrypt;
        private Panel PanelDestination;
        private Panel PanelCenter;
        private Panel PanelBottom;
        private TextBox TextBoxPipe;
        private ComboBox ComboBoxIpContact;
        private ComboBox ComboBoxSecretKey;
        private SplitContainer SplitChatView;
        private TextBox TextBoxSource;
        private TextBox TextBoxDestionation;
        private Button ButtonKey;
        private Button ButtonCheck;
        private RichTextBox RichTextBoxChat;
        private PictureBox PictureBoxYou;
        private PictureBox PictureBoxPartner;

        internal MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private ToolStripMenuItem toolStripMenuMain;
        private ToolStripMenuItem menuItemAbout;
        private ToolStripSeparator menuIPsSeparator;
        private ToolStripMenuItem menuItemInfo;
        private ToolStripMenuItem toolStripMenuItemExit;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuCQRChat;
        private ToolStripMenuItem menuFileItemOpen;
        private ToolStripMenuItem menuFileItemSave;
        private ToolStripMenuItem menuFileItemExit;
        private ToolStripMenuItem menuQuestionMark;
        private ToolStripMenuItem menuItemHelp;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripSeparator menuFileSeparator;
        private ToolStripMenuItem menuNetwork;
        private ToolStripMenuItem menuIItemMyIps;
        private ToolStripMenuItem menuItemFriendIp;
        private ToolStripMenuItem menuItemProxyServers;
        private ToolStripComboBox menuItempComboBoxFriendIp;
        private ToolStripMenuItem menuItemIPv6Secure;
        private ToolStripMenuItem menuCommands;
        private ToolStripMenuItem menuItemSend;
        private ToolStripMenuItem menuEdit;
        private ToolStripMenuItem menuItemRefresh;
        private ToolStripMenuItem menuItemClear;
        private ToolStripMenuItem menuEditItemCut;
        private ToolStripMenuItem menuEditItemCopy;
        private ToolStripMenuItem menuIEdittemPaste;
        private ToolStripMenuItem menuEditItemSelectAll;
        private ToolStripMenuItem menuViewItemLeftRíght;
        private ToolStripMenuItem menuViewItemTopBottom;
        private ToolStripMenuItem menuViewItem1View;
        private ToolStripMenuItem menuContacts;
        private ToolStripMenuItem menuContactstemImport;
        private ToolStripMenuItem menuContactsItemAdd;
        private ToolStripMenuItem menuContactsItemView;
        private ToolStripMenuItem menuContactsItemMe;
        private RichTextBox RichTextBoxOneView;
        private ToolStripMenuItem menuItemExternalIp;
        private ToolStripSeparator menuContactsSeparetor;
        private ToolStripMenuItem menuContactstemExport;
        private ToolStripMenuItem menuCqrChatItemPerist;
        private ToolStripSeparator meuFileToExitSperator;
        private ToolStripMenuItem menuItemAttach;
        private ToolStripSeparator menuCommandsSeperator;
        private ToolStripMenuItem toolMenuConnect;
        private ToolStripMenuItem menuConnectItemFriend;
        private ToolStripMenuItem menuConnectItemLoopback;
        private ToolStripComboBox menuConnectComboBoxIps;
        private ToolStripSeparator menuConnectSeparator;
        private ToolStripSeparator menuConnectSeparatorLast;

        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        private Button buttonAttach;
        private Controls.GroupBoxLinkLabels GroupBoxLinks;
        private Button button1;
        private Button buttonClear;

    }

}