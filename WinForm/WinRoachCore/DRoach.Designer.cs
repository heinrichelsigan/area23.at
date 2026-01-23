using Area23.At.WinForm.WinRoachCore.Properties;

namespace Area23.At.WinForm.WinRoachCore
{
    partial class DRoach
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
            panelDRoach = new Panel();
            SuspendLayout();
            // 
            // panelDRoach
            // 
            panelDRoach.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelDRoach.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelDRoach.BackColor = Color.Transparent;
            panelDRoach.BackgroundImage = Resource.DRoach;
            panelDRoach.BackgroundImageLayout = ImageLayout.None;
            panelDRoach.Location = new Point(0, 0);
            panelDRoach.Margin = new Padding(0);
            panelDRoach.Name = "panelDRoach";
            panelDRoach.Size = new Size(64, 64);
            panelDRoach.TabIndex = 0;
            panelDRoach.MouseClick += RoachExit;
            // 
            // DRoach
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.DimGray;
            ClientSize = new Size(64, 64);
            ControlBox = false;
            Controls.Add(panelDRoach);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MaximumSize = new Size(64, 64);
            MinimizeBox = false;
            Name = "DRoach";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            TransparencyKey = Color.DimGray;
            Load += OnLoad;
            Shown += OnShow;
            MouseClick += RoachExit;
            ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel panelDRoach;
    }
}

