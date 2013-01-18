using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// The abstract base class for all optimization algorithms, i.e. the ones to find an extremum.
    /// </summary>
    public abstract class OptimizationBase
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="f">The function to use.</param>
        /// <param name="a">The start point.</param>
        /// <param name="b">The end point.</param>
        /// <param name="n">The number of points.</param>
        public OptimizationBase(Func<double, double> f, double a, double b, int n)
        {
            this.f = f;
            this.a = a;
            this.b = b;
            this.n = n;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Equation solution
        /// </summary>
        public double Result { get; protected set; }

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
        public int n
        {
            get;
            private set;
        }

        #endregion
    }
}
