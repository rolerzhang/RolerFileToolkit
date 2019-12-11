using System.IO;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi.Engine
{
    internal static class IndxHeaderEngine
    {
        #region Const String

        public const string Identifier = "INDX";

        #endregion

        public static bool TryRead(Stream stream, long offset, out IndxHeader indxHeader)
        {
            bool result = false;
            indxHeader = null;
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.CheckStart(4, Identifier))
            {
                indxHeader = Read(stream, offset);
                result = true;
            }

            if (!result)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }

            return result;
        }

        public static IndxHeader Read(Stream stream, long offset)
        {
            IndxHeader result = new IndxHeader();
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.TryReadString(4, out string identifier))
            {
                result.Identifier = identifier;
            }
            if (stream.TryReadUint(out uint length))
            {
                result.Length = length;
            }
            if (stream.TryReadUint(out uint indexType))
            {
                result.IndexType = (IndexType)indexType;
            }

            stream.Seek(8, SeekOrigin.Current); //skip 8 unknown bytes.

            if (stream.TryReadUint(out uint idxtStart))
            {
                result.IdxtStart = idxtStart;
            }
            if (stream.TryReadUint(out uint indexCount))
            {
                result.IndexCount = indexCount;
            }
            if (stream.TryReadUint(out uint indexEncoding))
            {
                result.IndexEncoding = (TextEncoding)indexEncoding;
            }
            if (stream.TryReadString(4, out string indexLanguage))
            {
                result.IndexLanguage = indexLanguage;
            }
            if (stream.TryReadUint(out uint totalIndexCount))
            {
                result.TotalIndexCount = totalIndexCount;
            }
            if (stream.TryReadUint(out uint ordtStart))
            {
                result.OrdtStart = ordtStart;
            }
            if (stream.TryReadUint(out uint ligtStart))
            {
                result.LigtStart = ligtStart;
            }

            stream.Seek(8, SeekOrigin.Current); //skip 8 unknown bytes.
            stream.Seek(offset + length, SeekOrigin.Begin); //skip to end.

            return result;
        }

        public static void Write(IndxHeader file, Stream stream)
        {
        }

    }
}
