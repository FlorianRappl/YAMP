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
    [Description("In mathematics, Legendre functions are solutions to Legendre's differential equation. They are named after Adrien-Marie Legendre. This ordinary differential equation is frequently encountered in physics and other technical fields. In particular, it occurs when solving Laplace's equation (and related partial differential equations) in spherical coordinates. The Legendre differential equation may be solved using the standard power series method. The equation has regular singular points at x = ±1 so, in general, a series solution about the origin will only converge for |x| < 1. When n is an integer, the solution Pn(x) that is regular at x = 1 is also regular at x = −1, and the series for this solution terminates (i.e. is a polynomial).")]
    [Kind(PopularKinds.Function)]
    class LegendreFunction : ArgumentFunction
    {
        [Description("Evaluates the Legendre polynomial of some order n at the given point x in R.")]
        [Example("legendre(3, 0.5)", "Evaluates the Legendre polynomial of order 3 at the point x = 0.5.")]
        public ScalarValue Function(ScalarValue n, ScalarValue x)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Legendre polynomial of order n < 0 does not make sense.");

            var f = GetPolynom(nn);
            return new ScalarValue(f(x.Re));
        }

        [Description("Evaluates the Legendre polynomial of some order n at the given points in the matrix X in R.")]
        [Example("legendre(1, [-1, 0.5, 0, 0.5, 1])", "Evaluates the first Legendre polynomial (which is just x), at the points -1 to 1 with a spacing of 0.5.")]
        public MatrixValue Function(ScalarValue n, MatrixValue X)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Legendre polynomial of order n < 0 does not make sense.");

            var M = new MatrixValue(X.DimensionY, X.DimensionX);
            var f = GetPolynom(nn);

            for(var i = 1; i <= X.Length; i++)
                M[i] = new ScalarValue(f(X[i].Re));

            return M;
        }

        #region Polynomial

        public static Func<double, double> GetPolynom(int n)
        {
            switch (n)
            {
                case 0:
                    return x => 1.0;
                case 1:
                    return x => x;
                case 2:
                    return x => 1.5 * x * x - 0.5;
                case 3:
                    return x => 2.5 * x * x * x - 1.5 * x;
                case 4:
                    return x => (35.0 * x * x * x * x - 30.0 * x * x + 3.0) / 8.0;
                case 5:
                    return x => (63.0 * x * x * x * x * x - 70.0 * x * x * x + 15.0 * x) / 8.0;
                case 6:
                    return x => (231.0 * x * x * x * x * x * x - 315.0 * x * x * x * x + 105 * x * x - 5.0) / 16.0;
                default:
                    return x =>
                    {
                        var sum = 0.0;
                        var m = n / 2;

                        for(var k = 0; k <= m; k++)
                        {
                            var nom = Helpers.Factorial(2 * n - 2 * k) * Math.Pow(x, n - 2 * k);
                            var den = Helpers.Factorial(n - k) * Helpers.Factorial(n - 2 * k) * Helpers.Factorial(k) * Math.Pow(2, n);
                            sum += nom / den;
                        }

                        return sum;
                    };
            }
        }

        #endregion
    }
}
