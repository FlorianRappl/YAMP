using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Abstract base class for all non-linear algorithms to determine the closest root.
    /// </summary>
    public abstract class NonLinearBase
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="f">The function to consider.</param>
        /// <param name="d">The spacing to use.</param>
        public NonLinearBase(Func<double, double> f, double d)
        {
            this.f = f;
            this.d = d;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the equation's solution.
        /// </summary>
        public double[,] Result { get; protected set; }

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

        #endregion

        #region Helpers

        /// <summary>
        /// Computes the derivative
        /// </summary>
        /// <param name="x">Point</param>
        protected double fprime(double x)
        {
            return (f(x + d) - f(x)) / d;
        }

        #endregion
    }
}
