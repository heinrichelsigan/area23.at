using System.Collections.Generic;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    public interface IDecodable
    {
        IDecodable Decodable { get; }

        HashSet<char> ValidCharList { get; }

        string EnCode(byte[] inBytes);

        byte[] DeCode(string encodedString);
      

        bool Validate(string encodedString);

        bool IsValidShowError(string encodedString, out string error);

    }

}
