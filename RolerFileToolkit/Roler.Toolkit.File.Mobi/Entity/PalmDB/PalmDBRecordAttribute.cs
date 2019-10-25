namespace Roler.Toolkit.File.Mobi.Entity
{
    public enum PalmDBRecordAttribute : byte
    {
        Unkown,

        /// <summary>
        /// Secret record bit. 
        /// </summary>
        Secret = 0x10,

        /// <summary>
        /// Record in use (busy bit). 
        /// </summary>
        InUse = 0x20,

        /// <summary>
        /// Dirty record bit.
        /// </summary>
        Dirty = 0x40,

        /// <summary>
        /// Delete record on next HotSync. 
        /// </summary>
        Delete = 0x80,
    }
}
