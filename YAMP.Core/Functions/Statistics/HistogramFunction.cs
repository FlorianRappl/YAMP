using System;

namespace YAMP
{
    [Description("Computes a histogram that shows the distribution of data values.")]
    [Kind(PopularKinds.Statistic)]
    sealed class HistogramFunction : ArgumentFunction
    {
        [Description("Bins the elements in vector Y into 10 equally spaced containers and outputs the number of elements in each container in a row vector. If Y is an m-by-p matrix, histogram treats the columns of Y as vectors and outputs a 10-by-p matrix n. Each column of n contains the results for the corresponding column of Y. No elements of Y can be complex or of type integer.")]
        [Example("histogram(rand(100, 1))", "Places 100 uniformly generated random numbers into 10 bins with a spacing that should be approximately 0.1.")]
        public MatrixValue Function(MatrixValue Y)
        {
            return Function(Y, new ScalarValue(10));
        }

        [Description("Here x is a vector, such that the distribution of Y among length(x) bins with centers specified by x. For example, if x is a 5-element vector, histogram distributes the elements of Y into five bins centered on the x-axis at the elements in x, none of which can be complex.")]
        [Example("histogram(rand(100, 1), [0.1, 0.5, 0.9])", "Places 100 uniformly generated random numbers into 3 bins that center around 0.1, 0.5 and 0.9.")]
        public MatrixValue Function(MatrixValue Y, MatrixValue x)
        {
            var X = new double[x.Length];

            for (var i = 0; i < x.Length; i++)
                X[i] = x[i + 1].Re;

            if (Y.IsVector)
                return YMath.Histogram(Y, X);

            var M = new MatrixValue();

            for (var i = 1; i <= Y.DimensionX; i++)
            {
                var N = YMath.Histogram(Y.GetColumnVector(i), X);

                for (var j = 1; j <= N.Length; j++)
                    M[j, i] = N[j];
            }

            return M;
        }

        [Description("Bins the elements in vector Y into nbins equally spaced containers and outputs the number of elements as before.")]
        [Example("histogram(rand(100, 1), 20)", "Places 100 uniformly generated random numbers into 20 bins with a spacing that should be approximately 0.05.")]
        public MatrixValue Function(MatrixValue Y, ScalarValue nbins)
        {
            var nn = nbins.GetIntegerOrThrowException("nbins", Name);

            if (Y.IsVector)
                return YMath.Histogram(Y, nn);

            var M = new MatrixValue();

            for (var i = 1; i <= Y.DimensionX; i++)
            {
                var N = YMath.Histogram(Y.GetColumnVector(i), nn);

                for (var j = 1; j <= N.Length; j++)
                    M[j, i] = N[j];
            }

            return M;
        }
    }
}
