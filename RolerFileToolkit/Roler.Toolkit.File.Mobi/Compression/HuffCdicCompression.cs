using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Roler.Toolkit.File.Mobi.Compression
{
    /// <summary>
    /// HUFF/CDIC compression
    /// </summary>
    internal class HuffCdicCompression : ICompression
    {
        public readonly static byte[] HuffBytesStart = { 0x48, 0x55, 0x46, 0x46, 0x00, 0x00, 0x00, 0x18 };
        public readonly static byte[] CdicBytesStart = { 0x43, 0x44, 0x49, 0x43, 0x00, 0x00, 0x00, 0x10 };

        private readonly List<byte> _huff;
        private readonly List<byte[]> _cidcList;
        private readonly IList<Tuple<int, int, uint>> _huffDict1 = new List<Tuple<int, int, uint>>();
        private readonly IList<uint> _minCodeList = new List<uint>();
        private readonly IList<uint> _maxCodeList = new List<uint>();
        private readonly IList<Tuple<IList<byte>, int>> _dictionary = new List<Tuple<IList<byte>, int>>();

        public IReadOnlyList<byte> Huff => this._huff;
        public IReadOnlyList<byte[]> CdicList => this._cidcList;
        public uint ExtraFlags { get; set; }

        public HuffCdicCompression()
        {
        }

        public HuffCdicCompression(byte[] huff, IList<byte[]> cdicList)
        {
            if (huff is null)
            {
                throw new ArgumentNullException(nameof(huff));
            }

            if (cdicList is null)
            {
                throw new ArgumentNullException(nameof(cdicList));
            }

            this._huff = new List<byte>(huff);
            this._cidcList = new List<byte[]>(cdicList);

            this.InitMember();
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
            return this.Unpack(bytes);
        }

        private void InitMember()
        {
            this.LoadHuff();

            this._dictionary.Clear();
            foreach (var cdicBytes in this._cidcList)
            {
                this.LoadOneCdic(cdicBytes);
            }
        }

        private void LoadHuff()
        {
            if (this._huff.GetRange(0, 8).SequenceEqual(HuffBytesStart))
            {
                uint off1 = this._huff.GetRange(8, 4).ToArray().ToUInt32();
                uint off2 = this._huff.GetRange(12, 4).ToArray().ToUInt32();

                this._huffDict1.Clear();
                for (int i = 0; i < 256; i++)
                {
                    uint v = this._huff.GetRange((int)(off1 + (i * 4)), 4).ToArray().ToUInt32();
                    var codeLength = (int)(v & 0x1f);
                    var term = (int)(v & 0x80);
                    uint maxCode = v >> 8;

                    if (codeLength == 0)
                    {
                        throw new InvalidDataException("invalid data: Huff");
                    }
                    else
                    {
                        maxCode = ((maxCode + 1) << (32 - codeLength)) - 1;
                    }
                    this._huffDict1.Add(new Tuple<int, int, uint>(codeLength, term, maxCode));
                }

                this._minCodeList.Clear();
                this._maxCodeList.Clear();

                this._minCodeList.Add(uint.MinValue);
                this._maxCodeList.Add(uint.MaxValue);
                for (int i = 0; i < 64; i++)
                {
                    var v = this._huff.GetRange((int)(off2 + (i * 4)), 4).ToArray().ToUInt32();
                    var step = 32 - i / 2 - 1;
                    if (i % 2 == 0)
                    {
                        this._minCodeList.Add(v << step);
                    }
                    else
                    {
                        this._maxCodeList.Add(((v + 1) << step) - 1);
                    }
                }
            }
        }

        private void LoadOneCdic(byte[] cdicBytes)
        {
            if (cdicBytes is null)
            {
                throw new ArgumentNullException(nameof(cdicBytes));
            }
            if (cdicBytes.RangeEqual(0, 8, CdicBytesStart))
            {
                var phrases = cdicBytes.Skip(8).Take(4).ToArray().ToUInt32();
                var bits = cdicBytes.Skip(12).Take(4).ToArray().ToUInt32();

                var n = Math.Min(1 << (int)bits, phrases - this._dictionary.Count);
                for (int i = 0; i < n; i++)
                {
                    var off = cdicBytes.Skip(16 + (i * 2)).Take(2).ToArray().ToUInt16();
                    var blen = cdicBytes.Skip(16 + off).Take(2).ToArray().ToUInt16();
                    var length = 18 + off + (blen & 0x7fff);
                    var sliceByteList = new List<byte>();
                    for (int j = 18 + off; j < length; j++)
                    {
                        sliceByteList.Add(cdicBytes[j]);
                    }
                    this._dictionary.Add(new Tuple<IList<byte>, int>(sliceByteList, blen & 0x8000));
                }
            }
        }

        private byte[] Unpack(byte[] bytes)
        {
            var bitsLeft = bytes.Length * 8;
            var data = new List<byte>(bytes);
            data.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            var pos = 0;
            var x = data.GetRange(pos, 8).ToArray().ToUInt64();
            var n = 32;
            var result = new List<byte>();

            while (true)
            {
                if (n <= 0)
                {
                    pos += 4;
                    x = data.GetRange(pos, 8).ToArray().ToUInt64();
                    n += 32;
                }

                var code = (uint)((x >> n) & 0xFFFFFFFF);
                var tuple = this._huffDict1[(int)(code >> 24)];
                var codeLength = tuple.Item1;
                var maxCode = tuple.Item3;
                if (tuple.Item2 <= 0)
                {
                    while (code < this._minCodeList[codeLength])
                    {
                        codeLength++;
                    }

                    maxCode = this._maxCodeList[codeLength];
                }

                n -= codeLength;
                bitsLeft -= codeLength;
                if (bitsLeft < 0)
                    break;

                var r = (int)((maxCode - code) >> (32 - codeLength));
                var tuple2 = this._dictionary[r];
                var sliceList = tuple2.Item1;
                if (tuple2.Item2 <= 0)
                {
                    this._dictionary.RemoveAt(r);
                    this._dictionary.Insert(r, null);
                    sliceList = this.Unpack(sliceList.ToArray());
                    this._dictionary.RemoveAt(r);
                    this._dictionary.Insert(r, new Tuple<IList<byte>, int>(sliceList, 1));
                }

                result.AddRange(sliceList);
            }

            return result.ToArray();
        }
    }
}
