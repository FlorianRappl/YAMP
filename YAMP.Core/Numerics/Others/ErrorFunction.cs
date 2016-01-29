using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// This class contains everything about the error function.
    /// </summary>
	public static class ErrorFunction
    {
        #region Constants

        const double MAXLOG = -7.09782712893383996732e2;

        #endregion

        #region Real Methods

        /// <summary>
        /// Computes the normal error function erf(x).
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value of the error function.</returns>
        public static double Erf(double x)
		{
			double y, z;

			double[] T = {
				9.60497373987051638749E0,
				9.00260197203842689217E1,
				2.23200534594684319226E3,
				7.00332514112805075473E3,
				5.55923013010394962768E4
			};

			double[] U = {
				3.35617141647503099647E1,
				5.21357949780152679795E2,
				4.59432382970980127987E3,
				2.26290000613890934246E4,
				4.92673942608635921086E4
			};

			if (Math.Abs(x) > 1.0)
				return 1.0 - Erfc(x);

			z = x * x;
			y = x * polevl(z, T, 4) / p1evl(z, U, 5);
			return y;
		}

        /// <summary>
        /// Computes the complementary error function erfc(x).
        /// </summary>
        /// <param name="a">The argument.</param>
        /// <returns>The value of the compl. error function.</returns>
		public static double Erfc(double a)
		{
			double x, y, z, p, q;

			double[] P = {
				2.46196981473530512524E-10,
				5.64189564831068821977E-1,
				7.46321056442269912687E0,
				4.86371970985681366614E1,
				1.96520832956077098242E2,
				5.26445194995477358631E2,
				9.34528527171957607540E2,
				1.02755188689515710272E3,
				5.57535335369399327526E2
			};

			double[] Q = {
				1.32281951154744992508E1,
				8.67072140885989742329E1,
				3.54937778887819891062E2,
				9.75708501743205489753E2,
				1.82390916687909736289E3,
				2.24633760818710981792E3,
				1.65666309194161350182E3,
				5.57535340817727675546E2
			};

			double[] R = {
				5.64189583547755073984E-1,
				1.27536670759978104416E0,
				5.01905042251180477414E0,
				6.16021097993053585195E0,
				7.40974269950448939160E0,
				2.97886665372100240670E0
			};

			double[] S = {
				2.26052863220117276590E0,
				9.39603524938001434673E0,
				1.20489539808096656605E1,
				1.70814450747565897222E1,
				9.60896809063285878198E0,
				3.36907645100081516050E0
			};

			x = Math.Abs(a);

			if (x < 1.0) 
				return 1.0 - Erf(a);

			z = -a * a;

			if (z < MAXLOG)
			{
				if (a < 0)
					return (2.0);
				
				return (0.0);
			}

			z = Math.Exp(z);

			if (x < 8.0)
			{
				p = polevl(x, P, 8);
				q = p1evl(x, Q, 8);
			}
			else
			{
				p = polevl(x, R, 5);
				q = p1evl(x, S, 6);
			}

			y = (z * p) / q;

			if (a < 0) 
				y = 2.0 - y;

			if (y == 0.0)
			{
				if (a < 0) 
					return 2.0;
				
				return (0.0);
			}

			return y;
		}

        #endregion

        #region Complex Methods

        /// <summary>
        /// Computes the complex error function erf(z).
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of erf(z).</returns>
        public static ScalarValue Erf(ScalarValue z)
        {
            if (z.Abs() < 4.0)
                return Erf_Series(z);
            else if (z.Re < 0.0)
                return (-z * z).Exp() * Faddeeva(-ScalarValue.I * z) - 1.0;
            
            return 1.0 - (-z * z).Exp() * Faddeeva(ScalarValue.I * z);
        }

        /// <summary>
        /// Computes the complex complementary error function erfc(z).
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of erfc(z).</returns>
        public static ScalarValue Erfc(ScalarValue z)
        {
            return Erf(1.0 - z);
        }

        #endregion

        #region Other

        /// <summary>
        /// The Faddeeva function or Kramp function is a scaled complex complementary error function.
        /// </summary>
        /// <param name="z">The argument z.</param>
        /// <returns>The evaluated value.</returns>
        public static ScalarValue Faddeeva(ScalarValue z)
        {
            if (z.Im < 0.0) 
                return 2.0 * (-z * z).Exp() - Faddeeva(-z);

            if (z.Re < 0.0)
                return Faddeeva(-z.Conjugate()).Conjugate();

            var r = z.Abs();

            if (r < 2.0)
                return (-z * z).Exp() * (1.0 - Erf_Series(-ScalarValue.I * z));
            else if ((z.Im < 0.1) && (z.Re < 30.0))
                return Taylor(new ScalarValue(z.Re), Math.Exp(-z.Re * z.Re) + 2.0 * Dawson.DawsonIntegral(z.Re) / Helpers.SqrtPI * ScalarValue.I, new ScalarValue(0.0, z.Im));
            else if (r > 7.0)
                return ContinuedFraction(z);
            
            return Weideman(z);
        }

        #endregion

        #region Sub-Algorithms

        static ScalarValue Erf_Series(ScalarValue z)
        {
            var zp = 2.0 / Helpers.SqrtPI * z;
            var zz = -z * z;
            var f = zp;

            for (int k = 1; k < 250; k++)
            {
                var f_old = f;
                zp *= zz / k;
                f += zp / (2 * k + 1);

                if (f == f_old)
                    return (f);
            }

            throw new YAMPNotConvergedException("Erf");
        }

		static double polevl(double x, double[] coef, int N)
		{
			var ans = coef[0];

			for (int i = 1; i <= N; i++)
				ans = ans * x + coef[i];

			return ans;
		}

		static double p1evl(double x, double[] coef, int N)
		{
			var ans = x + coef[0];

			for (int i = 1; i < N; i++)
				ans = ans * x + coef[i];

			return ans;
        }

        #endregion

        #region Weideman Functions

        static ScalarValue Weideman(ScalarValue z)
        {
            var ZN = WeidemanL + ScalarValue.I * z;
            var ZD = WeidemanL - ScalarValue.I * z;
            var ZQ = ZN / ZD;
            ScalarValue f = new ScalarValue(WeidemanCoefficients[40]);

            for (int k = 39; k > 0; k--)
                f = f * ZQ + WeidemanCoefficients[k];

            var ZP = ZN * ZD;
            return 2.0 / ZP * f * ZQ + 1.0 / Helpers.SqrtPI / ZD;
        }

        static ScalarValue ContinuedFraction(ScalarValue z)
        {
            var a = 1.0;			// a_1
            var b = z;	            // b_1
            var D = 1.0 / b;		// D_1 = b_0/b_1
            var Df = a / b;		    // Df_1 = f_1 - f_0
            var f = 0.0 + Df;		// f_1 = f_0 + Df_1 = b_0 + Df_1

            for (int k = 1; k < 250; k++)
            {
                var f_old = f;
                a = -k / 2.0;
                D = 1.0 / (b + a * D);
                Df = (b * D - 1.0) * Df;
                f += Df;

                if (f == f_old)
                    return ScalarValue.I / Helpers.SqrtPI * f;
            }

            throw new YAMPNotConvergedException("Erf");
        }

        static ScalarValue Taylor(ScalarValue z0, ScalarValue w0, ScalarValue dz)
        {
            // first order Taylor expansion
            var wp_old = w0;
            var wp = 2.0 * (ScalarValue.I / Helpers.SqrtPI - z0 * w0);
            var zz = dz;
            var w = w0 + wp * dz;

            // higher orders
            for (int k = 2; k < 250; k++)
            {
                // remmeber the current value
                var w_old = w;

                // compute the next derivative
                var wp_new = -2.0 * (z0 * wp + (k - 1) * wp_old);

                wp_old = wp;
                wp = wp_new;

                // use it to generate the next term in the Taylor expansion
                zz = zz * dz / k;
                w = w_old + wp * zz;

                // test whether we have converged
                if (w == w_old)
                    return w;
            }

            throw new YAMPNotConvergedException("Erf");
        }

        #endregion

        #region Weideman Numbers

        static readonly double WeidemanL = Math.Sqrt(40.0 / Math.Sqrt(2.0));

        static readonly double[] WeidemanCoefficients = new double[]
        {
            3.0005271472811341147438, // 0
            2.899624509389705247492,
            2.616054152761860368947,
            2.20151379487831192991,
            1.725383084817977807050,
            1.256381567576513235243, // 5
            0.847217457659381821530,
            0.52665289882770863869581,
            0.2998943799615006297951,
            0.1550426380247949427170,
            0.0718236177907433682806, // 10
            0.0292029164712418670902,
            0.01004818624278342412539,
            0.002705405633073791311865,
            0.000439807015986966782752,
            -0.0000393936314548956872961, // 15
            -0.0000559130926424831822323,
            -0.00001800744714475095715480,
            -1.066013898494714388844e-6,
            1.483566113220077986810e-6,
            5.91213695189949384568e-7, // 20
            1.419864239993567456648e-8,
            -6.35177348504429108355e-8,
            -1.83156167830404631847e-8,
            3.24974651804369739084e-9,
            3.01778054000907084962e-9, // 25
            2.10860063470665179035e-10,
            -3.56323398659765326830e-10,
            -9.05512445092829268740e-11,
            3.47272670930455000726e-11,
            1.771449521401119186147e-11, // 30
            -2.72760231582004518397e-12,
            -2.90768834218286692054e-12,
            1.203145821938798755342e-13,
            4.53296667826067277389e-13,
            1.372562058671550042872e-14, // 35
            -7.07408626028685552231e-14,
            -5.40931028288214223366e-15,
            1.135768719899924165040e-14,
            1.128073562364402060469e-15,
            -1.89969494739492699566e-15 // 40
        };

        #endregion
    }
}
