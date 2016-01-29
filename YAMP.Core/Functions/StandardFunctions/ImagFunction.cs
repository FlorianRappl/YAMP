using System;

namespace YAMP
{
	[Description("Keeps only the imaginary part of the passed matrix or scalar value and omits the real part. If z = x + i * y, then imag(z) is just y.")]
	[Kind(PopularKinds.Function)]
    sealed class ImagFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return new ScalarValue(value.Im);
		}
	}
}
