using System;
using YAMP;

namespace YAMP.Numerics
{
    public class Mandelbrot
    {
        int maxIterations;
        int colors;

        public Mandelbrot() : this(768)
        {
        }

        public Mandelbrot(int maxIterations)
            : this(maxIterations, 25)
        {
        }

        public Mandelbrot(int maxIterations, int colors)
        {
            this.maxIterations = maxIterations;
            this.colors = colors;
        }

        public int MaxIterations { get { return maxIterations; } }

        public int Colors { get { return colors; } }

        public int R(double color)
        {
            var c = (int)(color * 768.0);
            return Math.Max(Math.Min(c, 255), 0);
        }

        public int G(double color)
        {
            var c = (int)(color * 768.0 - 255.0);
            return Math.Max(Math.Min(c, 255), 0);
        }

        public int B(double color)
        {
            var c = (int)(color * 768.0 - 511.0);
            return Math.Max(Math.Min(c, 255), 0);
        }

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
    }
}
