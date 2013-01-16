using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class RadixTwoTransformlet : Transformlet
    {
        public RadixTwoTransformlet(int N, ScalarValue[] u) 
            : base(2, N, u) 
        {
        }

        public override void FftPass(ScalarValue[] x, ScalarValue[] y, int Ns, int sign)
        {
            int dx = N / 2;
            for (int j = 0; j < dx; j++)
            {
                int du = (dx / Ns) * (j % Ns);
                int y0 = Expand(j, Ns, 2);

                if (sign < 0)
                    FftKernel(x[j], x[j + dx] * u[N - du], out y[y0], out y[y0 + Ns]);
                else
                    FftKernel(x[j], x[j + dx] * u[du], out y[y0], out y[y0 + Ns]);
            }
        }

        static void FftKernel(ScalarValue x0, ScalarValue x1, out ScalarValue y0, out ScalarValue y1)
        {
            double a0 = x0.Re;
            double b0 = x0.Im;
            double a1 = x1.Re;
            double b1 = x1.Im;

            y0 = new ScalarValue(a0 + a1, b0 + b1);
            y1 = new ScalarValue(a0 - a1, b0 - b1);
        }
    }
}
