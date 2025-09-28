using System;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{
    /// <summary>
    /// this exception is only implemented, because of a 1st 
    /// where 3-fish rides on AesEngine
    /// Everything under the namespace `Area23.At.Framework.Library.Crypt.Cipher` is licensed under the MIT License.
    /// <see href="https://opensource.org/license/mit">opensource.org/license/mit</see>
    /// </summary>
    [Obsolete("this exception is only implemented, because of 1st buggy release where 3-fish rides on AesEngine", false)]
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
