namespace Roler.Toolkit.File.Mobi.Compression
{
    internal interface ICompression
    {
        byte[] Compress(byte[] bytes);
        byte[] Decompress(byte[] bytes);
    }
}
