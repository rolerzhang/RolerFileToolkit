namespace Roler.Toolkit.File.Mobi.Entity
{
    public class PalmDOCHeader
    {
        public CompressionType Compression { get; set; }

        /// <summary>
        /// Always zero.
        /// </summary>
        public short Unused { get; }

        /// <summary>
        /// Gets or sets the uncompressed length of the entire text of the book.
        /// </summary>
        public uint TextLength { get; set; }

        /// <summary>
        /// Gets or sets the number of PDB records used for the text of the book. 
        /// </summary>
        public ushort RecordCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of each record containing text, always 4096.
        /// </summary>
        public ushort RecordSize { get; set; }

        /// <summary>
        /// Gets or sets the current reading position, as an offset into the uncompressed text.
        /// </summary>
        public uint CurrentPosition { get; set; }

        /// <summary>
        /// Gets or sets the encryption type, Only HUFF/CDIC compression. 
        /// </summary>
        public EncryptionType Encryption { get; set; }
    }
}
