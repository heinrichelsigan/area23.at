using Area23.At.WinForm.TransparentForms.Properties;
using System.Windows.Forms;

namespace Area23.At.WinForm.TransparentForms.Gui.TForms
{
    partial class TForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuItemMain = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAsteriks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemApps = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFortune = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.statusStrip.Location = new System.Drawing.Point(0, 419);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(624, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "Status";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemMain,
            this.menuItemApps,
            this.menuItemAsteriks});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(624, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // menuItemMain
            // 
            this.menuItemMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemNew,
            this.menuItemReload,
            this.menuItemRestart,
            this.menuItemExit});
            this.menuItemMain.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.menuItemMain.Name = "menuItemMain";
            this.menuItemMain.Size = new System.Drawing.Size(51, 20);
            this.menuItemMain.Text = "Main";
            // 
            // menuItemNew
            // 
            this.menuItemNew.Name = "menuItemNew";
            this.menuItemNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.N)));
            this.menuItemNew.Size = new System.Drawing.Size(193, 22);
            this.menuItemNew.Text = "New";
            // 
            // menuItemReload
            // 
            this.menuItemReload.Name = "menuItemReload";
            this.menuItemReload.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuItemReload.Size = new System.Drawing.Size(193, 22);
            this.menuItemReload.Text = "Reload";
            this.menuItemReload.Click += new System.EventHandler(this.menuItemReload_Click);
            // 
            // menuItemRestart
            // 
            this.menuItemRestart.Name = "menuItemRestart";
            this.menuItemRestart.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.menuItemRestart.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.menuItemRestart.Size = new System.Drawing.Size(193, 22);
            this.menuItemRestart.Text = "Restart";
            this.menuItemRestart.Click += new System.EventHandler(this.menuItemRestart_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(193, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemAsteriks
            // 
            this.menuItemAsteriks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemHelp,
            this.menuItemInfo,
            this.menuItemAbout});
            this.menuItemAsteriks.Name = "menuItemAsteriks";
            this.menuItemAsteriks.Size = new System.Drawing.Size(27, 20);
            this.menuItemAsteriks.Text = "?";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.menuItemHelp.Size = new System.Drawing.Size(180, 22);
            this.menuItemHelp.Text = "Help";
            // 
            // menuItemInfo
            // 
            this.menuItemInfo.Name = "menuItemInfo";
            this.menuItemInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F3)));
            this.menuItemInfo.Size = new System.Drawing.Size(180, 22);
            this.menuItemInfo.Text = "Info";
            this.menuItemInfo.Click += new System.EventHandler(this.menuItemInfo_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuItemAbout.Size = new System.Drawing.Size(180, 22);
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // menuItemApps
            // 
            this.menuItemApps.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFortune});
            this.menuItemApps.Name = "menuItemApps";
            this.menuItemApps.Size = new System.Drawing.Size(51, 20);
            this.menuItemApps.Text = "Apps";
            // 
            // menuItemFortune
            // 
            this.menuItemFortune.Name = "menuItemFortune";
            this.menuItemFortune.Size = new System.Drawing.Size(180, 22);
            this.menuItemFortune.Text = "Fortune";
            this.menuItemFortune.Click += new System.EventHandler(this.menuItemFortune_Click);
            // 
            // TForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "TForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TransparentForm";
            this.TransparencyKey = System.Drawing.SystemColors.ControlLight;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemMain;
        private ToolStripMenuItem menuItemNew;
        private ToolStripMenuItem menuItemReload;
        private ToolStripMenuItem menuItemRestart;
        private ToolStripMenuItem menuItemExit;
        private ToolStripMenuItem menuItemAsteriks;
        private ToolStripMenuItem menuItemHelp;
        private ToolStripMenuItem menuItemInfo;
        private ToolStripMenuItem menuItemAbout;
        private ToolStripMenuItem menuItemApps;
        private ToolStripMenuItem menuItemFortune;
    }
}

