using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Creates the class for evaluating a mandelbrot function.
    /// </summary>
    public class Mandelbrot : Fractal
    {
        #region ctor

        /// <summary>
        /// Creates a new mandelbrot instance with the default number of iterations (255).
        /// </summary>
        public Mandelbrot() : this(255)
        {
        }

        /// <summary>
        /// Creates a new mandelbrot instance with the default number of colors (25).
        /// </summary>
        /// <param name="maxIterations">The number of iterations.</param>
        public Mandelbrot(int maxIterations)
            : this(maxIterations, 25)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="colors">The number of colors.</param>
        public Mandelbrot(int maxIterations, int colors)
            : base(maxIterations, colors)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates a single mandelbrot value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>The result (color value 0..1).</returns>
        public override double Run(double x, double y)
        {
            var maxiter = MaxIterations;
            var iter = 0;
            var r1 = 0.0;
            var i1 = 0.0;
            var r1pow2 = 0.0;
            var i1pow2 = 0.0;
            var rpow = 0.0;
            var rlastpow = 0.0;

            while ((iter < maxiter) && (rpow < 4))
            {
                r1pow2 = r1 * r1;
                i1pow2 = i1 * i1;
                i1 = 2.0 * i1 * r1 + y;
                r1 = r1pow2 - i1pow2 + x;
                rlastpow = rpow;
                rpow = r1pow2 + i1pow2;
                iter++;
            }

            return Math.Max((double)(maxiter - iter * Colors) / (double)maxiter, 0.0);
        }

        #endregion
    }
}
