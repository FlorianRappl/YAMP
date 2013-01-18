using System;
using YAMP;

namespace YAMP.Numerics
{
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
        public static double DawsonIntegral(double x)
        {
            if (x < 0.0)
                return -DawsonIntegral(-x);
            else if (x < 1.0)
                return DawsonSeries(x);
            else if (x > 10.0)
                return DawsonAsymptotic(x);
           
            return DawsonRybicki(x);
        }

        #region Sub-Algorithms

        static double DawsonSeries(double x)
        {
            double xx = -2.0 * x * x;
            double df = x;
            double f = df;

            for (int k = 1; k < 250; k++)
            {
                double f_old = f;
                df = df * xx / (2 * k + 1);
                f += df;

                if (f == f_old)
                    return f;
            }

            throw new YAMPNotConvergedException("dawson");
        }

        static double DawsonAsymptotic(double x)
        {
            double xx = 2.0 * x * x;
            double df = 0.5 / x;
            double f = df;

            for (int k = 0; k < 250; k++)
            {
                double f_old = f;
                df = df * (2 * k + 1) / xx;
                f += df;

                if (f == f_old)
                    return f;
            }

            throw new YAMPNotConvergedException("dawson");
        }

        static double DawsonRybicki(double x)
        {
            int n0 = 2 * ((int)Math.Round(x / Dawson_Rybicki_h / 2.0));
            double x0 = n0 * Dawson_Rybicki_h;
            double y = x - x0;
            double f = 0.0;
            double b = Math.Exp(2.0 * Dawson_Rybicki_h * y);
            double bb = b * b;

            for (int k = 0; k < Dawson_Rybicki_coefficients.Length; k++)
            {
                double f_old = f;
                int m = 2 * k + 1;
                double df = Dawson_Rybicki_coefficients[k] * (b / (n0 + m) + 1.0 / b / (n0 - m));
                f += df;

                if (f == f_old)
                    return Math.Exp(-y * y) / Helpers.SqrtPI * f;

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
