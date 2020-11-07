using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSPeakArea.Spectrum;

namespace MSPeakArea.Process.PeakPicking.CWT
{
    public class PeakPickingCWT : IProcess
    {
        private double maxScale;
        private double steps;
        public PeakPickingCWT(double maxScale=120, double steps=6)
        {
            this.maxScale = maxScale;
            this.steps = steps;
        }
        public ISpectrum Process(ISpectrum spectrum)
        {
            List<IPeak> peaks = spectrum.GetPeaks();

            double[] signal = peaks.Select(p => p.GetIntensity()).ToArray();
            SortedDictionary<double, List<double>> matrix =
                new SortedDictionary<double, List<double>>();
            for (double a = 1.0; a <= maxScale; a += steps)
            {
                double[] processed = Algorithm.CWT.Transform(signal, a);
                matrix[a] = processed.ToList();
            }

            CoeffMatrix coeffMatrix = new CoeffMatrix(
                spectrum.GetPeaks().Select(p => p.GetMZ()).ToList(),
                matrix, 1.0, 2, 1, 2);

            List<RidgeLine> lines = coeffMatrix.FindRidgeLine();
            HashSet<double> mz = lines.Select(p => p.Pos).ToHashSet();

            List<IPeak> processedPeaks = peaks.Where(p => mz.Contains(p.GetMZ())).ToList();
            spectrum.SetPeaks(processedPeaks);
            return spectrum;
        }
    }
}
