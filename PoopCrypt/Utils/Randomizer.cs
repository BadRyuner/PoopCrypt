using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PoopCrypt.Utils
{
	public static class Randomizer
	{
		static Randomizer()
		{
			SharedBasicRandom = new Random();
			_x = (uint)SharedBasicRandom.Next();
			_y = (uint)SharedBasicRandom.Next();
			_z = (uint)SharedBasicRandom.Next();
			_w = (uint)SharedBasicRandom.Next();
			_bytes = new byte[4096];
			FillBuffer(_bytes);
		}

		public static Random SharedBasicRandom;

		public static byte Next()
		{
			if (from >= 4095) FillBuffer(_bytes);
			return _bytes[from++];
		}

		public static byte Next(byte max)
		{
			again:
			while (from < 4095)
			{
				var sel = _bytes[from++];
				if (sel <= max)
					return sel;
			}
			FillBuffer(_bytes); goto again;
		}

		public static byte Next(byte min, byte max)
		{
			again:
			while (from < 4095)
			{
				var sel = _bytes[from++];
				if (sel <= max && sel >= min)
					return sel;
			}
			FillBuffer(_bytes); goto again;
		}

		//source: https://roman.st/Article/Faster-Marsaglia-Xorshift-pseudo-random-generator-in-unsafe-C
		private static unsafe void FillBuffer(byte[] buf)
		{
			uint x = _x, y = _y, z = _z, w = _w;
			fixed (byte* pbytes = buf)
			{
				uint* pbuf = (uint*)(pbytes + 0);
				uint* pend = (uint*)(pbytes + 4096);
				while (pbuf < pend)
				{
					uint t = x ^ (x << 11);
					x = y; y = z; z = w;
					*(pbuf++) = w = w ^ (w >> 19) ^ (t ^ (t >> 8));
				}
			}
			//_x = (uint)SharedBasicRandom.Next();
			//_y = (uint)SharedBasicRandom.Next();
			//_z = (uint)SharedBasicRandom.Next();
			//_w = (uint)SharedBasicRandom.Next();
			from = 0;
		}

		public static unsafe void FillBytes(byte[] bytes)
		{
			uint x = _x, y = _y, z = _z, w = _w;
			fixed (byte* pbytes = bytes)
			{
				uint* pbuf = (uint*)(pbytes + 0);
				uint* pend = (uint*)(pbytes + (bytes.Length - 1));
				while (pbuf < pend)
				{
					uint t = x ^ (x << 11);
					x = y; y = z; z = w;
					*(pbuf++) = w = w ^ (w >> 19) ^ (t ^ (t >> 8));
				}
			}
			_x = x; _y = y; _z = z; _w = w;
		}


		private static byte[] _bytes;
		private static uint _x;
		private static uint _y;
		private static uint _z;
		private static uint _w;
		private static short from;
	}
}
