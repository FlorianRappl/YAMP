using System;

namespace YAMP.Numerics
{
    /// <summary>
    /// This is the so called Newton fractal.
    /// </summary>
    public class Newton : Fractal
    {
        #region ctor

        /// <summary>
        /// Creates a new Newton instance with the default number of iterations (32).
        /// </summary>
        public Newton() : this(255)
        {
        }

        /// <summary>
        /// Creates a new Newton instance with the default number of colors (25).
        /// </summary>
        /// <param name="maxIterations">The number of iterations.</param>
        public Newton(int maxIterations)
            : this(maxIterations, 25)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="colors">The number of colors.</param>
        public Newton(int maxIterations, int colors)
            : base(maxIterations, colors)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates a single Newton fractal value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>The result (color value 0..1).</returns>
        public override double Run(double x, double y)
        {
            var iter = 0;
            var maxiter = MaxIterations;

            ScalarValue zn = new ScalarValue(x, y);
            ScalarValue pz = ScalarValue.One;
            ScalarValue pzd = ScalarValue.Zero;

            if(x != 0 || y != 0)
            {
                while ((iter < maxiter) && pz.AbsSquare() > 1e-8)
                {
                    pz = zn.Pow(new ScalarValue(3)) - 1.0;
                    pzd = 3.0 * zn.Square();
                    zn = zn - pz / pzd;
                    iter++;
                }
            }

            return Math.Max((double)(maxiter - iter * Colors) / (double)maxiter, 0.0);
        }

        #endregion
    }
}
