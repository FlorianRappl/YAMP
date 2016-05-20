namespace YAMP
{
    [Description("ErrorbarFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class ErrorbarFunction : VisualizationFunction
    {
        public ErrorbarFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("ErrorbarFunctionDescriptionForMatrixMatrix")]
        [Example("errorbar(2^1:16, 0.3 * ones(16, 1))", "ErrorbarFunctionExampleForMatrixMatrix1")]
        [Example("errorbar([0:10, 2^(0:2:20), 2^(1:2:21)], [100000 * ones(16, 1), 0.5 * ones(16, 1)])", "ErrorbarFunctionExampleForMatrixMatrix2")]
        public ErrorPlotValue Function(MatrixValue Y, MatrixValue E)
        {
            var plot = new ErrorPlotValue();
            plot.AddPoints(Y, E);
            return plot;
        }

        [Description("ErrorbarFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("errorbar(-7:7, rand(15, 1), 0.1 * ones(15, 1))", "ErrorbarFunctionExampleForMatrixMatrixMatrix1")]
        [Example("errorbar(0:pi/10:2*pi, [sin(0:pi/10:2*pi), cos(0:pi/10:2*pi)], rand(20, 1))", "ErrorbarFunctionExampleForMatrixMatrixMatrix2")]
        public ErrorPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue E)
        {
            var plot = new ErrorPlotValue();
            plot.AddPoints(X, Y, E);
            return plot;
        }
    }
}
