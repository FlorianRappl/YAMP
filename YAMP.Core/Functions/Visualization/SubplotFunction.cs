namespace YAMP
{
    [Description("SubplotFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class SubplotFunction : VisualizationFunction
    {
        public SubplotFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("SubplotFunctionDescriptionForScalarScalar")]
        [Example("a = subplot(4, 3)", "SubplotFunctionExampleForScalarScalar1")]
        [Example("a = subplot(4, 3); a(1, 1) = plot(1:100, rand(100, 4))", "SubplotFunctionExampleForScalarScalar2")]
        [Example("a = subplot(4, 3); a(2:4, 1:3) = polar(0:pi/100:2*pi, sin(0:pi/100:2*pi))", "SubplotFunctionExampleForScalarScalar3")]
        [Example("a = subplot(4, 3); set(a(1, 1), \"title\", \"Example 1\")", "SubplotFunctionExampleForScalarScalar4")]
        public SubPlotValue Function(ScalarValue rows, ScalarValue columns)
        {
            var r = rows.GetIntegerOrThrowException("rows", Name);
            var c = columns.GetIntegerOrThrowException("columns", Name);
            var subplot = new SubPlotValue();
            subplot.Rows = r;
            subplot.Columns = c;
            return subplot;
        }
    }
}
