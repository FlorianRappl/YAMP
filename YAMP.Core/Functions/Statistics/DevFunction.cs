using System;

namespace YAMP
{
	[Description("In statistics and probability theory, standard deviation (represented by the symbol sigma, σ) shows how much variation or dispersion exists from the average (mean, or expected value).")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Standard_deviation")]
    sealed class DevFunction : ArgumentFunction
	{
		[Description("Standard deviation is a very common calculation in statistical analysis, and is formally defined as the square root of the variance of the data set.")]
		[Example("dev([2, 4, 4, 4, 5, 5, 7, 9])", "Computes the standard deviation of those eight values. The result is 2.")]
		public ScalarValue Function(MatrixValue M)
		{
			var deviation = ScalarValue.Zero;
			var mean = M.Sum() / M.Length;

			for (int i = 1; i <= M.Length; i++)
				deviation += (M[i] - mean).Square();

            return new ScalarValue(Math.Sqrt(deviation.Abs() / M.Length));
		}
	}
}