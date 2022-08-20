using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoopCrypt.ByteCrypters
{
	public class ArrayPlusArray : IByteCrypter
	{
		public IEnumerable<byte> CryptBytes(byte[] bytes)
		{
			byte[] toSum = new byte[bytes.Length];
			Utils.Randomizer.SharedBasicRandom.NextBytes(toSum);
			List<byte> ret = new List<byte>();
			ret.AddRange(toSum.Length.ToBytes());
			ret.AddRange(toSum);
			for (int i = 0; i < bytes.Length; i++)
				bytes[i] += toSum[i];
			ret.AddRange(bytes);
			return ret;
		}

		public IEnumerable<byte> DecryptBytes(byte[] bytes)
		{
			int length = bytes.Take(4).ToArray().IntFromBytes();
			byte[] toRem = bytes.Skip(4).Take(length).ToArray();
			byte[] toChange = bytes.Skip(4).Skip(length).Take(length).ToArray();
			for (int i = 0; i < length; i++)
				toChange[i] -= toRem[i];
			return toChange;
		}
	}
}
