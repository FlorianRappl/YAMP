using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Generates a matrix with binomial distributed random values. In probability theory and statistics, the binomial distribution is the discrete probability distribution of the number of successes in a sequence of n independent yes/no experiments, each of which yields success with probability p. Such a success/failure experiment is also called a Bernoulli experiment or Bernoulli trial; when n = 1, the binomial distribution is a Bernoulli distribution. The binomial distribution is the basis for the popular binomial test of statistical significance.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Binomial_distribution")]
    sealed class RandbFunction : ArgumentFunction
    {
        static readonly BinomialDistribution ran = new BinomialDistribution();

        [Description("Generates one binomial distributed random value with p set to 0.5 and n set to 1.")]
        [Example("randw()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public ScalarValue Function()
        {
            return new ScalarValue(Binomial());
        }

        [Description("Generates a n-by-n matrix with binomial distributed random values with p set to 0.5 and n set to 1.")]
        [Example("randw(3)", "Gives a 3x3 matrix with binomial dist. rand. values.")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("Generates a m-by-n matrix with binomial distributed random values with p set to 0.5 and n set to 1.")]
        [Example("randw(3, 1)", "Gives a 3x1 matrix with binomial dist. rand. values.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(0.5), new ScalarValue(1.0));
        }

        [Description("Generates a m-by-n matrix with binomial distributed random values with a custom p (scale) and n (shape) parameter.")]
        [Example("randw(3, 1, 0.5, 20)", "Gives a 3x1 matrix with binomial dist. rand. values around 10 with probability parameter p set to 0.5 and trials parameter n set to 20.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue p, ScalarValue n)
        {
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var nn = n.GetIntegerOrThrowException("n", Name);
            var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
                for (var j = 1; j <= k; j++)
                    m[j, i] = new ScalarValue(Binomial(p.Re, nn));

            return m;
        }

        double Binomial()
        {
            return Binomial(0.5, 1);
        }

        double Binomial(double p, int n)
        {
            ran.Alpha = p;
            ran.Beta = n;
            return ran.NextDouble();
        }
    }
}
