using System;

namespace Roler.Toolkit.File.Mobi.Entity
{
    public class MobiHeader
    {
        /// <summary>
        /// Gets or sets the identifier, always 'MOBI'.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the length of the MOBI header, including the previous 4 bytes.
        /// </summary>
        public uint Length { get; set; }

        public MobiType MobiType { get; set; }

        public TextEncoding TextEncoding { get; set; }

        /// <summary>
        /// Some kind of unique ID number.
        /// </summary>
        public uint UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the version of the Mobipocket format used in this file. 
        /// </summary>
        public uint FileVersion { get; set; }

        /// <summary>
        /// Gets or sets the section number of orthographic meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint OrtographicIndex { get; set; }

        /// <summary>
        /// Gets or sets the section number of inflection meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint InflectionIndex { get; set; }

        public uint IndexNames { get; set; }

        public uint IndexKeys { get; set; }

        /// <summary>
        /// Gets or sets the section number of extra 0 meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint ExtraIndex0 { get; set; }

        /// <summary>
        /// Gets or sets the section number of extra 1 meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint ExtraIndex1 { get; set; }

        /// <summary>
        /// Gets or sets the section number of extra 2 meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint ExtraIndex2 { get; set; }

        /// <summary>
        /// Gets or sets the section number of extra 3 meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint ExtraIndex3 { get; set; }

        /// <summary>
        /// Gets or sets the section number of extra 4 meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint ExtraIndex4 { get; set; }

        /// <summary>
        /// Gets or sets the section number of extra 5 meta index. 0xFFFFFFFF if index is not available. 
        /// </summary>
        public uint ExtraIndex5 { get; set; }

        /// <summary>
        /// Gets or sets the first record number (starting with 0) that's not the book's text.
        /// </summary>
        public uint FirstNonBookIndex { get; set; }

        /// <summary>
        /// Gets or sets the offset in record 0 (not from start of file) of the full name of the book.
        /// </summary>
        public uint FullNameOffset { get; set; }

        /// <summary>
        /// Gets or sets the length in bytes of the full name of the book.
        /// </summary>
        public uint FullNameLength { get; set; }

        /// <summary>
        /// Gets or sets the book locale code. Low byte is main language 09= English, next byte is dialect, 08 = British, 04 = US. Thus US English is 1033, UK English is 2057. 
        /// </summary>
        public uint Locale { get; set; }

        /// <summary>
        /// Gets or sets the input language for a dictionary.
        /// </summary>
        public string InputLanguage { get; set; }

        /// <summary>
        /// Gets or sets the output language for a dictionary.
        /// </summary>
        public string OutputLanguage { get; set; }

        /// <summary>
        /// Gets or sets the minimum mobipocket version support needed to read this file.
        /// </summary>
        public uint MinVersion { get; set; }

        /// <summary>
        /// Gets or sets the first record number (starting with 0) that contains an image. Image records should be sequential.
        /// </summary>
        public uint FirstImageIndex { get; set; }

        /// <summary>
        /// Gets or sets the record number of the first huffman compression record.
        /// </summary>
        public uint HuffmanRecordOffset { get; set; }

        /// <summary>
        /// Gets or sets the number of huffman compression records.
        /// </summary>
        public uint HuffmanRecordCount { get; set; }

        public uint HuffmanTableOffset { get; set; }

        public uint HuffmanTableLength { get; set; }

        public uint EXTHFlags { get; set; }

        /// <summary>
        /// Gets or sets the offset to DRM key info in DRMed files. 0xFFFFFFFF if no DRM.
        /// </summary>
        public uint DRMOffset { get; set; }

        /// <summary>
        /// Gets or sets the number of entries in DRM info. 0xFFFFFFFF if no DRM.
        /// </summary>
        public uint DRMCount { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes in DRM info. 
        /// </summary>
        public uint DRMSize { get; set; }

        /// <summary>
        /// Gets or sets some flags concerning the DRM info.
        /// </summary>
        public uint DRMFlags { get; set; }

        /// <summary>
        /// Gets or sets the number of first text record. Normally 1.
        /// </summary>
        public ushort FirstContentRecordNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of last image record or number of last text record if it contains no images. Includes Image, DATP, HUFF, DRM. .
        /// </summary>
        public ushort LastContentRecordNumber { get; set; }

        public uint FCISRecordNumber { get; set; }

        public uint FLISRecordNumber { get; set; }

        public uint ExtraRecordDataFlags { get; set; }

        /// <summary>
        /// Gets or sets the record number of the first INDX record created from an ncx file.
        /// </summary>
        public uint INDXRecordOffset { get; set; }
    }
}
