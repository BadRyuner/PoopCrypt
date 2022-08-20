using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt.ByteCrypters
{
	public class HalfByte : IByteCrypter
	{
		public IEnumerable<byte> CryptBytes(byte[] bytes)
		{
			var ret = new byte[bytes.Length * 2];
			for (int i = 0; i < ret.Length; i += 2)
			{
				var b = bytes[i / 2];
				ret[i] = (byte)(b & 0b1111_0000);
				ret[i + 1] = (byte)(b & 0b0000_1111);
			}
			return ret;
		}

		public IEnumerable<byte> DecryptBytes(byte[] bytes)
		{
			var ret = new byte[bytes.Length / 2];
			for (int i = 0; i < bytes.Length; i += 2)
			{
				int at = i / 2;
				ret[at] |= bytes[i];
				ret[at] |= bytes[i + 1];
			}
			return ret;
		}
	}
}
