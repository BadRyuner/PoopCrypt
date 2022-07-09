using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.TypeCrypters
{
    public class StringCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump) => DecryptStatic(data, ref jump);

        public static string DecryptStatic(byte[] data, ref int jump)
        {
            StringBuilder sb = new StringBuilder();

            int length = data.Skip(jump).Take(4).ToArray().IntFromBytes(); jump += 4;
            var kek = length.ToBytes();

            if (length == 0)
                return null;

            while (length > 0)
            {
                sb.Append(new byte[] { data[jump], data[jump + 1] }.CharFromBytes());
                jump += 2; length -= 1;
            }

            return sb.ToString();
        }

        public byte[] Encrypt(object target) => EncryptStatic((string)target);

        public static byte[] EncryptStatic(string encryptTarget)
        {
            if (encryptTarget == null)
                return new byte[] { 0, 0, 0, 0 };

            List<byte> buffer = new List<byte>(16);
            buffer.AddRange(encryptTarget.Length.ToBytes());
            foreach (char c in encryptTarget)
                buffer.AddRange(c.ToBytes());

            return buffer.ToArray();
        }
    }

    public class StringArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            string[] arr = new string[length];
            for (int i = 0; i < length; i++)
                arr[i] = StringCrypter.DecryptStatic(data, ref jump);
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (string[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(StringCrypter.EncryptStatic(i));

            return bytes.ToArray();
        }
    }
}
