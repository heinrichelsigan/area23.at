using Area23.At.Framework.Library.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.SecureChat.Entities
{

    /// <summary>
    /// CqrException is inherited from <see cref="ApplicationException"/>
    /// </summary>
    public class CqrException : ApplicationException
    {
        public static Exception LastException
        {
            get => (Exception)AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION);
            set => AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, value);
        }


        public CqrException(string message) : base(message)
        {
            System.AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
            Area23Log.LogStatic(this);
        }

        public CqrException(string message, Exception innerException) : base(message, innerException)
        {
            System.AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
            Area23Log.LogStatic(this);
        }

    }

}
