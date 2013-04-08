using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Generates a matrix with normal distributed random values. In probability theory, the normal (or Gaussian) distribution is a continuous probability distribution, defined on the entire real line, that has a bell-shaped probability density function, known as the Gaussian function or informally as the bell curve.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Normal_distribution")]
    sealed class RandnFunction : ArgumentFunction
	{	
		static readonly NormalDistribution ran = new NormalDistribution();

		[Description("Generates one normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		public ScalarValue Function()
		{
            return new ScalarValue(Gaussian());
		}

		[Description("Generates a n-by-n matrix with normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		[Example("randn(3)", "Gives a 3x3 matrix with normally dist. rand. values.")]
		public MatrixValue Function(ScalarValue dim)
		{
			return Function(dim, dim);
		}

		[Description("Generates a m-by-n matrix with normally (gaussian) distributed random value around 0 with standard deviation 1.")]
		[Example("randn(3, 1)", "Gives a 3x1 matrix with normally dist. rand. values.")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols)
		{
            return Function(rows, cols, new ScalarValue(), new ScalarValue(1.0));
		}

		[Description("Generates a m-by-n matrix with normally (gaussian) distributed random value around mu with standard deviation sigma.")]
		[Example("randn(3, 1, 10, 2.5)", "Gives a 3x1 matrix with normally dist. rand. values around 10 with standard deviation sigma.")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue mu, ScalarValue sigma)
		{
			var k = (int)rows.Re;
			var l = (int)cols.Re;
			var m = new MatrixValue(k, l);

			for (var i = 1; i <= l; i++)
				for (var j = 1; j <= k; j++)
                    m[j, i] = new ScalarValue(Gaussian(sigma.Re, mu.Re));

			return m;
		}

		double Gaussian()
		{
			return Gaussian(1.0, 0.0);
		}

		double Gaussian(double sigma, double mu)
		{
			ran.Sigma = sigma;
			ran.Mu = mu;

			return ran.NextDouble();
		}
	}
}

