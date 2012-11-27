using System;

namespace YAMP
{
	[Description("Represents the round function to round towards zero.")]
	[Kind(PopularKinds.Function)]
	class FixFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Floor(value.Value);
			var im = Math.Floor(value.ImaginaryValue);
			return new ScalarValue(re, im);
		}
	}
}
