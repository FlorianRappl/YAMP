using System;

namespace YAMP
{
	[Description("Returns a boolean matrix to state if the given numbers are prime integers.")]
	[Kind(PopularKinds.Logic)]
    [Link("http://en.wikipedia.org/wiki/Prime_number")]
    sealed class IsPrimeFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsPrime);
        }
    }
}
