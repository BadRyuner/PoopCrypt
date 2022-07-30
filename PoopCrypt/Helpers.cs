using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace PoopCrypt
{
    public unsafe static class Helpers
    {
        private static Random rand = new Random();

        public static T Rand<T>(this List<T> l) => l[rand.Next(0, l.Count - 1)];

        public static byte[] ToBytes(this long l)
        {
            byte* ptr = (byte*)&l;
            return new byte[] { ptr[0], ptr[1], ptr[2], ptr[3], ptr[4], ptr[5], ptr[6], ptr[7] };
        }

        public static byte[] ToBytes(this int i)
        {
            byte* ptr = (byte*)&i;
            return new byte[] { ptr[0], ptr[1], ptr[2], ptr[3] };
        }

        public static byte[] ToBytes(this char c)
        {
            byte* ptr = (byte*)&c;
            return new byte[] { ptr[0], ptr[1] };
        }

        public static byte[] ToBytes(this bool b)
        {
            byte* ptr = (byte*)&b;
            return new byte[] { ptr[0] };
        }

        public static byte[] ToBytes(this double d)
        {
            byte* ptr = (byte*)&d;
            return new byte[] { ptr[0], ptr[1], ptr[2], ptr[3], ptr[4], ptr[5], ptr[6], ptr[7] };
        }

        public static byte[] ToBytes(this float f)
        {
            byte* ptr = (byte*)&f;
            return new byte[] { ptr[0], ptr[1], ptr[2], ptr[3] };
        }

        public static long LongFromBytes(this byte[] b)
        {
            fixed (byte* p = &b[0]) return *(long*)p;
        }

        public static int IntFromBytes(this byte[] b)
        {
            fixed (byte* p = &b[0]) return *(int*)p;
        }

        public static char CharFromBytes(this byte[] b)
        {
            fixed (byte* p = &b[0]) return *(char*)p;
        }

        public static short ShortFromBytes(this byte[] b)
        {
            fixed (byte* p = &b[0]) return *(short*)p;
        }

        public static double DoubleFromBytes(this byte[] b)
        {
            fixed (byte* p = &b[0]) return *(double*)p;
        }

        public static float FloatFromBytes(this byte[] b)
        {
            fixed (byte* p = &b[0]) return *(float*)p;
        }
    }
}
