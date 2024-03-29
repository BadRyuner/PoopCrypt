﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt.ByteCrypters
{
    public class SimpleXor : IByteCrypter
    {
        //public byte[] randByte = new byte[] { 54 };

        public IEnumerable<byte> CryptBytes(byte[] bytes)
        {
            //Utils.Randomizer.SharedBasicRandom.NextBytes(randByte);
            //byte xor = randByte[0];
            byte xor = Utils.Randomizer.Next(); 

            List<byte> newBytes = new List<byte>(bytes.Length + 1);
            newBytes.Add(xor);

            for (int i = 0; i < bytes.Length; i++)
                newBytes.Add((byte)(bytes[i] ^ xor));

            return newBytes;
        }

        public IEnumerable<byte> DecryptBytes(byte[] bytes)
        {
            byte xor = bytes[0];

            List<byte> newBytes = new List<byte>(bytes.Length-1);

            for (int i = 1; i < bytes.Length; i++)
                newBytes.Add((byte)(bytes[i] ^ xor));

            return newBytes;
        }
    }
}
