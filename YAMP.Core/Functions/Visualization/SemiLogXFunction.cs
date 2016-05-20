namespace YAMP
{
    [Description("SemiLogXFunctionDescription")]
	[Kind(PopularKinds.Plot)]
    sealed class SemiLogXFunction : VisualizationFunction
	{
        public SemiLogXFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("SemiLogXFunctionDescriptionForMatrix")]
        [Example("semilogx(2^1:16)", "SemiLogXFunctionExampleForMatrix1")]
        [Example("semilogx([0:10, 2^(0:2:20)])", "SemiLogXFunctionExampleForMatrix2")]
        [Example("semilogx([0:10, 2^(0:2:20), 2^(1:2:21)])", "SemiLogXFunctionExampleForMatrix3")]
		public Plot2DValue Function(MatrixValue m)
        {
            var plot = new Plot2DValue();
            plot.AddPoints(m);
			plot.IsLogX = true;
			return plot;
		}

        [Description("SemiLogXFunctionDescriptionForMatrixMatrix")]
        [Example("semilogx(0:15, 2^1:16)", "SemiLogXFunctionExampleForMatrixMatrix1")]
        [Example("semilogx([1:11, 2^(1:2:21)], [0:10, 2^(0:2:20)])", "SemiLogXFunctionExampleForMatrixMatrix2")]
        [Example("semilogx(0:0.01:2*pi, [sin(0:0.01:2*pi), cos(0:0.01:2*pi), 0:0.01:2*pi])", "SemiLogXFunctionExampleForMatrixMatrix3")]
		public Plot2DValue Function(MatrixValue m, MatrixValue n)
        {
            var plot = new Plot2DValue();
            plot.AddPoints(m, n);
			plot.IsLogX = true;
			return plot;
		}
	}
}
