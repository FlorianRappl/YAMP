using System;
using YAMP;

namespace YAMP.Numerics
{
    internal class RadixThreeTransformlet : Transformlet
    {
        public RadixThreeTransformlet(int N, ScalarValue[] u)
            : base(3, N, u)
        {
        }

        public override void FftPass(ScalarValue[] x, ScalarValue[] y, int Ns, int sign)
        {
            int dx = N / 3;

            for (int j = 0; j < dx; j++)
            {
                int du = (dx / Ns) * (j % Ns);
                int y0 = Expand(j, Ns, 3);

                if (sign < 0)
                    FftKernel(x[j], x[j + dx] * u[N - du], x[j + 2 * dx] * u[N - 2 * du], out y[y0], out y[y0 + Ns], out y[y0 + 2 * Ns], -1);
                else
                    FftKernel(x[j], x[j + dx] * u[du], x[j + 2 * dx] * u[2 * du], out y[y0], out y[y0 + Ns], out y[y0 + 2 * Ns], 1);
            }
        }

        static void FftKernel(ScalarValue x0, ScalarValue x1, ScalarValue x2, out ScalarValue y0, out ScalarValue y1, out ScalarValue y2, int sign)
        {
            double a12p = x1.Re + x2.Re;
            double b12p = x1.Im + x2.Im;
            double sa = x0.Re + r31.Re * a12p;
            double sb = x0.Im + r31.Re * b12p;
            double ta = r31.Im * (x1.Re - x2.Re);
            double tb = r31.Im * (x1.Im - x2.Im);

            if (sign < 0) 
                ta = -ta; tb = -tb;

            y0 = new ScalarValue(x0.Re + a12p, x0.Im + b12p);
            y1 = new ScalarValue(sa - tb, sb + ta);
            y2 = new ScalarValue(sa + tb, sb - ta);
        }

        static readonly ScalarValue r31 = new ScalarValue(-1.0 / 2.0, Math.Sqrt(3.0) / 2.0);
    }
}
