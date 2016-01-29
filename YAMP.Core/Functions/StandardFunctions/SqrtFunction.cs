using System;

namespace YAMP
{
	[Description("The square root function raises the element to the power 1/2.")]
	[Kind(PopularKinds.Function)]
    sealed class SqrtFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Sqrt();
		}
	}
}

