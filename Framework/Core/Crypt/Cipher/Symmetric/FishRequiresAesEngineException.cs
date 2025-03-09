using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// this exception is only implemented, because of a 1st 
    /// where 3-fish rides on AesEngine
    /// </summary>
    public class FishRequiresAesEngineException : Exception
    {
        public FishRequiresAesEngineException() : base() { }


        public FishRequiresAesEngineException(string message) : base(message)
        {

        }

        public FishRequiresAesEngineException(string message, Exception innerException) : base(message, innerException)
        {

        }


    }

}
