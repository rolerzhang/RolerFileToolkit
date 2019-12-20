using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi
{
    internal class PalmDBRecord
    {
        public PalmDBRecordInfo Info { get; }
        public int Length { get; set; }

        public PalmDBRecord(PalmDBRecordInfo info)
        {
            this.Info = info;
        }
    }
}