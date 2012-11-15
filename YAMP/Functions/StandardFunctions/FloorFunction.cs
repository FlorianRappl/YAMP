using System;

namespace YAMP
{
	[Description("Represents the floor function to round down.")]
	[Kind(PopularKinds.Function)]
	class FloorFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Floor(value.Value);
			var im = Math.Floor(value.ImaginaryValue);
			return new ScalarValue(re, im);
		}	
	}
}

