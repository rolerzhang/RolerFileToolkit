using System.IO;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi.Engine
{
    internal static class RecordEngine
    {
        #region Const String

        public const string Identifier_Flis = "FLIS";
        public const string Identifier_Fcis = "FCIS";

        #endregion

        public static bool TryReadFlisRecord(Stream stream, long offset, out FlisRecord flisRecord)
        {
            bool result = false;
            flisRecord = null;
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.CheckStart(4, Identifier_Flis))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                flisRecord = new FlisRecord();
                if (stream.TryReadString(4, out string identifier))
                {
                    flisRecord.Identifier = identifier;
                }
                if (stream.TryReadUint(out uint value0))
                {
                    flisRecord.Value0 = value0;
                }
                if (stream.TryReadUshort(out ushort value1))
                {
                    flisRecord.Value1 = value1;
                }
                if (stream.TryReadUshort(out ushort value2))
                {
                    flisRecord.Value2 = value2;
                }
                if (stream.TryReadUint(out uint value3))
                {
                    flisRecord.Value3 = value3;
                }
                if (stream.TryReadUint(out uint value4))
                {
                    flisRecord.Value4 = value4;
                }
                if (stream.TryReadUshort(out ushort value5))
                {
                    flisRecord.Value5 = value5;
                }
                if (stream.TryReadUshort(out ushort value6))
                {
                    flisRecord.Value6 = value6;
                }
                if (stream.TryReadUint(out uint value7))
                {
                    flisRecord.Value7 = value7;
                }
                if (stream.TryReadUint(out uint value8))
                {
                    flisRecord.Value8 = value8;
                }
                if (stream.TryReadUint(out uint value9))
                {
                    flisRecord.Value9 = value9;
                }
                result = true;
            }

            if (!result)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }

            return result;
        }

        public static bool TryReadFcisRecord(Stream stream, long offset, out FcisRecord fcisRecord)
        {
            bool result = false;
            fcisRecord = null;
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.CheckStart(4, Identifier_Fcis))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                fcisRecord = new FcisRecord();
                if (stream.TryReadString(4, out string identifier))
                {
                    fcisRecord.Identifier = identifier;
                }
                if (stream.TryReadUint(out uint value0))
                {
                    fcisRecord.Value0 = value0;
                }
                if (stream.TryReadUint(out uint value1))
                {
                    fcisRecord.Value1 = value1;
                }
                if (stream.TryReadUint(out uint value2))
                {
                    fcisRecord.Value2 = value2;
                }
                if (stream.TryReadUint(out uint value3))
                {
                    fcisRecord.Value3 = value3;
                }
                if (stream.TryReadUint(out uint value4))
                {
                    fcisRecord.Value4 = value4;
                }
                if (stream.TryReadUint(out uint value5))
                {
                    fcisRecord.Value5 = value5;
                }
                if (stream.TryReadUint(out uint value6))
                {
                    fcisRecord.Value6 = value6;
                }
                if (stream.TryReadUint(out uint value7))
                {
                    fcisRecord.Value7 = value7;
                }
                if (stream.TryReadUshort(out ushort value8))
                {
                    fcisRecord.Value8 = value8;
                }
                if (stream.TryReadUshort(out ushort value9))
                {
                    fcisRecord.Value9 = value9;
                }
                if (stream.TryReadUint(out uint value10))
                {
                    fcisRecord.Value10 = value10;
                }
                result = true;
            }

            if (!result)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }

            return result;
        }

        public static void Write(IndxHeader file, Stream stream)
        {
        }

    }
}
