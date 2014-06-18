/*
    Copyright (c) 2012-2014, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
