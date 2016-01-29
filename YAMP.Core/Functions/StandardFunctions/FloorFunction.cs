using System;

namespace YAMP
{
	[Description("Represents the floor function to round down.")]
	[Kind(PopularKinds.Function)]
    sealed class FloorFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			var re = Math.Floor(value.Re);
			var im = Math.Floor(value.Im);
			return new ScalarValue(re, im);
		}	
	}
}

