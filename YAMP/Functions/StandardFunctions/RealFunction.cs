using System;

namespace YAMP
{
	[Description("Keeps only the real part of the passed matrix or scalar value and omits the imaginary part. If z = x + i * y, then real(z) is just x.")]
	[Kind(PopularKinds.Function)]
    sealed class RealFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return new ScalarValue(value.Re);
		}
	}
}
