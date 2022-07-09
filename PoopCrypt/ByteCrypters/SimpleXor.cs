using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt.ByteCrypters
{
    public class SimpleXor : IByteCrypter
    {
        public Random rand = new Random();
        public byte[] randByte = new byte[] { 54 };

        public byte[] CryptBytes(byte[] bytes)
        {
            if (Crypter.VERBOSE)
                Console.WriteLine("ByteCrypter.SimpleXor");

            rand.NextBytes(randByte);
            byte xor = randByte[0];

            List<byte> newBytes = new List<byte>(bytes.Length + 1);
            newBytes.Add(xor);

            for (int i = 0; i < bytes.Length; i++)
                newBytes.Add((byte)(bytes[i] ^ xor));

            return newBytes.ToArray();
        }

        public byte[] DecryptBytes(byte[] bytes)
        {
            byte xor = bytes[0];

            List<byte> newBytes = new List<byte>(bytes.Length-1);

            for (int i = 1; i < bytes.Length; i++)
                newBytes.Add((byte)(bytes[i] ^ xor));

            return newBytes.ToArray();
        }
    }
}
