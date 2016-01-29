using System;

namespace YAMP
{
	[Description("Visualizes a given function in form of a graph.")]
	[Kind(PopularKinds.Plot)]
	sealed class FplotFunction : VisualizationFunction
	{
		[Description("Visualizes the given function f between -1 and 1 for the x-axis.")]
		[Example("fplot(sin)", "Draws the real plot of the sine function sin(x) with x between -1 and 1.")]
		[Example("fplot(x => sin(x) * cos(x))", "Draws the real plot of sin(x) * cos(x) with x between -1 and 1.")]
		public Plot2DValue Function(FunctionValue f)
		{
			return Plot(f, -1.0, 1.0, 0.05);
		}

		[Description("Visualizes the given function f between the given values for x-axis.")]
		[Example("fplot(sin, 0, 2 * pi)", "Draws the plot of the sine function between 0 and 2pi.")]
		public Plot2DValue Function(FunctionValue f, ScalarValue min, ScalarValue max)
		{
			return Plot(f, min.Re, max.Re, 0.05);
		}

		[Description("Visualizes the given function f between the given values for the x-axes with the given precision.")]
		[Example("fplot(sin, 0, 2 * pi, 10^-3)", "Draws the plot of the sine function between 0 and 2pi with a precision of 0.001.")]
		public Plot2DValue Function(FunctionValue f, ScalarValue min, ScalarValue max, ScalarValue precision)
		{
			return Plot(f, min.Re, max.Re, precision.Re);
		}

		Plot2DValue Plot(IFunction f, double minx, double maxx, double precision)
		{
			var cp = new Plot2DValue();
			var N = (int)((maxx - minx) / precision) + 1;
			var M = new MatrixValue(N, 2);
			var x = new ScalarValue(minx);

			for (var i = 0; i < N; i++)
			{
				var row = i + 1;
				var y = f.Perform(Context, x);
				M[row, 1] = x.Clone();

				if (y is ScalarValue)
					M[row, 2] = (ScalarValue)y;
				else if (y is MatrixValue)
				{
					var Y = (MatrixValue)y;

					for (var j = 1; j <= Y.Length; j++)
						M[row, j + 1] = Y[j];
				}

				x.Re += precision;
			}

			cp.AddPoints(M);
			return cp;
		}
	}
}
