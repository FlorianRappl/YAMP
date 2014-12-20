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
    [Description("In mathematics, the polygamma function of order m is a meromorphic function on C and defined as the (m+1)-th derivative of the logarithm of the gamma function.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Polygamma")]
    class PsiFunction : ArgumentFunction
    {
        [Description("Evaluates the polylogarithm for a certain integer n at a scalar argument z.")]
        [Example("psi(3, 1)", "Computes the polylogarithm with n = 3 at z = 1. The result is 3! * Hzeta(4, 1) or pi^4 / 15.")]
        public ScalarValue Function(ScalarValue n, ScalarValue z)
        {
            var m = n.GetIntegerOrThrowException("n", Name);
            return Psi(m, z);
        }

        [Description("Evaluates the polylogarithm for a certain integer n for each value of the matrix Z.")]
        [Example("psi(3, [0.5, 1, 1.5; 2, 2.5, 3])", "Computes the polylogarithm with n = 3 at z = 0.5, 1, 1.5, ..., 3.0. The result is given in form of a matrix with the same dimensions as Z.")]
        public MatrixValue Function(ScalarValue n, MatrixValue Z)
        {
            var m = n.GetIntegerOrThrowException("n", Name);
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var j = 1; j <= Z.DimensionY; j++)
                for (var i = 1; i <= Z.DimensionX; i++)
                    M[j, i] = Psi(m, Z[j, i]);

            return M;
        }

        #region Number

        static readonly double[] psics_data = new double[] {
           -.038057080835217922,
            .491415393029387130, 
           -.056815747821244730,
            .008357821225914313,
           -.001333232857994342,
            .000220313287069308,
           -.000037040238178456,
            .000006283793654854,
           -.000001071263908506,
            .000000183128394654,
           -.000000031353509361,
            .000000005372808776,
           -.000000000921168141,
            .000000000157981265,
           -.000000000027098646,
            .000000000004648722,
           -.000000000000797527,
            .000000000000136827,
           -.000000000000023475,
            .000000000000004027,
           -.000000000000000691,
            .000000000000000118,
           -.000000000000000020
        };

        static readonly double[] apsics_data = new double[] {    
           -.0204749044678185,
           -.0101801271534859,
            .0000559718725387,
           -.0000012917176570,
            .0000000572858606,
           -.0000000038213539,
            .0000000003397434,
           -.0000000000374838,
            .0000000000048990,
           -.0000000000007344,
            .0000000000001233,
           -.0000000000000228,
            .0000000000000045,
           -.0000000000000009,
            .0000000000000002,
           -.0000000000000000 
         };

        #endregion

        #region Helpers

        static Helpers.ChebSeries psi_cs = new Helpers.ChebSeries
        {
            Coefficients = psics_data,
            Order = 22,
            LowerPoint = -1,
            UpperPoint = 1,
            SinglePrecisionOrder = 17
        };

        static Helpers.ChebSeries apsi_cs = new Helpers.ChebSeries
        {
            Coefficients = apsics_data,
            Order = 15,
            LowerPoint = -1,
            UpperPoint = 1,
            SinglePrecisionOrder = 9
        };

        #endregion

        #region Algorithms

        public static ScalarValue Psi0(ScalarValue z)
        {
            if (z.Re >= 0.0)
                return Psi0rhp(z);

            /* reflection formula [Abramowitz+Stegun, 6.3.7] */
            var omz = new ScalarValue(1.0 - z.Re, -z.Im);
            var zpi = z * Math.PI;
            var cotzpi = zpi.Cot();
            var result = Psi0rhp(omz);
            result.Re -= Math.PI * cotzpi.Re;
            result.Im -= Math.PI * cotzpi.Im;
            return result;
        }

        public static double Psi(int n, double x)
        {
            if (n < 0)
                throw new YAMPArgumentRangeException("n", 0.0);

            if (x <= 0.0)
                throw new YAMPArgumentRangeException("x", 0.0);

            if (n == 0)
                return Gamma.Psi(x);
            else if (n == 1)
                return Psi1(x);

            var hzeta = HzetaFunction.HurwitzZeta(n + 1.0, x);
            var ln_nf = Helpers.Factorial(n);
            var result = hzeta * ln_nf;

            if (n % 2 == 0)
                result = -result;

            Helpers.Factorial(1);

            return result;
        }

        public static ScalarValue Psi(int n, ScalarValue z)
        {
            if (n < 0)
                throw new YAMPArgumentRangeException("n", 0.0);

            if (z.Re <= 0.0 && z.Im == 0)
                return ScalarValue.RealInfinity;

            if (n == 0)
                return Psi0(z);
            else if (n == 1)
                return Psi1(z);

            var hzeta = HzetaFunction.HurwitzZeta(n + 1.0, z);
            var ln_nf = Helpers.Factorial(n);
            var result = hzeta * ln_nf;

            if (n % 2 == 0)
                result = -result;

            Helpers.Factorial(1);
            return result;
        }

        public static double Psi1(double x)
        {
            if (x == 0.0 || x == -1.0 || x == -2.0)
                throw new YAMPArgumentRangeException("x", "All values except 0.0, -1.0, -2.0");

            if (x > 0.0)
                return PsiXgt0(1, x);
            else if (x > -5.0)
            {
                /* Abramowitz + Stegun 6.4.6 */
                int M = -((int)(Math.Floor(x)));
                var fx = x + M;
                var sum = 0.0;

                if (fx == 0.0)
                    throw new YAMPNotConvergedException("psi");

                for (int m = 0; m < M; ++m)
                {
                    var xm = x + m;
                    sum += 1.0 / (xm * xm);
                }

                return PsiXgt0(1, fx) + sum;
            }

            /* Abramowitz + Stegun 6.4.7 */
            var sin_px = Math.Sin(Math.PI * x);
            var d = Math.PI * Math.PI / (sin_px * sin_px);
            return d - PsiXgt0(1, 1.0 - x);
        }

        public static ScalarValue Psi1(ScalarValue z)
        {
            if (z.Re == 0.0 || z.Re == -1.0 || z.Re == -2.0)
                throw new YAMPArgumentRangeException("z", "All values except 0.0, -1.0, -2.0");

            if (z.Re > 0.0)
                return PsiXgt0(1, z);
            else if (z.Re > -5.0)
            {
                /* Abramowitz + Stegun 6.4.6 */
                int M = -((int)(Math.Floor(z.Re)));
                var fx = z + M;
                var sum = ScalarValue.Zero;

                if (fx == 0.0)
                    throw new YAMPNotConvergedException("psi");

                for (int m = 0; m < M; ++m)
                {
                    var xm = z + m;
                    sum += 1.0 / (xm * xm);
                }

                return PsiXgt0(1, fx) + sum;
            }

            /* Abramowitz + Stegun 6.4.7 */
            var sin_px = (Math.PI * z).Sin();
            var d = Math.PI * Math.PI / (sin_px * sin_px);
            return d - PsiXgt0(1, 1.0 - z);
        }

        static double PsiXgt0(int n, double x)
        {
            if (n == 0)
                return Gamma.Psi(x);

            /* Abramowitz + Stegun 6.4.10 */
            var hzeta = HzetaFunction.HurwitzZeta(n + 1.0, x);
            var ln_nf = Helpers.Factorial(n);
            var result = hzeta * ln_nf;

            if (n % 2 == 0)
                result = -result;

            return result;
        }

        static ScalarValue PsiXgt0(int n, ScalarValue z)
        {
            if (n == 0)
                return Psi0(z);

            /* Abramowitz + Stegun 6.4.10 */
            var hzeta = HzetaFunction.HurwitzZeta(n + 1.0, z);
            var ln_nf = Helpers.Factorial(n);
            var result = hzeta * ln_nf;

            if (n % 2 == 0)
                result = -result;

            return result;
        }

        static ScalarValue Psi0rhp(ScalarValue z)
        {
            int n_recurse = 0;
            int i;

            if (z.Re == 0.0 && z.Im == 0.0)
                return ScalarValue.Zero;

            /* compute the number of recurrences to apply */
            if (z.Re < 20.0 && Math.Abs(z.Im) < 20.0)
            {
                double sp = Math.Sqrt(20.0 + z.Im);
                double sn = Math.Sqrt(20.0 - z.Im);
                double rhs = sp * sn - z.Re;

                if (rhs > 0.0)
                    n_recurse = (int)(Math.Ceiling(rhs));
            }

            /* compute asymptotic at the large value z + n_recurse */
            var a = Psi0asymptotic(z + n_recurse);

            /* descend recursively, if necessary */
            for (i = n_recurse; i >= 1; --i)
            {
                var zn = z + (i - 1.0);
                var zn_inverse = 1.0 / zn;
                a = a - zn_inverse;
            }

            return a;
        }

        static ScalarValue Psi0asymptotic(ScalarValue z)
        {
            /* coefficients in the asymptotic expansion for large z;
             * let w = z^(-2) and write the expression in the form
             *
             *   ln(z) - 1/(2z) - 1/12 w (1 + c1 w + c2 w + c3 w + ... )
             */
            const double c1 = -0.1;
            const double c2 = 1.0 / 21.0;
            const double c3 = -0.05;
            const double c4 = -1.0 / 12.0;

            var zi = 1.0 / z;
            var w = zi * zi;

            /* Horner method evaluation of term in parentheses */
            var sum = (w * (c3 / c2)) + 1.0;
            sum = sum * (c2 / c1);
            sum = ((sum * w) + 1.0) * c1 * w;
            sum += 1.0;

            /* correction added to log(z) */
            var cs = sum * w;
            cs = cs * c4;
            cs = cs + (zi * (-0.5));

            return z.Log() + cs;
        }

        #endregion
    }
}
