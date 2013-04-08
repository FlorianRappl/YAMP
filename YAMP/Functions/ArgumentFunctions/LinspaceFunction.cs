using System;

namespace YAMP
{
	[Description("Returns a uniformly increased vector.")]
	[Kind(PopularKinds.Function)]
    sealed class LinspaceFunction : ArgumentFunction
	{
		[Description("Creates a vector with count elements ranging from a certain value to a certain value.")]
		[Example("linspace(0, 10, 5)", "Creates the vector [0, 2.5, 5, 7.5, 10], i.e. stepping 2.5 and number of elements 5.")]
		public MatrixValue Function(ScalarValue from, ScalarValue to, ScalarValue count)
        {
            var c = count.GetIntegerOrThrowException("count", Name);

			if(c < 2)
				throw new ArgumentException("linspace");

			var step = (to.Re - from.Re) / (c - 1);
			return new RangeValue(from.Re, to.Re, step);
		}
	}
}

