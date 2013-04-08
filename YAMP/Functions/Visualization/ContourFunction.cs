using System;

namespace YAMP
{
	[Description("A contour plot displays isolines of matrix Z. A contour line (also isoline, isopleth, or isarithm) of a function of two variables is a curve along which the function has a constant value.")]
	[Kind(PopularKinds.Plot)]
	sealed class ContourFunction : VisualizationFunction
	{
		[Description("This draws a contour plot of matrix Z, where Z is interpreted as heights with respect to the x-y plane. Z must be at least a 2-by-2 matrix that contains at least two different values. The number of contour lines and the values of the contour lines are chosen automatically based on the minimum and maximum values of Z. The ranges of the x- and y-axis are [1:n] and [1:m], where [m,n] = size(Z).")]
		[Example("contour([1, 2, 3; 4, 5, 6; 7, 8, 9])", "Creates a contour plot of the given matrix.")]
		public ContourPlotValue Function(MatrixValue Z)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(Z);
			return plot;
		}

		[Description("Draws a contour plot of matrix Z with n contour levels where n is an integer scalar.")]
		[Example("contour([1, 2, 3; 4, 5, 6; 7, 8, 9], 2)", "Creates a contour plot of the given matrix.")]
		public ContourPlotValue Function(MatrixValue Z, ScalarValue n)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
			var plot = new ContourPlotValue();
			plot.AddPoints(Z);
			plot.SetLevels(nn);
			return plot;
		}

		[Description("This draws a contour plot of matrix Z with contour lines at the data values specified in the monotonically increasing vector v. The number of contour levels is equal to length(v). To draw a single contour of level i, use contour(Z,[i i]).")]
		[Example("contour([1, 2, 3; 4, 5, 6; 7, 8, 9], [1, 2])", "Creates a contour plot of the given matrix.")]
		public ContourPlotValue Function(MatrixValue Z, MatrixValue v)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(Z);
			plot.SetLevels(v);
			return plot;
		}

		[Description("Draw contour plots of Z using X and Y to determine the x- and y-axis limits. When X and Y are matrices, they must be the same size as Z and must be monotonically increasing.")]
		public ContourPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(X, Y, Z);
			return plot;
		}

		[Description("Draw contour plots of Z with n contour levels where n is an integer scalar using X and Y to determine the x- and y-axis limits. When X and Y are matrices, they must be the same size as Z and must be monotonically increasing.")]
		public ContourPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z, ScalarValue n)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
			var plot = new ContourPlotValue();
			plot.AddPoints(X, Y, Z);
			plot.SetLevels(nn);
			return plot;
		}

		[Description("Draw contour plots of Z with contour lines at the data values specified in the monotonically increasing vector v using X and Y to determine the x- and y-axis limits. When X and Y are matrices, they must be the same size as Z and must be monotonically increasing.")]
		public ContourPlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z, MatrixValue v)
		{
			var plot = new ContourPlotValue();
			plot.AddPoints(X, Y, Z);
			plot.SetLevels(v);
			return plot;
		}
	}
}
