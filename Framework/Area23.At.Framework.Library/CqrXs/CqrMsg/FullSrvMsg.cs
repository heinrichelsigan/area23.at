using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{

    [JsonObject]
    [Serializable]
    public class FullSrvMsg<TC> : MsgContent where TC : class
    {
        #region properties

        public CqrContact Sender { get; set; }

        public CqrContact Recipient { get; set; }

        public TC TContent { get; set; }

        #endregion properties

        #region ctor

        public FullSrvMsg() : base()
        {
            _message = string.Empty;
            _rawMessage = string.Empty;
            _hash = string.Empty;
            Sender = null;
            Recipient = null;
            TContent = null;
        }

        public FullSrvMsg(string fm, MsgEnum msgArt = MsgEnum.JsonSerialized) : base()
        {
            this.FromJson<FullSrvMsg<TC>>(fm);
        }

        public FullSrvMsg(CqrContact sender, CqrContact to, TC tc) : base()
        {
            Sender = sender;
            Recipient = to;
            TContent = tc;
        }

        public FullSrvMsg(CqrContact sender, CqrContact to, TC tc, string hash) : base()
        {
            Sender = sender;
            Recipient = to;
            TContent = tc;
            _hash = hash;
        }

        #endregion ctor


        public override string ToJson()
        {
            string jsonText = JsonConvert.SerializeObject(this);
            this._rawMessage = jsonText;
            return jsonText;
        }

        public override TType FromJson<TType>(string jsonText) 
        {
            TType t = JsonConvert.DeserializeObject<TType>(jsonText);
            try
            {
                if (t != null && t is FullSrvMsg<TC> fullSrvMsg)
                {
                    if (fullSrvMsg != null && !string.IsNullOrEmpty(fullSrvMsg?.Message))
                    {
                        Sender = fullSrvMsg.Sender;
                        Recipient = fullSrvMsg.Recipient;
                        TContent = fullSrvMsg.TContent;
                    }
                    return t;
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogStatic(exJson);
            }

            return default(TType);
        }

    }

}
