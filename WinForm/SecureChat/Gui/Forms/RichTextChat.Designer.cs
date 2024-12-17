
using Area23.At.Framework.Library.Core.Net.WebHttp;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextChat));
            menuStrip = new MenuStrip();
            toolStripMenuFile = new ToolStripMenuItem();
            menuFileItemOpen = new ToolStripMenuItem();
            menuFileItemSave = new ToolStripMenuItem();
            menuFileSeparator = new ToolStripSeparator();
            menuFileItemExit = new ToolStripMenuItem();
            menuEdit = new ToolStripMenuItem();
            menuEditItemCut = new ToolStripMenuItem();
            menuEditItemCopy = new ToolStripMenuItem();
            menuIEdittemPaste = new ToolStripMenuItem();
            menuEditItemSelectAll = new ToolStripMenuItem();
            menuView = new ToolStripMenuItem();
            menuViewItemLeftRíght = new ToolStripMenuItem();
            menuViewItemTopBottom = new ToolStripMenuItem();
            menuIViewItem1View = new ToolStripMenuItem();
            menuIPs = new ToolStripMenuItem();
            menuIItemMyIps = new ToolStripMenuItem();
            menuItemFriendIp = new ToolStripMenuItem();
            menuItempComboBoxFriendIp = new ToolStripComboBox();
            menuIPsSeparator = new ToolStripSeparator();
            menuItemProxyServers = new ToolStripMenuItem();
            menuItemComboBoxProxies = new ToolStripComboBox();
            menuItemIPv6Secure = new ToolStripMenuItem();
            menuCommands = new ToolStripMenuItem();
            menuItemSend = new ToolStripMenuItem();
            menuItemRefresh = new ToolStripMenuItem();
            menuItemClear = new ToolStripMenuItem();
            menuOptions = new ToolStripMenuItem();
            menuItemMyContact = new ToolStripMenuItem();
            menuItemAddContact = new ToolStripMenuItem();
            menuItemImportContacts = new ToolStripMenuItem();
            menuItemViewContacts = new ToolStripMenuItem();
            menuQuestionMark = new ToolStripMenuItem();
            menuItemAbout = new ToolStripMenuItem();
            menuItemHelp = new ToolStripMenuItem();
            menuItemInfo = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            statusStrip = new StatusStrip();
            toolStripSplitButton = new ToolStripSplitButton();
            splitButtonMenuItemLoad = new ToolStripMenuItem();
            splitButtonMenuItemSave = new ToolStripMenuItem();
            toolStripProgressBar = new ToolStripProgressBar();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripComboBox1 = new ToolStripComboBox();
            panelSource = new Panel();
            pictureBoxYou = new PictureBox();
            pictureBoxSource = new PictureBox();
            buttonSecretKey = new Button();
            buttonAddToPipeline = new Button();
            ComboBox_RemoteEndPoint = new ComboBox();
            ComboBox_LocalEndPoint = new ComboBox();
            buttonHashIv = new Button();
            panelEnCodeCrypt = new Panel();
            pictureBoxDestination = new PictureBox();
            richTextBoxChat = new RichTextBox();
            pictureBoxPartner = new PictureBox();
            panelDestination = new Panel();
            buttonExit = new Button();
            panelCenter = new Panel();
            richTextBoxOneView = new RichTextBox();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            panelSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxYou).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            panelEnCodeCrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDestination).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPartner).BeginInit();
            panelDestination.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.Transparent;
            menuStrip.Font = new Font("Lucida Sans Unicode", 10F);
            menuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuFile, menuEdit, menuView, menuIPs, menuCommands, menuOptions, menuQuestionMark });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(976, 25);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // toolStripMenuFile
            // 
            toolStripMenuFile.BackColor = SystemColors.MenuBar;
            toolStripMenuFile.DropDownItems.AddRange(new ToolStripItem[] { menuFileItemOpen, menuFileItemSave, menuFileSeparator, menuFileItemExit });
            toolStripMenuFile.ForeColor = SystemColors.MenuText;
            toolStripMenuFile.Name = "toolStripMenuFile";
            toolStripMenuFile.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuFile.ShortcutKeys = Keys.Alt | Keys.F;
            toolStripMenuFile.Size = new Size(42, 21);
            toolStripMenuFile.Text = "File";
            // 
            // menuFileItemOpen
            // 
            menuFileItemOpen.AutoToolTip = true;
            menuFileItemOpen.BackColor = SystemColors.Menu;
            menuFileItemOpen.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemOpen.ForeColor = SystemColors.MenuText;
            menuFileItemOpen.Margin = new Padding(1);
            menuFileItemOpen.Name = "menuFileItemOpen";
            menuFileItemOpen.ShortcutKeys = Keys.Control | Keys.O;
            menuFileItemOpen.Size = new Size(168, 22);
            menuFileItemOpen.Text = "Open";
            menuFileItemOpen.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemOpen.ToolTipText = "Open file";
            menuFileItemOpen.Click += toolStripMenuItemLoad_Click;
            // 
            // menuFileItemSave
            // 
            menuFileItemSave.AutoToolTip = true;
            menuFileItemSave.BackColor = SystemColors.Menu;
            menuFileItemSave.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemSave.ForeColor = SystemColors.MenuText;
            menuFileItemSave.Margin = new Padding(1);
            menuFileItemSave.Name = "menuFileItemSave";
            menuFileItemSave.ShortcutKeys = Keys.Control | Keys.S;
            menuFileItemSave.Size = new Size(168, 22);
            menuFileItemSave.Text = "Save";
            menuFileItemSave.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemSave.ToolTipText = "Save file";
            menuFileItemSave.Click += toolStripMenuItemSave_Click;
            // 
            // menuFileSeparator
            // 
            menuFileSeparator.BackColor = SystemColors.Menu;
            menuFileSeparator.ForeColor = SystemColors.MenuText;
            menuFileSeparator.Margin = new Padding(1);
            menuFileSeparator.Name = "menuFileSeparator";
            menuFileSeparator.Size = new Size(165, 6);
            // 
            // menuFileItemExit
            // 
            menuFileItemExit.BackColor = SystemColors.Menu;
            menuFileItemExit.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemExit.ForeColor = SystemColors.MenuText;
            menuFileItemExit.Name = "menuFileItemExit";
            menuFileItemExit.ShortcutKeys = Keys.Alt | Keys.X;
            menuFileItemExit.Size = new Size(168, 22);
            menuFileItemExit.Text = "Exit";
            menuFileItemExit.Click += menuFileItemExit_Click;
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
            menuEditItemCut.Size = new Size(192, 22);
            menuEditItemCut.Text = "Cat ✂";
            // 
            // menuEditItemCopy
            // 
            menuEditItemCopy.BackColor = SystemColors.Menu;
            menuEditItemCopy.Enabled = false;
            menuEditItemCopy.Name = "menuEditItemCopy";
            menuEditItemCopy.ShortcutKeys = Keys.Control | Keys.C;
            menuEditItemCopy.Size = new Size(192, 22);
            menuEditItemCopy.Text = "Copy";
            // 
            // menuIEdittemPaste
            // 
            menuIEdittemPaste.BackColor = SystemColors.Menu;
            menuIEdittemPaste.Enabled = false;
            menuIEdittemPaste.Name = "menuIEdittemPaste";
            menuIEdittemPaste.ShortcutKeys = Keys.Control | Keys.V;
            menuIEdittemPaste.Size = new Size(192, 22);
            menuIEdittemPaste.Text = "Paste";
            // 
            // menuEditItemSelectAll
            // 
            menuEditItemSelectAll.BackColor = SystemColors.Menu;
            menuEditItemSelectAll.Enabled = false;
            menuEditItemSelectAll.Name = "menuEditItemSelectAll";
            menuEditItemSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            menuEditItemSelectAll.Size = new Size(192, 22);
            menuEditItemSelectAll.Text = "Select All";
            // 
            // menuView
            // 
            menuView.BackColor = SystemColors.MenuBar;
            menuView.BackgroundImageLayout = ImageLayout.None;
            menuView.DropDownItems.AddRange(new ToolStripItem[] { menuViewItemLeftRíght, menuViewItemTopBottom, menuIViewItem1View });
            menuView.ForeColor = SystemColors.MenuText;
            menuView.ImageScaling = ToolStripItemImageScaling.None;
            menuView.Name = "menuView";
            menuView.Padding = new Padding(3, 0, 3, 0);
            menuView.ShortcutKeys = Keys.Alt | Keys.V;
            menuView.Size = new Size(50, 21);
            menuView.Text = "View";
            // 
            // menuViewItemLeftRíght
            // 
            menuViewItemLeftRíght.Name = "menuViewItemLeftRíght";
            menuViewItemLeftRíght.Size = new Size(160, 22);
            menuViewItemLeftRíght.Text = "Left-Ríght";
            menuViewItemLeftRíght.Click += menuViewItemLeftRíght_Click;
            // 
            // menuViewItemTopBottom
            // 
            menuViewItemTopBottom.Name = "menuViewItemTopBottom";
            menuViewItemTopBottom.Size = new Size(160, 22);
            menuViewItemTopBottom.Text = "Top-Bottom";
            menuViewItemTopBottom.Click += menuViewItemTopBottom_Click;
            // 
            // menuIViewItem1View
            // 
            menuIViewItem1View.Name = "menuIViewItem1View";
            menuIViewItem1View.Size = new Size(160, 22);
            menuIViewItem1View.Text = "1-View";
            menuIViewItem1View.Click += menuIViewItem1View_Click;
            // 
            // menuIPs
            // 
            menuIPs.BackColor = SystemColors.MenuBar;
            menuIPs.DropDownItems.AddRange(new ToolStripItem[] { menuIItemMyIps, menuItemFriendIp, menuIPsSeparator, menuItemProxyServers, menuItemIPv6Secure });
            menuIPs.Name = "menuIPs";
            menuIPs.ShortcutKeys = Keys.Alt | Keys.I;
            menuIPs.Size = new Size(107, 21);
            menuIPs.Text = "IP Addresses";
            // 
            // menuIItemMyIps
            // 
            menuIItemMyIps.BackColor = SystemColors.Menu;
            menuIItemMyIps.Name = "menuIItemMyIps";
            menuIItemMyIps.ShortcutKeys = Keys.Alt | Keys.M;
            menuIItemMyIps.Size = new Size(215, 22);
            menuIItemMyIps.Text = "My Ip's";
            // 
            // menuItemFriendIp
            // 
            menuItemFriendIp.BackColor = SystemColors.Menu;
            menuItemFriendIp.DropDownItems.AddRange(new ToolStripItem[] { menuItempComboBoxFriendIp });
            menuItemFriendIp.Name = "menuItemFriendIp";
            menuItemFriendIp.ShortcutKeys = Keys.Alt | Keys.F;
            menuItemFriendIp.Size = new Size(215, 22);
            menuItemFriendIp.Text = "Friend Ip";
            // 
            // menuItempComboBoxFriendIp
            // 
            menuItempComboBoxFriendIp.BackColor = SystemColors.ControlLightLight;
            menuItempComboBoxFriendIp.Name = "menuItempComboBoxFriendIp";
            menuItempComboBoxFriendIp.Size = new Size(121, 23);
            // 
            // menuIPsSeparator
            // 
            menuIPsSeparator.BackColor = SystemColors.Menu;
            menuIPsSeparator.ForeColor = SystemColors.ActiveBorder;
            menuIPsSeparator.Name = "menuIPsSeparator";
            menuIPsSeparator.Size = new Size(212, 6);
            // 
            // menuItemProxyServers
            // 
            menuItemProxyServers.BackColor = SystemColors.Menu;
            menuItemProxyServers.DropDownItems.AddRange(new ToolStripItem[] { menuItemComboBoxProxies });
            menuItemProxyServers.Name = "menuItemProxyServers";
            menuItemProxyServers.ShortcutKeys = Keys.Alt | Keys.P;
            menuItemProxyServers.Size = new Size(215, 22);
            menuItemProxyServers.Text = "Proxy Servers";
            // 
            // menuItemComboBoxProxies
            // 
            menuItemComboBoxProxies.BackColor = SystemColors.ControlLightLight;
            menuItemComboBoxProxies.Name = "menuItemComboBoxProxies";
            menuItemComboBoxProxies.Size = new Size(121, 23);
            // 
            // menuItemIPv6Secure
            // 
            menuItemIPv6Secure.BackColor = SystemColors.Menu;
            menuItemIPv6Secure.Name = "menuItemIPv6Secure";
            menuItemIPv6Secure.Size = new Size(215, 22);
            menuItemIPv6Secure.Text = "IPv6 Secure";
            // 
            // menuCommands
            // 
            menuCommands.BackColor = SystemColors.MenuBar;
            menuCommands.DropDownItems.AddRange(new ToolStripItem[] { menuItemSend, menuItemRefresh, menuItemClear });
            menuCommands.Name = "menuCommands";
            menuCommands.Size = new Size(134, 21);
            menuCommands.Text = "Chat Commands";
            // 
            // menuItemSend
            // 
            menuItemSend.BackColor = SystemColors.Menu;
            menuItemSend.Name = "menuItemSend";
            menuItemSend.ShortcutKeys = Keys.Alt | Keys.S;
            menuItemSend.Size = new Size(175, 22);
            menuItemSend.Text = "Send";
            menuItemSend.Click += menuItemSend_Click;
            // 
            // menuItemRefresh
            // 
            menuItemRefresh.BackColor = SystemColors.Menu;
            menuItemRefresh.Name = "menuItemRefresh";
            menuItemRefresh.ShortcutKeys = Keys.Alt | Keys.R;
            menuItemRefresh.Size = new Size(175, 22);
            menuItemRefresh.Text = "Refresh";
            // 
            // menuItemClear
            // 
            menuItemClear.BackColor = SystemColors.Menu;
            menuItemClear.Name = "menuItemClear";
            menuItemClear.ShortcutKeys = Keys.Alt | Keys.C;
            menuItemClear.Size = new Size(175, 22);
            menuItemClear.Text = "Clear";
            // 
            // menuOptions
            // 
            menuOptions.BackColor = SystemColors.MenuBar;
            menuOptions.DropDownItems.AddRange(new ToolStripItem[] { menuItemMyContact, menuItemAddContact, menuItemImportContacts, menuItemViewContacts });
            menuOptions.Name = "menuOptions";
            menuOptions.Size = new Size(74, 21);
            menuOptions.Text = "Options";
            // 
            // menuItemMyContact
            // 
            menuItemMyContact.Name = "menuItemMyContact";
            menuItemMyContact.Size = new Size(186, 22);
            menuItemMyContact.Text = "My Contact";
            menuItemMyContact.Click += menuItemMyContact_Click;
            // 
            // menuItemAddContact
            // 
            menuItemAddContact.BackColor = SystemColors.Menu;
            menuItemAddContact.Name = "menuItemAddContact";
            menuItemAddContact.Size = new Size(186, 22);
            menuItemAddContact.Text = "Add Contact";
            menuItemAddContact.Click += menuItemAddContact_Click;
            // 
            // menuItemImportContacts
            // 
            menuItemImportContacts.BackColor = SystemColors.Menu;
            menuItemImportContacts.Name = "menuItemImportContacts";
            menuItemImportContacts.Size = new Size(186, 22);
            menuItemImportContacts.Text = "Import Contacts";
            // 
            // menuItemViewContacts
            // 
            menuItemViewContacts.BackColor = SystemColors.Menu;
            menuItemViewContacts.Name = "menuItemViewContacts";
            menuItemViewContacts.Size = new Size(186, 22);
            menuItemViewContacts.Text = "View Contacts";
            // 
            // menuQuestionMark
            // 
            menuQuestionMark.BackColor = SystemColors.MenuBar;
            menuQuestionMark.BackgroundImageLayout = ImageLayout.None;
            menuQuestionMark.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuQuestionMark.DropDownItems.AddRange(new ToolStripItem[] { menuItemAbout, menuItemHelp, menuItemInfo });
            menuQuestionMark.ForeColor = SystemColors.MenuText;
            menuQuestionMark.ImageScaling = ToolStripItemImageScaling.None;
            menuQuestionMark.Name = "menuQuestionMark";
            menuQuestionMark.Padding = new Padding(3, 0, 3, 0);
            menuQuestionMark.ShortcutKeys = Keys.Alt | Keys.F7;
            menuQuestionMark.Size = new Size(24, 21);
            menuQuestionMark.Text = "?";
            // 
            // menuItemAbout
            // 
            menuItemAbout.BackColor = SystemColors.Menu;
            menuItemAbout.BackgroundImageLayout = ImageLayout.None;
            menuItemAbout.ForeColor = SystemColors.MenuText;
            menuItemAbout.Name = "menuItemAbout";
            menuItemAbout.Padding = new Padding(0, 2, 0, 2);
            menuItemAbout.ShortcutKeys = Keys.Alt | Keys.A;
            menuItemAbout.Size = new Size(166, 24);
            menuItemAbout.Text = "About";
            menuItemAbout.TextImageRelation = TextImageRelation.TextAboveImage;
            menuItemAbout.Click += toolStripMenuItemAbout_Click;
            // 
            // menuItemHelp
            // 
            menuItemHelp.BackColor = SystemColors.Menu;
            menuItemHelp.BackgroundImageLayout = ImageLayout.None;
            menuItemHelp.ForeColor = SystemColors.MenuText;
            menuItemHelp.Name = "menuItemHelp";
            menuItemHelp.Padding = new Padding(0, 2, 0, 2);
            menuItemHelp.ShortcutKeys = Keys.Alt | Keys.H;
            menuItemHelp.Size = new Size(166, 24);
            menuItemHelp.Text = "Help";
            // 
            // menuItemInfo
            // 
            menuItemInfo.BackColor = SystemColors.Menu;
            menuItemInfo.BackgroundImageLayout = ImageLayout.None;
            menuItemInfo.ForeColor = SystemColors.MenuText;
            menuItemInfo.Name = "menuItemInfo";
            menuItemInfo.ShortcutKeys = Keys.Alt | Keys.I;
            menuItemInfo.Size = new Size(166, 22);
            menuItemInfo.Text = "Info";
            menuItemInfo.TextImageRelation = TextImageRelation.TextAboveImage;
            menuItemInfo.Click += toolStripMenuItemInfo_Click;
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
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripSplitButton, toolStripProgressBar, toolStripStatusLabel, toolStripDropDownButton1 });
            statusStrip.Location = new Point(0, 689);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(976, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripSplitButton
            // 
            toolStripSplitButton.BackColor = SystemColors.ControlLight;
            toolStripSplitButton.BackgroundImageLayout = ImageLayout.None;
            toolStripSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButton.DropDownItems.AddRange(new ToolStripItem[] { splitButtonMenuItemLoad, splitButtonMenuItemSave });
            toolStripSplitButton.Font = new Font("Lucida Sans", 10F);
            toolStripSplitButton.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton.Margin = new Padding(0, 1, 0, 0);
            toolStripSplitButton.Name = "toolStripSplitButton";
            toolStripSplitButton.Size = new Size(16, 21);
            toolStripSplitButton.Text = "toolStripSplitButton";
            toolStripSplitButton.TextImageRelation = TextImageRelation.ImageAboveText;
            toolStripSplitButton.ToolTipText = "toolStripSplitButton";
            // 
            // splitButtonMenuItemLoad
            // 
            splitButtonMenuItemLoad.Name = "splitButtonMenuItemLoad";
            splitButtonMenuItemLoad.Size = new Size(107, 22);
            splitButtonMenuItemLoad.Text = "Load";
            // 
            // splitButtonMenuItemSave
            // 
            splitButtonMenuItemSave.Name = "splitButtonMenuItemSave";
            splitButtonMenuItemSave.Size = new Size(107, 22);
            splitButtonMenuItemSave.Text = "Save";
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
            toolStripStatusLabel.Size = new Size(614, 19);
            toolStripStatusLabel.Spring = true;
            toolStripStatusLabel.Text = "Status";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripComboBox1 });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(29, 20);
            toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(181, 22);
            toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // toolStripComboBox1
            // 
            toolStripComboBox1.Name = "toolStripComboBox1";
            toolStripComboBox1.Size = new Size(121, 23);
            // 
            // panelSource
            // 
            panelSource.BackColor = SystemColors.Control;
            panelSource.Controls.Add(pictureBoxYou);
            panelSource.Controls.Add(pictureBoxSource);
            panelSource.ForeColor = SystemColors.ActiveCaptionText;
            panelSource.Location = new Point(0, 72);
            panelSource.Margin = new Padding(0);
            panelSource.Name = "panelSource";
            panelSource.Size = new Size(148, 472);
            panelSource.TabIndex = 40;
            // 
            // pictureBoxYou
            // 
            pictureBoxYou.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxYou.Location = new Point(3, 2);
            pictureBoxYou.Margin = new Padding(1);
            pictureBoxYou.Name = "pictureBoxYou";
            pictureBoxYou.Padding = new Padding(1);
            pictureBoxYou.Size = new Size(142, 142);
            pictureBoxYou.TabIndex = 58;
            pictureBoxYou.TabStop = false;
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.BackColor = SystemColors.Control;
            pictureBoxSource.Location = new Point(3, 317);
            pictureBoxSource.Margin = new Padding(1);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Padding = new Padding(1);
            pictureBoxSource.Size = new Size(142, 142);
            pictureBoxSource.TabIndex = 55;
            pictureBoxSource.TabStop = false;
            // 
            // buttonSecretKey
            // 
            buttonSecretKey.BackColor = SystemColors.ButtonHighlight;
            buttonSecretKey.BackgroundImageLayout = ImageLayout.Center;
            buttonSecretKey.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonSecretKey.ForeColor = SystemColors.ActiveCaptionText;
            buttonSecretKey.Location = new Point(87, 1);
            buttonSecretKey.Margin = new Padding(1);
            buttonSecretKey.Name = "buttonSecretKey";
            buttonSecretKey.Padding = new Padding(1);
            buttonSecretKey.Size = new Size(48, 28);
            buttonSecretKey.TabIndex = 12;
            buttonSecretKey.UseVisualStyleBackColor = false;
            buttonSecretKey.Click += Button_SecretKey_Click;
            // 
            // buttonAddToPipeline
            // 
            buttonAddToPipeline.BackColor = SystemColors.ButtonHighlight;
            buttonAddToPipeline.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonAddToPipeline.ForeColor = SystemColors.ActiveCaptionText;
            buttonAddToPipeline.Location = new Point(462, 2);
            buttonAddToPipeline.Margin = new Padding(0);
            buttonAddToPipeline.Name = "buttonAddToPipeline";
            buttonAddToPipeline.Size = new Size(28, 28);
            buttonAddToPipeline.TabIndex = 15;
            buttonAddToPipeline.Text = "⇒";
            buttonAddToPipeline.UseVisualStyleBackColor = false;
            buttonAddToPipeline.Click += Button_AddToPipeline_Click;
            // 
            // ComboBox_RemoteEndPoint
            // 
            ComboBox_RemoteEndPoint.BackColor = SystemColors.ControlLightLight;
            ComboBox_RemoteEndPoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_RemoteEndPoint.ForeColor = SystemColors.ControlText;
            ComboBox_RemoteEndPoint.FormattingEnabled = true;
            ComboBox_RemoteEndPoint.Items.AddRange(new object[] { "3DES", "2FISH", "3FISH", "AES", "Cast5", "Cast6", "Camellia", "Ghost28147", "Idea", "Noekeon", "Rijndael", "RC2", "RC532", "RC6", "Seed", "Serpent", "Skipjack", "Tea", "Tnepres", "XTea", "ZenMatrix" });
            ComboBox_RemoteEndPoint.Location = new Point(492, 4);
            ComboBox_RemoteEndPoint.Margin = new Padding(1);
            ComboBox_RemoteEndPoint.Name = "ComboBox_RemoteEndPoint";
            ComboBox_RemoteEndPoint.Size = new Size(312, 24);
            ComboBox_RemoteEndPoint.TabIndex = 16;
            // 
            // ComboBox_LocalEndPoint
            // 
            ComboBox_LocalEndPoint.BackColor = SystemColors.ControlLightLight;
            ComboBox_LocalEndPoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_LocalEndPoint.ForeColor = SystemColors.ControlText;
            ComboBox_LocalEndPoint.FormattingEnabled = true;
            ComboBox_LocalEndPoint.Items.AddRange(new object[] { "Hex16", "Base16", "Base32", "Hex32", "Base64", "Uu", "Html", "Url" });
            ComboBox_LocalEndPoint.Location = new Point(148, 4);
            ComboBox_LocalEndPoint.Margin = new Padding(1);
            ComboBox_LocalEndPoint.Name = "ComboBox_LocalEndPoint";
            ComboBox_LocalEndPoint.Size = new Size(312, 24);
            ComboBox_LocalEndPoint.TabIndex = 14;
            // 
            // buttonHashIv
            // 
            buttonHashIv.BackColor = SystemColors.ButtonHighlight;
            buttonHashIv.BackgroundImageLayout = ImageLayout.Center;
            buttonHashIv.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonHashIv.ForeColor = SystemColors.ActiveCaptionText;
            buttonHashIv.Location = new Point(840, 4);
            buttonHashIv.Margin = new Padding(1);
            buttonHashIv.Name = "buttonHashIv";
            buttonHashIv.Padding = new Padding(1);
            buttonHashIv.Size = new Size(48, 28);
            buttonHashIv.TabIndex = 18;
            buttonHashIv.UseVisualStyleBackColor = false;
            // 
            // panelEnCodeCrypt
            // 
            panelEnCodeCrypt.BackColor = SystemColors.ActiveCaption;
            panelEnCodeCrypt.Controls.Add(buttonSecretKey);
            panelEnCodeCrypt.Controls.Add(buttonHashIv);
            panelEnCodeCrypt.Controls.Add(ComboBox_LocalEndPoint);
            panelEnCodeCrypt.Controls.Add(ComboBox_RemoteEndPoint);
            panelEnCodeCrypt.Controls.Add(buttonAddToPipeline);
            panelEnCodeCrypt.ForeColor = SystemColors.WindowText;
            panelEnCodeCrypt.Location = new Point(0, 28);
            panelEnCodeCrypt.Margin = new Padding(0);
            panelEnCodeCrypt.Name = "panelEnCodeCrypt";
            panelEnCodeCrypt.Size = new Size(976, 36);
            panelEnCodeCrypt.TabIndex = 10;
            // 
            // pictureBoxDestination
            // 
            pictureBoxDestination.Location = new Point(3, 317);
            pictureBoxDestination.Margin = new Padding(1);
            pictureBoxDestination.Name = "pictureBoxDestination";
            pictureBoxDestination.Padding = new Padding(1);
            pictureBoxDestination.Size = new Size(142, 142);
            pictureBoxDestination.TabIndex = 56;
            pictureBoxDestination.TabStop = false;
            // 
            // richTextBoxChat
            // 
            richTextBoxChat.BorderStyle = BorderStyle.FixedSingle;
            richTextBoxChat.Cursor = Cursors.No;
            richTextBoxChat.ForeColor = SystemColors.WindowText;
            richTextBoxChat.Location = new Point(3, 549);
            richTextBoxChat.Margin = new Padding(2);
            richTextBoxChat.Name = "richTextBoxChat";
            richTextBoxChat.Size = new Size(970, 136);
            richTextBoxChat.TabIndex = 57;
            richTextBoxChat.Text = "";
            // 
            // pictureBoxPartner
            // 
            pictureBoxPartner.Location = new Point(3, 2);
            pictureBoxPartner.Margin = new Padding(1);
            pictureBoxPartner.Name = "pictureBoxPartner";
            pictureBoxPartner.Padding = new Padding(1);
            pictureBoxPartner.Size = new Size(142, 142);
            pictureBoxPartner.TabIndex = 59;
            pictureBoxPartner.TabStop = false;
            // 
            // panelDestination
            // 
            panelDestination.BackColor = SystemColors.Control;
            panelDestination.Controls.Add(buttonExit);
            panelDestination.Controls.Add(pictureBoxPartner);
            panelDestination.Controls.Add(pictureBoxDestination);
            panelDestination.ForeColor = SystemColors.ActiveCaptionText;
            panelDestination.Location = new Point(828, 72);
            panelDestination.Margin = new Padding(0);
            panelDestination.Name = "panelDestination";
            panelDestination.Size = new Size(148, 472);
            panelDestination.TabIndex = 80;
            // 
            // buttonExit
            // 
            buttonExit.BackColor = SystemColors.ButtonHighlight;
            buttonExit.Location = new Point(10, 168);
            buttonExit.Margin = new Padding(1);
            buttonExit.Name = "buttonExit";
            buttonExit.Padding = new Padding(1);
            buttonExit.Size = new Size(128, 28);
            buttonExit.TabIndex = 88;
            buttonExit.Text = "E&xit";
            buttonExit.UseVisualStyleBackColor = false;
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(richTextBoxOneView);
            panelCenter.Location = new Point(148, 72);
            panelCenter.Margin = new Padding(0);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(680, 460);
            panelCenter.TabIndex = 81;
            // 
            // richTextBoxOneView
            // 
            richTextBoxOneView.Dock = DockStyle.Fill;
            richTextBoxOneView.Location = new Point(0, 0);
            richTextBoxOneView.Margin = new Padding(2);
            richTextBoxOneView.Name = "richTextBoxOneView";
            richTextBoxOneView.Size = new Size(680, 460);
            richTextBoxOneView.TabIndex = 0;
            richTextBoxOneView.Text = "";
            // 
            // RichTextChat
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(976, 711);
            Controls.Add(panelCenter);
            Controls.Add(panelDestination);
            Controls.Add(richTextBoxChat);
            Controls.Add(panelSource);
            Controls.Add(panelEnCodeCrypt);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Font = new Font("Lucida Sans Unicode", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            Name = "RichTextChat";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "RichTextChat";
            TransparencyKey = SystemColors.Control;
            Load += RichTextChat_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            panelSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxYou).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            panelEnCodeCrypt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxDestination).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPartner).EndInit();
            panelDestination.ResumeLayout(false);
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panelSource;
        private Button buttonAddToPipeline;
        private ComboBox ComboBox_RemoteEndPoint;
        private Button buttonSecretKey;
        private ComboBox ComboBox_LocalEndPoint;
        private Button buttonHashIv;
        private Panel panelEnCodeCrypt;
        private PictureBox pictureBoxSource;
        private PictureBox pictureBoxDestination;
        private RichTextBox richTextBoxChat;
        private PictureBox pictureBoxYou;
        private PictureBox pictureBoxPartner;
        private Panel panelDestination;
        private Button buttonExit;
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripMenuMain;
        private ToolStripMenuItem menuItemAbout;
        private ToolStripMenuItem toolStripMenuItemOld;
        private ToolStripSeparator menuIPsSeparator;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItemOpen;
        private ToolStripMenuItem toolStripMenuItemClose;
        private ToolStripMenuItem menuItemInfo;
        private ToolStripMenuItem toolStripMenuItemExit;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem toolStripMenuFile;
        private ToolStripMenuItem menuFileItemOpen;
        private ToolStripMenuItem menuFileItemSave;
        private ToolStripMenuItem menuFileItemExit;
        private ToolStripMenuItem toolStripMenuTForms;
        private ToolStripMenuItem menuQuestionMark;
        private ToolStripMenuItem menuItemHelp;
        private SaveFileDialog saveFileDialog;
        private StatusStrip statusStrip;
        private ToolStripSplitButton toolStripSplitButton;
        private ToolStripMenuItem splitButtonMenuItemLoad;
        private ToolStripMenuItem splitButtonMenuItemSave;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripStatusLabel toolStripStatusLabel;      
        private ToolStripSeparator menuFileSeparator;        
        private ToolStripMenuItem menuIPs;
        private ToolStripMenuItem menuIItemMyIps;
        private ToolStripMenuItem menuItemFriendIp;
        private ToolStripMenuItem menuItemProxyServers;
        private ToolStripComboBox menuItemComboBoxProxies;
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
        private ToolStripMenuItem menuIViewItem1View;
        private ToolStripMenuItem menuOptions;
        private ToolStripMenuItem menuItemImportContacts;
        private ToolStripMenuItem menuItemAddContact;
        private ToolStripMenuItem menuItemViewContacts;
        private ToolStripMenuItem menuItemMyContact;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripComboBox toolStripComboBox1;
        private Panel panelCenter;
        private RichTextBox richTextBoxOneView;
    }
}