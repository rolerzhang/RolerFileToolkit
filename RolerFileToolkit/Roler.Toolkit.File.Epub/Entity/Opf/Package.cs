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
    }
}
