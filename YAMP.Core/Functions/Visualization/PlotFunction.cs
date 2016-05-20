namespace YAMP
{
    using YAMP.Exceptions;

    [Description("PlotFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class PlotFunction : VisualizationFunction
	{
        public PlotFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("PlotFunctionDescriptionForMatrix")]
        [Example("plot(2^1:16)", "PlotFunctionExampleForMatrix1")]
        [Example("plot([0:10, 2^(0:2:20)])", "PlotFunctionExampleForMatrix2")]
        [Example("plot([0:10, 2^(0:2:20), 2^(1:2:21)])", "PlotFunctionExampleForMatrix3")]
		public Plot2DValue Function(MatrixValue m)
		{
			var plot = new Plot2DValue();
			plot.AddPoints(m);
			return plot;
		}

        [Description("PlotFunctionDescriptionForMatrixMatrix")]
        [Example("plot(0:15, 2^1:16)", "PlotFunctionExampleForMatrixMatrix1")]
        [Example("plot([1:11, 2^(1:2:21)], [0:10, 2^(0:2:20)])", "PlotFunctionExampleForMatrixMatrix2")]
        [Example("plot(0:0.01:2*pi, [sin(0:0.01:2*pi), cos(0:0.01:2*pi), 0:0.01:2*pi])", "PlotFunctionExampleForMatrixMatrix3")]
		public Plot2DValue Function(MatrixValue m, MatrixValue n)
		{
			var plot = new Plot2DValue();
			plot.AddPoints(m, n);
			return plot;
		}

        [Description("PlotFunctionDescriptionForMatrixMatrixArguments")]
        [Example("plot(0:15, 2^1:16, 3^1:16)", "PlotFunctionExampleForMatrixMatrixArguments1")]
        [Example("plot([1:11, 2^(1:2:21)], [0:10, 2^(0:2:20)], [-10:0, 2^(-20:2:0)])", "PlotFunctionExampleForMatrixMatrixArguments2")]
        [Example("plot(0:0.01:2*pi, [sin(0:0.01:2*pi), cos(0:0.01:2*pi), 0:0.01:2*pi], [sinh(0:0.01:2*pi), cosh(0:0.01:2*pi)])", "PlotFunctionExampleForMatrixMatrixArguments3")]
		[Arguments(2, 1)]
		public Plot2DValue Function(MatrixValue m, MatrixValue n, ArgumentsValue l)
		{
			var plot = new Plot2DValue();
			var values = new MatrixValue[l.Length];

			for (var i = 0; i != l.Length; i++)
			{
                if (l.Values[i] is MatrixValue)
                {
                    values[i] = (MatrixValue)l.Values[i];
                }
                else
                {
                    throw new YAMPOperationInvalidException("plot", l.Values[i]);
                }
			}

			plot.AddPoints(m, n, values);
			return plot;
		}
	}
}
