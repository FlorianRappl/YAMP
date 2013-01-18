using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Represents the Euler algorithm for computing ODEs.
    /// </summary>
    public class Euler : ODEBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="f">Function to be solved delegate</param>
        /// <param name="begin">Interval start point value</param>
        /// <param name="end">Interval end point value</param>
        /// <param name="y0">Starting condition</param>
        /// <param name="pointsNum">Points number</param>
        public Euler(Func<double, double, double> f, double begin, double end, double y0, int pointsNum)
            : base(f, begin, end, y0, pointsNum)
        {
        }

        /// <summary>
        /// Computes the result.
        /// </summary>
        protected override void Calculate()
        {
            double y = 0;
            double y1;
            double f1;
            double x = 0;
            double[,] result = new double[pointsNum + 1, 2];
            y1 = y0;

            for (int i = 0; i <= pointsNum; i++)
            {
                f1 = f(x, y);
                y = y1 + (h / 2) * (f(x, y) + f(x + h, y + h * f1));
                result[i, 0] = x;
                result[i, 1] = y1;
                y1 = y;
                x = x + h;
            }

            Result = result;
        }
    }
}
