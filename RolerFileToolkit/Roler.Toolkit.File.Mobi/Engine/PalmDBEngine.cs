using System.IO;
using System.Text;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi.Engine
{
    internal static class PalmDBEngine
    {
        #region Const 

        private const int HeaderByteLength = 72;
        private const int NameByteLength = 32;

        #endregion

        public static PalmDB Read(Stream stream)
        {
            PalmDB result = null;
            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[NameByteLength];
            if (stream.Read(buffer, 0, NameByteLength) == NameByteLength)
            {
                result = new PalmDB { Name = Encoding.UTF8.GetString(buffer) };
                if (stream.TryReadUshort(out ushort attribute))
                {
                    result.Attribute = (PalmDBAttribute)attribute;
                }
                if (stream.TryReadUshort(out ushort version))
                {
                    result.Version = version;
                }
                stream.Seek(4 * 6, SeekOrigin.Current);

                if (stream.TryReadUint(out uint type))
                {
                    result.Type = type;
                }
                if (stream.TryReadUint(out uint creator))
                {
                    result.Creator = creator;
                }
                if (stream.TryReadUint(out uint uniqueIDseed))
                {
                    result.UniqueIDseed = uniqueIDseed;
                }
                if (stream.TryReadUint(out uint nextRecordListID))
                {
                    result.NextRecordListID = nextRecordListID;
                }
                if (stream.TryReadUshort(out ushort recordCount))
                {
                    result.RecordCount = recordCount;
                }

                for (ushort i = 0; i < recordCount; i++)
                {
                    var recordInfo = new PalmDBRecordInfo();
                    if (stream.TryReadUint(out uint recordInfoOffset))
                    {
                        recordInfo.Offset = recordInfoOffset;
                    }
                    if (stream.TryReadByte(out byte recordInfoAttribute))
                    {
                        recordInfo.Attribute = (PalmDBRecordAttribute)recordInfoAttribute;
                    }
                    var recordUniqueIDBuff = new byte[3];
                    if (stream.Read(recordUniqueIDBuff, 0, 3) == 3)
                    {
                        recordInfo.UniqueID = recordUniqueIDBuff.ToUInt32();
                    }
                    result.RecordInfoList.Add(recordInfo);
                }
            }

            return result;
        }

        public static void Write(PalmDB file, Stream stream)
        {
        }

    }
}
