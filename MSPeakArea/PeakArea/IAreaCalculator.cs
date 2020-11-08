using MSPeakArea.Spectrum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSPeakArea.PeakArea
{
    public interface IAreaCalculator
    {
        void Init(List<IPeak> peaks);
        double Area(int left, int right);
    }
}
