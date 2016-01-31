namespace YAMP.Physics
{
    using System;
    using YAMP.Exceptions;

    [Description("In mathematics, the Lambert W function, also called the omega function or product logarithm, is a set of functions, namely the branches of the inverse relation of the function f(w) = w * eexp(w) where exp(w) is the exponential function and w is any complex number. In other words, the defining equation for W(z) is z = W(z) * exp(W(z)).")]
    [Kind(PopularKinds.Function)]
    class LambertFunction : StandardFunction
    {
        [Description("Computes the value of the funtion R_F(x, y, z) by solving the elliptic integral for the real arguments x, y, z.")]
        [Example("carlsonf(1, 2.5, 1.5)", "Evaluates R_F at x = 1, y = 2.5 and z = 1.5.")]
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return LambertW(value);
        }

        #region Algorithms

        static ScalarValue LambertW(ScalarValue x)
        {
            var EI = 1.0 / Math.E;

            // use an initial approximation
            var W = ScalarValue.Zero;
            var abs = x.Abs();

            if (abs < EI / 2.0)
            {
                W = SeriesSmall(x);

                if ((x + EI).Abs() < 1e-6)
                {
                    return W;
                }
            }
            else if (abs < EI)
            {
                W = SeriesZero(x);
            }
            else if (abs > Math.E)
            {
                W = SeriesLarge(x);
            }
            else
            {
                W = new ScalarValue(0.5);
            }

            return Halley(x, W);
        }

        static ScalarValue Halley(ScalarValue x, ScalarValue w0)
        {
            for (var i = 0; i < 250; i++)
            {
                var e = w0.Exp();
                var f = e * w0 - x;
                var dw = f / ((w0 + 1.0) * e - ((w0 + 2.0) / (w0 + 1.0)) * f / 2.0);
                var w1 = w0 - dw;

                if (w1 == w0)
                {
                    return w1;
                }

                w0 = w1;
            }

            throw new YAMPNotConvergedException("Lambert");
        }

        // series useful near 0
        static ScalarValue SeriesZero(ScalarValue x)
        {
            var xx = x * x;
            return x - xx + (3.0 / 2.0) * xx * x - (8.0 / 3.0) * xx * xx;
        }

        // series useful near -1/e
        static ScalarValue SeriesSmall(ScalarValue x)
        {
            var p = (2.0 * (Math.E * x + 1.0)).Sqrt();
            var pp = p * p;
            return -1.0 + p - pp / 3.0 + (11.0 / 72.0) * pp * p - (43.0 / 540.0) * pp * pp;
        }

        // series useful for large x
        static ScalarValue SeriesLarge(ScalarValue x)
        {
            var L1 = x.Ln();
            var L2 = L1.Ln();
            var L2L1 = L2 / L1;
            return L1 - L2 + L2L1 + (L2 - 2.0) * L2L1 / L1 / 2.0;
        }

        #endregion
    }
}
