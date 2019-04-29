using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class NavMap
    {
        public NavInfo NavInfo { get; set; }
        public IList<NavPoint> NavPoints { get; set; }
    }
}
