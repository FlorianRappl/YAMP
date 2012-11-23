using System;
using YAMP;

namespace YAMP.Numerics
{
    public abstract class OptimizationBase
    {
        /// <summary>
        /// Gets the Equation solution
        /// </summary>
        public double Result { get; protected set; }

        public OptimizationBase(Func<double, double> f, double a, double b, double n)
        {
            this.f = f;
            this.a = a;
            this.b = b;
            this.n = n;
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
        /// Gets the start point.
        /// </summary>
        public double a
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the end point.
        /// </summary>
        public double b
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of points.
        /// </summary>
        public double n
        {
            get;
            private set;
        }
    }
}
