using System;

namespace YAMP
{
	[Description("Returns a boolean matrix to state if the given numbers are prime integers.")]
	[Kind(PopularKinds.Logic)]
    class IsPrimeFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsPrime);
        }
    }
}
