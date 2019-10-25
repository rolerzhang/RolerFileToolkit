using System.IO;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi.Engine
{
    internal static class PalmDOCHeaderEngine
    {
        #region Const String


        #endregion

        public static PalmDOCHeader Read(Stream stream, long offset)
        {
            PalmDOCHeader result = new PalmDOCHeader();
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.TryReadUshort(out ushort compression))
            {
                result.Compression = (CompressionType)compression;
            }
            stream.Seek(2, SeekOrigin.Current);
            if (stream.TryReadUint(out uint textLength))
            {
                result.TextLength = textLength;
            }
            if (stream.TryReadUshort(out ushort recordCount))
            {
                result.RecordCount = recordCount;
            }
            if (stream.TryReadUshort(out ushort recordSize))
            {
                result.RecordSize = recordSize;
            }

            if (result.Compression == CompressionType.HUFF_CDIC)
            {
                if (stream.TryReadUshort(out ushort encryption))
                {
                    result.Encryption = (EncryptionType)encryption;
                }
                stream.Seek(2, SeekOrigin.Current);
            }
            else
            {
                if (stream.TryReadUint(out uint currentPosition))
                {
                    result.CurrentPosition = currentPosition;
                }
            }


            return result;
        }

        public static void Write(PalmDOCHeader file, Stream stream)
        {
        }

    }
}
