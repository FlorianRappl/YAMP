namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("NewtonFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("NewtonFunctionLink")]
    sealed class NewtonFunction : ArgumentFunction
    {
        [Description("NewtonFunctionDescriptionForVoid")]
        [Example("newton()", "NewtonFunctionExampleForVoid1")]
        public MatrixValue Function()
        {
            var m = new Newton();
            return m.CalculateMatrix(-1.0, 1.0, -1.0, 1.0, 150, 150);
        }

        [Description("NewtonFunctionDescriptionForScalar")]
        [Example("newton(-2.5 - 1i)", "NewtonFunctionExampleForScalar1")]
        public ScalarValue Function(ScalarValue z)
        {
            var m = new Newton();
            return new ScalarValue(m.Run(z.Re, z.Im));
        }

        [Description("NewtonFunctionDescriptionForScalarScalar")]
        [Example("newton(-0.5, -1)", "NewtonFunctionExampleForScalarScalar1")]
        public ScalarValue Function(ScalarValue x, ScalarValue y)
        {
            var m = new Newton();
            return new ScalarValue(m.Run(x.Re, y.Re));
        }

        [Description("NewtonFunctionDescriptionForScalarScalarScalar")]
        [Example("newton(-2.5 - i, 1 + i, 10)", "NewtonFunctionExampleForScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue steps)
        {
            var m = new Newton();
            return m.CalculateMatrix(start.Re, end.Re, start.Im, end.Im, steps.IntValue, steps.IntValue);
        }

        [Description("NewtonFunctionDescriptionForScalarScalarScalarScalar")]
        [Example("newton(-2.5, 1, -1, 1)", "NewtonFunctionExampleForScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn)
        {
            var m = new Newton();
            var xsteps = (int)Math.Abs(Math.Ceiling((xn.Re - x0.Re) / 0.1));
            var ysteps = (int)Math.Abs(Math.Ceiling((yn.Re - y0.Re) / 0.1));
            return m.CalculateMatrix(x0.Re, xn.Re, y0.Re, yn.Re, xsteps, ysteps);
        }

        [Description("NewtonFunctionDescriptionForScalarScalarScalarScalarScalarScalar")]
        [Example("newton(-0.5, 1, -1, 1, 5, 5)", "NewtonFunctionExampleForScalarScalarScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue x0, ScalarValue xn, ScalarValue y0, ScalarValue yn, ScalarValue xsteps, ScalarValue ysteps)
        {
            var m = new Newton();
            return m.CalculateMatrix(x0.Re, xn.Re, y0.Re, yn.Re, xsteps.IntValue, ysteps.IntValue);
        }
    }
}
