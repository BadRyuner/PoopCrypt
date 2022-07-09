using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt
{
    public interface ITypeCrypter
    {
        byte[] Encrypt(object target);
        object Decrypt(byte[] data, ref int jump);
    }
}
