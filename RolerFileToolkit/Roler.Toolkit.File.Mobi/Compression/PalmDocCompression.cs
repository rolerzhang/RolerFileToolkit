using System;
using System.Collections.Generic;

namespace Roler.Toolkit.File.Mobi.Compression
{
    /// <summary>
    /// PalmDoc byte pair compression (LZ77).
    /// </summary>
    internal class PalmDocCompression : ICompression
    {
        #region Const

        private const int BlockSize = 4096;

        #endregion

        public byte[] Compress(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            throw new NotImplementedException();
        }


        /// <remarks>
        /// Decompress:
        /// 0x00: "1 literal" copy that byte unmodified to the decompressed stream.
        /// 0x09 to 0x7f: "1 literal" copy that byte unmodified to the decompressed stream.
        /// 0x01 to 0x08: "literals": the byte is interpreted as a count from 1 to 8, and that many literals are copied unmodified from the compressed stream to the decompressed stream.
        /// 0x80 to 0xbf: "length, distance" pair: the 2 leftmost bits of this byte ('10') are discarded, and the following 6 bits are combined with the 8 bits of the next byte to make a 14 bit "distance, length" item.Those 14 bits are broken into 11 bits of distance backwards from the current location in the uncompressed text, and 3 bits of length to copy from that point(copying n+3 bytes, 3 to 10 bytes).
        /// 0xc0 to 0xff: "byte pair": this byte is decoded into 2 characters: a space character, and a letter formed from this byte XORed with 0x80.
        /// </remarks>
        public byte[] Decompress(byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var blockBuilder = new List<byte>(BlockSize);
            int dataLength = bytes.Length;
            int pos = 0;

            while (pos < dataLength && blockBuilder.Count < BlockSize)
            {
                byte ab = bytes[pos++];
                if (ab == 0x00 || (ab >= 0x09 && ab <= 0x7f))
                {
                    blockBuilder.Add(ab);
                }
                else if (ab >= 0x01 && ab <= 0x08)
                {
                    if (pos + ab > dataLength)
                    {
                        //invaild data, not enough to copy.
                        blockBuilder.Clear();
                        break;
                    }
                    for (byte i = 0; i < ab; i++)
                    {
                        blockBuilder.Add(bytes[pos++]);
                    }
                }
                else if (ab >= 0x80 && ab <= 0xbf)
                {
                    byte nextByte = (pos < dataLength) ? bytes[pos++] : (byte)0;
                    uint b = (uint)(((ab & 0x3f) << 8) | nextByte);
                    uint dist = b >> 3;
                    int uncompressedPos = blockBuilder.Count - ((int)dist);
                    if (uncompressedPos >= 0 && dist != 0)
                    {
                        uint len = (b & 0x07) + 3;
                        for (int i = 0; i < len; i++)
                        {
                            blockBuilder.Add(blockBuilder[uncompressedPos + i]);
                        }
                    }
                    else
                    {
                        //invaild data, distance larger then uncommpressed stream length  or equal to 0.
                        blockBuilder.Clear();
                        break;
                    }
                }
                else if (ab >= 0xc0 && ab <= 0xff)
                {
                    blockBuilder.Add(0x20);
                    blockBuilder.Add((byte)(ab ^ 0x80));
                }
            }
            return blockBuilder.ToArray();
        }
    }
}
