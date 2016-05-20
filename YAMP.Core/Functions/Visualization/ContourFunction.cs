namespace YAMP
{
    [Description("ContourFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class ContourFunction : VisualizationFunction
	{
        public ContourFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("ContourFunctionDescriptionForMatrix")]
        [Example("contour([1, 2, 3; 4, 5, 6; 7, 8, 9])", "ContourFunctionExampleForMatrix1")]
		public ContourPlotValue Function(MatrixValue Z)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(Z);
			return plot;
		}

        [Description("ContourFunctionDescriptionForMatrixScalar")]
        [Example("contour([1, 2, 3; 4, 5, 6; 7, 8, 9], 2)", "ContourFunctionExampleForMatrixScalar1")]
		public ContourPlotValue Function(MatrixValue Z, ScalarValue n)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
			var plot = new ContourPlotValue();
			plot.AddPoints(Z);
			plot.SetLevels(nn);
			return plot;
		}

        [Description("ContourFunctionDescriptionForMatrixMatrix")]
        [Example("contour([1, 2, 3; 4, 5, 6; 7, 8, 9], [1, 2])", "ContourFunctionExampleForMatrixMatrix1")]
		public ContourPlotValue Function(MatrixValue Z, MatrixValue v)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(Z);
			plot.SetLevels(v);
			return plot;
		}

        [Description("ContourFunctionDescriptionForMatrixMatrixMatrix")]
		public ContourPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(X, Y, Z);
			return plot;
		}

        [Description("ContourFunctionDescriptionForMatrixMatrixMatrixScalar")]
		public ContourPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z, ScalarValue n)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
			var plot = new ContourPlotValue();
			plot.AddPoints(X, Y, Z);
			plot.SetLevels(nn);
			return plot;
		}

        [Description("ContourFunctionDescriptionForMatrixMatrixMatrixMatrix")]
		public ContourPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z, MatrixValue v)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(X, Y, Z);
			plot.SetLevels(v);
			return plot;
		}
	}
}
