namespace Roler.Toolkit.File.Mobi.Entity
{
    public enum ExthRecordType : uint
    {
        Unkown,
        DRMServerId = 1,
        DRMCommerceId = 2,
        DRMEbookbaseBookId = 3,
        Author = 100,
        Publisher,
        Imprint,
        Description,
        ISBN,
        Subject,
        PublishingDate,
        Review,
        Contributor,
        Rights,
        SubjectCode,
        Type,
        Source,
        ASIN,
        VersionNumber,
        Sample,
        StartReading,
        Adult,
        RetailPrice,
        RetailPriceCurrency,
        KF8BoundaryOffset = 121,
        ResourceCount = 125,
        KF8CoverUri = 129,
        Unknown1 = 131,
        DictionaryShortName = 200,
        CoverOffset,
        thumbOffset,
        HasFakeCover,
        CreatorSoftware,
        CreatorMajorVersion,
        CreatorMinorVersion,
        CreatorBuildVersion,
        Watermark,
        TamperProofKeys,
        FontSignature = 300,
        ClippingLimit = 401,
        PublisherLimit,
        Unknown2,

        /// <summary>
        /// 1 - Text to Speech disabled; 0 - Text to Speech enabled
        /// </summary>
        TTSFlag,

        Unknown3,
        RentBorrowExpirationDate,
        Unknown4 = 407,
        Unknown5 = 450,
        Unknown6,
        Unknown7,
        Unknown8,

        /// <summary>
        /// PDOC - Personal Doc; EBOK - ebook; EBSP - ebook sample; 
        /// </summary>
        CdeType = 501,

        LastUpdateTime,
        UpdatedTitle,
        ASIN_Copy,
        Language = 524,
        WritingMode,
        CreatorBuildNumber = 535,
        Unknown9,
        Unknown10 = 542,
        InMemory = 547,
    }
}
