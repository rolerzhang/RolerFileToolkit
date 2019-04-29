using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class Manifest
    {
        public string Id { get; set; }
        public IList<ManifestItem> Items { get; } = new List<ManifestItem>();
    }
}
