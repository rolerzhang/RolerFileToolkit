using System.Collections.Generic;

namespace Roler.Toolkit.File.Epub
{
    public class Epub
    {
        public Structure Structure { get; set; }

        public string Contributor { get; set; }
        public string Coverage { get; set; }
        public string Creator { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string Identifier { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public string Relation { get; set; }
        public string Rights { get; set; }
        public string Source { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public ContentFile Cover { get; set; }
        public IList<Chapter> Chapters { get; } = new List<Chapter>();
        public IList<ContentFile> ReadingFiles { get; } = new List<ContentFile>();
        public IList<ContentFile> AllFiles { get; } = new List<ContentFile>();
    }
}
