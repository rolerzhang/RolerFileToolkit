using System;
using System.IO;
using System.Linq;
using Roler.Toolkit.File.Mobi.Engine;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi
{
    public class MobiReader : IDisposable
    {
        private bool _disposed;
        private readonly Stream _stream;

        public MobiReader(Stream stream)
        {
            this._stream = stream;
        }

        #region Methods

        public Mobi Read()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("stream");
            }

            var structure = this.ReadStructure();
            var result = new Mobi
            {
                Structure = structure,
            };

            return result;
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
