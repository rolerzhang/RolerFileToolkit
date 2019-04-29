using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class Nav
    {
        public IList<NavElement> NavElements { get; } = new List<NavElement>();
    }
}
