using System;

namespace YAMP
{
	[Description("Visualizes a given set of points in form of a polar plot.")]
	[Kind(PopularKinds.Plot)]
	sealed class PolarFunction : VisualizationFunction
	{
		[Description("Performs the polar plot of a matrix. The first column is interpreted as x-values if more than one column is given. All other columns will be interpreted as y-values.")]
		[Example("polar(sin(0:0.1:2*pi))", "Plots the values of the sine function in a polar (circular) plot.")]
		[Example("polar([0:0.1:2*pi, sin(0:0.1:2*pi)])", "Plots the values of sine in a polar (circular) plot.")]
		[Example("polar([0:0.1:2*pi, sin(0:0.1:2*pi), cos(0:0.1:2*pi)])", "Plots the values of sine and cosine in a polar (circular) plot.")]
		public PolarPlotValue Function(MatrixValue m)
		{
			var plot = new PolarPlotValue();
			plot.AddPoints(m);
			return plot;
		}

		[Description("Performs the polar plot of a matrix. The first column is interpreted as x-values if it has only one column. In this case the columns of the second matrix are interpreted as a collection of y-values. Otherwise both matrices are viewed as a collection of y-values corresponding to a set of x-values.")]
		[Example("polar(0:0.1:2*pi, sin(0:0.1:2*pi))", "Plots the values of sine in a polar (circular) plot.")]
		[Example("polar([0:0.1:2*pi, sin(0:0.1:2*pi)], [-pi/4:0.1:pi/4, tan(-pi/4:0.1:pi/4)])", "Plots the sine and tangent functions at different angles in a polar (circular) plot.")]
		[Example("polar(-pi/4:0.1:pi/4, [sin(-pi/4:0.1:pi/4), cos(-pi/4:0.1:pi/4), tan(-pi/4:0.1:pi/4)])", "Plots the values of a sin, cos and tan with angles from -Pi/4 to Pi/4 in a polar (circular) plot.")]
		public PolarPlotValue Function(MatrixValue m, MatrixValue n)
		{
			var plot = new PolarPlotValue();
			plot.AddPoints(m, n);
			return plot;
		}
	}
}
