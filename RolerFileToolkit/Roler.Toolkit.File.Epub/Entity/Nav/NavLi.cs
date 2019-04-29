namespace Roler.Toolkit.File.Epub.Entity
{
    public class NavLi
    {
        public string Id { get; set; }
        public bool IsHidden { get; set; }
        public NavA A { get; set; }
        public NavSpan Span { get; set; }
        public NavOl Ol { get; set; }
    }
}
