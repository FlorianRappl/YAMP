using System;

namespace YAMP
{
    [Kind(PopularKinds.Statistic)]
	[Description("Calculates the geometric mean, which is the n-th root of the product of n elements.")]
    [Link("http://en.wikipedia.org/wiki/Geometric_mean")]
    sealed class MeanFunction : ArgumentFunction
	{
		[Description("The geometric mean is similar to the arithmetic mean in that it is a type of average for the data, except it has a rather different way of calculating it.")]
		[Example("mean([1, 4, 8])", "Computes the geometric mean of 1, 4 and 8, i.e. (1 * 3 * 9)^(1 / 3). The result is 3.")]
		[Example("mean([1, 4, 8; 2, 5, 7])", "Computes the geometric mean of 1, 4 and 8 as well as 2, 5 and 7. The result is a 1x2 matrix.")]
		public Value Function(MatrixValue M)
		{
			return YMath.Mean(M);
		}
	}
}
