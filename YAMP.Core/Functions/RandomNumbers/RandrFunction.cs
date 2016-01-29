using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Generates a matrix with Rayleigh distributed random values. In probability theory and statistics, the Rayleigh distribution is a continuous probability distribution. A Rayleigh distribution is often observed when the overall magnitude of a vector is related to its directional components. One example where the Rayleigh distribution naturally arises is when wind velocity is analyzed into its orthogonal 2-dimensional vector components. Assuming that the magnitude of each component is uncorrelated, normally distributed with equal variance, and zero mean, then the overall wind speed (vector magnitude) will be characterized by a Rayleigh distribution. A second example of the distribution arises in the case of random complex numbers whose real and imaginary components are independently and identically distributed Gaussian with equal variance and zero mean. In that case, the absolute value of the complex number is Rayleigh-distributed. The distribution is named after Lord Rayleigh.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Rayleigh_distribution")]
    sealed class RandrFunction : ArgumentFunction
    {
        static readonly RayleighDistribution ran = new RayleighDistribution();

        [Description("Generates one Rayleigh distributed random value with sigma set to 1.")]
        [Example("randr()", "Outputs a scalar that has been generated with respect to the distribution.")]
        public ScalarValue Function()
        {
            return new ScalarValue(Rayleigh());
        }

        [Description("Generates a n-by-n matrix with Rayleigh distributed random values with sigma set to 1.")]
        [Example("randr(3)", "Gives a 3x3 matrix with Rayleigh dist. rand. values.")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("Generates a m-by-n matrix with Rayleigh distributed random values with sigma set to 1.")]
        [Example("randr(3, 1)", "Gives a 3x1 matrix with Rayleigh dist. rand. values.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0));
        }

        [Description("Generates a m-by-n matrix with Rayleigh distributed random values with a custom sigma (mode).")]
        [Example("randr(3, 1, 10)", "Gives a 3x1 matrix with Rayleigh dist. rand. values with the mode sigma set to 10.")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue sigma)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
                for (var j = 1; j <= n; j++)
                    m[j, i] = new ScalarValue(Rayleigh(sigma.Re));

            return m;
        }

        double Rayleigh()
        {
            return Rayleigh(1.0);
        }

        double Rayleigh(double sigma)
        {
            ran.Sigma = sigma;
            return ran.NextDouble();
        }
    }
}
