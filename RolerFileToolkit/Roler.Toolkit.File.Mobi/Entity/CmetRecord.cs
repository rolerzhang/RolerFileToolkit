namespace Roler.Toolkit.File.Mobi.Entity
{
    public class CmetRecord
    {
        /// <summary>
        /// Gets or sets the identifier, always 'CMET'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the value0, fixed value: 0x0000000C.
        /// </summary>
        public uint Value0 { get; set; }

        /// <summary>
        /// Gets or sets the length of the text.
        /// </summary>
        public uint TextLength { get; set; }

        /// <summary>
        /// Gets or sets the compilation output text, line endings are CRLF.
        /// </summary>
        public string Text { get; set; }
    }
}
