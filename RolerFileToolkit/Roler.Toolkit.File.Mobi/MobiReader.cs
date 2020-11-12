using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Roler.Toolkit.File.Mobi.Compression;
using Roler.Toolkit.File.Mobi.Engine;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi
{
    public class MobiReader : IDisposable
    {
        private bool _disposed;
        private readonly Stream _stream;
        private readonly IList<PalmDBRecord> _palmDBRecordList = new List<PalmDBRecord>();

        public MobiReader(Stream stream)
        {
            this._stream = stream;
        }

        #region Methods

        public Mobi ReadWithoutText()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("stream");
            }

            var structure = this.ReadStructure();
            var result = new Mobi
            {
                Structure = structure,
                Title = structure.FullName,
            };

            if (structure.ExthHeader != null)
            {
                var exthHeader = structure.ExthHeader;
                result.Creator = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Author)?.DataAsString();
                result.Publisher = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Publisher)?.DataAsString();
                result.Description = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Description)?.DataAsString();
                result.Subject = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Subject)?.DataAsString();
                result.Date = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.PublishingDate)?.DataAsString();
                result.Contributor = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Contributor)?.DataAsString();
                result.Rights = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Rights)?.DataAsString();
                result.Type = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Type)?.DataAsString();
                result.Source = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Source)?.DataAsString();
                result.Language = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.Language)?.DataAsString();

            }

            this.RefreshPalmDBRecordList(structure.PalmDB.RecordInfoList);

            result.Cover = this.ReadCover(structure);

            return result;
        }

        public bool TryReadWithoutText(out Mobi mobi)
        {
            bool result;

            try
            {
                mobi = this.ReadWithoutText();
                result = true;
            }
            catch (Exception)
            {
                mobi = null;
                result = false;
            }

            return result;
        }

        public Mobi Read()
        {
            var result = this.ReadWithoutText();
            result.Text = this.ReadText(result.Structure);

            return result;
        }

        public bool TryRead(out Mobi mobi)
        {
            bool result;

            try
            {
                mobi = this.Read();
                result = true;
            }
            catch (Exception)
            {
                mobi = null;
                result = false;
            }

            return result;
        }

        public string ReadText(Structure structure)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("stream");
            }
            if (structure is null)
            {
                throw new ArgumentNullException(nameof(structure));
            }

            var decompressedByteList = new List<byte>();
            ICompression compression = null;
            Encoding encoding = Encoding.UTF8;
            ushort maxRecordSize = 4096;

            if (structure.PalmDOCHeader != null)
            {
                if (structure.PalmDOCHeader.RecordSize > 0)
                {
                    maxRecordSize = structure.PalmDOCHeader.RecordSize;
                }
                switch (structure.PalmDOCHeader.Compression)
                {
                    case CompressionType.PalmDOC:
                        {
                            compression = new PalmDocCompression();
                        }
                        break;
                    case CompressionType.HUFF_CDIC:
                        {
                            compression = CreateHuffCdicCompression(structure.MobiHeader);
                        }
                        break;
                    default:
                        {
                            compression = new NoneCompression();
                        }
                        break;
                }
            }

            if (structure.MobiHeader != null)
            {
                var firstTextRecordIndex = this.FindFirstTextRecordIndex(structure.MobiHeader);
                var firstNonTextRecordIndex = this.FindFirstNonTextRecordIndex(structure.MobiHeader);
                for (int i = firstTextRecordIndex; i < firstNonTextRecordIndex; i++)
                {
                    var recordBytes = this.ReadPalmDBRecord(this._palmDBRecordList[i]);
                    var decompressedBytes = compression.Decompress(recordBytes);
                    var fixedDecompressedBytes = decompressedBytes.Length > maxRecordSize ?
                        decompressedBytes.Take(maxRecordSize) :
                        decompressedBytes;
                    decompressedByteList.AddRange(fixedDecompressedBytes);
                }
            }

            return encoding.GetString(decompressedByteList.ToArray());
        }

        public Stream ReadContentFile(ContentFile contentFile)
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("stream");
            }
            if (contentFile is null)
            {
                throw new ArgumentNullException(nameof(contentFile));
            }

            if (int.TryParse(contentFile.Source, out int index) && index < this._palmDBRecordList.Count)
            {
                byte[] bytes = this.ReadPalmDBRecord(this._palmDBRecordList[index]);
                if (bytes != null)
                {
                    return new MemoryStream(bytes);
                }
            }
            return null;
        }

        #region Structure

        private Structure ReadStructure()
        {
            var result = new Structure();
            var palmDB = PalmDBEngine.Read(this._stream) ?? throw new InvalidDataException("file can not open.");
            result.PalmDB = palmDB;
            if (palmDB.RecordInfoList.Any())
            {
                long firstRecordOffset = palmDB.RecordInfoList.First().Offset;
                result.PalmDOCHeader = PalmDOCHeaderEngine.Read(this._stream, firstRecordOffset) ?? throw new InvalidDataException("invalid file! missing part:PalmDOC Header.");

                if (MobiHeaderEngine.TryRead(this._stream, this._stream.Position, out MobiHeader mobiHeader))
                {
                    result.MobiHeader = mobiHeader;
                }
                else
                {
                    throw new InvalidDataException("invalid file! missing part:MOBI Header.");
                }

                if (ExthHeaderEngine.TryRead(this._stream, this._stream.Position, out ExthHeader exthHeader))
                {
                    result.ExthHeader = exthHeader;
                }

                if (mobiHeader.FullNameLength > 0)
                {
                    long fullNameOffset = firstRecordOffset + mobiHeader.FullNameOffset;
                    this._stream.Seek(fullNameOffset, SeekOrigin.Begin);
                    if (this._stream.TryReadString((int)mobiHeader.FullNameLength, out string fullName))
                    {
                        result.FullName = fullName;
                    }
                }

                if (mobiHeader.INDXRecordOffset != MobiHeaderEngine.UnavailableIndex &&
                    mobiHeader.INDXRecordOffset < palmDB.RecordInfoList.Count &&
                    IndxHeaderEngine.TryRead(this._stream, palmDB.RecordInfoList[(int)mobiHeader.INDXRecordOffset].Offset, out IndxHeader indxHeader))
                {
                    result.IndxHeader = indxHeader;
                }

                if (mobiHeader.FLISRecordOffset != MobiHeaderEngine.UnavailableIndex &&
                    mobiHeader.FLISRecordOffset < palmDB.RecordInfoList.Count &&
                    RecordEngine.TryReadFlisRecord(this._stream, palmDB.RecordInfoList[(int)mobiHeader.FLISRecordOffset].Offset, out FlisRecord flisRecord))
                {
                    result.FlisRecord = flisRecord;
                }

                if (mobiHeader.FCISRecordOffset != MobiHeaderEngine.UnavailableIndex &&
                    mobiHeader.FCISRecordOffset < palmDB.RecordInfoList.Count &&
                    RecordEngine.TryReadFcisRecord(this._stream, palmDB.RecordInfoList[(int)mobiHeader.FCISRecordOffset].Offset, out FcisRecord fcisRecord))
                {
                    result.FcisRecord = fcisRecord;
                }

            }

            return result;
        }

        #endregion

        #region Text

        private void RefreshPalmDBRecordList(IList<PalmDBRecordInfo> palmDBRecordInfoList)
        {
            this._palmDBRecordList.Clear();
            PalmDBRecord lastRecord = null;
            foreach (var palmDBRecordInfo in palmDBRecordInfoList)
            {
                var record = new PalmDBRecord(palmDBRecordInfo);
                if (lastRecord != null)
                {
                    lastRecord.Length = (int)(palmDBRecordInfo.Offset - lastRecord.Info.Offset);
                }
                this._palmDBRecordList.Add(record);
                lastRecord = record;
            }
        }

        private HuffCdicCompression CreateHuffCdicCompression(MobiHeader mobiHeader)
        {
            HuffCdicCompression result = null;
            if (mobiHeader.HuffmanRecordOffset != MobiHeaderEngine.UnavailableIndex &&
                mobiHeader.HuffmanRecordOffset < this._palmDBRecordList.Count)
            {
                var huff = this.ReadPalmDBRecord(this._palmDBRecordList[(int)mobiHeader.HuffmanRecordOffset]);

                var cdicList = new List<byte[]>();
                for (int i = 1; i < mobiHeader.HuffmanRecordCount; i++)
                {
                    var recordBytes = this.ReadPalmDBRecord(this._palmDBRecordList[(int)mobiHeader.HuffmanRecordOffset + i]);
                    cdicList.Add(recordBytes);
                }

                result = new HuffCdicCompression(huff, cdicList)
                {
                    ExtraFlags = mobiHeader.ExtraRecordDataFlags
                };
            }
            return result;
        }

        private int FindFirstTextRecordIndex(MobiHeader mobiHeader)
        {
            return mobiHeader.FirstContentRecordOffset > 0 ? mobiHeader.FirstContentRecordOffset : 1;
        }

        private long FindFirstNonTextRecordIndex(MobiHeader mobiHeader)
        {
            long result;
            if (mobiHeader.FirstNonBookIndex != MobiHeaderEngine.UnavailableIndex &&
                mobiHeader.FirstNonBookIndex < this._palmDBRecordList.Count)
            {
                result = mobiHeader.FirstNonBookIndex;
            }
            else
            {
                result = Math.Min(mobiHeader.LastContentRecordOffset, mobiHeader.INDXRecordOffset);
                result = Math.Min(result, mobiHeader.FLISRecordOffset);
                result = Math.Min(result, mobiHeader.FCISRecordOffset);
                result = Math.Min(result, this._palmDBRecordList.Count);
            }
            return result;
        }

        private byte[] ReadPalmDBRecord(PalmDBRecord palmDBRecord)
        {
            byte[] result = new byte[palmDBRecord.Length];
            this._stream.Seek(palmDBRecord.Info.Offset, SeekOrigin.Begin);
            this._stream.Read(result, 0, result.Length);
            return result;
        }

        #endregion

        #region Cover

        private ContentFile ReadCover(Structure structure)
        {
            if (structure is null)
            {
                throw new ArgumentNullException(nameof(structure));
            }

            ContentFile result = null;
            if (structure.MobiHeader != null &&
                structure.MobiHeader.FirstImageIndex != MobiHeaderEngine.UnavailableIndex &&
                structure.MobiHeader.FirstImageIndex < this._palmDBRecordList.Count &&
                structure.ExthHeader != null)
            {
                var exthHeader = structure.ExthHeader;
                var coverOffsetBytes = exthHeader.RecordList.FirstOrDefault(p => p.Type == ExthRecordType.CoverOffset)?.Data;
                if (coverOffsetBytes != null)
                {
                    var coverImageIndex = structure.MobiHeader.FirstImageIndex + coverOffsetBytes.ToUInt32();
                    if (coverImageIndex < this._palmDBRecordList.Count)
                    {
                        result = new ContentFile(String.Empty, coverImageIndex.ToString());
                    }
                }
            }
            return result;
        }

        #endregion

        #region Disposable

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
            }

            _disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }
}
