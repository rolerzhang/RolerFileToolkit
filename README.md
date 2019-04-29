# Epub

```csharp
using (var epubReader = new EpubReader(stream))
{
    if (epubReader.TryRead(out Epub epub))
    {
        var title = epub.Title;
        var creator = epub.Creator;
        var publisher = epub.Publisher;
        var description = epub.Description;
        //...

        ContentFile cover = epub.Cover;
        IList<Chapter> chapters = epub.Chapters;
        IList<ContentFile> allFiles = epub.AllFiles;
        IList<ContentFile> ReadingFiles = epub.ReadingFiles;    //Ordered files for read.

        Structure structure = epub.Structure;   //Structure inside epub file.
        float version = structure.Package.Version;  //The version of epub file.

        Stream coverStream = epubReader.ReadContentFile(cover.FilePath);    //read content file by file path.
    }
}
```