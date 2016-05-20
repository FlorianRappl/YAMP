namespace YAMP
{
    [Description("LogLogFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class LogLogFunction : VisualizationFunction
	{
        public LogLogFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("LogLogFunctionDescriptionForMatrix")]
        [Example("loglog(2^1:16)", "LogLogFunctionExampleForMatrix1")]
        [Example("loglog([0:10, 2^(0:2:20)])", "LogLogFunctionExampleForMatrix2")]
        [Example("loglog([0:10, 2^(0:2:20), 2^(1:2:21)])", "LogLogFunctionExampleForMatrix3")]
		public Plot2DValue Function(MatrixValue m)
		{
			var plot = new Plot2DValue();
			plot.AddPoints(m);
			plot.IsLogX = true;
			plot.IsLogY = true;
			return plot;
		}

        [Description("LogLogFunctionDescriptionForMatrixMatrix")]
        [Example("loglog(0:15, 2^1:16)", "LogLogFunctionExampleForMatrixMatrix1")]
        [Example("loglog([1:11, 2^(1:2:21)], [0:10, 2^(0:2:20)])", "LogLogFunctionExampleForMatrixMatrix2")]
        [Example("loglog(0:0.01:2*pi, [sin(0:0.01:2*pi), cos(0:0.01:2*pi), 0:0.01:2*pi])", "LogLogFunctionExampleForMatrixMatrix3")]
		public Plot2DValue Function(MatrixValue m, MatrixValue n)
		{
			var plot = new Plot2DValue();
			plot.AddPoints(m, n);
			plot.IsLogX = true;
			plot.IsLogY = true;
			return plot;
		}
	}
}
