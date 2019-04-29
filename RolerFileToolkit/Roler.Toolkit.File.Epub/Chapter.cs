using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub
{
    public class Chapter
    {
        public string Title { get; set; }
        public string ContentFilePath { get; set; }
        public string SecondPath { get; set; }
        public IList<Chapter> Chapters { get; } = new List<Chapter>();
    }
}
