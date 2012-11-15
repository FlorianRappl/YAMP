using System;

namespace YAMP
{
	[Description("Represents the ceil function to round up.")]
	[Kind(PopularKinds.Function)]
	class CeilFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Ceiling(value.Value);
			var im = Math.Ceiling(value.ImaginaryValue);
			return new ScalarValue(re, im);
		}	
	}
}

