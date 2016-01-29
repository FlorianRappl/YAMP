using System;

namespace YAMP
{
	[Description("Represents the round function to round up or down to the nearest integer.")]
	[Kind(PopularKinds.Function)]
    sealed class RoundFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            var re = Math.Round(value.Re);
            var im = Math.Round(value.Im);
            return new ScalarValue(re, im);
        }
    }
}
