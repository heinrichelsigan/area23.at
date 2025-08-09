using Newtonsoft.Json;
using System;

namespace Area23.At.Framework.Library.Cache
{

    [Serializable]
    public class CacheTestData
    {
        static byte[] buffer = new byte[16384];
        static Random random = new Random((DateTime.Now.Millisecond + 1) * (DateTime.Now.Second + 1));
        
        [JsonIgnore]
        protected internal int CIndex { get; set; }
        public string CKey { get; set; }
        public string CValue { get; set; }
        public int CThreadId { get; set; }
        public DateTime CTime { get; set; }

        static CacheTestData()
        {
            random.NextBytes(buffer);
        }

        public CacheTestData()
        {
            CIndex = 0;
            CValue = string.Empty;
            CKey = string.Empty;
            CTime = DateTime.MinValue;
            CThreadId = -1;
        }

        public CacheTestData(string ckey) : this()
        {
            CKey = ckey;
            CIndex = Int32.Parse(ckey.Replace("Key_", ""));
            CValue = GetRandomString(CIndex);
            CTime = DateTime.Now;
        }

        public CacheTestData(string ckey, int cThreadId) : this(ckey)
        {
            CThreadId = cThreadId;
        }

        public CacheTestData(int cThreadId) : this(string.Concat("Key_", cThreadId))
        {
            CThreadId = cThreadId;
        }


        internal string GetRandomString(int idx, int size = 64, bool newRandom = true)
        {
            if (newRandom) 
                random.NextBytes(buffer);

            int byteLen = ((idx + size) > buffer.Length) ? buffer.Length - idx : size;
            byte[] restBytes = new byte[byteLen];
            
            Array.Copy(buffer, idx, restBytes, 0, byteLen);
            return Convert.ToBase64String(restBytes, 0, byteLen);
        }

    }


}
