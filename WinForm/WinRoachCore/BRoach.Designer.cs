using Area23.At.WinForm.WinRoachCore.Properties;

namespace Area23.At.WinForm.WinRoachCore
{
    partial class BRoach
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
            panelBRoach = new Panel();
            SuspendLayout();
            // 
            // panelBRoach
            // 
            panelBRoach.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBRoach.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelBRoach.BackColor = Color.Transparent;
            panelBRoach.BackgroundImage = Resource.CRoach;
            panelBRoach.BackgroundImageLayout = ImageLayout.None;
            panelBRoach.Location = new Point(0, 0);
            panelBRoach.Margin = new Padding(0);
            panelBRoach.Name = "panelBRoach";
            panelBRoach.Size = new Size(64, 64);
            panelBRoach.TabIndex = 0;
            panelBRoach.MouseDoubleClick += RoachExit;
            // 
            // BRoach
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ControlLightLight;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(64, 64);
            ControlBox = false;
            Controls.Add(panelBRoach);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MaximumSize = new Size(64, 64);
            MinimizeBox = false;
            Name = "BRoach";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            TransparencyKey = SystemColors.ControlLightLight;
            Load += OnLoad;
            Shown += OnShow;
            MouseDoubleClick += RoachExit;
            ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.Panel panelBRoach;
    }
}

