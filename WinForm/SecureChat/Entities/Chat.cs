using Area23.At.Framework.Library.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Entities
{
    /// <summary>
    /// Chat persistency
    /// </summary>
    internal class Chat : IDisposable
    {
        private static bool _disposed = false;

        // TODO: replace it in C# 9.0 to private static readonly lock _lock
        private static readonly object _lock = true;
        
        internal int ChatId { get; set; }  

        internal Contact Friend { get; set; }

        internal DateTime TimeStamp { get; set; }

        internal DateTime? SaveStamp { get; set; }

        internal HashSet<DateTime> MyMsgTStamps { get; set; }

        internal HashSet<DateTime> FriendMsgTStamps { get; set; } 

        internal ConcurrentDictionary<DateTime, string> CqrMsgs {  get; set; }


        public Chat()
        {
            TimeStamp = DateTime.Now;
            ChatId = 0;
            Friend = Entities.Settings.Instance.MyContact;
            MyMsgTStamps = new HashSet<DateTime>();
            FriendMsgTStamps = new HashSet<DateTime>();
            CqrMsgs = new ConcurrentDictionary<DateTime, string>();
        }

        public Chat(Contact friend) : this()
        {
            Friend = friend;
            ChatId = friend.ContactId;
            Chat? cqrChat = Chat.Load(ChatId);
            if (cqrChat != null)
            {
                this.CqrMsgs = cqrChat.CqrMsgs;
                this.FriendMsgTStamps = cqrChat.FriendMsgTStamps;
                this.MyMsgTStamps = cqrChat.MyMsgTStamps;
            }
        }

        public Chat(int chatId) : this()
        {
            ChatId = chatId;
            Chat? cqrChat = Chat.Load(ChatId);
            if (cqrChat != null)
            {
                this.TimeStamp = cqrChat.TimeStamp;
                this.CqrMsgs = cqrChat.CqrMsgs;
                this.FriendMsgTStamps = cqrChat.FriendMsgTStamps;
                this.MyMsgTStamps = cqrChat.MyMsgTStamps;
                this.SaveStamp = cqrChat.SaveStamp;
            }
        }


        internal static Chat? Load(int chatId)
        {
            string chatString = string.Empty;
            Chat? cqrchat = null;
            string fileName = LibPaths.SystemDirPath + String.Format(Constants.CQR_CHAT_FILE, chatId);
            try
            {
                if (!File.Exists(fileName) && Directory.Exists(LibPaths.AppPath))
                {
                    File.CreateText(fileName);
                }

                chatString = File.ReadAllText(fileName);
                cqrchat = JsonConvert.DeserializeObject<Chat>(chatString);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                // TODO: What shell we do with drunken saylor??
            }

            return cqrchat;
        }


        /// <summary>
        /// json serializes current chat and saves it to AppDomain.BaseDirectory + cqr{ContactId}chat.json
        /// </summary>
        /// <param name="cqrChat">secure Chat to save</param>
        /// <returns>true, if saving operation was successfully</returns>
        public static bool Save(Chat? cqrChat)
        {
            string saveString = string.Empty;
            if (cqrChat == null)
                return false;

            try
            {
                cqrChat.SaveStamp = DateTime.Now;
                saveString = JsonConvert.SerializeObject(cqrChat);
                string fileName = LibPaths.SystemDirPath + String.Format(Constants.CQR_CHAT_FILE, cqrChat.ChatId);
                File.WriteAllText(fileName, saveString);
            }
            catch (Exception ex)
            {
                CqrException.LastException = ex;
                return false;
            }
            return true;
        }

        #region Dispose() Dispose(bool disposing) ~Chat()

        public void Dispose()
        {
            Dispose(false);
        }

        public bool Dispose(bool disposing)
        {
            if (!_disposed || disposing)
            {
                lock (_lock)
                {                    
                    _disposed = Chat.Save(this);
                }
            }
            
            return _disposed;
        }

        ~Chat()
        {
            if (!Dispose(true))
            {                
                string fileName = LibPaths.SystemDirPath + String.Format(Constants.CQR_CHAT_FILE, ChatId);
                throw new CqrException($"~Chat(): couldn't save chat {ChatId} to {fileName}.", CqrException.LastException);
            }
                
            _disposed = true;
            SaveStamp = null;
            CqrMsgs.Clear();
            FriendMsgTStamps.Clear();
            MyMsgTStamps.Clear();
        }


        #endregion Dispose() Dispose(bool disposing) ~Chat()
    }
}
