using System;
using System.Collections.Generic;
using System.Linq;

namespace Roler.Toolkit.File.Mobi.Compression
{
    internal class HuffCdicCompression : ICompression
    {
        private uint _off1;
        private uint _off2;
        private uint _entryBits;
        private uint[] _huffDict1;
        private uint[] _huffDict2;

        public List<byte> HuffData { get; }
        public List<byte> CdicData { get; }
        public IList<IList<byte>> HuffDicts { get; }
        public uint ExtraFlags { get; set; }

        public HuffCdicCompression(List<byte> huffData, List<byte> cdicData, IList<IList<byte>> huffDicts)
        {
            this.HuffData = huffData ?? throw new ArgumentNullException(nameof(huffData));
            this.CdicData = cdicData ?? throw new ArgumentNullException(nameof(cdicData));
            this.HuffDicts = huffDicts ?? throw new ArgumentNullException(nameof(huffDicts));

            this.InitMember();
        }

        private void InitMember()
        {
            var byteList = new List<byte>();

            byteList.Clear();
            byteList.AddRange(this.HuffData.GetRange(16, 4));
            this._off1 = BytesToUint(byteList.ToArray());

            byteList.Clear();
            byteList.AddRange(this.HuffData.GetRange(20, 4));
            this._off2 = BytesToUint(byteList.ToArray());

            byteList.Clear();
            byteList.AddRange(this.CdicData.GetRange(12, 4));
            this._entryBits = BytesToUint(byteList.ToArray());

            var uintList = new List<uint>();
            for (int i = 0; i < 256; i++)
            {
                byteList.Clear();
                byteList.AddRange(this.HuffData.GetRange((int)(_off1 + (i * 4)), 4));
                uintList.Clear();
                uintList.Add(BitConverter.ToUInt32(byteList.ToArray(), 0));
                this._huffDict1 = uintList.ToArray();
            }
            for (int i = 0; i < 64; i++)
            {
                byteList.Clear();
                byteList.AddRange(this.HuffData.GetRange((int)(_off2 + (i * 4)), 4));
                uintList.Clear();
                uintList.Add(BitConverter.ToUInt32(byteList.ToArray(), 0));
                this._huffDict2 = uintList.ToArray();
            }
        }

        public byte[] Compress(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            throw new NotImplementedException();
        }

        public byte[] Decompress(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            List<byte> dataTemp = new List<byte>(bytes);
            int size = GetSizeOfTrailingDataEntries(dataTemp.ToArray(), dataTemp.Count, this.ExtraFlags);
            return Unpack(new BitReader(dataTemp.GetRange(0, dataTemp.Count - size).ToArray()), 0, this._huffDict1, this._huffDict2, this.HuffDicts, (int)this._entryBits);
        }

        private static uint BytesToUint(byte[] bytes)
        {
            return (uint)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
        }

        private static int GetSizeOfTrailingDataEntries(byte[] ptr, int size, uint flags)
        {
            int retval = 0;
            flags >>= 1;
            while (flags > 0)
            {
                if ((flags & 1) > 0)
                {
                    retval += (int)GetSizeOfTrailingDataEntry(ptr, size - retval);
                }
                flags >>= 1;
            }
            return retval;
        }

        private static uint GetSizeOfTrailingDataEntry(byte[] ptr, int size)
        {
            uint retval = 0;
            int bitpos = 0;
            while (true)
            {
                uint v = (char)(ptr[size - 1]);
                retval |= (v & 0x7F) << bitpos;
                bitpos += 7;
                size -= 1;
                if ((v & 0x80) != 0 || (bitpos >= 28) || (size == 0))
                {
                    return retval;
                }
            }
        }

        private static byte[] Unpack(BitReader bitReader, int depth, uint[] huffDict1, uint[] huffDict2, IList<IList<byte>> huffDicts, int entryBits)
        {
            List<byte> result = new List<byte>();

            if (depth > 32)
            {
                throw new Exception("corrupt file");
            }
            while (bitReader.IsEnd)
            {
                ulong dw = bitReader.Peek(32);
                uint v = (huffDict1[dw >> 24]);
                uint codeLen = v & 0x1F;
                ulong code = dw >> (int)(32 - codeLen);
                ulong r = (v >> 8);
                if ((v & 0x80) == 0)
                {
                    while (code < huffDict2[(codeLen - 1) * 2])
                    {
                        codeLen += 1;
                        code = dw >> (int)(32 - codeLen);
                    }
                    r = huffDict2[(codeLen - 1) * 2 + 1];
                }
                r -= code;
                if (bitReader.Eat(codeLen))
                {
                    ulong dicno = r >> entryBits;
                    ulong off1 = 16 + (r - (dicno << entryBits)) * 2;
                    IList<byte> dic = huffDicts[(int)(long)dicno];
                    int off2 = 16 + (char)(dic[(int)((long)off1)]) * 256 + (char)(dic[(int)((long)off1) + 1]);
                    int blen = ((char)(dic[off2]) * 256 + (char)(dic[off2 + 1]));
                    List<byte> slicelist = dic.ToList().GetRange(off2 + 2, blen & 0x7fff);
                    byte[] slice = slicelist.ToArray();
                    if ((blen & 0x8000) > 0)
                    {
                        result.AddRange(slice);
                    }
                    else
                    {
                        result.AddRange(Unpack(new BitReader(slice), depth + 1, huffDict1, huffDict2, huffDicts, entryBits));
                    }
                }
            }
            return result.ToArray();
        }
    }
}
