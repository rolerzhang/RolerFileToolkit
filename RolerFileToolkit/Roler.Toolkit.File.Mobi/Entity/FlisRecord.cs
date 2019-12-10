namespace Roler.Toolkit.File.Mobi.Entity
{
    public class FlisRecord
    {
        /// <summary>
        /// Gets or sets the identifier, always 'FLIS'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the value0, fixed value: 8.
        /// </summary>
        public uint Value0 { get; set; }

        /// <summary>
        /// Gets or sets the value1, fixed value: 65.
        /// </summary>
        public ushort Value1 { get; set; }

        /// <summary>
        /// Gets or sets the value2, fixed value: 0.
        /// </summary>
        public ushort Value2 { get; set; }

        /// <summary>
        /// Gets or sets the value3, fixed value: 0.
        /// </summary>
        public uint Value3 { get; set; }

        /// <summary>
        /// Gets or sets the value4, fixed value: -1 (0xFFFFFFFF).
        /// </summary>
        public uint Value4 { get; set; }

        /// <summary>
        /// Gets or sets the value5, fixed value: 1.
        /// </summary>
        public ushort Value5 { get; set; }

        /// <summary>
        /// Gets or sets the value6, fixed value: 3.
        /// </summary>
        public ushort Value6 { get; set; }

        /// <summary>
        /// Gets or sets the value7, fixed value: 3.
        /// </summary>
        public uint Value7 { get; set; }

        /// <summary>
        /// Gets or sets the value8, fixed value: 1.
        /// </summary>
        public uint Value8 { get; set; }

        /// <summary>
        /// Gets or sets the value9, fixed value: -1 (0xFFFFFFFF).
        /// </summary>
        public uint Value9 { get; set; }
    }
}
