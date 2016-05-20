namespace YAMP
{
    using System;

    [Description("HistogramFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    sealed class HistogramFunction : ArgumentFunction
    {
        [Description("HistogramFunctionDescriptionForMatrix")]
        [Example("histogram(rand(100, 1))", "HistogramFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue Y)
        {
            return Function(Y, new ScalarValue(10));
        }

        [Description("HistogramFunctionDescriptionForMatrixMatrix")]
        [Example("histogram(rand(100, 1), [0.1, 0.5, 0.9])", "HistogramFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue Y, MatrixValue x)
        {
            var X = new Double[x.Length];

            for (var i = 0; i < x.Length; i++)
            {
                X[i] = x[i + 1].Re;
            }

            if (!Y.IsVector)
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

                return M;
            }

            return YMath.Histogram(Y, X);
        }

        [Description("HistogramFunctionDescriptionForMatrixScalar")]
        [Example("histogram(rand(100, 1), 20)", "HistogramFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue Y, ScalarValue nbins)
        {
            var nn = nbins.GetIntegerOrThrowException("nbins", Name);

            if (!Y.IsVector)
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

                return M;
            }

            return YMath.Histogram(Y, nn);
        }
    }
}
