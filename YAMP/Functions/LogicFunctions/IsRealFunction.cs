using System;

namespace YAMP
{
	[Description("Returns a boolean matrix to state if the given values are real.")]
	[Kind(PopularKinds.Logic)]
    [Link("http://en.wikipedia.org/wiki/Real_number")]
    sealed class IsRealFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsReal);
        }
    }
}
