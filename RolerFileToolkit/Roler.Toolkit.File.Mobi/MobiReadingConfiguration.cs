using System;
using Roler.Toolkit.File.Mobi.Engine;
using Roler.Toolkit.File.Mobi.Entity;

namespace Roler.Toolkit.File.Mobi
{
    public class MobiReadingConfiguration
    {
        public virtual int FindFirstTextRecordIndex(MobiHeader mobiHeader)
        {
            if (mobiHeader is null)
            {
                throw new ArgumentNullException(nameof(mobiHeader));
            }

            return mobiHeader.FirstContentRecordOffset > 0 ? mobiHeader.FirstContentRecordOffset : 1;
        }

        public virtual long FindFirstNonTextRecordIndex(MobiHeader mobiHeader, long recordCount)
        {
            if (mobiHeader is null)
            {
                throw new ArgumentNullException(nameof(mobiHeader));
            }

            long result;
            if (mobiHeader.FirstNonBookIndex != MobiHeaderEngine.UnavailableIndex &&
                mobiHeader.FirstNonBookIndex < recordCount)
            {
                result = mobiHeader.FirstNonBookIndex;
            }
            else
            {
                result = Math.Min(mobiHeader.LastContentRecordOffset, mobiHeader.INDXRecordOffset);
                result = Math.Min(result, mobiHeader.FLISRecordOffset);
                result = Math.Min(result, mobiHeader.FCISRecordOffset);
                result = Math.Min(result, recordCount);
            }
            return result;
        }
    }
}
