using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Roler.Toolkit.File.Epub.Define;
using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub.Engine
{
    internal static class NavEngine
    {
        #region Define

        public const string ELEMENT_NAV = "nav";
        public const string ELEMENT_H1 = "h1";
        public const string ELEMENT_H2 = "h2";
        public const string ELEMENT_H3 = "h3";
        public const string ELEMENT_H4 = "h4";
        public const string ELEMENT_H5 = "h5";
        public const string ELEMENT_H6 = "h6";
        public const string ELEMENT_OL = "ol";
        public const string ELEMENT_LI = "li";
        public const string ELEMENT_A = "a";
        public const string ELEMENT_SPAN = "span";

        public static readonly XName ATTRIBUTE_TYPE = Const.EPUB_NAMESPACE + "type";
        public const string ATTRIBUTE_ID = "id";
        public const string ATTRIBUTE_HIDDEN = "hidden";
        public const string ATTRIBUTE_HREF = "href";
        public const string ATTRIBUTE_TITLE = "title";

        private static readonly IReadOnlyList<string> NAV_HEAD_LIST = new List<string> { ELEMENT_H1, ELEMENT_H2, ELEMENT_H3, ELEMENT_H4, ELEMENT_H5, ELEMENT_H6 };

        #endregion

        public static Nav Read(Stream stream)
        {
            Nav result = null;
            using (var streamReader = new StreamReader(stream))
            {
                string xmlStr = streamReader.ReadToEnd();
                var document = XElement.Parse(xmlStr);

                var xNamespace = document.GetDefaultNamespace();
                result = ParseNav(document) ?? throw new InvalidDataException("invalid data of nav file: nav");
            }
            return result;
        }

        public static void Write(Nav file, Stream stream)
        {
        }

        private static Nav ParseNav(XElement element)
        {
            Nav result = null;

            if (element != null)
            {
                result = new Nav();

                var xNamespace = element.GetDefaultNamespace();
                foreach (var nav in element.Descendants(xNamespace + ELEMENT_NAV))
                {
                    var navElement = ParseNavElement(nav);
                    if (navElement != null)
                    {
                        result.NavElements.Add(navElement);
                    }
                }
            }

            return result;
        }

        private static NavElement ParseNavElement(XElement element)
        {
            NavElement result = null;

            if (element != null && element.Name.LocalName == ELEMENT_NAV && element.Attribute(ATTRIBUTE_TYPE) != null)
            {
                var xNamespace = element.GetDefaultNamespace();

                result = new NavElement
                {
                    Type = element.Attribute(ATTRIBUTE_TYPE)?.Value,
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                    IsHidden = element.Attribute(ATTRIBUTE_HIDDEN) != null,
                    Ol = ParseNavOl(element.Element(xNamespace + ELEMENT_OL)),
                };

                foreach (var head in NAV_HEAD_LIST)
                {
                    var headELement = element.Element(xNamespace + head);
                    if (headELement != null)
                    {
                        result.NavHead = new NavHead
                        {
                            Name = head,
                            Value = headELement.Value,
                        };
                        break;
                    }
                }

            }

            return result;
        }

        private static NavOl ParseNavOl(XElement element)
        {
            NavOl result = null;

            if (element != null && element.Name.LocalName == ELEMENT_OL)
            {
                var xNamespace = element.GetDefaultNamespace();

                result = new NavOl
                {
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                    IsHidden = element.Attribute(ATTRIBUTE_HIDDEN) != null,
                };

                foreach (var liElement in element.Elements(xNamespace + ELEMENT_LI))
                {
                    var navLi = ParseNavLi(liElement);
                    if (navLi != null)
                    {
                        result.Items.Add(navLi);
                    }
                }
            }

            return result;
        }

        private static NavLi ParseNavLi(XElement element)
        {
            NavLi result = null;

            if (element != null && element.Name.LocalName == ELEMENT_LI)
            {
                var xNamespace = element.GetDefaultNamespace();

                result = new NavLi
                {
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                    IsHidden = element.Attribute(ATTRIBUTE_HIDDEN) != null,
                    Ol = ParseNavOl(element.Element(xNamespace + ELEMENT_OL)),
                };

                var aElement = element.Element(xNamespace + ELEMENT_A);
                if (aElement != null)
                {
                    result.A = new NavA
                    {
                        Href = aElement.Attribute(ATTRIBUTE_HREF)?.Value,
                        Title = aElement.Attribute(ATTRIBUTE_TITLE)?.Value,
                        Value = aElement.Value,
                    };
                }
                var spanElement = element.Element(xNamespace + ELEMENT_SPAN);
                if (spanElement != null)
                {
                    result.Span = new NavSpan
                    {
                        Value = spanElement.Value,
                    };
                }
            }

            return result;
        }
    }
}
