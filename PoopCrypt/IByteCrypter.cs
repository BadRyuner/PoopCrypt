using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt
{
    public interface IByteCrypter
    {
        byte[] CryptBytes(byte[] bytes);
        byte[] DecryptBytes(byte[] bytes);
    }
}
