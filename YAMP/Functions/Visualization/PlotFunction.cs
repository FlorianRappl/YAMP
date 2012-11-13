﻿using System;

namespace YAMP
{
    [Description("Visualizes a given set of points in form of a graph.")]
    class PlotFunction : VisualizationFunction
    {
        [Description("Performs the plot of a matrix. The first column is interpreted as x-values if more than one column is given. All other columns will be interpreted as y-values.")]
        [Example("plot(2^1:16)", "Plots the powers of 2 with y = 2, 4, 8, ..., 65536 at x = 1, 2, ..., 16 (since no X values are given).")]
        [Example("plot([0:10, 2^(0:2:20)])", "Plots the even powers of 2 with y = 1, 4, 16, ..., 2^20 at x = 0, 1, ..., 10.")]
        [Example("plot([0:10, 2^(0:2:20), 2^(1:2:21)])", "Plots the even and odd powers of 2 at x = 0, 1, ..., 10.")]
        public Plot2DValue Function(MatrixValue m)
        {
            var plot = new Plot2DValue();
            plot.AddPoints(m);
            return plot;
        }

        [Description("Performs the plot of a matrix. The first column is interpreted as x-values if it has only one column. In this case the columns of the second matrix are interpreted as a collection of y-values. Otherwise both matrices are viewed as a collection of y-values corresponding to a set of x-values.")]
        [Example("plot(0:15, 2^1:16)", "Plots the powers of 2 with y = 2, 4, 8, ..., 65536 at x = 0, 1, ..., 15.")]
        [Example("plot([1:11, 2^(1:2:21)], [0:10, 2^(0:2:20)])", "Plots the odd and even powers of 2 at different x-values.")]
        [Example("plot(0:0.01:2*pi, [sin(0:0.01:2*pi), cos(0:0.01:2*pi), 0:0.01:2*pi])", "Plots the values of a sin, cos and linear function with x-values from 0 to 2 Pi.")]
        public Plot2DValue Function(MatrixValue m, MatrixValue n)
        {
            var plot = new Plot2DValue();
            plot.AddPoints(m, n);
            return plot;
        }
    }
}