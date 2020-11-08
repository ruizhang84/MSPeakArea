using MSPeakArea.Spectrum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSPeakArea.PeakArea
{
    public class TrapezoidalRule : IAreaCalculator
    {
        private List<IPeak> peaks;
        public void Init(List<IPeak> peaks)
        {
            this.peaks = peaks;
        }

        public double Area(int left, int right)
        {
            if (left >= right || left < 0 || right >= peaks.Count)
                return 0;

            double sums = 0;
            for(int i = left; i <= right; i++)
            {
                if (i == left || i == right)
                    sums += peaks[i].GetIntensity();
                else
                    sums += 2 * peaks[i].GetIntensity();
            }

            return (peaks[right].GetMZ() - peaks[left].GetMZ()) / (right - left) * sums;
        }

    }
}
