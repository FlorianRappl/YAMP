using System;

namespace YAMP
{
	[Description("A histogram shows the distribution of data values.")]
	[Kind(PopularKinds.Plot)]
	class HistFunction : VisualizationFunction
	{
		[Description("Bins the elements in vector Y into 10 equally spaced containers and plots the number of elements in each container as a row vector. If Y is an m-by-p matrix, hist treats the columns of Y as vectors and plots a 10-by-p matrix n. Each column of n contains the results for the corresponding column of Y. No elements of Y can be complex or of type integer.")]
		[Example("hist(rand(100, 1))", "Places 100 uniformly generated random numbers into 10 bins with a spacing that should be approximately 0.1.")]
		public BarPlotValue Function(MatrixValue Y)
		{
			return Function(Y, new ScalarValue(10.0));
		}

		[Description("Here x is a vector, such that the distribution of Y among length(x) bins with centers specified by x. For example, if x is a 5-element vector, hist distributes the elements of Y into five bins centered on the x-axis at the elements in x, none of which can be complex.")]
		[Example("hist(rand(100, 1), [0.1, 0.5, 0.9])", "Places 100 uniformly generated random numbers into 3 bins that center around 0.1, 0.5 and 0.9.")]
		public BarPlotValue Function(MatrixValue Y, MatrixValue x)
		{
			var bp = new BarPlotValue();
			var X = new double[x.Length];

			for (var i = 0; i < x.Length; i++)
				X[i] = x[i + 1].Value;

			if (Y.IsVector)
				bp.AddPoints(Histogram(Y, X));
			else
			{
				var M = new MatrixValue();

				for (var i = 1; i <= Y.DimensionX; i++)
				{
					var N = Histogram(Y.GetColumnVector(i), X);

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
				bp.AddPoints(Histogram(Y, nn));
			else
			{
				var M = new MatrixValue();

				for (var i = 1; i <= Y.DimensionX; i++)
				{
					var N = Histogram(Y.GetColumnVector(i), nn);

					for (var j = 1; j <= N.Length; j++)
						M[j, i] = N[j];
				}

				bp.AddPoints(M);
			}

			return bp;
		}

		static MatrixValue Histogram(MatrixValue v, double[] centers)
		{
            if (centers.Length == 0)
                throw new YAMPWrongLengthException(0, 1);

			var H = new MatrixValue(centers.Length, 1);
			var N = new int[centers.Length];
			var last = centers.Length - 1;

			for (var i = 1; i <= v.Length; i++)
			{
				var y = v[i].Value;

				if (y < centers[0])
					N[0]++;
				else if (y > centers[last])
					N[last]++;
				else
				{
					var min = double.MaxValue;
					var index = 0;

					for (var j = 0; j < centers.Length; j++)
					{
						var dist = Math.Abs(y - centers[j]);

						if (dist < min)
						{
							index = j;
							min = dist;
						}
					}

					N[index]++;
				}
			}

			for (var i = 1; i <= centers.Length; i++)
				H[i, 1] = new ScalarValue(N[i - 1]);

			return H;
		}

		static MatrixValue Histogram(MatrixValue v, int nbins)
		{
			var min = double.MaxValue;
			var max = double.MinValue;

			for (var i = 1; i <= v.Length; i++)
			{
				if (v[i].Value > max)
					max = v[i].Value;

				if (v[i].Value < min)
					min = v[i].Value;
			}

			var delta = (max - min) / nbins;
			var D = new double[nbins];

			for (var i = 0; i < nbins; i++)
				D[i] = delta * (i + 0.5);

			return Histogram(v, D);
		}
	}
}
