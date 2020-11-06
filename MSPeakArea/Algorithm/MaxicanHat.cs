using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MSPeakArea.Algorithm
{
    public class MaxicanHat
    {
        // factor = 2.0 / (Math.Sqrt(3.0) * Math.Pow(Math.PI, 1.0 / 4))
        const double factor = 0.867325070584078;

        // a - width parameter of the wavelet
        public static Complex Wavelet(Complex t, double a = 1.0)
        {
            return 1.0 / Math.Sqrt(a) * factor * (1 - (t / a) * (t / a)) * Complex.Exp(-(t / a) * (t / a)/2.0);
        }

        public static Complex[] Wavelet(Complex[] t, double a = 1.0)
        {
            Complex[] ricker = new Complex[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                ricker[i] = Wavelet(t[i], a);
            }
            return ricker;
        }

    }
}
