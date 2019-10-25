using System;
using System.Collections.Generic;
using System.Text;

namespace Roler.Toolkit.File.Mobi.Entity
{
    public class PalmDBRecordInfo
    {
        /// <summary>
        /// Gets or sets the offset of record n from the start of the PDB of this record.
        /// </summary>
        public uint Offset { get; set; }

        public PalmDBRecordAttribute Attribute { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for this record. Often just a sequential count from 0.
        /// </summary>
        public uint UniqueID { get; set; }
    }
}
