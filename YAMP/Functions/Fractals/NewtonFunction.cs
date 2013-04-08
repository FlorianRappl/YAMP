using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Calculates values within the Mandelbrot fractal.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Newton_fractal")]
    sealed class NewtonFunction : ArgumentFunction
    {
        [Description("Calculates the most interesting region of the Newton fractal.")]
        [Example("newton()", "Computes the most interesting region x in (-1.0, 1.0), y in (-1.0, 1.0) with a resolution of 150 x 150 points.")]
        public MatrixValue Function()
        {
            var m = new Newton();
            return m.CalculateMatrix(-1.0, 1.0, -1.0, 1.0, 150, 150);
        }

        [Description("Calculates a point in the Newton fractal.")]
        [Example("newton(-2.5 - 1i)", "Computes the point z = x + iy with x = -2.5, y = -1.")]
        public ScalarValue Function(ScalarValue z)
        {
            var m = new Newton();
            return new ScalarValue(m.Run(z.Re, z.Im));
        }

        [Description("Calculates a point in the Newton fractal.")]
        [Example("newton(-0.5, -1)", "Computes the point with x = -0.5, y = -1.")]
        public ScalarValue Function(ScalarValue x, ScalarValue y)
        {
            var m = new Newton();
            return new ScalarValue(m.Run(x.Re, y.Re));
        }

        [Description("Calculates a subset of the Newton fractal.")]
        [Example("newton(-2.5 - i, 1 + i, 10)", "Computes a matrix (10 rows, 10 columns) within x = -2.5..1 and y = -1..1.")]
        public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue steps)
        {
            var m = new Newton();
            return m.CalculateMatrix(start.Re, end.Re, start.Im, end.Im, steps.IntValue, steps.IntValue);
        }

        [Description("Calculates a subset of the Newton fractal.")]
        [Example("newton(-2.5, 1, -1, 1)", "Computes the matrix within x = -2.5..1 and y = -1..1 with a precision of 0.1.")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn)
        {
            var m = new Newton();
            var xsteps = (int)Math.Abs(Math.Ceiling((xn.Re - x0.Re) / 0.1));
            var ysteps = (int)Math.Abs(Math.Ceiling((yn.Re - y0.Re) / 0.1));
            return m.CalculateMatrix(x0.Re, xn.Re, y0.Re, yn.Re, xsteps, ysteps);
        }

        [Description("Calculates a subset of the Newton fractal.")]
        [Example("newton(-0.5, 1, -1, 1, 5, 5)", "Computes a 5x5 matrix within x = -0.5..1 and y = -1..1.")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn, ScalarValue xsteps, ScalarValue ysteps)
        {
            var m = new Newton();
            return m.CalculateMatrix(x0.Re, xn.Re, y0.Re, yn.Re, xsteps.IntValue, ysteps.IntValue);
        }
    }
}
