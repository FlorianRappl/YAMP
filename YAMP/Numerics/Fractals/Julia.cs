using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// This is the (more general) Julia fractal (superset of the Mandelbrot set).
    /// </summary>
    public class Julia : Fractal
    {
        #region Members

        double _cx;
        double _cy;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new Julia instance with the default number of iterations (250).
        /// </summary>
        public Julia() : this(250)
        {
        }

        /// <summary>
        /// Creates a new Julia instance with the default number of colors (25).
        /// </summary>
        /// <param name="maxIterations">The number of iterations.</param>
        public Julia(int maxIterations)
            : this(maxIterations, 25)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="colors">The number of colors.</param>
        public Julia(int maxIterations, int colors)
            : this(maxIterations, colors, 0.337941427889731, 0.40057514332355)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="colors">The number of colors.</param>
        /// <param name="c">Sets the coefficient in both directions.</param>
        public Julia(int maxIterations, int colors, double c)
            : this(maxIterations, colors, c, c)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="colors">The number of colors.</param>
        /// <param name="cx">Sets the coefficient in x direction.</param>
        /// <param name="cy">Sets the coefficient in y direction.</param>
        public Julia(int maxIterations, int colors, double cx, double cy)
            : base(maxIterations, colors)
        {
            _cx = cx;
            _cy = cy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the coefficient in x direction.
        /// </summary>
        public double Cx
        {
            get { return _cx; }
            set { _cx = value; }
        }

        /// <summary>
        /// Gets or sets the coefficient in y direction.
        /// </summary>
        public double Cy
        {
            get { return _cy; }
            set { _cy = value; }
        }

        /// <summary>
        /// Gets or sets both values (XC, YC) to a single value.
        /// </summary>
        public double C
        {
            get { return 0.5 * (_cx + _cy); }
            set { _cx = value; _cy = value; }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Calculates a single Julia value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>The result (color value 0..1).</returns>
        public override double Run(double x, double y)
        {
            var maxiter = MaxIterations;
            var iter = 0;
            var r1 = x;
            var i1 = y;
            var r1pow2 = x * x;
            var i1pow2 = y * y;
            var rpow = 0.0;
            var rlastpow = 0.0;

            while ((iter < maxiter) && (rpow < 4))
            {
                r1pow2 = r1 * r1;
                i1pow2 = i1 * i1;
                i1 = 2 * i1 * r1 + _cy;
                r1 = r1pow2 - i1pow2 + _cx;
                rlastpow = rpow;
                rpow = r1pow2 + i1pow2;
                iter++;
            }

            return Math.Max((double)(maxiter - iter * Colors) / (double)maxiter, 0.0);
        }

        #endregion
    }
}
