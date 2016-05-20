namespace YAMP
{
    [Description("HeatmapFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class HeatmapFunction : VisualizationFunction
    {
        public HeatmapFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("HeatmapFunctionDescriptionForMatrix")]
        [Example("heatmap(rand(50))", "HeatmapFunctionExampleForMatrix1")]
        public HeatmapPlotValue Function(MatrixValue Z)
        {
            var plot = new HeatmapPlotValue();
            plot.AddPoints(Z);
            return plot;
        }
    }
}
