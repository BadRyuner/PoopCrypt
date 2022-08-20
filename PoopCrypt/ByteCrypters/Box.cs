using System;
using System.Collections.Generic;
using System.Text;

namespace PoopCrypt.ByteCrypters
{
	public class Box : IByteCrypter
	{
		public IEnumerable<byte> CryptBytes(byte[] bytes)
		{
			var row = (int)Math.Sqrt(bytes.Length);
			var rows = bytes.Length / row;
			for (int line = 0; line < rows; line++)
			for (int pos = 0; pos < row; pos++)
				bytes[line * row + pos] ^= (byte)(line * row + pos);
			return bytes;
		}

		public IEnumerable<byte> DecryptBytes(byte[] bytes)
		{
			var row = (int)Math.Sqrt(bytes.Length);
			var rows = bytes.Length / row;
			for (int line = 0; line < rows; line++)
			for (int pos = 0; pos < row; pos++)
				bytes[line * row + pos] ^= (byte)(line * row + pos);
			return bytes;
		}
	}
}
