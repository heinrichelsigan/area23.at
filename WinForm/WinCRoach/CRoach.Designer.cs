using Area23.At.WinForm.WinCRoach.Properties;

namespace Area23.At.WinForm.WinCRoach
{
    partial class CRoach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRoach));
            this.panelRoach = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelRoach
            // 
            this.panelRoach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRoach.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRoach.BackColor = System.Drawing.Color.Transparent;
            this.panelRoach.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelRoach.BackgroundImage")));
            this.panelRoach.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelRoach.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelRoach.Location = new System.Drawing.Point(0, 0);
            this.panelRoach.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoach.Name = "panelRoach";
            this.panelRoach.Size = new System.Drawing.Size(64, 64);
            this.panelRoach.TabIndex = 0;
            this.panelRoach.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RoachExit);
            // 
            // CRoach
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(64, 64);
            this.ControlBox = false;
            this.Controls.Add(this.panelRoach);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(64, 64);
            this.MinimizeBox = false;
            this.Name = "CRoach";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnLoad);
            this.Click += new System.EventHandler(this.AppExit);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RoachExit);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel panelRoach;
    }
}

