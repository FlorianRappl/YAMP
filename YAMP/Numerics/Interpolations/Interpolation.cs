using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class Interpolation
    {
        int np;
        MatrixValue _samples;

        public Interpolation(MatrixValue samples)
        {
            _samples = samples;
            np = samples.DimensionY;

            if (samples.DimensionX != 2)
                throw new MatrixFormatException("a Nx2 matrix, but the number of columns is " + samples.DimensionX + ".");
        }

        public MatrixValue Samples { get { return _samples; } }

        public int Np { get { return np; } }

        public virtual MatrixValue ComputeValues(MatrixValue x)
        {
            var y = new MatrixValue(x.DimensionY, x.DimensionX);

            for (var i = 1; i <= x.DimensionX; i++)
                for (var j = 1; j <= x.DimensionY; j++)
                    y[j, i].Value = ComputeValue(x[j, i].Value);

            return y;
        }

        public abstract double ComputeValue(double x);

        protected void SolveTridiag(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
        {
            int i;

            for (i = 2; i <= n; i++)
            {
                sub[i] = sub[i] / diag[i - 1];
                diag[i] = diag[i] - sub[i] * sup[i - 1];
                b[i] = b[i] - sub[i] * b[i - 1];
            }

            b[n] = b[n] / diag[n];

            for (i = n - 1; i >= 1; i--)
                b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
        }
    }
}
