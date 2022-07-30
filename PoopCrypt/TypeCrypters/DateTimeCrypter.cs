using System;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.TypeCrypters
{
    internal unsafe class DateTimeCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            fixed (byte* ptr = &data[jump])
            {
                jump += 8;
                return new DateTime(*(long*)ptr);
            }
        }

        public byte[] Encrypt(object target) => ((DateTime)target).Ticks.ToBytes();
    }
}
