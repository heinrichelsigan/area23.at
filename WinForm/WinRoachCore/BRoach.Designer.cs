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
        protected internal virtual void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BRoach));
            panelRoach = new Panel();
            SuspendLayout();
            // 
            // panelRoach
            // 
            panelRoach.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelRoach.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelRoach.BackColor = Color.Transparent;
            panelRoach.BackgroundImage = Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
            panelRoach.BackgroundImageLayout = ImageLayout.None;
            panelRoach.Location = new Point(0, 0);
            panelRoach.Margin = new Padding(0);
            panelRoach.Name = "panelRoach";
            panelRoach.Size = new Size(64, 64);
            panelRoach.TabIndex = 0;
            panelRoach.MouseDoubleClick += RoachExit;
            // 
            // BRoach
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(64, 64);
            ControlBox = false;
            Controls.Add(panelRoach);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MaximumSize = new Size(64, 64);
            MinimizeBox = false;
            Name = "BRoach";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            TransparencyKey = SystemColors.Control;
            Load += OnLoad;
            Shown += OnShow;
            MouseDoubleClick += RoachExit;
            ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.Panel panelRoach;
    }
}

