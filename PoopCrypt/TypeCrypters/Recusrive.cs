using System.Linq;

namespace PoopCrypt.TypeCrypters
{
    public class Recusrive<T> : ITypeCrypter where T : new()
    {
        internal Crypter local;
        internal bool ignore;

        public object Decrypt(byte[] data, ref int jump)
        {
            int length = data.Skip(jump).Take(4).ToArray().IntFromBytes(); jump += 4;
            var klass = data.Skip(jump).Take(length).ToArray(); jump += length;
            return local.Decrypt<T>(klass);
        }

        public byte[] Encrypt(object target)
        {
            var bytes = local.Encrypt(target, ignore).ToList();
            bytes.InsertRange(0, bytes.Count.ToBytes());
            return bytes.ToArray();
        }
    }
}
