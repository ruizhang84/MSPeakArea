using MSPeakArea.Spectrum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSPeakArea.PeakArea
{
    public interface IBounder
    {
        void Init(List<IPeak> peaks);
        int Left(IPeak peak);
        int Right(IPeak peak);
    }
}
