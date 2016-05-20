namespace YAMP
{
    using System;

    [Description("HistFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class HistFunction : VisualizationFunction
	{
        public HistFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("HistFunctionDescriptionForMatrix")]
        [Example("hist(rand(100, 1))", "HistFunctionExampleForMatrix1")]
		public BarPlotValue Function(MatrixValue Y)
		{
			return Function(Y, new ScalarValue(10));
		}

        [Description("HistFunctionDescriptionForMatrixMatrix")]
        [Example("hist(rand(100, 1), [0.1, 0.5, 0.9])", "HistFunctionExampleForMatrixMatrix1")]
		public BarPlotValue Function(MatrixValue Y, MatrixValue x)
		{
			var bp = new BarPlotValue();
			var X = new Double[x.Length];

            for (var i = 0; i < x.Length; i++)
            {
                X[i] = x[i + 1].Re;
            }

            if (Y.IsVector)
            {
                bp.AddPoints(YMath.Histogram(Y, X));
            }
            else
            {
                var M = new MatrixValue();

                for (var i = 1; i <= Y.DimensionX; i++)
                {
                    var N = YMath.Histogram(Y.GetColumnVector(i), X);

                    for (var j = 1; j <= N.Length; j++)
                    {
                        M[j, i] = N[j];
                    }
                }

                bp.AddPoints(M);
            }

			return bp;
		}

        [Description("HistFunctionDescriptionForMatrixScalar")]
        [Example("hist(rand(100, 1), 20)", "HistFunctionExampleForMatrixScalar1")]
		public BarPlotValue Function(MatrixValue Y, ScalarValue nbins)
        {
            var nn = nbins.GetIntegerOrThrowException("nbins", Name);
			var bp = new BarPlotValue();

            if (Y.IsVector)
            {
                bp.AddPoints(YMath.Histogram(Y, nn));
            }
            else
            {
                var M = new MatrixValue();

                for (var i = 1; i <= Y.DimensionX; i++)
                {
                    var N = YMath.Histogram(Y.GetColumnVector(i), nn);

                    for (var j = 1; j <= N.Length; j++)
                    {
                        M[j, i] = N[j];
                    }
                }

                bp.AddPoints(M);
            }

			return bp;
		}
	}
}
