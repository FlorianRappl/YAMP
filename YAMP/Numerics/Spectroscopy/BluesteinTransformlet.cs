using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class BluesteinTransformlet : Transformlet
    {
        #region ctor

        public BluesteinTransformlet(int R, int N, ScalarValue[] u)
            : base(R, N, u)
        {
            // figure out the right Bluestein length and create a transformer for it
            Nb = SetBluesteinLength(2 * R - 1);
            ft = new Fourier(Nb);

            // compute the Bluestein coeffcients and compute the FT of the filter based on them
            b = ComputeBluesteinCoefficients(R);
            var c = new ScalarValue[Nb];
            c[0] = ScalarValue.One;

            for (int i = 1; i < R; i++)
            {
                c[i] = b[i].Conjugate();
                c[Nb - i] = c[i];
            }

            for (int i = R; i <= Nb - R; i++)
                c[i] = ScalarValue.Zero;

            bt = ft.Transform(c);
        }

        #endregion

        #region Members

        // Length of convolution transform
        int Nb;

        // Fourier transform for convolution transform
        Fourier ft;

        // R Bluestein coefficients
        ScalarValue[] b;

        // Nb-length Fourier transform of the symmetric Bluestein coefficient filter
        ScalarValue[] bt;

        #endregion

        #region Methods

        static ScalarValue[] ComputeBluesteinCoefficients(int R)
        {
            var b = new ScalarValue[R];
            double t = Math.PI / R;
            b[0] = new ScalarValue(1.0);
            int s = 0;
            int TwoR = 2 * R;

            for (int i = 1; i < R; i++)
            {
                s += (2 * i - 1); if (s >= TwoR) s -= TwoR;
                double ts = t * s;
                b[i] = new ScalarValue(Math.Cos(ts), Math.Sin(ts));
            }

            return b;
        }

        public override void FftKernel(ScalarValue[] x, ScalarValue[] y, int y0, int dy, int sign)
        {
            // create c = b x and transform it into Fourier space
            var c = new ScalarValue[Nb];

            if (sign > 0)
            {
                for (int i = 0; i < R; i++)
                    c[i] = b[i] * x[i];
            }
            else
            {
                for (int i = 0; i < R; i++)
                    c[i] = b[i] * x[i].Conjugate();
            }

            for (int i = R; i < c.Length; i++)
                c[i] = ScalarValue.Zero;

            var ct = ft.Transform(c);

            // multiply b-star and c = b x in Fourier space, and inverse transform the product back into configuration space
            for (int i = 0; i < Nb; i++)
            {
                ct[i] = ct[i] * bt[i];
            }

            c = ft.InverseTransform(ct);

            // read off the result
            if (sign > 0)
            {
                for (int i = 0; i < R; i++) 
                    y[y0 + i * dy] = b[i] * c[i];
            }
            else
            {
                for (int i = 0; i < R; i++) 
                    y[y0 + i * dy] = b[i].Conjugate() * c[i].Conjugate();
            }
        }

        static int SetBluesteinLength(int N)
        {
            // try the next power of two
            int t = NextPowerOfBase(N, 2);
            int M = t;

            // see if replacing factors of 4 by 3, which shortens the length, will still be long enough
            while (M % 4 == 0)
            {
                t = (M / 4) * 3;

                if (t < N)
                    break;

                if (t < M) 
                    M = t;
            }

            // try the next power of three
            t = NextPowerOfBase(N, 3);

            if ((t > 0) && (t < M)) 
                M = t;

            return M;
        }

        static int NextPowerOfBase(int n, int b)
        {
            for (int m = b; m <= Int32.MaxValue; m *= b)
            {
                if (m >= n) 
                    return m;
            }

            return -1;
        }

        #endregion
    }
}
