namespace Roler.Toolkit.File.Epub.Entity
{
    public class NavElement
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public bool IsHidden { get; set; }
        public NavHead NavHead { get; set; }
        public NavOl Ol { get; set; }
    }
}
