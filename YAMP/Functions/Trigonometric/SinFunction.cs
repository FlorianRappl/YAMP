using System;

namespace YAMP
{
	[Description("The standard sin(x) function.")]
	[Kind(PopularKinds.Function)]
	class SinFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin();
        }
	}
}

