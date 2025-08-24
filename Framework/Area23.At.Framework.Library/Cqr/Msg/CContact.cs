using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Area23.At.Framework.Library.Cqr.Msg
{

    [Serializable]
    public class CContact : CContent, IMsgAble
    {

        #region properties
        public int ContactId { get; set; }

        public Guid Cuid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        [JsonIgnore]
        protected string SecretKey { get; set; }

        public CImage ContactImage { get; set; }

        [JsonIgnore]
        public string NameEmail { get => string.IsNullOrEmpty(Email) ? Name : $"{Name} <{Email}>"; }

        #endregion properties

        #region constructors

        /// <summary>
        /// Parameterless default constructor
        /// </summary>
        public CContact() : base()
        {
            ContactId = -1;
            Cuid = Guid.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            Address = string.Empty;
            SecretKey = string.Empty;
            ContactImage = null;
        }

        /// <summary>
        /// ctor with serialized string and serialization type
        /// </summary>
        /// <param name="serialized">serialized or mime string</param>
        /// <param name="serType">serialized type</param>
        public CContact(string serialized, SerType serType = SerType.Json)
        {
            switch (serType)
            {
                case SerType.Xml:
                    FromXml<CContact>(serialized);
                    break;
                case SerType.Raw:     // TODO= implement it
                case SerType.None:    // TODO= implement it
                    break;
                case SerType.Mime:
                    string json = Encoding.UTF8.GetString(Convert.FromBase64String(serialized));
                    FromJson<CContact>(json);
                    break;
                case SerType.Json:
                default:
                    FromJson<CContact>(serialized);
                    break;
            }
        }

        public CContact(int cid, string name, string email, string mobile, string address) : base()
        {
            ContactId = cid;
            Name = name;
            Email = email;
            Mobile = mobile;
            Address = address;
            SecretKey = string.Empty;
        }

        public CContact(Guid guid, string name, string email, string mobile, string address) : base()
        {
            Cuid = guid;
            Name = name;
            Email = email;
            Mobile = mobile;
            Address = address;
            SecretKey = string.Empty;
        }

        public CContact(int cid, string name, string email, string mobile, string address, CImage cqrImage)
            : this(cid, name, email, mobile, address)
        {
            ContactImage = cqrImage;
        }

        public CContact(int cid, Guid cuid, string name, string email, string mobile, string address, CImage cqrImage)
            : this(cid, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = cqrImage;
        }

        public CContact(int cid, Guid cuid, string name, string email, string mobile, string address, CImage cqrImage, string hash)
            : this(cid, cuid, name, email, mobile, address, cqrImage)
        {
            Hash = hash;
        }

        public CContact(int cid, string name, string email, string mobile, string address, Image image)
            : this(cid, name, email, mobile, address)
        {
            ContactImage = CImage.FromDrawingImage(image);
        }

        public CContact(int cid, Guid cuid, string name, string email, string mobile, string address, Image image)
            : this(cid, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = CImage.FromDrawingImage(image);
        }

        public CContact(int cid, Guid cuid, string name, string email, string mobile, string address, Image image, string hash)
            : this(cid, cuid, name, email, mobile, address, image)
        {
            this.Hash = hash;
        }

        public CContact(CContact ccntct, string hash)
            : this(ccntct.ContactId, ccntct.Cuid, ccntct.Name, ccntct.Email, ccntct.Mobile, ccntct.Address, ccntct.ContactImage, hash)
        {
            CloneCopy(ccntct, this);
            this.Hash = hash;
            // SerializedMsg = "";
            // SerializedMsg = this.ToJson();
        }

        public CContact(CContact ccntct, string ChatRoomNr, string hash) : this(ccntct, hash)
        {
            CloneCopy(ccntct, this);
            Hash = hash;
            ContactImage = null;
            Cuid = (ccntct.Cuid == Guid.Empty) ? Guid.NewGuid() : ccntct.Cuid;
            Message = ChatRoomNr;
            // SerializedMsg = "";
            // SerializedMsg = this.ToJson();
        }

        public CContact(CContact ccntct)
        {
            CloneCopy(ccntct, this);
            Cuid = (ccntct.Cuid == Guid.Empty) ? Guid.NewGuid() : ccntct.Cuid;
            // SerializedMsg = "";
            // SerializedMsg = this.ToJson();
        }

        public CContact(CContact ccntct, string chatRoomNr, string hash, CImage cqrImage) : this(ccntct, chatRoomNr, hash)
        {
            CloneCopy(ccntct, this);
            Cuid = (ccntct.Cuid == Guid.Empty) ? Guid.NewGuid() : ccntct.Cuid;
            Hash = hash;
            ContactImage = cqrImage;
            Message = chatRoomNr;
            // SerializedMsg = "";
            // SerializedMsg = this.ToJson();      
        }

        #endregion constructors


        #region EnDeCrypt+DeSerialize

        public override string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (Encrypt(serverKey, encoder, zipType))
            {
                // this.SerializedMsg = ToJson();
                return this.SerializedMsg;
            }
            throw new CqrException($"EncryptToJson(string severKey) failed for CContact.");
        }



        public new CContact DecryptFromJson(
            string serverKey,
            string serialized = "",
            EncodingType decoder = EncodingType.Base64,
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            CContact contact = JsonConvert.DeserializeObject<CContact>(serialized);

            if (contact != null && contact.Decrypt(serverKey, decoder, zipType))
            {
                CloneCopy(contact, this);
                return contact;
            }
            throw new CqrException($"DecryptFromJson<T>(string severKey, string serialized) failed for CContact");
        }


        #endregion EnDeCrypt+DeSerialize

        #region members

        public override CContent CCopy(CContent leftDest, CContent rightSrc)
        {
            if (leftDest is CContact && rightSrc is CContact)
                return CloneCopy(rightSrc, leftDest);

            return base.CCopy(leftDest, rightSrc);
        }

        public override string ToXml() => Utils.SerializeToXml<CContact>(this);


        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="DataGridView"/>
        /// </summary>
        public object[] GetRowParams()
        {
            List<object> oList = new List<object>();
            oList.Add(ContactId);
            oList.Add(Cuid);
            oList.Add(Name);
            oList.Add(Email);
            oList.Add(Mobile);
            oList.Add(Address);
            return oList.ToArray();
        }

        #endregion members


        #region static members 

        #region static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        /// <summary>
        /// ToJsonEncrypt
        /// </summary>
        /// <param name="serverKey">server key to encrypt</param>
        /// <param name="ccntct"><see cref="CContact"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CContact"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string ToJsonEncrypt(
            string serverKey,
            CContact ccntct,
            EncodingType encoder = EncodingType.Base64,
            Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(serverKey) || ccntct == null)
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, CContact ccntct) failed: NULL reference!");

            if (!EncryptSrvMsg(serverKey, ref ccntct, encoder, zipType))
                throw new CqrException($"static string ToJsonEncrypt(string severKey, CContact ccntct) failed.");

            string serializedJson = ccntct.ToJson();
            return serializedJson;
        }

        public static bool EncryptSrvMsg(
            string serverKey,
            ref CContact ccntct,
            EncodingType encoder = EncodingType.Base64,
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            string encrypted = "", pipeString = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                pipeString = (new SymmCipherPipe(serverKey, keyHash)).PipeString;

                encrypted = SymmCipherPipe.EncrpytToString(ccntct.Message, serverKey, out pipeString, encoder, zipType);
                ccntct.Hash = pipeString;
                ccntct.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, keyHash, pipeString, ccntct.Message), "");

                ccntct.Message = encrypted;
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }

        /// <summary>
        /// FromJsonDecrypt
        /// </summary>
        /// <param name="serverKey">server key to decrypt</param>
        /// <param name="serialized">serialized string of <see cref="CContact"/></param>
        /// <returns>deserialized and decrypted <see cref="CContact"/></returns>
        /// <exception cref="CqrException">thrown, 
        /// when serialized string to decrypt and deserialize is either null or empty 
        /// or <see cref="CContact"/> can't be decrypted and deserialized.
        /// </exception>
        public static CContact FromJsonDecrypt(
            string serverKey,
            string serialized,
            EncodingType decoder = EncodingType.Base64,
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CContact FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CContact ccntct = Newtonsoft.Json.JsonConvert.DeserializeObject<CContact>(serialized);
            CContact decrContact = DecryptSrvMsg(serverKey, ref ccntct, decoder, zipType);
            if (decrContact == null)
                throw new CqrException($"static CContact FromJsonDecrypt(string serverKey, string serialized) failed.");

            return ccntct;
        }

        public static CContact DecryptSrvMsg(
            string serverKey,
            ref CContact ccntct,
            EncodingType decoder = EncodingType.Base64,
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            string pipeString = "", decrypted = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                pipeString = (new SymmCipherPipe(serverKey, keyHash)).PipeString;

                decrypted = SymmCipherPipe.DecrpytToString(ccntct.Message, serverKey, out pipeString, EncodingType.Base64, Zfx.ZipType.None);

                if (!ccntct.Hash.Equals(pipeString))
                {
                    string errMsg = $"ccntct.Hash={ccntct.Hash} doesn't match pipeString={pipeString}";
                    Area23Log.Log(errMsg);
                    // throw new CqrException(errMsg);
                    ;
                }
                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, ccntct.Hash, pipeString, decrypted), "");
                if (!md5Hash.Equals(ccntct.Md5Hash))
                {
                    string md5ErrExcMsg = $"ccntct-Md5Hash={ccntct.Md5Hash} doesn't match md5Hash={md5Hash}";
                    Area23Log.Log(md5ErrExcMsg);
                    ;
                }

                ccntct.Message = decrypted; ;
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return ccntct;
        }

        #endregion static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        public new static CContact CloneCopy(CContact source, CContact destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CContact();

            destination.Hash = source.Hash;
            destination.Message = source.Message;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.ContactId = source.ContactId;
            destination.Cuid = source.Cuid;
            destination.Name = source.Name;
            destination.Email = source.Email;
            destination.Mobile = source.Mobile;
            destination.Address = source.Address;
            destination.SecretKey = source.SecretKey;
            try
            {
                CImage.CloneCopy(source.ContactImage, destination.ContactImage);
            }
            catch (Exception exImg)
            {
                Area23Log.LogOriginMsgEx("CContact", "CloneCopy error on prcocessing image", exImg);
            }
            CImage contactImage = source.ContactImage;
            if (contactImage != null)
            {
                try
                {
                    destination.ContactImage = new CImage(contactImage.ToDrawingBitmap(), contactImage.ImageFileName);
                }
                catch (Exception exImg)
                {
                    Area23Log.LogOriginMsgEx("CContact", "CloneCopy error on prcocessing image", exImg);
                }
            }

            // destination.SerializedMsg = "";
            // destination.SerializedMsg = destination.ToJson();

            return destination;

        }

        #endregion static members 

    }

}
