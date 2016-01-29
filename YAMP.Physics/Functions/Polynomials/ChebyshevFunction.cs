using System;
using YAMP;
using YAMP.Numerics;

namespace YAMP.Physics
{
    [Kind(PopularKinds.Function)]
    [Description("In mathematics the Chebyshev polynomials are a sequence of orthogonal polynomials which are related to de Moivre's formula and which can be defined recursively. One usually distinguishes between Chebyshev polynomials of the first kind which are denoted Tn and Chebyshev polynomials of the second kind which are denoted Un. The letter T is used because of the alternative transliterations of the name Chebyshev as Tchebycheff (French) or Tschebyschow (German). This function corresponds to the Chebyshev polynomials of first kind.")]
    class ChebyshevFunction : ArgumentFunction
    {
        [Description("Evaluates the Chebyshev polynomial of some order n at the given point x in R.")]
        [Example("chebyshev(3, 0.5)", "Evaluates the Chebyshev polynomial of order 3 at the point x = 0.5.")]
        public ScalarValue Function(ScalarValue n, ScalarValue x)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Chebyshev polynomial of order n < 0 does not make sense.");

            var f = GetPolynom(nn);
            return new ScalarValue(f(x.Re));
        }

        [Description("Evaluates the Chebyshev polynomial of some order n at the given points in the matrix X in R.")]
        [Example("chebyshev(1, [-1, 0.5, 0, 0.5, 1])", "Evaluates the first Chebyshev polynomial (which is just x), at the points -1 to 1 with a spacing of 0.5.")]
        public MatrixValue Function(ScalarValue n, MatrixValue X)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Chebyshev polynomial of order n < 0 does not make sense.");

            var M = new MatrixValue(X.DimensionY, X.DimensionX);
            var f = GetPolynom(nn);

            for (var i = 1; i <= X.Length; i++)
                M[i] = new ScalarValue(f(X[i].Re));

            return M;
        }

        #region Polynom

        public static Func<double, double> GetPolynom(int n)
        {
            switch (n)
            {
                case 0:
                    return x => 1.0;
                case 1:
                    return x => x;
                default:
                    return x =>
                    {
                        var sum = 0.0;
                        var m = n / 2;
                        var s = 1;

                        for (var k = 0; k <= m; k++)
                        {
                            var nom = s * Helpers.Factorial(n - k - 1) * Math.Pow(2 * x, n - 2 * k);
                            var den = Helpers.Factorial(n - 2 * k) * Helpers.Factorial(k);
                            s *= (-1);
                            sum += nom / den;
                        }

                        return n / 2.0 * sum;
                    };
            }
        }

        #endregion
    }
}
