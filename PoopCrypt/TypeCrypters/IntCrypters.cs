using System;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.TypeCrypters
{
    internal class Int64Crypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data.Skip(jump).Take(8).ToArray().LongFromBytes(); jump += 8;
            return i;
        }

        public byte[] Encrypt(object target) => ((long)target).ToBytes();
    }

    internal class Int32Crypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data.Skip(jump).Take(4).ToArray().IntFromBytes(); jump += 4;
            return i;
        }

        public byte[] Encrypt(object target) => ((int)target).ToBytes();
    }

    internal class Int16Crypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data.Skip(jump).Take(2).ToArray().CharFromBytes(); jump += 2;
            return i;
        }

        public byte[] Encrypt(object target) => ((char)target).ToBytes();
    }

    internal class Int8Crypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data[jump]; jump += 1;
            return i;
        }

        public byte[] Encrypt(object target) => new byte[] { (byte)target };
    }

    internal class BoolCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var i = data[jump] == 0 ? false : true; jump += 1;
            return i;
        }

        public byte[] Encrypt(object target) => ((bool)target).ToBytes();
    }

    #region ARRAY

    internal class Int64ArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = (ushort)data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            long[] arr = new long[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(8).ToArray().LongFromBytes(); jump += 8;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (long[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(i.ToBytes());
            return bytes.ToArray();
        }
    }

    internal class Int32ArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = (ushort)data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            long[] arr = new long[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(8).ToArray().LongFromBytes(); jump += 4;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (int[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(i.ToBytes());
            return bytes.ToArray();
        }
    }

    internal class Int16ArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = (ushort)data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            long[] arr = new long[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(8).ToArray().LongFromBytes(); jump += 2;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (char[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(i.ToBytes());
            return bytes.ToArray();
        }
    }

    internal class Int8ArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = (ushort)data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            long[] arr = new long[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(8).ToArray().LongFromBytes(); jump += 1;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (byte[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            bytes.AddRange(arr);
            return bytes.ToArray();
        }
    }

    internal class BoolArrCrypter : ITypeCrypter
    {
        public object Decrypt(byte[] data, ref int jump)
        {
            var length = (ushort)data.Skip(jump).Take(2).ToArray().ShortFromBytes(); jump += 2;
            long[] arr = new long[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = data.Skip(jump).Take(8).ToArray().LongFromBytes(); jump += 1;
            }
            return arr;
        }

        public byte[] Encrypt(object target)
        {
            var arr = (bool[])target;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(((char)arr.Length).ToBytes());
            foreach (var i in arr)
                bytes.AddRange(i.ToBytes());
            return bytes.ToArray();
        }
    }

    #endregion
}
