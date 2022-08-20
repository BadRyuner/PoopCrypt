using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt.ByteCrypters
{
	public class Bit8 : IByteCrypter
	{
		public IEnumerable<byte> CryptBytes(byte[] bytes)
		{
			var ret = new byte[bytes.Length*8]; //Console.WriteLine("Writed bytes -> " + bytes.Length);
			for (int i = 0; i < ret.Length; i+=8)
			{
				var b = bytes[i/8];
				ret[i] =   (byte)(b & 1);
				ret[i+1] = (byte)(b & (1 << 1));
				ret[i+2] = (byte)(b & (1 << 2));
				ret[i+3] = (byte)(b & (1 << 3));
				ret[i+4] = (byte)(b & (1 << 4));
				ret[i+5] = (byte)(b & (1 << 5));
				ret[i+6] = (byte)(b & (1 << 6));
				ret[i+7] = (byte)(b & (1 << 7));
			}
			return ret;
		}

		public IEnumerable<byte> DecryptBytes(byte[] bytes)
		{
			var ret = new byte[bytes.Length/8]; //Console.WriteLine("Readed bytes -> " + ret.Length);
			for (int i = 0; i < bytes.Length; i += 8)
			{
				int at = i / 8;
				ret[at] |= bytes[i];
				ret[at] |= bytes[i+1];
				ret[at] |= bytes[i+2];
				ret[at] |= bytes[i+3];
				ret[at] |= bytes[i+4];
				ret[at] |= bytes[i+5];
				ret[at] |= bytes[i+6];
				ret[at] |= bytes[i+7];
			}
			return ret;
		}
	}
}
