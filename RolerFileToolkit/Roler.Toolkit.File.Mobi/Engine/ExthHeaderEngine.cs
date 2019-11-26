using System.IO;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi.Engine
{
    internal static class ExthHeaderEngine
    {
        #region Const String

        public const string Identifier = "EXTH";
        public const uint UnavailableIndex = 0xFFFFFFFF;

        #endregion

        public static bool TryRead(Stream stream, long offset, out ExthHeader exthHeader)
        {
            exthHeader = null;
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.TryReadString(4, out string identifier))
            {
                if (string.Equals(Identifier, identifier, System.StringComparison.OrdinalIgnoreCase))
                {
                    exthHeader = Read(stream, offset);
                    return true;
                }
            }

            return false;
        }

        public static ExthHeader Read(Stream stream, long offset)
        {
            ExthHeader result = new ExthHeader();
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.TryReadString(4, out string identifier))
            {
                result.Identifier = identifier;
            }
            if (stream.TryReadUint(out uint length))
            {
                result.Length = length;
            }
            if (stream.TryReadUint(out uint count))
            {
                result.RecordCount = count;
            }

            for (int i = 0; i < count; i++)
            {
                var exthRecord = ReadRecord(stream, stream.Position);
                if (exthRecord != null)
                {
                    result.RecordList.Add(exthRecord);
                }
            }

            stream.Seek(offset + length, SeekOrigin.Begin); //skip to end.

            return result;
        }

        public static void Write(ExthHeader file, Stream stream)
        {
        }

        private static ExthRecord ReadRecord(Stream stream, long offset)
        {
            ExthRecord result = new ExthRecord();
            stream.Seek(offset, SeekOrigin.Begin);

            if (stream.TryReadUint(out uint recordType))
            {
                result.Type = (ExthRecordType)recordType;
            }
            if (stream.TryReadUint(out uint length))
            {
                result.Length = length;
            }

            var dataBytesLength = (int)length - 8;
            if (dataBytesLength > 0)
            {
                if (stream.TryReadString(dataBytesLength, out string data))
                {
                    result.Data = data;
                }
            }

            stream.Seek(offset + length, SeekOrigin.Begin); //skip to end.
            return result;
        }

    }
}
