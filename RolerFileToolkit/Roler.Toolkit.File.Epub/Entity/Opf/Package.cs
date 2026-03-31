using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class Package
    {
        public float Version { get; set; }
        public string Identifier { get; set; }
        public string Prefix { get; set; }
        public string Language { get; set; }
        public string Dir { get; set; }
        public string Id { get; set; }
        public Metadata Metadata { get; set; }
        public Manifest Manifest { get; set; }
        public Spine Spine { get; set; }

        /// <summary>
        /// EPUB 3.3: A list of collection elements that define related groups of resources.
        /// </summary>
        public IList<Collection> Collections { get; } = new List<Collection>();
    }
}
