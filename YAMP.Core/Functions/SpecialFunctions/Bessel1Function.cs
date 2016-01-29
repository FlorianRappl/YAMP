using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("In mathematics, Bessel functions, first defined by the mathematician Daniel Bernoulli and generalized by Friedrich Bessel, are canonical solutions y(x) of Bessel's differential equation. This function represents Bessel functions of the first kind with order 1.")]
	[Kind(PopularKinds.Function)]
    sealed class Bessel1Function : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return new ScalarValue(Bessel.j1(value.Re));
		}
	}
}
