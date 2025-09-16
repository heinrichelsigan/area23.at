namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    partial class TransparentFormCore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransparentFormCore));
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
            menuItemSerializerText = new ToolStripMenuItem();
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
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.AllowDrop = true;
            menuStrip.AutoSize = false;
            menuStrip.BackColor = SystemColors.Control;
            menuStrip.Font = new Font("Lucida Sans Unicode", 10F);
            menuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuFile, toolStripMenuView, toolStripMenuQuestionMark });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(784, 25);
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
            // 
            // toolStripMenuView
            // 
            toolStripMenuView.BackColor = SystemColors.MenuBar;
            toolStripMenuView.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuView.DropDownItems.AddRange(new ToolStripItem[] { menuViewMenuICrypt, menuViewMenuUnix, menuItemSerializerText });
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
            menuViewMenuICrypt.Size = new Size(173, 22);
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
            menuViewMenuUnix.Size = new Size(173, 22);
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
            // menuItemSerializerText
            // 
            menuItemSerializerText.Name = "menuItemSerializerText";
            menuItemSerializerText.Size = new Size(173, 22);
            menuItemSerializerText.Text = "Serializer Test";
            menuItemSerializerText.Click += menuItemSerializerText_Click;
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
            toolStripMenuItemAbout.Size = new Size(166, 24);
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
            toolStripMenuItemHelp.Size = new Size(166, 24);
            toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemInfo
            // 
            toolStripMenuItemInfo.BackColor = SystemColors.Menu;
            toolStripMenuItemInfo.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemInfo.ForeColor = SystemColors.MenuText;
            toolStripMenuItemInfo.Name = "toolStripMenuItemInfo";
            toolStripMenuItemInfo.ShortcutKeys = Keys.Alt | Keys.I;
            toolStripMenuItemInfo.Size = new Size(166, 22);
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
            statusStrip.Location = new Point(0, 539);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(784, 22);
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
            toolStripSplitButton.Image = (Image)resources.GetObject("toolStripSplitButton.Image");
            toolStripSplitButton.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton.Margin = new Padding(0, 1, 0, 0);
            toolStripSplitButton.Name = "toolStripSplitButton";
            toolStripSplitButton.Size = new Size(32, 21);
            toolStripSplitButton.Text = "toolStripSplitButton";
            toolStripSplitButton.TextImageRelation = TextImageRelation.ImageAboveText;
            toolStripSplitButton.ToolTipText = "toolStripSplitButton";
            // 
            // splitButtonMenuItemLoad
            // 
            splitButtonMenuItemLoad.Image = Properties.Resources.a_right;
            splitButtonMenuItemLoad.Name = "splitButtonMenuItemLoad";
            splitButtonMenuItemLoad.Size = new Size(107, 22);
            splitButtonMenuItemLoad.Text = "Load";
            // 
            // splitButtonMenuItemSave
            // 
            splitButtonMenuItemSave.Image = Properties.Resources.a_right_key;
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
            toolStripStatusLabel.Size = new Size(435, 19);
            toolStripStatusLabel.Spring = true;
            toolStripStatusLabel.Text = "Status";
            // 
            // TransparentFormCore
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(784, 561);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Font = new Font("Lucida Sans Unicode", 10F);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "TransparentFormCore";
            Opacity = 0.6D;
            Text = "TransparentFormCore8";
            TransparencyKey = Color.Transparent;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        protected internal MenuStrip menuStrip;
        protected internal ToolStripMenuItem toolStripMenuMain;
        protected internal ToolStripMenuItem toolStripMenuItemAbout;
        protected internal ToolStripMenuItem toolStripMenuItemOld;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator3;
        protected internal ToolStripMenuItem toolStripMenuItemOpen;
        protected internal ToolStripMenuItem toolStripMenuItemClose;
        protected internal ToolStripMenuItem toolStripMenuItemInfo;
        protected internal ToolStripMenuItem toolStripMenuItemExit;
        protected internal OpenFileDialog openFileDialog;
        protected internal ToolStripMenuItem toolStripMenuView;
        protected internal ToolStripMenuItem toolStripMenuFile;
        protected internal ToolStripMenuItem menuFileItemOpen;
        protected internal ToolStripMenuItem menuFileItemSave;
        protected internal ToolStripMenuItem toolStripMenuTForms;
        protected internal ToolStripMenuItem toolStripMenuQuestionMark;
        protected internal ToolStripMenuItem toolStripMenuItemHelp;
        protected internal SaveFileDialog saveFileDialog;
        protected internal StatusStrip statusStrip;
        private ToolStripSplitButton toolStripSplitButton;
        protected internal ToolStripMenuItem splitButtonMenuItemLoad;
        protected internal ToolStripMenuItem splitButtonMenuItemSave;
        protected internal ToolStripProgressBar toolStripProgressBar;
        protected internal ToolStripStatusLabel toolStripStatusLabel;
        protected internal ToolStripMenuItem menuViewMenuUnix;
        protected internal ToolStripMenuItem menuViewMenuUnixItemNetAddr;
        protected internal ToolStripMenuItem menuViewMenuUnixItemFortnune;
        protected internal ToolStripMenuItem menuViewMenuUnixItemHexDump;
        protected internal ToolStripMenuItem menuViewMenuICrypt;
        protected internal ToolStripMenuItem menuViewMenuCryptItemEnDeCode;
        protected internal ToolStripMenuItem menuViewMenuCryptItemCrypt;
        protected internal ToolStripMenuItem menuFileItemNew;
        protected internal ToolStripMenuItem menuFileItemDiscard;
        private ToolStripSeparator toolStripSeparator2;
        protected internal ToolStripMenuItem menuFileItemExit;
        protected internal ToolStripMenuItem menuViewMenuUnixItemScp;
        protected internal ToolStripMenuItem menuViewMenuUnixItemSecureChat;
        private ToolStripMenuItem menuItemSerializerText;
    }
}