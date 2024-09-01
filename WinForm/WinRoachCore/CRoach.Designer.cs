using Area23.At.WinForm.WinRoachCore.Properties;

namespace Area23.At.WinForm.WinRoachCore
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
            this.panelCRoach = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelCRoach
            // 
            this.panelCRoach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCRoach.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelCRoach.BackColor = System.Drawing.Color.Transparent;
            this.panelCRoach.BackgroundImage = (System.Drawing.Image)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
            this.panelCRoach.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelCRoach.Location = new System.Drawing.Point(0, 0);
            this.panelCRoach.Margin = new System.Windows.Forms.Padding(0);
            this.panelCRoach.Name = "panelCRoach";
            this.panelCRoach.Size = new System.Drawing.Size(64, 64);
            this.panelCRoach.TabIndex = 0;
            this.panelCRoach.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RoachExit);
            // 
            // CRoach
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(64, 64);
            this.ControlBox = false;
            this.Controls.Add(this.panelCRoach);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(64, 64);
            this.MinimizeBox = false;
            this.Name = "CRoach";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = false;
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShow);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RoachExit);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel panelCRoach;
    }
}

