using System;

namespace YAMP
{
    [Description("A heatmap plot displays entries of matrix Z with a color. Even though numbers might be hard to read for large matrices, a heatmap can give a first impression about the structure of a matrix.")]
    [Kind(PopularKinds.Plot)]
    sealed class HeatmapFunction : VisualizationFunction
    {
        [Description("This draws a heatmap plot of matrix Z, where Z is the matrix to be plotted. The plot considers all entries of the matrix Z with their absolute values.")]
        [Example("heatmap(rand(50))", "Creates a heatmap plot of the given matrix 50x50 (uniform) random matrix.")]
        public HeatmapPlotValue Function(MatrixValue Z)
        {
            var plot = new HeatmapPlotValue();
            plot.AddPoints(Z);
            return plot;
        }
    }
}
