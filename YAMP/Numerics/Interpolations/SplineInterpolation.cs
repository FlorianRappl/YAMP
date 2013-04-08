using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Interpolation with the spline method.
    /// </summary>
    public class SplineInterpolation : Interpolation
    {
        double[] a;
        double[] h;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="samples">The Nx2 matrix containing the sample data.</param>
        public SplineInterpolation(MatrixValue samples) : base(samples)
        {
            a = new double[Np];
            h = new double[Np];

            for (int i = 2; i <= Np; i++)
                h[i - 1] = samples[i, 1].Re - samples[i - 1, 1].Re;

            if (Np > 2)
            {
                double[] sub = new double[Np - 1];
                double[] diag = new double[Np - 1];
                double[] sup = new double[Np - 1];

                for (int i = 2; i < Np; i++)
                {
                    int j = i - 1;
                    diag[j] = (h[j] + h[j + 1]) / 3;
                    sup[j] = h[j + 1] / 6;
                    sub[j] = h[j] / 6;
                    a[j] = (samples[i + 1, 2].Re - samples[i, 2].Re) / h[j + 1] - (samples[i, 2].Re - samples[i - 1, 2].Re) / h[j];

                }

                SolveTridiag(sub, diag, sup, ref a, Np - 2);
            }
        }

        /// <summary>
        /// Computes a specific interpolated value f(x).
        /// </summary>
        /// <param name="x">The value where to interpolate.</param>
        /// <returns>The computed value y = f(t).</returns>
        public override double ComputeValue(double x)
        {
            MatrixValue samples = Samples;

            if (a.Length > 1)
            {
                int gap = 1;
                double previous = 0.0;

                for (int i = 1; i < a.Length; i++)
                {
                    if (samples[i, 1].Re < x && (i == 1 || samples[i, 1].Re > previous))
                    {
                        previous = samples[i, 1].Re;
                        gap = i;
                    }
                }

                double x1 = x - previous;
                double x2 = h[gap] - x1;
                return ((-a[gap - 1] / 6 * (x2 + h[gap]) * x1 + samples[gap, 2].Re) * x2 + (-a[gap] / 6 * (x1 + h[gap]) * x2 + samples[gap + 1, 2].Re) * x1) / h[gap];
            }

            return 0;
        }
    }
}
