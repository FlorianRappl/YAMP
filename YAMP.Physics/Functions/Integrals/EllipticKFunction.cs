namespace YAMP.Physics
{
    using System;
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("Elliptic Integrals are said to be 'complete' when the amplitude φ = π/2 and therefore x = 1. The complete elliptic integral of the first kind K is evaluated by this function.")]
    [Kind(PopularKinds.Function)]
    class EllipticKFunction : StandardFunction
    {
        [Description("Computes the complete elliptic integral of the first kind at the argument x.")]
        [Example("elliptick(3)", "Evaluates the complete elliptic integral at x = 3.")]
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(EllipticK(value.Re));
        }

        #region Algorithms

        public static Double EllipticK(Double k)
        {
            if ((k < 0) || (k > 1.0))
                throw new YAMPArgumentRangeException("k", 0.0, 1.0);

            if (k < 0.25)
            {
                return EllipticSeries(k);
            }
            else if (k > 0.875)
            {
                // for large k, use the asymptotic expansion near k~1, k'~0
                var k1 = Math.Sqrt(1.0 - k * k);

                // k'=0.484 at k=0.875
                if (k1 == 0.0)
                {
                    return Double.PositiveInfinity;
                }

                return EllipticAsymptotic(k1);
            }

            return EllipticAGM(k);
        }

        static Double EllipticSeries(Double k)
        {
            var z = 1.0;
            var f = 1.0;

            for (var n = 1; n < 250; n++)
            {
                var f_old = f;
                z = z * (2 * n - 1) / (2 * n) * k;
                f += z * z;

                if (f == f_old)
                {
                    return Helpers.HalfPI * f;
                }
            }

            throw new YAMPNotConvergedException("EllipticK");
        }

        static Double EllipticAsymptotic(Double k1)
        {
            var p = 1.0;
            var q = Math.Log(1.0 / k1) + 2.0 * Helpers.LogTwo;
            var f = q;

            for (var m = 1; m < 250; m++)
            {
                var f_old = f;
                p *= k1 / m * (m - 0.5);
                q -= 1.0 / m / (2 * m - 1);
                var df = p * p * q;
                f += df;

                if (f == f_old)
                {
                    return f;
                }
            }

            throw new YAMPNotConvergedException("EllipticK");
        }

        static Double EllipticAGM(Double k)
        {
            var tol = Math.Pow(2.0, -24);
            var a = 1.0 - k;
            var b = 1.0 + k;

            for (var n = 0; n < 250; n++)
            {
                var am = (a + b) / 2.0;

                if (Math.Abs(a - b) < tol)
                {
                    return Helpers.HalfPI / am;
                }

                var gm = Math.Sqrt(a * b);
                a = am;
                b = gm;
            }

            throw new YAMPNotConvergedException("EllipticK");
        }

        #endregion
    }
}
