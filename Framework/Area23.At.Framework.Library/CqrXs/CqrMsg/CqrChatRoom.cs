using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{


    /// <summary>
    /// CqrChatRoom is a contact for CqrJd
    /// </summary>
    [Serializable]
    public class CqrChatRoom : MsgContent, ICqrMessagable
    {       

        public Guid ChatRuid { get; set; }

        public string ChatRoomNr { get; set; }

        public List<long> TicksLong { get; set; }

        public DateTime LastPushed { get; set; }

        public DateTime LastPolled { get; set; }

        #region ICqrMessagable interface

        public new MsgEnum MsgType => MsgEnum.Json;

        //public new string RawMessage { get => ToJson(); set; }

        public new string Hash => _hash;

        // public new string Md5Hash { get => MD5Sum.HashString(_message); set; }

        #endregion ICqrMessagable interface

        public CqrChatRoom() : base()
        {
            ChatRuid = Guid.NewGuid();
            ChatRoomNr = "";
            _message = "";
            TicksLong = new List<long>();
            LastPushed = DateTime.MinValue;
            LastPolled = DateTime.MinValue;            
            _hash = "";
        }


        #region members

        public override string ToJson()
        {
            // CqrContact cqrContact = new CqrContact(ContactId, Cuid, Name, Email, Mobile, Address, ContactImage);
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this.RawMessage = jsonString;
            return jsonString;
        }

        public override T FromJson<T>(string jsonText) 
        {
            T tt = default(T);
            try
            {
                tt = JsonConvert.DeserializeObject<T>(jsonText);
                if (tt != null && tt is CqrChatRoom chatRoom)
                {
                    if (chatRoom != null && chatRoom.ChatRuid != Guid.Empty && !string.IsNullOrEmpty(chatRoom.ChatRoomNr))
                    {
                        ChatRuid = chatRoom.ChatRuid;
                        ChatRoomNr = chatRoom.ChatRoomNr;
                        TicksLong = chatRoom.TicksLong;
                        LastPushed = chatRoom.LastPushed;
                        LastPolled = chatRoom.LastPolled;
                        _message = chatRoom._message;
                        _hash = chatRoom._hash;                        

                        return (T)tt;
                    }
                }
            }
            catch (Exception exJson)
            {
                SLog.Log(exJson);
            }

            return (T)tt;

        }

        public override string ToXml() => this.ToXml();

        public override T FromXml<T>(string xmlText) 
        {
            T cqrT = base.FromXml<T>(xmlText);
            if (cqrT is CqrChatRoom chatRoom && chatRoom != null && chatRoom.ChatRuid != Guid.Empty && !string.IsNullOrEmpty(chatRoom.ChatRoomNr))
            {
                ChatRuid = chatRoom.ChatRuid;
                ChatRoomNr = chatRoom.ChatRoomNr;
                TicksLong = chatRoom.TicksLong;
                LastPushed = chatRoom.LastPushed;
                LastPolled = chatRoom.LastPolled;
                _message = chatRoom._message;
                _hash = chatRoom._hash;
              

                return (T)cqrT;
            }

            return cqrT;

        }


        public override string ToString()
        {

            return
                "\"ChatRuid\": \t\"" + ChatRuid + "\";" + Environment.NewLine +
                "\"ChatRoomNr\": \t\"" + ChatRoomNr + "\";" + Environment.NewLine +
                "\"TicksLong\": \t\"" + String.Join(",", TicksLong) + "\";" + Environment.NewLine +
                "\"LastPushed\": \t\"" + LastPushed ?? "" + "\";" + Environment.NewLine +
                "\"LastPolled\": \t\"" + LastPolled ?? "" + "\";" + Environment.NewLine +
                "\"_message\": \t\"" + _message ?? "" + "\";" + Environment.NewLine +
                "\"_hash\": \t\"" + _hash ?? "" + "\";" + Environment.NewLine +
                "\"RawMessage\": \t\"" + RawMessage ?? "" + "\";" + Environment.NewLine +
                "\"Md5Hash\": \t\"" + Md5Hash ?? "" + "\";" + Environment.NewLine;
        }

        #endregion members

    }


}
