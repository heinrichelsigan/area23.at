using Area23.At.WinForm.WinRoachCore.Properties;

namespace Area23.At.WinForm.WinRoachCore
{
    partial class FRoach
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
            this.panelFRoach = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelRoach
            // 
            this.panelFRoach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFRoach.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFRoach.BackColor = System.Drawing.Color.Transparent;
            this.panelFRoach.BackgroundImage = (System.Drawing.Image)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
            this.panelFRoach.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelFRoach.Location = new System.Drawing.Point(0, 0);
            this.panelFRoach.Margin = new System.Windows.Forms.Padding(0);
            this.panelFRoach.Name = "panelFRoach";
            this.panelFRoach.Size = new System.Drawing.Size(64, 64);
            this.panelFRoach.TabIndex = 0;
            this.panelFRoach.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RoachExit);
            // 
            // CRoach
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(64, 64);
            this.ControlBox = false;
            this.Controls.Add(this.panelFRoach);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(64, 64);
            this.MinimizeBox = false;
            this.Name = "FRoach";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnLoad);
            this.Click += new System.EventHandler(this.AppExit);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RoachExit);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel panelFRoach;
    }
}

