using System.IO;
using System.Linq;
using System.Xml.Linq;
using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub.Engine
{
    internal static class ContainerEngine
    {
        #region Const String

        private const string FULLPATH = "full-path";
        private const string MEDIATYPE = "media-type";
        private const string ROOTFILE = "rootfile";

        #endregion

        public static Container Read(Stream stream)
        {
            Container result = null;
            using (var streamReader = new StreamReader(stream))
            {
                string xmlStr = streamReader.ReadToEnd();
                var document = XElement.Parse(xmlStr);

                var xNamespace = document.GetDefaultNamespace();
                result = new Container
                {
                    Namespace = xNamespace.NamespaceName
                };

                var xElement = document.Descendants(xNamespace + ROOTFILE).FirstOrDefault();
                if (xElement != null)
                {
                    result.FullPath = xElement.Attribute(FULLPATH).Value;
                    result.MediaType = xElement.Attribute(MEDIATYPE).Value;
                }
            }
            return result;
        }

        public static void Write(Container file, Stream stream)
        {
        }

    }
}
