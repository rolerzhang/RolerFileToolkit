namespace Roler.Toolkit.File.Mobi.Entity
{
    public enum CompressionType : ushort
    {
        Unkown,

        /// <summary>
        /// No compression.
        /// </summary>
        No = 1,

        /// <summary>
        /// PalmDOC compression.
        /// </summary>
        PalmDOC = 2,

        /// <summary>
        /// HUFF/CDIC compression.
        /// </summary>
        HUFF_CDIC = 17480,
    }
}
