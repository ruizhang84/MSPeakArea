using MSPeakArea.Algorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NUnitTest
{
    public class CWTTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void test1()
        {
            Complex[] t = new Complex[] { -4.5, -3.5, -2.5, -1.5, -0.5, 0.5, 1.5, 2.5, 3.5, 4.5};
            Complex[] y = MaxicanHat.Wavelet(t, 4.0);

            foreach(Complex v in y)
            {
                Console.WriteLine(v);
            }

            Assert.AreEqual(y[2].Real, 0.2173769, 0.0001);
        }

        [Test]
        public void test2()
        {
            Complex[] u = new Complex[] { -2, 2 };
            Complex[] v = new Complex[] { 3, 4, 1, 5, 6};
            Complex[] w = CWT.Convolve(u, v);
            foreach(Complex i in w)
            {
                Console.WriteLine(i);
            }
        }

        [Test]
        public void test3()
        {
            int n = 16;
            double[] t = new double[n];
            for(int i = 0; i < n;  i++)
            {
                t[i] = i - (n - 1) / 2.0;
            }

            double[] y = CWT.Transform(t, 2.0);

            foreach (double v in y)
            {
                Console.WriteLine(v); 
            }
        }
    }
}
