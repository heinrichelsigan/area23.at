using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Provides a simple crypt pipe for <see cref="SymmCipherEnum"/>
    /// </summary>
    public class SymmCipherPipe
    {

        private readonly SymmCipherEnum[] inPipe;

        public readonly SymmCipherEnum[] outPipe;

        public SymmCipherEnum[] InPipe { get => inPipe; }

        public SymmCipherEnum[] OutPipe { get => outPipe; }

        public string PipeString
        {
            get
            {
                string hashSymms = string.Empty;
                foreach (SymmCipherEnum symmCipher in inPipe)
                    hashSymms += symmCipher.GetSymmCipherChar();

                return hashSymms;
            }
        }

        public SymmCipherPipe(SymmCipherEnum[] symmCipherEnums)
        {
            inPipe = new List<SymmCipherEnum>(symmCipherEnums).ToArray();
            outPipe = symmCipherEnums.Reverse<SymmCipherEnum>().ToArray();                        
        }

        /// <summary>
        /// SymmCipherPipe ctor with array of user key bytes
        /// </summary>
        /// <param name="keyBytes">user key bytes</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        public SymmCipherPipe(byte[] keyBytes, uint maxpipe = 8)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            ushort scnt = 0;
            List<SymmCipherEnum> pipeList = new List<SymmCipherEnum>();
            Dictionary<char, SymmCipherEnum> symDict = new Dictionary<char, SymmCipherEnum>();
            foreach (SymmCipherEnum symmC in Enum.GetValues(typeof(SymmCipherEnum)))
            {
                string hex = $"{((ushort)symmC):x1}";
                scnt++;
                symDict.Add(hex[0], symmC);
            }

            string hexString = string.Empty;
            HashSet<char> hashBytes = new HashSet<char>();
            foreach (byte bb in keyBytes)
            {
                hexString = string.Format("{0:x2}", bb);
                if (hexString.Length > 0 && !hashBytes.Contains(hexString[0]))
                    hashBytes.Add(hexString[0]);
                if (hexString.Length > 0 && !hashBytes.Contains(hexString[1]))
                    hashBytes.Add(hexString[1]);
            }

            hexString = string.Empty;
            for (int kcnt = 0; kcnt < hashBytes.Count && pipeList.Count < maxpipe; kcnt++)
            {
                hexString += hashBytes.ElementAt(kcnt).ToString();
                SymmCipherEnum sym0 = symDict[hashBytes.ElementAt(kcnt)];
                pipeList.Add(sym0);
            }

            inPipe = new List<SymmCipherEnum>(pipeList).ToArray();
            outPipe = pipeList.Reverse<SymmCipherEnum>().ToArray();

        }

    }

}
