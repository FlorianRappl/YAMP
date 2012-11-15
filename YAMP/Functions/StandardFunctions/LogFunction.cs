using System;

namespace YAMP
{
	[Description("This is the decimal logarithm, i.e. the one used to the basis 10.")]
	[Kind(PopularKinds.Function)]
	class LogFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Log();
		}
	}
}

