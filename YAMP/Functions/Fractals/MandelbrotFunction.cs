﻿using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Calculates values within the Mandelbrot fractal.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Mandelbrot_set")]
    class MandelbrotFunction : ArgumentFunction
    {
        [Description("Calculates the most interesting region of the Mandelbrot fractal.")]
        [Example("mandelbrot()", "Computes the most interesting region x in (-2.5, 1.0), y in (-1.0, 1.0) with a resolution of 150 x 150 points.")]
        public MatrixValue Function()
        {
            var m = new Mandelbrot();
            return m.CalculateMatrix(-2.5, 1.0, -1.0, 1.0, 150, 150);
        }

		[Description("Calculates a point in the Mandelbrot fractal.")]
		[Example("mandelbrot(-2.5 - 1i)", "Computes the point z = x + iy with x = -2.5, y = -1.")]
		public ScalarValue Function(ScalarValue z)
		{
			var m = new Mandelbrot();
			return new ScalarValue(m.Run(z.Value, z.ImaginaryValue));
		}

        [Description("Calculates a point in the Mandelbrot fractal.")]
        [Example("mandelbrot(-2.5, -1)", "Computes the point with x = -2.5, y = -1.")]
        public ScalarValue Function(ScalarValue x, ScalarValue y)
        {
            var m = new Mandelbrot();
            return new ScalarValue(m.Run(x.Value, y.Value));
        }

        [Description("Calculates a subset of the Mandelbrot fractal.")]
        [Example("mandelbrot(-2.5 - i, 1 + i, 10 + 8i)", "Computes a matrix (8 rows, 10 columns) within x = -2.5..1 and y = -1..1.")]
        public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue steps)
        {
            var m = new Mandelbrot();
            return m.CalculateMatrix(start.Value, end.Value, start.ImaginaryValue, end.ImaginaryValue, steps.IntValue, steps.ImaginaryIntValue);
        }

        [Description("Calculates a subset of the Mandelbrot fractal.")]
        [Example("mandelbrot(-2.5, 1, -1, 1)", "Computes the matrix within x = -2.5..1 and y = -1..1 with a precision of 0.1.")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn)
        {
            var m = new Mandelbrot();
            var xsteps = (int)Math.Abs(Math.Ceiling((xn.Value - x0.Value) / 0.1));
            var ysteps = (int)Math.Abs(Math.Ceiling((yn.Value - y0.Value) / 0.1));
            return m.CalculateMatrix(x0.Value, xn.Value, y0.Value, yn.Value, xsteps, ysteps);
        }

        [Description("Calculates a subset of the Mandelbrot fractal.")]
        [Example("mandelbrot(-2.5, 1, -1, 1, 5, 5)", "Computes a 5x5 matrix within x = -2.5..1 and y = -1..1.")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn, ScalarValue xsteps, ScalarValue ysteps)
        {
            var m = new Mandelbrot();
            return m.CalculateMatrix(x0.Value, xn.Value, y0.Value, yn.Value, xsteps.IntValue, ysteps.IntValue);
        }
    }
}
