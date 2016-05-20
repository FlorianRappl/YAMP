namespace YAMP
{
    using System;

    [Description("CplotFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class CplotFunction : VisualizationFunction
    {
        public CplotFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("CplotFunctionDescriptionForFunction")]
        [Example("cplot(sin)", "CplotFunctionExampleForFunction1")]
        [Example("cplot(z => sin(z) * cos(z))", "CplotFunctionExampleForFunction2")]
        public ComplexPlotValue Function(FunctionValue f)
        {
            return Plot(f, -1.0, 1.0, -1.0, 1.0);
        }

        [Description("CplotFunctionDescriptionForFunctionScalarScalar")]
        [Example("cplot(sin, 0 - 1i, 2 * pi + 1i)", "CplotFunctionExampleForFunctionScalarScalar1")]
        public ComplexPlotValue Function(FunctionValue f, ScalarValue min, ScalarValue max)
        {
            return Plot(f, min.Re, max.Re, min.Im, max.Im);
        }

        [Description("CplotFunctionDescriptionForFunctionScalarScalarScalarScalar")]
        [Example("cplot(sin, 0, 2 * pi, -1, 1)", "CplotFunctionExampleForFunctionScalarScalarScalarScalar1")]
        public ComplexPlotValue Function(FunctionValue f, ScalarValue minX, ScalarValue maxX, ScalarValue minY, ScalarValue maxY)
        {
            return Plot(f, minX.Re, maxX.Re, minY.Re, maxY.Re);
        }

        static ComplexPlotValue Plot(FunctionValue f, Double minx, Double maxx, Double miny, Double maxy)
        {
            var cp = new ComplexPlotValue();
            cp.SetFunction(f);
            cp.MinY = miny;
            cp.MaxY = maxy;
            cp.MinX = minx;
            cp.MaxX = maxx;
            return cp;
        }
    }
}
