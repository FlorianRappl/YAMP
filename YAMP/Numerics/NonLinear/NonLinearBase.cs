using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class NonLinearBase
    {
        /// <summary>
        /// Equation solution
        /// </summary>
        public double[,] Result { get; protected set; }

        public NonLinearBase(Func<double, double> f, double d)
        {
            this.f = f;
            this.d = d;
        }

        /// <summary>
        /// Gets the function to use.
        /// </summary>
        public Func<double, double> f
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the spacing d.
        /// </summary>
        public double d
        {
            get;
            private set;
        }

        /// <summary>
        /// Computes the derivative
        /// </summary>
        /// <param name="x">Point</param>
        public double fprime(double x)
        {
            return (f(x + d) - f(x)) / d;
        }
    }
}
