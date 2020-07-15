## Build Status
| Target | Branch |Recommended package version |
| ------ | ------ | ------ | ------ |
| Roler.Toolkit.File.Epub | master | [1.0.1](https://www.nuget.org/packages/Roler.Toolkit.File.Epub) | 
| Roler.Toolkit.File.Mobi | master | [1.0.1](https://www.nuget.org/packages/Roler.Toolkit.File.Mobi) |

## Sample Code
### Epub

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

### Mobi

```csharp
using (var mobiReader = new MobiReader(fileStream))
{
    var mobi = mobiReader.Read();

    var creator = mobi.Creator;
    var publisher = mobi.Publisher;
    var description = mobi.Description;
    //...

    Structure structure = mobi.Structure;   //Structure inside mobi file.

    string text = mobi.Text;    //full text content.
}
```