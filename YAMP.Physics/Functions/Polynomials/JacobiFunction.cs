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
    [Description("In mathematics, Jacobi polynomials (occasionally called hypergeometric polynomials) are a class of classical orthogonal polynomials. They are orthogonal with respect to the weight in the interval [-1, 1].")]
    [Kind(PopularKinds.Function)]
    class JacobiFunction : ArgumentFunction
    {
        [Description("Evaluates the Jacobi polynomial of some order n with the parameters alpha and beta at the given point z in C.")]
        [Example("jacobi(1, 0, 2, 3.5)", "Evaluates the Jacobi polynomial of order 1 with alpha = 0 and beta = 2 at the point z = 3.5.")]
        public ScalarValue Function(ScalarValue n, ScalarValue alpha, ScalarValue beta, ScalarValue z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Jacobi polynomial of order n < 0 does not make sense.");

            return JacobiPolynomial(nn, alpha, beta, z);
        }

        [Description("Evaluates the Jacobi polynomial of some order n with the parameters alpha and beta at the given points of the matrix Z in C.")]
        [Example("jacobi(2, 1, 1, [-1, 0.5, 0, 0.5, 1])", "Evaluates the Jacobi polynomial of order 2 with alpha = 1 and beta = 1 at the points z = -1, 0.5, 0, 0.5 and 1.")]
        public MatrixValue Function(ScalarValue n, ScalarValue alpha, ScalarValue beta, MatrixValue Z)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);

            if (nn < 0)
                throw new Exception("Jacobi polynomial of order n < 0 does not make sense.");

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.Length; i++)
                M[i] = JacobiPolynomial(nn, alpha, beta, Z[i]);

            return M;
        }

        #region Algorithm

        /// <summary>
        /// Returns the JacobiP of order n with parameters alpha and beta of the specified number.
        /// </summary>
        /// <param name="n">Order</param>
        /// <param name="alpha">parameter alpha</param>
        /// <param name="beta">parameter beta</param>
        /// <param name="z">z</param>
        /// <returns>Value</returns>
        static ScalarValue JacobiPolynomial(int n, ScalarValue alpha, ScalarValue beta, ScalarValue z)
        {
            var m = n + 1;
            var gamma_alpha_m_1 = new ScalarValue[m];
            var gamma_alpha_beta_n_m_1 = new ScalarValue[m];
            gamma_alpha_m_1[0] = Gamma.LinearGamma(alpha + 1);
            gamma_alpha_beta_n_m_1[0] = Gamma.LinearGamma(alpha + beta + (n + 1));

            for (int k = 0; k < m - 1; k++)
            {
                gamma_alpha_m_1[k + 1] = (alpha + (k + 1)) * gamma_alpha_m_1[k];
                gamma_alpha_beta_n_m_1[k + 1] = (alpha + beta + (n + k + 1)) * gamma_alpha_beta_n_m_1[k];
            }

            var n_over_m = new int[m];

            for (int k = 0; k < n / 2 + 1; k++)
            {
                n_over_m[k] = 1;

                for (int i = 1; i <= k; i++)
                    n_over_m[k] *= (n - (k - i)) / i;

                n_over_m[n - k] = n_over_m[k];
            }

            double n_factorial = 1;

            for (int k = 2; k < m; k++)
                n_factorial *= k;

            var p = n_over_m[0] * gamma_alpha_beta_n_m_1[0] / gamma_alpha_m_1[0];
            var z_minus_1_over_2 = z; z_minus_1_over_2 -= 1; z_minus_1_over_2 /= 2;
            var z_minus_1_over_2_to_the_m = ScalarValue.One;

            for (int k = 1; k < m; k++)
            {
                z_minus_1_over_2_to_the_m *= z_minus_1_over_2;
                p += n_over_m[k] * gamma_alpha_beta_n_m_1[k] / gamma_alpha_m_1[k] * z_minus_1_over_2_to_the_m;
            }

            return p * gamma_alpha_m_1[n] / (gamma_alpha_beta_n_m_1[0] * n_factorial);
        }

        #endregion
    }
}
