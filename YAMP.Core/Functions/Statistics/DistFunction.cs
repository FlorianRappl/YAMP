using System;

namespace YAMP
{
    [Description("Computes an approximation of the distribution of data values.")]
    [Kind(PopularKinds.Statistic)]
    sealed class DistFunction : ArgumentFunction
    {
        [Description("Returns a function which approximates the distribution of the data values in Y. For that Y is binned into nbins equally spaced containers. The approximation uses nParameters parameters to describe the data.")]
        [Example("dist([randn(500, 1); randn(1000, 1) + 5], 40, 10)", "Returns an approximate of the distribution of the data (two gaussians centered at 0 and 5 and relative height 1:2) binned in 40 bins and with 10 parameters.")]
        public FunctionValue Function(MatrixValue Y, ScalarValue nbins, ScalarValue nParameters)
        {
            var nn = nbins.GetIntegerOrThrowException("nbins", Name);
            var nP = nParameters.GetIntegerOrThrowException("nParameters", Name);
            var N = Y.Length;
            var min_idx = Y.Min();
            var min = Y[min_idx.Row, min_idx.Column];
            var max_idx = Y.Max();
            var max = Y[max_idx.Row, max_idx.Column];
            var median = YMath.Median(Y);
            
            var variance = ScalarValue.Zero;
            var mean = Y.Sum() / Y.Length;

            for (int i = 1; i <= Y.Length; i++)
                variance += (Y[i] - mean).Square();

            variance /= Y.Length;

            var delta = (max - min) / nn;

            var x = new MatrixValue(nn, 1);

            for(int i = 0; i < nn; i++)
                x[i+1] = min + delta * i;

            var histogram = new HistogramFunction();
            var fx = histogram.Function(Y, x);
            var linearfit = new LinfitFunction();

            var dist = linearfit.Function(x, fx, new FunctionValue((context, argument) =>
            {
                var _x = (argument as ScalarValue - median / 2) / (variance / 4);
                var _exp_x_2 = (-_x * _x).Exp();
                var result = new MatrixValue(1, nP - 1);

                for (int i = 0; i < nP - 1; i++)
                    result[i + 1] = _exp_x_2 * _x.Pow(new ScalarValue(i));

                return result;
            }, true));

            var norm = Y.Length * (max - min) / nbins;
            var normed_dist = new FunctionValue((context, argument) =>
            {
                var temp = dist.Perform(context, argument);

                if (temp is ScalarValue)
                    return (temp as ScalarValue) / norm;
                else if (temp is MatrixValue)
                    return (temp as MatrixValue) / norm;
                else
                    throw new YAMPOperationInvalidException();
            }, true);

            return normed_dist;
        }

        [Description("Returns a function which approximates the distribution of the data values in Y. For that Y is binned into nbins equally spaced containers. The approximation uses nParameters parameters to describe the data.")]
        [Example("dist([randn(500, 1); randn(1000, 1) + 5])", "Returns an approximate of the distribution of the data (two gaussians centered at 0 and 5 and relative height 1:2) binned in about sqrt(1500) bins and about log(1500) parameters.")]
        public FunctionValue Function(MatrixValue Y)
        {
            ScalarValue nbins = new ScalarValue(Math.Round(Math.Sqrt(Y.Length)));
            ScalarValue nParameters = new ScalarValue(Math.Round(Math.Log(Y.Length)));
            return Function(Y, nbins, nParameters);
        }
    }
}
