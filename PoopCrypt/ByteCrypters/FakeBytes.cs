using System;
using System.Collections.Generic;
using System.Linq;

namespace PoopCrypt.ByteCrypters
{
	public class FakeBytes : IByteCrypter
	{
		public IEnumerable<byte> CryptBytes(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
				bytes[i] = (byte)~bytes[i];

			List<byte> ret = new List<byte>();
			List<byte> keys = new List<byte>();
			List<byte> allowedBytes = new List<byte>();
			for (byte i = 0; i <= 254; i++)
			{
				bool contains = false;
				for (int x = 0; x < bytes.Length; x++)
					if (bytes[x] == i)
					{
						contains = true;
						break;
					}
				if (!contains)
				{
					keys.Add(i);
					allowedBytes.Add(i);
				}
			}

			if (keys.Count == 0)
				return null;

			int keysCount = Utils.Randomizer.SharedBasicRandom.Next(1, keys.Count > 8 ? 8 : keys.Count);
			var newlist = new List<byte>();
			//while (keys.Count > keysCount) keys.RemoveAt(Utils.Randomizer.SharedBasicRandom.Next(keys.Count));
			while (newlist.Count < keysCount) newlist.Add(keys[Utils.Randomizer.SharedBasicRandom.Next(keys.Count-1)]);
			keys = newlist;

			if (keys.Count == 0)
				return null;

			foreach (byte b in keys)
				allowedBytes.Remove(b);

			ret.Add((byte)keysCount);
			ret.AddRange(keys);
			byte pos = 0;
			byte toAddRandBytes = (byte)Utils.Randomizer.SharedBasicRandom.Next(1, bytes.Length > 50 ? 2 : 12);
			while (pos != bytes.Length)
			{
				for (; toAddRandBytes > 0; toAddRandBytes--)
					ret.Add(allowedBytes.Rand());

				toAddRandBytes = (byte)Utils.Randomizer.SharedBasicRandom.Next(1, bytes.Length > 50 ? 2 : 12);
				byte key = keys.Rand();
				ret.Add(key);
				ret.Add(bytes[pos]);
				pos++;
			}
			return ret;
		}

		public IEnumerable<byte> DecryptBytes(byte[] bytes)
		{
			byte keysCount = bytes[0];
			byte[] keys = bytes.Skip(1).Take(keysCount).ToArray();
			List<byte> ret = new List<byte>();

			bool add = false;
			for (int i = keysCount + 1; i < bytes.Length; i++)
			{
				byte next = bytes[i];
				bool contains = false;
				for (int x = 0; x < keys.Length; x++)
					if (keys[x] == next)
					{
						contains = true;
						break;
					}
				if (contains)
				{
					add = true;
				}
				else if (add)
				{
					add = false;
					ret.Add((byte)~next);
				}
			}
			return ret;
		}
	}
}
