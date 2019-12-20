using System.Collections.Generic;

namespace Roler.Toolkit.File.Mobi
{
    internal class BitReader
    {
        private readonly IList<byte> _data;
        private readonly int _nbits;

        private uint _pos = 0;

        public bool IsEnd => this._nbits > this._pos;

        public BitReader(byte[] bytes)
        {
            this._data = new List<byte>(bytes)
            {
                0,
                0,
                0,
                0
            };
            this._nbits = (this._data.Count - 4) * 8;
        }

        public ulong Peek(ulong n)
        {
            ulong r = 0;
            ulong g = 0;
            while (g < n)
            {
                r = (r << 8) | (char)(_data[(int)((long)(_pos + g >> 3))]);
                g = g + 8 - ((_pos + g) & 7);
            }
            return r >> (int)((long)(g - n)) & ((ulong)(1) << (int)n) - 1;
        }

        public bool Eat(uint n)
        {
            _pos += n;
            return _pos <= _nbits;
        }
    }
}
