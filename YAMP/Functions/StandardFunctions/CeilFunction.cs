using System;

namespace YAMP
{
	[Description("Represents the ceil function to round up.")]
	[Kind(PopularKinds.Function)]
    sealed class CeilFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Ceiling(value.Re);
			var im = Math.Ceiling(value.Im);
			return new ScalarValue(re, im);
		}	
	}
}

