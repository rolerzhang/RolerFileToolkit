using System.IO;
using System.Xml.Linq;
using Roler.Toolkit.File.Epub.Define;
using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub.Engine
{
    internal static class OpfEngine
    {
        #region Define

        public const string NAMESPACE_DC = "http://purl.org/dc/elements/1.1/";

        public const string ELEMENT_PACKAGE = "package";
        public const string ELEMENT_METADATA = "metadata";
        public const string ELEMENT_META = "meta";
        public const string ELEMENT_LINK = "link";
        public const string ELEMENT_MANIFEST = "manifest";
        public const string ELEMENT_ITEM = "item";
        public const string ELEMENT_SPINE = "spine";
        public const string ELEMENT_ITEMREF = "itemref";

        public const string DC_ELEMENT_CONTRIBUTOR = "contributor";
        public const string DC_ELEMENT_COVERAGE = "coverage";
        public const string DC_ELEMENT_CREATOR = "creator";
        public const string DC_ELEMENT_DATE = "date";
        public const string DC_ELEMENT_DESCRIPTION = "description";
        public const string DC_ELEMENT_FORMAT = "format";
        public const string DC_ELEMENT_IDENTIFIER = "identifier";
        public const string DC_ELEMENT_LANGUAGE = "language";
        public const string DC_ELEMENT_PUBLISHER = "publisher";
        public const string DC_ELEMENT_RELATION = "relation";
        public const string DC_ELEMENT_RIGHTS = "rights";
        public const string DC_ELEMENT_SOURCE = "source";
        public const string DC_ELEMENT_SUBJECT = "subject";
        public const string DC_ELEMENT_TITLE = "title";
        public const string DC_ELEMENT_TYPE = "type";

        public const string ATTRIBUTE_VERSION = "version";
        public const string ATTRIBUTE_IDENTIFIER = "unique-identifier";
        public const string ATTRIBUTE_PREFIX = "prefix";
        public const string ATTRIBUTE_DIR = "dir";
        public const string ATTRIBUTE_ID = "id";
        public const string ATTRIBUTE_SCHEME = "scheme";
        public const string ATTRIBUTE_PROPERTY = "property";
        public const string ATTRIBUTE_REFINES = "refines";
        public const string ATTRIBUTE_NAME = "name";
        public const string ATTRIBUTE_CONTENT = "content";
        public const string ATTRIBUTE_HREF = "href";
        public const string ATTRIBUTE_REL = "rel";
        public const string ATTRIBUTE_MEDIATYPE = "media-type";
        public const string ATTRIBUTE_FALLBACK = "fallback";
        public const string ATTRIBUTE_PROPERTIES = "properties";
        public const string ATTRIBUTE_MEDIAOVERLAY = "media-overlay";
        public const string ATTRIBUTE_TOC = "toc";
        public const string ATTRIBUTE_PAGEPROGRESSIONDIRECTION = "page-progression-direction";
        public const string ATTRIBUTE_IDREF = "idref";
        public const string ATTRIBUTE_LINEAR = "linear";

        #endregion

        public static Package Read(Stream stream)
        {
            Package result = null;
            using (var streamReader = new StreamReader(stream))
            {
                string xmlStr = streamReader.ReadToEnd();
                var document = XElement.Parse(xmlStr.FixXml());

                var xNamespace = document.GetDefaultNamespace();
                result = ParsePackage(document) ?? throw new InvalidDataException("invalid data of opf file: package");

                var metadataElement = document.Element(xNamespace + ELEMENT_METADATA);
                var metadata = ParseMetadata(metadataElement);
                result.Metadata = metadata ?? throw new InvalidDataException("invalid data of opf file: metadata");

                var manifestElement = document.Element(xNamespace + ELEMENT_MANIFEST);
                var manifest = ParseManifest(manifestElement);
                result.Manifest = manifest ?? throw new InvalidDataException("invalid data of opf file: manifest");

                var spineElement = document.Element(xNamespace + ELEMENT_SPINE);
                var spine = ParseSpine(spineElement);
                result.Spine = spine ?? throw new InvalidDataException("invalid data of opf file: spine");
            }
            return result;
        }

        public static void Write(Container file, Stream stream)
        {
        }

        private static Package ParsePackage(XElement element)
        {
            Package result = null;

            if (element != null)
            {
                result = new Package
                {
                    Identifier = element.Attribute(ATTRIBUTE_IDENTIFIER)?.Value,
                    Prefix = element.Attribute(ATTRIBUTE_PREFIX)?.Value,
                    Language = element.Attribute(Const.ATTRIBUTE_LANGUAGE)?.Value,
                    Dir = element.Attribute(ATTRIBUTE_DIR)?.Value,
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                };

                if (float.TryParse(element.Attribute(ATTRIBUTE_VERSION)?.Value, out float version))
                {
                    result.Version = version;
                }
            }

            return result;
        }

        private static Metadata ParseMetadata(XElement element)
        {
            Metadata result = null;

            if (element != null && element.Name.LocalName == ELEMENT_METADATA)
            {
                result = new Metadata();
                foreach (var childElement in element.Elements())
                {
                    if (childElement.Name.Namespace == NAMESPACE_DC)
                    {
                        var dcElement = new DcElement
                        {
                            Id = childElement.Attribute(ATTRIBUTE_ID)?.Value,
                            Language = childElement.Attribute(Const.ATTRIBUTE_LANGUAGE)?.Value,
                            Dir = childElement.Attribute(ATTRIBUTE_DIR)?.Value,
                            Value = childElement.Value
                        };
                        switch (childElement.Name.LocalName)
                        {
                            case DC_ELEMENT_CONTRIBUTOR: result.Contributors.Add(dcElement); break;
                            case DC_ELEMENT_COVERAGE: result.Coverages.Add(dcElement); break;
                            case DC_ELEMENT_CREATOR: result.Creators.Add(dcElement); break;
                            case DC_ELEMENT_DATE: result.Date = dcElement; break;
                            case DC_ELEMENT_DESCRIPTION: result.Descriptions.Add(dcElement); break;
                            case DC_ELEMENT_FORMAT: result.Formats.Add(dcElement); break;
                            case DC_ELEMENT_IDENTIFIER: result.Identifiers.Add(dcElement); break;
                            case DC_ELEMENT_LANGUAGE: result.Languages.Add(dcElement); break;
                            case DC_ELEMENT_PUBLISHER: result.Publishers.Add(dcElement); break;
                            case DC_ELEMENT_RELATION: result.Relations.Add(dcElement); break;
                            case DC_ELEMENT_RIGHTS: result.Rights.Add(dcElement); break;
                            case DC_ELEMENT_SOURCE: result.Sources.Add(dcElement); break;
                            case DC_ELEMENT_SUBJECT: result.Subjects.Add(dcElement); break;
                            case DC_ELEMENT_TITLE: result.Titles.Add(dcElement); break;
                            case DC_ELEMENT_TYPE: result.Types.Add(dcElement); break;
                            default: break;
                        }
                    }
                    else
                    {
                        switch (childElement.Name.LocalName)
                        {
                            case ELEMENT_META:
                                {
                                    var metaElement = new MetaElement
                                    {
                                        Id = childElement.Attribute(ATTRIBUTE_ID)?.Value,
                                        Language = childElement.Attribute(Const.ATTRIBUTE_LANGUAGE)?.Value,
                                        Dir = childElement.Attribute(ATTRIBUTE_DIR)?.Value,
                                        Scheme = childElement.Attribute(ATTRIBUTE_SCHEME)?.Value,
                                        Property = childElement.Attribute(ATTRIBUTE_PROPERTY)?.Value,
                                        Refines = childElement.Attribute(ATTRIBUTE_REFINES)?.Value,
                                        Value = childElement.Value,
                                        Name = childElement.Attribute(ATTRIBUTE_NAME)?.Value,
                                        Content = childElement.Attribute(ATTRIBUTE_CONTENT)?.Value,
                                    };
                                    result.Metas.Add(metaElement);
                                }
                                break;
                            case ELEMENT_LINK:
                                {
                                    var linkElement = new LinkElement
                                    {
                                        Href = childElement.Attribute(ATTRIBUTE_HREF)?.Value,
                                        Rel = childElement.Attribute(ATTRIBUTE_REL)?.Value,
                                        Id = childElement.Attribute(ATTRIBUTE_ID)?.Value,
                                        Refines = childElement.Attribute(ATTRIBUTE_REFINES)?.Value,
                                        MediaType = childElement.Attribute(ATTRIBUTE_MEDIATYPE)?.Value,
                                    };
                                    result.Links.Add(linkElement);
                                }
                                break;
                            default: break;
                        }
                    }
                }
            }

            return result;
        }

        private static Manifest ParseManifest(XElement element)
        {
            Manifest result = null;

            if (element != null && element.Name.LocalName == ELEMENT_MANIFEST)
            {
                result = new Manifest
                {
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value
                };
                foreach (var childElement in element.Elements())
                {
                    if (childElement.Name.LocalName == ELEMENT_ITEM)
                    {
                        var manifestItem = new ManifestItem
                        {
                            Id = childElement.Attribute(ATTRIBUTE_ID)?.Value,
                            Href = childElement.Attribute(ATTRIBUTE_HREF)?.Value,
                            MediaType = childElement.Attribute(ATTRIBUTE_MEDIATYPE)?.Value,
                            Fallback = childElement.Attribute(ATTRIBUTE_FALLBACK)?.Value,
                            Properties = childElement.Attribute(ATTRIBUTE_PROPERTIES)?.Value,
                            MediaOverlay = childElement.Attribute(ATTRIBUTE_MEDIAOVERLAY)?.Value,
                        };
                        result.Items.Add(manifestItem);
                    }
                }
            }
            return result;
        }

        private static Spine ParseSpine(XElement element)
        {
            Spine result = null;

            if (element != null && element.Name.LocalName == ELEMENT_SPINE)
            {
                result = new Spine
                {
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                    Toc = element.Attribute(ATTRIBUTE_TOC)?.Value,
                    PageProgressionDirection = element.Attribute(ATTRIBUTE_PAGEPROGRESSIONDIRECTION)?.Value,
                };
                foreach (var childElement in element.Elements())
                {
                    if (childElement.Name.LocalName == ELEMENT_ITEMREF)
                    {
                        var spineItemRef = new SpineItemRef
                        {
                            IdRef = childElement.Attribute(ATTRIBUTE_IDREF)?.Value,
                            Linear = childElement.Attribute(ATTRIBUTE_LINEAR)?.Value,
                            Id = childElement.Attribute(ATTRIBUTE_ID)?.Value,
                            Properties = childElement.Attribute(ATTRIBUTE_PROPERTIES)?.Value,
                        };
                        result.Items.Add(spineItemRef);
                    }
                }
            }
            return result;
        }
    }
}
