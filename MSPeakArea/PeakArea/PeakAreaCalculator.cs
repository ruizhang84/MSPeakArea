using MSPeakArea.Spectrum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSPeakArea.PeakArea
{
    public class PeakAreaCalculator
    {
        private IAreaCalculator calculator;
        private IBounder bounder;

        public PeakAreaCalculator(IAreaCalculator calculator, IBounder bounder)
        {
            this.calculator = calculator;
            this.bounder = bounder;
        }

        public void Init(List<IPeak> peaks)
        {
            calculator.Init(peaks);
            bounder.Init(peaks);
        }

        public double Area(IPeak peak)
        {
            int left = bounder.Left(peak);
            int right = bounder.Right(peak);

            return calculator.Area(left, right);
        }

    }
}
