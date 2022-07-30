using System;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.ByteCrypters
{
    public class PseudoAdd : IByteCrypter
    {
        public static byte[] adds = new byte[] { 1,3,7,10,23,76,130 };
        public static byte next = 0;

        public IEnumerable<byte> CryptBytes(byte[] bytes)
        {
            List<byte> data = new List<byte>();

            foreach(byte _b in bytes)
            {
                byte b = _b;
                while(b > 0)
                {
                    byte a = adds.Where(m => m <= b).Max();
                    b -= a;
                    data.Add(a);
                }
                data.Add(next);
            }

            return data;
        }

        public IEnumerable<byte> DecryptBytes(byte[] bytes)
        {
            List<byte> data = new List<byte>();

            byte lastbyte = 0;

            foreach (byte b in bytes)
            {
                if (b == next)
                {
                    data.Add(lastbyte);
                    lastbyte = 0;
                }
                lastbyte += b;
            }

            return data;
        }
    }
}
