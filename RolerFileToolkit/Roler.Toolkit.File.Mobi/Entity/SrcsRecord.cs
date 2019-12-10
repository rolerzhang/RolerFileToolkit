namespace Roler.Toolkit.File.Mobi.Entity
{
    public class SrcsRecord
    {
        /// <summary>
        /// Gets or sets the identifier, always 'SRCS'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the value0, fixed value: 0x00000010.
        /// </summary>
        public uint Value0 { get; set; }

        /// <summary>
        /// Gets or sets the value1, fixed value: 0x0000002f.
        /// </summary>
        public uint Value1 { get; set; }

        /// <summary>
        /// Gets or sets the value2, fixed value: 0x00000001.
        /// </summary>
        public uint Value2 { get; set; }

        /// <summary>
        /// Gets or sets the zip archive continues to the end of this record.
        /// </summary>
        public byte[] Zip { get; set; }
    }
}
