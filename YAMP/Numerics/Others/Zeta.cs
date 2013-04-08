using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Provides access to the useful Riemann-Zeta function.
    /// </summary>
    public static class Zeta
    {
        #region Constants

        static readonly double[] zetaGreater1 = new double[] {
           19.3918515726724119415911269006,
            9.1525329692510756181581271500,
            0.2427897658867379985365270155,
           -0.1339000688262027338316641329,
            0.0577827064065028595578410202,
           -0.0187625983754002298566409700,
            0.0039403014258320354840823803,
           -0.0000581508273158127963598882,
           -0.0003756148907214820704594549,
            0.0001892530548109214349092999,
           -0.0000549032199695513496115090,
            8.7086484008939038610413331863e-6,
            6.4609477924811889068410083425e-7,
           -9.6749773915059089205835337136e-7,
            3.6585400766767257736982342461e-7,
           -8.4592516427275164351876072573e-8,
            9.9956786144497936572288988883e-9,
            1.4260036420951118112457144842e-9,
           -1.1761968823382879195380320948e-9,
            3.7114575899785204664648987295e-10,
           -7.4756855194210961661210215325e-11,
            7.8536934209183700456512982968e-12,
            9.9827182259685539619810406271e-13,
           -7.5276687030192221587850302453e-13,
            2.1955026393964279988917878654e-13,
           -4.1934859852834647427576319246e-14,
            4.6341149635933550715779074274e-15,
            2.3742488509048340106830309402e-16,
           -2.7276516388124786119323824391e-16,
            7.8473570134636044722154797225e-17
        };

        static readonly double[] zetaLighter1 = new double[] {
           1.48018677156931561235192914649,
           0.25012062539889426471999938167,
           0.00991137502135360774243761467,
          -0.00012084759656676410329833091,
          -4.7585866367662556504652535281e-06,
           2.2229946694466391855561441361e-07,
          -2.2237496498030257121309056582e-09,
          -1.0173226513229028319420799028e-10,
           4.3756643450424558284466248449e-12,
          -6.2229632593100551465504090814e-14,
          -6.6116201003272207115277520305e-16,
           4.9477279533373912324518463830e-17,
          -1.0429819093456189719660003522e-18,
           6.9925216166580021051464412040e-21,
        };

        #endregion

        #region Methods

        /// <summary>
        /// Computes the complex Riemann Zeta function.
        /// </summary>
        /// <param name="s">The complex argument.</param>
        /// <returns>The (in general complex) value.</returns>
        public static ScalarValue RiemannZeta(ScalarValue s)
        {
            if (s == 1.0)
                return new ScalarValue(double.PositiveInfinity);
            else if (s.Re >= 0.0)
                return RiemannZetaGt0(s);

            //See real zeta function for more information
            var zeta_one_minus_s = RiemannZeta1msLt0(s);
            var sin_term = (0.5 * Math.PI * s).Sin() / Math.PI;
            var sabs = s.Abs();

            if (sin_term == 0.0)
                return ScalarValue.Zero;
            else if (sabs < 170)
            {
                //See below
                int n = (int)Math.Floor(sabs / 10.0);
                var p = new ScalarValue(2.0 * Math.PI).Pow(s + 10.0 * n) / Helpers.TwoPIpow[n];
                var g = Gamma.LinearGamma(1.0 - s);
                return p * g * sin_term * zeta_one_minus_s;
            }
                
            throw new YAMPNumericOverflowException("Zeta");
        }

        /// <summary>
        /// Computes the real Riemann Zeta function.
        /// </summary>
        /// <param name="s">The real argument</param>
        /// <returns>The real value.</returns>
        public static double RiemannZeta(double s)
        {
            if (s == 1.0)
                return double.PositiveInfinity;
            else if (s >= 0.0)
                return RiemannZetaGt0(s);

            /* reflection formula, [Abramowitz+Stegun, 23.2.5] */
            var zeta_one_minus_s = RiemannZeta1msLt0(s);
            var sin_term = ((s % 2.0) == 0.0) ? 0.0 : Math.Sin(0.5 * Math.PI * (s % 4.0)) / Math.PI;

            if (sin_term == 0.0)
                return 0.0;
            else if (s > -170)
            {
                /* 
                 * We have to be careful about losing digits
                 * in calculating pow(2 Pi, s). The gamma
                 * function is fine because we were careful
                 * with that implementation.
                 * We keep an array of (2 Pi)^(10 n).
                 */
                var n = (int)Math.Floor((-s) / 10.0);
                var fs = s + 10.0 * n;
                var p = Math.Pow(2.0 * Math.PI, fs) / Helpers.TwoPIpow[n];
                var g = Gamma.LinearGamma(1.0 - s);
                return p * g * sin_term * zeta_one_minus_s;
            }

            throw new YAMPNumericOverflowException("Zeta");
        }

        #endregion

        #region Algorithms

        static ScalarValue RiemannZetaGt0(ScalarValue s)
        {
            if (s.Re < 1.0)
            {
                var c = Helpers.ChebEval(zetaLt1, 2.0 * s - 1.0);
                return c / (s - 1.0);
            }
            else if (s.Re <= 20.0)
            {
                var x = (2.0 * s - 21.0) / 19.0;
                var c = Helpers.ChebEval(zetaGt1, x);
                return c / (s - 1.0);
            }

            var f2 = 1.0 - new ScalarValue(2.0).Pow(-s);
            var f3 = 1.0 - new ScalarValue(3.0).Pow(-s);
            var f5 = 1.0 - new ScalarValue(5.0).Pow(-s);
            var f7 = 1.0 - new ScalarValue(7.0).Pow(-s);
            return 1.0 / (f2 * f3 * f5 * f7);
        }

        static double RiemannZetaGt0(double s)
        {
            if (s < 1.0)
            {
                var c = Helpers.ChebEval(zetaLt1, 2.0 * s - 1.0);
                return c / (s - 1.0);
            }
            else if (s <= 20.0)
            {
                var x = (2.0 * s - 21.0) / 19.0;
                var c = Helpers.ChebEval(zetaGt1, x);
                return c / (s - 1.0);
            }

            var f2 = 1.0 - Math.Pow(2.0, -s);
            var f3 = 1.0 - Math.Pow(3.0, -s);
            var f5 = 1.0 - Math.Pow(5.0, -s);
            var f7 = 1.0 - Math.Pow(7.0, -s);
            return 1.0 / (f2 * f3 * f5 * f7);
        }

        static ScalarValue RiemannZeta1msLt0(ScalarValue s)
        {
            if (s.Re > -19.0)
            {
                var x = (-19 - 2.0 * s) / 19.0;
                var c = Helpers.ChebEval(zetaGt1, x);
                return c / (-s);
            }

            var f2 = 1.0 - new ScalarValue(2.0).Pow(-(1.0 - s));
            var f3 = 1.0 - new ScalarValue(3.0).Pow(-(1.0 - s));
            var f5 = 1.0 - new ScalarValue(5.0).Pow(-(1.0 - s));
            var f7 = 1.0 - new ScalarValue(7.0).Pow(-(1.0 - s));
            return 1.0 / (f2 * f3 * f5 * f7);
        }

        static double RiemannZeta1msLt0(double s)
        {
            if (s > -19.0)
            {
                var x = (-19 - 2.0 * s) / 19.0;
                var c = Helpers.ChebEval(zetaGt1, x);
                return c / (-s);
            }

            var f2 = 1.0 - Math.Pow(2.0, -(1.0 - s));
            var f3 = 1.0 - Math.Pow(3.0, -(1.0 - s));
            var f5 = 1.0 - Math.Pow(5.0, -(1.0 - s));
            var f7 = 1.0 - Math.Pow(7.0, -(1.0 - s));
            return 1.0 / (f2 * f3 * f5 * f7);
        }

        #endregion

        #region Helpers

        static Helpers.ChebSeries zetaLt1 = new Helpers.ChebSeries
        {
            Coefficients = zetaLighter1,
            Order = 13,
            LowerPoint = -1,
            UpperPoint = 1,
            SinglePrecisionOrder = 8
        };

        static Helpers.ChebSeries zetaGt1 = new Helpers.ChebSeries
        {
            Coefficients = zetaGreater1,
            Order = 29,
            LowerPoint = -1,
            UpperPoint = 1,
            SinglePrecisionOrder = 17
        };

        #endregion
    }
}
