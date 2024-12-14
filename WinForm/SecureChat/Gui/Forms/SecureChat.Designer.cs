
using Area23.At.Framework.Library.Core.Net.WebHttp;
using System.Net;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    partial class SecureChat
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
            toolStripMenuFile = new ToolStripMenuItem();
            menuFileItemNew = new ToolStripMenuItem();
            menuFileItemOpen = new ToolStripMenuItem();
            menuFileItemDiscard = new ToolStripMenuItem();
            menuFileItemSave = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            menuFileItemExit = new ToolStripMenuItem();
            toolStripMenuView = new ToolStripMenuItem();
            menuViewMenuICrypt = new ToolStripMenuItem();
            menuViewMenuCryptItemEnDeCode = new ToolStripMenuItem();
            menuViewMenuCryptItemCrypt = new ToolStripMenuItem();
            menuViewMenuUnix = new ToolStripMenuItem();
            menuViewMenuUnixItemNetAddr = new ToolStripMenuItem();
            menuViewMenuUnixItemSecureChat = new ToolStripMenuItem();
            menuViewMenuUnixItemScp = new ToolStripMenuItem();
            menuViewMenuUnixItemFortnune = new ToolStripMenuItem();
            menuViewMenuUnixItemHexDump = new ToolStripMenuItem();
            toolStripMenuQuestionMark = new ToolStripMenuItem();
            toolStripMenuItemAbout = new ToolStripMenuItem();
            toolStripMenuItemHelp = new ToolStripMenuItem();
            toolStripMenuItemInfo = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            statusStrip = new StatusStrip();
            toolStripSplitButton = new ToolStripSplitButton();
            splitButtonMenuItemLoad = new ToolStripMenuItem();
            splitButtonMenuItemSave = new ToolStripMenuItem();
            toolStripProgressBar = new ToolStripProgressBar();
            toolStripStatusLabel = new ToolStripStatusLabel();
            splitContainer = new SplitContainer();
            TextBoxSource = new TextBox();
            TextBoxDestionation = new TextBox();
            buttonDelete = new Button();
            buttonSend = new Button();
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
            buttonReload = new Button();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxYou).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            panelEnCodeCrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDestination).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPartner).BeginInit();
            panelDestination.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.Transparent;
            menuStrip.Font = new Font("Lucida Sans Unicode", 10F);
            menuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuFile, toolStripMenuView, toolStripMenuQuestionMark });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(976, 25);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // toolStripMenuFile
            // 
            toolStripMenuFile.BackColor = SystemColors.MenuBar;
            toolStripMenuFile.DropDownItems.AddRange(new ToolStripItem[] { menuFileItemNew, menuFileItemOpen, menuFileItemDiscard, menuFileItemSave, toolStripSeparator2, menuFileItemExit });
            toolStripMenuFile.ForeColor = SystemColors.MenuText;
            toolStripMenuFile.Name = "toolStripMenuFile";
            toolStripMenuFile.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuFile.ShortcutKeys = Keys.Alt | Keys.F;
            toolStripMenuFile.Size = new Size(42, 21);
            toolStripMenuFile.Text = "File";
            // 
            // menuFileItemNew
            // 
            menuFileItemNew.AutoToolTip = true;
            menuFileItemNew.BackColor = SystemColors.Menu;
            menuFileItemNew.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemNew.ForeColor = SystemColors.MenuText;
            menuFileItemNew.Margin = new Padding(1);
            menuFileItemNew.Name = "menuFileItemNew";
            menuFileItemNew.ShortcutKeys = Keys.Control | Keys.N;
            menuFileItemNew.Size = new Size(181, 22);
            menuFileItemNew.Text = "New";
            menuFileItemNew.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemNew.ToolTipText = "New file";
            menuFileItemNew.Click += menuFileItemNew_Click;
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
            menuFileItemOpen.Size = new Size(181, 22);
            menuFileItemOpen.Text = "Open";
            menuFileItemOpen.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemOpen.ToolTipText = "Open file";
            menuFileItemOpen.Click += toolStripMenuItemLoad_Click;
            // 
            // menuFileItemDiscard
            // 
            menuFileItemDiscard.AutoToolTip = true;
            menuFileItemDiscard.BackColor = SystemColors.Menu;
            menuFileItemDiscard.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemDiscard.ForeColor = SystemColors.MenuText;
            menuFileItemDiscard.Margin = new Padding(1);
            menuFileItemDiscard.Name = "menuFileItemDiscard";
            menuFileItemDiscard.ShortcutKeys = Keys.Control | Keys.D;
            menuFileItemDiscard.Size = new Size(181, 22);
            menuFileItemDiscard.Text = "Discard";
            menuFileItemDiscard.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemDiscard.ToolTipText = "Discard";
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
            menuFileItemSave.Size = new Size(181, 22);
            menuFileItemSave.Text = "Save";
            menuFileItemSave.TextImageRelation = TextImageRelation.TextAboveImage;
            menuFileItemSave.ToolTipText = "Save file";
            menuFileItemSave.Click += toolStripMenuItemSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.BackColor = SystemColors.Menu;
            toolStripSeparator2.ForeColor = SystemColors.MenuText;
            toolStripSeparator2.Margin = new Padding(1);
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(178, 6);
            // 
            // menuFileItemExit
            // 
            menuFileItemExit.BackColor = SystemColors.Menu;
            menuFileItemExit.BackgroundImageLayout = ImageLayout.Center;
            menuFileItemExit.ForeColor = SystemColors.MenuText;
            menuFileItemExit.Name = "menuFileItemExit";
            menuFileItemExit.ShortcutKeys = Keys.Alt | Keys.X;
            menuFileItemExit.Size = new Size(181, 22);
            menuFileItemExit.Text = "Exit";
            menuFileItemExit.Click += menuFileItemExit_Click;
            // 
            // toolStripMenuView
            // 
            toolStripMenuView.BackColor = SystemColors.MenuBar;
            toolStripMenuView.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuView.DropDownItems.AddRange(new ToolStripItem[] { menuViewMenuICrypt, menuViewMenuUnix });
            toolStripMenuView.ForeColor = SystemColors.MenuText;
            toolStripMenuView.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuView.Name = "toolStripMenuView";
            toolStripMenuView.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuView.ShortcutKeys = Keys.Alt | Keys.V;
            toolStripMenuView.Size = new Size(50, 21);
            toolStripMenuView.Text = "View";
            // 
            // menuViewMenuICrypt
            // 
            menuViewMenuICrypt.AutoToolTip = true;
            menuViewMenuICrypt.BackColor = SystemColors.Menu;
            menuViewMenuICrypt.BackgroundImageLayout = ImageLayout.Zoom;
            menuViewMenuICrypt.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuViewMenuICrypt.DropDownItems.AddRange(new ToolStripItem[] { menuViewMenuCryptItemEnDeCode, menuViewMenuCryptItemCrypt });
            menuViewMenuICrypt.ForeColor = SystemColors.MenuText;
            menuViewMenuICrypt.Margin = new Padding(1);
            menuViewMenuICrypt.Name = "menuViewMenuICrypt";
            menuViewMenuICrypt.ShortcutKeys = Keys.Alt | Keys.C;
            menuViewMenuICrypt.Size = new Size(180, 22);
            menuViewMenuICrypt.Text = "Crypt";
            menuViewMenuICrypt.ToolTipText = "Crypt Forms Submenu";
            // 
            // menuViewMenuCryptItemEnDeCode
            // 
            menuViewMenuCryptItemEnDeCode.AutoToolTip = true;
            menuViewMenuCryptItemEnDeCode.BackColor = SystemColors.Menu;
            menuViewMenuCryptItemEnDeCode.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuCryptItemEnDeCode.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuViewMenuCryptItemEnDeCode.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuCryptItemEnDeCode.ForeColor = SystemColors.MenuText;
            menuViewMenuCryptItemEnDeCode.Margin = new Padding(1);
            menuViewMenuCryptItemEnDeCode.Name = "menuViewMenuCryptItemEnDeCode";
            menuViewMenuCryptItemEnDeCode.Size = new Size(163, 22);
            menuViewMenuCryptItemEnDeCode.Text = "En-/Decode";
            menuViewMenuCryptItemEnDeCode.ToolTipText = "Encode & Decode Form";
            menuViewMenuCryptItemEnDeCode.Click += menuViewMenuCrypItemEnDeCode_Click;
            // 
            // menuViewMenuCryptItemCrypt
            // 
            menuViewMenuCryptItemCrypt.AutoToolTip = true;
            menuViewMenuCryptItemCrypt.BackColor = SystemColors.Menu;
            menuViewMenuCryptItemCrypt.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuCryptItemCrypt.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuViewMenuCryptItemCrypt.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuCryptItemCrypt.ForeColor = SystemColors.MenuText;
            menuViewMenuCryptItemCrypt.Margin = new Padding(1);
            menuViewMenuCryptItemCrypt.Name = "menuViewMenuCryptItemCrypt";
            menuViewMenuCryptItemCrypt.Size = new Size(163, 22);
            menuViewMenuCryptItemCrypt.Text = "En-/DeCrypt";
            menuViewMenuCryptItemCrypt.ToolTipText = "Encrypt Decrypt Pipeline";
            menuViewMenuCryptItemCrypt.Click += menuViewMenuCryptItemCrypt_Click;
            // 
            // menuViewMenuUnix
            // 
            menuViewMenuUnix.AutoToolTip = true;
            menuViewMenuUnix.BackColor = SystemColors.Menu;
            menuViewMenuUnix.DisplayStyle = ToolStripItemDisplayStyle.Text;
            menuViewMenuUnix.DropDownItems.AddRange(new ToolStripItem[] { menuViewMenuUnixItemNetAddr, menuViewMenuUnixItemSecureChat, menuViewMenuUnixItemScp, menuViewMenuUnixItemFortnune, menuViewMenuUnixItemHexDump });
            menuViewMenuUnix.ForeColor = SystemColors.MenuText;
            menuViewMenuUnix.Margin = new Padding(1);
            menuViewMenuUnix.Name = "menuViewMenuUnix";
            menuViewMenuUnix.ShortcutKeys = Keys.Alt | Keys.U;
            menuViewMenuUnix.Size = new Size(180, 22);
            menuViewMenuUnix.Text = "Unix";
            menuViewMenuUnix.ToolTipText = "Unix Tools Submenu";
            // 
            // menuViewMenuUnixItemNetAddr
            // 
            menuViewMenuUnixItemNetAddr.BackColor = SystemColors.Menu;
            menuViewMenuUnixItemNetAddr.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuUnixItemNetAddr.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuUnixItemNetAddr.ForeColor = SystemColors.MenuText;
            menuViewMenuUnixItemNetAddr.ImageScaling = ToolStripItemImageScaling.None;
            menuViewMenuUnixItemNetAddr.Margin = new Padding(1);
            menuViewMenuUnixItemNetAddr.Name = "menuViewMenuUnixItemNetAddr";
            menuViewMenuUnixItemNetAddr.Size = new Size(193, 22);
            menuViewMenuUnixItemNetAddr.Text = "Network Address";
            menuViewMenuUnixItemNetAddr.Click += menuViewMenuUnixItemNetAddr_Click;
            // 
            // menuViewMenuUnixItemSecureChat
            // 
            menuViewMenuUnixItemSecureChat.BackColor = SystemColors.Menu;
            menuViewMenuUnixItemSecureChat.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuUnixItemSecureChat.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuUnixItemSecureChat.ForeColor = SystemColors.MenuText;
            menuViewMenuUnixItemSecureChat.ImageScaling = ToolStripItemImageScaling.None;
            menuViewMenuUnixItemSecureChat.Margin = new Padding(1);
            menuViewMenuUnixItemSecureChat.Name = "menuViewMenuUnixItemSecureChat";
            menuViewMenuUnixItemSecureChat.Size = new Size(193, 22);
            menuViewMenuUnixItemSecureChat.Text = "Secure Chat";
            menuViewMenuUnixItemSecureChat.Click += menuViewMenuUnixItemSecureChat_Click;
            // 
            // menuViewMenuUnixItemScp
            // 
            menuViewMenuUnixItemScp.BackColor = SystemColors.Menu;
            menuViewMenuUnixItemScp.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuUnixItemScp.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuUnixItemScp.ForeColor = SystemColors.MenuText;
            menuViewMenuUnixItemScp.ImageScaling = ToolStripItemImageScaling.None;
            menuViewMenuUnixItemScp.Margin = new Padding(1);
            menuViewMenuUnixItemScp.Name = "menuViewMenuUnixItemScp";
            menuViewMenuUnixItemScp.Size = new Size(193, 22);
            menuViewMenuUnixItemScp.Text = "Scp";
            menuViewMenuUnixItemScp.Click += menuViewMenuUnixItemScp_Click;
            // 
            // menuViewMenuUnixItemFortnune
            // 
            menuViewMenuUnixItemFortnune.BackColor = SystemColors.Menu;
            menuViewMenuUnixItemFortnune.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuUnixItemFortnune.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuUnixItemFortnune.ForeColor = SystemColors.MenuText;
            menuViewMenuUnixItemFortnune.ImageScaling = ToolStripItemImageScaling.None;
            menuViewMenuUnixItemFortnune.Margin = new Padding(1);
            menuViewMenuUnixItemFortnune.Name = "menuViewMenuUnixItemFortnune";
            menuViewMenuUnixItemFortnune.Size = new Size(193, 22);
            menuViewMenuUnixItemFortnune.Text = "Fortnune";
            menuViewMenuUnixItemFortnune.Click += menuViewMenuUnixItemFortnune_Click;
            // 
            // menuViewMenuUnixItemHexDump
            // 
            menuViewMenuUnixItemHexDump.BackColor = SystemColors.Menu;
            menuViewMenuUnixItemHexDump.BackgroundImageLayout = ImageLayout.None;
            menuViewMenuUnixItemHexDump.Font = new Font("Lucida Sans Unicode", 10F);
            menuViewMenuUnixItemHexDump.ForeColor = SystemColors.MenuText;
            menuViewMenuUnixItemHexDump.ImageScaling = ToolStripItemImageScaling.None;
            menuViewMenuUnixItemHexDump.Name = "menuViewMenuUnixItemHexDump";
            menuViewMenuUnixItemHexDump.Size = new Size(193, 22);
            menuViewMenuUnixItemHexDump.Text = "HexDump";
            // 
            // toolStripMenuQuestionMark
            // 
            toolStripMenuQuestionMark.BackColor = SystemColors.MenuBar;
            toolStripMenuQuestionMark.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuQuestionMark.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuQuestionMark.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemAbout, toolStripMenuItemHelp, toolStripMenuItemInfo });
            toolStripMenuQuestionMark.ForeColor = SystemColors.MenuText;
            toolStripMenuQuestionMark.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuQuestionMark.Name = "toolStripMenuQuestionMark";
            toolStripMenuQuestionMark.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuQuestionMark.ShortcutKeys = Keys.Alt | Keys.F7;
            toolStripMenuQuestionMark.Size = new Size(24, 21);
            toolStripMenuQuestionMark.Text = "?";
            // 
            // toolStripMenuItemAbout
            // 
            toolStripMenuItemAbout.BackColor = SystemColors.Menu;
            toolStripMenuItemAbout.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemAbout.ForeColor = SystemColors.MenuText;
            toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            toolStripMenuItemAbout.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemAbout.ShortcutKeys = Keys.Alt | Keys.A;
            toolStripMenuItemAbout.Size = new Size(180, 24);
            toolStripMenuItemAbout.Text = "About";
            toolStripMenuItemAbout.TextImageRelation = TextImageRelation.TextAboveImage;
            toolStripMenuItemAbout.Click += toolStripMenuItemAbout_Click;
            // 
            // toolStripMenuItemHelp
            // 
            toolStripMenuItemHelp.BackColor = SystemColors.Menu;
            toolStripMenuItemHelp.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemHelp.ForeColor = SystemColors.MenuText;
            toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            toolStripMenuItemHelp.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemHelp.ShortcutKeys = Keys.Alt | Keys.H;
            toolStripMenuItemHelp.Size = new Size(180, 24);
            toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemInfo
            // 
            toolStripMenuItemInfo.BackColor = SystemColors.Menu;
            toolStripMenuItemInfo.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemInfo.ForeColor = SystemColors.MenuText;
            toolStripMenuItemInfo.Name = "toolStripMenuItemInfo";
            toolStripMenuItemInfo.ShortcutKeys = Keys.Alt | Keys.I;
            toolStripMenuItemInfo.Size = new Size(180, 22);
            toolStripMenuItemInfo.Text = "Info";
            toolStripMenuItemInfo.TextImageRelation = TextImageRelation.TextAboveImage;
            toolStripMenuItemInfo.Click += toolStripMenuItemInfo_Click;
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
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripSplitButton, toolStripProgressBar, toolStripStatusLabel });
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
            toolStripStatusLabel.Size = new Size(643, 19);
            toolStripStatusLabel.Spring = true;
            toolStripStatusLabel.Text = "Status";
            // 
            // splitContainer
            // 
            splitContainer.BackColor = SystemColors.ControlLight;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Location = new Point(148, 72);
            splitContainer.Margin = new Padding(0);
            splitContainer.MaximumSize = new Size(800, 600);
            splitContainer.MinimumSize = new Size(320, 200);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.AllowDrop = true;
            splitContainer.Panel1.BackgroundImageLayout = ImageLayout.None;
            splitContainer.Panel1.Controls.Add(TextBoxSource);
            splitContainer.Panel1MinSize = 300;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.BackgroundImageLayout = ImageLayout.None;
            splitContainer.Panel2.Controls.Add(TextBoxDestionation);
            splitContainer.Panel2MinSize = 300;
            splitContainer.Size = new Size(680, 472);
            splitContainer.SplitterDistance = 336;
            splitContainer.SplitterIncrement = 8;
            splitContainer.SplitterWidth = 8;
            splitContainer.TabIndex = 20;
            splitContainer.TabStop = false;
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
            TextBoxSource.Size = new Size(336, 472);
            TextBoxSource.TabIndex = 23;
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
            TextBoxDestionation.Size = new Size(336, 472);
            TextBoxDestionation.TabIndex = 43;
            // 
            // buttonDelete
            // 
            buttonDelete.BackColor = SystemColors.ButtonHighlight;
            buttonDelete.Location = new Point(12, 576);
            buttonDelete.Margin = new Padding(1);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Padding = new Padding(1);
            buttonDelete.Size = new Size(120, 28);
            buttonDelete.TabIndex = 48;
            buttonDelete.Text = "&Delete";
            buttonDelete.UseVisualStyleBackColor = false;
            buttonDelete.Click += Button_Encode_Click;
            // 
            // buttonSend
            // 
            buttonSend.BackColor = SystemColors.ButtonHighlight;
            buttonSend.Location = new Point(12, 536);
            buttonSend.Margin = new Padding(1);
            buttonSend.Name = "buttonSend";
            buttonSend.Padding = new Padding(1);
            buttonSend.Size = new Size(120, 28);
            buttonSend.TabIndex = 47;
            buttonSend.Text = "&Send";
            buttonSend.UseVisualStyleBackColor = false;
            buttonSend.Click += Button_Save_Click;
            // 
            // panelSource
            // 
            panelSource.BackColor = SystemColors.Control;
            panelSource.Controls.Add(buttonDelete);
            panelSource.Controls.Add(pictureBoxYou);
            panelSource.Controls.Add(buttonSend);
            panelSource.Controls.Add(pictureBoxSource);
            panelSource.ForeColor = SystemColors.ActiveCaptionText;
            panelSource.Location = new Point(0, 72);
            panelSource.Margin = new Padding(0);
            panelSource.Name = "panelSource";
            panelSource.Size = new Size(148, 610);
            panelSource.TabIndex = 40;
            // 
            // pictureBoxYou
            // 
            pictureBoxYou.Location = new Point(10, 2);
            pictureBoxYou.Margin = new Padding(1);
            pictureBoxYou.Name = "pictureBoxYou";
            pictureBoxYou.Padding = new Padding(1);
            pictureBoxYou.Size = new Size(128, 128);
            pictureBoxYou.TabIndex = 58;
            pictureBoxYou.TabStop = false;
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.BackColor = SystemColors.Control;
            pictureBoxSource.Location = new Point(8, 396);
            pictureBoxSource.Margin = new Padding(1);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Padding = new Padding(1);
            pictureBoxSource.Size = new Size(128, 128);
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
            ComboBox_LocalEndPoint.Items.AddRange(new object[] { "hex16", "base16", "base32", "base64", "unix2unix", "html", "url" });
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
            pictureBoxDestination.Location = new Point(10, 396);
            pictureBoxDestination.Margin = new Padding(1);
            pictureBoxDestination.Name = "pictureBoxDestination";
            pictureBoxDestination.Padding = new Padding(1);
            pictureBoxDestination.Size = new Size(128, 128);
            pictureBoxDestination.TabIndex = 56;
            pictureBoxDestination.TabStop = false;
            // 
            // richTextBoxChat
            // 
            richTextBoxChat.BorderStyle = BorderStyle.FixedSingle;
            richTextBoxChat.Cursor = Cursors.No;
            richTextBoxChat.ForeColor = SystemColors.WindowText;
            richTextBoxChat.Location = new Point(148, 564);
            richTextBoxChat.Margin = new Padding(2);
            richTextBoxChat.Name = "richTextBoxChat";
            richTextBoxChat.Size = new Size(678, 116);
            richTextBoxChat.TabIndex = 57;
            richTextBoxChat.Text = "";
            // 
            // pictureBoxPartner
            // 
            pictureBoxPartner.Location = new Point(10, 2);
            pictureBoxPartner.Margin = new Padding(1);
            pictureBoxPartner.Name = "pictureBoxPartner";
            pictureBoxPartner.Padding = new Padding(1);
            pictureBoxPartner.Size = new Size(128, 128);
            pictureBoxPartner.TabIndex = 59;
            pictureBoxPartner.TabStop = false;
            // 
            // panelDestination
            // 
            panelDestination.BackColor = SystemColors.Control;
            panelDestination.Controls.Add(buttonExit);
            panelDestination.Controls.Add(pictureBoxPartner);
            panelDestination.Controls.Add(pictureBoxDestination);
            panelDestination.Controls.Add(buttonReload);
            panelDestination.ForeColor = SystemColors.ActiveCaptionText;
            panelDestination.Location = new Point(828, 72);
            panelDestination.Margin = new Padding(0);
            panelDestination.Name = "panelDestination";
            panelDestination.Size = new Size(148, 610);
            panelDestination.TabIndex = 80;
            // 
            // buttonExit
            // 
            buttonExit.BackColor = SystemColors.ButtonHighlight;
            buttonExit.Location = new Point(12, 576);
            buttonExit.Margin = new Padding(1);
            buttonExit.Name = "buttonExit";
            buttonExit.Padding = new Padding(1);
            buttonExit.Size = new Size(120, 28);
            buttonExit.TabIndex = 88;
            buttonExit.Text = "E&xit";
            buttonExit.UseVisualStyleBackColor = false;
            // 
            // buttonReload
            // 
            buttonReload.BackColor = SystemColors.ButtonHighlight;
            buttonReload.ForeColor = SystemColors.ActiveCaptionText;
            buttonReload.Location = new Point(12, 536);
            buttonReload.Margin = new Padding(1);
            buttonReload.Name = "buttonReload";
            buttonReload.Padding = new Padding(1);
            buttonReload.Size = new Size(120, 28);
            buttonReload.TabIndex = 84;
            buttonReload.Text = "&Reload";
            buttonReload.UseVisualStyleBackColor = false;
            // 
            // SecureChat
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(976, 711);
            Controls.Add(panelDestination);
            Controls.Add(richTextBoxChat);
            Controls.Add(panelSource);
            Controls.Add(panelEnCodeCrypt);
            Controls.Add(splitContainer);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Font = new Font("Lucida Sans Unicode", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            Name = "SecureChat";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "SecureChat";
            TransparencyKey = SystemColors.Control;
            Load += SecureChat_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxYou).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            panelEnCodeCrypt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxDestination).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPartner).EndInit();
            panelDestination.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private SplitContainer splitContainer;
        private TextBox TextBoxSource;
        private TextBox TextBoxDestionation;
        private Button buttonDelete;
        private Button buttonSend;
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
        private Button buttonReload;
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripMenuMain;
        private ToolStripMenuItem toolStripMenuItemAbout;
        private ToolStripMenuItem toolStripMenuItemOld;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItemOpen;
        private ToolStripMenuItem toolStripMenuItemClose;
        private ToolStripMenuItem toolStripMenuItemInfo;
        private ToolStripMenuItem toolStripMenuItemExit;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem toolStripMenuView;
        private ToolStripMenuItem toolStripMenuFile;
        private ToolStripMenuItem menuFileItemOpen;
        private ToolStripMenuItem menuFileItemSave;
        private ToolStripMenuItem toolStripMenuTForms;
        private ToolStripMenuItem toolStripMenuQuestionMark;
        private ToolStripMenuItem toolStripMenuItemHelp;
        private SaveFileDialog saveFileDialog;
        private StatusStrip statusStrip;
        private ToolStripSplitButton toolStripSplitButton;
        private ToolStripMenuItem splitButtonMenuItemLoad;
        private ToolStripMenuItem splitButtonMenuItemSave;
        private ToolStripProgressBar toolStripProgressBar;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripMenuItem menuViewMenuUnix;
        private ToolStripMenuItem menuViewMenuUnixItemNetAddr;
        private ToolStripMenuItem menuViewMenuUnixItemFortnune;
        private ToolStripMenuItem menuViewMenuUnixItemHexDump;
        private ToolStripMenuItem menuViewMenuICrypt;
        private ToolStripMenuItem menuViewMenuCryptItemEnDeCode;
        private ToolStripMenuItem menuViewMenuCryptItemCrypt;
        private ToolStripMenuItem menuFileItemNew;
        private ToolStripMenuItem menuFileItemDiscard;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem menuFileItemExit;
        private ToolStripMenuItem menuViewMenuUnixItemScp;
        private ToolStripMenuItem menuViewMenuUnixItemSecureChat;
    }
}