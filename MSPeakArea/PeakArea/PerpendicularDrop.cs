using MSPeakArea.Algorithm;
using MSPeakArea.Spectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSPeakArea.PeakArea
{
    public class PerpendicularDrop : IBounder
    {
        private List<IPeak> peaks;
        private List<double> mz;
        public void Init(List<IPeak> peaks)
        {
            this.peaks = peaks;
            mz = peaks.Select(p => p.GetMZ()).ToList();
        }
        public int Index(IPeak peak)
        {
            return BinarySearch.Search(mz, peak.GetMZ(), 0.0);
        }

        public int Left(IPeak peak)
        {
            int index = Index(peak);
            int left = index;
            for(; left>0; left--)
            {
                if (peaks[left].GetIntensity() < peaks[left-1].GetIntensity())
                    break;
            }
            return left;
        }

        public int Right(IPeak peak)
        {
            int index = Index(peak);
            int right = index;
            for (; right<peaks.Count-1; right++)
            {
                if (peaks[right].GetIntensity() < peaks[right+1].GetIntensity())
                    break;
            }
            return right;
        }


    }
}
