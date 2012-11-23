using System;

namespace YAMP
{
	[Description("The median is defined as the data point that falls exactly at the midpoint of a set of data points. If there is an even number of points, then the average is taken of the middle two points.")]
	[Kind(PopularKinds.Function)]
	class MedianFunction : ArgumentFunction
	{
		[Description("Computes the median of the given values.")]
		[Example("median([1, 5, 2, 8, 7])", "Evaluates the values 1, 5, 2, 8, 7 and computes the median, which is 5.")]
		[Example("median([1, 6, 2, 8, 7, 2])", "Evaluates the values 1, 6, 2, 8, 7, 2 and computes the median, which is 4.")]
		public ScalarValue Function(MatrixValue m)
		{
			if (m.Length == 0)
				return new ScalarValue();
			else if (m.Length == 1)
				return m[1];

			m = m.VectorSort();
			int midPoint;
			var sum = new ScalarValue();

			if (m.Length % 2 == 1)
			{
				midPoint = m.Length / 2;
				sum = m[midPoint + 1];
			}
			else
			{
				midPoint = (m.Length / 2);
				sum = m[midPoint] + m[midPoint + 1];
				sum /= 2.0;
			}

			return sum;
		}
	}
}
