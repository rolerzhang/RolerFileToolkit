using Roler.Toolkit.File.Epub.Entity;

namespace Roler.Toolkit.File.Epub
{
    public class Structure
    {
        public Container Container { get; set; }
        public Package Package { get; set; }

        /// <summary>
        /// 获取或设置ncx相关信息，仅在Epub3.0之前有效，3.0以后请使用Nav属性。
        /// </summary>
        public Ncx Ncx { get; set; }

        /// <summary>
        /// 获取或设置导航文件信息，仅在Epub3.0之后有效，3.0之前请使用Ncx属性。
        /// </summary>
        public Nav Nav { get; set; }
    }
}
