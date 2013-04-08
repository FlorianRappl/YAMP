using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Generates a matrix with exponentially distributed random values. In probability theory and statistics, the exponential distribution (a.k.a. negative exponential distribution) is a family of continuous probability distributions. It describes the time between events in a Poisson process, i.e. a process in which events occur continuously and independently at a constant average rate. It is the continuous analogue of the geometric distribution.")]
    [Kind(PopularKinds.Random)]
    [Link("http://en.wikipedia.org/wiki/Exponential_distribution")]
    sealed class RandeFunction : ArgumentFunction
	{
		static readonly ExponentialDistribution ran = new ExponentialDistribution();

		[Description("Generates one exponentially distributed random value around 0 with lambda 1.")]
		[Example("rande()", "Outputs a scalar that has been generated with respect to the distribution.")]
		public ScalarValue Function()
		{
			return new ScalarValue(Exponential());
		}

		[Description("Generates a n-by-n matrix with exponentially distributed random values with lambda set to 1.")]
		[Example("rande(3)", "Gives a 3x3 matrix with normally dist. rand. values.")]
		public MatrixValue Function(ScalarValue dim)
        {
			return Function(dim, dim);
		}

		[Description("Generates a m-by-n matrix with exponentially distributed random values with lambda set to 1.")]
		[Example("rande(3, 1)", "Gives a 3x1 matrix with normally dist. rand. values.")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols)
		{
            return Function(rows, cols, new ScalarValue(1.0));
		}

		[Description("Generates a m-by-n matrix with exponentially distributed random values that have been generated with a specified lambda.")]
		[Example("rande(3, 1, 2.5)", "Gives a 3x1 matrix with exponentially distributed random values with lambda = 2.5.")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue lambda)
        {
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
			var m = new MatrixValue(k, l);

			for (var i = 1; i <= l; i++)
				for (var j = 1; j <= k; j++)
                    m[j, i] = new ScalarValue(Exponential(lambda.Re));

			return m;
		}

		double Exponential()
		{
			return Exponential(1.0);
		}

		double Exponential(double lambda)
		{
			ran.Lambda = lambda;

			return ran.NextDouble();
		}
	}
}

