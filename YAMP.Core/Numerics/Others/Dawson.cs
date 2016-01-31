namespace YAMP.Numerics
{
    using System;
    using YAMP.Exceptions;

    /// <summary>
    /// This class contains the dawson integral.
    /// </summary>
    public static class Dawson
    {
        #region Constant Coefficients

        const double Dawson_Rybicki_h = 0.25;

        static readonly double[] Dawson_Rybicki_coefficients = Compute_Dawson_Rybicki_Coefficients(0.25, 16);

        #endregion

        /// <summary>
        /// Computes the dawson integral.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value of F(x).</returns>
        public static Double DawsonIntegral(Double x)
        {
            if (x < 0.0)
            {
                return -DawsonIntegral(-x);
            }
            else if (x < 1.0)
            {
                return DawsonSeries(x);
            }
            else if (x > 10.0)
            {
                return DawsonAsymptotic(x);
            }
           
            return DawsonRybicki(x);
        }

        #region Sub-Algorithms

        static Double DawsonSeries(Double x)
        {
            var xx = -2.0 * x * x;
            var df = x;
            var f = df;

            for (int k = 1; k < 250; k++)
            {
                var f_old = f;
                df = df * xx / (2 * k + 1);
                f += df;

                if (f == f_old)
                {
                    return f;
                }
            }

            throw new YAMPNotConvergedException("dawson");
        }

        static Double DawsonAsymptotic(Double x)
        {
            var xx = 2.0 * x * x;
            var df = 0.5 / x;
            var f = df;

            for (int k = 0; k < 250; k++)
            {
                var f_old = f;
                df = df * (2 * k + 1) / xx;
                f += df;

                if (f == f_old)
                {
                    return f;
                }
            }

            throw new YAMPNotConvergedException("dawson");
        }

        static Double DawsonRybicki(Double x)
        {
            var n0 = 2 * ((Int32)Math.Round(x / Dawson_Rybicki_h / 2.0));
            var x0 = n0 * Dawson_Rybicki_h;
            var y = x - x0;
            var f = 0.0;
            var b = Math.Exp(2.0 * Dawson_Rybicki_h * y);
            var bb = b * b;

            for (int k = 0; k < Dawson_Rybicki_coefficients.Length; k++)
            {
                var f_old = f;
                var m = 2 * k + 1;
                var df = Dawson_Rybicki_coefficients[k] * (b / (n0 + m) + 1.0 / b / (n0 - m));
                f += df;

                if (f == f_old)
                {
                    return Math.Exp(-y * y) / Helpers.SqrtPI * f;
                }

                b = b * bb;
            }

            throw new YAMPNotConvergedException("dawson");
        }

        #endregion

        #region Helpers

        static double[] Compute_Dawson_Rybicki_Coefficients(double h, int n)
        {
            double[] coefficients = new double[n];

            for (int k = 0; k < n; k++)
            {
                int m = 2 * k + 1;
                double z = h * m;
                coefficients[k] = Math.Exp(-z * z);
            }

            return (coefficients);
        }

        #endregion
    }
}
