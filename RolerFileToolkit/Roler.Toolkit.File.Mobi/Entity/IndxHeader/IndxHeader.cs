namespace Roler.Toolkit.File.Mobi.Entity
{
    public class IndxHeader
    {
        /// <summary>
        /// Gets or sets the identifier, always 'INDX'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the length of the INDX header, including the previous 4 bytes.
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// Gets or sets the type of the index.
        /// </summary>
        public IndexType IndexType { get; set; }

        /// <summary>
        /// Gets or sets the offset to the IDXT section.
        /// </summary>
        public uint IdxtStart { get; set; }

        /// <summary>
        /// Gets or sets the number of index records.
        /// </summary>
        public uint IndexCount { get; set; }

        /// <summary>
        /// Gets or sets the number of index records.
        /// </summary>
        public TextEncoding IndexEncoding { get; set; }

        /// <summary>
        /// Gets or sets the language code of the index.
        /// </summary>
        public string IndexLanguage { get; set; }

        /// <summary>
        /// Gets or sets the number of index entries.
        /// </summary>
        public uint TotalIndexCount { get; set; }

        /// <summary>
        /// Gets or sets the offset to the ORDT section.
        /// </summary>
        public uint OrdtStart { get; set; }

        /// <summary>
        /// Gets or sets the offset to the LIGT section.
        /// </summary>
        public uint LigtStart { get; set; }

    }
}
