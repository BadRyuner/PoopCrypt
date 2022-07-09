using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.TypeCrypters
{
    internal class FloatCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data.Skip(jump).Take(4).ToArray().FloatFromBytes(); jump += 4;
            return i;
        }

        public byte[] Encrypt(object target) => ((float)target).ToBytes();
    }
    
    internal class DoubleCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data.Skip(jump).Take(8).ToArray().DoubleFromBytes(); jump += 8;
            return i;
        }

        public byte[] Encrypt(object target) => ((double)target).ToBytes();
    }

    internal class FloatArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            var arr = new float[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(4).ToArray().FloatFromBytes(); jump += 4;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (float[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(i.ToBytes());
            return bytes.ToArray();
        }
    }

    internal class DoubleArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            var arr = new double[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(8).ToArray().DoubleFromBytes(); jump += 8;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (double[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(i.ToBytes());
            return bytes.ToArray();
        }
    }
}
