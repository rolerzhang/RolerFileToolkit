using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Roler.Toolkit.File.Epub.Define;
using Roler.Toolkit.File.Epub.Engine;
using Roler.Toolkit.File.Epub.Entity;
using Roler.Toolkit.File.Epub.Helper;

namespace Roler.Toolkit.File.Epub
{
    public class EpubReader : IDisposable
    {
        private const string SEPARATOR = ",";
        private const string COVER_PROPERTY = "cover-image";
        private const string COVER_NAME = "cover";
        private const char HREF_SEPARATOR = '#';

        private bool _disposed;
        private readonly ZipArchive _archive;

        public EpubReader(Stream stream)
        {
            this._archive = new ZipArchive(stream);
        }

        #region Methods

        public Epub Read()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("archive");
            }

            var structure = this.ReadStructure();
            var metadata = structure.Package.Metadata;
            var result = new Epub
            {
                Structure = structure,
                Contributer = String.Join(SEPARATOR, metadata.Contributors.Select(p => p.Value)),
                Coverage = String.Join(SEPARATOR, metadata.Coverages.Select(p => p.Value)),
                Creator = String.Join(SEPARATOR, metadata.Creators.Select(p => p.Value)),
                Date = metadata.Date?.Value,
                Description = String.Join(SEPARATOR, metadata.Descriptions.Select(p => p.Value)),
                Format = String.Join(SEPARATOR, metadata.Formats.Select(p => p.Value)),
                Identifier = String.Join(SEPARATOR, metadata.Identifiers.Select(p => p.Value)),
                Language = String.Join(SEPARATOR, metadata.Languages.Select(p => p.Value)),
                Publisher = String.Join(SEPARATOR, metadata.Publishers.Select(p => p.Value)),
                Relation = String.Join(SEPARATOR, metadata.Relations.Select(p => p.Value)),
                Rights = String.Join(SEPARATOR, metadata.Rights.Select(p => p.Value)),
                Source = String.Join(SEPARATOR, metadata.Sources.Select(p => p.Value)),
                Subject = String.Join(SEPARATOR, metadata.Subjects.Select(p => p.Value)),
                Title = String.Join(SEPARATOR, metadata.Titles.Select(p => p.Value)),
                Type = String.Join(SEPARATOR, metadata.Types.Select(p => p.Value)),
            };

            var opfDirectory = Path.GetDirectoryName(structure.Container.FullPath);

            result.Cover = FindCoverFile(structure.Package, opfDirectory);
            FillChapters(result.Chapters, structure, opfDirectory);
            FillReadingFiles(result.ReadingFiles, structure.Package, opfDirectory);
            FillAllFiles(result.AllFiles, structure.Package, opfDirectory);

            return result;
        }

        public bool TryRead(out Epub epub)
        {
            bool result;

            try
            {
                epub = this.Read();
                result = true;
            }
            catch (Exception)
            {
                epub = null;
                result = false;
            }

            return result;
        }

        public Stream ReadContentFile(string filePath)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("archive");
            }

            if (!String.IsNullOrWhiteSpace(filePath))
            {
                var zipEntry = this._archive.GetEntry(filePath);
                if (zipEntry != null)
                {
                    return zipEntry.Open();
                }
            }
            return Stream.Null;
        }

        public string ReadContentAsText(string filePath)
        {
            using (var stream = this.ReadContentFile(filePath))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        #region Structure

        private Structure ReadStructure()
        {
            var container = ReadFileFromArchive(this._archive, Const.CONTAINER_PATH, p => ContainerEngine.Read(p));
            if (container == null)
            {
                throw new InvalidDataException("container file not found.");
            }

            Structure result = new Structure
            {
                Container = container,
                Package = ReadFileFromArchive(this._archive, container.FullPath, p => OpfEngine.Read(p)) ?? throw new InvalidDataException("opf file not found."),
            };

            var opfDirectory = Path.GetDirectoryName(container.FullPath);
            if (result.Package.Version >= 3f)
            {
                string relativePath = FindNavFilePath(result.Package);
                var filePath = PathHelper.Combine(opfDirectory, relativePath);
                result.Nav = ReadFileFromArchive(this._archive, filePath, p => NavEngine.Read(p)) ?? throw new InvalidDataException("navigation file not found.");
            }
            else
            {
                var relativePath = FindNcxFilePath(result.Package);
                var filePath = PathHelper.Combine(opfDirectory, relativePath);
                result.Ncx = ReadFileFromArchive(this._archive, filePath, p => NcxEngine.Read(p)) ?? throw new InvalidDataException("ncx file not found.");
            }

            return result;
        }

        private static T ReadFileFromArchive<T>(ZipArchive zipArchive, string filePath, Func<Stream, T> func)
        {
            if (zipArchive != null && !String.IsNullOrWhiteSpace(filePath) && func != null)
            {
                var zipEntry = zipArchive.GetEntry(filePath);
                if (zipEntry != null && zipEntry.Length > 0)
                {
                    using (Stream stream = zipEntry.Open())
                    {
                        return func(stream);
                    }
                }
            }
            return default(T);
        }

        private static string FindNavFilePath(Package package)
        {
            if (package != null && package.Manifest != null && package.Manifest.Items.Any())
            {
                var mainfestItem = package.Manifest.Items.FirstOrDefault(p => p.IsPropertiesContains("nav"));
                return mainfestItem?.Href;
            }
            return null;
        }

        private static string FindNcxFilePath(Package package)
        {
            if (package != null && package.Spine != null && !String.IsNullOrWhiteSpace(package.Spine.Toc) && package.Manifest != null)
            {
                var mainfestItem = package.Manifest.Items.FirstOrDefault(p => p.Id == package.Spine.Toc);
                return mainfestItem?.Href;
            }
            return null;
        }

        #endregion

        #region Cover

        private static ContentFile FindCoverFile(Package package, string opfDirectory)
        {
            var coverItem = FindCoverManifestItem(package);
            if (coverItem != null)
            {
                return new ContentFile(coverItem.MediaType, PathHelper.Combine(opfDirectory, coverItem.Href));
            }
            return null;
        }

        private static ManifestItem FindCoverManifestItem(Package package)
        {
            if (package != null && package.Manifest != null && package.Manifest.Items.Any())
            {
                if (package.Version >= 3f)
                {
                    return package.Manifest.Items.FirstOrDefault(p => p.IsPropertiesContains(COVER_PROPERTY));
                }
                else
                {
                    var manifestItemId = package.Metadata?.Metas?.FirstOrDefault(p => p.Name == COVER_NAME)?.Content;
                    if (!String.IsNullOrWhiteSpace(manifestItemId))
                    {
                        return package.Manifest.Items.FirstOrDefault(p => p.Id == manifestItemId);
                    }
                }
            }
            return null;
        }

        #endregion

        #region Chapters

        private static void FillChapters(IList<Chapter> chapters, Structure structure, string opfDirectory)
        {
            if (chapters != null && structure != null && structure.Package != null)
            {
                if (structure.Package.Version >= 3f)
                {
                    FillChatpersByNav(chapters, structure.Nav, opfDirectory);
                }
                else
                {
                    FillChaptersByNcx(chapters, structure.Ncx, opfDirectory);
                }
            }
        }

        private static void FillChatpersByNav(IList<Chapter> chapters, Nav nav, string opfDirectory)
        {
            if (chapters != null && nav != null)
            {
                var navLis = nav.NavElements.FirstOrDefault(p => !p.IsHidden)?.Ol?.Items?.Where(p => !p.IsHidden);
                if (navLis != null && navLis.Any())
                {
                    foreach (var li in navLis)
                    {
                        chapters.Add(GetChapterFromNavLi(li, opfDirectory));
                    }
                }
            }
        }

        private static void FillChaptersByNcx(IList<Chapter> chapters, Ncx ncx, string opfDirectory)
        {
            if (chapters != null && ncx != null && ncx.NavMap != null && ncx.NavMap.NavPoints != null)
            {
                foreach (var navPoint in ncx.NavMap.NavPoints)
                {
                    chapters.Add(GetChapterFromNavPoint(navPoint, opfDirectory));
                }
            }
        }

        private static Chapter GetChapterFromNavLi(NavLi li, string opfDirectory)
        {
            Chapter result = null;
            if (li != null && !li.IsHidden)
            {
                result = new Chapter();
                if (li.A != null)
                {
                    result.Title = String.IsNullOrWhiteSpace(li.A.Value) ? li.A.Title : li.A.Value;
                    if (!String.IsNullOrWhiteSpace(li.A.Href))
                    {
                        var hrefSplit = li.A.Href.Split(new char[] { HREF_SEPARATOR });
                        result.ContentFilePath = PathHelper.Combine(opfDirectory, hrefSplit[0]);
                        if (hrefSplit.Length > 1)
                        {
                            result.SecondPath = hrefSplit[1];
                        }
                    }
                }
                else if (li.Span != null)
                {
                    result.Title = li.Span.Value;
                }

                if (li.Ol != null && !li.Ol.IsHidden && li.Ol.Items.Any())
                {
                    foreach (var liItem in li.Ol.Items)
                    {
                        result.Chapters.Add(GetChapterFromNavLi(liItem, opfDirectory));
                    }
                }
            }
            return result;
        }

        private static Chapter GetChapterFromNavPoint(NavPoint navPoint, string opfDirectory)
        {
            Chapter result = null;
            if (navPoint != null)
            {
                result = new Chapter { Title = navPoint.Label?.Text };
                if (!String.IsNullOrWhiteSpace(navPoint.Content?.Source))
                {
                    var hrefSplit = navPoint.Content.Source.Split(new char[] { HREF_SEPARATOR });
                    result.ContentFilePath = PathHelper.Combine(opfDirectory, hrefSplit[0]);
                    if (hrefSplit.Length > 1)
                    {
                        result.SecondPath = hrefSplit[1];
                    }
                }
                if (navPoint.Children != null)
                {
                    foreach (var chid in navPoint.Children)
                    {
                        result.Chapters.Add(GetChapterFromNavPoint(chid, opfDirectory));
                    }
                }
            }
            return result;
        }

        #endregion

        #region ReadingFiles

        private static void FillReadingFiles(IList<ContentFile> readingFiles, Package package, string opfDirectory)
        {
            if (readingFiles != null && package != null && package.Spine != null && package.Manifest != null)
            {
                foreach (var spineItem in package.Spine.Items)
                {
                    var manifestItem = package.Manifest.Items.FirstOrDefault(p => p.Id == spineItem.IdRef);
                    if (manifestItem != null)
                    {
                        readingFiles.Add(new ContentFile(manifestItem.MediaType, PathHelper.Combine(opfDirectory, manifestItem.Href)));
                    }
                }
            }
        }

        #endregion

        #region AllFiles

        private static void FillAllFiles(IList<ContentFile> allFiles, Package package, string opfDirectory)
        {
            if (allFiles != null && package != null && package.Manifest != null)
            {
                foreach (var manifestItem in package.Manifest.Items)
                {
                    allFiles.Add(new ContentFile(manifestItem.MediaType, PathHelper.Combine(opfDirectory, manifestItem.Href)));
                }
            }
        }

        #endregion

        #region Disposable

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                this._archive.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }
}
