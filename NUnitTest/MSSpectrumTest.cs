using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MSAlignmentClassLibrary.Reader;
using MSPeakArea.Reader;
using MSPeakArea.Spectrum;
using NUnit.Framework;
using MSPeakArea.Algorithm;
using MSPeakArea.Process.PeakPicking.CWT;

namespace NUnitTest
{
    public class MSSpectrumTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void test1()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            string path = @"C:\Users\Rui Zhang\Downloads\Serum_1_C18_03292019_Ali.raw";

            ISpectrumReader reader = new ThermoRawSpectrumReader();
            reader.Init(path);

            //for (int i = reader.GetFirstScan(); i < reader.GetLastScan(); i++)
            int i = 342;
            {
                if (reader.GetMSnOrder(i) < 2)  
                {
                    ISpectrum spectrum = reader.GetSpectrum(i);
                    List<IPeak> peaks = spectrum.GetPeaks();

                    double[] signal = peaks.Select(p => p.GetIntensity()).ToArray();
                    SortedDictionary<double, List<double>> matrix = 
                        new SortedDictionary<double, List<double>>();
                    for (double a = 1; a <= 64; a += 4)
                    {
                        double[] processed = CWT.Transform(signal, a);
                        matrix[a] = processed.ToList();
                    }

                    CoeffMatrix coeffMatrix = new CoeffMatrix(
                        spectrum.GetPeaks().Select(p => p.GetMZ()).ToList(),
                        matrix, 1.0, 2);

                    List<RidgeLine> lines = coeffMatrix.FindRidgeLine();
                    Console.WriteLine(lines.Count);
                    Console.WriteLine(peaks.Count);
                    foreach (RidgeLine line in lines)
                    {
                        Console.WriteLine(line.Pos);
                    }
                }

                //break;

            }

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            //Console.Read();
        }
    }
}
