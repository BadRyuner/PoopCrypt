using System;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.ByteCrypters
{
    public class Pattern : IByteCrypter
    {
        public static Dictionary<byte, Func<bool, byte[], byte[]>> Patterns = new Dictionary<byte, Func<bool, byte[], byte[]>>();

        public Random rand = new Random();

        static Pattern()
        {
            Patterns.Add(0, BasicPatterns.ArrayPlusArray);
            Patterns.Add(1, BasicPatterns.FakeBytes);
        }

        public IEnumerable<byte> CryptBytes(byte[] bytes)
        {
            if (Crypter.VERBOSE)
                Console.Write("ByteCrypter.Pattern -> ");

            byte patternType = Patterns.Keys.ToArray()[rand.Next(0, Patterns.Count)];
            byte[] newBytes = Patterns[patternType](false, bytes);
            if (newBytes == null)
                return CryptBytes(bytes);
            List<byte> ret = new List<byte>(newBytes.Length + 1);
            ret.Add(patternType);
            ret.AddRange(newBytes);
            return ret;
        }

        public IEnumerable<byte> DecryptBytes(byte[] bytes) => Patterns[bytes[0]](true, bytes.Skip(1).ToArray());
    }

    public static class BasicPatterns
    {
        public static Random rand = new Random();

        public static byte[] ArrayPlusArray(bool decrypt, byte[] bytes)
        {
            if (decrypt)
            {
                int length = bytes.Take(4).ToArray().IntFromBytes();
                byte[] toRem = bytes.Skip(4).Take(length).ToArray();
                byte[] toChange = bytes.Skip(4).Skip(length).Take(length).ToArray();
                for (int i = 0; i < length; i++)
                    toChange[i] -= toRem[i];
                return toChange;
            }
            else
            {
                if (Crypter.VERBOSE)
                    Console.WriteLine("ArrayPlusArray");

                byte[] toSum = new byte[bytes.Length];
                rand.NextBytes(toSum);
                List<byte> ret = new List<byte>();
                ret.AddRange(toSum.Length.ToBytes());
                ret.AddRange(toSum);
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] += toSum[i];
                ret.AddRange(bytes);
                return ret.ToArray();
            }
        }

        public static byte[] FakeBytes(bool decrypt, byte[] bytes)
        {
            if (decrypt)
            {
                byte keysCount = bytes[0];
                byte[] keys = bytes.Skip(1).Take(keysCount).ToArray();
                List<byte> ret = new List<byte>();

                bool add = false;
                byte prevKey = 0;
                for (int i = keysCount + 1; i < bytes.Length; i++)
                {
                    byte next = bytes[i];
                    if (keys.Contains(next))
                    {
                        add = true;
                        prevKey = next;
                    }
                    else if (add)
                    {
                        add = false;
                        ret.Add((byte)~next);
                    }
                }
                return ret.ToArray();
            }
            else
            {
                if (Crypter.VERBOSE)
                    Console.WriteLine($"FakeBytes");

                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = (byte)~bytes[i];

                List<byte> ret = new List<byte>();
                List<byte> keys = new List<byte>();
                List<byte> allowedBytes = new List<byte>();
                for (byte i = 0; i <= 254; i++)
                    if (!bytes.Contains(i))
                    {
                        keys.Add(i);
                        allowedBytes.Add(i);
                    }

                if (keys.Count == 0)
                    return null;

                int keysCount = rand.Next(1, keys.Count > 8 ? 8 : keys.Count);

                while (keys.Count > keysCount)
                    keys.Remove(keys.Rand());

                foreach (byte b in keys)
                    allowedBytes.Remove(b);

                ret.Add((byte)keysCount);
                ret.AddRange(keys);
                byte pos = 0;
                byte toAddRandBytes = (byte)rand.Next(1, bytes.Length > 50 ? 2 : 12);
                while (pos != bytes.Length)
                {
                    for(; toAddRandBytes > 0; toAddRandBytes--)
                        ret.Add(allowedBytes.Rand());

                    toAddRandBytes = (byte)rand.Next(1, bytes.Length > 50 ? 2 : 12);
                    byte key = keys.Rand();
                    ret.Add(key);
                    ret.Add(bytes[pos]);
                    pos++;
                }
                return ret.ToArray();
            }
        }
    }
}
