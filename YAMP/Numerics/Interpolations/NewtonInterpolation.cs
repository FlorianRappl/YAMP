using System;
using YAMP;

namespace YAMP.Numerics
{
    public class NewtonInterpolation : Interpolation
    {
        public NewtonInterpolation(MatrixValue vector) : base(vector)
        {
        }

        public override double ComputeValue(double t)
        {
            double F, LN, XX, X = 1;
            int i, j, k;

            for (i = 2, LN = Samples[1, 2].Value; i <= Np; i++)
            {
                X *= (t - Samples[i, 1].Value);

                for (j = 1, F = 0; j <= i; j++)
                {
                    for (k = 1, XX = 1; k <= i; k++)
                    {
                        if (k != j)
                            XX *= Samples[j, 1].Value - Samples[k, 1].Value;
                    }

                    F += Samples[j, 2].Value / XX;
                }

                LN += X * F;
            }

            return LN;
        }
    }
}
