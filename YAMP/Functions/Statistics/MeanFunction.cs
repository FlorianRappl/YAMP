using System;

namespace YAMP
{
	[Kind(PopularKinds.Function)]
	[Description("Calculates the geometric mean, which is the n-th root of the product of n elements.")]
	class MeanFunction : ArgumentFunction
	{
		[Description("The geometric mean is similar to the arithmetic mean in that it is a type of average for the data, except it has a rather different way of calculating it.")]
		[Example("mean([1, 4, 8])", "Computes the geometric mean of 1, 4 and 8, i.e. (1 * 3 * 9)^(1 / 3). The result is 3.")]
		[Example("mean([1, 4, 8; 2, 5, 7])", "Computes the geometric mean of 1, 4 and 8 as well as 2, 5 and 7. The result is a 1x2 matrix.")]
		public Value Function(MatrixValue M)
		{
			return Mean(M);
		}

		public static Value Mean(MatrixValue M)
		{
			if (M.Length == 0)
				return new ScalarValue();

			if (M.IsVector)
			{
				var s = new ScalarValue(1.0);

				for (var i = 1; i <= M.Length; i++)
					s *= M[i];

				return (ScalarValue)s.Power(new ScalarValue(1.0 / M.Length));
			}
			else
			{
				var s = new MatrixValue(1, M.DimensionX);

				for (var i = 1; i < M.DimensionX; i++)
					s[1, i] = new ScalarValue(1.0);

				for (var i = 1; i <= M.DimensionY; i++)
					for (int j = 1; j <= M.DimensionX; j++)
						s[1, j] *= M[i, j];

				for (int j = 1; j <= s.DimensionX; j++)
					s[1, j] = (ScalarValue)s[1, j].Power(new ScalarValue(1.0 / M.DimensionY));

				return s;
			}
		}
	}
}
