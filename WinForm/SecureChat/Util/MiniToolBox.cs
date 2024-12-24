using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Util
{
    internal static class MiniToolBox
    {

        internal static string ShowZenMatrixPermutation(string secretKey, string iv = "")
        {
            ZenMatrix.ZenMatrixGenWithKey(secretKey, iv, true);
            string zmVisualize = "| 0 | => | ";

            foreach (sbyte sb in ZenMatrix.PermKeyHash)
            {
                zmVisualize += sb.ToString("x1") + " ";
            }
            zmVisualize += "| \r\n";
            for (int zeni = 1; zeni < ZenMatrix.PermKeyHash.Count; zeni++)
            {
                sbyte sb = (sbyte)ZenMatrix.PermKeyHash.ElementAt(zeni);
                zmVisualize += "| " + zeni.ToString("x1") + " | => | " + sb.ToString("x1") + " | " + "\r\n";
            }
            // this.TextBoxDestionation.Text += ZenMatrix.EncryptString(this.richTextBoxChat.Text) + "\n";

            return zmVisualize;
        }

        //internal static byte[] ScrambleBytes(params byte[] bytes)
        //{
        //    foreach ()
        //}
        
    }
}
