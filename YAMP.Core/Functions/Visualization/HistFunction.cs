using System;

namespace YAMP
{
	[Description("Plots the histogram that shows the distribution of of given values.")]
	[Kind(PopularKinds.Plot)]
	sealed class HistFunction : VisualizationFunction
	{
		[Description("Bins the elements in vector Y into 10 equally spaced containers and plots the number of elements in each container as a row vector. If Y is an m-by-p matrix, hist treats the columns of Y as vectors and plots a 10-by-p matrix n. Each column of n contains the results for the corresponding column of Y. No elements of Y can be complex or of type integer.")]
		[Example("hist(rand(100, 1))", "Places 100 uniformly generated random numbers into 10 bins with a spacing that should be approximately 0.1.")]
		public BarPlotValue Function(MatrixValue Y)
		{
			return Function(Y, new ScalarValue(10));
		}

		[Description("Here x is a vector, such that the distribution of Y among length(x) bins with centers specified by x. For example, if x is a 5-element vector, hist distributes the elements of Y into five bins centered on the x-axis at the elements in x, none of which can be complex.")]
		[Example("hist(rand(100, 1), [0.1, 0.5, 0.9])", "Places 100 uniformly generated random numbers into 3 bins that center around 0.1, 0.5 and 0.9.")]
		public BarPlotValue Function(MatrixValue Y, MatrixValue x)
		{
			var bp = new BarPlotValue();
			var X = new double[x.Length];

			for (var i = 0; i < x.Length; i++)
				X[i] = x[i + 1].Re;

			if (Y.IsVector)
                bp.AddPoints(YMath.Histogram(Y, X));
			else
			{
				var M = new MatrixValue();

				for (var i = 1; i <= Y.DimensionX; i++)
				{
					var N = YMath.Histogram(Y.GetColumnVector(i), X);

					for (var j = 1; j <= N.Length; j++)
						M[j, i] = N[j];
				}

				bp.AddPoints(M);
			}

			return bp;
		}

		[Description("Bins the elements in vector Y into nbins equally spaced containers and plots the number of elements as before.")]
		[Example("hist(rand(100, 1), 20)", "Places 100 uniformly generated random numbers into 20 bins with a spacing that should be approximately 0.05.")]
		public BarPlotValue Function(MatrixValue Y, ScalarValue nbins)
        {
            var nn = nbins.GetIntegerOrThrowException("nbins", Name);
			var bp = new BarPlotValue();

			if (Y.IsVector)
                bp.AddPoints(YMath.Histogram(Y, nn));
			else
			{
				var M = new MatrixValue();

				for (var i = 1; i <= Y.DimensionX; i++)
				{
                    var N = YMath.Histogram(Y.GetColumnVector(i), nn);

					for (var j = 1; j <= N.Length; j++)
						M[j, i] = N[j];
				}

				bp.AddPoints(M);
			}

			return bp;
		}
	}
}
