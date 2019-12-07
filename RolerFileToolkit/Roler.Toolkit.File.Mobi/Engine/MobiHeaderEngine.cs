using System.IO;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi.Engine
{
    internal static class MobiHeaderEngine
    {
        #region Const String

        public const string Identifier = "MOBI";
        public const uint UnavailableIndex = 0xFFFFFFFF;

        #endregion

        public static bool TryRead(Stream stream, long offset, out MobiHeader mobiHeader)
        {
            bool result = false;
            mobiHeader = null;
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.TryReadString(4, out string identifier))
            {
                if (string.Equals(Identifier, identifier, System.StringComparison.OrdinalIgnoreCase))
                {
                    mobiHeader = Read(stream, offset);
                    result = true;
                }
            }

            if (!result)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }

            return result;
        }

        public static MobiHeader Read(Stream stream, long offset)
        {
            MobiHeader result = new MobiHeader();
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.TryReadString(4, out string identifier))
            {
                result.Identifier = identifier;
            }
            if (stream.TryReadUint(out uint length))
            {
                result.Length = length;
            }
            if (stream.TryReadUint(out uint mobiType))
            {
                result.MobiType = (MobiType)mobiType;
            }
            if (stream.TryReadUint(out uint textEncoding))
            {
                result.TextEncoding = (TextEncoding)textEncoding;
            }
            if (stream.TryReadUint(out uint uniqueId))
            {
                result.UniqueId = uniqueId;
            }
            if (stream.TryReadUint(out uint fileVersion))
            {
                result.FileVersion = fileVersion;
            }
            if (stream.TryReadUint(out uint ortographicIndex))
            {
                result.OrtographicIndex = ortographicIndex;
            }
            if (stream.TryReadUint(out uint inflectionIndex))
            {
                result.InflectionIndex = inflectionIndex;
            }
            if (stream.TryReadUint(out uint indexNames))
            {
                result.IndexNames = indexNames;
            }
            if (stream.TryReadUint(out uint indexKeys))
            {
                result.IndexKeys = indexKeys;
            }
            if (stream.TryReadUint(out uint extraIndex0))
            {
                result.ExtraIndex0 = extraIndex0;
            }
            if (stream.TryReadUint(out uint extraIndex1))
            {
                result.ExtraIndex1 = extraIndex1;
            }
            if (stream.TryReadUint(out uint extraIndex2))
            {
                result.ExtraIndex2 = extraIndex2;
            }
            if (stream.TryReadUint(out uint extraIndex3))
            {
                result.ExtraIndex3 = extraIndex3;
            }
            if (stream.TryReadUint(out uint extraIndex4))
            {
                result.ExtraIndex4 = extraIndex4;
            }
            if (stream.TryReadUint(out uint extraIndex5))
            {
                result.ExtraIndex5 = extraIndex5;
            }
            if (stream.TryReadUint(out uint firstNonBookIndex))
            {
                result.FirstNonBookIndex = firstNonBookIndex;
            }
            if (stream.TryReadUint(out uint fullNameOffset))
            {
                result.FullNameOffset = fullNameOffset;
            }
            if (stream.TryReadUint(out uint fullNameLength))
            {
                result.FullNameLength = fullNameLength;
            }
            if (stream.TryReadUint(out uint locale))
            {
                result.Locale = locale;
            }
            if (stream.TryReadString(4, out string inputLanguage))
            {
                result.InputLanguage = inputLanguage;
            }
            if (stream.TryReadString(4, out string outputLanguage))
            {
                result.OutputLanguage = outputLanguage;
            }
            if (stream.TryReadUint(out uint minVersion))
            {
                result.MinVersion = minVersion;
            }
            if (stream.TryReadUint(out uint firstImageIndex))
            {
                result.FirstImageIndex = firstImageIndex;
            }
            if (stream.TryReadUint(out uint huffmanRecordOffset))
            {
                result.HuffmanRecordOffset = huffmanRecordOffset;
            }
            if (stream.TryReadUint(out uint huffmanRecordCount))
            {
                result.HuffmanRecordCount = huffmanRecordCount;
            }
            if (stream.TryReadUint(out uint huffmanTableOffset))
            {
                result.HuffmanTableOffset = huffmanTableOffset;
            }
            if (stream.TryReadUint(out uint huffmanTableLength))
            {
                result.HuffmanTableLength = huffmanTableLength;
            }
            if (stream.TryReadUint(out uint exthFlags))
            {
                result.EXTHFlags = exthFlags;
            }

            stream.Seek(32 + 4, SeekOrigin.Current); //skip 32 unknown bytes and 0xFFFFFFFF.

            if (stream.TryReadUint(out uint drmOffset))
            {
                result.DRMOffset = drmOffset;
            }
            if (stream.TryReadUint(out uint drmCount))
            {
                result.DRMCount = drmCount;
            }
            if (stream.TryReadUint(out uint drmSize))
            {
                result.DRMSize = drmSize;
            }
            if (stream.TryReadUint(out uint drmFlags))
            {
                result.DRMFlags = drmFlags;
            }

            stream.Seek(8, SeekOrigin.Current); //skip 8 bytes, Bytes to the end of the MOBI header, including the following if the header length >= 228 (244 from start of record). Use 0x0000000000000000.

            if (stream.TryReadUshort(out ushort firstContentRecordNumber))
            {
                result.FirstContentRecordNumber = firstContentRecordNumber;
            }
            if (stream.TryReadUshort(out ushort lastContentRecordNumber))
            {
                result.LastContentRecordNumber = lastContentRecordNumber;
            }

            stream.Seek(4, SeekOrigin.Current); //skip 4 bytes, 0x00000001. 

            if (stream.TryReadUint(out uint fcisRecordNumber))
            {
                result.FCISRecordNumber = fcisRecordNumber;
            }

            stream.Seek(4, SeekOrigin.Current); //skip 4 bytes, 0x00000001.

            if (stream.TryReadUint(out uint flisRecordNumber))
            {
                result.FLISRecordNumber = flisRecordNumber;
            }

            stream.Seek(28, SeekOrigin.Current); //skip 0x00000001, 0x0000000000000000, 0xFFFFFFFF, 0x00000000, 0xFFFFFFFF, 0xFFFFFFFF,

            if (stream.TryReadUint(out uint extraRecordDataFlags))
            {
                result.ExtraRecordDataFlags = extraRecordDataFlags;
            }
            if (stream.TryReadUint(out uint iNDXRecordOffset))
            {
                result.INDXRecordOffset = iNDXRecordOffset;
            }

            stream.Seek(offset + length, SeekOrigin.Begin); //skip to end.

            return result;
        }

        public static void Write(MobiHeader file, Stream stream)
        {
        }

    }
}
