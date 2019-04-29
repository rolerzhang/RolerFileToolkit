using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class Metadata
    {
        public IList<DcElement> Contributors { get; } = new List<DcElement>();
        public IList<DcElement> Coverages { get; } = new List<DcElement>();
        public IList<DcElement> Creators { get; } = new List<DcElement>();
        public DcElement Date { get; set; }
        public IList<DcElement> Descriptions { get; } = new List<DcElement>();
        public IList<DcElement> Formats { get; } = new List<DcElement>();
        public IList<DcElement> Identifiers { get; } = new List<DcElement>();
        public IList<DcElement> Languages { get; } = new List<DcElement>();
        public IList<DcElement> Publishers { get; } = new List<DcElement>();
        public IList<DcElement> Relations { get; } = new List<DcElement>();
        public IList<DcElement> Rights { get; } = new List<DcElement>();
        public IList<DcElement> Sources { get; } = new List<DcElement>();
        public IList<DcElement> Subjects { get; } = new List<DcElement>();
        public IList<DcElement> Titles { get; } = new List<DcElement>();
        public IList<DcElement> Types { get; } = new List<DcElement>();
        public IList<MetaElement> Metas { get; } = new List<MetaElement>();
        public IList<LinkElement> Links { get; } = new List<LinkElement>();
    }
}
