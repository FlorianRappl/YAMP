using System;
using YAMP;

namespace YAMP.Numerics
{
    public class NevilleInterpolator : Interpolation
    {
        public NevilleInterpolator(MatrixValue vector) : base(vector)
        {
        }

        public override double ComputeValue(double t)
        {
            var x = Samples.GetRealVector(0, Np, 0, 1);
            var f = Samples.GetRealVector(0, Np, 1, 1);
            var n = Np - 1;

            for (int m = 1; m <= n; m++)
            {
                for (int i = 0; i <= n - m; i++)
                    f[i] = ((t - x[i + m]) * f[i] + (x[i] - t) * f[i + 1]) / (x[i] - x[i + m]);
            }

            return f[0];
        }
    }
}
