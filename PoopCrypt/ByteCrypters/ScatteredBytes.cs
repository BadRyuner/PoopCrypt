using System;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.ByteCrypters
{
    internal class ScatteredBytes : IByteCrypter
    {
        Random rand = new Random();

        public byte[] CryptBytes(byte[] bytes)
        {
            if (Crypter.VERBOSE)
                Console.WriteLine("ByteCrypter.ScatteredBytes");

            var toParse = new Dictionary<ushort, byte>(bytes.Length);
            for (ushort i = 0; i < bytes.Length; i++)
                toParse.Add(i, bytes[i]);

            var newBytes = new List<byte>(bytes.Length);
            while(toParse.Count > 0)
            {
                var next = toParse.Keys.ToList().Rand();
                newBytes.AddRange(((char)next).ToBytes());
                newBytes.Add(toParse[next]);
                toParse.Remove(next);
            }

            return newBytes.ToArray();
        }

        public byte[] DecryptBytes(byte[] bytes)
        {
            var parsed = new Dictionary<ushort, byte>();
            for (int i = 0; i < bytes.Length; i += 3)
                parsed.Add((ushort)(new byte[] { bytes[i], bytes[i+1] }.ShortFromBytes()), bytes[i+2]);

            List<byte> newBytes = new List<byte>();

            var sorted = parsed.OrderBy(d => d.Key).ToArray();
            foreach (var i in sorted)
                newBytes.Add(i.Value);

            return newBytes.ToArray();
        }
    }
}
