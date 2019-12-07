using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi
{
    public class Structure
    {
        public PalmDB PalmDB { get; set; }
        public PalmDOCHeader PalmDOCHeader { get; set; }
        public MobiHeader MobiHeader { get; set; }
        public ExthHeader ExthHeader { get; set; }
        public string FullName { get; set; }
    }
}
