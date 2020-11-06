using FFTW.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MSPeakArea.Algorithm
{
    public class CWT
    {
        public static Complex[] Convolve(Complex[] u, Complex[] v)
        {
            if (u.Length > v.Length)
            {
                Complex[] temp = u;
                u = v;
                v = temp;
            }

            int m = u.Length;
            int n = v.Length;
            int k = n + m -1;

            Complex[] w = new Complex[k];
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (i-j >= 0 && i-j < n)
                    {
                        w[i] += u[j] * v[i-j];
                    }
                }
            }
            int skip = k - Math.Max(m, n) - (int) Math.Floor(Math.Min(m, n) / 2.0);
            return w.Skip(skip).Take(Math.Max(m, n)).ToArray();
        }

        public static double[] Transform(double[] X, double a)
        {
            // convert to Complex
            Complex[] x = X.Select(i => new Complex(i, 0)).ToArray();
            
            Complex[] points = new Complex[x.Length];
            for (int i = 0; i < x.Length; i++) 
            {
                points[i] = new Complex(i- (x.Length-1)/2.0, 0);
            }
            Complex[] psi = MaxicanHat.Wavelet(points, a)
                .Select(p => Complex.Conjugate(p)).Reverse().ToArray();
                 
            // compute convolution of signal and wavelet
            Complex[] result = Convolve(x, psi);

            return result.Select(p => p.Real).ToArray();
        }

    }
}
