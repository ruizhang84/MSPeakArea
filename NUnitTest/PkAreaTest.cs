using System;
using System.Collections.Generic;
using System.Text;
using MSAlignmentClassLibrary.Reader;
using MSPeakArea.PeakArea;
using MSPeakArea.Process;
using MSPeakArea.Process.PeakPicking.CWT;
using MSPeakArea.Reader;
using MSPeakArea.Spectrum;
using NUnit.Framework;

namespace NUnitTest
{
    public class PkAreaTest
    {
        [Test]
        public void test1()
        {
            double[] mz = new double[] { 1, 2, 3, 4, 5 };
            double[] intensity = new double[] { 1, 2, 3, 4, 1 };
            List<IPeak> peaks = new List<IPeak>();
            for(int i = 0; i < mz.Length; i++)
            {
                peaks.Add(new GeneralPeak(mz[i], intensity[i]));
            }

            IAreaCalculator calculator = new TrapezoidalRule();
            IBounder bounder = new PerpendicularDrop();
            PeakAreaCalculator areaCalculator = new PeakAreaCalculator(calculator, bounder);
            areaCalculator.Init(peaks);

            Console.WriteLine(areaCalculator.Area(new GeneralPeak(4.0, 1)));
        }

        [Test]
        public void test2()
        {
            string path = @"C:\Users\Rui Zhang\Downloads\Serum_1_C18_03292019_Ali.raw";

            ISpectrumReader reader = new ThermoRawSpectrumReader();
            reader.Init(path);
            RidgeLineFinder coeffMatrix = new RidgeLineFinder(1.0, 2, 1, 2);
            IProcess processer = new PeakPickingCWT(coeffMatrix);

            //for (int i = reader.GetFirstScan(); i < reader.GetLastScan(); i++)
            int i = 347;
            {
                if (reader.GetMSnOrder(i) < 2)
                {
                    ISpectrum spectrum = reader.GetSpectrum(i);
                    List<IPeak> peaks = spectrum.GetPeaks();
                    spectrum = processer.Process(spectrum);

                    IAreaCalculator calculator = new TrapezoidalRule();
                    IBounder bounder = new PerpendicularDrop();
                    PeakAreaCalculator areaCalculator = new PeakAreaCalculator(calculator, bounder);
                    areaCalculator.Init(peaks);

                    foreach(IPeak peak in spectrum.GetPeaks())
                    {
                        Console.WriteLine(peak.GetMZ() + " : " + areaCalculator.Area(peak).ToString());
                    }
                }
            }
        }
    }
}
