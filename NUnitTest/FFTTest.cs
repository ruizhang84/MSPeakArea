using FFTW.NET;
using MSPeakArea.Algorithm;
using NUnit.Framework;
using System;
using System.Numerics;

namespace NUnitTest
{
    public class FFTTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void test1()
        {
            Complex[] input = { 1.0, 1.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0 };

            Complex[] output = FFT.Transform(input);

            foreach(Complex c in output)
            {
                Console.WriteLine(c);
            }

            Assert.AreEqual(output[1].Imaginary, -2.41421356237309, 0.01);

        }

        [Test]
        public void test2()
        {
            Complex[] input = new Complex[] { 1.0, 2.0, 1.0, -1.0, 1.5 };
            Complex[] output = new Complex[input.Length];

            using (var pinIn = new PinnedArray<Complex>(input))
            using (var pinOut = new PinnedArray<Complex>(output))
            {
                DFT.FFT(pinIn, pinOut);
                //DFT.IFFT(pinOut, pinOut);
            }

            for (int i = 0; i < input.Length; i++)
                Console.WriteLine(output[i].ToString(), input[i].ToString());

            Assert.AreEqual(output[1].Imaginary, -1.65109876, 0.01);

        }

    }
}