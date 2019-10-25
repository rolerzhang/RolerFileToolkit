namespace Roler.Toolkit.File.Mobi.Entity
{
    public enum PalmDBAttribute : ushort
    {
        Unkown,

        /// <summary>
        /// Read-Only.
        /// </summary>
        ReadOnly = 0x0002,

        /// <summary>
        /// Dirty AppInfoArea.
        /// </summary>
        DirtyAppInfoArea = 0x0004,

        /// <summary>
        /// Backup this database.
        /// </summary>
        Backup = 0x0008,

        /// <summary>
        /// Okay to install newer over existing copy, if present on PalmPilot.
        /// </summary>
        OverInstall = 0x0010,

        /// <summary>
        /// Force the PalmPilot to reset after this database is installed.
        /// </summary>
        ResetAfterInstall = 0x0020,

        /// <summary>
        /// Don't allow copy of file to be beamed to other Pilot.
        /// </summary>
        DisallowCopy = 0x0040,
    }
}
