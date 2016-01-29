using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Generates a matrix with Weibull distributed random values. In probability theory and statistics, the Weibull distribution is a continuous probability distribution.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Weibull_distribution")]
    sealed class RandwFunction : ArgumentFunction
    {
        static readonly WeibullDistribution ran = new WeibullDistribution();

        [Description("Generates one Weibull distributed random value with lambda and k set to 1.")]
        [Example("randw()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public ScalarValue Function()
        {
            return new ScalarValue(Weibull());
        }

        [Description("Generates a n-by-n matrix with Weibull distributed random values with lambda and k set to 1.")]
        [Example("randw(3)", "Gives a 3x3 matrix with Weibull dist. rand. values.")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("Generates a m-by-n matrix with Weibull distributed random values with lambda and k set to 1.")]
        [Example("randw(3, 1)", "Gives a 3x1 matrix with Weibull dist. rand. values.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0), new ScalarValue(1.0));
        }

        [Description("Generates a m-by-n matrix with Weibull distributed random values with a custom lambda (scale) and k (shape) parameter.")]
        [Example("randw(3, 1, 10, 2.5)", "Gives a 3x1 matrix with Weibull dist. rand. values around 10 with scale parameter lambda set to 10 and shape parameter k set to 2.5.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue lambda, ScalarValue k)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
                for (var j = 1; j <= n; j++)
                    m[j, i] = new ScalarValue(Weibull(lambda.Re, k.Re));

            return m;
        }

        double Weibull()
        {
            return Weibull(1.0, 1.0);
        }

        double Weibull(double lambda, double k)
        {
            ran.Lambda = lambda;
            ran.Alpha = k;
            return ran.NextDouble();
        }
    }
}
