using System;
using YAMP;
using YAMP.Numerics;

namespace YAMP.Physics
{
    [Description("In mathematics, spherical harmonics are the angular portion of a set of solutions to Laplace's equation. Represented in a system of spherical coordinates, Laplace's spherical harmonics  are a specific set of spherical harmonics that forms an orthogonal system. Spherical harmonics are important in many theoretical and practical applications, particularly in the computation of atomic orbital electron configurations, representation of gravitational fields, geoids, and the magnetic fields of planetary bodies and stars, and characterization of the cosmic microwave background radiation.")]
    [Kind(PopularKinds.Function)]
    class YlmFunction : ArgumentFunction
    {
        [Description("Computes the spherical harmonics at given l, m with values for theta and phi.")]
        [Example("ylm(1, 1, pi/2, 0)", "Evaluates the spherical harmonics Ylm(theta ,phi) with l = 1, m = 1, theta = pi / 2 and phi = 0.")]
        public ScalarValue Function(ScalarValue l, ScalarValue m, ScalarValue theta, ScalarValue phi)
        {
            if (l.IntValue < 0)
                throw new Exception("Spherical harmonics of order l < 0 does not make sense.");

            return new ScalarValue(Ylm(l.IntValue, m.IntValue, theta.Value, phi.Value));
        }

        //TODO Somethings still wrong / fishy...
        //Check with http://en.wikipedia.org/wiki/Table_of_spherical_harmonics
        static ScalarValue Ylm(int l, int m, double theta, double phi)
        {
            var expphi = new ScalarValue(0.0, m * phi).Exp();
            var factor = Math.Sqrt((2 * l + 1) * Helpers.Factorial(l - m) / Helpers.Factorial(l + m));
            var legend = Plm(l, m, Math.Cos(theta));
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
                /* 
                 * Starting value for recursion.
                 * Y_m^m(x) = sqrt( (2m+1)/(4pi m) gamma(m+1/2)/gamma(m) ) (-1)^m (1-x^2)^(m/2) / pi^(1/4)
                 */
                double sgn = m % 2 == 1 ? -1.0 : 1.0;
                double y_mmp1_factor = x * sqrt(2.0 * m + 3.0);
                double lncirc = Math.Log(1 - x * x);
                double lnpoch = -Math.Log(Gamma.LinearGamma(m + 0.5) / Gamma.LinearGamma(m));  /* Gamma(m+1/2)/Gamma(m) */
                double lnpre_val = -0.25 * Math.Log(Math.PI) + 0.5 * (lnpoch + m * lncirc);

                /* Compute exp(ln_pre) with error term, avoiding call to gsl_sf_exp_err BJG */
                double ex_pre = Math.Exp(lnpre_val);
                double sr = sqrt((2.0 + 1.0 / m) / (4.0 * Math.PI));
                double y_mm = sgn * sr * ex_pre;
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

        /// <summary>
        /// Calculate P_m^m(x) from the analytic result: 
        /// P_m^m(x) = (-1)^m (2m-1)!! (1-x^2)^(m/2) , m > 0
        ///          = 1, m = 0
        /// </summary>
        /// <param name="m">order m</param>
        /// <param name="x">argument x</param>
        /// <returns></returns>
        static double Pmm(int m, double x)
        {
            if (m == 0)
                return 1.0;
            else
            {
                var p_mm = 1.0;
                var root_factor = sqrt(1.0 - x) * sqrt(1.0 + x);
                var fact_coeff = 1.0;

                for (int i = 1; i <= m; i++)
                {
                    p_mm *= -fact_coeff * root_factor;
                    fact_coeff += 2.0;
                }

                return p_mm;
            }
        }
    }
}
