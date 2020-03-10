using System;
using System.Linq;
using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub
{
    internal static class ExtendMethod
    {
        private const string XML_1_0_START = "<?xml version=\"1.0\"";
        private const string XML_1_1_START = "<?xml version=\"1.1\"";

        public static bool IsPropertiesContains(this ManifestItem manifestItem, string value)
        {
            return !String.IsNullOrWhiteSpace(manifestItem.Properties) && manifestItem.Properties.Split(' ').Contains(value);
        }

        public static string FixXml(this string originStr)
        {
            return originStr.StartsWith(XML_1_1_START) ?
                XML_1_0_START + originStr.Substring(XML_1_1_START.Length) :
                originStr;
        }

    }
}
