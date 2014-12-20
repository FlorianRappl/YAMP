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

namespace YAMP.Physics
{
    [Description("In mathematics, the Hurwitz zeta function, named after Adolf Hurwitz, is one of the many zeta functions. It is formally defined for complex arguments s with Re(s) > 1 and q with Re(q) > 0. This series is absolutely convergent for the given values of s and q and can be extended to a meromorphic function defined for all s ≠ 1. The Riemann zeta function is ζ(s, 1).")]
    [Link("http://en.wikipedia.org/wiki/Hurwitz_zeta_function")]
    [Kind(PopularKinds.Function)]
    class HzetaFunction : ArgumentFunction
    {
        [Description("Evaluates the Hurwitz Zeta function for a certain value s at the scalar argument q.")]
        [Example("hzeta(3, 1)", "Computes the Hzeta function with s = 3 at q = 1.")]
        public ScalarValue Function(ScalarValue s, ScalarValue q)
        {
            return HurwitzZeta(s, q);
        }

        [Description("Evaluates the Hurwitz Zeta function for a certain value s for each value of the matrix Q.")]
        [Example("hzeta(3, [0.5, 1, 1.5; 2, 2.5, 3])", "Computes the Hzeta function with s = 3 at z = 0.5, 1, 1.5, ..., 3.0. The result is given in form of a matrix with the same dimensions as Q.")]
        public MatrixValue Function(ScalarValue s, MatrixValue Q)
        {
            var M = new MatrixValue(Q.DimensionY, Q.DimensionX);

            for (var j = 1; j <= Q.DimensionY; j++)
                for (var i = 1; i <= Q.DimensionX; i++)
                    M[j, i] = HurwitzZeta(s, Q[j, i]);

            return M;
        }

        #region Constants

        static readonly double[] COEFFICIENTS = new[] 
        {
            1.00000000000000000000000000000,
            0.083333333333333333333333333333,
            -0.00138888888888888888888888888889,
            0.000033068783068783068783068783069,
            -8.2671957671957671957671957672e-07,
            2.0876756987868098979210090321e-08,
            -5.2841901386874931848476822022e-10,
            1.3382536530684678832826980975e-11,
            -3.3896802963225828668301953912e-13,
            8.5860620562778445641359054504e-15,
            -2.1748686985580618730415164239e-16,
            5.5090028283602295152026526089e-18,
            -1.3954464685812523340707686264e-19,
            3.5347070396294674716932299778e-21,
            -8.9535174270375468504026113181e-23
        };

        #endregion

        #region Algorithms

        public static double HurwitzZeta(double s, double q)
        {
            if (s <= 1.0)
                throw new YAMPArgumentRangeException("s", 1.0);

            if (q <= 0.0)
                throw new YAMPArgumentRangeException("q", 0.0);

            double max_bits = 54.0;
            double ln_term0 = -s * Math.Log(q);

            if ((s > max_bits && q < 1.0) || (s > 0.5 * max_bits && q < 0.25))
                return Math.Pow(q, -s);
            else if (s > 0.5 * max_bits && q < 1.0)
            {
                double p1 = Math.Pow(q, -s);
                double p2 = Math.Pow(q / (1.0 + q), s);
                double p3 = Math.Pow(q / (2.0 + q), s);
                return p1 * (1.0 + p2 + p3);
            }

            /* Euler-Maclaurin summation formula 
             * [Moshier, p. 400, with several typo corrections]
             */
            const int jmax = 12;
            const int kmax = 10;

            double pmax = Math.Pow(kmax + q, -s);
            double scp = s;
            double pcp = pmax / (kmax + q);
            double ans = pmax * ((kmax + q) / (s - 1.0) + 0.5);

            for (var k = 0; k < kmax; k++)
                ans += Math.Pow(k + q, -s);

            for (var j = 0; j <= jmax; j++)
            {
                double delta = COEFFICIENTS[j + 1] * scp * pcp;
                ans += delta;

                if (Math.Abs(delta / ans) < 0.5 * double.Epsilon)
                    break;

                scp *= (s + 2 * j + 1) * (s + 2 * j + 2);
                pcp /= (kmax + q) * (kmax + q);
            }

            return ans;
        }

        public static ScalarValue HurwitzZeta(double s, ScalarValue q)
        {
            return HurwitzZeta(new ScalarValue(s), q);
        }

        public static ScalarValue HurwitzZeta(ScalarValue s, ScalarValue q)
        {
            if (s.Re <= 1.0)
                throw new YAMPArgumentRangeException("s", 1.0);

            if (q.Re <= 0.0)
                throw new YAMPArgumentRangeException("q", 0.0);

            double max_bits = 54.0;
            var ln_term0 = -s * q.Log();
            var qabs = q.Abs();
            var sabs = s.Abs();
            var ss = s;

            if ((sabs > max_bits && qabs < 1.0) || (sabs > 0.5 * max_bits && qabs < 0.25))
                return q.Pow(-ss);
            else if (sabs > 0.5 * max_bits && qabs < 1.0)
            {
                var p1 = q.Pow(-ss);
                var p2 = (q / (1.0 + q)).Pow(ss);
                var p3 = (q / (2.0 + q)).Pow(ss);
                return p1 * (1.0 + p2 + p3);
            }

            /* Euler-Maclaurin summation formula 
             * [Moshier, p. 400, with several typo corrections]
             */
            const int jmax = 12;
            const int kmax = 10;

            var pmax = (kmax + q).Pow(-ss);
            var scp = s;
            var pcp = pmax / (kmax + q);
            var ans = pmax * ((kmax + q) / (s - 1.0) + 0.5);

            for (var k = 0; k < kmax; k++)
                ans += (k + q).Pow(-ss);

            for (var j = 0; j <= jmax; j++)
            {
                var delta = COEFFICIENTS[j + 1] * scp * pcp;
                ans += delta;

                if ((delta / ans).Abs() < 0.5 * double.Epsilon)
                    break;

                scp *= (s + 2 * j + 1) * (s + 2 * j + 2);
                pcp /= (kmax + q) * (kmax + q);
            }

            return ans;
        }

        #endregion
    }
}
