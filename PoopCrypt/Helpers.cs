using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PoopCrypt
{
    public unsafe static class Helpers
    {
        private static Random rand = new Random();

        public static T Rand<T>(this List<T> l) => l[rand.Next(0, l.Count - 1)];

        public static byte[] ToBytes(this long l)
        {
            var bytes = new AsBytes() { obj = l };
            return new byte[] { bytes.b1, bytes.b2, bytes.b3, bytes.b4, bytes.b5, bytes.b6, bytes.b7, bytes.b8 };
        }

        public static byte[] ToBytes(this int i)
        {
            var bytes = new AsBytes() { obj = i };
            return new byte[] { bytes.b1, bytes.b2, bytes.b3, bytes.b4 };
        }

        public static byte[] ToBytes(this char c)
        {
            var bytes = new AsBytes() { obj = c };
            return new byte[] { bytes.b1, bytes.b2 };
        }

        public static byte[] ToBytes(this bool b)
        {
            var bytes = new AsBytesBool() { obj = b };
            return new byte[] { bytes.b1 };
        }

        public static byte[] ToBytes(this double d)
        {
            var bytes = new AsBytesDouble() { obj = d };
            return new byte[] { bytes.b1, bytes.b2, bytes.b3, bytes.b4, bytes.b5, bytes.b6, bytes.b7, bytes.b8 };
        }

        public static byte[] ToBytes(this float f)
        {
            var bytes = new AsBytesFloat() { obj = f };
            return new byte[] { bytes.b1, bytes.b2, bytes.b3, bytes.b4 };
        }

        public static long LongFromBytes(this byte[] b)
        {
            return new AsBytes() { b1 = b[0], b2 = b[1], b3 = b[2], b4 = b[3], b5 = b[4], b6 = b[5], b7 = b[6], b8 = b[7] }.obj;
        }

        public static int IntFromBytes(this byte[] b)
        {
            return (int)new AsBytes() { b1 = b[0], b2 = b[1], b3 = b[2], b4 = b[3] }.obj;
        }

        public static char CharFromBytes(this byte[] b)
        {
            return (char)new AsBytes() { b1 = b[0], b2 = b[1] }.obj;
        }

        public static short ShortFromBytes(this byte[] b)
        {
            return (short)new AsBytes() { b1 = b[0], b2 = b[1] }.obj;
        }

        public static double DoubleFromBytes(this byte[] b)
        {
            return new AsBytesDouble() { b1 = b[0], b2 = b[1], b3 = b[2], b4 = b[3], b5 = b[4], b6 = b[5], b7 = b[6], b8 = b[7] }.obj;
        }

        public static float FloatFromBytes(this byte[] b)
        {
            return new AsBytesFloat() { b1 = b[0], b2 = b[1], b3 = b[2], b4 = b[3] }.obj;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct AsBytes
        {
            [FieldOffset(0)] public long obj;
            [FieldOffset(0)] public byte b1;
            [FieldOffset(1)] public byte b2;
            [FieldOffset(2)] public byte b3;
            [FieldOffset(3)] public byte b4;
            [FieldOffset(4)] public byte b5;
            [FieldOffset(5)] public byte b6;
            [FieldOffset(6)] public byte b7;
            [FieldOffset(7)] public byte b8;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct AsBytesDouble
        {
            [FieldOffset(0)] public double obj;
            [FieldOffset(0)] public byte b1;
            [FieldOffset(1)] public byte b2;
            [FieldOffset(2)] public byte b3;
            [FieldOffset(3)] public byte b4;
            [FieldOffset(4)] public byte b5;
            [FieldOffset(5)] public byte b6;
            [FieldOffset(6)] public byte b7;
            [FieldOffset(7)] public byte b8;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct AsBytesFloat
        {
            [FieldOffset(0)] public float obj;
            [FieldOffset(0)] public byte b1;
            [FieldOffset(1)] public byte b2;
            [FieldOffset(2)] public byte b3;
            [FieldOffset(3)] public byte b4;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct AsBytesBool
        {
            [FieldOffset(0)] public bool obj;
            [FieldOffset(0)] public byte b1;
        }
    }
}
