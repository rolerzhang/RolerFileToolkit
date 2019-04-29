namespace Roler.Toolkit.File.Epub.Entity
{
    public class MetaElement
    {
        public string Id { get; set; }
        public string Language { get; set; }
        public string Dir { get; set; }
        public string Scheme { get; set; }
        public string Property { get; set; }
        public string Refines { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Attribute name, Only Epub2.0
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute content, Only Epub2.0
        /// </summary>
        public string Content { get; set; }
    }
}
