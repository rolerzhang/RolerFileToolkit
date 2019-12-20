using System;
using System.Collections.Generic;
using System.Linq;

namespace Roler.Toolkit.File.Mobi.Compression
{
    internal class PalmDocCompression : ICompression
    {
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

            IList<byte> blockBuilder = new List<byte>();
            IList<byte> dataTemp = new List<byte>(bytes)
            {
                0
            };
            int pos = 0;
            IList<byte> temps = new List<byte>();

            while (pos < dataTemp.Count && blockBuilder.Count < 4096)
            {
                byte ab = dataTemp[pos++];
                if (ab == 0x00 || (ab > 0x08 && ab <= 0x7f))
                {
                    blockBuilder.Add(ab);
                }
                else if (ab > 0x00 && ab <= 0x08)
                {
                    temps.Clear();
                    temps.Add(0);
                    temps.Add(0);
                    temps.Add(0);
                    temps.Add(ab);
                    uint value = BytesToUint(temps.ToArray());
                    for (uint i = 0; i < value; i++)
                    {
                        blockBuilder.Add(dataTemp[pos++]);
                    }
                }
                else if (ab > 0x7f && ab <= 0xbf)
                {
                    temps.Clear();
                    temps.Add(0);
                    temps.Add(0);
                    temps.Add((byte)(ab & 0x3f));
                    if (pos < dataTemp.Count)
                    {
                        temps.Add(dataTemp[pos++]);
                    }
                    else
                    {
                        temps.Add(0);
                    }

                    uint b = BytesToUint(temps.ToArray());
                    uint dist = b >> 3;
                    uint len = ((b << 29) >> 29);
                    int uncompressedPos = blockBuilder.Count - ((int)dist);
                    for (int i = 0; i < (len + 3); i++)
                    {
                        try
                        {
                            blockBuilder.Add(blockBuilder[uncompressedPos + i]);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else if (ab > 0xbf && ab <= 0xff)
                {
                    blockBuilder.Add(32);
                    blockBuilder.Add((byte)(ab ^ 0x80));
                }
            }
            return blockBuilder.ToArray();
        }

        private static uint BytesToUint(byte[] bytes)
        {
            return (uint)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
        }
    }
}
