namespace Roler.Toolkit.File.Epub.Entity
{
    public class LinkElement
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Id { get; set; }
        public string Refines { get; set; }
        public string MediaType { get; set; }

        /// <summary>
        /// EPUB 3.3: A space-separated list of property values.
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// EPUB 3.3: The language of the resource referenced by the href attribute.
        /// </summary>
        public string Hreflang { get; set; }
    }
}
