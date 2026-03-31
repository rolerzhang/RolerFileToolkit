using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    /// <summary>
    /// EPUB 3.3: The collection element defines a related group of resources.
    /// </summary>
    public class Collection
    {
        public string Role { get; set; }
        public string Dir { get; set; }
        public string Id { get; set; }
        public string Language { get; set; }
        public Metadata Metadata { get; set; }
        public IList<Collection> Collections { get; } = new List<Collection>();
        public IList<LinkElement> Links { get; } = new List<LinkElement>();
    }
}
