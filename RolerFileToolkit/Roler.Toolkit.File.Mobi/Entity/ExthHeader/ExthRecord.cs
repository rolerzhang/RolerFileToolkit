namespace Roler.Toolkit.File.Mobi.Entity
{
    public class ExthRecord
    {
        /// <summary>
        /// Gets or sets the Exth Record type. Just a number identifying what's stored in the record.
        /// </summary>
        public ExthRecordType Type { get; set; }

        /// <summary>
        /// Gets or sets the length of the EXTH record = L , including the 8 bytes in the type and length fields.
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// Gets or sets the Data of the EXTH record.
        /// </summary>
        public string Data { get; set; }
    }
}
