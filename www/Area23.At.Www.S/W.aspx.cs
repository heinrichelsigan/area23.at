using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Area23.At.Www.S
{
    public partial class W : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string addr = string.Empty;
            foreach (char c in Request.UserHostAddress.ToString())
            {
                foreach (char vc in "0123456789abcdef:.")
                {
                    if (c == vc)
                    {
                        addr += c;
                        break;
                    }                            
                }
            }
            Area23Log.LogOriginMsg("Area23.at.Www.S", "W.aspx, Page_Load addr=" + addr);

            Bitmap bmp = (Bitmap)MergeImage(addr);
            string phypath = Server.MapPath("~/res/img/");
            string fName = DateTime.Now.Ticks + ".png";
            
            // TODO: change this
            ImageIp4.ImageUrl = "data:image/png;base64," + bmp.ToBase64(); // new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}")
            // ImageIp4.Width = 768;
            ImageIp4.Height = 200;
        }

        protected System.Drawing.Image MergeImage(string hexstring)
        {
            string phypath = Server.MapPath("~/res/img/");
            if (!phypath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                phypath += Path.DirectorySeparatorChar;
            string bmpName = phypath;
            ImageList list = new ImageList();
            Bitmap ximage = new Bitmap(384, 200);
            if (hexstring.Length > 8)
            {
                if (hexstring.Length <= 16)
                    ximage = new Bitmap(768, 200);
                else if (hexstring.Length <= 40)
                    ximage = new Bitmap(1920, 200);
                else if (hexstring.Length <= 60)
                {
                    ximage = new Bitmap(2880, 200);
                    hexstring = hexstring.Substring(0, 57) + "...";
                }
            }

 
            hexstring = hexstring.ToLower();

            System.Drawing.Bitmap mergeimg = new System.Drawing.Bitmap(720, 200);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int addInt = 0, w = 60, offset = 0;
                for (int i = 0; (i < hexstring.Length); i++)
                {
                    w = 60;
                    char ch = hexstring[i];
                    
                    if (ch == '0' || ch == '1' || ch == '2' || ch == '3'  || ch == '4' || 
                        ch == '5' || ch == '6' || ch == '7' || ch == '8' || ch == '9' || 
                        ch == 'a' || ch == 'b' || ch == 'c' || ch == 'd' || ch == 'e' || ch == 'f')
                        bmpName = phypath + ch.ToString() + ".png";                    
                    else if (ch == ':')
                    {
                        bmpName = phypath + "col.png";
                        w = 12;
                    }
                    else 
                    {
                        bmpName = phypath + "p.png";
                        w = 12;
                    }

                    ximage = new Bitmap(bmpName);
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 0, w, 200));
                    offset += w;
                }
            }

            return (mergeimg);
        }
    
    
    }
}