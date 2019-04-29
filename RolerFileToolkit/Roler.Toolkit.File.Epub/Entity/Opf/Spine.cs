using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class Spine
    {
        public string Id { get; set; }
        public string Toc { get; set; }
        public string PageProgressionDirection { get; set; }
        public IList<SpineItemRef> Items { get; } = new List<SpineItemRef>();
    }
}
