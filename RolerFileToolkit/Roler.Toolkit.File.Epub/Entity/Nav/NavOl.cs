using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub.Entity
{
    public class NavOl
    {
        public string Id { get; set; }
        public bool IsHidden { get; set; }
        public IList<NavLi> Items { get; } = new List<NavLi>();
    }
}
