using Area23.At.Framework.Library.Util;
using Area23.At.WinForm.TransparentForms.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.TransparentForms.Gui.TForms
{
    public partial class Form1 : Form
    {
        StreamByteEncoding streamByteEncoding = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1 == null)
            {
                this.openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
                
            openFileDialog1.Filter = "all files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            DialogResult dia = openFileDialog1.ShowDialog(this);
            if (dia == DialogResult.OK) 
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    byte[] bytes = File.ReadAllBytes(openFileDialog1.FileName);
                    string name = Path.GetFileName(openFileDialog1.FileName);
                    streamByteEncoding = new StreamByteEncoding(name, bytes);
                    this.pictureBox1.Image = streamByteEncoding.AnImage;
                }
            }
        }

        private void buttonSaveImage_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1 == null)
            {
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                saveFileDialog1.RestoreDirectory = true;
            }
            DialogResult dia = saveFileDialog1.ShowDialog( this);
            if (dia == DialogResult.OK)
            {
                if (!File.Exists(saveFileDialog1.FileName))
                    MessageBox.Show("Error saving file to " +  saveFileDialog1.FileName, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonToJson_Click(object sender, EventArgs e)
        {
            if (streamByteEncoding != null && streamByteEncoding.Base64Enc != null)
            {
                this.textBox2.Text = streamByteEncoding.ToJson();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text.ToLower())
            {
                case "byte[]":
                    this.textBox2.Text = streamByteEncoding.Bytes.ToHexString(); break;
                case "stream":
                    this.textBox2.Text = streamByteEncoding.MemStream.ToString(); break;
                case "base64":
                    this.textBox2.Text = streamByteEncoding.Base64Enc.ToString(); break;
                case "name":
                    this.textBox2.Text = streamByteEncoding.Name.ToString(); break;
                case "base64type":
                case "mimetype":
                    this.textBox2.Text = streamByteEncoding.Base64Type.ToString(); break;
                case "json":
                    this.textBox2.Text = streamByteEncoding.ToJson();
                    break;
                default:
                    this.textBox2.Text = string.Empty;
                    break;
            }
        }
    }
}
