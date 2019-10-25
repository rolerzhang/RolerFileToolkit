namespace Roler.Toolkit.File.Mobi.Entity
{
    public enum EncryptionType : ushort
    {
        /// <summary>
        /// No encryption.
        /// </summary>
        None = 0,

        /// <summary>
        /// Old Mobipocket Encryption.
        /// </summary>
        OldMobi = 1,

        /// <summary>
        /// Mobipocket Encryption.
        /// </summary>
        Mobi = 2,
    }
}
