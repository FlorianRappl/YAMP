using System;
using YAMP;

namespace YAMP.Numerics
{
    public class SecantMethod : NonLinearBase
    {
        /// <summary>
        /// Constants
        /// </summary>
        // const double shag = 0.01f;
        const double dl = 0.05f;
        //const double delta = 0.0001f;

        /// <summary>
        /// Description constructor
        /// </summary>
        /// <param name="function">Function to be solved delegate</param>
        public SecantMethod(Func<double, double> f, double shag, double delta) : base(f, shag)
        {
            Result = new double[2, 1];

            for (double i = 0; i <= 10; i = i + dl)
            {
                if (f(i) * f(i + dl) < 0)
                    Perform(i + dl, delta);
            }
        }

        /// <summary>
        /// Description SecantMethod
        /// </summary>
        /// <param name="x0">Initial value</param>
        /// <param name="f">Function to be solved delegate</param>
        void Perform(double x0, double delta)
        {
            int j = 0;
            double x1;
            x1 = x0 - f(x0) / fprime(x0);

            while (Math.Abs(x1 - x0) <= delta)
            {
                x1 = x0 - f(x0) / fprime(x0);

                if (Math.Abs(x1 - x0) > delta)
                    x0 = x1;
            }

            Result[0, 0] = x1;
            Result[1, 0] = j;
        }
    }
}
