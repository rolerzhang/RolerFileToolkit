
namespace Roler.Toolkit.File.Epub
{
    public class ContentFile
    {
        public string MediaType { get; set; }
        public string FilePath { get; private set; }

        public ContentFile(string mediaType, string filePath)
        {
            MediaType = mediaType;
            FilePath = filePath;
        }
    }
}
