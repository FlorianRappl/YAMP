using System;

namespace YAMP
{
	[Description("Represents the round function to round towards zero.")]
	[Kind(PopularKinds.Function)]
    sealed class FixFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Floor(value.Re);
			var im = Math.Floor(value.Im);
			return new ScalarValue(re, im);
		}
	}
}
