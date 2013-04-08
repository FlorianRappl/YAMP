using System;

namespace YAMP
{
    [Description("The function provides polynomial evaluation. In mathematics, a polynomial is an expression of finite length constructed from variables (also called indeterminates) and constants, using only the operations of addition, subtraction, multiplication, and non-negative integer exponents. However, the division by a constant is allowed, because the multiplicative inverse of a non zero constant is also a constant.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Polynomial")]
    sealed class PolyvalFunction : ArgumentFunction
    {
        [Description("The function returns the value of a polynomial of degree n evaluated at X. The input argument p is a vector of length n + 1 whose elements are the coefficients in ascending powers of the polynomial to be evaluated.")]
        [Example("polyval([1 2 3], [5 7 9])", "The polynomial p(x) = 3 * x^2 + 2 * x + 1 is evaluated at x = 5, 7, and 9 with the result 86, 162, 262.")]
        public MatrixValue Function(MatrixValue p, MatrixValue X)
        {
            var coeff = p.ToArray();
            var M = new MatrixValue(X.Rows, X.Columns);
            var poly = BuildPolynom(coeff);

            for (var j = 1; j <= X.Rows; j++)
                for (var i = 1; i <= X.Columns; i++)
                    M[j, i] = poly(X[j, i]);

            return M;
        }

        [Description("The function returns the value of a polynomial of degree n evaluated at z. The input argument p is a vector of length n + 1 whose elements are the coefficients in ascending powers of the polynomial to be evaluated.")]
        [Example("polyval([1 2 3], 5)", "The polynomial p(z) = 3 * z^2 + 2 * z + 1 is evaluated at z = 5 with the result 86.")]
        public ScalarValue Function(MatrixValue p, ScalarValue z)
        {
            var coeff = p.ToArray();
            var poly = BuildPolynom(coeff);
            return poly(z);
        }

        Func<ScalarValue, ScalarValue> BuildPolynom(ScalarValue[] coeff)
        {
            return z =>
            {
                var pow = ScalarValue.One;
                var sum = ScalarValue.Zero;

                for (var i = 0; i < coeff.Length; i++)
                {
                    var c = coeff[i];
                    sum += c * pow;
                    pow *= z;
                }

                return sum;
            };
        }
    }
}
