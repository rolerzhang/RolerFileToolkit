
namespace Roler.Toolkit.File.Mobi
{
    public class ContentFile
    {
        public string MediaType { get; set; }
        public string Source { get; private set; }

        public ContentFile(string mediaType, string source)
        {
            MediaType = mediaType;
            Source = source;
        }
    }
}
