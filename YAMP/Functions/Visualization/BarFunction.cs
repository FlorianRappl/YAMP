using System;

namespace YAMP
{
    [Description("A bar chart or bar graph is a chart with rectangular bars with lengths proportional to the values that they represent. The bars can be plotted vertically or horizontally. Bar charts provide a visual presentation of categorical data. Categorical data is a grouping of data into discrete groups, such as months of the year, age group, shoe sizes, and animals. In a column bar chart, the categories appear along the horizontal axis; the height of the bar corresponds to the value of each category.")]
    [Kind(PopularKinds.Plot)]
    class BarFunction : VisualizationFunction
    {
        [Description("This function draws one bar for each element in Y.")]
        [Example("bar([5, 2, 9, 6, 19])", "")]
        public BarPlotValue Function(MatrixValue Y)
        {
            var bp = new BarPlotValue();
            bp.AddPoints(Y);
            return bp;
        }
    }
}
