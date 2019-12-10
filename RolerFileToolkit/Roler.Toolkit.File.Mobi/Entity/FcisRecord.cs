namespace Roler.Toolkit.File.Mobi.Entity
{
    public class FcisRecord
    {
        /// <summary>
        /// Gets or sets the identifier, always 'FCIS'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the value0, fixed value: 20.
        /// </summary>
        public uint Value0 { get; set; }

        /// <summary>
        /// Gets or sets the value1, fixed value: 16.
        /// </summary>
        public uint Value1 { get; set; }

        /// <summary>
        /// Gets or sets the value2, fixed value: 1.
        /// </summary>
        public uint Value2 { get; set; }

        /// <summary>
        /// Gets or sets the value3, fixed value: 0.
        /// </summary>
        public uint Value3 { get; set; }

        /// <summary>
        /// Gets or sets the value4, text length (the same value as "text length" in the PalmDoc header).
        /// </summary>
        public uint Value4 { get; set; }

        /// <summary>
        /// Gets or sets the value5, fixed value: 0.
        /// </summary>
        public uint Value5 { get; set; }

        /// <summary>
        /// Gets or sets the value6, fixed value: 32.
        /// </summary>
        public uint Value6 { get; set; }

        /// <summary>
        /// Gets or sets the value7, fixed value: 8.
        /// </summary>
        public uint Value7 { get; set; }

        /// <summary>
        /// Gets or sets the value8, fixed value: 1.
        /// </summary>
        public ushort Value8 { get; set; }

        /// <summary>
        /// Gets or sets the value9, fixed value: 1.
        /// </summary>
        public ushort Value9 { get; set; }

        /// <summary>
        /// Gets or sets the value10, fixed value: 0.
        /// </summary>
        public uint Value10 { get; set; }
    }
}
