using System;

namespace YAMP
{
	[Description("Returns a boolean matrix to state if the given values have imaginary parts.")]
	[Kind(PopularKinds.Logic)]
    [Link("http://en.wikipedia.org/wiki/Complex_number")]
    sealed class IsComplexFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsComplex);
        }
    }
}
