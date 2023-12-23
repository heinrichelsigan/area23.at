namespace Area23.At.WinForm.TWinFormCore
{
    partial class TWinForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TWinForm));
            SuspendLayout();
            // 
            // TWinForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(640, 480);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TWinForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "TWinFoirm";
            Load += OnLoad;
            ResizeEnd += OnResizeEnd;
            LocationChanged += OnLocationChanged;
            ResumeLayout(false);
        }

        #endregion
    }
}