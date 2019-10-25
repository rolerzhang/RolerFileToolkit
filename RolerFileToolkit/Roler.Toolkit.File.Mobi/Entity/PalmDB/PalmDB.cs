using System;
using System.Collections.Generic;
using System.Text;

namespace Roler.Toolkit.File.Mobi.Entity
{
    public class PalmDB
    {
        /// <summary>
        /// Gets or sets the name of PDB. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets attributes of PDB. 
        /// </summary>
        public PalmDBAttribute Attribute { get; set; }

        public ushort Version { get; set; }

        /// <summary>
        /// See above table. (For Applications this data will be 'appl') 
        /// </summary>
        public uint Type { get; set; }

        /// <summary>
        /// See above table. This program will be launched if the file is tapped.
        /// </summary>
        public uint Creator { get; set; }

        /// <summary>
        /// used internally to identify record.
        /// </summary>
        public uint UniqueIDseed { get; set; }

        /// <summary>
        /// Only used when in-memory on Palm OS. Always set to zero in stored files. 
        /// </summary>
        public uint NextRecordListID { get; set; }

        /// <summary>
        /// Gets or sets the number of records in the file.
        /// </summary>
        public ushort RecordCount { get; set; }

        public IList<PalmDBRecordInfo> RecordInfoList { get; } = new List<PalmDBRecordInfo>();
    }
}
