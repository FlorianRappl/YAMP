using System;

namespace YAMP
{
	[Description("In probability theory and statistics, the variance is a measure of how far a set of numbers is spread out.")]
	[Kind(PopularKinds.Function)]
	class VarFunction : ArgumentFunction
	{
		[Description("The variance is obtained by calculating the data set consisting of each data point in the original data set subtracting the arithmetic mean for the data set, and then squaring it.")]
		[Example("var([1, 2, 3, 4, 5, 6])", "Gives the variance for throwing a perfect sided die, which is roughly 2.9.")]
		public ScalarValue Function(MatrixValue M)
		{
			var variance = new ScalarValue();
			var mean = M.Sum() / M.Length;

			for (int i = 1; i <= M.Length; i++)
				variance += (M[i] - mean).Square();

			return variance / M.Length;
		}
	}
}