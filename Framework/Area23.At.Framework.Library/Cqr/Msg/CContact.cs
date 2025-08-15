using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

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

        public string SecretKey { get; set; }

        public CImage ContactImage { get; set; }

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

        public CContact(string cs, CType msgArt = CType.Json)
        {
            FromJson<CContact>(cs);
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
            CCopy(this, ccntct);
            this.Hash = hash;
            SerializedMsg = "";
            SerializedMsg = this.ToJson();
        }

        public CContact(CContact ccntct, string ChatRoomNr, string hash) : this(ccntct, hash)
        {
            CCopy(this, ccntct);
            Hash = hash;
            ContactImage = null;
            Cuid = (ccntct.Cuid == Guid.Empty) ? Guid.NewGuid() : ccntct.Cuid;
            Message = ChatRoomNr;
            SerializedMsg = "";
            SerializedMsg = this.ToJson();
        }

        public CContact(CContact ccntct)
        {
            CCopy(this, ccntct);
            Cuid = (ccntct.Cuid == Guid.Empty) ? Guid.NewGuid() : ccntct.Cuid;
            SerializedMsg = "";
            SerializedMsg = this.ToJson();
        }

        public CContact(CContact ccntct, string chatRoomNr, string hash, CImage cqrImage) : this(ccntct, chatRoomNr, hash)
        {
            CCopy(this, ccntct);
            Cuid = (ccntct.Cuid == Guid.Empty) ? Guid.NewGuid() : ccntct.Cuid;
            Hash = hash;
            ContactImage = cqrImage;
            Message = chatRoomNr;
            SerializedMsg = "";
            SerializedMsg = this.ToJson();
        }

        #endregion constructors

    

        public new CContact CCopy(CContact leftDest, CContact rightSrc)
        {
            return CloneCopy(rightSrc, leftDest);
        }


        #region EnDeCrypt+DeSerialize


        public override string EncryptToJson(string serverKey)
        {
            if (Encrypt(serverKey))
            {
                this.SerializedMsg = JsonConvert.SerializeObject(this);
                return this.SerializedMsg;
            }
            throw new CqrException($"EncryptToJson(string severKey) failed for CContact.");
        }

        public override bool Encrypt(string serverKey)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);
                Hash = symmPipe.PipeString;
                Md5Hash = MD5Sum.HashString(String.Concat(serverKey, Hash, symmPipe.PipeString, Message), "");

                byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(Message);
                byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, serverKey, hash) : msgBytes;

                CBytes = cqrbytes;
                Message = Base64.ToBase64(CBytes);
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }


        public new CContact DecryptFromJson(string serverKey, string serialized = "")
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            var ctc = this.FromJson<CContact>(serialized);
            CContact contact = JsonConvert.DeserializeObject<CContact>(serialized);

            if (contact != null && contact.Decrypt(serverKey))
            {
                contact.SerializedMsg = "";
                contact.SerializedMsg = contact.ToJson();
                CCopy(this, contact);
                return contact;
            }
            throw new CqrException($"DecryptFromJson<T>(string severKey, string serialized) failed for CContact");
        }

        public override bool Decrypt(string serverKey)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);

                byte[] cipherBytes = CBytes;
                byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, serverKey, hash) : cipherBytes;
                string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
                while (decrypted[decrypted.Length - 1] == '\0')
                    decrypted = decrypted.Substring(0, decrypted.Length - 1);


                if (!Hash.Equals(symmPipe.PipeString))
                    throw new CqrException($"Hash: {Hash} doesn't match symmPipe.PipeString: {symmPipe.PipeString}");
                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, Hash, symmPipe.PipeString, decrypted), "");
                if (!md5Hash.Equals(Md5Hash))
                    throw new CqrException($"md5Hash: {md5Hash} doesn't match property Md5Hash: {Md5Hash}");

                Message = decrypted;
                CBytes = new byte[0];
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }


        #endregion EnDeCrypt+DeSerialize

        #region members

        public override string ToJson()
        {
            this.SerializedMsg = "";
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this.SerializedMsg = jsonString;
            return jsonString;
        }

        public new T FromJson<T>(string jsonText)
        {
            T tt = JsonConvert.DeserializeObject<T>(jsonText);
            if (tt != null && tt is CContact contactJson)
            {
                if (contactJson != null && contactJson.ContactId > -1 && !string.IsNullOrEmpty(contactJson.Name))
                {
                    CCopy(this, contactJson);
                }
            }

            return tt;
        }


        public override string ToXml()
        {
            SerializedMsg = "";
            string xmlString = Utils.SerializeToXml<CContact>(this);
            SerializedMsg = xmlString;
            return xmlString;
        }

        public new T FromXml<T>(string xmlText)
        {
            T cqrT = base.FromXml<T>(xmlText);
            if (cqrT is CContact cCnt)
            {
                CCopy(this, cCnt);
            }

            return (T)cqrT;

        }


        public override string ToString()
        {
            return
                "\"ContactId\": \t\"" + ContactId + "\";" + Environment.NewLine +
                "\"Cuid\": \t\"" + Cuid + "\";" + Environment.NewLine +
                "\"Name\": \t\"" + Name + "\";" + Environment.NewLine +
                "\"Email\": \t\"" + Email ?? "" + "\";" + Environment.NewLine +
                "\"Mobile\": \t\"" + Mobile ?? "" + "\";" + Environment.NewLine +
                "\"Address\": \t\"" + Address ?? "" + "\";" + Environment.NewLine +
                "\"NameEmail\": \t\"" + NameEmail ?? "" + "\";" + Environment.NewLine +
                ((ContactImage != null) ?
                    "\"ContactImage\": \t" + Environment.NewLine +
                    "\"ContactImage.ImageFileName\": \t\"" + ContactImage.ImageFileName + "\";" + Environment.NewLine +
                    "\"ContactImage.ImageMimeType\": \t\"" + ContactImage.ImageMimeType + "\";" + Environment.NewLine +
                    "\"ContactImage.ImageBase64\": \t\"" + ContactImage.ImageBase64 + "\";" + Environment.NewLine
                    : "")
                ;
        }

        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="DataGridView"/>
        /// </summary>
        public object[] GetRowParams()
        {
            List<object> oList = new List<object>();
            oList.Add(ContactId);
            oList.Add(Name);
            oList.Add(Email);
            oList.Add(Mobile);
            oList.Add(Address);
            return oList.ToArray();
        }

        #endregion members


        #region static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        /// <summary>
        /// ToJsonEncrypt
        /// </summary>
        /// <param name="serverKey">server key to encrypt</param>
        /// <param name="ccntct"><see cref="CContact"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CContact"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string ToJsonEncrypt(string serverKey, CContact ccntct)
        {
            if (string.IsNullOrEmpty(serverKey) || ccntct == null)
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, CContact ccntct) failed: NULL reference!");

            if (!EncryptSrvMsg(serverKey, ref ccntct))
                throw new CqrException($"static string ToJsonEncrypt(string severKey, CContact ccntct) failed.");

            string serializedJson = ccntct.ToJson();
            return serializedJson;
        }

        public static bool EncryptSrvMsg(string serverKey, ref CContact ccntct)
        {
            string pipeString = "", encrypted = "";
            try
            {
                pipeString = (new SymmCipherPipe(serverKey)).PipeString;
                ccntct.Hash = pipeString;
                ccntct.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, EnDeCodeHelper.KeyToHex(serverKey), ccntct.Hash, ccntct.Message), "");

                encrypted = SymmCipherPipe.EncrpytToString(ccntct.Message, serverKey, out pipeString, EncodingType.Base64, Zfx.ZipType.None);
               
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
        public static CContact FromJsonDecrypt(string serverKey, string serialized)
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CContact FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CContact ccntct = new CContact(serialized, CType.Json);
            ccntct = DecryptSrvMsg(serverKey, ccntct);
            if (ccntct == null)
                throw new CqrException($"static CContact FromJsonDecrypt(string serverKey, string serialized) failed.");

            return ccntct;
        }

        public static CContact DecryptSrvMsg(string serverKey, CContact ccntct)
        {
            string pipeString = "", decrypted = "";
            try
            {
                pipeString = new SymmCipherPipe(serverKey).PipeString;

                decrypted = SymmCipherPipe.DecrpytToString(ccntct.Message, serverKey, out pipeString, EncodingType.Base64, Zfx.ZipType.None);
                
                if (!ccntct.Hash.Equals(pipeString))
                {
                    string errMsg = $"Hash: {ccntct.Hash} doesn't match symmPipe.PipeString: {pipeString}";
                    // throw new CqrException(errMsg);
                    ;
                }
                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, EnDeCodeHelper.KeyToHex(serverKey), pipeString, decrypted), "");
                if (!md5Hash.Equals(ccntct.Md5Hash))
                {
                    string md5ErrExcMsg = $"md5Hash: {md5Hash} doesn't match property Md5Hash: {ccntct.Md5Hash}";
                    // throw new CqrException(md5ErrExcMsg);
                    ;
                }

                ccntct.Message = decrypted;
                ccntct.CBytes = new byte[0];
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return ccntct;
        }


        public new static CContact CloneCopy(CContact source, CContact destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CContact(source);

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
                destination.ContactImage = source.ContactImage;
            }
            catch (Exception exImg)
            {
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
                }
            }

            destination.SerializedMsg = "";
            destination.SerializedMsg = destination.ToJson();

            return destination;

        }

        #endregion static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

    }

}
