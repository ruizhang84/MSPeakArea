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
        private RidgeLineFinder finder;

        public PeakPickingCWT(RidgeLineFinder finder, double maxScale=120, double steps=6)
        {
            this.finder = finder;
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

            List<RidgeLine> lines = finder.Find(
                spectrum.GetPeaks().Select(p => p.GetMZ()).ToList(),
                matrix);
            HashSet<double> mz = lines.Select(p => p.Pos).ToHashSet();

            List<IPeak> processedPeaks = peaks.Where(p => mz.Contains(p.GetMZ())).ToList();
            ISpectrum newSpectrum = spectrum.Clone();
            newSpectrum.SetPeaks(processedPeaks);
            return newSpectrum;
        }
    }
}
