using System;
using System.Linq;
using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub
{
    internal static class ExtendMethod
    {
        public static bool IsPropertiesContains(this ManifestItem manifestItem, string value)
        {
            return !String.IsNullOrWhiteSpace(manifestItem.Properties) && manifestItem.Properties.Split(' ').Contains(value);
        }
    }
}
