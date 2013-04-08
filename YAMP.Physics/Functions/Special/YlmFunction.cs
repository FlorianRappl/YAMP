/*
    Copyright (c) 2012, Florian Rappl.
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
    [Description("In mathematics, spherical harmonics are the angular portion of a set of solutions to Laplace's equation. Represented in a system of spherical coordinates, Laplace's spherical harmonics  are a specific set of spherical harmonics that forms an orthogonal system. Spherical harmonics are important in many theoretical and practical applications, particularly in the computation of atomic orbital electron configurations, representation of gravitational fields, geoids, and the magnetic fields of planetary bodies and stars, and characterization of the cosmic microwave background radiation.")]
    [Kind(PopularKinds.Function)]
    class YlmFunction : ArgumentFunction
    {
        [Description("Computes the spherical harmonics at given l, m with values for theta and phi. This results in the value of Ylm with the given l, m at the given angles.")]
        [Example("ylm(0, 0, 0, 0)", "Computes the spherical harmonics Ylm(theta, phi) at l = 0, m = 0 - which gives the constant expression 1/2 * sqrt(1/pi) independent of theta and phi.")]
        [Example("ylm(1, 1, pi / 2, 0)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = pi / 2 and phi = 0.")]
        public ScalarValue Function(ScalarValue l, ScalarValue m, ScalarValue theta, ScalarValue phi)
        {
            var nn = l.GetIntegerOrThrowException("l", Name);

            if (nn < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var nm = m.GetIntegerOrThrowException("m", Name);
            return Ylm(nn, nm, theta.Re, phi.Re);
        }

        [Description("Computes the spherical harmonics at given l, m with multiple values for theta and one value for phi. This results in a matrix of Ylm values for the given l, m at the given angles. The matrix has the dimension of theta.")]
        [Example("ylm(1, 1, [-pi / 2, pi / 2; 0, pi], 0)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = a 2 x2 matrix and phi = 0.")]
        public MatrixValue Function(ScalarValue l, ScalarValue m, MatrixValue theta, ScalarValue phi)
        {
            var nn = l.GetIntegerOrThrowException("l", Name);

            if (nn < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var nm = m.GetIntegerOrThrowException("m", Name);
            var M = new MatrixValue(theta.DimensionY, theta.DimensionX);

            for(var i = 1; i <= theta.DimensionX; i++)
                for(var j = 1; j <= theta.DimensionY; j++)
                    M[j, i] = Ylm(nn, nm, theta[j, i].Re, phi.Re);

            return M;
        }

        [Description("Computes the spherical harmonics at given l, m with one value for theta and multiple values for phi. This results in a matrix of Ylm values for the given l, m at the given angles. The matrix has the dimension of phi.")]
        [Example("ylm(1, 1, pi / 2, 0 : pi / 10 : pi)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = pi / 2 and phi being a vector from 0 to pi with a spacing of pi / 10.")]
        public MatrixValue Function(ScalarValue l, ScalarValue m, ScalarValue theta, MatrixValue phi)
        {
            var nn = l.GetIntegerOrThrowException("l", Name);

            if (l.IntValue < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var nm = m.GetIntegerOrThrowException("m", Name);
            var M = new MatrixValue(phi.DimensionY, phi.DimensionX);

            for (var i = 1; i <= phi.DimensionX; i++)
                for (var j = 1; j <= phi.DimensionY; j++)
                    M[j, i] = Ylm(nn, nm, theta.Re, phi[j, i].Re);

            return M;
        }

        [Description("Computes the spherical harmonics at given l, m with multiple values for theta and phi. This results in a matrix of Ylm values for the given l, m at the given angles. The matrix has dimension of the length of theta times the length of phi.")]
        [Example("ylm(1, 1, [-pi / 2, pi / 2; 0, pi], 0 : pi / 10 : pi)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = a 2 x2 matrix and phi = 0 : pi / 10 : pi.")]
        public MatrixValue Function(ScalarValue l, ScalarValue m, MatrixValue theta, MatrixValue phi)
        {
            var nn = l.GetIntegerOrThrowException("l", Name);

            if (nn < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var M = new MatrixValue(theta.Length, phi.Length);
            var nm = m.GetIntegerOrThrowException("m", Name);

            for (var i = 1; i <= phi.Length; i++)
                for (var j = 1; j <= theta.Length; j++)
                    M[j, i] = Ylm(nn, nm, theta[j].Re, phi[i].Re);

            return M;
        }

        #region Algorithms

        public static ScalarValue Ylm(int l, int m, double theta, double phi)
        {
            var expphi = new ScalarValue(0.0, m * phi).Exp();
            var factor = m < 0 ? Math.Pow(-1, -m) : 1.0;
            var legend = Plm(l, Math.Abs(m), Math.Cos(theta));
            return factor * expphi * legend;
        }

        static double Pl(int l, double x)
        {
            if (l == 0)
                return 1.0;
            else if (l == 1)
                return x;
            else if (l == 2)
                return 0.5 * (3.0 * x * x - 1.0);
            else if (x == 1.0)
                return 1.0;
            else if (x == -1.0)
                return (l % 2 == 1 ? -1.0 : 1.0);
            else if (l < 100000)
            {
                /* upward recurrence: l P_l = (2l-1) z P_{l-1} - (l-1) P_{l-2} */

                var p_ellm2 = 1.0;    /* P_0(x) */
                var p_ellm1 = x;      /* P_1(x) */
                var p_ell = p_ellm1;
                var e_ellm2 = double.Epsilon;
                var e_ellm1 = Math.Abs(x) * double.Epsilon;
                var e_ell = e_ellm1;

                for (int ell = 2; ell <= l; ell++)
                {
                    p_ell = (x * (2 * ell - 1) * p_ellm1 - (ell - 1) * p_ellm2) / ell;
                    p_ellm2 = p_ellm1;
                    p_ellm1 = p_ell;
                    e_ell = 0.5 * (Math.Abs(x) * (2 * ell - 1.0) * e_ellm1 + (ell - 1.0) * e_ellm2) / ell;
                    e_ellm2 = e_ellm1;
                    e_ellm1 = e_ell;
                }

                return p_ell;
            }

            /* 
             * Asymptotic expansion.
             * [Olver, p. 473]
             */
            double u = l + 0.5;
            double th = Math.Acos(x);
            double J0 = Bessel.j0(u * th);
            double Jm1 = Bessel.jn(-1, u * th);
            double pre;
            double B00;
            double c1;

            /* 
             * B00 = 1/8 (1 - th cot(th) / th^2
             * pre = sqrt(th/sin(th))
             */
            if (th < 1.2207031250000000e-04)
            {
                B00 = (1.0 + th * th / 15.0) / 24.0;
                pre = 1.0 + th * th / 12.0;
            }
            else
            {
                double sin_th = Math.Sqrt(1.0 - x * x);
                double cot_th = x / sin_th;
                B00 = 1.0 / 8.0 * (1.0 - th * cot_th) / (th * th);
                pre = Math.Sqrt(th / sin_th);
            }

            c1 = th / u * B00;
            return pre * (J0 + c1 * Jm1);
        }

        static double Plm(int l, int m, double x)
        {
            if (m == 0)
            {
                var res = Pl(l, x);
                var pre = Math.Sqrt((2.0 * l + 1.0) / (4.0 * Math.PI));
                return pre * res;
            }
            else if (x == 1.0 || x == -1.0)
                return 0.0;

            //Y_m^m(x) = sqrt( (2m+1)/(4pi m) gamma(m+1/2)/gamma(m) ) (-1)^m (1-x^2)^(m/2) / pi^(1/4)
            double sgn = m % 2 == 1 ? -1.0 : 1.0;
            double y_mmp1_factor = x * Math.Sqrt(2.0 * m + 3.0);
            double lncirc = Math.Log(1 - x * x);
            double lnpoch = Gamma.LogGamma(m + 0.5) - Gamma.LogGamma(m);
            double expf = Math.Exp(0.5 * (lnpoch + m * lncirc) - 0.25 * Helpers.LogPI);
            double sr = Math.Sqrt((2.0 + 1.0 / m) / Helpers.FourPI);
            double y_mm = sgn * sr * expf;
            double y_mmp1 = y_mmp1_factor * y_mm;

            if (l == m)
                return y_mm;
            else if (l == m + 1)
                return y_mmp1;

            var y_ell = 0.0;

            /* Compute Y_l^m, l > m + 1, upward recursion on l. */
            for (int ell = m + 2; ell <= l; ell++)
            {
                var rat1 = (double)(ell - m) / (double)(ell + m);
                var rat2 = (ell - m - 1.0) / (ell + m - 1.0);
                var factor1 = Math.Sqrt(rat1 * (2.0 * ell + 1.0) * (2.0 * ell - 1.0));
                var factor2 = Math.Sqrt(rat1 * rat2 * (2.0 * ell + 1.0) / (2.0 * ell - 3.0));
                y_ell = (x * y_mmp1 * factor1 - (ell + m - 1.0) * y_mm * factor2) / (ell - m);
                y_mm = y_mmp1;
                y_mmp1 = y_ell;
            }

            return y_ell;
        }

        #endregion
    }
}
