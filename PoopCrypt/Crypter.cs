using System;
using System.Linq;
using System.Collections.Generic;

using PoopCrypt.TypeCrypters;

namespace PoopCrypt
{
    public class Crypter
    {
        public static bool VERBOSE =
#if DEBUG
            true;
#else
            false;
#endif

        public Random rand = new Random();

        public Dictionary<Type, ITypeCrypter> TypeCrypters;
        public Dictionary<byte, IByteCrypter> ByteCrypters;

        public Crypter()
        {
            RegisterDefaultTypeCrypters();
            RegisterDefaultByteCrypters();
        }

        public void RegisterDefaultTypeCrypters()
        {
            TypeCrypters = new Dictionary<Type, ITypeCrypter>();
            TypeCrypters.Add(typeof(string), new StringCrypter());
            TypeCrypters.Add(typeof(string[]), new StringArrCrypter());

            TypeCrypters.Add(typeof(double), new DoubleCrypter());
            TypeCrypters.Add(typeof(float), new FloatCrypter());
            TypeCrypters.Add(typeof(double[]), new DoubleArrCrypter());
            TypeCrypters.Add(typeof(float[]), new FloatArrCrypter());

            TypeCrypters.Add(typeof(long), new Int64Crypter());
            TypeCrypters.Add(typeof(ulong), new Int64Crypter());
            TypeCrypters.Add(typeof(long[]), new Int64ArrCrypter());
            TypeCrypters.Add(typeof(ulong[]), new Int64ArrCrypter());

            TypeCrypters.Add(typeof(int), new Int32Crypter());
            TypeCrypters.Add(typeof(uint), new Int32Crypter());
            TypeCrypters.Add(typeof(int[]), new Int32ArrCrypter());
            TypeCrypters.Add(typeof(uint[]), new Int32ArrCrypter());

            TypeCrypters.Add(typeof(short), new Int16Crypter());
            TypeCrypters.Add(typeof(ushort), new Int16Crypter());
            TypeCrypters.Add(typeof(char), new Int16Crypter());
            TypeCrypters.Add(typeof(short[]), new Int16ArrCrypter());
            TypeCrypters.Add(typeof(ushort[]), new Int16ArrCrypter());
            TypeCrypters.Add(typeof(char[]), new Int16ArrCrypter());

            TypeCrypters.Add(typeof(byte), new Int8Crypter());
            TypeCrypters.Add(typeof(sbyte), new Int8Crypter());
            TypeCrypters.Add(typeof(byte[]), new Int8ArrCrypter());
            TypeCrypters.Add(typeof(sbyte[]), new Int8ArrCrypter());

            TypeCrypters.Add(typeof(bool), new BoolCrypter());
            TypeCrypters.Add(typeof(bool[]), new BoolArrCrypter());
        }
        public void RegisterDefaultByteCrypters()
        {
            ByteCrypters = new Dictionary<byte, IByteCrypter>();
            ByteCrypters.Add(0, new ByteCrypters.Pattern());
            ByteCrypters.Add(1, new ByteCrypters.StairXor());
            ByteCrypters.Add(2, new ByteCrypters.ScatteredBytes());
            ByteCrypters.Add(3, new ByteCrypters.SimpleXor());
        }

        public void GenerateAutoTypeCrypterFor<T>(bool ignoreAttribute = false) where T : new() => TypeCrypters.Add(typeof(T), new Recusrive<T>() { local = this, ignore = ignoreAttribute });
        
        public byte[] Encrypt<T>(T target, bool ignoreAttribute = false)
        {
            var toEncrypt = target.GetType()
                .GetFields()
                .Where(f => ignoreAttribute ? true : f.CustomAttributes.Any(attr => attr.AttributeType == typeof(EncryptMeAttribute)));

            List<byte> bytes = new List<byte>();

            foreach (var field in toEncrypt)
                bytes.AddRange(TypeCrypters[field.FieldType].Encrypt(field.GetValue(target)));

            int parsedBytes = 0;
            var nextBytes = bytes.ToArray();
            List<byte> encryptedBytes = new List<byte>(bytes.Count);
            while (parsedBytes != bytes.Count)
            {
                var _take = bytes.Count - parsedBytes;
                var take = rand.Next(1, _take > 150 ? 150 : _take);
                var targetBytes = nextBytes.Take(take).ToArray();

                var randomCrypter = GetRandomCrypter();
                var newBytes = randomCrypter.crypter.CryptBytes(targetBytes);
                encryptedBytes.Add(randomCrypter.b);
                encryptedBytes.AddRange(((char)(short)newBytes.Length).ToBytes());
                encryptedBytes.AddRange(newBytes);

                nextBytes = nextBytes.Skip(take).ToArray(); parsedBytes += take;
            }

            return encryptedBytes.ToArray();
        }

        public T Decrypt<T>(byte[] target) where T : new()
        {
            var toDecrypt = typeof(T)
                .GetFields()
                .Where(f => f.CustomAttributes.Any(attr => attr.AttributeType == typeof(EncryptMeAttribute)));

            T obj = new T();
            var refObj = __makeref(obj);

            int pos = 0;
            List<byte> parsedBytes = new List<byte>(target.Length);

            while (pos < target.Length)
            {
                var byteCrypterType = target[pos]; pos++;
                var length = new byte[] { target[pos], target[pos+1] }.ShortFromBytes(); pos += 2;
                var bytes = new byte[length];
                for (int i = 0; i < length; i++)
                    bytes[i] = target[pos + i];

                parsedBytes.AddRange(ByteCrypters[byteCrypterType].DecryptBytes(bytes));

                pos += length;
            }

            pos = 0;
            target = parsedBytes.ToArray();

            foreach (var field in toDecrypt)
                field.SetValueDirect(refObj, TypeCrypters[field.FieldType].Decrypt(target, ref pos));

            return obj;
        }

        private (byte b, IByteCrypter crypter) GetRandomCrypter()
        {
            var values = ByteCrypters.Keys.ToArray();
            var selected = rand.Next(0, values.Length - 1);
            return (values[selected], ByteCrypters[values[selected]]);
        }
    }
}
