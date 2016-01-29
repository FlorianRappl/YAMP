using System;

namespace YAMP
{
    [Description("Visualizes a given set of points with errors in form of a graph with error bars.")]
    [Kind(PopularKinds.Plot)]
    sealed class ErrorbarFunction : VisualizationFunction
    {
        [Description("Performs the plot of a matrix with errors. All columns are interpreted as y-values if more than one column is given. The error matrix gives the y-errors (and optionally the x-errors) for the plot.")]
        [Example("errorbar(2^1:16, 0.3 * ones(16, 1))", "Plots the powers of 2 with y = 2, 4, 8, ..., 65536 at x = 1, 2, ..., 16 (since no X values are given) with an error of 0.2.")]
        [Example("errorbar([0:10, 2^(0:2:20), 2^(1:2:21)], [100000 * ones(16, 1), 0.5 * ones(16, 1)])", "Plots the even and odd powers of 2 at x = 0, 1, ..., 10 with an y-error of 100000 and an x-error of 0.5.")]
        public ErrorPlotValue Function(MatrixValue Y, MatrixValue E)
        {
            var plot = new ErrorPlotValue();
            plot.AddPoints(Y, E);
            return plot;
        }

        [Description("Performs the plot of a matrix. The first column is interpreted as x-values if it has only one column. In this case the columns of the second matrix are interpreted as a collection of y-values. Otherwise both matrices are viewed as a collection of y-values corresponding to a set of x-values.")]
        [Example("errorbar(-7:7, rand(15, 1), 0.1 * ones(15, 1))", "Plots some random numbers at x = -7, -6, ..., 7 with an error of 0.1.")]
        [Example("errorbar(0:pi/10:2*pi, [sin(0:pi/10:2*pi), cos(0:pi/10:2*pi)], rand(20, 1))", "Plots the values of a sin, and cos with x-values from 0 to 2 Pi and a random error.")]
        public ErrorPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue E)
        {
            var plot = new ErrorPlotValue();
            plot.AddPoints(X, Y, E);
            return plot;
        }
    }
}
