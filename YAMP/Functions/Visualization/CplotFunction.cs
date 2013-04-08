using System;

namespace YAMP
{
    [Description("In mathematics, a complex function is a function with the complex numbers (see the imaginary numbers and the complex plane) as both its domain and codomain. The complex plot assigns a color to each point of complex plane. the origin is white, 1 is red, −1 is cyan, and a point at the infinity is black.")]
    [Kind(PopularKinds.Plot)]
    sealed class CplotFunction : VisualizationFunction
    {
        [Description("Visualizes the given complex function f between -1 and 1 for the real and the imaginary axes.")]
        [Example("cplot(sin)", "Draws the complex plot of the sine function sin(z) with z = x + iy.")]
        [Example("cplot(z => sin(z) * cos(z))", "Draws the complex plot of sin(z) * cos(z) with z = x + iy.")]
        public ComplexPlotValue Function(FunctionValue f)
        {
            return Plot(f, -1.0, 1.0, -1.0, 1.0);
        }

        [Description("Visualizes the given complex function f between the given values for the real and the imaginary axes.")]
        [Example("cplot(sin, 0 - 1i, 2 * pi + 1i)", "Draws the complex plot of the sine function between 0 and 2pi on the real axis and -1 and 1 on the imaginary axes.")]
        public ComplexPlotValue Function(FunctionValue f, ScalarValue min, ScalarValue max)
        {
            return Plot(f, min.Re, max.Re, min.Im, max.Im);
        }

        [Description("Visualizes the given complex function f between the given values for the real and the imaginary axes.")]
        [Example("cplot(sin, 0, 2 * pi, -1, 1)", "Draws the complex plot of the sine function between 0 and 2pi on the real axis and -1 and 1 on the imaginary axes.")]
        public ComplexPlotValue Function(FunctionValue f, ScalarValue minX, ScalarValue maxX, ScalarValue minY, ScalarValue maxY)
        {
            return Plot(f, minX.Re, maxX.Re, minY.Re, maxY.Re);
        }

        public static ComplexPlotValue Plot(FunctionValue f, double minx, double maxx, double miny, double maxy)
        {
            var cp = new ComplexPlotValue();
            cp.SetFunction(f);
            cp.MinY = miny;
            cp.MaxY = maxy;
            cp.MinX = minx;
            cp.MaxX = maxx;
            return cp;
        }
    }
}
