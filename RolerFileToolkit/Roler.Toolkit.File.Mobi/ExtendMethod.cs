using System;
using System.IO;
using System.Linq;

namespace Roler.Toolkit.File.Mobi
{
    internal static class ExtendMethod
    {
        public static bool TryReadByte(this Stream stream, out byte outResult)
        {
            byte[] buffer = new byte[1];
            if (stream.Read(buffer, 0, 1) == 1)
            {
                outResult = buffer[0];
                return true;
            }
            outResult = 0;
            return false;
        }

        public static bool TryReadBytes(this Stream stream, int length, out byte[] outResult)
        {
            byte[] buffer = new byte[length];
            if (stream.Read(buffer, 0, length) == length)
            {
                outResult = buffer;
                return true;
            }
            outResult = null;
            return false;
        }

        public static bool TryReadUshort(this Stream stream, out ushort outResult)
        {
            byte[] buffer = new byte[2];
            if (stream.Read(buffer, 0, 2) == 2)
            {
                outResult = buffer.ToUInt16();
                return true;
            }
            outResult = 0;
            return false;
        }

        public static bool TryReadUint(this Stream stream, out uint outResult)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) == 4)
            {
                outResult = buffer.ToUInt32();
                return true;
            }
            outResult = 0;
            return false;
        }

        public static bool TryReadString(this Stream stream, int length, out string outResult)
        {
            byte[] buffer = new byte[length];
            if (stream.Read(buffer, 0, length) == length)
            {
                outResult = System.Text.Encoding.UTF8.GetString(buffer);
                return true;
            }
            outResult = null;
            return false;
        }

        public static void Skip(this Stream stream)
        {
            int b;
            do
            {
                b = stream.ReadByte();
            } while (b == 0);
            if (b > 0)
            {
                stream.Seek(stream.Position - 1, SeekOrigin.Begin);
            }
        }

        public static bool CheckStart(this Stream stream, int length, string value)
        {
            if (stream.TryReadString(length, out string start))
            {
                return string.Equals(start, value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public static ushort ToUInt16(this byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                throw new ArgumentException("bytes can not empty.");
            }

            var length = Math.Min(2, bytes.Length);
            ushort result = 0;
            for (int i = 0; i < length; i++)
            {
                result |= (ushort)(bytes[i] << (length - 1 - i) * 8);
            }
            return result;
        }

        public static uint ToUInt32(this byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                throw new ArgumentException("bytes can not empty.");
            }

            var length = Math.Min(4, bytes.Length);
            uint result = 0;
            for (int i = 0; i < length; i++)
            {
                result |= (uint)bytes[i] << ((length - 1 - i) * 8);
            }
            return result;
        }

        public static ulong ToUInt64(this byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                throw new ArgumentException("bytes can not empty.");
            }

            var length = Math.Min(8, bytes.Length);
            ulong result = 0;
            for (int i = 0; i < length; i++)
            {
                result |= (ulong)bytes[i] << ((length - 1 - i) * 8);
            }
            return result;
        }

        public static bool RangeEqual(this byte[] bytes, int start, int length, byte[] second)
        {
            if (second is null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return bytes.Skip(start).Take(length).SequenceEqual(second);
        }
    }
}
