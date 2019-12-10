namespace Roler.Toolkit.File.Mobi.Entity
{
    public class VideRecord
    {
        /// <summary>
        /// Gets or sets the identifier, always 'VIDE'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the media data continues to the end of this record.
        /// </summary>
        public byte[] Media { get; set; }
    }
}
