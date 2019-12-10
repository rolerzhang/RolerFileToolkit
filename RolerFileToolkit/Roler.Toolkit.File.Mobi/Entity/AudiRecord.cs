namespace Roler.Toolkit.File.Mobi.Entity
{
    public class AudiRecord
    {
        /// <summary>
        /// Gets or sets the identifier, always 'AUDI'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the media data continues to the end of this record.
        /// </summary>
        public byte[] Media { get; set; }
    }
}
