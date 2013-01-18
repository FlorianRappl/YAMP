using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Access to the half division method for getting the closest root.
    /// </summary>
    public class HalfDivisionMethod : NonLinearBase
    {
        /// <summary>
        /// Description constructor
        /// </summary>
        /// <param name="f">Function to be solved delegate</param>
        /// <param name="x0">Interval start point value</param>
        /// <param name="x1">Interval end point value</param>
        /// <param name="d">Amount divisions of segment</param>
        public HalfDivisionMethod(Func<double, double> f, double x0, double x1, double d) : base(f, d)
        {
            Result = new double[2, 1];
            int t = 0;
            int j = 0;
            double x2;
            double y = Math.Abs(x0 - x1);

            while (y > d)
            {
                t++;
                x2 = (x0 + x1) / 2;
                if (f(x0) * f(x2) > 0)
                    x0 = x2;
                else
                    x1 = x2;
                y = Math.Abs(x0 - x1);
            }

            Result[0, j] = x1;
            Result[1, j] = t;
        }
    }
}
