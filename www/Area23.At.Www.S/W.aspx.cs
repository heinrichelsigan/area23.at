using System;
using System.Collections.Generic;
using System.Drawing;
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

        }

        protected System.Drawing.Image GetMergedImage(string hexstring)
        {
            string phypath = Server.MapPath("~/res/img/");
            ImageList list = new ImageList();
            Bitmap x1Image = new Bitmap(720, 200);
            /*
            for (int i = 0; (i < bytes.Length && i < 12); i++)
            {
                
                string bmpName = phypath + Path.PathSeparator;
                int addInt = 0;
                char ch = hexstring[i];
                if ((((int)ch) >= ((int)'1')) && (((int)ch) <= ((int)'9')))
                    addInt = (((int)ch) - ((int)'1') + 1);
                if (addInt > 0)
                    bmpName += addInt.ToString() + ".png";
                else if (ch == '0' || ch == 'a' || ch == 'b' || ch == 'c' || ch == 'd' || ch == 'e' || ch == 'f')
                    bmpName += ch.ToString() + ".png";

                x1Image = new Bitmap(bmpName);

            }
  
            list.Images.Add()
            
                .
            System.Drawing.Image mergedImage = new System.Drawing.Bitmap(720, 200);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergedImage))
            {
                int offSetX = 0;
                int offSetY = 0;
                g.DrawImage(x1Image, new System.Drawing.Rectangle(offSetX, offSetY, x1Image.Width, x1Image.Height));
                offSetX = 710;
                offSetY = 59;
                g.DrawImage(menuImg, new System.Drawing.Rectangle(offSetX, offSetY, menuImg.Width, menuImg.Height));
                offSetX = 23;
                offSetY = 17;
                g.DrawImage(headerImg, new System.Drawing.Rectangle(offSetX, offSetY, headerImg.Width, headerImg.Height));
                offSetX = 710;
                offSetY = 630;
                g.DrawImage(qsImg, new System.Drawing.Rectangle(offSetX, offSetY, qsImg.Width, qsImg.Height));
                offSetX = 840;
                offSetY = 443;
                g.DrawImage(stnCntImg, new System.Drawing.Rectangle(offSetX, offSetY, stnCntImg.Width, stnCntImg.Height));
                offSetX = 710;
                offSetY = 528;
                g.DrawImage(picMsgImg, new System.Drawing.Rectangle(offSetX, offSetY, picMsgImg.Width, picMsgImg.Height));
            }
            */
            return (x1Image);
        }
    
    
    }
}