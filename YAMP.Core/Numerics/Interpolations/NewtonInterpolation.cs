using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The Newton polynomial interpolation method.
    /// </summary>
    public class NewtonInterpolation : Interpolation
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="vector">The Nx2 vector with the samples.</param>
        public NewtonInterpolation(MatrixValue vector) : base(vector)
        {
        }

        /// <summary>
        /// Computes a value f(t) at t.
        /// </summary>
        /// <param name="t">The t value.</param>
        /// <returns>Returns the interpolated y = f(t) value.</returns>
        public override double ComputeValue(double t)
        {
            double F, LN, XX, X = 1;
            int i, j, k;

            for (i = 2, LN = Samples[1, 2].Re; i <= Np; i++)
            {
                X *= (t - Samples[i, 1].Re);

                for (j = 1, F = 0; j <= i; j++)
                {
                    for (k = 1, XX = 1; k <= i; k++)
                    {
                        if (k != j)
                            XX *= Samples[j, 1].Re - Samples[k, 1].Re;
                    }

                    F += Samples[j, 2].Re / XX;
                }

                LN += X * F;
            }

            return LN;
        }
    }
}
