using System;

namespace YAMP
{
	[Description("Represents the factorial function, which is used for the ! operator and integer expressions.")]
	[Kind(PopularKinds.Function)]
    sealed class FactorialFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Factorial();
		}
	}
}

