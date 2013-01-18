using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAMP.Numerics.Optimization
{
    /// <summary>
    /// Represents the Newton method.
    /// </summary>
   public class NewtonMethod : NonLinearBase
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="f">Function to be solved delegate.</param>
        /// <param name="x">The starting point.</param>
        /// <param name="d">The spacing to use.</param>
        public NewtonMethod(Func<double, double> f, double x, double d) : base(f, d)
        {
            Result = new double[1, 2];
            int t = 0;
            double x1, y;

            do
            {
                t++;
                x1 = x - f(x) / fprime(x);
                x = x1;
                y = f(x);
            }
            while (Math.Abs(y) >= d);

            Result[0, 0] = x;
            Result[0, 1] = t;
        }
    }
}
