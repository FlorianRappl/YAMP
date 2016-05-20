namespace YAMP
{
    [Description("SemiLogYFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class SemiLogYFunction : VisualizationFunction
	{
        public SemiLogYFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("SemiLogYFunctionDescriptionForMatrix")]
        [Example("semilogy(2^1:16)", "SemiLogYFunctionExampleForMatrix1")]
        [Example("semilogy([0:10, 2^(0:2:20)])", "SemiLogYFunctionExampleForMatrix2")]
        [Example("semilogy([0:10, 2^(0:2:20), 2^(1:2:21)])", "SemiLogYFunctionExampleForMatrix3")]
		public Plot2DValue Function(MatrixValue m)
		{
			var plot = new Plot2DValue();
			plot.AddPoints(m);
			plot.IsLogY = true;
			return plot;
		}

        [Description("SemiLogYFunctionDescriptionForMatrixMatrix")]
        [Example("semilogy(0:15, 2^1:16)", "SemiLogYFunctionExampleForMatrixMatrix1")]
        [Example("semilogy([1:11, 2^(1:2:21)], [0:10, 2^(0:2:20)])", "SemiLogYFunctionExampleForMatrixMatrix2")]
        [Example("semilogy(0:0.01:2*pi, [sin(0:0.01:2*pi), cos(0:0.01:2*pi), 0:0.01:2*pi])", "SemiLogYFunctionExampleForMatrixMatrix3")]
		public Plot2DValue Function(MatrixValue m, MatrixValue n)
		{
			var plot = new Plot2DValue();
			plot.AddPoints(m, n);
			plot.IsLogY = true;
			return plot;
		}
	}
}
