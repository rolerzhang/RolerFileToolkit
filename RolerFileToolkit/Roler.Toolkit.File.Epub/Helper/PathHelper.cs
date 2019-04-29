using System.IO;

namespace Roler.Toolkit.File.Epub.Helper
{
    internal static class PathHelper
    {
        public static string Combine(params string[] paths)
        {
            var path = Path.Combine(paths);
            if (path != null)
            {
                path = path.Replace(@"\", "/");
            }
            return path;
        }
    }
}
