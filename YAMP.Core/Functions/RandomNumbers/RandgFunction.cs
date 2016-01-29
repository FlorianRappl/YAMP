using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Generates a matrix with gamma distributed random values. In probability theory and statistics, the gamma distribution is a two-parameter family of continuous probability distributions.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Gamma_distribution")]
    sealed class RandgFunction : ArgumentFunction
    {
        static readonly GammaDistribution ran = new GammaDistribution();

        [Description("Generates one gamma distributed random value with theta and k set to 1.")]
        [Example("randg()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public ScalarValue Function()
        {
            return new ScalarValue(Gamma());
        }

        [Description("Generates a n-by-n matrix with gamma distributed random values with theta and k set to 1.")]
        [Example("randg(3)", "Gives a 3x3 matrix with gamma dist. rand. values.")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("Generates a m-by-n matrix with gamma distributed random values with theta and k set to 1.")]
        [Example("randg(3, 1)", "Gives a 3x1 matrix with gamma dist. rand. values.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0), new ScalarValue(1.0));
        }

        [Description("Generates a m-by-n matrix with gamma distributed random values with a custom theta (scale) and k (shape) parameter.")]
        [Example("randg(3, 1, 10, 2.5)", "Gives a 3x1 matrix with gamma dist. rand. values around 10 with scale parameter theta set to 10 and shape parameter k set to 2.5.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue theta, ScalarValue k)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
                for (var j = 1; j <= n; j++)
                    m[j, i] = new ScalarValue(Gamma(theta.Re, k.Re));

            return m;
        }

        double Gamma()
        {
            return Gamma(1.0, 1.0);
        }

        double Gamma(double theta, double k)
        {
            ran.Theta = theta;
            ran.Alpha = k;
            return ran.NextDouble();
        }
    }
}
