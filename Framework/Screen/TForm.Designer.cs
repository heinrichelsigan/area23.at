using Area23.At.Framework.Screen;.Properties;

namespace Area23.At.Framework.Screen;
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
            this.SuspendLayout();
            // 
            // TForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.DoubleBuffered = true;
            this.Icon = global::WinCRoach.Properties.Resources.TransparentFormsIcon;
            this.Name = "TForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TransparentStandardForm";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResizeEnd += new System.EventHandler(this.OnResizeEnd);
            this.LocationChanged += new System.EventHandler(this.OnLocationChanged);
            this.Enter += new System.EventHandler(this.OnLeave);
            this.Leave += new System.EventHandler(this.OnLeave);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

