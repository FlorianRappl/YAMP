namespace YAMP
{
    [Description("PolarFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class PolarFunction : VisualizationFunction
	{
        public PolarFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("PolarFunctionDescriptionForMatrix")]
        [Example("polar(sin(0:0.1:2*pi))", "PolarFunctionExampleForMatrix1")]
        [Example("polar([0:0.1:2*pi, sin(0:0.1:2*pi)])", "PolarFunctionExampleForMatrix2")]
        [Example("polar([0:0.1:2*pi, sin(0:0.1:2*pi), cos(0:0.1:2*pi)])", "PolarFunctionExampleForMatrix3")]
		public PolarPlotValue Function(MatrixValue m)
		{
			var plot = new PolarPlotValue();
			plot.AddPoints(m);
			return plot;
		}

        [Description("PolarFunctionDescriptionForMatrixMatrix")]
        [Example("polar(0:0.1:2*pi, sin(0:0.1:2*pi))", "PolarFunctionExampleForMatrixMatrix1")]
        [Example("polar([0:0.1:2*pi, sin(0:0.1:2*pi)], [-pi/4:0.1:pi/4, tan(-pi/4:0.1:pi/4)])", "PolarFunctionExampleForMatrixMatrix2")]
        [Example("polar(-pi/4:0.1:pi/4, [sin(-pi/4:0.1:pi/4), cos(-pi/4:0.1:pi/4), tan(-pi/4:0.1:pi/4)])", "PolarFunctionExampleForMatrixMatrix3")]
		public PolarPlotValue Function(MatrixValue m, MatrixValue n)
		{
			var plot = new PolarPlotValue();
			plot.AddPoints(m, n);
			return plot;
		}
	}
}
