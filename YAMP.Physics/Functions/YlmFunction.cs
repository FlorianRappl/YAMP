using System;
using YAMP;
using YAMP.Numerics;

namespace YAMP.Physics
{
    [Description("In mathematics, spherical harmonics are the angular portion of a set of solutions to Laplace's equation. Represented in a system of spherical coordinates, Laplace's spherical harmonics  are a specific set of spherical harmonics that forms an orthogonal system. Spherical harmonics are important in many theoretical and practical applications, particularly in the computation of atomic orbital electron configurations, representation of gravitational fields, geoids, and the magnetic fields of planetary bodies and stars, and characterization of the cosmic microwave background radiation.")]
    [Kind(PopularKinds.Function)]
    class YlmFunction : ArgumentFunction
    {
        static readonly double lnpi = Math.Log(Math.PI);
        static readonly double fourpi = (4.0 * Math.PI);

        [Description("Computes the spherical harmonics at given l, m with values for theta and phi. This results in the value of Ylm with the given l, m at the given angles.")]
        [Example("ylm(0, 0, 0, 0)", "Computes the spherical harmonics Ylm(theta, phi) at l = 0, m = 0 - which gives the constant expression 1/2 * sqrt(1/pi) independent of theta and phi.")]
        [Example("ylm(1, 1, pi / 2, 0)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = pi / 2 and phi = 0.")]
        public ScalarValue Function(ScalarValue l, ScalarValue m, ScalarValue theta, ScalarValue phi)
        {
            if (l.IntValue < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            return new ScalarValue(Ylm(l.IntValue, m.IntValue, theta.Value, phi.Value));
        }

        [Description("Computes the spherical harmonics at given l, m with multiple values for theta and one value for phi. This results in a matrix of Ylm values for the given l, m at the given angles. The matrix has the dimension of theta.")]
        [Example("ylm(1, 1, [-pi / 2, pi / 2; 0, pi], 0)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = a 2 x2 matrix and phi = 0.")]
        public MatrixValue Function(ScalarValue l, ScalarValue m, MatrixValue theta, ScalarValue phi)
        {
            if (l.IntValue < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var M = new MatrixValue(theta.DimensionY, theta.DimensionX);

            for(var i = 1; i <= theta.DimensionX; i++)
                for(var j = 1; j <= theta.DimensionY; j++)
                    M[j, i] = new ScalarValue(Ylm(l.IntValue, m.IntValue, theta[j, i].Value, phi.Value));

            return M;
        }

        [Description("Computes the spherical harmonics at given l, m with one value for theta and multiple values for phi. This results in a matrix of Ylm values for the given l, m at the given angles. The matrix has the dimension of phi.")]
        [Example("ylm(1, 1, pi / 2, 0 : pi / 10 : pi)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = pi / 2 and phi being a vector from 0 to pi with a spacing of pi / 10.")]
        public MatrixValue Function(ScalarValue l, ScalarValue m, ScalarValue theta, MatrixValue phi)
        {
            if (l.IntValue < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var M = new MatrixValue(phi.DimensionY, phi.DimensionX);

            for (var i = 1; i <= phi.DimensionX; i++)
                for (var j = 1; j <= phi.DimensionY; j++)
                    M[j, i] = new ScalarValue(Ylm(l.IntValue, m.IntValue, theta.Value, phi[j, i].Value));

            return M;
        }

        [Description("Computes the spherical harmonics at given l, m with multiple values for theta and phi. This results in a matrix of Ylm values for the given l, m at the given angles. The matrix has dimension of the length of theta times the length of phi.")]
        [Example("ylm(1, 1, [-pi / 2, pi / 2; 0, pi], 0 : pi / 10 : pi)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = a 2 x2 matrix and phi = 0 : pi / 10 : pi.")]
        public MatrixValue Function(ScalarValue l, ScalarValue m, MatrixValue theta, MatrixValue phi)
        {
            if (l.IntValue < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            var M = new MatrixValue(theta.Length, phi.Length);

            for (var i = 1; i <= phi.Length; i++)
                for (var j = 1; j <= theta.Length; j++)
                    M[j, i] = new ScalarValue(Ylm(l.IntValue, m.IntValue, theta[j].Value, phi[i].Value));

            return M;
        }

        //
        // General formulas
        //
        // ylm(0, 0, theta, phi) = 1/2 * sqrt(1/pi)
        // ylm(1, 0, theta, phi) = 1/2 * sqrt(3/pi) * cos(theta)
        // ylm(1, -1, theta, phi) = 1/2 * sqrt(3/(2*pi)) * exp(-i*phi) * sin(theta)
        // ylm(1, 1, theta, phi) = -1/2 * sqrt(3/(2*pi)) * exp(i*phi) * sin(theta)
        // die negativen m sind gleich zu den positiven m, nur mit einem vorzeichen (-1)^m und entsprechenden exp(- arg)
        // ylm(2, 0, theta, phi) = 1/4 * sqrt(5/pi) * (3*cos(theta)^2 - 1)
        // ylm(2, 1, theta, phi) = -1/2 * sqrt(15/2/pi) * exp(i * phi) * sin(theta) * cos(theta)
        // ylm(2, 2, theta, phi) = 1/4 * sqrt(15/2/pi) * exp(2*i*phi) * sin(theta)^2
        // ylm(3, 0, theta, phi) = 1/4 * sqrt(7/pi) * (5 * cos(theta)^3 - 3 * cos(theta))
        // ylm(3, 1, theta, phi) = -1/8 * sqrt(21/pi) * exp(i * phi) * sin(theta) * (5 * cos(theta)^2 - 1)
        // ylm(3, 2, theta, phi) = 1/4 * sqrt(105/2/pi) * exp(2 * i * phi) * sin(theta)^2 * cos(theta)
        // ....
        // Examples with values for theta and phi
        //
        // ylm(0, 0, 0, 0) = 1/2 * sqrt(1/pi) ~ 0.28
        // ylm(1, 0, 0, 0) = 1/2 * sqrt(3/pi) ~ 0.48
        // ylm(1, -1, pi / 2, 0) = 1/2 * sqrt(3/(2*pi)) ~ 0.34
        // ylm(1, 1, pi / 2, 0) = -1/2 * sqrt(3/(2*pi)) ~ -0.34
        // ylm(2, 0, 0, 0) = 1/4 * sqrt(5/pi) * 2 ~ 0.63
        // ylm(2, 1, pi / 4, 0) = -1/2 * sqrt(15/2/pi) * sin(pi / 4) * cos(pi / 4) ~ 0.38
        // ylm(2, 2, pi / 2, 0) = 1/4 * sqrt(15/2/pi) ~ 0.38
        // ylm(3, 0, 0, 0) = 1/4 * sqrt(7/pi) * 2 ~ 0.74
        // ylm(3, 1, pi / 4, 0) = -1/8 * sqrt(21/pi) * sin(pi / 4) * (5 * cos(pi / 4)^2 - 1) ~ -0.34
        // ylm(3, 2, pi / 4, 0) = 1/4 * sqrt(105/2/pi) * sin(pi / 4)^2 * cos(pi / 4) ~ 0.36
        //
        static ScalarValue Ylm(int l, int m, double theta, double phi)
        {
            var expphi = new ScalarValue(0.0, m * phi).Exp();
            var factor = m < 0 ? Math.Pow(-1, -m) : 1.0;
            var legend = Plm(l, Math.Abs(m), Math.Cos(theta));
            return factor * expphi * legend;
        }

        static double sqrt(double x)
        {
            return Math.Sqrt(x);
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
            else
            {
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

                /* B00 = 1/8 (1 - th cot(th) / th^2
                 * pre = sqrt(th/sin(th))
                 */
                if (th < 1.2207031250000000e-04)
                {
                    B00 = (1.0 + th * th / 15.0) / 24.0;
                    pre = 1.0 + th * th / 12.0;
                }
                else
                {
                    double sin_th = sqrt(1.0 - x * x);
                    double cot_th = x / sin_th;
                    B00 = 1.0 / 8.0 * (1.0 - th * cot_th) / (th * th);
                    pre = sqrt(th / sin_th);
                }

                c1 = th / u * B00;
                return pre * (J0 + c1 * Jm1);
            }
        }

        static double Plm(int l, int m, double x)
        {
            if (m == 0)
            {
                var res = Pl(l, x);
                var pre = sqrt((2.0 * l + 1.0) / (4.0 * Math.PI));
                return pre * res;
            }
            else if (x == 1.0 || x == -1.0)
            {
                return 0.0;
            }
            else /* m > 0 and |x| < 1 here */
            { 
                //Y_m^m(x) = sqrt( (2m+1)/(4pi m) gamma(m+1/2)/gamma(m) ) (-1)^m (1-x^2)^(m/2) / pi^(1/4)
                double sgn = m % 2 == 1 ? -1.0 : 1.0;
                double y_mmp1_factor = x * sqrt(2.0 * m + 3.0);
                double lncirc = Math.Log(1 - x * x);
                double lnpoch = Gamma.LogGamma(m + 0.5) - Gamma.LogGamma(m);
                double expf = Math.Exp(0.5 * (lnpoch + m * lncirc) - 0.25 * lnpi);
                double sr = sqrt((2.0 + 1.0 / m) / fourpi);
                double y_mm = sgn * sr * expf;
                double y_mmp1 = y_mmp1_factor * y_mm;

                if (l == m)
                    return y_mm;
                else if (l == m + 1)
                    return y_mmp1;
                else
                {
                    var y_ell = 0.0;

                    /* Compute Y_l^m, l > m+1, upward recursion on l. */
                    for (int ell = m + 2; ell <= l; ell++)
                    {
                        var rat1 = (double)(ell - m) / (double)(ell + m);
                        var rat2 = (ell - m - 1.0) / (ell + m - 1.0);
                        var factor1 = sqrt(rat1 * (2.0 * ell + 1.0) * (2.0 * ell - 1.0));
                        var factor2 = sqrt(rat1 * rat2 * (2.0 * ell + 1.0) / (2.0 * ell - 3.0));
                        y_ell = (x * y_mmp1 * factor1 - (ell + m - 1.0) * y_mm * factor2) / (ell - m);
                        y_mm = y_mmp1;
                        y_mmp1 = y_ell;
                    }

                    return y_ell;
                }
            }
        }
    }
}
