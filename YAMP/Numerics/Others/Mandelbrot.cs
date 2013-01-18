using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Creates the class for evaluating a mandelbrot function.
    /// </summary>
    public class Mandelbrot
    {
        #region Members

        int maxIterations;
        int colors;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new mandelbrot instance with the default number of iterations (768).
        /// </summary>
        public Mandelbrot() : this(768)
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
        {
            this.maxIterations = maxIterations;
            this.colors = colors;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the maximum number of iterations.
        /// </summary>
        public int MaxIterations { get { return maxIterations; } }

        /// <summary>
        /// Gets the number of colors to use.
        /// </summary>
        public int Colors { get { return colors; } }

        #endregion

        #region Color Methods

        /// <summary>
        /// Gets the red part of the color (0-255) for the given value.
        /// </summary>
        /// <param name="color">A value for the color (0...1)</param>
        /// <returns>The red part of the color (value 0 - 255).</returns>
        public int R(double color)
        {
            var c = (int)(color * maxIterations);
            return Math.Max(Math.Min(c, 255), 0);
        }

        /// <summary>
        /// Gets the green part of the color (0-255) for the given value.
        /// </summary>
        /// <param name="color">A value for the color (0...1)</param>
        /// <returns>The green part of the color (value 0 - 255).</returns>
        public int G(double color)
        {
            var c = (int)((color - 0.33333) * maxIterations);
            return Math.Max(Math.Min(c, 255), 0);
        }

        /// <summary>
        /// Gets the blue part of the color (0-255) for the given value.
        /// </summary>
        /// <param name="color">A value for the color (0...1)</param>
        /// <returns>The blue part of the color (value 0 - 255).</returns>
        public int B(double color)
        {
            var c = (int)((color - 0.66666) * maxIterations);
            return Math.Max(Math.Min(c, 255), 0);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the matrix with all the mandelbrot values.
        /// </summary>
        /// <param name="xi">The initial (start) x.</param>
        /// <param name="xf">The final (end) x.</param>
        /// <param name="yi">The initial (start) y.</param>
        /// <param name="yf">The final (end) y.</param>
        /// <param name="xsteps">The number of steps in x direction.</param>
        /// <param name="ysteps">The number of steps in y direction.</param>
        /// <returns>The matrix with all the values.</returns>
        public MatrixValue CalculateMatrix(double xi, double xf, double yi, double yf, int xsteps, int ysteps)
        {
            var width = (double)xsteps;
            var height = (double)ysteps;
            var xs = (xf - xi) / width;
            var ys = (yf - yi) / height;
            var M = new MatrixValue(ysteps, xsteps);

            for (var i = 0; i < width; i++)
            {
                var x = i * xs + xi;

                for (var j = 0; j < height; j++)
                {
                    var y = j * ys + yi;
                    M[j + 1, i + 1] = new ScalarValue(Run(x, y));
                }
            }

            return M;
        }

        /// <summary>
        /// Calculates a single mandelbrot value.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>The result (color value 0..1).</returns>
        public double Run(double x, double y) 
        {
            var xt = 0.0;
		    var yt = 0.0;
		    var iteration = 0;

            while (iteration < maxIterations && xt * xt + yt * yt < 4.0)
            {
		        var t = xt * xt - yt * yt + x;
		        yt = 2.0 * xt * yt + y;
		        xt = t;
		        iteration++;
	        }

            return Math.Max((double)(maxIterations - iteration * colors) / (double)maxIterations, 0.0);
        }

        #endregion
    }
}
