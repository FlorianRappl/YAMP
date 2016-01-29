using System;

namespace YAMP
{
	[Description("This is the exponential function, i.e. sum of n = 0 to infinity of x^n / n!.")]
	[Kind(PopularKinds.Function)]
    sealed class ExpFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Exp();
		}
	}
}

