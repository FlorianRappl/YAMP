using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Generates a matrix with Poisson distributed random values. In probability theory and statistics, the Poisson distribution is a discrete probability distribution that expresses the probability of a given number of events occurring in a fixed interval of time and / or space if these events occur with a known average rate and independently of the time since the last event. The Poisson distribution can also be used for the number of events in other specified intervals such as distance, area or volume.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Poisson_distribution")]
    sealed class RandpFunction : ArgumentFunction
    {
        static readonly PoissonDistribution ran = new PoissonDistribution();

        [Description("Generates one Poisson distributed random value with lambda set to 1.")]
        [Example("randp()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public ScalarValue Function()
        {
            return new ScalarValue(Poisson());
        }

        [Description("Generates a n-by-n matrix with Poisson distributed random values with lambda set to 1.")]
        [Example("randp(3)", "Gives a 3x3 matrix with Poisson dist. rand. values.")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("Generates a m-by-n matrix with Poisson distributed random values with lambda set to 1.")]
        [Example("randp(3, 1)", "Gives a 3x1 matrix with Poisson dist. rand. values.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0));
        }

        [Description("Generates a m-by-n matrix with Poisson distributed random values with a custom lambda (mean).")]
        [Example("randp(3, 1, 10)", "Gives a 3x1 matrix with Poisson dist. rand. values with the mean mu set to 10.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue lambda)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
                for (var j = 1; j <= n; j++)
                    m[j, i] = new ScalarValue(Poisson(lambda.Re));

            return m;
        }

        double Poisson()
        {
            return Poisson(1.0);
        }

        double Poisson(double lambda)
        {
            ran.Lambda = lambda;
            return ran.NextDouble();
        }
    }
}
