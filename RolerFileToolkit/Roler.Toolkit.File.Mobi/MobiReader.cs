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
            var palmDB = PalmDBEngine.Read(this._stream) ?? throw new InvalidDataException("file can not open.");
            PalmDOCHeader palmDOCHeader = null;
            if (palmDB.RecordInfoList.Any())
            {
                var startOffset = palmDB.RecordInfoList.First().Offset;
                palmDOCHeader = PalmDOCHeaderEngine.Read(this._stream, startOffset) ?? throw new InvalidDataException("file can not open.");
            }

            var offset = this._stream.Position;
            if (MobiHeaderEngine.TryRead(this._stream, offset, out MobiHeader mobiHeader))
            {
                offset = this._stream.Position;
            }

            return new Structure
            {
                PalmDB = palmDB,
                PalmDOCHeader = palmDOCHeader,
                MobiHeader = mobiHeader,
            };
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
