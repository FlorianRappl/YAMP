using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Represents the gamma function, which is taken as the faculty approximation for non-integers.")]
	[Kind(PopularKinds.Function)]
    class GammaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
			return Gamma.LinearGamma(value);
        }
    }
}
