using System.Xml.Linq;

namespace Roler.Toolkit.File.Epub.Define
{
    internal class Const
    {
        public const string CONTAINER_PATH = @"META-INF/container.xml";

        public readonly static XName ATTRIBUTE_LANGUAGE = XNamespace.Xml + "lang";
        public readonly static XNamespace EPUB_NAMESPACE = @"http://www.idpf.org/2007/ops";
    }
}
