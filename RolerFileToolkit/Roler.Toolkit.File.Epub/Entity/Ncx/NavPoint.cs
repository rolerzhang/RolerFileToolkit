using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class NavPoint
    {
        public string Id { get; set; }
        public string PlayOrder { get; set; }
        public NavPointLabel Label { get; set; }
        public NavPointContent Content { get; set; }
        public IList<NavPoint> Children { get; set; }
    }
}
