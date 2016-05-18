namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("MandelbrotFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("MandelbrotFunctionLink")]
    sealed class MandelbrotFunction : ArgumentFunction
    {
        [Description("MandelbrotFunctionDescriptionForVoid")]
        [Example("mandelbrot()", "MandelbrotFunctionExampleForVoid1")]
        public MatrixValue Function()
        {
            var m = new Mandelbrot();
            return m.CalculateMatrix(-2.5, 1.0, -1.0, 1.0, 150, 150);
        }

		[Description("MandelbrotFunctionDescriptionForScalar")]
		[Example("mandelbrot(-2.5 - 1i)", "MandelbrotFunctionExampleForScalar1")]
		public ScalarValue Function(ScalarValue z)
		{
			var m = new Mandelbrot();
            return new ScalarValue(m.Run(z.Re, z.Im));
		}

        [Description("MandelbrotFunctionDescriptionForScalarScalar")]
        [Example("mandelbrot(-2.5, -1)", "MandelbrotFunctionExampleForScalarScalar1")]
        public ScalarValue Function(ScalarValue x, ScalarValue y)
        {
            var m = new Mandelbrot();
            return new ScalarValue(m.Run(x.Re, y.Re));
        }

        [Description("MandelbrotFunctionDescriptionForScalarScalarScalar")]
        [Example("mandelbrot(-2.5 - i, 1 + i, 10)", "MandelbrotFunctionExampleForScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue steps)
        {
            var s = steps.GetIntegerOrThrowException("steps", Name);
            var m = new Mandelbrot();
            return m.CalculateMatrix(start.Re, end.Re, start.Im, end.Im, s, s);
        }

        [Description("MandelbrotFunctionDescriptionForScalarScalarScalarScalar")]
        [Example("mandelbrot(-2.5, 1, -1, 1)", "MandelbrotFunctionExampleForScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn)
        {
            var m = new Mandelbrot();
            var xsteps = (Int32)Math.Abs(Math.Ceiling((xn.Re - x0.Re) / 0.1));
            var ysteps = (Int32)Math.Abs(Math.Ceiling((yn.Re - y0.Re) / 0.1));
            return m.CalculateMatrix(x0.Re, xn.Re, y0.Re, yn.Re, xsteps, ysteps);
        }

        [Description("MandelbrotFunctionDescriptionForScalarScalarScalarScalarScalarScalar")]
        [Example("mandelbrot(-2.5, 1, -1, 1, 5, 5)", "MandelbrotFunctionExampleForScalarScalarScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn, ScalarValue xsteps, ScalarValue ysteps)
        {
            var xs = xsteps.GetIntegerOrThrowException("xsteps", Name);
            var ys = ysteps.GetIntegerOrThrowException("ysteps", Name);
            var m = new Mandelbrot();
            return m.CalculateMatrix(x0.Re, xn.Re, y0.Re, yn.Re, xs, ys);
        }
    }
}
