using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt
{
    public interface IByteCrypter
    {
        IEnumerable<byte> CryptBytes(byte[] bytes);
        IEnumerable<byte> DecryptBytes(byte[] bytes);
    }
}
