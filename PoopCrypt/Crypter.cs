using System;
using System.Linq;
using System.Collections.Generic;

using PoopCrypt.TypeCrypters;

namespace PoopCrypt
{
    public unsafe class Crypter
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

            TypeCrypters.Add(typeof(double), new DoubleCrypter());
            TypeCrypters.Add(typeof(float), new FloatCrypter());

            TypeCrypters.Add(typeof(long), new Int64Crypter());
            TypeCrypters.Add(typeof(ulong), new Int64Crypter());

            TypeCrypters.Add(typeof(int), new Int32Crypter());
            TypeCrypters.Add(typeof(uint), new Int32Crypter());

            TypeCrypters.Add(typeof(short), new Int16Crypter());
            TypeCrypters.Add(typeof(ushort), new Int16Crypter());
            TypeCrypters.Add(typeof(char), new Int16Crypter());

            TypeCrypters.Add(typeof(byte), new Int8Crypter());
            TypeCrypters.Add(typeof(sbyte), new Int8Crypter());

            TypeCrypters.Add(typeof(bool), new BoolCrypter());

            TypeCrypters.Add(typeof(DateTime), new DateTimeCrypter());
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
        
        public IEnumerable<byte> Encrypt<T>(T target, bool ignoreAttribute = false)
        {
            var toEncrypt = (IEnumerable<System.Reflection.FieldInfo>)target.GetType().GetFields();
            if (ignoreAttribute is false) toEncrypt = toEncrypt.Where(f => f.CustomAttributes.Any(attr => attr.AttributeType == typeof(EncryptMeAttribute)));

            List<byte> bytes = new List<byte>(1024);

            foreach (var field in toEncrypt)
            {
                var fType = field.FieldType;
                var value = field.GetValue(target);

                if (value == null)
                {
                    bytes.Add(0);
                    continue;
                }
                else bytes.Add(1);

                if (!TypeCrypters.Keys.Contains(fType) && typeof(System.Collections.IList).IsAssignableFrom(fType))
                {
                    var list = (System.Collections.IList)value;
                    bytes.AddRange(list.Count.ToBytes());
                    foreach (var obj in list)
                        bytes.AddRange(TypeCrypters[obj.GetType()].Encrypt(obj));
                }
                else bytes.AddRange(TypeCrypters[fType].Encrypt(value));
            }

            int parsedBytes = 0;
            var bytesAsArr = bytes.ToArray();
            List<byte> encryptedBytes = new List<byte>(bytes.Count);
            while (parsedBytes != bytes.Count)
            {
                var _take = bytes.Count - parsedBytes;
                var take = rand.Next(1, _take > 150 ? 150 : _take);

                byte[] arr = new byte[take];
                Array.Copy(bytesAsArr, parsedBytes, arr, 0, take);
                var randomCrypter = GetRandomCrypter();
                var newBytes = randomCrypter.crypter.CryptBytes(arr);
                encryptedBytes.Add(randomCrypter.b);
                encryptedBytes.AddRange(((char)(short)newBytes.Count()).ToBytes());
                encryptedBytes.AddRange(newBytes);

                parsedBytes += take;
            }

            encryptedBytes.Insert(0, ignoreAttribute ? (byte)1 : (byte)0);

            return encryptedBytes;
        }

        public T Decrypt<T>(byte[] target) where T : new()
        {
            bool ignoreAttribute = target[0] == 1 ? true : false;

            var toDecrypt = (IEnumerable<System.Reflection.FieldInfo>)typeof(T).GetFields();
            if (ignoreAttribute is false) toDecrypt = toDecrypt.Where(f => f.CustomAttributes.Any(attr => attr.AttributeType == typeof(EncryptMeAttribute)));

            T obj = new T();
            var refObj = __makeref(obj);

            int pos = 1;
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
            {
                if (target[pos] == 0)
                {
                    pos++;
                    continue;
                }
                pos++;
                if (!TypeCrypters.Keys.Contains(field.FieldType) && typeof(System.Collections.IList).IsAssignableFrom(field.FieldType))
                {
                    var decType = TypeCrypters[field.FieldType.GetInterfaces().First(i => i.IsGenericType).GenericTypeArguments[0]];
                    int length = 0; byte* lPtr = (byte*)&length;
                    lPtr[0] = target[pos]; pos++;
                    lPtr[1] = target[pos]; pos++;
                    lPtr[2] = target[pos]; pos++;
                    lPtr[3] = target[pos]; pos++;
                    var arr = (Array)field.FieldType.GetConstructors().First(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(int))
                    .Invoke(new object[] { length });
                    for (int j = 0; j < length; j++)
                        arr.SetValue(decType.Decrypt(target, ref pos), j);
                    field.SetValueDirect(refObj, arr);
                }
                else field.SetValueDirect(refObj, TypeCrypters[field.FieldType].Decrypt(target, ref pos));
            }

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
