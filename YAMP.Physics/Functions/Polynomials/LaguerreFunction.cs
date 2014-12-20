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
    [Description("In mathematics, the Laguerre polynomials are solutions of Laguerre's equation, which is a second-order linear differential equation. This equation has nonsingular solutions only if n is a non-negative integer. The Laguerre polynomials arise in quantum mechanics, in the radial part of the solution of the Schrödinger equation for a one-electron atom.")]
    [Kind(PopularKinds.Function)]
    class LaguerreFunction : ArgumentFunction
    {
        [Description("Evaluates the Laguerre polynomial of some order n with the parameter alpha at the given point z in C.")]
        [Example("laguerre(3, 1.0, 1.5)", "Evaluates the Laguerre polynomial of order 3 with alpha set to 1.0 at the point z = 1.5.")]
        public ScalarValue Function(ScalarValue n, ScalarValue alpha, ScalarValue z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Laguerre polynomial of order n < 0 does not make sense.");

            return LaguerrePolynomial(nn, alpha, z);
        }

        [Description("Evaluates the Laguerre polynomial of some order n with the parameter alpha at the given points of the matrix Z in C.")]
        [Example("laguerre(2, 1.0, [0.5, 1.5, 2.5])", "Evaluates the Laguerre polynomial of order 2 with alpha set to 1.0 at the points z = 0.5, 1.5 and 2.5.")]
        public MatrixValue Function(ScalarValue n, ScalarValue alpha, MatrixValue Z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Laguerre polynomial of order n < 0 does not make sense.");

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.Length; i++)
                M[i] = LaguerrePolynomial(nn, alpha, Z[i]);

            return M;
        }

        [Description("Evaluates the Laguerre polynomial of some order n with the parameter alpha set to 0 at the given point z in C.")]
        [Example("laguerre(3, 1.5)", "Evaluates the Laguerre polynomial of order 3 at the point z = 1.5.")]
        public ScalarValue Function(ScalarValue n, ScalarValue z)
        {
            return Function(n, ScalarValue.Zero, z);
        }

        [Description("Evaluates the Laguerre polynomial of some order n with the parameter alpha set to 0 at the given points of the matrix Z in C.")]
        [Example("laguerre(2, [0.5, 1.5, 2.5])", "Evaluates the Laguerre polynomial of order 2 at the points z = 0.5, 1.5 and 2.5")]
        public MatrixValue Function(ScalarValue n, MatrixValue Z)
        {
            return Function(n, ScalarValue.Zero, Z);
        }

        #region Algorithm

        /// <summary>
        /// Returns the Laguerre polynomial of order n with parameters alpha and beta of the specified number.
        /// </summary>
        /// <param name="n">Order</param>
        /// <param name="alpha">parameter alpha</param>
        /// <param name="z">z</param>
        /// <returns>Value</returns>
        static ScalarValue LaguerrePolynomial(int n, ScalarValue alpha, ScalarValue z)
        {
            var p = ScalarValue.Zero;
            var m = n + 1;
            var s = 1;

            for (int i = 0; i < m; i++)
            {
                p += s * Helpers.BinomialCoefficient(n + alpha, new ScalarValue(n - i)) * Helpers.Power(z, i) / Helpers.Factorial(i);
                s *= (-1);
            }

            return p;
        }

        #endregion
    }
}
