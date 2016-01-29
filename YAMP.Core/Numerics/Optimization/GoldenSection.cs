using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The golden section search is a technique for finding the extremum (minimum or maximum) of a
    /// strictly unimodal function by successively narrowing the range of values inside which the extremum
    /// is known to exist.
    /// </summary>
    public class GoldenSection : OptimizationBase
    {
        /// <summary>
        /// Creates a new golden section search instance.
        /// </summary>
        /// <param name="f">The function to optimize.</param>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="n">The number of points.</param>
        public GoldenSection(Func<double, double> f, double a, double b, int n) : base(f, a, b, n)
        {
            int i = 0;
            double s1 = 0;
            double s2 = 0;
            double u1 = 0;
            double u2 = 0;
            double fu1 = 0;
            double fu2 = 0;

            s1 = (3 - Math.Sqrt(5)) / 2;
            s2 = (Math.Sqrt(5) - 1) / 2;
            u1 = a + s1 * (b - a);
            u2 = a + s2 * (b - a);
            fu1 = f(u1);
            fu2 = f(u2);

            for (i = 1; i <= n; i++)
            {
                if (fu1 <= fu2)
                {
                    b = u2;
                    u2 = u1;
                    fu2 = fu1;
                    u1 = a + s1 * (b - a);
                    fu1 = f(u1);
                }
                else
                {
                    a = u1;
                    u1 = u2;
                    fu1 = fu2;
                    u2 = a + s2 * (b - a);
                    fu2 = f(u2);
                }
            }

            Result = a;
            AlternativeResult = b;
        }

        /// <summary>
        /// Gets the alternative result.
        /// </summary>
        public double AlternativeResult
        {
            get; private set;
        }
    }
}
