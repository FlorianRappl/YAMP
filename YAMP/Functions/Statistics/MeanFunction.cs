using System;

namespace YAMP
{
	[Kind(PopularKinds.Function)]
	[Description("Calculates the geometric mean, which is the n-th root of the product of n elements.")]
	class MeanFunction : ArgumentFunction
	{
		[Description("The geometric mean is similar to the arithmetic mean in that it is a type of average for the data, except it has a rather different way of calculating it.")]
		[Example("mean([1, 4, 8])", "Computes the geometric mean of 1, 4 and 8, i.e. (1 * 3 * 9)^(1 / 3). The result is 3.")]
		public ScalarValue Function(MatrixValue m)
		{
			if (m.Length == 0)
				return new ScalarValue();

			var s = new ScalarValue(1.0);

			for (var i = 1; i <= m.Length; i++)
				s *= m[i];

			return (ScalarValue)s.Power(new ScalarValue(1.0 / m.Length));
		}
	}
}
