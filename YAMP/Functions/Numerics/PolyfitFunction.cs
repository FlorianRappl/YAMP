using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Polynomial curve fitting by finding coefficients for constructing a polynom with degree n. Curve fitting is the process of constructing a curve, or mathematical function, that has the best fit to a series of data points, possibly subject to constraints. Curve fitting can involve either interpolation, where an exact fit to the data is required, or smoothing, in which a smooth function is constructed that approximately fits the data.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Curve_fitting")]
    sealed class PolyfitFunction : ArgumentFunction
    {
        [Description("Polyfit finds the coefficients of a polynomial p(x) of degree n that fits the data, p(x(i)) to y(i), in a least squares sense. The result p is a row vector of length n + 1 containing the polynomial coefficients in ascending powers, i.e. p(1) + p(2) * x + p(3) * x^2 ... + p(n + 1) * x^n.")]
        [Example("polyfit(0:0.1:2.5, erf(0:0.1:2.5), 6)", "Evaluates the polynom of degree 6 of the error function between 0 and 2.5. The result is are coefficients for a polynom like p(x) = 0.0084 * x^6 - 0.0983 * x^5 + 0.4217 * x^4 - 0.7435 * x^3 + 0.1471 * x^2 + 1.1064 * x + 0.0004.")]
        public MatrixValue Function(MatrixValue x, MatrixValue y, ScalarValue n)
        {
            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);

            var nn = n.GetIntegerOrThrowException("n", Name);
            var m = nn + 1;

            if (m < 2)
                throw new YAMPArgumentRangeException("n", 0.0);

            var M = new MatrixValue(x.Length, m);
            var b = new MatrixValue(x.Length, 1);

            for (var j = 1; j <= M.Rows; j++)
            {
                var el = ScalarValue.One;
                var z = x[j];

                for (var i = 1; i <= M.Columns; i++)
                {
                    M[j, i] = el;
                    el *= z;
                }

                b[j, 1] = y[j];
            }

            var qr = QRDecomposition.Create(M);
            return qr.Solve(b);
        }
    }
}
