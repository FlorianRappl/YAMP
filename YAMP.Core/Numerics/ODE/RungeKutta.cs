using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// This is the Runge-Kutta Algorithm for solving ODEs.
    /// </summary>
    public class RungeKutta : ODEBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="f">Function to be solved delegate</param>
        /// <param name="begin">Interval start point value</param>
        /// <param name="end">Interval end point value</param>
        /// <param name="y0">Starting condition</param>
        /// <param name="pointsNum">Points number</param>
        public RungeKutta(Func<double, double, double> f, double begin, double end, double y0, int pointsNum) 
            : base(f, begin, end, y0, pointsNum)
        {
        }

        /// <summary>
        /// Calculates the result.
        /// </summary>
        protected override void Calculate()
        {
            double k1, k2, k3;
            double y1 = y0;
            double x = 0;
            double y = 0;
            double[,] result = new double[pointsNum + 1, 2];
            result[0, 0] = x;
            result[0, 1] = y1;

            for (int i = 1; i <= pointsNum; i++)
            {
                k1 = h * f(x, y);
                x = x + h / 2;
                y = y1 + k1 / 2;
                k2 = f(x, y) * h;
                y = y1 + k2 / 2;
                k3 = f(x, y) * h;
                x = x + h / 2;
                y = y1 + (k1 + 2 * k2 + 2 * k3 + f(x, y) * h) / 6;
                y1 = y;
                result[i, 0] = x;
                result[i, 1] = y1;
            }

            Result = result;
        }
    }
}
