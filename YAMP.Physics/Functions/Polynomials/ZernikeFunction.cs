namespace YAMP.Physics
{
    using YAMP.Exceptions;

    [Description("In mathematics, the Zernike polynomials are a sequence of polynomials that are orthogonal on the unit disk. Named after Nobel Prize winner and optical physicist, and inventor of the phase contrast microscopy, Frits Zernike, they play an important role in beam optics.")]
    [Kind(PopularKinds.Function)]
    class ZernikeFunction : ArgumentFunction
    {
        [Description("Computes the Zernike polynomial with order n and m at the point z.")]
        [Example("zernike(0, 0, 0.5)", "The 0th order polynomial is always 1.0.")]
        [Example("zernike(1, 0, 0.25)", "Computes the 1st order polynomial with parameter m = 0 at the point 0.25, which gives us -0.25.")]
        public ScalarValue Function(ScalarValue n, ScalarValue m, ScalarValue z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
            var nm = m.GetIntegerOrThrowException("m", Name);
            return Zernike(nn, nm, z);
        }

        [Description("Computes the Zernike polynomial with order n and m at the points in Z.")]
        [Example("zernike(1, 1, 0:0.1:1)", "The polynomial at order 1, 1 evaluated at the values 0, 0.1, 0.2, ..., 1.0.")]
        public MatrixValue Function(ScalarValue n, ScalarValue m, MatrixValue Z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
            var nm = m.GetIntegerOrThrowException("m", Name);
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.DimensionX; i++)
            {
                for (var j = 1; j <= Z.DimensionY; j++)
                {
                    M[j, i] = Zernike(nn, nm, Z[j, i]);
                }
            }

            return M;
        }

        #region Algorithm

        static ScalarValue Zernike(int n, int m, ScalarValue rho)
        {
            if (n < 0)
                throw new YAMPArgumentRangeException("n");

            if ((m < 0) || (m > n))
                throw new YAMPArgumentRangeException("m", "n >= m >= 0");

            // n and m have the same parity
            if ((n - m) % 2 != 0)
            {
                return ScalarValue.Zero;
            }

            // R00
            if (n == 0)
            {
                return ScalarValue.One;
            }

            var absrho = rho.Abs();

            if ((absrho < 0.0) || (absrho > 1.0))
                throw new YAMPNotConvergedException("zernike");

            // R^{m}_m
            var r2 = rho.Pow(new ScalarValue(m));

            if (n == m)
            {
                return r2;
            }

            // R^{m+1}_{m+1}
            var k = m;
            var r1 = r2 * rho;

            while (true)
            {
                k += 2;

                // *
                //  \
                //   * recurrence involving two lesser m's
                //  /
                // *
                // 2n R^{m+1}_{n-1} = (n+m) R^{m}_{n-2} + (n-m) R^{m}_{n}

                var r0 = ((2 * k) * rho * r1 - (k + m) * r2) / (k - m);

                if (k == n)
                {
                    return r0;
                }

                //   *
                //  /
                // * recurrence involving two greater m's
                //  \
                //   *
                // 

                var rp = (2 * (k + 1) * rho * r0 - (k - m) * r1) / (k + m + 2);

                r2 = r0;
                r1 = rp;
            }
        }

        #endregion
    }
}
