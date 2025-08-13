using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cqr.Msg
{
    [Serializable]
    public class CChatRoom : CContent, IMsgAble
    {
        public Guid ChatRuid { get; set; }

        public string ChatRoomNr { get; set; }

        public List<long> TicksLong { get; set; }

        public DateTime LastPushed { get; set; }

        public DateTime LastPolled { get; set; }

        public List<string> InvitedEmails { get; set; }

        #region ICqrMessagable interface

        // public new CType MsgType => CType.Json;

        //public new string RawMessage { get => ToJson(); set; }        

        // public new string Md5Hash { get => MD5Sum.HashString(_message); set; }

        #endregion ICqrMessagable interface

        #region ctor

        public CChatRoom() : base()
        {
            ChatRuid = Guid.Empty;
            InvitedEmails = new List<string>();
            ChatRoomNr = "";
            Message = "";
            TicksLong = new List<long>();
            LastPushed = DateTime.MinValue;
            LastPolled = DateTime.MinValue;
            Hash = "";
            Md5Hash = "";
            MsgType = CType.None;
            CBytes = new byte[0];
        }

        public CChatRoom(string chatRoomNr) : this()
        {
            ChatRoomNr = chatRoomNr;
            ChatRuid = Guid.NewGuid();
            LastPushed = DateTime.MinValue;
            LastPolled = DateTime.MinValue;
            TicksLong = new List<long>();
        }

        public CChatRoom(string chatRoomNr, Guid chatRuid, DateTime lastPushed, DateTime lastPolled) : this()
        {
            ChatRoomNr = chatRoomNr;
            ChatRuid = (chatRuid == Guid.Empty) ? Guid.NewGuid() : chatRuid;
            LastPushed = lastPushed;
            LastPolled = lastPolled;
            TicksLong = new List<long>();
        }

        public CChatRoom(string chatRoomNr, Guid chatRuid, DateTime lastPushed, DateTime lastPolled, List<long> ticks, List<string> invited, string hash, string md5sum, byte[] bytes) : this()
        {
            ChatRoomNr = chatRoomNr;
            ChatRuid = (chatRuid == Guid.Empty) ? Guid.NewGuid() : chatRuid;
            LastPushed = lastPushed;
            LastPolled = lastPolled;
            TicksLong = new List<long>(ticks);
            InvitedEmails = new List<string>(invited);
            Hash = hash;
            Md5Hash = md5sum;
            CBytes = bytes;
            MsgType = CType.Json;
            SerializedMsg = "";
            SerializedMsg = this.ToJson();
        }

        public CChatRoom(CChatRoom chatRoom) : this()
        {
            if (chatRoom != null)
            {
                CCopy(this, chatRoom);
            }
        }

        #endregion ctor


        public new CChatRoom CCopy(CChatRoom leftDest, CChatRoom rightSrc)
        {
            return CloneCopy(rightSrc, leftDest);
        }

        public new static CChatRoom CloneCopy(CChatRoom source, CChatRoom destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CChatRoom(source);

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.ChatRoomNr = source.ChatRoomNr;
            destination.ChatRuid = source.ChatRuid;
            destination.TicksLong = source.TicksLong;
            destination.LastPolled = source.LastPolled;
            destination.LastPushed = source.LastPushed;
            destination.InvitedEmails = source.InvitedEmails;
            destination.SerializedMsg = "";
            destination.SerializedMsg = destination.ToJson();

            return destination;
        }


        #region members

        public override string ToJson()
        {
            // CqrContact cqrContact = new CqrContact(ContactId, Cuid, Name, Email, Mobile, Address, ContactImage);
            this.SerializedMsg = "";
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this.SerializedMsg = jsonString;
            return jsonString;
        }

        public new CChatRoom FromJson(string jsonText)
        {
            CChatRoom ccRoom = null;
            try
            {
                ccRoom = JsonConvert.DeserializeObject<CChatRoom>(jsonText);
                if (ccRoom != null && ccRoom.ChatRuid != Guid.Empty && !string.IsNullOrEmpty(ccRoom.ChatRoomNr))
                {
                    return CCopy(this, ccRoom);
                }
            }
            catch (Exception exJson)
            {
                SLog.Log(exJson);
            }

            return ccRoom;

        }

        public override string ToXml()
        {
            SerializedMsg = "";
            string xmlString = Utils.SerializeToXml<CChatRoom>(this);
            SerializedMsg = xmlString;
            return xmlString;
        }

        public new CChatRoom FromXml(string xmlText)
        {
            CChatRoom chatRoom = base.FromXml<CChatRoom>(xmlText);
            if (chatRoom != null && chatRoom.ChatRuid != Guid.Empty && !string.IsNullOrEmpty(chatRoom.ChatRoomNr))
            {
                return CCopy(this, chatRoom);
            }

            return chatRoom;
        }


        public override string ToString()
        {

            return
                "\"ChatRuid\": \t\"" + ChatRuid + "\";" + Environment.NewLine +
                "\"ChatRoomNr\": \t\"" + ChatRoomNr + "\";" + Environment.NewLine +
                "\"TicksLong\": \t\"" + String.Join(",", TicksLong) + "\";" + Environment.NewLine +
                "\"LastPushed\": \t\"" + LastPushed ?? "" + "\";" + Environment.NewLine +
                "\"LastPolled\": \t\"" + LastPolled ?? "" + "\";" + Environment.NewLine +
                "\"_message\": \t\"" + Message ?? "" + "\";" + Environment.NewLine +
                "\"Hash\": \t\"" + Hash ?? "" + "\";" + Environment.NewLine +
                "\"SerializedMsg\": \t\"" + SerializedMsg ?? "" + "\";" + Environment.NewLine +
                "\"Md5Hash\": \t\"" + Md5Hash ?? "" + "\";" + Environment.NewLine;
        }

        #endregion members
    }

}
