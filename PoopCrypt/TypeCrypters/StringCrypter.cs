using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.TypeCrypters
{
    public class StringCrypter : ITypeCrypter
    {
        public static StringCrypterMode Mode = StringCrypterMode.Auto;

        public unsafe object Decrypt(byte[] data, ref int jump)
        {
			//StringBuilder sb = new StringBuilder();

			//int length = data.Skip(jump).Take(4).ToArray().IntFromBytes(); jump += 4;
			int length = default;
			fixed (byte* p = &data[jump])
				length = *(int*)p;

			jump += 4;

			if (length == 0)
                return null;

            StringCrypterMode read = (StringCrypterMode)data.Skip(jump).Take(1).First(); jump += 1;
            byte[] bytes = data.Skip(jump).Take(length).ToArray(); jump += length;

            switch(read)
            {
                case StringCrypterMode.ASCII:
                    return Encoding.ASCII.GetString(bytes);
                case StringCrypterMode.Utf8:
                    return Encoding.UTF8.GetString(bytes);
                case StringCrypterMode.Unicode:
					return Encoding.Unicode.GetString(bytes);
				case StringCrypterMode.Utf32:
					return Encoding.UTF32.GetString(bytes);
			}
            return null;
			//return sb.ToString();
		}

        public unsafe byte[] Encrypt(object target) // 443
        {
            var encryptTarget = (string)target;
            if (encryptTarget == null)
                return new byte[] { 0, 0, 0, 0 };

            List<byte> buffer = new List<byte>(encryptTarget.Length + 8);
            // buffer.AddRange(encryptTarget.Length.ToBytes());
            if (Mode != StringCrypterMode.Auto)
                buffer.Add((byte)Mode);

            if (Mode == StringCrypterMode.Auto)
            {
                bool useASCII = true;
                fixed (char* ptr = encryptTarget)
                    for (int i = 0; i < encryptTarget.Length; i++)
                        if (ptr[i] > 255)
                        {
                            useASCII = false;
                            break;
                        }
                if (useASCII)
                {
					buffer.Add((byte)StringCrypterMode.ASCII);
					buffer.AddRange(Encoding.ASCII.GetBytes(encryptTarget));
				}
                else
                {
					buffer.Add((byte)StringCrypterMode.Utf8);
					buffer.AddRange(Encoding.UTF8.GetBytes(encryptTarget));
				}
			}
            else if (Mode == StringCrypterMode.ASCII)
                buffer.AddRange(Encoding.ASCII.GetBytes(encryptTarget));
            else if (Mode == StringCrypterMode.Utf8)
				buffer.AddRange(Encoding.UTF8.GetBytes(encryptTarget));
            else if (Mode == StringCrypterMode.Unicode)
				buffer.AddRange(Encoding.Unicode.GetBytes(encryptTarget));
			else if (Mode == StringCrypterMode.Utf32)
				buffer.AddRange(Encoding.UTF32.GetBytes(encryptTarget));

			buffer.InsertRange(0, (buffer.Count - 1).ToBytes());

			return buffer.ToArray();
        }

        public enum StringCrypterMode : byte
        {
            Auto, ASCII, Utf8, Unicode, Utf32
        }
    }
}
