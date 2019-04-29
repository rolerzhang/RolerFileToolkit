using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Roler.Toolkit.File.Epub.Define;
using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub.Engine
{
    internal static class NcxEngine
    {
        #region Define

        public const string ELEMENT_NCX = "ncx";
        public const string ELEMENT_NAVMAP = "navMap";
        public const string ELEMENT_NAVINFO = "navInfo";
        public const string ELEMENT_TEXT = "text";
        public const string ELEMENT_NAVPOINT = "navPoint";
        public const string ELEMENT_NAVLABEL = "navLabel";
        public const string ELEMENT_CONTENT = "content";

        public const string ATTRIBUTE_VERSION = "version";
        public const string ATTRIBUTE_ID = "id";
        public const string ATTRIBUTE_PLAYORDER = "playOrder";
        public const string ATTRIBUTE_SRC = "src";

        #endregion

        public static Ncx Read(Stream stream)
        {
            Ncx result = null;
            using (var streamReader = new StreamReader(stream))
            {
                string xmlStr = streamReader.ReadToEnd();
                var document = XElement.Parse(xmlStr);

                var xNamespace = document.GetDefaultNamespace();
                result = ParseNcx(document) ?? throw new InvalidDataException("invalid data of ncx file: ncx");
            }
            return result;
        }

        public static void Write(Ncx file, Stream stream)
        {
        }

        private static Ncx ParseNcx(XElement element)
        {
            Ncx result = null;

            if (element != null && element.Name.LocalName == ELEMENT_NCX)
            {
                var xNamespace = element.Name.Namespace;

                result = new Ncx
                {
                    Version = element.Attribute(ATTRIBUTE_VERSION)?.Value,
                    Language = element.Attribute(Const.ATTRIBUTE_LANGUAGE)?.Value,
                    NavMap = ParseNavMap(element.Element(xNamespace + ELEMENT_NAVMAP)),
                };
            }

            return result;
        }

        private static NavMap ParseNavMap(XElement element)
        {
            NavMap result = null;

            if (element != null && element.Name.LocalName == ELEMENT_NAVMAP)
            {
                var xNamespace = element.Name.Namespace;
                result = new NavMap();
                var infoText = element.Element(xNamespace + ELEMENT_NAVINFO)?.Element(xNamespace + ELEMENT_TEXT).Value;
                if (!String.IsNullOrWhiteSpace(infoText))
                {
                    result.NavInfo = new NavInfo { Text = infoText };
                }
                result.NavPoints = ParseNavPointList(element.Elements(xNamespace + ELEMENT_NAVPOINT), xNamespace);
            }

            return result;
        }

        private static IList<NavPoint> ParseNavPointList(IEnumerable<XElement> elements, XNamespace xNamespace)
        {
            IList<NavPoint> result = null;
            if (elements != null && elements.Any())
            {
                var navPoints = new List<NavPoint>();
                var data = from element in elements
                           select new NavPoint
                           {
                               Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                               PlayOrder = element.Attribute(ATTRIBUTE_PLAYORDER)?.Value,
                               Label = ParseNavPointLabel(element.Element(xNamespace + ELEMENT_NAVLABEL)),
                               Content = ParseNavPointContent(element.Element(xNamespace + ELEMENT_CONTENT)),
                               Children = ParseNavPointList(element.Elements(xNamespace + ELEMENT_NAVPOINT), xNamespace),
                           };
                navPoints.AddRange(data);
                result = navPoints;
            }
            return result;
        }

        private static NavPointLabel ParseNavPointLabel(XElement element)
        {
            NavPointLabel result = null;
            if (element != null && element.Name.LocalName == ELEMENT_NAVLABEL)
            {
                var xNamespace = element.Name.Namespace;
                var text = element.Element(xNamespace + ELEMENT_TEXT).Value;
                if (!String.IsNullOrWhiteSpace(text))
                {
                    result = new NavPointLabel { Text = text };
                }
            }
            return result;
        }

        private static NavPointContent ParseNavPointContent(XElement element)
        {
            NavPointContent result = null;
            if (element != null && element.Name.LocalName == ELEMENT_CONTENT)
            {
                result = new NavPointContent
                {
                    Id = element.Attribute(ATTRIBUTE_ID)?.Value,
                    Source = element.Attribute(ATTRIBUTE_SRC)?.Value,
                };
            }
            return result;
        }

    }
}
