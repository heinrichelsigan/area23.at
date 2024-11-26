namespace Area23.At.WinForm.TWinFormCore.UI.Forms
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
            menuStrip = new MenuStrip();
            toolStripMenuMain = new ToolStripMenuItem();
            toolStripMenuItemNew = new ToolStripMenuItem();
            toolStripMenuItemOld = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripMenuItemClose = new ToolStripMenuItem();
            toolStripMenuItemExit = new ToolStripMenuItem();
            toolStripMenuFile = new ToolStripMenuItem();
            toolStripMenuItemLoad = new ToolStripMenuItem();
            toolStripMenuItemSave = new ToolStripMenuItem();
            toolStripMenuUnix = new ToolStripMenuItem();
            myAddrToolStripMenuItem = new ToolStripMenuItem();
            fortnuneToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuQr = new ToolStripMenuItem();
            cryptToolStripMenu = new ToolStripMenuItem();
            toolStripMenuItemEnDeCode = new ToolStripMenuItem();
            toolStripMenuItemCrypt = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItemAbout = new ToolStripMenuItem();
            toolStripMenuItemHelp = new ToolStripMenuItem();
            toolStripMenuItemInfo = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.Transparent;
            menuStrip.Font = new Font("Lucida Sans Unicode", 10F);
            menuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuMain, toolStripMenuFile, toolStripMenuUnix, toolStripMenuQr, cryptToolStripMenu, toolStripMenuItem1 });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(784, 25);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // toolStripMenuMain
            // 
            toolStripMenuMain.BackColor = SystemColors.ControlLight;
            toolStripMenuMain.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuMain.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuMain.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemNew, toolStripMenuItemOld, toolStripSeparator1, toolStripMenuItemClose, toolStripMenuItemExit });
            toolStripMenuMain.Font = new Font("Lucida Sans Unicode", 10F);
            toolStripMenuMain.ForeColor = SystemColors.Desktop;
            toolStripMenuMain.ImageTransparentColor = Color.Transparent;
            toolStripMenuMain.Name = "toolStripMenuMain";
            toolStripMenuMain.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuMain.ShortcutKeys = Keys.Alt | Keys.M;
            toolStripMenuMain.Size = new Size(51, 21);
            toolStripMenuMain.Text = "Main";
            // 
            // toolStripMenuItemNew
            // 
            toolStripMenuItemNew.BackColor = Color.Transparent;
            toolStripMenuItemNew.BackgroundImage = Properties.Resources.TransparentMenuImage;
            toolStripMenuItemNew.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemNew.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItemNew.ForeColor = SystemColors.Desktop;
            toolStripMenuItemNew.Image = Properties.Resources.TransparentMenuImage1;
            toolStripMenuItemNew.Name = "toolStripMenuItemNew";
            toolStripMenuItemNew.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemNew.ShortcutKeys = Keys.Alt | Keys.N;
            toolStripMenuItemNew.Size = new Size(153, 24);
            toolStripMenuItemNew.Text = "New";
            toolStripMenuItemNew.Click += toolStripMenuItemNew_Click;
            // 
            // toolStripMenuItemOld
            // 
            toolStripMenuItemOld.BackColor = Color.Transparent;
            toolStripMenuItemOld.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemOld.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItemOld.ForeColor = SystemColors.Desktop;
            toolStripMenuItemOld.ImageTransparentColor = Color.Transparent;
            toolStripMenuItemOld.Name = "toolStripMenuItemOld";
            toolStripMenuItemOld.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemOld.ShortcutKeys = Keys.Alt | Keys.O;
            toolStripMenuItemOld.Size = new Size(153, 24);
            toolStripMenuItemOld.Text = "Old";
            toolStripMenuItemOld.TextImageRelation = TextImageRelation.ImageAboveText;
            toolStripMenuItemOld.Click += toolStripMenuItemOld_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.BackColor = Color.Transparent;
            toolStripSeparator1.ForeColor = SystemColors.Desktop;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(150, 6);
            // 
            // toolStripMenuItemClose
            // 
            toolStripMenuItemClose.BackColor = Color.Transparent;
            toolStripMenuItemClose.ForeColor = SystemColors.Desktop;
            toolStripMenuItemClose.Name = "toolStripMenuItemClose";
            toolStripMenuItemClose.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemClose.Size = new Size(153, 24);
            toolStripMenuItemClose.Text = "Close";
            toolStripMenuItemClose.Click += toolStripMenuItemClose_Click;
            // 
            // toolStripMenuItemExit
            // 
            toolStripMenuItemExit.BackColor = Color.Transparent;
            toolStripMenuItemExit.ForeColor = SystemColors.Desktop;
            toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            toolStripMenuItemExit.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemExit.ShortcutKeys = Keys.Alt | Keys.X;
            toolStripMenuItemExit.Size = new Size(153, 24);
            toolStripMenuItemExit.Text = "Exit";
            toolStripMenuItemExit.Click += toolStripMenuItemExit_Click;
            // 
            // toolStripMenuFile
            // 
            toolStripMenuFile.BackColor = SystemColors.ControlLightLight;
            toolStripMenuFile.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemLoad, toolStripMenuItemSave });
            toolStripMenuFile.ForeColor = SystemColors.Desktop;
            toolStripMenuFile.Name = "toolStripMenuFile";
            toolStripMenuFile.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuFile.ShortcutKeys = Keys.Alt | Keys.F;
            toolStripMenuFile.Size = new Size(42, 21);
            toolStripMenuFile.Text = "File";
            // 
            // toolStripMenuItemLoad
            // 
            toolStripMenuItemLoad.BackColor = Color.Transparent;
            toolStripMenuItemLoad.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemLoad.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItemLoad.ForeColor = SystemColors.Desktop;
            toolStripMenuItemLoad.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuItemLoad.ImageTransparentColor = SystemColors.Control;
            toolStripMenuItemLoad.Name = "toolStripMenuItemLoad";
            toolStripMenuItemLoad.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemLoad.Size = new Size(109, 24);
            toolStripMenuItemLoad.Text = "Load";
            toolStripMenuItemLoad.Click += toolStripMenuItemLoad_Click;
            // 
            // toolStripMenuItemSave
            // 
            toolStripMenuItemSave.BackColor = SystemColors.ControlLightLight;
            toolStripMenuItemSave.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemSave.ForeColor = SystemColors.Desktop;
            toolStripMenuItemSave.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            toolStripMenuItemSave.Size = new Size(109, 22);
            toolStripMenuItemSave.Text = "Save";
            toolStripMenuItemSave.Click += toolStripMenuItemSave_Click;
            // 
            // toolStripMenuUnix
            // 
            toolStripMenuUnix.BackColor = SystemColors.ControlLight;
            toolStripMenuUnix.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuUnix.DropDownItems.AddRange(new ToolStripItem[] { myAddrToolStripMenuItem, fortnuneToolStripMenuItem });
            toolStripMenuUnix.ForeColor = SystemColors.Desktop;
            toolStripMenuUnix.ImageTransparentColor = Color.Transparent;
            toolStripMenuUnix.Name = "toolStripMenuUnix";
            toolStripMenuUnix.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuUnix.ShortcutKeys = Keys.Alt | Keys.U;
            toolStripMenuUnix.Size = new Size(50, 21);
            toolStripMenuUnix.Text = "Unix";
            // 
            // myAddrToolStripMenuItem
            // 
            myAddrToolStripMenuItem.Name = "myAddrToolStripMenuItem";
            myAddrToolStripMenuItem.Size = new Size(180, 22);
            myAddrToolStripMenuItem.Text = "MyAddr";
            myAddrToolStripMenuItem.Click += myAddrToolStripMenuItem_Click;
            // 
            // fortnuneToolStripMenuItem
            // 
            fortnuneToolStripMenuItem.Name = "fortnuneToolStripMenuItem";
            fortnuneToolStripMenuItem.Size = new Size(180, 22);
            fortnuneToolStripMenuItem.Text = "Fortnune";
            fortnuneToolStripMenuItem.Click += fortnuneToolStripMenuItem_Click;
            // 
            // toolStripMenuQr
            // 
            toolStripMenuQr.BackColor = SystemColors.ControlLightLight;
            toolStripMenuQr.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuQr.ForeColor = SystemColors.Desktop;
            toolStripMenuQr.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuQr.Name = "toolStripMenuQr";
            toolStripMenuQr.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuQr.ShortcutKeys = Keys.Alt | Keys.Q;
            toolStripMenuQr.Size = new Size(35, 21);
            toolStripMenuQr.Text = "Qr";
            // 
            // cryptToolStripMenu
            // 
            cryptToolStripMenu.BackColor = SystemColors.ControlLight;
            cryptToolStripMenu.BackgroundImageLayout = ImageLayout.None;
            cryptToolStripMenu.DisplayStyle = ToolStripItemDisplayStyle.Text;
            cryptToolStripMenu.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemEnDeCode, toolStripMenuItemCrypt });
            cryptToolStripMenu.ForeColor = SystemColors.Desktop;
            cryptToolStripMenu.ImageScaling = ToolStripItemImageScaling.None;
            cryptToolStripMenu.ImageTransparentColor = Color.Transparent;
            cryptToolStripMenu.Name = "cryptToolStripMenu";
            cryptToolStripMenu.Size = new Size(57, 21);
            cryptToolStripMenu.Text = "Crypt";
            // 
            // toolStripMenuItemEnDeCode
            // 
            toolStripMenuItemEnDeCode.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemEnDeCode.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItemEnDeCode.ForeColor = SystemColors.Desktop;
            toolStripMenuItemEnDeCode.ImageTransparentColor = SystemColors.Control;
            toolStripMenuItemEnDeCode.Name = "toolStripMenuItemEnDeCode";
            toolStripMenuItemEnDeCode.Size = new Size(180, 22);
            toolStripMenuItemEnDeCode.Text = "En-/Decode";
            toolStripMenuItemEnDeCode.Click += toolStripMenuItemEnDeCode_Click;
            // 
            // toolStripMenuItemCrypt
            // 
            toolStripMenuItemCrypt.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemCrypt.ForeColor = SystemColors.Desktop;
            toolStripMenuItemCrypt.Name = "toolStripMenuItemCrypt";
            toolStripMenuItemCrypt.Size = new Size(180, 22);
            toolStripMenuItemCrypt.Text = "En-/DeCrypt";
            toolStripMenuItemCrypt.Click += toolStripMenuItemCrypt_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = SystemColors.ControlLightLight;
            toolStripMenuItem1.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItem1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemAbout, toolStripMenuItemHelp, toolStripMenuItemInfo });
            toolStripMenuItem1.ForeColor = SystemColors.Desktop;
            toolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Padding = new Padding(3, 0, 3, 0);
            toolStripMenuItem1.ShortcutKeys = Keys.Alt | Keys.F7;
            toolStripMenuItem1.Size = new Size(24, 21);
            toolStripMenuItem1.Text = "?";
            // 
            // toolStripMenuItemAbout
            // 
            toolStripMenuItemAbout.BackColor = Color.Transparent;
            toolStripMenuItemAbout.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemAbout.ForeColor = SystemColors.Desktop;
            toolStripMenuItemAbout.ImageTransparentColor = Color.Transparent;
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
            toolStripMenuItemHelp.BackColor = SystemColors.ControlLightLight;
            toolStripMenuItemHelp.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemHelp.ForeColor = SystemColors.Desktop;
            toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            toolStripMenuItemHelp.Padding = new Padding(0, 2, 0, 2);
            toolStripMenuItemHelp.ShortcutKeys = Keys.Alt | Keys.H;
            toolStripMenuItemHelp.Size = new Size(166, 24);
            toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemInfo
            // 
            toolStripMenuItemInfo.BackColor = Color.Transparent;
            toolStripMenuItemInfo.BackgroundImageLayout = ImageLayout.None;
            toolStripMenuItemInfo.ForeColor = SystemColors.Desktop;
            toolStripMenuItemInfo.ImageTransparentColor = Color.Transparent;
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
            // TransparentFormCore8
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(784, 561);
            Controls.Add(menuStrip);
            Font = new Font("Lucida Sans Unicode", 10F);
            MainMenuStrip = menuStrip;
            Name = "TransparentFormCore8";
            Text = "TransparentFormCore8";
            TransparencyKey = SystemColors.Control;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripMenuMain;
        private ToolStripMenuItem toolStripMenuItemAbout;
        private ToolStripMenuItem toolStripMenuItemNew;
        private ToolStripMenuItem toolStripMenuItemOld;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItemOpen;
        private ToolStripMenuItem toolStripMenuItemClose;
        private ToolStripMenuItem toolStripMenuItemInfo;
        private ToolStripMenuItem toolStripMenuItemExit;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem toolStripMenuFile;
        private ToolStripMenuItem toolStripMenuItemLoad;
        private ToolStripMenuItem toolStripMenuItemSave;
        private ToolStripMenuItem toolStripMenuUnix;
        private ToolStripMenuItem toolStripMenuQr;
        private ToolStripMenuItem cryptToolStripMenu;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItemHelp;
        private ToolStripMenuItem toolStripMenuItemEnDeCode;
        private ToolStripMenuItem toolStripMenuItemCrypt;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem myAddrToolStripMenuItem;
        private ToolStripMenuItem fortnuneToolStripMenuItem;
    }
}