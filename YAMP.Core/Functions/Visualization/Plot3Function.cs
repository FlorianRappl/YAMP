using System;

namespace YAMP
{
    [Description("The plot3 function displays a three-dimensional plot of a set of data points.")]
    [Kind(PopularKinds.Plot)]
    sealed class Plot3Function : VisualizationFunction
    {
        [Description("Draws a set of points where X1, Y1, Z1 are vectors or matrices. Each set of coordinates for the i-th point is defined by the i-th value of X, Y and Z.")]
        [Example("t = 0:pi/50:10*pi; plot3(sin(t), cos(t), t)", "Plots a three-dimensional helix, i.e. a spring with uniform radius.")]
        public Plot3DValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z)
        {
            var p3 = new Plot3DValue();
            p3.AddPoints(X, Y, Z);
            return p3;
        }
    }
}
