using System;

namespace YAMP
{
    [Description("Subplot divides the current figure into rectangular panes that are numbered rowwise. Each pane contains an axes object. Subsequent plots are output to the current pane.")]
    [Kind(PopularKinds.Plot)]
    sealed class SubplotFunction : VisualizationFunction
    {
        [Description("Creates a uniform subplot grid with the specified number of rows and columns.")]
        [Example("a = subplot(4, 3)", "Creates a uniform 4x3 plot-grid and saves the grid in the variable a. See the other examples for usage information.")]
        [Example("a = subplot(4, 3); a(1, 1) = plot(1:100, rand(100, 4))", "Assigns a simple 2D plot with 4 series consisting of 100 random values to row 1, column 1 of the plot-grid.")]
        [Example("a = subplot(4, 3); a(2:4, 1:3) = polar(0:pi/100:2*pi, sin(0:pi/100:2*pi))", "Assigns a polar plot to row 1, column 1 with rowspan 3 and columnspan 3.")]
        [Example("a = subplot(4, 3); set(a(1, 1), \"title\", \"Example 1\")", "Sets the title of the plot contained in column 1, row 1.")]
        public SubPlotValue Function(ScalarValue rows, ScalarValue columns)
        {
            var r = rows.GetIntegerOrThrowException("rows", Name);
            var c = columns.GetIntegerOrThrowException("columns", Name);
            var subplot = new SubPlotValue();
            subplot.Rows = r;
            subplot.Columns = c;
            return subplot;
        }
    }
}
