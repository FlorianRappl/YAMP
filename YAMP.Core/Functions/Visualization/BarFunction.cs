namespace YAMP
{
    [Description("BarFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class BarFunction : VisualizationFunction
    {
        public BarFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("BarFunctionDescriptionForMatrix")]
        [Example("bar([5, 2, 9, 6, 19])", "BarFunctionExampleForMatrix1")]
        [Example("bar([4, 9; 5, 9; 4, 8; 3, 7])", "BarFunctionExampleForMatrix2")]
        public BarPlotValue Function(MatrixValue Y)
        {
            var bp = new BarPlotValue();
            bp.AddPoints(Y);
            return bp;
        }
    }
}
