using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace PoopCrypt.ByteCrypters
{
    internal class ScatteredBytes : IByteCrypter
    {
        Random rand = new Random();

        /*public IEnumerable<byte> CryptBytes(byte[] bytes)
        {
            if (Crypter.VERBOSE)
                Console.WriteLine("ByteCrypter.ScatteredBytes");

            var toParse = new Dictionary<ushort, byte>(bytes.Length);
            for (ushort i = 0; i < bytes.Length; i++)
                toParse.Add(i, bytes[i]);

            var newBytes = new List<byte>(bytes.Length);
            while(toParse.Count > 0)
            {
                var next = rand.Next(toParse.Count);
                KeyValuePair<ushort, byte> insert = default;

                foreach(var i in toParse)
                {
                    if (next == 0)
                    {
                        insert = i;
                        break;
                    }
					next--;
				}

				newBytes.AddRange(((char)insert.Key).ToBytes());
				newBytes.Add(insert.Value);
				toParse.Remove(insert.Key);
			}

            return newBytes;
        } */

		public unsafe IEnumerable<byte> CryptBytes(byte[] bytes)
		{
			if (Crypter.VERBOSE)
				Console.WriteLine("ByteCrypter.ScatteredBytes");

            byte[] newBytes = new byte[bytes.Length * 3];

            fixed (byte* bPtr = &bytes[0]) 
            {
                List<ushort> notUsedKeys = new List<ushort>();
                for (ushort i = 0; i < bytes.Length; i++)
                    notUsedKeys.Add(i);

                short pos = 0;
                while (notUsedKeys.Count > 0)
                {
                    var takeAt = rand.Next(notUsedKeys.Count);
                    var key = notUsedKeys[takeAt]; notUsedKeys.RemoveAt(takeAt);

                    newBytes[pos] = ((byte*)&key)[0];
                    newBytes[pos + 1] = ((byte*)&key)[1];
                    newBytes[pos + 2] = bPtr[key];

                    pos += 3;
                }
            }
			return newBytes;
		}

		public IEnumerable<byte> DecryptBytes(byte[] bytes)
        {
            var parsed = new Dictionary<ushort, byte>();
            for (int i = 0; i < bytes.Length; i += 3)
                parsed.Add((ushort)(new byte[] { bytes[i], bytes[i+1] }.ShortFromBytes()), bytes[i+2]);

            var newBytes = new byte[parsed.Count];
            for (ushort i = 0; i < parsed.Count; i++)
	            newBytes[i] = parsed[i];

            return newBytes;
        }
    }
}
