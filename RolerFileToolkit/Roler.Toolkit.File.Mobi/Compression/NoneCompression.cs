namespace Roler.Toolkit.File.Mobi.Compression
{
    internal class NoneCompression : ICompression
    {
        public byte[] Compress(byte[] bytes)
        {
            return bytes;
        }

        public byte[] Decompress(byte[] bytes)
        {
            return bytes;
        }
    }
}
