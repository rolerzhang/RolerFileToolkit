using System.Collections.Generic;

namespace Roler.Toolkit.File.Mobi.Entity
{
    public class ExthHeader
    {
        /// <summary>
        /// Gets or sets the identifier, always 'EXTH'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the length of the EXTH header, including the previous 4 bytes.
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// Gets or sets the number of records in the EXTH header. the rest of the EXTH header consists of repeated EXTH records to the end of the EXTH length.
        /// </summary>
        public uint RecordCount { get; set; }

        public IList<ExthRecord> RecordList { get; } = new List<ExthRecord>();
    }
}
