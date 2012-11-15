using System;

namespace YAMP
{
	[Description("The standard cos(x) function.")]
	[Kind(PopularKinds.Function)]
	class CosFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos();
        }
	}
}

